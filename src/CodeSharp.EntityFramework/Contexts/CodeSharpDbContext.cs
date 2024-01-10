using CodeSharp.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace CodeSharp.EntityFramework.Contexts;

public class CodeSharpDbContext : DbContext
{
    public CodeSharpDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Test> Tests { get; set; }
}
