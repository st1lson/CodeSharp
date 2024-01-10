namespace CodeSharp.Core.Contracts;

public interface ITestLog : ITestLog<Guid>
{
}

public interface ITestLog<TKey>
{
    TKey Id { get; }
    bool Success { get; set; }
}
