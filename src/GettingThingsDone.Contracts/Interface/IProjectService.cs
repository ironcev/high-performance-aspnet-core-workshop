using System.Collections.Generic;
using GettingThingsDone.Contracts.Dto;

namespace GettingThingsDone.Contracts.Interface
{
    public interface IProjectService
    {
        ServiceResult<ProjectDto> GetProject(int id);
        ServiceResult<List<ProjectDto>> GetAll();
        ServiceResult<ProjectDto> CreateOrUpdate(ProjectDto projectDto);
        ServiceResult<List<ActionDto>> GetProjectActions(int id);
        ServiceResult Delete(int id);
    }
}