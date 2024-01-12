using AutoMapper;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AuthentifyCore.DTOs;
using AuthentifyCore.Models;
using AuthentifyCore.Utils;

namespace AuthentifyCore.Controllers
{    
    [ApiController]
    [Route("api/v1/user")]
    public class UserController: ControllerBase
    {
        private readonly DevelopContext context;
        private readonly IMapper mapper;

        public UserController(DevelopContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<User>>> GetUsers(string search)
        {
            try
            {
                if (!String.IsNullOrEmpty(search))
                {
                    return await context.Users.Where(s => s.Name.ToUpper().Contains(search.ToUpper())
                                || s.Lastname.ToUpper().Contains(search.ToUpper()) || s.Email.ToUpper().Contains(search.ToUpper())).ToListAsync();
                }

                return await context.Users.ToListAsync();
                    
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
                
            }
        }

        // Get by id
        [HttpGet("{id:int}")]
        public async Task<ActionResult<User>> GetUserById(int id)
        {
            try
            {
                var user = await context.Users.FirstOrDefaultAsync(x => x.Id == id);

                if (user == null)
                {
                    return NotFound("User not found");
                }

                return user;
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateUserDTO userDto)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    var userExists = await context.Users.AnyAsync(x => x.Email == userDto.Email);

                    if (userExists)
                    {
                        return BadRequest(new ResponseStatusCode
                        {
                            Success = false,
                            Data = null,
                            Message = "This email is already registered"
                        });
                    }

                    userDto.Password = PasswordHash.HashSha256(userDto.Password);
                    userDto.Active = 1;                    

                    var user = mapper.Map<User>(userDto);

                    context.Add(user);
                    await context.SaveChangesAsync();
                    return Ok("User created successfully");
                }                

                return BadRequest("Cannot create user");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
                
            }
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateUser(User user, int id)
        {            
            try
            {              
                
                var entity = context.Users.FirstOrDefault(x => x.Id == id);

                if (entity == null)
                {
                    return NotFound( new ResponseStatusCode
                    {
                        Success = false,
                        Data = null,
                        Message = "User not found"
                    });
                }

                // Si se actualiza la contraseña
                if (user.Password != entity.Password)
                {
                    user.Password = PasswordHash.HashSha256(user.Password);                    
                }
                
                entity.Username = user.Username;
                entity.Password = user.Password;
                entity.Name = user.Name;
                entity.Lastname = user.Lastname;
                entity.Email = user.Email;
                entity.Active = user.Active;

                context.Update(entity);
                await context.SaveChangesAsync();
                return Ok("User updated successfully");
                
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
                
            }
        }

        [HttpPatch("{id:int}")]
        public async Task<ActionResult> PatchUser(int id, JsonPatchDocument<PatchUserDTO> patchDocument)
        {
            try
            {
                if (patchDocument == null)
                {
                    return BadRequest();
                }

                var userDB = await context.Users.FirstOrDefaultAsync(x => x.Id == id);

                if (userDB == null)
                {
                    return NotFound(new ResponseStatusCode
                    {
                        Success = false,
                        Data = null,
                        Message = "User not found"
                    });
                }                

                var user = mapper.Map<PatchUserDTO>(userDB);

                patchDocument.ApplyTo(user, ModelState);

                var isValid = TryValidateModel(user);

                if (!isValid)
                {
                    return BadRequest(ModelState);
                }

                mapper.Map(user, userDB);

                await context.SaveChangesAsync();
                return Ok("User updated successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message); 
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            try
            {
                var existe = await context.Users.AnyAsync(x => x.Id == id);

                if (!existe)
                {
                    return NotFound();
                }

                context.Remove(new User() { Id = id });
                await context.SaveChangesAsync();
                return Ok("User deleted successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(400, ex.Message);                
            }

        }
    }
}
