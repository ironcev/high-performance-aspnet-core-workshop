using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using GettingThingsDone.Contracts.Model;

namespace GettingThingsDone.WebApi.Security
{
    public static class CustomClaimTypes
    {
        /// <summary>
        /// Represents user's date of birth.
        /// </summary>
        public const string DateOfBirth = nameof(DateOfBirth);

        /// <summary>
        /// Represent the Id of the <see cref="Project"/> that
        /// the user owns.
        /// </summary>
        public const string OwnerProjectId = nameof(OwnerProjectId);

        public static int? GetOwnerProjectId(this ClaimsPrincipal user)
        {
            var identity = (ClaimsIdentity) user.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            var claim = claims.FirstOrDefault(x => x.Type == OwnerProjectId);
            if (claim == null) return null;

            return Convert.ToInt32(claim.Value);
        }
    }
}