using Core.Docker.Models;
using Core.Docker.Providers;
using Core.Services;

IContainerPortProvider portProvider = new ContainerPortProvider();
IContainerEndpointProvider endpointProvider = new ContainerEndpointProvider(portProvider);
IContainerHealthCheckProvider healthCheckProvider = new HttpContainerHealthCheckProvider(endpointProvider);

ContainerConfiguration configuration = new ContainerConfiguration
{
    Image = Image.CreateImage("codesharp.executor:latest")
};

//ICompilationService compilationService = new CompilationService(
//    configuration,
//    new RandomContainerNameProvider(),
//    portProvider,
//    healthCheckProvider,
//    endpointProvider);
//var compilationResult = await compilationService.CompileAsync(@"
//using System;

//class Program
//{
//    static void Main()
//    {
//        Console.WriteLine(""Hello, World!"");
//    }
//}
//");

ITestingService testingService = new TestingService(
    configuration,
    new RandomContainerNameProvider(),
    portProvider,
    healthCheckProvider,
    endpointProvider);
var testingResult = await testingService.TestAsync(
"public class Calculator\n{\n    public int Add(int a, int b)\n    {\n        return a + b;\n    }\n\n    public int Subtract(int a, int b)\n    {\n        return a - b;\n    }\n}",
"using Xunit;\n\npublic class CalculatorTests\n{\n    [Theory]\n    [InlineData(3, 5, 8)]  // Test case 1: 3 + 5 = 8\n    [InlineData(-3, 7, 4)] // Test case 2: -3 + 7 = 4\n    [InlineData(0, 0, 0)]   // Test case 3: 0 + 0 = 0\n    public void Add_ShouldReturnCorrectSum(int a, int b, int expectedSum)\n    {\n        // Arrange\n        Calculator calculator = new Calculator();\n\n        // Act\n        int result = calculator.Add(a, b);\n\n        // Assert\n        Assert.Equal(expectedSum, result);\n    }\n\n    [Theory]\n    [InlineData(10, 4, 6)]   // Test case 1: 10 - 4 = 6\n    [InlineData(-5, -2, -3)] // Test case 2: -5 - (-2) = -3\n    [InlineData(0, 0, 0)]    // Test case 3: 0 - 0 = 0\n    public void Subtract_ShouldReturnCorrectDifference(int a, int b, int expectedDifference)\n    {\n        // Arrange\n        Calculator calculator = new Calculator();\n\n        // Act\n        int result = calculator.Subtract(a, b);\n\n        // Assert\n        Assert.Equal(expectedDifference, result);\n    }\n}");

Console.WriteLine(testingResult);