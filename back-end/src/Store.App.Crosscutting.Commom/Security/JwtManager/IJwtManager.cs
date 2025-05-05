using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.App.Crosscutting.Commom.Security.JwtManager
{
    public interface IJwtManager
    {
        string GenerateToken(User user);
        User GetUserFromToken(string token);
        User ObterUsuarioLogado();
        string GetTokenFromHeader();
    }
}
