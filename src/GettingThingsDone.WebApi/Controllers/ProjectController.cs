using System.Collections.Generic;
using System.Threading.Tasks;
using GettingThingsDone.ApplicationCore.Helpers;
using GettingThingsDone.Contracts.Dto;
using GettingThingsDone.Contracts.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GettingThingsDone.WebApi.Security;

namespace GettingThingsDone.WebApi.Controllers
{
    [Authorize(Roles = UserRoles.User)]
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
        public async Task<ActionResult<List<ProjectDto>>> GetAll()
        {
            return FromValueServiceResult(await _projectService.GetAll());
        }

        [HttpGet("{id}", Name = Routes.GetProjectById)]
        public async Task<ActionResult<ProjectDto>> GetById(int id)
        {
            // Get needed claims from the User.
            var projectId = User.GetOwnerProjectId();

            // See if they match to that what the user is trying to do
            // and return unauthorized response if they do not match.
            if (projectId != id)
                return Unauthorized();

            return FromValueServiceResult(await _projectService.GetProject(id));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProjectDto value)
        {
            if (!value.RepresentsNewEntity)
                return BadRequest();

            var result = await _projectService.CreateOrUpdate(value);

            if (!result.IsOk())
                return FromServiceResult(result);

            return CreatedAtRoute(Routes.GetProjectById, new { id = result.Value.Id }, result.Value);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] ProjectDto value)
        {
            if (value.RepresentsNewEntity)
                return BadRequest();

            var result = await _projectService.CreateOrUpdate(value);

            if (!result.IsOk())
                return FromServiceResult(result);

            return NoContent();
        }

        [Authorize(Policy = CustomPolicies.OnlyUsersOlderThan)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _projectService.Delete(id);

            if (!result.IsOk())
                return FromServiceResult(result);

            return NoContent();
        }

        [HttpGet("{id}/actions")]
        public async Task<ActionResult<List<ActionDto>>> GetProjectActions(int id)
        {
            return FromValueServiceResult(await _projectService.GetProjectActions(id));
        }
    }
}