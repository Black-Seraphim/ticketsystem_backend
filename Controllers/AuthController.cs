using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using ticketsystem_backend.Data;
using ticketsystem_backend.Models;

namespace ticketsystem_backend.Controllers
{
    /// <summary>
    /// AuthController provide actions for login procedure.
    /// </summary>
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly TicketSystemDbContext _context;

        public AuthController(TicketSystemDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Login-Action that sends a token if the send user credentials are valid.
        /// The Token is needed for each other API-Controller, that needs authorization
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [Route("login")]
        [HttpPost]
        public IActionResult Login([FromBody]LoginModel user)
        {
            // check if user exist
            if (user == null)
            {
                return BadRequest("Invalid client request. Empty user model");
            }

            // check if user is valid
            if (string.IsNullOrEmpty(user.UserName) || string.IsNullOrEmpty(user.Password))
            {
                return BadRequest("Invalid client request. User or Password empty");
            }

            // create and send token if user is valid
            if (UserValidate(user.UserName, user.Password))
            {
                SymmetricSecurityKey secretKey = new(Base64UrlEncoder.DecodeBytes("MBcCT4UEs67vh3shK683Lxhn33t2LTtH"));
                SigningCredentials signingCredentials = new(secretKey, SecurityAlgorithms.HmacSha256);

                // get original user data out of the database
                User fullUser = _context.Users
                    .Where(u => u.UserName == user.UserName)
                    .Include(u => u.Role)
                    .FirstOrDefault();
                Role role = _context.Roles
                    .Where(r => r == fullUser.Role)
                    .FirstOrDefault();

                // add userName and userRole to claim
                List<Claim> claims = new()
                {
                    new Claim(ClaimTypes.Name, fullUser.UserName),
                    new Claim(ClaimTypes.Role, role.Name)
                };

                // create token
                JwtSecurityToken tokenOptions = new(
                    issuer: "*",
                    audience: "*",
                    claims: claims,
                    expires: DateTime.Now.AddHours(12),
                    signingCredentials: signingCredentials                    
                    );

                string tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
                return Ok(new { Token = tokenString });
            }

            // return unauthorized if user could not be validated
            return Unauthorized();
        }

        /// <summary>
        /// Returns true, if user exist and password matches
        /// </summary>
        /// <param name="userName">Username as Email</param>
        /// <param name="password">User-Password</param>
        /// <returns></returns>
        private bool UserValidate(string userName, string password)
        {
            User user = _context.Users
                .Where(u => u.UserName == userName)
                .FirstOrDefault();
            
            if (user == null)
            {
                return false;
            }

            return user.Password == password;
        }
    }
}
