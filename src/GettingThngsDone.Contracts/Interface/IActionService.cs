using GettingThingsDone.Contract.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using Action = GettingThingsDone.Contract.Model.Action;

namespace GettingThingsDone.Contract.Interface
{
    public interface IActionService
    {
        GettingThingsDone.Contract.Model.Action GetAction(int id);
        List<GettingThingsDone.Contract.Model.Action> GetTop(int count);
        Contract.Model.Action CreateOrUpdate(int? id, GettingThingsDone.Contract.Model.Action action);
        DataActionResultDTO Delete(int id);
        DataActionResultDTO MoveToList(int id, int listId);
        DataActionResultDTO AssignToProject(int id, int projectId);

    }
}
