using AutoFixture;
using CodeSharp.Core.Contracts;
using CodeSharp.Core.Models;
using CodeSharp.Core.Services;
using NSubstitute;
using Xunit;

namespace CodeSharp.Core.Tests.Services;

public class TestLogServiceTests
{
    private readonly ITestLogStore<TestLog, Guid> _testLogStore;
    private readonly TestLogService<TestLog> _testLogService;

    private readonly IFixture _fixture = new Fixture();

    public TestLogServiceTests()
    {
        _testLogStore = Substitute.For<ITestLogStore<TestLog, Guid>>();
        _testLogService = new TestLogService<TestLog>(_testLogStore);
    }

    [Fact]
    public async Task AddTestLogAsync_ShouldStoreTestLog()
    {
        // Arrange
        var testLog = _fixture.Create<TestLog>();

        // Act
        await _testLogService.AddTestLogAsync(testLog);

        // Assert
        await _testLogStore.Received(1).CreateAsync(testLog, CancellationToken.None);
    }

    [Fact]
    public async Task GetTestLogAsync_ShouldRetrieveTestLogById()
    {
        // Arrange
        var testLog = _fixture.Create<TestLog>();
        _testLogStore.GetByIdAsync(testLog.Id, CancellationToken.None).Returns(testLog);

        // Act
        var result = await _testLogService.GetTestLogAsync(testLog.Id);

        // Assert
        Assert.Equal(testLog, result);
    }

    [Fact]
    public async Task GetTestLogsAsync_ShouldRetrieveAllTestLogs()
    {
        // Arrange
        var testLogs = _fixture.CreateMany<TestLog>().ToList();
        _testLogStore.GetAllAsync(CancellationToken.None).Returns(testLogs);

        // Act
        var result = await _testLogService.GetTestLogsAsync();

        // Assert
        Assert.Equal(testLogs, result);
    }

    [Fact]
    public async Task RemoveTestLogAsync_ShouldRemoveTestLogById()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act
        await _testLogService.RemoveTestLogAsync(id);

        // Assert
        await _testLogStore.Received(1).RemoveAsync(id, CancellationToken.None);
    }
}
