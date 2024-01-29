using CodeSharp.AspNetCore;
using CodeSharp.Core.Models;
using CodeSharp.EntityFramework.Stores;

namespace CodeSharp.EntityFramework.AspNetCore;

public static class ServiceCollectionExtensions
{
    public static EntityFrameworkCodeSharpBuilder AddCodeSharpStores<TContext>(this CodeSharpBuilder builder)
    {
        var efBuilder = new EntityFrameworkCodeSharpBuilder(builder, typeof(TContext));

        efBuilder.AddCompilationLogStore<CompilationLogStore<CompilationLog>>();
        efBuilder.AddTestStore<TestStore<Test>>();
        efBuilder.AddTestLogStore<TestLogStore<TestLog>>();

        return efBuilder;
    }
}
