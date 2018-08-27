using GettingThingsDone.Contract.DTO;
using GettingThingsDone.Contract.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GettingThingsDone.Infrastructure.Database
{
    public class ActionRepository : EfRepository<Contract.Model.Action>, IActionRepository
    {
        public ActionRepository(GettingThingsDoneDbContext dbContext) : base(dbContext)
        {
        }

        public Contract.Model.Action GetByIdWithLists(int id)
        {
            return _dbContext.Actions
                .Include(o => o.List)
                .FirstOrDefault();
        }

        public List<Contract.Model.Action> GetTop(int count)
        {
            return _dbContext.Actions
                .OrderByDescending(x => x.Id)
                .Take(count)
                .ToList();
        }

        public DataActionResultDTO MoveToList(int id, int listId)
        {
            DataActionResultDTO result = new DataActionResultDTO();
            result.IsSaved = false;
            var action = _dbContext.Actions.Find(id);
            if (action == null)
            {
                result.Message = "Action not found!";
                return result;
            }
                

            var list = _dbContext.Lists.Find(listId);
            if (list == null)
            {
                result.Message = "List not found!";
                return result;
            }

            action.List = list;

            _dbContext.Actions.Update(action);
            _dbContext.SaveChanges();

            result.IsSaved = true;

            return result;
        }

        public DataActionResultDTO AssignToProject(int id, int projectId)
        {
            DataActionResultDTO result = new DataActionResultDTO();
            var action = _dbContext.Actions.Find(id);
            if (action == null)
            {
                result.Message = "Action not found!";
                return result;
            }

            var project = _dbContext.Projects.Find(projectId);
            if (project == null)
            {
                result.Message = "Project not found!";
                return result;
            }

            action.Project = project;

            _dbContext.Actions.Update(action);
            _dbContext.SaveChanges();

            result.IsSaved = true;

            return result;
        }
    }
}
