using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ticketsystem_backend.Data;
using ticketsystem_backend.Models;
using Microsoft.EntityFrameworkCore;


namespace ticketsystem_backend.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly TicketSystemDbContext _context;

        public AuthController(TicketSystemDbContext context)
        {
            _context = context;
        }

        [HttpPost, Route("login")]
        public IActionResult Login([FromBody]LoginModel user)
        {
            if (user == null)
            {
                return BadRequest("Invalid client request");
            }

            if (UserValidate(user.UserName, user.Password))
            {
                var secretKey = new SymmetricSecurityKey(Base64UrlEncoder.DecodeBytes("superSecretKey@345"));
                //var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"));
                var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Role, GetUserRole(user.UserName).Name)
                };

                var tokenOptions = new JwtSecurityToken(
                    issuer: "https://localhost:5001",
                    audience: "https://localhost:5001",
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(60),
                    signingCredentials: signingCredentials
                    );

                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
                return Ok(new { Token = tokenString });
            }

            return Unauthorized();
        }

        private Role GetUserRole(string userName)
        {
            User user = _context.Users.Include(u => u.Role).Where(u => u.UserName == userName).FirstOrDefault();
            Role role = _context.Roles.Where(r => r == user.Role).FirstOrDefault();
            return role;
        }

        private bool UserValidate(string userName, string password)
        {
            User user = _context.Users.Where(u => u.UserName == userName).FirstOrDefault();

            if (user == null)
            {
                return false;
            }

            return user.Password == password;
        }
    }
}
