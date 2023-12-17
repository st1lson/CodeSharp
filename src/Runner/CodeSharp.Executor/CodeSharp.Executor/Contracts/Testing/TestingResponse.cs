using CodeSharp.Executor.Contracts.Shared;

namespace CodeSharp.Executor.Contracts.Testing;

public class TestingResponse : AnalyzableResponse
{
    public bool Success => TestResults.Count > 0 && TestResults.All(tr => tr.Passed);
    public IList<TestResult> TestResults { get; set; } = new List<TestResult>();
}