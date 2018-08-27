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

        public bool MoveToList(int id, int listId)
        {
            var action = _dbContext.Actions.Find(id);
            if (action == null)
                return false;

            var list = _dbContext.Lists.Find(listId);
            if (list == null)
                return false;

            action.List = list;

            _dbContext.Actions.Update(action);
            _dbContext.SaveChanges();

            return true;
        }

        public bool AssignToProject(int id, int projectId)
        {
            var action = _dbContext.Actions.Find(id);
            if (action == null)
                return false;

            var project = _dbContext.Projects.Find(projectId);
            if (project == null)
                return false;

            action.Project = project;

            _dbContext.Actions.Update(action);
            _dbContext.SaveChanges();

            return true;
        }
    }
}
