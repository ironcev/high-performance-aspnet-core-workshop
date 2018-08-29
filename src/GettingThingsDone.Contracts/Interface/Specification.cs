using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace GettingThingsDone.Contracts.Interface
{
    public class Specification<T> : ISpecification<T>
    {
        public Expression<Func<T, bool>> Criteria { get; } 
        public IEnumerable<Expression<Func<T, object>>> Includes { get; }
        public IEnumerable<string> IncludesAsStrings { get; }

        public Specification(
            Expression<Func<T, bool>> criteria = null,
            IEnumerable<Expression<Func<T, object>>> includes = null,
            IEnumerable<string> includesAsStrings = null)
        {
            Criteria = criteria ?? (t => true);            
            Includes = includes ?? Enumerable.Empty<Expression<Func<T, object>>>();
            IncludesAsStrings = includesAsStrings ?? Enumerable.Empty<string>();
        }
    }
}