using System.Collections.Generic;
using System.Threading.Tasks;
using GettingThingsDone.ApplicationCore.Helpers;
using GettingThingsDone.Contracts.Dto;
using GettingThingsDone.Contracts.Interface;
using Microsoft.AspNetCore.Mvc;

namespace GettingThingsDone.WebApi.Controllers
{
    [Route("api/lists")]
    [ApiController]
    public class ActionListController : BaseController
    {
        private static class Routes
        {
            public const string GetListById = nameof(GetListById);
        }

        private readonly IActionListService _actionListService;

        public ActionListController(IActionListService actionListService)
        {
            _actionListService = actionListService;
        }

        [HttpGet]
        public async Task<ActionResult<List<ActionListDto>>> GetAll()
        {
            return FromValueServiceResult(await _actionListService.GetAll());
        }

        [HttpGet("{id}", Name = Routes.GetListById)]
        public async Task<ActionResult<ActionListDto>> GetById(int id)
        {
            return FromValueServiceResult(await _actionListService.GetList(id));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ActionListDto value)
        {
            if (!value.RepresentsNewEntity)
                return BadRequest();

            var result = await _actionListService.CreateOrUpdate(value);

            if (!result.IsOk())
                return FromServiceResult(result);

            return CreatedAtRoute(Routes.GetListById, new { id = result.Value.Id }, result.Value);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] ActionListDto value)
        {
            if (value.RepresentsNewEntity)
                return BadRequest();

            var result = await _actionListService.CreateOrUpdate(value);

            if (!result.IsOk())
                return FromServiceResult(result);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _actionListService.Delete(id);

            if (!result.IsOk())
                return FromServiceResult(result);

            return NoContent();
        }

        [HttpGet("{id}/actions")]
        public async Task<ActionResult<List<ActionDto>>> GetListActions(int id)
        {
            return FromValueServiceResult(await _actionListService.GetListActions(id));
        }
    }
}