using CodeSharp.AspNetCore;
using CodeSharp.Core.Models;
using CodeSharp.EntityFramework.Stores;

namespace CodeSharp.EntityFramework.AspNetCore;

public static class ServiceCollectionExtensions
{
    public static EntityFrameworkCodeSharpBuilder<TCompilationLog, TTest, TTestLog> AddCodeSharpStores<TCompilationLog, TTest, TTestLog>(this CodeSharpBuilder<TCompilationLog, TTest, TTestLog> builder)
    {
        var efBuilder = new EntityFrameworkCodeSharpBuilder<TCompilationLog, TTest, TTestLog>(builder.Services);
        
        efBuilder.AddCompilationLogStore<CompilationLogStore<CompilationLog>>();
        efBuilder.AddTestStore<TestStore<Test>>();
        efBuilder.AddTestLogStore<TestLogStore<TestLog>>();

        return efBuilder;
    }
}
