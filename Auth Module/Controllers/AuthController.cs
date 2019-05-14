using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AuthModule.Models;
using AuthModule.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Auth_Module.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IDataService _data;
        private IConfiguration _config;
        
        public AuthController(IConfiguration config,IDataService dataService)
        {
            _data = dataService;
            _config = config;
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login([FromBody]User userLogin)
        {
            IActionResult response = Unauthorized();
            var user = authenticateUser(userLogin);
            if (user != null)
            {
                var tokenString = generateToken(userLogin);
                response = Ok(new { token = tokenString });
            }

            return response;
        }

        private string generateToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credential = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,user.userName)
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"], _config["Jwt:Issuer"], claims,
                expires:DateTime.Now.AddMinutes(120),
                signingCredentials:credential
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
         }

        private User authenticateUser(User userdetails)
        {
            User user=_data.isAuthenticated(userdetails);
            return user;
        }

    }
}