using CodeSharp.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace CodeSharp.EntityFramework.Contexts;

public class CodeSharpDbContext : DbContext
{
    public CodeSharpDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Test> Tests => Set<Test>();
    public DbSet<TestLog> TestLogs => Set<TestLog>();
    public DbSet<CompilationLog> CompilationLogs => Set<CompilationLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }
}
