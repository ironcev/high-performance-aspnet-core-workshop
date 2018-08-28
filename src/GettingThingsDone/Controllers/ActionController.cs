using System.Collections.Generic;
using GettingThingsDone.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;
using GettingThingsDone.Contract.Interface;
using GettingThingsDone.Contract.Dto;

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

        public ActionController(GettingThingsDoneDbContext dbContext, IActionService actionService)
        {
            _actionService = actionService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ActionDto>> GetAll()
        {
            return Ok(_actionService.GetTop(10));
        }

        [HttpGet("{id}", Name = Routes.GetActionById)]
        public ActionResult<ActionDto> GetById(int id)
        {
            return Ok(_actionService.GetAction(id));
        }

        [HttpPost]
        public IActionResult Create([FromBody] ActionDto value)
        {
            var action = _actionService.CreateOrUpdate(null, value);
            return CreatedAtRoute(Routes.GetActionById, new { id = action.Id }, action);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] ActionDto value)
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