import http from 'k6/http';
import { check, sleep } from 'k6';
import { Rate } from 'k6/metrics';

// Custom metrics
const errorRate = new Rate('errors');
const feedbackSuccessRate = new Rate('feedback_success');

// Test configuration
export const options = {
  stages: [
    { duration: '1m', target: 10 },   // Ramp up to 10 users
    { duration: '2m', target: 25 },   // Ramp up to 25 users
    { duration: '3m', target: 50 },   // Ramp up to 50 users
    { duration: '5m', target: 50 },   // Stay at 50 users
    { duration: '2m', target: 0 },    // Ramp down to 0 users
  ],
  thresholds: {
    http_req_duration: ['p(95)<10000'], // 95% of requests should be below 10s
    http_req_failed: ['rate<0.05'],     // Error rate should be less than 5%
    errors: ['rate<0.05'],
    feedback_success: ['rate>0.85'],    // 85% of feedback requests should succeed
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

// Sample code snippets for AI review
const codeSnippets = [
  {
    name: 'Basic Controller',
    code: `using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    [HttpGet]
    public IActionResult GetUsers()
    {
        var users = new[] { "Alice", "Bob", "Charlie" };
        return Ok(users);
    }
}`,
    context: 'ASP.NET Core Web API controller',
  },
  {
    name: 'Service with DI',
    code: `public class UserService
{
    private readonly IUserRepository _repository;
    
    public UserService(IUserRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<User> GetUserAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }
}`,
    context: 'Service layer with dependency injection',
  },
  {
    name: 'Entity Model',
    code: `public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public DateTime CreatedAt { get; set; }
}`,
    context: 'Entity Framework Core entity model',
  },
  {
    name: 'Repository Pattern',
    code: `public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;
    
    public UserRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<User> GetByIdAsync(int id)
    {
        return await _context.Users.FindAsync(id);
    }
    
    public async Task<List<User>> GetAllAsync()
    {
        return await _context.Users.ToListAsync();
    }
}`,
    context: 'Repository pattern implementation',
  },
  {
    name: 'Authentication Logic',
    code: `public class AuthService
{
    public string HashPassword(string password)
    {
        return BCrypt.HashPassword(password);
    }
    
    public bool VerifyPassword(string password, string hash)
    {
        return BCrypt.Verify(password, hash);
    }
}`,
    context: 'Password hashing service',
  },
];

function getRandomUser() {
  return testUsers[Math.floor(Math.random() * testUsers.length)];
}

function getRandomCode() {
  return codeSnippets[Math.floor(Math.random() * codeSnippets.length)];
}

export function setup() {
  console.log('Starting load test: AI Feedback');
  console.log(`Base URL: ${BASE_URL}`);
  console.log('Target: 50 concurrent AI feedback requests');
  console.log('Performance target: p95 < 10s');
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
    timeout: '15s', // Allow up to 15s for AI feedback
  };
  
  // Request AI code review
  const codeSnippet = getRandomCode();
  const reviewPayload = JSON.stringify({
    code: codeSnippet.code,
    context: codeSnippet.context,
  });
  
  const reviewRes = http.post(`${BASE_URL}/api/code/review`, reviewPayload, authParams);
  
  const reviewSuccess = check(reviewRes, {
    'review status is 200': (r) => r.status === 200,
    'review has suggestions': (r) => {
      try {
        const body = JSON.parse(r.body);
        return body.suggestions !== undefined || body.feedback !== undefined;
      } catch (e) {
        return false;
      }
    },
    'review response time < 10s': (r) => r.timings.duration < 10000,
  });
  
  errorRate.add(!reviewSuccess);
  feedbackSuccessRate.add(reviewSuccess);
  
  if (!reviewSuccess) {
    console.error(`AI review failed: ${reviewRes.status} - ${reviewRes.body}`);
  } else {
    try {
      const reviewBody = JSON.parse(reviewRes.body);
      const suggestions = reviewBody.suggestions || reviewBody.feedback || [];
      
      check(reviewRes, {
        'review has feedback items': () => Array.isArray(suggestions) && suggestions.length > 0,
        'feedback has structure': () => {
          if (!Array.isArray(suggestions) || suggestions.length === 0) return true;
          const firstItem = suggestions[0];
          return firstItem.message !== undefined || firstItem.type !== undefined;
        },
      });
    } catch (e) {
      console.error('Failed to parse review response');
    }
  }
  
  sleep(3);
}

export function teardown(data) {
  console.log('Load test completed: AI Feedback');
}
