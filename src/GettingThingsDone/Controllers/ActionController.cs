using System.Collections.Generic;
using GettingThingsDone.Database;
using Action = GettingThingsDone.Contract.Model.Action;
using Microsoft.AspNetCore.Mvc;

namespace GettingThingsDone.Controllers
{
    [Route("api/actions")]
    [ApiController]
    public class ActionController : BaseController
    {
        private static class Routes
        {
            public const string GetActionById = nameof(GetActionById);
        }

        public ActionController(GettingThingsDoneDbContext dbContext) : base(dbContext) { }

        [HttpGet]
        public ActionResult<IEnumerable<Action>> GetAll()
        {
            return DbContext.Actions;
        }

        [HttpGet("{id}", Name = Routes.GetActionById)]
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

            return CreatedAtRoute(Routes.GetActionById, new { id = value.Id }, value);
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
            var action = DbContext.Actions.Find(id);
            if (action == null)
            {
                return NotFound();
            }

            DbContext.Actions.Remove(action);
            DbContext.SaveChanges();

            return NoContent();
        }

        [HttpPut("{id}/move-to/{listId}")]
        public IActionResult MoveToList(int id, int listId)
        {
            var action = DbContext.Actions.Find(id);
            if (action == null)
                return NotFound();

            var list = DbContext.Lists.Find(listId);
            if (list == null)
                return NotFound();

            action.List = list;

            DbContext.Actions.Update(action);
            DbContext.SaveChanges();

            return NoContent();
        }

        [HttpPut("{id}/assign-to/{projectId}")]
        public IActionResult AssignToProject(int id, int projectId)
        {
            var action = DbContext.Actions.Find(id);
            if (action == null)
                return NotFound();

            var project = DbContext.Projects.Find(projectId);
            if (project == null)
                return NotFound();

            action.Project = project;

            DbContext.Actions.Update(action);
            DbContext.SaveChanges();

            return NoContent();
        }
    }
}