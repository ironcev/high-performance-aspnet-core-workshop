using System.Collections.Generic;
using GettingThingsDone.Database;
using Action = GettingThingsDone.Model.Action;
using Microsoft.AspNetCore.Mvc;

namespace GettingThingsDone.Controllers
{
    [Route("api/actions")]
    [ApiController]
    public class ActionController : BaseController
    {
        private static class Routes
        {
            public const string GetAction = nameof(GetAction);
        }

        public ActionController(GettingThingsDoneDbContext dbContext) : base(dbContext) { }

        [HttpGet]
        public ActionResult<IEnumerable<Action>> GetAll()
        {
            return DbContext.Actions;
        }

        [HttpGet("{id}", Name = Routes.GetAction)]
        public ActionResult<Action> GetById(int id)
        {
            var action = DbContext.Actions.Find(id);
            if (action == null)
                return NotFound();
            return action;
        }

        [HttpPost]
        public IActionResult Create([FromBody] Action value)
        {
            DbContext.Actions.Add(value);
            DbContext.SaveChanges();

            return CreatedAtRoute(Routes.GetAction, new { id = value.Id }, value);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Action value)
        {
            var action = DbContext.Actions.Find(id);
            if (action == null)
                return NotFound();

            action.Title = value.Title;
            action.DueDate = value.DueDate;
            action.DoOn = value.DoOn;
            action.DoneAt = value.DoneAt;

            DbContext.Actions.Update(action);
            DbContext.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var todo = DbContext.Actions.Find(id);
            if (todo == null)
            {
                return NotFound();
            }

            DbContext.Actions.Remove(todo);
            DbContext.SaveChanges();

            return NoContent();
        }
    }
}
