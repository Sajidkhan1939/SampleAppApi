using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SampleAppApi.Migrations;
using SampleAppApi.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SampleAppApi.Controllers
{
    //Test 1
    [Route("api/[controller]")]
    [ApiController]
    public class UserTokenController : ControllerBase
    {
        private DatabaseContext _dbcontext;
        private Jwtsettings jwtSettings;

        public UserTokenController(DatabaseContext dbcontext,IOptions<Jwtsettings> options)
        {
            _dbcontext = dbcontext;
            this.jwtSettings = options.Value;
        }
        [Route("token")]
        [HttpPost]
        public async Task<IActionResult> Token([FromBody] AuthUser authuser)
        {
            try
            {
                var usercred = await this._dbcontext.Users
                    .Include(u => u.Role)
                    .FirstOrDefaultAsync(item => item.Email == authuser.UserName && item.Password == authuser.Password);

                if (usercred == null)
                {
                    return Ok("Email or Password is invalid");
                }

                var tokenhandler = new JwtSecurityTokenHandler();
                var tokenkey = Encoding.UTF8.GetBytes(this.jwtSettings.Key);

                var tokendesc = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                new Claim(ClaimTypes.Name, usercred.Email),
                usercred.Role != null ? new Claim(ClaimTypes.Role, usercred.Role.Name) : null
                    }),
                    Expires = DateTime.Now.AddDays(20),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenkey), SecurityAlgorithms.HmacSha256),
                };

                var token = tokenhandler.CreateToken(tokendesc);
                var finaltoken = tokenhandler.WriteToken(token);
                return Ok(new {token= finaltoken});
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
