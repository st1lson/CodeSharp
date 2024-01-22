using CodeSharp.Core.Contracts;
using CodeSharp.Core.Executors.Models.Testing;

namespace CodeSharp.Core.Models;

public class TestLog : TestLog<Guid>
{
}

public class TestLog<TKey> : TestingResponse, ITestLog<TKey>
{
    public TKey Id { get; }
}