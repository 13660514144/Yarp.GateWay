using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Yarp.GateWay.Models;

namespace Yarp.GateWay.Identity
{
    public class AuthenticateService : IAuthenticateService
    {
        public readonly JWTmodels _jwtModel;
        public AuthenticateService(IOptions<JWTmodels> jwtModel)
        {
            _jwtModel = jwtModel.Value;
        }

        public bool IsAuthenticated(Users user, out string token)
        {
            token = string.Empty;

            var claims = new[] {
                new Claim(ClaimTypes.Name,user.UserName),               
                    new Claim("PWD",user.PWD),
                    new Claim("UserName",user.UserName)
            };
            //密钥
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtModel.Secret));
            //凭证
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            //生成Token
            var jwtToken = new JwtSecurityToken(
                issuer: _jwtModel.Issuer, audience: _jwtModel.Audience, claims: claims,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddMinutes(_jwtModel.AccessExpiration),
                signingCredentials: credentials);
            token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            return true;
            //throw new NotImplementedException();
        }
    }
}
