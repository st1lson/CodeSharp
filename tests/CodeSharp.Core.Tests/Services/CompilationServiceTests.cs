using AutoFixture;
using CodeSharp.Core.Contracts;
using CodeSharp.Core.Executors;
using CodeSharp.Core.Models;
using CodeSharp.Core.Services;
using NSubstitute;
using Xunit;

namespace CodeSharp.Core.Tests.Services;

public class CompilationServiceTests
{
    private readonly ICompileExecutor<CompilationLog> _compileExecutor;
    private readonly ICompilationLogStore<CompilationLog, Guid> _compilationLogStore;
    private readonly CompilationService<CompilationLog> _compilationService;

    private readonly IFixture _fixture = new Fixture();

    public CompilationServiceTests()
    {
        _compileExecutor = Substitute.For<ICompileExecutor<CompilationLog>>();
        _compilationLogStore = Substitute.For<ICompilationLogStore<CompilationLog, Guid>>();
        _compilationService = new CompilationService<CompilationLog>(_compileExecutor, _compilationLogStore);
    }

    [Fact]
    public async Task CompileAsync_ShouldCompileCodeAndStoreCompilationLog()
    {
        // Arrange
        var code = "Test code";
        var compilationLog = _fixture.Create<CompilationLog>();

        _compileExecutor.CompileAsync(code, false, CancellationToken.None).Returns(compilationLog);

        // Act
        var result = await _compilationService.CompileAsync(code);

        // Assert
        await _compilationLogStore.Received(1).CreateAsync(compilationLog, CancellationToken.None);
        Assert.Equal(compilationLog, result);
    }

    [Fact]
    public async Task AddCompilationLogAsync_ShouldStoreCompilationLog()
    {
        // Arrange
        var compilationLog = _fixture.Create<CompilationLog>();

        // Act
        await _compilationService.AddCompilationLogAsync(compilationLog);

        // Assert
        await _compilationLogStore.Received(1).CreateAsync(compilationLog, CancellationToken.None);
    }

    [Fact]
    public async Task GetCompilationLogAsync_ShouldRetrieveCompilationLogById()
    {
        // Arrange
        var compilationLog = _fixture.Create<CompilationLog>();
        _compilationLogStore.GetByIdAsync(compilationLog.Id, CancellationToken.None).Returns(compilationLog);

        // Act
        var result = await _compilationService.GetCompilationLogAsync(compilationLog.Id);

        // Assert
        Assert.Equal(compilationLog, result);
    }

    [Fact]
    public async Task GetCompilationLogsAsync_ShouldRetrieveAllCompilationLogs()
    {
        // Arrange
        var compilationLogs = _fixture.CreateMany<CompilationLog>().ToList();

        _compilationLogStore.GetAllAsync(CancellationToken.None).Returns(compilationLogs);

        // Act
        var result = await _compilationService.GetCompilationLogsAsync();

        // Assert
        Assert.Equal(compilationLogs, result);
    }

    [Fact]
    public async Task RemoveCompilationLogAsync_ShouldRemoveCompilationLogById()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act
        await _compilationService.RemoveCompilationLogAsync(id);

        // Assert
        await _compilationLogStore.Received(1).RemoveAsync(id, CancellationToken.None);
    }
}
