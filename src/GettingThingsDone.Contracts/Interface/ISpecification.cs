using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace GettingThingsDone.Contract.Interface
{
    /// <summary>
    /// A specification of a query over a <see cref="IRepository{T}"/>.
    /// </summary>
    public interface ISpecification<T>
    {
        /// <summary>
        /// The query criteria. E.g.
        /// action => action.Title.StartsWith("Workshop") && action.DueDate == someDate
        /// </summary>
        Expression<Func<T, bool>> Criteria { get; }
        /// <summary>
        /// The list of entity properties of <see cref="T"/> that
        /// should be included in the result. E.g.
        /// x => x.Project
        /// </summary>
        List<Expression<Func<T, object>>> Includes { get; }
        /// <summary>
        /// The list of entity properties of <see cref="T"/> that
        /// should be included in the result represented as strings. E.g.
        /// "Project"
        /// </summary>
        List<string> IncludesAsStrings { get; }
    }
}