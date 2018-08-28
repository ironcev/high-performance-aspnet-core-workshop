namespace GettingThingsDone.Contract.Dto
{
    /// <summary>
    /// A base DTO object for all entities.
    /// </summary>
    /// <remarks>
    /// To learn more about Data Transfer Objects (DTOs) see:
    /// https://en.wikipedia.org/wiki/Data_transfer_object
    /// </remarks>
    public abstract class EntityDto
    {
        /// <summary>
        /// The Id of the existing entity or zero if the entity
        /// represented by this DTO still does not exist in the repository.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// True if the entity represented by this DTO still does not exist in the repository.
        /// </summary>
        public bool RepresentsNewEntity => Id == default(int);
    }
}