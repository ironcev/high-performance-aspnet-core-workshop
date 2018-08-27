using System;
using System.Collections.Generic;
using System.Text;

namespace GettingThingsDone.Contract.Interface
{
    public interface IActionRepository : IRepository<GettingThingsDone.Contract.Model.Action>
    {
        GettingThingsDone.Contract.Model.Action GetByIdWithLists(int id);

        List<GettingThingsDone.Contract.Model.Action> GetTop(int count);

        bool MoveToList(int id, int listId);

        bool AssignToProject(int id, int projectId);
    }
}
