using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FinShark.Extension
{
    public static class ClaimExtension
    {
        public static string GetUsername(this ClaimsPrincipal user)
        {

            var username = user.Claims.FirstOrDefault(c => c.Type.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname")).Value;

            return username;

        }
    }
}
