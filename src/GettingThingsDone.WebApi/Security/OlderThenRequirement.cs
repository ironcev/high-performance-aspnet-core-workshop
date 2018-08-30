using Microsoft.AspNetCore.Authorization;

namespace GettingThingsDone.WebApi.Security
{
    /// <summary>
    /// Older than authorization requirement specifies that
    /// for certain action, a certain <see cref="MinimumAge"/>
    /// is required from the user.
    /// </summary>
    /// <remarks>
    /// If the <see cref="MinimumAge"/> is above 50 we could actually
    /// call it MatoriRequirement :-) (A Croatian joke, sorry :-)).
    /// </remarks>
    public class OlderThanRequirement : IAuthorizationRequirement
    {
        public int MinimumAge { get; }

        public OlderThanRequirement(int minimumAge)
        {
            MinimumAge = minimumAge;
        }
    }
}
