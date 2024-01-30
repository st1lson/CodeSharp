using CodeSharp.AspNetCore;
using CodeSharp.Core.Contracts;
using CodeSharp.EntityFramework.Stores;
using Microsoft.Extensions.DependencyInjection;

namespace CodeSharp.EntityFramework.AspNetCore;

public class EntityFrameworkCodeSharpBuilder : CodeSharpBuilder
{
    public Type DbContextType { get; }

    public EntityFrameworkCodeSharpBuilder(IServiceCollection services, Type compilationLogType, Type testType, Type testLogType, Type dbContextType)
        : base(services, compilationLogType, testType, testLogType)
    {
        DbContextType = dbContextType;
    }

    public EntityFrameworkCodeSharpBuilder(CodeSharpBuilder baseBuilder, Type dbContextType)
        : this(baseBuilder.Services, baseBuilder.CompilationLogType, baseBuilder.TestType, baseBuilder.TestLogType, dbContextType)
    {
    }

    internal void AddDefaultStores()
    {
        ////
        //var addCompilationLogStoreMethod = GetType().GetMethod(nameof(AddCompilationLogStore))!;

        //var compilationLogKeyType = GetKeyType(CompilationLogType);

        //var compilationLogStoreType = typeof(CompilationLogStore<,,>).MakeGenericType(CompilationLogType, compilationLogKeyType, DbContextType);

        //var genericAddCompilationLogStoreMethod = addCompilationLogStoreMethod.MakeGenericMethod(compilationLogStoreType);
        //genericAddCompilationLogStoreMethod.Invoke(this, null);

        ////
        //var addTestStoreMethod = GetType().GetMethod(nameof(AddTestStore))!;

        //var testKeyType = GetKeyType(TestType);

        //var testStoreType = typeof(TestStore<,,>).MakeGenericType(TestType, testKeyType, DbContextType);

        //var genericAddTestStoreMethod = addTestStoreMethod.MakeGenericMethod(testStoreType);
        //genericAddTestStoreMethod.Invoke(this, null);

        ////
        //var addTestLogStoreMethod = GetType().GetMethod(nameof(AddTestLogStore))!;

        //var testLogKeyType = GetKeyType(TestLogType);

        //var testLogStoreType = typeof(TestLogStore<,,>).MakeGenericType(TestLogType, testLogKeyType, DbContextType);

        //var genericAddTestLogStoreMethod = addTestLogStoreMethod.MakeGenericMethod(testLogStoreType);
        //genericAddTestLogStoreMethod.Invoke(this, null);

        AddStore(typeof(CompilationLogStore<,,>), nameof(AddCompilationLogStore), CompilationLogType);
        AddStore(typeof(TestStore<,,>), nameof(AddTestStore), TestType);
        AddStore(typeof(TestLogStore<,,>), nameof(AddTestLogStore), TestLogType);
    }

    public CodeSharpBuilder AddCompilationLogStore<TCompilationLogStore>() where TCompilationLogStore : class
    {
        var keyType = GetKeyType(CompilationLogType);

        var storeType = typeof(ICompilationLogStore<,>).MakeGenericType(CompilationLogType, keyType);
        var implementationType = typeof(TCompilationLogStore);

        RegisterImplementations(storeType, implementationType);

        return this;
    }

    public CodeSharpBuilder AddTestStore<TTestStore>() where TTestStore : class
    {
        var keyType = GetKeyType(TestType);

        var storeType = typeof(ITestStore<,>).MakeGenericType(TestType, keyType);
        var implementationType = typeof(TTestStore);

        RegisterImplementations(storeType, implementationType);

        return this;
    }

    public CodeSharpBuilder AddTestLogStore<TTestLogStore>() where TTestLogStore : class
    {
        var keyType = GetKeyType(TestLogType);

        var storeType = typeof(ITestLogStore<,>).MakeGenericType(TestLogType, keyType);
        var implementationType = typeof(TTestLogStore);

        RegisterImplementations(storeType, implementationType);

        return this;
    }

    private void AddStore(Type storeType, string methodName, Type itemType)
    {
        var method = GetType().GetMethod(methodName)!;
        var keyType = GetKeyType(itemType);
        var storeGenericType = storeType.MakeGenericType(itemType, keyType, DbContextType);
        var genericMethod = method.MakeGenericMethod(storeGenericType);
        genericMethod.Invoke(this, null);
    }
}
