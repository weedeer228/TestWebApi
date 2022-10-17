using Microsoft.EntityFrameworkCore;

namespace TestWebApi.Data;

public class TestWebApiContext : DbContext
{
    public TestWebApiContext(DbContextOptions<TestWebApiContext> options)
        : base(options)
    {

    }

    public DbSet<Game> Games { get; set; } = default!;
    public DbSet<Genre> Genres { get; set; } = default!;
}
