using System.Collections.Generic;
using GettingThingsDone.Contracts.Dto;

namespace GettingThingsDone.Contracts.Interface
{
    public interface IActionService
    {
        ServiceResult<ActionDto> GetAction(int id);
        ServiceResult<List<ActionDto>> GetAll();
        ServiceResult<ActionDto> CreateOrUpdate(ActionDto actionDto);
        ServiceResult Delete(int id);
        ServiceResult MoveToList(int id, int listId);
        ServiceResult AssignToProject(int id, int projectId);
    }
}