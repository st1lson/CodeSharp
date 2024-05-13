namespace CodeSharp.Core.Contracts;

public interface ITestLog : ITestLog<Guid>
{
}

public interface ITestLog<out TKey>
{
    TKey Id { get; }
    bool Passed { get; }
}
