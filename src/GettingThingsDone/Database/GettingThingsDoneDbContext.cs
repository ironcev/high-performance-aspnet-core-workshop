using GettingThingsDone.Model;
using Action = GettingThingsDone.Model.Action;
using Microsoft.EntityFrameworkCore;

namespace GettingThingsDone.Database
{
    public class GettingThingsDoneDbContext : DbContext
    {
        public DbSet<Action> Actions { get; set; }
        public DbSet<ActionList> Lists { get; set; }

        public GettingThingsDoneDbContext(DbContextOptions<GettingThingsDoneDbContext> options)
            :base(options)
        {            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ActionList>()
                .HasMany(x => x.Actions)
                .WithOne(x => x.List);
        }
    }
}