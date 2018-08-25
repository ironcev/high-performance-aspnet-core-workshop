using System.Collections.Generic;
using GettingThingsDone.Database;
using GettingThingsDone.Model;
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

        /// <summary>
        /// Sets the List and Project property on all <paramref name="actions"/> to null.
        /// </summary>
        /// <returns>Actions with List and Project property set to null</returns>
        /// <remarks>
        /// We have to remove the List and Project object from the action object
        /// otherwise the JSON serializer is not able to properly
        /// serialize the result because of circular relationships.
        /// This is most likely not the best way to solve this issue
        /// but at this point in the workshop we simply do no know any
        /// better way :-(
        /// Stay tuned and watch this workaround disappear later.
        /// </remarks>
        protected IEnumerable<Action> RemoveListAndProjectFromActions(List<Action> actions)
        {
            foreach (var action in actions)
            {
                action.List = null;
                action.Project = null;
                yield return action;
            }
        }
    }
}
