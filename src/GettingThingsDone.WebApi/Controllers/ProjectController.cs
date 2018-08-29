using System.Collections.Generic;
using GettingThingsDone.ApplicationCore.Helpers;
using GettingThingsDone.Contracts.Dto;
using GettingThingsDone.Contracts.Interface;
using Microsoft.AspNetCore.Mvc;

namespace GettingThingsDone.WebApi.Controllers
{
    [Route("api/projects")]
    [ApiController]
    public class ProjectController : BaseController
    {
        private static class Routes
        {
            public const string GetProjectById = nameof(GetProjectById);
        }

        private readonly IProjectService _projectService;

        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpGet]
        public ActionResult<List<ProjectDto>> GetAll()
        {
            return FromValueServiceResult(_projectService.GetAll());
        }

        [HttpGet("{id}", Name = Routes.GetProjectById)]
        public ActionResult<ProjectDto> GetById(int id)
        {
            return FromValueServiceResult(_projectService.GetProject(id));
        }

        [HttpPost]
        public IActionResult Create([FromBody] ProjectDto value)
        {
            if (!value.RepresentsNewEntity)
                return BadRequest();

            var result = _projectService.CreateOrUpdate(value);

            if (!result.IsOk())
                return FromServiceResult(result);

            return CreatedAtRoute(Routes.GetProjectById, new { id = result.Value.Id }, result.Value);
        }

        [HttpPut]
        public IActionResult Update([FromBody] ProjectDto value)
        {
            if (value.RepresentsNewEntity)
                return BadRequest();

            var result = _projectService.CreateOrUpdate(value);

            if (!result.IsOk())
                return FromServiceResult(result);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = _projectService.Delete(id);

            if (!result.IsOk())
                return FromServiceResult(result);

            return NoContent();
        }

        [HttpGet("{id}/actions")]
        public ActionResult<List<ActionDto>> GetProjectActions(int id)
        {
            return FromValueServiceResult(_projectService.GetProjectActions(id));
        }
    }
}