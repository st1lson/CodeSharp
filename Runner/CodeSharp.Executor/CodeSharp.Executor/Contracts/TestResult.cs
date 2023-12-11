namespace CodeSharp.Executor.Contracts;

public class TestResult
{
    public required string TestName { get; set; }
    public bool Passed { get; set; }
    public double ExecutionTime { get; set; }
    public required string ErrorMessage { get; set; }
}