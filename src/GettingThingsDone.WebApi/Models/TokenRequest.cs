using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GettingThingsDone.WebApi.Models
{
    /// <summary>
    /// Authentication request for getting token
    /// </summary>
    public class TokenRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
