using Microsoft.EntityFrameworkCore;

namespace CodeSharp.Samples.WebAPI;

public class MyContext : DbContext
{
    public MyContext(DbContextOptions options) : base(options)
    {
    }
}
