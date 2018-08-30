namespace GettingThingsDone.WebApi.Models
{
    /// <summary>
    /// Information needed for the authentication request
    /// for getting the token.
    /// </summary>
    public class TokenRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}