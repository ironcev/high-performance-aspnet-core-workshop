namespace GettingThingsDone.Contracts.Interface
{
    /// <summary>
    /// Status of a service call.
    /// </summary>
    public enum ServiceCallStatus
    {
        /// <summary>
        /// The call completed successfully.
        /// </summary>
        Ok,
        /// <summary>
        /// One or more needed entities were not found.
        /// </summary>
        EntityNotFound,
        /// <summary>
        /// The client does not have sufficient rights.
        /// </summary>
        UnauthorizedAccess,
        /// <summary>
        /// The client calls the service in a non-defined way.
        /// </summary>
        InvalidOperation,
        /// <summary>
        /// A validation of an entity failed.
        /// </summary>
        InvalidEntity
    }
}