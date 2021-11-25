using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserAPI.Models;
using UserAPI.Repositories;

namespace UserAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]

        public async Task<IEnumerable<User>> GetUsers()
        {
            return await _userRepository.Get();
            //async konvertálja a user objektumot JSON-á?
        }

        [HttpGet("{id}")]//itt adjuk majd át az id paramétert ha nem akarjuk méekérdzni az összeset
        public async Task<ActionResult<User>> GetUsers(int id)
        { 
            return await _userRepository.Get(id);
        }

        [HttpPost]

        public async Task<ActionResult<User>> PostUsers(int id, [FromBody] User user)
        {
            var newUser = await _userRepository.Create(user);
            return CreatedAtAction(nameof(GetUsers), new { id = newUser.Id }, newUser);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> PutUsers(int id, [FromBody] User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }
            await _userRepository.Update(user);

            return NoContent();
        }

        [HttpDelete("{id}")]

        public async Task<ActionResult> Delete(int id)
        {
            var userToDelete = await _userRepository.Get(id);
            if (userToDelete == null)
            {
                return NotFound();
            }
            await _userRepository.Delete(userToDelete.Id);
            return NoContent();
        }
    }
}
