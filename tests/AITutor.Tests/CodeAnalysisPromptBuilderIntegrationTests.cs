using AITutor.Service.Services;

namespace AITutor.Tests;

/// <summary>
/// Integration tests demonstrating how CodeAnalysisPromptBuilder integrates with the AI Tutor service.
/// These tests validate the complete message structure for Groq API integration.
/// </summary>
public class CodeAnalysisPromptBuilderIntegrationTests
{
    private readonly CodeAnalysisPromptBuilder _builder;

    public CodeAnalysisPromptBuilderIntegrationTests()
    {
        _builder = new CodeAnalysisPromptBuilder();
    }

    [Fact]
    public void BuildMessages_ForControllerCode_CreatesValidMessageStructure()
    {
        // Arrange - Sample controller code with potential issues
        var code = @"
public class UserController : ControllerBase
{
    private readonly UserRepository _repository;

    public UserController()
    {
        _repository = new UserRepository();
    }

    [HttpGet]
    public IActionResult GetUsers()
    {
        var users = _repository.GetAll().Result;
        return Ok(users);
    }
}";
        var context = "This is a user management controller";

        // Act
        var messages = _builder.BuildMessages(code, context);

        // Assert
        Assert.Equal(2, messages.Count);
        
        // Verify system message structure
        var systemMessage = messages[0];
        Assert.Equal("system", systemMessage.Role);
        Assert.Contains("SOLID Principles", systemMessage.Content);
        Assert.Contains("Security Vulnerabilities", systemMessage.Content);
        Assert.Contains("Performance Issues", systemMessage.Content);
        Assert.Contains("ASP.NET Core Conventions", systemMessage.Content);
        
        // Verify user message structure
        var userMessage = messages[1];
        Assert.Equal("user", userMessage.Role);
        Assert.Contains(code, userMessage.Content);
        Assert.Contains(context, userMessage.Content);
        Assert.Contains("```csharp", userMessage.Content);
    }

    [Fact]
    public void BuildMessages_ForRepositoryCode_IncludesRelevantGuidelines()
    {
        // Arrange - Sample repository code
        var code = @"
public class ProductRepository
{
    private readonly string _connectionString = ""Server=localhost;Database=MyDb;User=sa;Password=Pass123"";

    public List<Product> GetProducts(string category)
    {
        using var connection = new SqlConnection(_connectionString);
        var query = ""SELECT * FROM Products WHERE Category = '"" + category + ""'"";
        var command = new SqlCommand(query, connection);
        // ... execute query
    }
}";

        // Act
        var messages = _builder.BuildMessages(code);

        // Assert
        var systemMessage = messages[0];
        
        // Should include security guidelines (SQL injection, hardcoded secrets)
        Assert.Contains("SQL Injection", systemMessage.Content);
        Assert.Contains("Hardcoded Secrets", systemMessage.Content);
        Assert.Contains("parameterized queries", systemMessage.Content);
    }

    [Fact]
    public void BuildMessages_ForServiceCode_IncludesArchitectureGuidelines()
    {
        // Arrange - Sample service code
        var code = @"
public class OrderService
{
    public void ProcessOrder(Order order)
    {
        // Direct database access in service
        var connection = new SqlConnection(""..."");
        // Business logic mixed with data access
    }
}";

        // Act
        var messages = _builder.BuildMessages(code);

        // Assert
        var systemMessage = messages[0];
        
        // Should include architecture guidelines
        Assert.Contains("Clean Architecture", systemMessage.Content);
        Assert.Contains("separation of concerns", systemMessage.Content);
        Assert.Contains("Dependency Injection", systemMessage.Content);
    }

    [Fact]
    public void BuildMessages_ValidatesRequirements_4_2_Through_4_5()
    {
        // Arrange
        var code = "public class TestClass { }";

        // Act
        var messages = _builder.BuildMessages(code);
        var systemPrompt = messages[0].Content;

        // Assert - Requirement 4.2: Evaluate code for SOLID principles compliance
        Assert.Contains("Single Responsibility Principle", systemPrompt);
        Assert.Contains("Open/Closed Principle", systemPrompt);
        Assert.Contains("Liskov Substitution Principle", systemPrompt);
        Assert.Contains("Interface Segregation Principle", systemPrompt);
        Assert.Contains("Dependency Inversion Principle", systemPrompt);

        // Assert - Requirement 4.3: Evaluate code for clean architecture patterns
        Assert.Contains("Clean Architecture", systemPrompt);
        Assert.Contains("separation of concerns", systemPrompt);

        // Assert - Requirement 4.4: Identify security vulnerabilities in the code
        Assert.Contains("Security Vulnerabilities", systemPrompt);
        Assert.Contains("SQL Injection", systemPrompt);
        Assert.Contains("Cross-Site Scripting", systemPrompt);
        Assert.Contains("Authentication Issues", systemPrompt);
        Assert.Contains("Sensitive Data Exposure", systemPrompt);

        // Assert - Requirement 4.5: Identify performance issues in the code
        Assert.Contains("Performance Issues", systemPrompt);
        Assert.Contains("N+1 Query Problem", systemPrompt);
        Assert.Contains("Memory Leaks", systemPrompt);
        Assert.Contains("Blocking Calls", systemPrompt);
    }

    [Fact]
    public void BuildMessages_UserPrompt_FormatsCodeCorrectly()
    {
        // Arrange
        var code = @"public class MyClass
{
    public int Id { get; set; }
}";
        var context = "Simple entity class";

        // Act
        var messages = _builder.BuildMessages(code, context);
        var userPrompt = messages[1].Content;

        // Assert
        Assert.Contains("Please analyze the following C# code:", userPrompt);
        Assert.Contains("## Context", userPrompt);
        Assert.Contains(context, userPrompt);
        Assert.Contains("## Code to Analyze", userPrompt);
        Assert.Contains("```csharp", userPrompt);
        Assert.Contains(code, userPrompt);
        Assert.Contains("```", userPrompt);
        Assert.Contains("Provide your analysis in the JSON format", userPrompt);
    }

    [Fact]
    public void BuildMessages_WithoutContext_OmitsContextSection()
    {
        // Arrange
        var code = "public class MyClass { }";

        // Act
        var messages = _builder.BuildMessages(code);
        var userPrompt = messages[1].Content;

        // Assert
        Assert.DoesNotContain("## Context", userPrompt);
        Assert.Contains("## Code to Analyze", userPrompt);
        Assert.Contains(code, userPrompt);
    }

    [Fact]
    public void BuildMessages_ExpectedJSONFormat_IsDocumented()
    {
        // Arrange
        var code = "public class MyClass { }";

        // Act
        var messages = _builder.BuildMessages(code);
        var systemPrompt = messages[0].Content;

        // Assert - Verify the expected JSON structure is documented
        Assert.Contains("Feedback Format", systemPrompt);
        Assert.Contains("\"suggestions\"", systemPrompt);
        Assert.Contains("\"type\"", systemPrompt);
        Assert.Contains("\"message\"", systemPrompt);
        Assert.Contains("\"lineNumber\"", systemPrompt);
        Assert.Contains("\"codeExample\"", systemPrompt);
        Assert.Contains("\"overallScore\"", systemPrompt);
        Assert.Contains("\"securityIssues\"", systemPrompt);
        Assert.Contains("\"performanceIssues\"", systemPrompt);
    }

    [Fact]
    public void BuildMessages_FeedbackTypes_AreDocumented()
    {
        // Arrange
        var code = "public class MyClass { }";

        // Act
        var messages = _builder.BuildMessages(code);
        var systemPrompt = messages[0].Content;

        // Assert - Verify all feedback types are documented
        Assert.Contains("Security", systemPrompt);
        Assert.Contains("Performance", systemPrompt);
        Assert.Contains("BestPractice", systemPrompt);
        Assert.Contains("Architecture", systemPrompt);
    }

    [Theory]
    [InlineData("Controller with dependency injection issues")]
    [InlineData("Repository with SQL injection vulnerability")]
    [InlineData("Service with performance problems")]
    public void BuildMessages_WithVariousContexts_IncludesContextInUserMessage(string context)
    {
        // Arrange
        var code = "public class TestClass { }";

        // Act
        var messages = _builder.BuildMessages(code, context);

        // Assert
        Assert.Contains(context, messages[1].Content);
    }

    [Fact]
    public void BuildMessages_LargeCodeSnippet_HandlesCorrectly()
    {
        // Arrange - Simulate a larger code file
        var code = @"
using System;
using Microsoft.AspNetCore.Mvc;

namespace MyApp.Controllers
{
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IProductService productService, ILogger<ProductController> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            try
            {
                var products = await _productService.GetAllAsync();
                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ""Error retrieving products"");
                return StatusCode(500, ""Internal server error"");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] ProductDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var product = await _productService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetProducts), new { id = product.Id }, product);
        }
    }
}";

        // Act
        var messages = _builder.BuildMessages(code);

        // Assert
        Assert.Equal(2, messages.Count);
        Assert.Contains(code, messages[1].Content);
        Assert.Contains("```csharp", messages[1].Content);
    }
}
