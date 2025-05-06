using JWT;
using JWT.Algorithms;
using JWT.Exceptions;
using JWT.Serializers;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using Store.App.Crosscutting.Commom.Logging;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Store.App.Crosscutting.Commom.Security.JwtManager
{
    public class JwtManager : IJwtManager
    {
        IHttpContextAccessor _httpContextAccessor;
        public JwtManager(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable("KEY_API"));
            var teste = DateTime.UtcNow.AddSeconds(60);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Name.ToString()),
                    new Claim(ClaimTypes.Role, user.Role.ToString()),
                    new Claim(ClaimTypes.NameIdentifier, user.NameId.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(10),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

		public User GetUserFromToken(string token)
        {
            try
            {
                #region VALIDAR SE O TOKEN É VALIDO E NAO ESTA EXPIRADO
                IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
                IJsonSerializer serializer = new JsonNetSerializer();
                IDateTimeProvider provider = new UtcDateTimeProvider();
                IJwtValidator validator = new JwtValidator(serializer, provider);
                IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder, algorithm);
                var parseToken = decoder.DecodeToObject<User>(token, Environment.GetEnvironmentVariable("KEY_API"), verify: true);
                parseToken.EmailUsuarioLogado = parseToken.NameId;
                return parseToken;
                #endregion
            }
            catch (TokenExpiredException ex)
            {
                LogWriter.GenerateDefaultLog("Token expirado " + ex.Message + " " + ex.StackTrace);
                return null;
            }
            catch (SignatureVerificationException ex)
            {
                LogWriter.GenerateDefaultLog("Assintatura do Token inválida" + ex.Message + " " + ex.StackTrace);
                return null;
            }
        }

        public User ObterUsuarioLogado()
        {
            var token = GetTokenFromHeader();

            var usuarioJWT = GetUserFromToken(token);

            if (usuarioJWT == null)
            {
                throw new ValidationException("Usuario não reconhecido");
            }

            return usuarioJWT;
        }

        public string GetTokenFromHeader()
        {
            try
            {
                if (_httpContextAccessor?.HttpContext?.Request != null)
                {
                    var req = _httpContextAccessor.HttpContext.Request;
                    string bearerToken = req.Headers["Authorization"];

                    if (bearerToken != null)
                    {
                        var token = bearerToken.StartsWith("Bearer ") ? bearerToken.Substring(7) : bearerToken;
                        return token;
                    }
                }
            }
            catch
            {
                return null;
            }

            return null;
        }
    }
}
