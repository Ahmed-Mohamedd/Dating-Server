using Api.Data;
using Api.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly DateContext _context;

        public UsersController(DateContext context)
        {
            _context=context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
        {
            return Ok(await _context.Users.ToListAsync());
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<AppUser>> GetUser([FromRoute]int id)
        {
            var user = await _context.Users.FindAsync(id);
            if(user == null)
                return NotFound();

            return Ok(user);
        }
    }
}
