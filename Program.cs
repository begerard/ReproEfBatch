using Microsoft.EntityFrameworkCore;
using NodaTime;
using System.Diagnostics;

Console.WriteLine("ReproEfBatch started");
using (var context = new Context())
{
    context.Database.Migrate();

    context.DeleteRangeAsync<Entity>();

    Debug.Assert(context.Entities.Count() == 0);
    Console.WriteLine("ReproEfBatch entities emptied");

    context.Entities.Add(new Entity());
    context.Entities.Add(new Entity());
    context.Entities.Add(new Entity());
    context.SaveChanges();

    Debug.Assert(context.Entities.Count() == 3);
    Console.WriteLine("ReproEfBatch entities added");

    var first = context.Entities.First();
    context.DeleteRangeAsync<Entity>(e => e.Id != first.Id);

    Debug.Assert(context.Entities.Count() == 1);
    Console.WriteLine("ReproEfBatch entities deleted");
}

class Entity
{
    public int Id { get; set; }

    public TimeSpan TimeSpanProp { get; set; }
    public Duration DurationProp { get; set; }
    public TimeOnly TimeOnlyProp { get; set; }
}

class Context : DbContext
{
    public DbSet<Entity> Entities => Set<Entity>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.LogTo(Console.WriteLine);
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=ReproEfBatch;Username=postgres;Password=admin",
            npgsqlOptions => npgsqlOptions.UseNodaTime());
        optionsBuilder.UseBatchEF_Npgsql();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Entity>().Property(e => e.Id).UseIdentityAlwaysColumn();
    }
}