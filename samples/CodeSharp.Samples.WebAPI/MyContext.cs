using CodeSharp.EntityFramework.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CodeSharp.Samples.WebAPI;

public class MyContext : CodeSharpDbContext
{
    public MyContext(DbContextOptions options) : base(options)
    {
    }
}
