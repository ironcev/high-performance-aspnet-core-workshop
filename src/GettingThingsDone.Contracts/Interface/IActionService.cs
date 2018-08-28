using System.Collections.Generic;
using GettingThingsDone.Contracts.Dto;

namespace GettingThingsDone.Contracts.Interface
{
    public interface IActionService
    {
        ActionDto GetAction(int id);
        List<ActionDto> GetAll();
        ActionDto CreateOrUpdate(ActionDto actionDto);
        DataActionResultDto Delete(int id);
        DataActionResultDto MoveToList(int id, int listId);
        DataActionResultDto AssignToProject(int id, int projectId);
    }
}