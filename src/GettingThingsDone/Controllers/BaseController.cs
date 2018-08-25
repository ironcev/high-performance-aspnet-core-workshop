using GettingThingsDone.Database;
using Microsoft.AspNetCore.Mvc;

namespace GettingThingsDone.Controllers
{
    public abstract class BaseController : ControllerBase
    {
        protected GettingThingsDoneDbContext DbContext { get; }

        protected BaseController(GettingThingsDoneDbContext dbContext)
        {
            DbContext = dbContext;
        }
    }
}
