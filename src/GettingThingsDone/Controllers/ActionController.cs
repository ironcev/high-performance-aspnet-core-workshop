using System.Collections.Generic;
using GettingThingsDone.Infrastructure.Database;
using Action = GettingThingsDone.Contract.Model.Action;
using Microsoft.AspNetCore.Mvc;
using GettingThingsDone.Contract.Interface;
using GettingThingsDone.Contract.DTO;

namespace GettingThingsDone.Controllers
{
    [Route("api/actions")]
    [ApiController]
    public class ActionController: Controller 
    {
        private static class Routes
        {
            public const string GetActionById = nameof(GetActionById);
        }

        private readonly IActionService _actionService;
        private GettingThingsDoneDbContext _dbContext;

        public ActionController(GettingThingsDoneDbContext dbContext, IActionService actionService)  {
            _dbContext = dbContext;
            _actionService = actionService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ActionDTO>> GetAll()
        {
            return Ok(_actionService.GetTop(10));
        }

        [HttpGet("{id}", Name = Routes.GetActionById)]
        public ActionResult<ActionDTO> GetById(int id)
        {
            return Ok(_actionService.GetAction(id));

        }

        [HttpPost]
        public IActionResult Create([FromBody] ActionDTO value)
        {
            var action = _actionService.CreateOrUpdate(null, value);
            return CreatedAtRoute(Routes.GetActionById, new { id = action.Id }, action);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] ActionDTO value)
        {
            _actionService.CreateOrUpdate(id, value);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {

            var result = _actionService.Delete(id);
            if (!result.IsSaved) return NotFound();

            return NoContent();
        }

        [HttpPut("{id}/move-to/{listId}")]
        public IActionResult MoveToList(int id, int listId)
        {
            _actionService.MoveToList(id, listId);
            return NoContent();
        }

        [HttpPut("{id}/assign-to/{projectId}")]
        public IActionResult AssignToProject(int id, int projectId)
        {
            _actionService.AssignToProject(id, projectId);
            return NoContent();
        }
    }
}