using System;
using System.Collections.Generic;
using GettingThingsDone.Contracts.Interface;
using Microsoft.AspNetCore.Mvc;
using Action = GettingThingsDone.Contracts.Model.Action;

namespace GettingThingsDone.WebApi.Controllers
{
    public abstract class BaseController : Controller
    {
        /// <summary>
        /// Maps <see cref="ServiceResult{TValue}"/> to <see cref="ActionResult{TValue}"/>
        /// with suitable HTTP error codes.
        /// </summary>
        protected ActionResult<TValue> FromValueServiceResult<TValue>(ServiceResult<TValue> result)
        {
            switch (result.Status)
            {
                case ServiceCallStatus.Ok:
                    return Ok(result.Value);
                case ServiceCallStatus.EntityNotFound:
                    return NotFound(result.Messages);
                case ServiceCallStatus.UnauthorizedAccess:
                    return Unauthorized();
                case ServiceCallStatus.InvalidOperation:
                    return BadRequest(result.Messages);
                case ServiceCallStatus.InvalidEntity:
                    return BadRequest(result.Messages);
                default: throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Maps <see cref="IServiceResult"/> to <see cref="IActionResult"/>
        /// with suitable HTTP error codes.
        /// </summary>
        protected IActionResult FromServiceResult(IServiceResult result)
        {
            switch (result.Status)
            {
                case ServiceCallStatus.Ok:
                    return Ok();
                case ServiceCallStatus.EntityNotFound:
                    return NotFound(result.Messages);
                case ServiceCallStatus.UnauthorizedAccess:
                    return Unauthorized();
                case ServiceCallStatus.InvalidOperation:
                    return BadRequest(result.Messages);
                case ServiceCallStatus.InvalidEntity:
                    return BadRequest(result.Messages);
                default: throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Sets the List and Project property on all <paramref name="actions"/> to null.
        /// </summary>
        /// <returns>Actions with List and Project property set to null</returns>
        /// <remarks>
        /// We have to remove the List and Project object from the action object
        /// otherwise the JSON serializer is not able to properly
        /// serialize the result because of circular relationships.
        /// This is most likely not the best way to solve this issue
        /// but at this point in the workshop we simply do no know any
        /// better way :-(
        /// Stay tuned and watch this workaround disappear later.
        /// </remarks>
        protected IEnumerable<Action> RemoveListAndProjectFromActions(List<Action> actions)
        {
            foreach (var action in actions)
            {
                action.List = null;
                action.Project = null;
                yield return action;
            }
        }
    }
}