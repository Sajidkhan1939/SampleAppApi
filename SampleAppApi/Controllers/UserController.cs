using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SampleAppApi.Model;

namespace SampleAppApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,User")]
    public class UserController : ControllerBase
    {
        private DatabaseContext _dbcontext;
        public UserController(DatabaseContext dbcontext)
        {
            _dbcontext= dbcontext;  
        }
        [Route("GetUsers")]
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async  Task<IActionResult> GetUser()
        {
           //var roles= _dbcontext.Roles.ToList();
            var users = await _dbcontext.Users.ToListAsync();
            List<User> userList = users.Select(item => new User
            {
                Id = item.Id,
                Name = item.Name,
                LastName = item.LastName,
                Email = item.Email,
                City = item.City,
                Country = item.Country,
                Phone = item.Phone
            }).ToList();
            return Ok(userList);
        }
        [Route("GetUser/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var user = await _dbcontext.Users.FindAsync(id);
            return Ok(user);
        }
        [Route("Register")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody]User data)
        {
     
            var users = new User()
            {
                Id = Guid.NewGuid(),
                Name = data.Name,
                LastName = data.LastName,
                Email = data.Email,
                Password = data.Password,
                Phone = data.Phone,
                City = data.City,
                Country = data.Country,
                CreatedDateTime = data.CreatedDateTime,
          
            };
            if (data.RoleId != null && data.RoleId==Guid.Empty)
            {
                users.RoleId = data.RoleId;
            }
            else
            {
                users.RoleId = Guid.Parse("3F0E2626-72BE-4752-A078-22E32C10AD64");
            }

            var emailExist = _dbcontext.Users.Any(x => x.Email == data.Email);
            if (emailExist)
            {
                return Ok("email already exist");
            }
            else
            {
                await _dbcontext.Users.AddAsync(users);
                await _dbcontext.SaveChangesAsync();
                return Ok("registered successfully");
            }

        }
        [Route("UpdateUser/{id}")]
        [HttpPut]
        public async Task<IActionResult> EditUser(Guid id,[FromBody]User editUser)
        {
            try
            {
                var existingUser = await _dbcontext.Users.FindAsync(id);
                if (existingUser == null)
                {
                    return NotFound();
                }
                if (editUser != null)
                {
                    existingUser.Name = editUser.Name;
                    existingUser.LastName = editUser.LastName;
                    existingUser.Email = editUser.Email;
                    existingUser.Password = editUser.Password;
                    existingUser.Phone = editUser.Phone;
                    existingUser.City = editUser.City;
                    existingUser.Country = editUser.Country;

                }
                await _dbcontext.SaveChangesAsync();
                return Ok(existingUser);

                //if(entity == null)
                //{
                //    return BadRequest();
                //}
                //if (id.Equals(userupdate.Id))
                //{

                //}
                //   entity.Name = userupdate.Name; 
                //   entity.LastName = userupdate.LastName;
                //   entity.Email = userupdate.Email;
                //   entity.Password = userupdate.Password;
                //   entity.Phone = userupdate.Phone;
                //   entity.City = userupdate.City;
                //   entity.Country = userupdate.Country;
                //_dbcontext.Users.Update(entity);
                //await _dbcontext.SaveChangesAsync();
                //return Ok("user updated");

            }
            catch (Exception)
            {

                throw;
            }
        }
        [Route("Login")]
        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return Ok("please add valid email");
            }
            else if (string.IsNullOrWhiteSpace(password))
            {
                return Ok("enter valid password");
            }
            else
            {
                //var login = _dbcontext.Users.FindAsync(user.Email , user.Password);
                var Email= await _dbcontext.Users.FirstOrDefaultAsync(x => x.Email == email);
                var Password    = await _dbcontext.Users.FirstOrDefaultAsync(x=>x.Password == password);
                if(Email!=null && Password!=null)
                {
                    return Ok("login succesfully");
                }
                else
                {
                    return Ok("email or password is not correct");
                }
            }
        }

    }
}
