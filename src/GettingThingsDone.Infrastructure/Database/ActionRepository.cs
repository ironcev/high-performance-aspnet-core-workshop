using GettingThingsDone.Contract.Dto;
using GettingThingsDone.Contract.Interface;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace GettingThingsDone.Infrastructure.Database
{
    public class ActionRepository : EfRepository<Contract.Model.Action>, IActionRepository
    {
        public ActionRepository(GettingThingsDoneDbContext dbContext) : base(dbContext)
        {
        }

        public Contract.Model.Action GetByIdWithLists(int id)
        {
            return DbContext.Actions
                .Include(o => o.List)
                .FirstOrDefault();
        }

        public List<Contract.Model.Action> GetTop(int count)
        {
            return DbContext.Actions
                .OrderByDescending(x => x.Id)
                .Take(count)
                .ToList();
        }

        public DataActionResultDto MoveToList(int id, int listId)
        {
            DataActionResultDto result = new DataActionResultDto();
            result.IsSaved = false;
            var action = DbContext.Actions.Find(id);
            if (action == null)
            {
                result.Message = "Action not found.";
                return result;
            }
                

            var list = DbContext.Lists.Find(listId);
            if (list == null)
            {
                result.Message = "List not found.";
                return result;
            }

            action.List = list;

            DbContext.Actions.Update(action);
            DbContext.SaveChanges();

            result.IsSaved = true;

            return result;
        }

        public DataActionResultDto AssignToProject(int id, int projectId)
        {
            DataActionResultDto result = new DataActionResultDto();
            var action = DbContext.Actions.Find(id);
            if (action == null)
            {
                result.Message = "Action not found.";
                return result;
            }

            var project = DbContext.Projects.Find(projectId);
            if (project == null)
            {
                result.Message = "Project not found.";
                return result;
            }

            action.Project = project;

            DbContext.Actions.Update(action);
            DbContext.SaveChanges();

            result.IsSaved = true;

            return result;
        }
    }
}
