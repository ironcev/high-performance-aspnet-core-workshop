using System;

namespace GettingThingsDone.Model
{
    /// <summary>
    /// Represents a GTD action.
    /// </summary>
    public class Action : Entity
    {
        /// <summary>
        /// Action title. Title is mandatory.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Optional due date.
        /// </summary>
        public DateTimeOffset? DueDate { get; set; }

        /// <summary>
        /// Optional date on which the action is planned to be done.
        /// It must be smaller then <see cref="DueDate"/>.
        /// </summary>
        public DateTimeOffset? DoOn { get; set; }

        /// <summary>
        /// The date on which action is done, or null if the action is still undone.
        /// </summary>
        public DateTimeOffset? DoneAt { get; set; }

        /// <summary>
        /// True if the action is done. A done action cannot be changed.
        /// </summary>
        public bool IsDone => DoneAt != null;
    }
}