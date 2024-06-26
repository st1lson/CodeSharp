﻿using CodeSharp.Core.Contracts;

namespace CodeSharp.Core.Models;

public class Test : Test<Guid>
{
}

public class Test<TKey> : ITest<TKey>
{
    public TKey Id { get; set; } = default!;
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required string Tests { get; set; }
}
