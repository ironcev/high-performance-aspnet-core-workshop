using GettingThingsDone.Contract.Model;
using Action = GettingThingsDone.Contract.Model.Action;
using Microsoft.EntityFrameworkCore;

namespace GettingThingsDone.Infrastructure.Database
{
    public class GettingThingsDoneDbContext : DbContext
    {
        public DbSet<Action> Actions { get; set; }
        public DbSet<ActionList> Lists { get; set; }
        public DbSet<Project> Projects { get; set; }

        public GettingThingsDoneDbContext(DbContextOptions<GettingThingsDoneDbContext> options)
            :base(options)
        {            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ActionList>()
                .HasMany(x => x.Actions)
                .WithOne(x => x.List);

            modelBuilder.Entity<Project>()
                .HasMany(x => x.Actions)
                .WithOne(x => x.Project);
        }
    }
}