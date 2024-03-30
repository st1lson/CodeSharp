using CodeSharp.Executor.Contracts.Shared;

namespace CodeSharp.Executor.Contracts.Testing;

public class TestingResponse : AnalyzableResponse
{
    public bool Passed => TestResults.All(tr => tr.Passed) && CompiledSuccessfully && TestedSuccessfully;
    public bool CompiledSuccessfully { get; set; }
    public bool TestedSuccessfully { get; set; }
    public TimeSpan CompilationDuration { get; set; }
    public TimeSpan? TestingDuration { get; set; }
    public IList<TestResult> TestResults { get; set; } = new List<TestResult>();
}