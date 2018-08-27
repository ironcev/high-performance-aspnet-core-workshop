using GettingThingsDone.Contract.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using Action = GettingThingsDone.Contract.Model.Action;

namespace GettingThingsDone.Contract.Interface
{
    public interface IActionService
    {
        ActionDTO GetAction(int id);
        List<ActionDTO> GetTop(int count);
        ActionDTO CreateOrUpdate(int? id, ActionDTO actionDto);
        DataActionResultDTO Delete(int id);
        DataActionResultDTO MoveToList(int id, int listId);
        DataActionResultDTO AssignToProject(int id, int projectId);

    }
}
