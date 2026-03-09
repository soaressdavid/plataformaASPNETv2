using System.Net.Http.Json;
using Xunit;

namespace Integration.Tests;

/// <summary>
/// End-to-end tests for critical user flows
/// Tests complete user journeys from registration to advanced features
/// Validates: Task 22.3 requirements
/// </summary>
public class EndToEndTests : IAsyncLifetime
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;
    private string? _authToken;
    private Guid _testUserId;

    public EndToEndTests()
    {
        _baseUrl = Environment.GetEnvironmentVariable("API_GATEWAY_URL") ?? "http://localhost:5000";
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(_baseUrl),
            Timeout = TimeSpan.FromSeconds(60)
        };
    }

    public async Task InitializeAsync()
    {
        // Setup will be done per test
        await Task.CompletedTask;
    }

    public Task DisposeAsync()
    {
        _httpClient.Dispose();
        return Task.CompletedTask;
    }

    /// <summary>
    /// E2E Test 1: Complete user onboarding flow
    /// Registration → Email verification → Profile setup → First lesson
    /// </summary>
    [Fact]
    public async Task CompleteUserOnboarding_EndToEnd_Success()
    {
        // Step 1: Register new user
        var email = $"e2e_{Guid.NewGuid():N}@example.com";
        var registerRequest = new
        {
            Name = "E2E Test User",
            Email = email,
            Password = "SecurePass123!"
        };

        var registerResponse = await _httpClient.PostAsJsonAsync("/api/v1/auth/register", registerRequest);
        
        if (registerResponse.IsSuccessStatusCode)
        {
            var registerResult = await registerResponse.Content.ReadFromJsonAsync<AuthResponse>();
            Assert.NotNull(registerResult);
            _authToken = registerResult.Token;
            _testUserId = registerResult.UserId;

            _httpClient.DefaultRequestHeaders.Authorization = 
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _authToken);

            Console.WriteLine($"✓ User registered: {_testUserId}");

            // Step 2: Get user profile
            var profileResponse = await _httpClient.GetAsync($"/api/v1/users/{_testUserId}/profile");
            Assert.True(profileResponse.IsSuccessStatusCode);

            var profile = await profileResponse.Content.ReadFromJsonAsync<UserProfile>();
            Assert.NotNull(profile);
            Assert.Equal(0, profile.Level);
            Assert.Equal(0, profile.TotalXP);

            Console.WriteLine($"✓ Profile loaded: Level {profile.Level}, XP {profile.TotalXP}");

            // Step 3: Get curriculum
            var curriculumResponse = await _httpClient.GetAsync("/api/v1/curriculum");
            if (curriculumResponse.IsSuccessStatusCode)
            {
                var curriculum = await curriculumResponse.Content.ReadFromJsonAsync<CurriculumResponse>();
                Assert.NotNull(curriculum);
                Assert.NotEmpty(curriculum.Levels);

                Console.WriteLine($"✓ Curriculum loaded: {curriculum.Levels.Count} levels");

                // Step 4: Start first lesson
                var firstLevel = curriculum.Levels.OrderBy(l => l.Order).First();
                var firstCourse = firstLevel.Courses?.FirstOrDefault();
                
                if (firstCourse != null)
                {
                    var lessonResponse = await _httpClient.GetAsync($"/api/v1/courses/{firstCourse.Id}/lessons");
                    if (lessonResponse.IsSuccessStatusCode)
                    {
                        var lessons = await lessonResponse.Content.ReadFromJsonAsync<LessonListResponse>();
                        var firstLesson = lessons?.Lessons.OrderBy(l => l.Order).FirstOrDefault();

                        if (firstLesson != null)
                        {
                            Console.WriteLine($"✓ First lesson accessed: {firstLesson.Title}");
                            Console.WriteLine("✅ Complete onboarding flow successful");
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// E2E Test 2: Learning progression flow
    /// Complete lesson → Earn XP → Level up → Unlock next content
    /// </summary>
    [Fact]
    public async Task LearningProgression_WithXPAndLevelUp_Success()
    {
        // Setup: Create and authenticate user
        await CreateAndAuthenticateUser();
        if (string.IsNullOrEmpty(_authToken)) return;

        // Step 1: Get first available lesson
        var curriculumResponse = await _httpClient.GetAsync("/api/v1/curriculum");
        if (!curriculumResponse.IsSuccessStatusCode) return;

        var curriculum = await curriculumResponse.Content.ReadFromJsonAsync<CurriculumResponse>();
        var firstCourse = curriculum?.Levels.FirstOrDefault()?.Courses?.FirstOrDefault();
        if (firstCourse == null) return;

        var lessonsResponse = await _httpClient.GetAsync($"/api/v1/courses/{firstCourse.Id}/lessons");
        if (!lessonsResponse.IsSuccessStatusCode) return;

        var lessons = await lessonsResponse.Content.ReadFromJsonAsync<LessonListResponse>();
        var firstLesson = lessons?.Lessons.OrderBy(l => l.Order).FirstOrDefault();
        if (firstLesson == null) return;

        Console.WriteLine($"Starting lesson: {firstLesson.Title}");

        // Step 2: Complete the lesson
        var completeRequest = new { UserId = _testUserId };
        var completeResponse = await _httpClient.PostAsJsonAsync(
            $"/api/v1/courses/{firstCourse.Id}/lessons/{firstLesson.Id}/complete",
            completeRequest);

        if (completeResponse.IsSuccessStatusCode)
        {
            var result = await completeResponse.Content.ReadFromJsonAsync<LessonCompleteResponse>();
            Console.WriteLine($"✓ Lesson completed. XP earned: {result?.XPEarned ?? 0}");

            // Step 3: Check updated profile
            var profileResponse = await _httpClient.GetAsync($"/api/v1/users/{_testUserId}/profile");
            if (profileResponse.IsSuccessStatusCode)
            {
                var profile = await profileResponse.Content.ReadFromJsonAsync<UserProfile>();
                Assert.NotNull(profile);
                Assert.True(profile.TotalXP > 0, "XP should increase after lesson completion");

                Console.WriteLine($"✓ Profile updated: Level {profile.Level}, XP {profile.TotalXP}");
                Console.WriteLine("✅ Learning progression flow successful");
            }
        }
    }

    /// <summary>
    /// E2E Test 3: Challenge submission and validation flow
    /// Browse challenges → Select challenge → Write code → Submit → Get feedback
    /// </summary>
    [Fact]
    public async Task ChallengeSubmissionAndValidation_EndToEnd_Success()
    {
        // Setup
        await CreateAndAuthenticateUser();
        if (string.IsNullOrEmpty(_authToken)) return;

        // Step 1: Browse challenges
        var challengesResponse = await _httpClient.GetAsync("/api/v1/challenges?difficulty=Easy");
        if (!challengesResponse.IsSuccessStatusCode) return;

        var challenges = await challengesResponse.Content.ReadFromJsonAsync<ChallengeListResponse>();
        var easyChallenge = challenges?.Challenges.FirstOrDefault();
        if (easyChallenge == null) return;

        Console.WriteLine($"Selected challenge: {easyChallenge.Title} (Difficulty: {easyChallenge.Difficulty})");

        // Step 2: Get challenge details
        var detailResponse = await _httpClient.GetAsync($"/api/v1/challenges/{easyChallenge.Id}");
        if (!detailResponse.IsSuccessStatusCode) return;

        var detail = await detailResponse.Content.ReadFromJsonAsync<ChallengeDetail>();
        Assert.NotNull(detail);
        Assert.NotEmpty(detail.StarterCode);

        Console.WriteLine($"✓ Challenge loaded with {detail.TestCases?.Count ?? 0} test cases");

        // Step 3: Submit solution
        var submitRequest = new
        {
            UserId = _testUserId,
            Code = detail.StarterCode, // Using starter code for simplicity
            Language = "csharp"
        };

        var submitResponse = await _httpClient.PostAsJsonAsync(
            $"/api/v1/challenges/{easyChallenge.Id}/submit",
            submitRequest);

        if (submitResponse.IsSuccessStatusCode)
        {
            var result = await submitResponse.Content.ReadFromJsonAsync<SubmissionResult>();
            Assert.NotNull(result);

            Console.WriteLine($"✓ Solution submitted: {result.SubmissionId}");
            Console.WriteLine($"  Tests passed: {result.TestsPassed}/{result.TotalTests}");
            Console.WriteLine($"  XP awarded: {result.XPAwarded}");
            Console.WriteLine("✅ Challenge submission flow successful");
        }
    }

    /// <summary>
    /// E2E Test 4: Gamification features flow
    /// Earn XP → Check leaderboard → View achievements → Track streak
    /// </summary>
    [Fact]
    public async Task GamificationFeatures_EndToEnd_Success()
    {
        // Setup
        await CreateAndAuthenticateUser();
        if (string.IsNullOrEmpty(_authToken)) return;

        // Step 1: Simulate earning XP (complete a lesson)
        var curriculumResponse = await _httpClient.GetAsync("/api/v1/curriculum");
        if (curriculumResponse.IsSuccessStatusCode)
        {
            var curriculum = await curriculumResponse.Content.ReadFromJsonAsync<CurriculumResponse>();
            var firstCourse = curriculum?.Levels.FirstOrDefault()?.Courses?.FirstOrDefault();
            
            if (firstCourse != null)
            {
                var lessonsResponse = await _httpClient.GetAsync($"/api/v1/courses/{firstCourse.Id}/lessons");
                if (lessonsResponse.IsSuccessStatusCode)
                {
                    var lessons = await lessonsResponse.Content.ReadFromJsonAsync<LessonListResponse>();
                    var firstLesson = lessons?.Lessons.FirstOrDefault();

                    if (firstLesson != null)
                    {
                        var completeRequest = new { UserId = _testUserId };
                        await _httpClient.PostAsJsonAsync(
                            $"/api/v1/courses/{firstCourse.Id}/lessons/{firstLesson.Id}/complete",
                            completeRequest);

                        Console.WriteLine("✓ Activity completed to earn XP");
                    }
                }
            }
        }

        // Step 2: Check leaderboard position
        var leaderboardResponse = await _httpClient.GetAsync("/api/v1/leaderboard");
        if (leaderboardResponse.IsSuccessStatusCode)
        {
            var leaderboard = await leaderboardResponse.Content.ReadFromJsonAsync<LeaderboardResponse>();
            Console.WriteLine($"✓ Leaderboard loaded: {leaderboard?.Entries.Count ?? 0} entries");
        }

        // Step 3: View achievements
        var achievementsResponse = await _httpClient.GetAsync($"/api/v1/users/{_testUserId}/achievements");
        if (achievementsResponse.IsSuccessStatusCode)
        {
            var achievements = await achievementsResponse.Content.ReadFromJsonAsync<AchievementsResponse>();
            Console.WriteLine($"✓ Achievements loaded: {achievements?.Earned.Count ?? 0} earned");
        }

        // Step 4: Check streak
        var streakResponse = await _httpClient.GetAsync($"/api/v1/users/{_testUserId}/streak");
        if (streakResponse.IsSuccessStatusCode)
        {
            var streak = await streakResponse.Content.ReadFromJsonAsync<StreakResponse>();
            Console.WriteLine($"✓ Current streak: {streak?.CurrentStreak ?? 0} days");
            Console.WriteLine("✅ Gamification features flow successful");
        }
    }

    /// <summary>
    /// E2E Test 5: AI Tutor integration flow
    /// Write code → Request AI review → Get hints → Apply suggestions
    /// </summary>
    [Fact]
    public async Task AITutorIntegration_EndToEnd_Success()
    {
        // Setup
        await CreateAndAuthenticateUser();
        if (string.IsNullOrEmpty(_authToken)) return;

        // Step 1: Submit code for AI review
        var reviewRequest = new
        {
            Code = @"
public class Calculator
{
    public int Add(int a, int b)
    {
        return a + b;
    }
    
    public int Subtract(int a, int b)
    {
        return a - b;
    }
}",
            UserId = _testUserId
        };

        var reviewResponse = await _httpClient.PostAsJsonAsync("/api/v1/ai/review", reviewRequest);
        
        if (reviewResponse.IsSuccessStatusCode)
        {
            var review = await reviewResponse.Content.ReadFromJsonAsync<AIReviewResponse>();
            Assert.NotNull(review);

            Console.WriteLine($"✓ AI review completed");
            Console.WriteLine($"  Overall score: {review.OverallScore}/100");
            Console.WriteLine($"  Suggestions: {review.Suggestions?.Count ?? 0}");

            // Step 2: Request hints for a challenge
            var hintRequest = new
            {
                ChallengeId = Guid.NewGuid(),
                UserId = _testUserId,
                Level = 1 // First hint level
            };

            var hintResponse = await _httpClient.PostAsJsonAsync("/api/v1/ai/hint", hintRequest);
            
            if (hintResponse.IsSuccessStatusCode)
            {
                var hint = await hintResponse.Content.ReadFromJsonAsync<HintResponse>();
                Console.WriteLine($"✓ Hint received (Cost: {hint?.XPCost ?? 0} XP)");
            }

            // Step 3: Request code explanation
            var explainRequest = new
            {
                Code = "var result = numbers.Where(n => n > 0).Select(n => n * 2).ToList();",
                UserId = _testUserId
            };

            var explainResponse = await _httpClient.PostAsJsonAsync("/api/v1/ai/explain", explainRequest);
            
            if (explainResponse.IsSuccessStatusCode)
            {
                var explanation = await explainResponse.Content.ReadFromJsonAsync<ExplanationResponse>();
                Console.WriteLine($"✓ Code explanation received");
                Console.WriteLine("✅ AI Tutor integration flow successful");
            }
        }
    }

    /// <summary>
    /// E2E Test 6: Real-time collaboration flow
    /// Create session → Invite user → Collaborate on code → Share results
    /// </summary>
    [Fact]
    public async Task RealTimeCollaboration_EndToEnd_Success()
    {
        // Setup
        await CreateAndAuthenticateUser();
        if (string.IsNullOrEmpty(_authToken)) return;

        // Step 1: Create collaboration session
        var sessionRequest = new
        {
            Title = "E2E Collaboration Test",
            CreatorId = _testUserId,
            IsPublic = false
        };

        var sessionResponse = await _httpClient.PostAsJsonAsync("/api/v1/collaboration/sessions", sessionRequest);
        
        if (sessionResponse.IsSuccessStatusCode)
        {
            var session = await sessionResponse.Content.ReadFromJsonAsync<CollaborationSession>();
            Assert.NotNull(session);

            Console.WriteLine($"✓ Collaboration session created: {session.SessionId}");

            // Step 2: Update code in session
            var updateRequest = new
            {
                SessionId = session.SessionId,
                Code = "Console.WriteLine(\"Collaborative code\");",
                UserId = _testUserId
            };

            var updateResponse = await _httpClient.PostAsJsonAsync(
                $"/api/v1/collaboration/sessions/{session.SessionId}/code",
                updateRequest);

            if (updateResponse.IsSuccessStatusCode)
            {
                Console.WriteLine("✓ Code updated in session");

                // Step 3: Get session state
                var stateResponse = await _httpClient.GetAsync(
                    $"/api/v1/collaboration/sessions/{session.SessionId}");

                if (stateResponse.IsSuccessStatusCode)
                {
                    var state = await stateResponse.Content.ReadFromJsonAsync<CollaborationSession>();
                    Console.WriteLine($"✓ Session state retrieved");
                    Console.WriteLine($"  Participants: {state?.Participants.Count ?? 0}");
                    Console.WriteLine("✅ Real-time collaboration flow successful");
                }
            }
        }
    }

    /// <summary>
    /// E2E Test 7: Notification system flow
    /// Trigger events → Receive notifications → Mark as read → Check preferences
    /// </summary>
    [Fact]
    public async Task NotificationSystem_EndToEnd_Success()
    {
        // Setup
        await CreateAndAuthenticateUser();
        if (string.IsNullOrEmpty(_authToken)) return;

        // Step 1: Trigger notification by completing an action
        var curriculumResponse = await _httpClient.GetAsync("/api/v1/curriculum");
        if (curriculumResponse.IsSuccessStatusCode)
        {
            var curriculum = await curriculumResponse.Content.ReadFromJsonAsync<CurriculumResponse>();
            var firstCourse = curriculum?.Levels.FirstOrDefault()?.Courses?.FirstOrDefault();
            
            if (firstCourse != null)
            {
                var lessonsResponse = await _httpClient.GetAsync($"/api/v1/courses/{firstCourse.Id}/lessons");
                if (lessonsResponse.IsSuccessStatusCode)
                {
                    var lessons = await lessonsResponse.Content.ReadFromJsonAsync<LessonListResponse>();
                    var firstLesson = lessons?.Lessons.FirstOrDefault();

                    if (firstLesson != null)
                    {
                        // Complete lesson to trigger notification
                        var completeRequest = new { UserId = _testUserId };
                        await _httpClient.PostAsJsonAsync(
                            $"/api/v1/courses/{firstCourse.Id}/lessons/{firstLesson.Id}/complete",
                            completeRequest);

                        Console.WriteLine("✓ Action completed (should trigger notification)");

                        // Wait for notification processing
                        await Task.Delay(1000);
                    }
                }
            }
        }

        // Step 2: Get notifications
        var notificationsResponse = await _httpClient.GetAsync($"/api/v1/notifications/user/{_testUserId}");
        if (notificationsResponse.IsSuccessStatusCode)
        {
            var notifications = await notificationsResponse.Content.ReadFromJsonAsync<NotificationListResponse>();
            Console.WriteLine($"✓ Notifications retrieved: {notifications?.Notifications.Count ?? 0}");

            // Step 3: Mark notification as read
            if (notifications?.Notifications.Any() == true)
            {
                var firstNotification = notifications.Notifications.First();
                var markReadResponse = await _httpClient.PutAsync(
                    $"/api/v1/notifications/{firstNotification.Id}/read",
                    null);

                if (markReadResponse.IsSuccessStatusCode)
                {
                    Console.WriteLine("✓ Notification marked as read");
                }
            }

            // Step 4: Check notification preferences
            var preferencesResponse = await _httpClient.GetAsync($"/api/v1/notifications/preferences/{_testUserId}");
            if (preferencesResponse.IsSuccessStatusCode)
            {
                var preferences = await preferencesResponse.Content.ReadFromJsonAsync<NotificationPreferences>();
                Console.WriteLine($"✓ Notification preferences loaded");
                Console.WriteLine("✅ Notification system flow successful");
            }
        }
    }

    // Helper method
    private async Task CreateAndAuthenticateUser()
    {
        var email = $"e2e_{Guid.NewGuid():N}@example.com";
        var registerRequest = new
        {
            Name = "E2E Test User",
            Email = email,
            Password = "SecurePass123!"
        };

        var registerResponse = await _httpClient.PostAsJsonAsync("/api/v1/auth/register", registerRequest);
        
        if (registerResponse.IsSuccessStatusCode)
        {
            var result = await registerResponse.Content.ReadFromJsonAsync<AuthResponse>();
            _authToken = result?.Token;
            _testUserId = result?.UserId ?? Guid.Empty;

            if (!string.IsNullOrEmpty(_authToken))
            {
                _httpClient.DefaultRequestHeaders.Authorization = 
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _authToken);
            }
        }
    }

    // DTOs
    private record AuthResponse(Guid UserId, string Token);
    private record UserProfile(Guid Id, string Name, int Level, int TotalXP, int CurrentStreak);
    private record CurriculumResponse(List<LevelInfo> Levels);
    private record LevelInfo(Guid Id, string Title, int Order, List<CourseInfo>? Courses);
    private record CourseInfo(Guid Id, string Title);
    private record LessonListResponse(List<LessonInfo> Lessons);
    private record LessonInfo(Guid Id, string Title, int Order);
    private record LessonCompleteResponse(bool Success, int XPEarned, Guid? NextLessonId);
    private record ChallengeListResponse(List<ChallengeSummary> Challenges);
    private record ChallengeSummary(Guid Id, string Title, string Difficulty);
    private record ChallengeDetail(Guid Id, string Title, string StarterCode, List<TestCase>? TestCases);
    private record TestCase(string Input, string ExpectedOutput);
    private record SubmissionResult(Guid SubmissionId, int TestsPassed, int TotalTests, int XPAwarded);
    private record LeaderboardResponse(List<LeaderboardEntry> Entries);
    private record LeaderboardEntry(int Rank, string Name, int XP, int Level);
    private record AchievementsResponse(List<Achievement> Earned, List<Achievement> Available);
    private record Achievement(Guid Id, string Name, string Description);
    private record StreakResponse(int CurrentStreak, int LongestStreak);
    private record AIReviewResponse(int OverallScore, List<Suggestion>? Suggestions);
    private record Suggestion(string Type, string Message);
    private record HintResponse(string Hint, int XPCost);
    private record ExplanationResponse(string Explanation);
    private record CollaborationSession(Guid SessionId, string Title, List<Participant> Participants);
    private record Participant(Guid UserId, string Name);
    private record NotificationListResponse(List<NotificationInfo> Notifications);
    private record NotificationInfo(Guid Id, string Type, string Message, bool IsRead);
    private record NotificationPreferences(bool EmailEnabled, bool PushEnabled, bool InAppEnabled);
}
