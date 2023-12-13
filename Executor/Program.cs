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
@"using System;\r\n\r\npublic class Calculator\r\n{\r\n    public int Add(int a, int b)\r\n    {\r\n        return a + b;\r\n    }\r\n\r\n    public int Subtract(int a, int b)\r\n    {\r\n        return a - b;\r\n    }\r\n}",
@"// CalculatorTests.cs\r\nusing Xunit;\r\n\r\npublic class CalculatorTests\r\n{\r\n    [Theory]\r\n    [InlineData(3, 4, 7)]\r\n    [InlineData(-2, 5, 3)]\r\n    [InlineData(0, 0, 0)]\r\n    public void Add_ShouldReturnCorrectResult(int a, int b, int expected)\r\n    {\r\n        // Arrange\r\n        var calculator = new Calculator();\r\n\r\n        // Act\r\n        int result = calculator.Add(a, b);\r\n\r\n        // Assert\r\n        Assert.Equal(expected, result);\r\n    }\r\n\r\n    [Theory]\r\n    [InlineData(3, 4, -1)]\r\n    [InlineData(-2, 5, -7)]\r\n    [InlineData(0, 0, 0)]\r\n    public void Subtract_ShouldReturnCorrectResult(int a, int b, int expected)\r\n    {\r\n        // Arrange\r\n        var calculator = new Calculator();\r\n\r\n        // Act\r\n        int result = calculator.Subtract(a, b);\r\n\r\n        // Assert\r\n        Assert.Equal(expected, result);\r\n    }\r\n}\r\n");

Console.WriteLine(testingResult);