using CodeSharp.AspNetCore;

namespace CodeSharp.EntityFramework.AspNetCore;

public static class ServiceCollectionExtensions
{
    public static EntityFrameworkCodeSharpBuilder AddCodeSharpStores<TContext>(this CodeSharpBuilder builder)
    {
        var efBuilder = new EntityFrameworkCodeSharpBuilder(builder, typeof(TContext));

        efBuilder.AddDefaultStores();

        return efBuilder;
    }
}
