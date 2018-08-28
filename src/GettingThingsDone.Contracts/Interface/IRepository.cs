using System.Collections.Generic;
using GettingThingsDone.Contracts.Model;

namespace GettingThingsDone.Contracts.Interface
{
    /// <summary>
    /// A generic repository of entities of type <typeparamref name="T"/>.
    /// </summary>
    /// <remarks>
    /// To learn more about the Repository Pattern see:
    /// https://martinfowler.com/eaaCatalog/repository.html
    /// </remarks>
    public interface IRepository<T> where T : Entity
    {
        /// <summary>
        /// Gets an entity by id or null if such entity does not exist.
        /// </summary>
        T GetById(int id);
        /// <summary>
        /// Gets the first entity that satisfies the <paramref name="specification"/>
        /// or null if such entity does not exist.
        /// </summary>
        T GetFirst(ISpecification<T> specification);
        /// <summary>
        /// Gets all entities or empty enumerable if the repository does not contain
        /// any entity.
        /// </summary>
        IEnumerable<T> GetAll();
        /// <summary>
        /// Gets all entities that satisfy the <paramref name="specification"/>
        /// or empty enumerable if such entities do not exist.
        /// </summary>
        IEnumerable<T> GetAll(ISpecification<T> specification);
        /// <summary>
        /// Adds a new or updates an existing <paramref name="entity"/> in the repository.
        /// </summary>
        T AddOrUpdate(T entity);
        /// <summary>
        /// Deletes an existing <paramref name="entity"/> form the repository.
        /// </summary>
        void Delete(T entity);
    }
}