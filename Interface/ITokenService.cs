using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinShark.Model;

namespace FinShark.Interface
{
    public interface ITokenService
    {
        public string CreateToken(AppUser user);
    }
}