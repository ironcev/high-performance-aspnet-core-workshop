using System.Collections.Generic;
using GettingThingsDone.Contracts.Dto;
using GettingThingsDone.Contracts.Interface;
using Microsoft.AspNetCore.Mvc;

namespace GettingThingsDone.WebApi.Controllers
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

        public ActionController(IActionService actionService)
        {
            _actionService = actionService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ActionDto>> GetAll()
        {
            return Ok(_actionService.GetAll());
        }

        [HttpGet("{id}", Name = Routes.GetActionById)]
        public ActionResult<ActionDto> GetById(int id)
        {
            return Ok(_actionService.GetAction(id));
        }

        [HttpPost]
        public IActionResult Create([FromBody] ActionDto value)
        {
            if (!value.RepresentsNewEntity)
                return BadRequest();

            var action = _actionService.CreateOrUpdate(value);
            return CreatedAtRoute(Routes.GetActionById, new { id = action.Id }, action);
        }

        [HttpPut]
        public IActionResult Update([FromBody] ActionDto value)
        {
            if (value.RepresentsNewEntity)
                return BadRequest();

            _actionService.CreateOrUpdate(value);

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