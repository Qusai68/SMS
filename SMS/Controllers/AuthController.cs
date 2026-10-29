using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SMS.DbContexts;
using SMS.Dtos.Auth;
using SMS.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly SchoolContext _dbcontext;
        public AuthController(SchoolContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto loginDto)
        {
            try
            {
                var user = _dbcontext.Users.FirstOrDefault(u => u.Username.ToUpper() == loginDto.Username.ToUpper());
                if (user == null)
                {
                    return BadRequest("Invalid username or password");
                }
                if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.HashedPassword))
                {
                    return BadRequest("Invalid username or password");
                }

                var token = GenerateJwtToken(user);

                return Ok(token);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    

    private string GenerateJwtToken(User user)
        {
            var claims = new List<Claim>(); // user information

            //key -> value
            //Id --> 1
            //Username --> admin
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            claims.Add(new Claim(ClaimTypes.Name, user.Username));
            if (user.IsAdmin)
            {
                claims.Add(new Claim(ClaimTypes.Role, "Admin"));
            }
            else
            {
                var teacher = _dbcontext.Teacher.Include(t => t.Position).FirstOrDefault(t => t.UserId == user.Id);
                claims.Add(new Claim(ClaimTypes.Role, teacher.Position.Name));
            }
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("WHAFWEI#!@S!!112312WQEQW@RWQEQW432"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var tokenSetting = new JwtSecurityToken(
                claims: claims,//user information
                expires: DateTime.Now.AddDays(1),//token expiration time
                signingCredentials: creds//secret key
                );
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.WriteToken(tokenSetting);
            return token;



        }

      }
   }
