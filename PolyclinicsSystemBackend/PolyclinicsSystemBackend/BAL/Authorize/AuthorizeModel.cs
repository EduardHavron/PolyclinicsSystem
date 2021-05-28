using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace PolyclinicsSystemBackend.BAL.Authorize
{
    public class AuthorizeModel
    {
        public string? Token { get; set; }
        
        public IEnumerable<IdentityError>? Errors { get; set; }
        
        public bool Succeeded { get; set; }
    }
}