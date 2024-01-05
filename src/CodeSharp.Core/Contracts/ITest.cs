namespace CodeSharp.Core.Contracts;

public interface ITest : ITest<Guid>
{
}

public interface ITest<TKey>
{
    TKey Id { get; }
    string Tests { get; set; }
}
