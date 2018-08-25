using Action = GettingThingsDone.Model.Action;
using Microsoft.EntityFrameworkCore;

namespace GettingThingsDone.Database
{
    public class GettingThingsDoneDbContext : DbContext
    {
        public DbSet<Action> Actions { get; set; }

        public GettingThingsDoneDbContext(DbContextOptions<GettingThingsDoneDbContext> options)
            :base(options)
        {            
        }
    }
}
