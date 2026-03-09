/**
 * Load Testing Scenarios using k6
 * Tests platform performance under various load conditions
 * Validates: Task 22.4 requirements
 * 
 * Installation: https://k6.io/docs/getting-started/installation/
 * Run: k6 run load-test-scenarios.js
 */

import http from 'k6/http';
import { check, sleep, group } from 'k6';
import { Rate, Trend, Counter } from 'k6/metrics';

// Custom metrics
const errorRate = new Rate('errors');
const apiResponseTime = new Trend('api_response_time');
const successfulRequests = new Counter('successful_requests');
const failedRequests = new Counter('failed_requests');

// Configuration
const BASE_URL = __ENV.API_GATEWAY_URL || 'http://localhost:5000';

// Test scenarios configuration
export const options = {
    scenarios: {
        // Scenario 1: Smoke test (1 user)
        smoke_test: {
            executor: 'constant-vus',
            vus: 1,
            duration: '1m',
            tags: { test_type: 'smoke' },
            exec: 'smokeTest',
        },
        
        // Scenario 2: Load test (100 concurrent users)
        load_test_100: {
            executor: 'ramping-vus',
            startVUs: 0,
            stages: [
                { duration: '2m', target: 50 },   // Ramp up to 50 users
                { duration: '5m', target: 100 },  // Ramp up to 100 users
                { duration: '5m', target: 100 },  // Stay at 100 users
                { duration: '2m', target: 0 },    // Ramp down to 0 users
            ],
            tags: { test_type: 'load_100' },
            exec: 'loadTest',
            startTime: '2m', // Start after smoke test
        },
        
        // Scenario 3: Stress test (1,000 concurrent users)
        stress_test_1000: {
            executor: 'ramping-vus',
            startVUs: 0,
            stages: [
                { duration: '3m', target: 250 },   // Ramp up to 250
                { duration: '3m', target: 500 },   // Ramp up to 500
                { duration: '3m', target: 750 },   // Ramp up to 750
                { duration: '3m', target: 1000 },  // Ramp up to 1,000
                { duration: '10m', target: 1000 }, // Stay at 1,000
                { duration: '3m', target: 0 },     // Ramp down
            ],
            tags: { test_type: 'stress_1000' },
            exec: 'stressTest',
            startTime: '16m', // Start after load test
        },
        
        // Scenario 4: Spike test (sudden traffic spike)
        spike_test: {
            executor: 'ramping-vus',
            startVUs: 0,
            stages: [
                { duration: '30s', target: 100 },   // Normal load
                { duration: '30s', target: 2000 },  // Sudden spike
                { duration: '1m', target: 2000 },   // Maintain spike
                { duration: '30s', target: 100 },   // Return to normal
                { duration: '30s', target: 0 },     // Ramp down
            ],
            tags: { test_type: 'spike' },
            exec: 'spikeTest',
            startTime: '42m', // Start after stress test
        },
        
        // Scenario 5: Endurance test (10,000 concurrent users)
        endurance_test_10000: {
            executor: 'ramping-vus',
            startVUs: 0,
            stages: [
                { duration: '5m', target: 2500 },   // Ramp up
                { duration: '5m', target: 5000 },   // Ramp up
                { duration: '5m', target: 7500 },   // Ramp up
                { duration: '5m', target: 10000 },  // Ramp up to 10,000
                { duration: '30m', target: 10000 }, // Stay at 10,000 for 30 minutes
                { duration: '5m', target: 0 },      // Ramp down
            ],
            tags: { test_type: 'endurance_10000' },
            exec: 'enduranceTest',
            startTime: '48m', // Start after spike test
        },
    },
    
    // Thresholds (SLA requirements)
    thresholds: {
        'http_req_duration': ['p(95)<200', 'p(99)<500'], // 95% < 200ms, 99% < 500ms
        'http_req_failed': ['rate<0.01'],                 // Error rate < 1%
        'errors': ['rate<0.05'],                          // Custom error rate < 5%
    },
};

// Test data
const testUsers = [];
for (let i = 0; i < 100; i++) {
    testUsers.push({
        email: `loadtest_${i}_${Date.now()}@example.com`,
        password: 'LoadTest123!',
        name: `Load Test User ${i}`,
    });
}

// Smoke Test - Basic functionality check
export function smokeTest() {
    group('Smoke Test - Basic Health Checks', () => {
        // Health check
        const healthRes = http.get(`${BASE_URL}/health`);
        check(healthRes, {
            'health check status is 200': (r) => r.status === 200,
        });
        
        // API Gateway check
        const gatewayRes = http.get(`${BASE_URL}/api/v1/health`);
        check(gatewayRes, {
            'gateway health status is 200': (r) => r.status === 200,
        });
        
        sleep(1);
    });
}

// Load Test - Normal load simulation
export function loadTest() {
    const user = testUsers[Math.floor(Math.random() * testUsers.length)];
    
    group('User Authentication', () => {
        const loginPayload = JSON.stringify({
            email: user.email,
            password: user.password,
        });
        
        const params = {
            headers: { 'Content-Type': 'application/json' },
        };
        
        const loginRes = http.post(`${BASE_URL}/api/v1/auth/login`, loginPayload, params);
        
        const success = check(loginRes, {
            'login status is 200 or 401': (r) => r.status === 200 || r.status === 401,
            'login response time < 200ms': (r) => r.timings.duration < 200,
        });
        
        errorRate.add(!success);
        apiResponseTime.add(loginRes.timings.duration);
        
        if (success) {
            successfulRequests.add(1);
        } else {
            failedRequests.add(1);
        }
    });
    
    group('Browse Curriculum', () => {
        const curriculumRes = http.get(`${BASE_URL}/api/v1/curriculum`);
        
        check(curriculumRes, {
            'curriculum status is 200': (r) => r.status === 200,
            'curriculum response time < 200ms': (r) => r.timings.duration < 200,
        });
        
        apiResponseTime.add(curriculumRes.timings.duration);
    });
    
    group('Browse Challenges', () => {
        const challengesRes = http.get(`${BASE_URL}/api/v1/challenges`);
        
        check(challengesRes, {
            'challenges status is 200': (r) => r.status === 200,
            'challenges response time < 200ms': (r) => r.timings.duration < 200,
        });
        
        apiResponseTime.add(challengesRes.timings.duration);
    });
    
    group('View Leaderboard', () => {
        const leaderboardRes = http.get(`${BASE_URL}/api/v1/leaderboard`);
        
        check(leaderboardRes, {
            'leaderboard status is 200': (r) => r.status === 200,
            'leaderboard response time < 200ms': (r) => r.timings.duration < 200,
        });
        
        apiResponseTime.add(leaderboardRes.timings.duration);
    });
    
    sleep(Math.random() * 3 + 1); // Random sleep 1-4 seconds
}

// Stress Test - High load simulation
export function stressTest() {
    const user = testUsers[Math.floor(Math.random() * testUsers.length)];
    
    group('Code Execution Under Stress', () => {
        const codePayload = JSON.stringify({
            code: 'Console.WriteLine("Stress test");',
            language: 'csharp',
        });
        
        const params = {
            headers: { 'Content-Type': 'application/json' },
        };
        
        const execRes = http.post(`${BASE_URL}/api/v1/execute/run`, codePayload, params);
        
        const success = check(execRes, {
            'execution status is 200 or 503': (r) => r.status === 200 || r.status === 503,
            'execution response time < 500ms': (r) => r.timings.duration < 500,
        });
        
        errorRate.add(!success);
        apiResponseTime.add(execRes.timings.duration);
    });
    
    group('SQL Execution Under Stress', () => {
        const sqlPayload = JSON.stringify({
            query: 'SELECT 1 AS TestValue',
            sessionId: `stress_${Date.now()}_${Math.random()}`,
        });
        
        const params = {
            headers: { 'Content-Type': 'application/json' },
        };
        
        const sqlRes = http.post(`${BASE_URL}/api/v1/sql/execute`, sqlPayload, params);
        
        check(sqlRes, {
            'sql execution status is 200 or 503': (r) => r.status === 200 || r.status === 503,
        });
        
        apiResponseTime.add(sqlRes.timings.duration);
    });
    
    sleep(Math.random() * 2); // Random sleep 0-2 seconds
}

// Spike Test - Sudden traffic spike
export function spikeTest() {
    group('API Gateway Under Spike', () => {
        const endpoints = [
            '/api/v1/curriculum',
            '/api/v1/challenges',
            '/api/v1/leaderboard',
            '/health',
        ];
        
        const endpoint = endpoints[Math.floor(Math.random() * endpoints.length)];
        const res = http.get(`${BASE_URL}${endpoint}`);
        
        check(res, {
            'spike response status is 200 or 429 or 503': (r) => 
                r.status === 200 || r.status === 429 || r.status === 503,
        });
        
        apiResponseTime.add(res.timings.duration);
    });
    
    sleep(0.1); // Minimal sleep during spike
}

// Endurance Test - Sustained high load
export function enduranceTest() {
    const user = testUsers[Math.floor(Math.random() * testUsers.length)];
    
    group('Sustained Load - Mixed Operations', () => {
        // Random operation selection
        const operations = [
            () => http.get(`${BASE_URL}/api/v1/curriculum`),
            () => http.get(`${BASE_URL}/api/v1/challenges`),
            () => http.get(`${BASE_URL}/api/v1/leaderboard`),
            () => {
                const payload = JSON.stringify({
                    code: 'Console.WriteLine("Endurance test");',
                    language: 'csharp',
                });
                return http.post(`${BASE_URL}/api/v1/execute/run`, payload, {
                    headers: { 'Content-Type': 'application/json' },
                });
            },
        ];
        
        const operation = operations[Math.floor(Math.random() * operations.length)];
        const res = operation();
        
        const success = check(res, {
            'endurance response status is acceptable': (r) => 
                r.status === 200 || r.status === 429 || r.status === 503,
            'endurance response time < 1000ms': (r) => r.timings.duration < 1000,
        });
        
        errorRate.add(!success);
        apiResponseTime.add(res.timings.duration);
        
        if (success) {
            successfulRequests.add(1);
        } else {
            failedRequests.add(1);
        }
    });
    
    sleep(Math.random() * 2 + 0.5); // Random sleep 0.5-2.5 seconds
}

// Teardown function
export function teardown(data) {
    console.log('Load test completed');
    console.log(`Total successful requests: ${successfulRequests.value}`);
    console.log(`Total failed requests: ${failedRequests.value}`);
}
