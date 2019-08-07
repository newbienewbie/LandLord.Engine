using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace LandLord.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class TokenController : ControllerBase
    {
        private readonly IConfiguration _config;

        public TokenController(IConfiguration config)
        {
            this._config = config;
        }

        [Authorize]
        [HttpPost]
        public IActionResult Build()
        {
            var user = HttpContext.User;

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["GameHubJwtAuthentication:Key"]));
            var sign = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiredAt = DateTime.Now.AddMinutes(double.Parse(_config["GameHubJwtAuthentication:ExpireTime"]));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(user.Claims),
                Expires = expiredAt,
                Issuer = _config["GameHubJwtAuthentication:Issuer"],
                Audience = _config["GameHubJwtAuthentication:Audience"],
                SigningCredentials = sign,
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            var res = new {
                Id = user.Identity.Name,
                Username = user.Identity.Name,
                Token = tokenString,
            };
            return Ok(res);
        }
    }
}