using System.Collections.Generic;
using System.Linq;

namespace GettingThingsDone.Contracts.Interface
{
    /// <summary>
    /// Represent a service result that returns an additional value of type <typeparamref name="TValue"/>.
    /// By convention, if the status is not <see cref="ServiceCallStatus.Ok"/> the <see cref="Value"/>
    /// should be the default value of <typeparamref name="TValue"/>.
    /// </summary>
    public sealed class ServiceResult<TValue> : IServiceResult
    {
        public TValue Value { get; }
        public IEnumerable<string> Messages { get; }
        public ServiceCallStatus Status { get; }

        internal ServiceResult(TValue value, ServiceCallStatus status, string message)
            : this(value, status, new [] { message })
        {
        }

        internal ServiceResult(TValue value, ServiceCallStatus status, IEnumerable<string> messages = null)
        {
            Value = value;
            Status = status;
            Messages = messages ?? Enumerable.Empty<string>();
        }
    }

    /// <summary>
    /// Represent a service result that does not return any additional value.
    /// </summary>
    public sealed class ServiceResult : IServiceResult
    {
        public IEnumerable<string> Messages { get; }
        public ServiceCallStatus Status { get; }

        private ServiceResult(ServiceCallStatus status, string message)
            :this(status, new [] { message })
        {
        }

        private ServiceResult(ServiceCallStatus status, IEnumerable<string> messages = null)
        {
            Status = status;
            Messages = messages ?? Enumerable.Empty<string>();
        }

        private static readonly ServiceResult OkSingleton = new ServiceResult(ServiceCallStatus.Ok);
        public static ServiceResult Ok()
        {
            return OkSingleton;
        }

        public static ServiceResult<TResult> Ok<TResult>(TResult result)
        {
            return new ServiceResult<TResult>(result, ServiceCallStatus.Ok);
        }

        public static ServiceResult EntityNotFound()
        {
            return new ServiceResult(ServiceCallStatus.EntityNotFound);
        }

        public static ServiceResult EntityNotFound(string errorMessage)
        {
            return new ServiceResult(ServiceCallStatus.EntityNotFound, errorMessage);
        }

        public static ServiceResult<TResult> EntityNotFound<TResult>()
        {
            return new ServiceResult<TResult>(default, ServiceCallStatus.EntityNotFound);
        }

        public static ServiceResult<TResult> EntityNotFound<TResult>(TResult result)
        {
            return new ServiceResult<TResult>(result, ServiceCallStatus.EntityNotFound);
        }

        public static ServiceResult<TResult> EntityNotFound<TResult>(string errorMessage)
        {
            return new ServiceResult<TResult>(default, ServiceCallStatus.EntityNotFound, errorMessage);
        }

        public static ServiceResult UnauthorizedAccess(string errorMessage)
        {
            return new ServiceResult(ServiceCallStatus.UnauthorizedAccess, errorMessage);
        }

        public static ServiceResult<TResult> UnauthorizedAccess<TResult>(string errorMessage)
        {
            return new ServiceResult<TResult>(default, ServiceCallStatus.UnauthorizedAccess, errorMessage);
        }

        public static ServiceResult InvalidOperation(string errorMessage)
        {
            return new ServiceResult(ServiceCallStatus.InvalidOperation, errorMessage);
        }

        public static ServiceResult<TResult> InvalidOperation<TResult>(string errorMessage)
        {
            return new ServiceResult<TResult>(default, ServiceCallStatus.InvalidOperation, errorMessage);
        }

        public static ServiceResult InvalidEntity(string errorMessage)
        {
            return new ServiceResult(ServiceCallStatus.InvalidEntity, errorMessage);
        }

        public static ServiceResult<TResult> InvalidEntity<TResult>(string errorMessage)
        {
            return new ServiceResult<TResult>(default, ServiceCallStatus.InvalidEntity, errorMessage);
        }

        public static ServiceResult<TResult> InvalidEntity<TResult>(IEnumerable<string> errorMessages)
        {
            return new ServiceResult<TResult>(default, ServiceCallStatus.InvalidEntity, errorMessages);
        }
    }
}