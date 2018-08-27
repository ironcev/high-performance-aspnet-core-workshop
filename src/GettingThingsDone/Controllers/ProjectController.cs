using System.Collections.Generic;
using System.Linq;
using GettingThingsDone.Database;
using GettingThingsDone.Contract.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GettingThingsDone.Controllers
{
    [Route("api/projects")]
    [ApiController]
    public class ProjectController : BaseController
    {
        private static class Routes
        {
            public const string GetProject = nameof(GetProject);
        }

        public ProjectController(GettingThingsDoneDbContext dbContext) : base(dbContext) { }

        [HttpGet]
        public ActionResult<IEnumerable<Project>> GetAll()
        {
            return DbContext.Projects;
        }

        [HttpGet("{id}", Name = Routes.GetProject)]
        public ActionResult<Project> GetById(int id)
        {
            var project = DbContext.Projects.Find(id);
            if (project == null)
                return NotFound();
            return project;
        }

        [HttpPost]
        public IActionResult Create([FromBody] Project value)
        {
            DbContext.Projects.Add(value);
            DbContext.SaveChanges();

            return CreatedAtRoute(Routes.GetProject, new { id = value.Id }, value);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Project value)
        {
            var project = DbContext.Projects.Find(id);
            if (project == null)
                return NotFound();

            project.Name = value.Name;

            DbContext.Projects.Update(project);
            DbContext.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var project = DbContext.Projects.Find(id);
            if (project == null)
            {
                return NotFound();
            }

            DbContext.Projects.Remove(project);
            DbContext.SaveChanges();

            return NoContent();
        }

        [HttpGet("{id}/actions")]
        public ActionResult<IEnumerable<Action>> GetProjectActions(int id)
        {
            var project = DbContext.Projects.Include(x => x.Actions).FirstOrDefault(l => l.Id == id);
            if (project == null)
                return NotFound();
            var actionsToReturn = RemoveListAndProjectFromActions(project.Actions.ToList());
            return new ActionResult<IEnumerable<Action>>(actionsToReturn);
        }
    }
}