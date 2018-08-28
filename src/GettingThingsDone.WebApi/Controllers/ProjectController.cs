using System.Collections.Generic;
using System.Linq;
using GettingThingsDone.Contracts.Model;
using GettingThingsDone.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GettingThingsDone.WebApi.Controllers
{
    [Route("api/projects")]
    [ApiController]
    public class ProjectController : BaseController
    {
        private static class Routes
        {
            public const string GetProject = nameof(GetProject);
        }

        private readonly GettingThingsDoneDbContext _dbContext;

        public ProjectController(GettingThingsDoneDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Project>> GetAll()
        {
            return _dbContext.Projects;
        }

        [HttpGet("{id}", Name = Routes.GetProject)]
        public ActionResult<Project> GetById(int id)
        {
            var project = _dbContext.Projects.Find(id);
            if (project == null)
                return NotFound();
            return project;
        }

        [HttpPost]
        public IActionResult Create([FromBody] Project value)
        {
            _dbContext.Projects.Add(value);
            _dbContext.SaveChanges();

            return CreatedAtRoute(Routes.GetProject, new { id = value.Id }, value);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Project value)
        {
            var project = _dbContext.Projects.Find(id);
            if (project == null)
                return NotFound();

            project.Name = value.Name;

            _dbContext.Projects.Update(project);
            _dbContext.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var project = _dbContext.Projects.Find(id);
            if (project == null)
            {
                return NotFound();
            }

            _dbContext.Projects.Remove(project);
            _dbContext.SaveChanges();

            return NoContent();
        }

        [HttpGet("{id}/actions")]
        public ActionResult<IEnumerable<Action>> GetProjectActions(int id)
        {
            var project = _dbContext.Projects.Include(x => x.Actions).FirstOrDefault(l => l.Id == id);
            if (project == null)
                return NotFound();
            var actionsToReturn = RemoveListAndProjectFromActions(project.Actions.ToList());
            return new ActionResult<IEnumerable<Action>>(actionsToReturn);
        }
    }
}