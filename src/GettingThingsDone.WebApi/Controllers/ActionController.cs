using System.Collections.Generic;
using GettingThingsDone.ApplicationCore.Helpers;
using GettingThingsDone.Contracts.Dto;
using GettingThingsDone.Contracts.Interface;
using Microsoft.AspNetCore.Mvc;

namespace GettingThingsDone.WebApi.Controllers
{
    [Route("api/actions")]
    [ApiController]
    public class ActionController: BaseController 
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
        public ActionResult<List<ActionDto>> GetAll()
        {
            return FromValueServiceResult(_actionService.GetAll());
        }

        [HttpGet("{id}", Name = Routes.GetActionById)]
        public ActionResult<ActionDto> GetById(int id)
        {
            return FromValueServiceResult(_actionService.GetAction(id));
        }

        [HttpPost]
        public IActionResult Create([FromBody] ActionDto value)
        {
            if (!value.RepresentsNewEntity)
                return BadRequest();

            var result = _actionService.CreateOrUpdate(value);

            if (!result.IsOk())
                return FromServiceResult(result);

            return CreatedAtRoute(Routes.GetActionById, new { id = result.Value.Id }, result.Value);
        }

        [HttpPut]
        public IActionResult Update([FromBody] ActionDto value)
        {
            if (value.RepresentsNewEntity)
                return BadRequest();

            var result = _actionService.CreateOrUpdate(value);

            if (!result.IsOk())
                return FromServiceResult(result);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = _actionService.Delete(id);

            if (!result.IsOk())
                return FromServiceResult(result);

            return NoContent();
        }

        [HttpPut("{id}/move-to/{listId}")]
        public IActionResult MoveToList(int id, int listId)
        {
            var result = _actionService.MoveToList(id, listId);

            if (!result.IsOk())
                return FromServiceResult(result);

            return NoContent();
        }

        [HttpPut("{id}/assign-to/{projectId}")]
        public IActionResult AssignToProject(int id, int projectId)
        {
            var result =_actionService.AssignToProject(id, projectId);

            if (!result.IsOk())
                return FromServiceResult(result);

            return NoContent();
        }
    }
}