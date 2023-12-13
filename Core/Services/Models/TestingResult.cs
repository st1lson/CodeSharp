namespace Core.Services.Models;

public class TestingResult
{
    public bool Success { get; set; }
    public IList<TestResult> TestResults { get; set; } = new List<TestResult>();
}
