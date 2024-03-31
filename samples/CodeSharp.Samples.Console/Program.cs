using CodeSharp.Core.Docker.Factories;
using CodeSharp.Core.Docker.Models;
using CodeSharp.Core.Docker.Providers;
using CodeSharp.Core.Executors;
using CodeSharp.Core.Executors.Models.Compilation;
using CodeSharp.Core.Executors.Models.Testing;
using CodeSharp.Core.Executors.Strategies;

var httpClient = new HttpClient();

IContainerPortProvider portProvider = new ContainerPortProvider();
IContainerEndpointProvider endpointProvider = new ContainerEndpointProvider(portProvider);
IContainerHealthCheckProvider healthCheckProvider = new HttpContainerHealthCheckProvider(endpointProvider, httpClient);
IDockerClientFactory dockerClientFactory = new DockerClientFactory();
ICommunicationStrategy communicationStrategy = new HttpCommunicationStrategy(httpClient);

ContainerConfiguration configuration = new ContainerConfiguration
{
    Image = Image.Default
};

ICompileExecutor<CompilationResponse> compilationService = new CompileExecutor(
    configuration,
    new RandomContainerNameProvider(),
    portProvider,
    healthCheckProvider,
    endpointProvider,
    dockerClientFactory,
    communicationStrategy);

var compilationOptions = new CompilationOptions
{
    Run = true,
    MaxCompilationTime = TimeSpan.FromSeconds(10),
    MaxRamUsage = 10,
    MaxExecutionTime = TimeSpan.FromSeconds(1),
};

var compilationResult = await compilationService.CompileAsync(@"
using System;

namespace CodeSharp.Executor;

class Program
{
    static void Main()
    {
        Console.WriteLine(""Hello, World!"");
    }
}
", compilationOptions);

Console.WriteLine(compilationResult);

var testingOptions = new TestingOptions
{
    MaxCompilationTime = TimeSpan.FromSeconds(10),
    MaxRamUsage = 10,
    MaxTestingTime = TimeSpan.FromSeconds(5),
};

ITestExecutor<TestingResponse> testingService = new TestExecutor(
    configuration,
    new RandomContainerNameProvider(),
    portProvider,
    healthCheckProvider,
    endpointProvider,
    dockerClientFactory,
    communicationStrategy
    );
var testingResult = await testingService.TestAsync(
"public class Calculator\n{\n    public int Add(int a, int b)\n    {\n        return a + b;\n    }\n\n    public int Subtract(int a, int b)\n    {\n        return a - b;\n    }\n}",
"using Xunit;\n\npublic class CalculatorTests\n{\n    [Theory]\n    [InlineData(3, 5, 8)]  // Test case 1: 3 + 5 = 8\n    [InlineData(-3, 7, 4)] // Test case 2: -3 + 7 = 4\n    [InlineData(0, 0, 0)]   // Test case 3: 0 + 0 = 0\n    public void Add_ShouldReturnCorrectSum(int a, int b, int expectedSum)\n    {\n        // Arrange\n        Calculator calculator = new Calculator();\n\n        // Act\n        int result = calculator.Add(a, b);\n\n        // Assert\n        Assert.Equal(expectedSum, result);\n    }\n\n    [Theory]\n    [InlineData(10, 4, 6)]   // Test case 1: 10 - 4 = 6\n    [InlineData(-5, -2, -3)] // Test case 2: -5 - (-2) = -3\n    [InlineData(0, 0, 0)]    // Test case 3: 0 - 0 = 0\n    public void Subtract_ShouldReturnCorrectDifference(int a, int b, int expectedDifference)\n    {\n        // Arrange\n        Calculator calculator = new Calculator();\n\n        // Act\n        int result = calculator.Subtract(a, b);\n\n        // Assert\n        Assert.Equal(expectedDifference, result);\n    }\n}",
testingOptions);

Console.WriteLine(testingResult);