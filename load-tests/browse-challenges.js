import http from 'k6/http';
import { check, sleep } from 'k6';
import { Rate } from 'k6/metrics';

// Custom metrics
const errorRate = new Rate('errors');

// Test configuration
export const options = {
  stages: [
    { duration: '2m', target: 100 },   // Ramp up to 100 users
    { duration: '3m', target: 500 },   // Ramp up to 500 users
    { duration: '5m', target: 1000 },  // Ramp up to 1000 users
    { duration: '5m', target: 1000 },  // Stay at 1000 users
    { duration: '2m', target: 0 },     // Ramp down to 0 users
  ],
  thresholds: {
    http_req_duration: ['p(95)<200'], // 95% of requests should be below 200ms
    http_req_failed: ['rate<0.01'],   // Error rate should be less than 1%
    errors: ['rate<0.01'],
  },
};

const BASE_URL = __ENV.BASE_URL || 'http://localhost:5000';

// Test data
const testUsers = [
  { email: 'test1@example.com', password: 'Test123!@#' },
  { email: 'test2@example.com', password: 'Test123!@#' },
  { email: 'test3@example.com', password: 'Test123!@#' },
  { email: 'test4@example.com', password: 'Test123!@#' },
  { email: 'test5@example.com', password: 'Test123!@#' },
];

function getRandomUser() {
  return testUsers[Math.floor(Math.random() * testUsers.length)];
}

export function setup() {
  console.log('Starting load test: Browse Challenges');
  console.log(`Base URL: ${BASE_URL}`);
  console.log('Target: 1000 concurrent users');
  console.log('Performance target: p95 < 200ms');
}

export default function () {
  // Select a random test user
  const user = getRandomUser();
  
  // Login to get authentication token
  const loginPayload = JSON.stringify({
    email: user.email,
    password: user.password,
  });
  
  const loginParams = {
    headers: {
      'Content-Type': 'application/json',
    },
  };
  
  const loginRes = http.post(`${BASE_URL}/api/auth/login`, loginPayload, loginParams);
  
  const loginSuccess = check(loginRes, {
    'login status is 200': (r) => r.status === 200,
    'login has token': (r) => {
      try {
        const body = JSON.parse(r.body);
        return body.token !== undefined;
      } catch (e) {
        return false;
      }
    },
  });
  
  errorRate.add(!loginSuccess);
  
  if (!loginSuccess) {
    console.error(`Login failed for ${user.email}: ${loginRes.status} - ${loginRes.body}`);
    sleep(1);
    return;
  }
  
  const token = JSON.parse(loginRes.body).token;
  const authParams = {
    headers: {
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json',
    },
  };
  
  // Browse challenges list
  const challengesRes = http.get(`${BASE_URL}/api/challenges`, authParams);
  
  const challengesSuccess = check(challengesRes, {
    'challenges list status is 200': (r) => r.status === 200,
    'challenges list has data': (r) => {
      try {
        const body = JSON.parse(r.body);
        return Array.isArray(body) || (body.challenges && Array.isArray(body.challenges));
      } catch (e) {
        return false;
      }
    },
    'challenges response time < 200ms': (r) => r.timings.duration < 200,
  });
  
  errorRate.add(!challengesSuccess);
  
  sleep(1);
  
  // Get challenge details for a random challenge
  let challenges;
  try {
    const body = JSON.parse(challengesRes.body);
    challenges = Array.isArray(body) ? body : body.challenges;
  } catch (e) {
    console.error('Failed to parse challenges response');
    sleep(1);
    return;
  }
  
  if (challenges && challenges.length > 0) {
    const randomChallenge = challenges[Math.floor(Math.random() * challenges.length)];
    const challengeId = randomChallenge.id;
    
    const challengeDetailRes = http.get(`${BASE_URL}/api/challenges/${challengeId}`, authParams);
    
    const detailSuccess = check(challengeDetailRes, {
      'challenge detail status is 200': (r) => r.status === 200,
      'challenge detail has data': (r) => {
        try {
          const body = JSON.parse(r.body);
          return body.id !== undefined && body.title !== undefined;
        } catch (e) {
          return false;
        }
      },
      'challenge detail response time < 200ms': (r) => r.timings.duration < 200,
    });
    
    errorRate.add(!detailSuccess);
  }
  
  sleep(2);
  
  // Get user dashboard
  const dashboardRes = http.get(`${BASE_URL}/api/progress/dashboard`, authParams);
  
  const dashboardSuccess = check(dashboardRes, {
    'dashboard status is 200': (r) => r.status === 200,
    'dashboard response time < 200ms': (r) => r.timings.duration < 200,
  });
  
  errorRate.add(!dashboardSuccess);
  
  sleep(1);
}

export function teardown(data) {
  console.log('Load test completed: Browse Challenges');
}
