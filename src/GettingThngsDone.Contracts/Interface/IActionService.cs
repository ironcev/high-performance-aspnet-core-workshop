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
        bool Delete(int id);
        bool MoveToList(int id, int listId);
        bool AssignToProject(int id, int projectId);

    }
}
