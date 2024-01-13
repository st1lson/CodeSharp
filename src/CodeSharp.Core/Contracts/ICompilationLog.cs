namespace CodeSharp.Core.Contracts;

public interface ICompilationLog : ICompilationLog<Guid>
{
}

public interface ICompilationLog<TKey>
{
    TKey Id { get; }
    bool Success { get; set; }
}
