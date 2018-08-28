using System.Collections.Generic;
using System.Linq;
using GettingThingsDone.Contracts.Model;
using GettingThingsDone.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GettingThingsDone.WebApi.Controllers
{
    [Route("api/lists")]
    [ApiController]
    public class ActionListController : BaseController
    {
        private static class Routes
        {
            public const string GetList = nameof(GetList);
        }

        private readonly GettingThingsDoneDbContext _dbContext;

        public ActionListController(GettingThingsDoneDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ActionList>> GetAll()
        {
            return _dbContext.Lists;
        }

        [HttpGet("{id}", Name = Routes.GetList)]
        public ActionResult<ActionList> GetById(int id)
        {
            var list = _dbContext.Lists.Find(id);
            if (list == null)
                return NotFound();
            return list;
        }

        [HttpPost]
        public IActionResult Create([FromBody] ActionList value)
        {
            _dbContext.Lists.Add(value);
            _dbContext.SaveChanges();

            return CreatedAtRoute(Routes.GetList, new { id = value.Id }, value);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] ActionList value)
        {
            var list = _dbContext.Lists.Find(id);
            if (list == null)
                return NotFound();

            list.Name = value.Name;

            _dbContext.Lists.Update(list);
            _dbContext.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var list = _dbContext.Lists.Find(id);
            if (list == null)
            {
                return NotFound();
            }

            _dbContext.Lists.Remove(list);
            _dbContext.SaveChanges();

            return NoContent();
        }

        [HttpGet("{id}/actions")]
        public ActionResult<IEnumerable<Action>> GetListActions(int id)
        {
            var list = _dbContext.Lists.Include(x => x.Actions).FirstOrDefault(l => l.Id == id);
            if (list == null)
                return NotFound();
            var actionsToReturn = RemoveListAndProjectFromActions(list.Actions.ToList());
            return new ActionResult<IEnumerable<Action>>(actionsToReturn);
        }
    }
}