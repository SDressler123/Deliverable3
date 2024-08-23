using Microsoft.EntityFrameworkCore;

class PromiDb : DbContext
{
    public PromiDb(DbContextOptions<PromiDb> options)
        : base(options) { }

    public DbSet<Promi> Promis => Set<Promi>();
    public DbSet<Comment> comments => Set<Comment>();
    public DbSet<Score> Scores => Set<Score>();
}
