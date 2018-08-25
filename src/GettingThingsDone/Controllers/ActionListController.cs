using System.Collections.Generic;
using System.Linq;
using GettingThingsDone.Database;
using GettingThingsDone.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GettingThingsDone.Controllers
{
    [Route("api/lists")]
    [ApiController]
    public class ActionListController : BaseController
    {
        private static class Routes
        {
            public const string GetList = nameof(GetList);
        }

        public ActionListController(GettingThingsDoneDbContext dbContext) : base(dbContext) { }

        [HttpGet]
        public ActionResult<IEnumerable<ActionList>> GetAll()
        {
            return DbContext.Lists;
        }

        [HttpGet("{id}", Name = Routes.GetList)]
        public ActionResult<ActionList> GetById(int id)
        {
            var list = DbContext.Lists.Find(id);
            if (list == null)
                return NotFound();
            return list;
        }

        [HttpPost]
        public IActionResult Create([FromBody] ActionList value)
        {
            DbContext.Lists.Add(value);
            DbContext.SaveChanges();

            return CreatedAtRoute(Routes.GetList, new { id = value.Id }, value);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] ActionList value)
        {
            var list = DbContext.Lists.Find(id);
            if (list == null)
                return NotFound();

            list.Name = value.Name;

            DbContext.Lists.Update(list);
            DbContext.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var list = DbContext.Lists.Find(id);
            if (list == null)
            {
                return NotFound();
            }

            DbContext.Lists.Remove(list);
            DbContext.SaveChanges();

            return NoContent();
        }

        [HttpGet("{id}/actions")]
        public ActionResult<IEnumerable<Action>> GetListActions(int id)
        {
            var list = DbContext.Lists.Include(x => x.Actions).FirstOrDefault(l => l.Id == id);
            if (list == null)
                return NotFound();
            var actionsToReturn = RemoveListAndProjectFromActions(list.Actions.ToList());
            return new ActionResult<IEnumerable<Action>>(actionsToReturn);
        }
    }
}