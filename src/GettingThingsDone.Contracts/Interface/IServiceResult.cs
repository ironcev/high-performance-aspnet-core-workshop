using System.Collections.Generic;

namespace GettingThingsDone.Contracts.Interface
{
    /// <summary>
    /// Represent a rich result that comes from a service call.
    /// The result contains the call status and an arbitrary number of messages.
    /// </summary>
    public interface IServiceResult
    {
        IEnumerable<string> Messages { get; }
        ServiceCallStatus Status { get; }
    }
}
