using GettingThingsDone.Contract.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace GettingThingsDone.Contract.Interface
{
    public interface IActionRepository : IRepository<GettingThingsDone.Contract.Model.Action>
    {
        GettingThingsDone.Contract.Model.Action GetByIdWithLists(int id);

        List<GettingThingsDone.Contract.Model.Action> GetTop(int count);

        DataActionResultDTO MoveToList(int id, int listId);

        DataActionResultDTO AssignToProject(int id, int projectId);
    }
}
