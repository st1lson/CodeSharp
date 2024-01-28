using CodeSharp.AspNetCore;
using CodeSharp.Core.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace CodeSharp.EntityFramework.AspNetCore;

public class EntityFrameworkCodeSharpBuilder<TCompilationLog, TTest, TTestLog> : CodeSharpBuilder<TCompilationLog, TTest, TTestLog>
{
    public EntityFrameworkCodeSharpBuilder(IServiceCollection services) : base(services)
    {
    }

    public CodeSharpBuilder<TCompilationLog, TTest, TTestLog> AddCompilationLogStore<TCompilationLogStore>() where TCompilationLogStore : class
    {
        var compilationLogType = typeof(TCompilationLog);

        var keyType = GetKeyType(compilationLogType);

        var storeType = typeof(ICompilationLogStore<,>).MakeGenericType(compilationLogType, keyType);
        var implementationType = typeof(TCompilationLogStore);

        RegisterImplementations(storeType, implementationType);

        return this;
    }

    public CodeSharpBuilder<TCompilationLog, TTest, TTestLog> AddTestStore<TTestStore>() where TTestStore : class
    {
        var testType = typeof(TTest);

        var keyType = GetKeyType(testType);

        var storeType = typeof(ITestStore<,>).MakeGenericType(testType, keyType);
        var implementationType = typeof(TTestStore);

        RegisterImplementations(storeType, implementationType);

        return this;
    }

    public CodeSharpBuilder<TCompilationLog, TTest, TTestLog> AddTestLogStore<TTestLogStore>() where TTestLogStore : class
    {
        var testLogType = typeof(TTestLog);

        var keyType = GetKeyType(testLogType);

        var storeType = typeof(ITestLogStore<,>).MakeGenericType(testLogType, keyType);
        var implementationType = typeof(TTestLogStore);

        RegisterImplementations(storeType, implementationType);

        return this;
    }
}
