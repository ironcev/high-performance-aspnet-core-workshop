using System.Collections.Generic;
using System.Linq;
using GettingThingsDone.ApplicationCore.Helpers;
using GettingThingsDone.Contracts.Dto;
using GettingThingsDone.Contracts.Interface;
using GettingThingsDone.Contracts.Model;
using static GettingThingsDone.Contracts.Interface.ServiceResult;

namespace GettingThingsDone.ApplicationCore.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IRepository<Project> _projectRepository;

        public ProjectService(IRepository<Project> projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public ServiceResult<ProjectDto> GetProject(int id)
        {
            var project = _projectRepository.GetById(id);

            if (project == null)
                return EntityNotFound<ProjectDto>();

            return project
                .TranslateTo<ProjectDto>()
                .ToOkServiceResult();
        }

        public ServiceResult<List<ProjectDto>> GetAll()
        {
            return _projectRepository
                .GetAll()
                .Select(list => list.TranslateTo<ProjectDto>())
                .ToList()
                .ToOkServiceResult();
        }

        public ServiceResult<ProjectDto> CreateOrUpdate(ProjectDto projectDto)
        {
            Project project = projectDto.RepresentsNewEntity
                ? projectDto.TranslateTo<Project>()
                : _projectRepository.GetById(projectDto.Id).CopyPropertiesFrom(projectDto);

            if (project == null)
                return EntityNotFound(projectDto);

            // TODO: Later on we will do the checks here.
            //       So far we assume everything always works fine.

            project = _projectRepository.AddOrUpdate(project);

            projectDto.Id = project.Id;

            return Ok(projectDto);
        }

        public ServiceResult<List<ActionDto>> GetProjectActions(int id)
        {
            var list = _projectRepository.GetByIdAndInclude(id, x => x.Actions);
            if (list == null)
                return EntityNotFound<List<ActionDto>>();

            return list.Actions
                .Select(action => action.TranslateTo<ActionDto>())
                .ToList()
                .ToOkServiceResult();
        }

        public ServiceResult Delete(int id)
        {
            var project = _projectRepository.GetById(id);

            if (project == null)
                return EntityNotFound();

            _projectRepository.Delete(project);

            return Ok();
        }
    }
}