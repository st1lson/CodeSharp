using CodeSharp.AspNetCore;
using CodeSharp.Core.Models;
using CodeSharp.EntityFramework.Stores;

namespace CodeSharp.EntityFramework.AspNetCore;

public static class ServiceCollectionExtensions
{
    public static EntityFrameworkCodeSharpBuilder<TCompilationLog, TTest, TTestLog> AddCodeSharp<TCompilationLog, TTest, TTestLog>(this CodeSharpBuilder<TCompilationLog, TTest, TTestLog> builder)
    {
        if (builder is not EntityFrameworkCodeSharpBuilder<TCompilationLog, TTest, TTestLog> efBuilder)
        {
            throw new InvalidOperationException("Invalid builder type.");
        }

        efBuilder.AddCompilationLogStore<CompilationLogStore<CompilationLog>>();
        efBuilder.AddTestStore<TestStore<Test>>();
        efBuilder.AddTestLogStore<TestLogStore<TestLog>>();

        return efBuilder;
    }
}
