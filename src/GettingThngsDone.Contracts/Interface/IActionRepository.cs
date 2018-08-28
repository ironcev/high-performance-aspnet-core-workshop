using GettingThingsDone.Contract.Dto;
using System.Collections.Generic;

namespace GettingThingsDone.Contract.Interface
{
    public interface IActionRepository : IRepository<Model.Action>
    {
        Model.Action GetByIdWithLists(int id);

        List<Model.Action> GetTop(int count);

        DataActionResultDto MoveToList(int id, int listId);

        DataActionResultDto AssignToProject(int id, int projectId);
    }
}