using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using GettingThingsDone.Contracts.Interface;
using GettingThingsDone.Contracts.Model;

namespace GettingThingsDone.ApplicationCore.Helpers
{
    public static class RepositoryExtensions
    {
        public static IEnumerable<T> GetAll<T>(this IRepository<T> repository, Expression<Func<T, bool>> criteria)
            where T: Entity
        {
            return repository.GetAll(new Specification<T>(criteria));
        }

        public static T GetByIdAndInclude<T>(this IRepository<T> repository, int id, Expression<Func<T, object>> include)
            where T : Entity
        {
            return repository.GetAll(new Specification<T>(
                t => t.Id == id,
                new [] { include }
            ))
            .FirstOrDefault();
        }
    }
}