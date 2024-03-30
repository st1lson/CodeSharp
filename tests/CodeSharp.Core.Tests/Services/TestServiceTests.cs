using AutoFixture;
using CodeSharp.Core.Contracts;
using CodeSharp.Core.Executors;
using CodeSharp.Core.Executors.Models.Testing;
using CodeSharp.Core.Models;
using CodeSharp.Core.Services;
using NSubstitute;
using Xunit;

namespace CodeSharp.Core.Tests.Services;

public class TestServiceTests
{
    private readonly ITestStore<Test, Guid> _testStore;
    private readonly ITestExecutor<TestLog> _testExecutor;
    private readonly ITestLogStore<TestLog, Guid> _testLogStore;
    private readonly TestService<Test> _testService;

    private readonly IFixture _fixture = new Fixture();

    public TestServiceTests()
    {
        _testStore = Substitute.For<ITestStore<Test, Guid>>();
        _testExecutor = Substitute.For<ITestExecutor<TestLog>>();
        _testLogStore = Substitute.For<ITestLogStore<TestLog, Guid>>();

        _testService = new TestService<Test>(_testStore, _testExecutor, _testLogStore);
    }

    [Fact]
    public async Task AddTestAsync_ShouldStoreTest()
    {
        // Arrange
        var test = _fixture.Create<Test>();

        // Act
        await _testService.AddTestAsync(test);

        // Assert
        await _testStore.Received(1).CreateAsync(test, CancellationToken.None);
    }

    [Fact]
    public async Task ExecuteTestAsync_ShouldExecuteTestAndStoreTestLog()
    {
        // Arrange
        var test = _fixture.Create<Test>();
        var code = "Test code";
        var testLog = _fixture.Create<TestLog>();
        var testingOptions = TestingOptions.Default;

        _testExecutor.TestAsync(code, test.Tests, testingOptions, CancellationToken.None).Returns(testLog);

        // Act
        var result = await _testService.ExecuteTestAsync(test, code);

        // Assert
        await _testLogStore.Received(1).CreateAsync(testLog, CancellationToken.None);
        Assert.Equal(testLog, result);
    }

    [Fact]
    public async Task ExecuteTestByIdAsync_ShouldExecuteTestAndStoreTestLog()
    {
        // Arrange
        var id = Guid.NewGuid();
        var test = _fixture.Create<Test>();
        var code = "Test code";
        var testLog = _fixture.Create<TestLog>();
        var testingOptions = TestingOptions.Default;

        _testStore.GetByIdAsync(id, CancellationToken.None).Returns(test);
        _testExecutor.TestAsync(code, test.Tests, testingOptions, CancellationToken.None).Returns(testLog);

        // Act
        var result = await _testService.ExecuteTestByIdAsync(id, code);

        // Assert
        await _testLogStore.Received(1).CreateAsync(testLog, CancellationToken.None);
        Assert.Equal(testLog, result);
    }

    [Fact]
    public async Task GetTestAsync_ShouldRetrieveTestById()
    {
        // Arrange
        var id = Guid.NewGuid();
        var test = _fixture.Create<Test>();
        _testStore.GetByIdAsync(id, CancellationToken.None).Returns(test);

        // Act
        var result = await _testService.GetTestAsync(id);

        // Assert
        Assert.Equal(test, result);
    }

    [Fact]
    public async Task GetTestsAsync_ShouldRetrieveAllTests()
    {
        // Arrange
        var tests = _fixture.CreateMany<Test>().ToList();
        _testStore.GetAllAsync(CancellationToken.None).Returns(tests);

        // Act
        var result = await _testService.GetTestsAsync();

        // Assert
        Assert.Equal(tests, result);
    }

    [Fact]
    public async Task DeleteTestAsync_ShouldDeleteTestById()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act
        await _testService.DeleteTestAsync(id);

        // Assert
        await _testStore.Received(1).DeleteAsync(id, CancellationToken.None);
    }

    [Fact]
    public async Task UpdateTestAsync_ShouldUpdateTest()
    {
        // Arrange
        var test = _fixture.Create<Test>();

        // Act
        await _testService.UpdateTestAsync(test);

        // Assert
        await _testStore.Received(1).UpdateAsync(test, CancellationToken.None);
    }
}
