using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace GettingThingsDone.Contract.Model
{
    /// <summary>
    /// Represents a GTD action list.
    /// </summary>
    public class ActionList : Entity
    {
        public ActionList()
        {
            Actions = new Collection<Action>();
        }

        /// <summary>
        /// List name. Name is mandatory.
        /// </summary>
        public string Name { get; set; }

        public ICollection<Action> Actions { get; set; }
    }
}