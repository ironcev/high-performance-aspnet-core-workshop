using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GettingThingsDone.Contracts.Interface;
using GettingThingsDone.Contracts.Model;

namespace GettingThingsDone.Infrastructure.Database
{
    /// <summary>
    /// Entity Framework based implementation of the <see cref="IAsyncRepository{T}"/>.
    /// </summary>
    public class EfAsyncRepository<T> : IAsyncRepository<T> where T : Entity
    {
        protected GettingThingsDoneDbContext DbContext { get; }

        public EfAsyncRepository(GettingThingsDoneDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task<T> GetById(int id)
        {
            return await DbContext.Set<T>().FindAsync(id);
        }

        public async Task<T> GetFirst(ISpecification<T> specification)
        {
            return await GetQueryableForSpecification(specification).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> GetAll(TrackingOption tracking = TrackingOption.WithTracking)
        {
            return await DbContext.Set<T>().WithTrackingOption(tracking).ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAll(ISpecification<T> specification)
        {
            return await GetQueryableForSpecification(specification).ToListAsync();
        }

        private IQueryable<T> GetQueryableForSpecification(ISpecification<T> specification)
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
                .WithTrackingOption(specification.Tracking);
        }

        public async Task<T> AddOrUpdate(T entity)
        {
            if (entity.Id == default(int))
            {
                DbContext.Set<T>().Add(entity);
            }
            else
            {
                DbContext.Entry(entity).State = EntityState.Modified;
            }

            await DbContext.SaveChangesAsync();

            return entity;
        }

        public async Task Delete(T entity)
        {
            DbContext.Set<T>().Remove(entity);
            await DbContext.SaveChangesAsync();
        }
    }
}