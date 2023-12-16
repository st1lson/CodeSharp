using CodeSharp.Samples.WebAPI.Models;
using CodeSharp.Samples.WebAPI.Models.Requests;
using Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace CodeSharp.Samples.WebAPI.Controllers;

[Route("api/[controller]")]
public class TestingController : ControllerBase
{
    public readonly ITestingService _testingService;

    private static readonly Test _test = new()
    {
        Id = Guid.NewGuid(),
        Description = "Create a simple calculator",
        InitialUserCode = "public class Calculator\n{\n    public int Add(int a, int b)\n    {\n        //Your code here\n    }\n\n    public int Subtract(int a, int b)\n    {\n        //Your code here\n    }\n}",
        TestsCode = "using Xunit;\n\npublic class CalculatorTests\n{\n    [Theory]\n    [InlineData(3, 5, 8)]  // Test case 1: 3 + 5 = 8\n    [InlineData(-3, 7, 4)] // Test case 2: -3 + 7 = 4\n    [InlineData(0, 0, 0)]   // Test case 3: 0 + 0 = 0\n    public void Add_ShouldReturnCorrectSum(int a, int b, int expectedSum)\n    {\n        // Arrange\n        Calculator calculator = new Calculator();\n\n        // Act\n        int result = calculator.Add(a, b);\n\n        // Assert\n        Assert.Equal(expectedSum, result);\n    }\n\n    [Theory]\n    [InlineData(10, 4, 6)]   // Test case 1: 10 - 4 = 6\n    [InlineData(-5, -2, -3)] // Test case 2: -5 - (-2) = -3\n    [InlineData(0, 0, 0)]    // Test case 3: 0 - 0 = 0\n    public void Subtract_ShouldReturnCorrectDifference(int a, int b, int expectedDifference)\n    {\n        // Arrange\n        Calculator calculator = new Calculator();\n\n        // Act\n        int result = calculator.Subtract(a, b);\n\n        // Assert\n        Assert.Equal(expectedDifference, result);\n    }\n}"
    };

    public TestingController(ITestingService testingService)
    {
        _testingService = testingService;
    }

    [HttpGet]
    public IActionResult GetTest()
    {
        return Ok(_test);
    }

    [HttpPost]
    public async Task<IActionResult> Test([FromBody] TestingRequest request)
    {
        if (_test.Id != request.TestId)
        {
            return BadRequest();
        }

        var testingResult = await _testingService.TestAsync(request.Code, _test.TestsCode);

        return Ok(testingResult);
    }
}
