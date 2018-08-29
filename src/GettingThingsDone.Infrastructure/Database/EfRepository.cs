using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using GettingThingsDone.Contracts.Interface;
using GettingThingsDone.Contracts.Model;

namespace GettingThingsDone.Infrastructure.Database
{
    /// <summary>
    /// Entity Framework based implementation of the <see cref="IRepository{T}"/>.
    /// </summary>
    public class EfRepository<T> : IRepository<T> where T : Entity
    {
        protected GettingThingsDoneDbContext DbContext { get; }

        public EfRepository(GettingThingsDoneDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public T GetById(int id)
        {
            return DbContext.Set<T>().Find(id);
        }

        public T GetFirst(ISpecification<T> specification)
        {
            return GetAll(specification).FirstOrDefault();
        }

        public IEnumerable<T> GetAll(TrackingOption tracking = TrackingOption.WithTracking)
        {
            return DbContext.Set<T>().WithTrackingOption(tracking).AsEnumerable();
        }

        public IEnumerable<T> GetAll(ISpecification<T> specification)
        {
            // Fetch a queryable that includes all expression-based includes.
            var queryableResultWithIncludes = specification
                .Includes
                .Aggregate(DbContext.Set<T>().AsQueryable(), (current, include) => current.Include(include));

            // Modify the queryable to include any string-based include statements.
            var secondaryResult = specification
                .IncludesAsStrings
                .Aggregate(queryableResultWithIncludes, (current, include) => current.Include(include));

            // Return the result of the query using the specification's criteria expression.
            return secondaryResult
                .Where(specification.Criteria)
                .WithTrackingOption(specification.Tracking)
                .AsEnumerable();
        }

        public T AddOrUpdate(T entity)
        {
            if (entity.Id == default(int))
            {
                DbContext.Set<T>().Add(entity);
            }
            else
            {
                DbContext.Entry(entity).State = EntityState.Modified;
            }

            DbContext.SaveChanges();

            return entity;
        }

        public void Delete(T entity)
        {
            DbContext.Set<T>().Remove(entity);
            DbContext.SaveChanges();
        }
    }
}