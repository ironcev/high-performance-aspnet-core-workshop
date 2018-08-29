using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GettingThingsDone.ApplicationCore.Helpers;
using GettingThingsDone.Contracts.Dto;
using GettingThingsDone.Contracts.Interface;
using GettingThingsDone.Contracts.Model;
using static GettingThingsDone.Contracts.Interface.ServiceResult;

namespace GettingThingsDone.ApplicationCore.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IAsyncRepository<Project> _projectRepository;

        public ProjectService(IAsyncRepository<Project> projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task<ServiceResult<ProjectDto>> GetProject(int id)
        {
            var project = await _projectRepository.GetById(id);

            if (project == null)
                return EntityNotFound<ProjectDto>();

            return project
                .TranslateTo<ProjectDto>()
                .ToOkServiceResult();
        }

        public async Task<ServiceResult<List<ProjectDto>>> GetAll()
        {
            return (await _projectRepository
                .GetAll())
                .Select(list => list.TranslateTo<ProjectDto>())
                .ToList()
                .ToOkServiceResult();
        }

        public async Task<ServiceResult<ProjectDto>> CreateOrUpdate(ProjectDto projectDto)
        {
            Project project = projectDto.RepresentsNewEntity
                ? projectDto.TranslateTo<Project>()
                : (await _projectRepository.GetById(projectDto.Id)).CopyPropertiesFrom(projectDto);

            if (project == null)
                return EntityNotFound(projectDto);

            // TODO: Later on we will do the checks here.
            //       So far we assume everything always works fine.

            project = await _projectRepository.AddOrUpdate(project);

            projectDto.Id = project.Id;

            return Ok(projectDto);
        }

        public async Task<ServiceResult<List<ActionDto>>> GetProjectActions(int id)
        {
            var project = await _projectRepository.GetByIdAndInclude(id, x => x.Actions);
            if (project == null)
                return EntityNotFound<List<ActionDto>>();

            return project.Actions
                .Select(action => action.TranslateTo<ActionDto>())
                .ToList()
                .ToOkServiceResult();
        }

        public async Task<ServiceResult> Delete(int id)
        {
            var project = await _projectRepository.GetById(id);

            if (project == null)
                return EntityNotFound();

            await _projectRepository.Delete(project);

            return Ok();
        }
    }
}