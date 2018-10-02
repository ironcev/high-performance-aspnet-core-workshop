using System.Collections.Generic;
using System.Threading.Tasks;
using GettingThingsDone.ApplicationCore.Helpers;
using GettingThingsDone.Contracts.Dto;
using GettingThingsDone.Contracts.Interface;
using Microsoft.AspNetCore.Mvc;
using Serilog;

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
        //[ResponseCache(Duration = 20, VaryByQueryKeys = new string[]{ "refresh", "new"})]
        public async Task<ActionResult<List<ActionDto>>> GetAll()
        {
            Log.Information("Get all Actions!");
            return FromValueServiceResult(await _actionService.GetAll());
        }

        [HttpGet("{id}", Name = Routes.GetActionById)]
        public async Task<ActionResult<ActionDto>> GetById(int id)
        {
            return FromValueServiceResult(await _actionService.GetAction(id));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ActionDto value)
        {
            Log.Warning("Action create {@value}", value);

            if (!value.RepresentsNewEntity)
                return BadRequest();

            var result = await _actionService.CreateOrUpdate(value);

            if (!result.IsOk())
                return FromServiceResult(result);

            return CreatedAtRoute(Routes.GetActionById, new { id = result.Value.Id }, result.Value);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] ActionDto value)
        {
            if (value.RepresentsNewEntity)
                return BadRequest();

            var result = await _actionService.CreateOrUpdate(value);

            if (!result.IsOk())
                return FromServiceResult(result);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _actionService.Delete(id);

            if (!result.IsOk())
                return FromServiceResult(result);

            return NoContent();
        }

        [HttpPut("{id}/move-to/{listId}")]
        public async Task<IActionResult> MoveToList(int id, int listId)
        {
            var result = await _actionService.MoveToList(id, listId);

            if (!result.IsOk())
                return FromServiceResult(result);

            return NoContent();
        }

        [HttpPut("{id}/assign-to/{projectId}")]
        public async Task<IActionResult> AssignToProject(int id, int projectId)
        {
            var result = await _actionService.AssignToProject(id, projectId);

            if (!result.IsOk())
                return FromServiceResult(result);

            return NoContent();
        }
    }
}