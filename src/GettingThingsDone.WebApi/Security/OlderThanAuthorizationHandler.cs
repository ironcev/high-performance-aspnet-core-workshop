using System;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace GettingThingsDone.WebApi.Security
{
    public class OlderThanAuthorizationHandler : AuthorizationHandler<OlderThanRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            OlderThanRequirement requirement)
        {
            var birthDateValue = context
                .User
                .FindFirst(claim => claim.Type == CustomClaimTypes.DateOfBirth)
                .Value;
            
            var birthDate = DateTime.ParseExact(birthDateValue, "u", CultureInfo.InvariantCulture);

            var age = DateTime.Now.Year - birthDate.Year;

            if (age >= requirement.MinimumAge)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}