import http from 'k6/http';
import { check, sleep } from 'k6';
import { Rate } from 'k6/metrics';

// Custom metrics
const errorRate = new Rate('errors');
const executionSuccessRate = new Rate('execution_success');

// Test configuration
export const options = {
  stages: [
    { duration: '1m', target: 20 },   // Ramp up to 20 users
    { duration: '2m', target: 50 },   // Ramp up to 50 users
    { duration: '3m', target: 100 },  // Ramp up to 100 users
    { duration: '5m', target: 100 },  // Stay at 100 users
    { duration: '2m', target: 0 },    // Ramp down to 0 users
  ],
  thresholds: {
    http_req_duration: ['p(95)<5000'], // 95% of requests should be below 5s
    http_req_failed: ['rate<0.05'],    // Error rate should be less than 5%
    errors: ['rate<0.05'],
    execution_success: ['rate>0.90'],  // 90% of executions should succeed
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

// Sample code snippets for execution
const codeSnippets = [
  {
    name: 'Hello World',
    code: `using System;

class Program
{
    static void Main()
    {
        Console.WriteLine("Hello, World!");
    }
}`,
  },
  {
    name: 'Simple Math',
    code: `using System;

class Program
{
    static void Main()
    {
        int a = 10;
        int b = 20;
        Console.WriteLine($"Sum: {a + b}");
    }
}`,
  },
  {
    name: 'Loop Example',
    code: `using System;

class Program
{
    static void Main()
    {
        for (int i = 1; i <= 5; i++)
        {
            Console.WriteLine($"Count: {i}");
        }
    }
}`,
  },
  {
    name: 'Array Processing',
    code: `using System;
using System.Linq;

class Program
{
    static void Main()
    {
        int[] numbers = { 1, 2, 3, 4, 5 };
        int sum = numbers.Sum();
        Console.WriteLine($"Sum: {sum}");
    }
}`,
  },
];

function getRandomUser() {
  return testUsers[Math.floor(Math.random() * testUsers.length)];
}

function getRandomCode() {
  return codeSnippets[Math.floor(Math.random() * codeSnippets.length)];
}

export function setup() {
  console.log('Starting load test: Code Execution');
  console.log(`Base URL: ${BASE_URL}`);
  console.log('Target: 100 concurrent code executions');
  console.log('Performance target: p95 < 5s');
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
  });
  
  errorRate.add(!loginSuccess);
  
  if (!loginSuccess) {
    console.error(`Login failed for ${user.email}: ${loginRes.status}`);
    sleep(1);
    return;
  }
  
  const token = JSON.parse(loginRes.body).token;
  const authParams = {
    headers: {
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json',
    },
    timeout: '30s', // Allow up to 30s for execution
  };
  
  // Execute code
  const codeSnippet = getRandomCode();
  const executePayload = JSON.stringify({
    code: codeSnippet.code,
    files: [
      {
        name: 'Program.cs',
        content: codeSnippet.code,
      },
    ],
    entryPoint: 'Program.cs',
  });
  
  const executeRes = http.post(`${BASE_URL}/api/code/execute`, executePayload, authParams);
  
  const executeSuccess = check(executeRes, {
    'execute status is 200 or 202': (r) => r.status === 200 || r.status === 202,
    'execute has jobId': (r) => {
      try {
        const body = JSON.parse(r.body);
        return body.jobId !== undefined;
      } catch (e) {
        return false;
      }
    },
  });
  
  errorRate.add(!executeSuccess);
  
  if (!executeSuccess) {
    console.error(`Code execution failed: ${executeRes.status} - ${executeRes.body}`);
    sleep(1);
    return;
  }
  
  const jobId = JSON.parse(executeRes.body).jobId;
  
  // Poll for execution result
  let attempts = 0;
  let maxAttempts = 10;
  let executionComplete = false;
  let executionSucceeded = false;
  
  while (attempts < maxAttempts && !executionComplete) {
    sleep(1); // Wait 1 second between polls
    
    const statusRes = http.get(`${BASE_URL}/api/code/status/${jobId}`, authParams);
    
    const statusSuccess = check(statusRes, {
      'status check is 200': (r) => r.status === 200,
    });
    
    if (statusSuccess) {
      try {
        const statusBody = JSON.parse(statusRes.body);
        const status = statusBody.status;
        
        if (status === 'Completed') {
          executionComplete = true;
          executionSucceeded = true;
          
          check(statusRes, {
            'execution completed successfully': () => true,
            'execution time < 5s': (r) => {
              const body = JSON.parse(r.body);
              return body.executionTimeMs < 5000;
            },
          });
        } else if (status === 'Failed' || status === 'Timeout' || status === 'MemoryExceeded') {
          executionComplete = true;
          executionSucceeded = false;
          console.warn(`Execution failed with status: ${status}`);
        }
      } catch (e) {
        console.error('Failed to parse status response');
      }
    }
    
    attempts++;
  }
  
  if (!executionComplete) {
    console.warn(`Execution did not complete after ${maxAttempts} attempts`);
  }
  
  executionSuccessRate.add(executionSucceeded);
  errorRate.add(!executionSucceeded);
  
  sleep(2);
}

export function teardown(data) {
  console.log('Load test completed: Code Execution');
}
