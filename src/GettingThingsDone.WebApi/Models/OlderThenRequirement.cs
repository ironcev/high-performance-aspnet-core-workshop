using GettingThingsDone.ApplicationCore.Helpers;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GettingThingsDone.WebApi.Models
{
    /// <summary>
    /// Older then requirement - authorization for some actions
    /// example: 18
    /// if 50 then it could be called MatoriRequirement ;-)
    /// </summary>
    public class OlderThenRequirement : IAuthorizationRequirement
    {
        public int MinimumYears { get; private set; }

        public OlderThenRequirement(int minimumYears)
        {
            this.MinimumYears = minimumYears;
        }
    }

    public class OlderThenHandler
        : AuthorizationHandler<OlderThenRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            OlderThenRequirement requirement)
        {
            var birthDateValue = context.User
                .FindFirst(claim => claim.Type == CustomClaimTypes.BirthDate).Value;

            var birthDate = Convert.ToDateTime(birthDateValue);

            var yearOld = DateTime.Now.Year - birthDate.Year;

            if (yearOld >= requirement.MinimumYears)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }

   

    public static class CustomClaimTypes
    {
        public const string BirthDate = "BirthDate";
        public const string OwnerProjectId = "OwnerProjectId";
        public const string Editor = "Editor";
    }
}
