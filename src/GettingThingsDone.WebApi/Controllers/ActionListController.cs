using System.Collections.Generic;
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
        public ActionResult<List<ActionListDto>> GetAll()
        {
            return FromValueServiceResult(_actionListService.GetAll());
        }

        [HttpGet("{id}", Name = Routes.GetListById)]
        public ActionResult<ActionListDto> GetById(int id)
        {
            return FromValueServiceResult(_actionListService.GetList(id));
        }

        [HttpPost]
        public IActionResult Create([FromBody] ActionListDto value)
        {
            if (!value.RepresentsNewEntity)
                return BadRequest();

            var result = _actionListService.CreateOrUpdate(value);

            if (!result.IsOk())
                return FromServiceResult(result);

            return CreatedAtRoute(Routes.GetListById, new { id = result.Value.Id }, result.Value);
        }

        [HttpPut]
        public IActionResult Update([FromBody] ActionListDto value)
        {
            if (value.RepresentsNewEntity)
                return BadRequest();

            var result = _actionListService.CreateOrUpdate(value);

            if (!result.IsOk())
                return FromServiceResult(result);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = _actionListService.Delete(id);

            if (!result.IsOk())
                return FromServiceResult(result);

            return NoContent();
        }

        [HttpGet("{id}/actions")]
        public ActionResult<List<ActionDto>> GetListActions(int id)
        {
            return FromValueServiceResult(_actionListService.GetListActions(id));
        }
    }
}