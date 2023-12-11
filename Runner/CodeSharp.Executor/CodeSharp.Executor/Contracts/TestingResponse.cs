namespace CodeSharp.Executor.Contracts;

public class TestingResponse
{
    public bool Success => TestResults.All(tr => tr.Passed);
    public IList<TestResult> TestResults { get; set; } = new List<TestResult>();
}