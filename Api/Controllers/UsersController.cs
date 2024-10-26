using Api.Data;
using Api.Data.Entities;
using Api.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    
    public class UsersController : BaseApiController
    {
        private readonly DateContext _context;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public UsersController( IUserRepository userRepository, IMapper mapper)
        {
            _userRepository=userRepository;
            _mapper=mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
        {
            return Ok(await _userRepository.GetUsers());
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<AppUser>> GetUser([FromRoute]int id)
        {
            var user = await _userRepository.GetUserById(id);
            if(user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpGet("{username}")]
        public async Task<ActionResult<AppUser>> GetUserByUsername(string username)
        {
            var user = await _userRepository.GetUserByName(username);
            return Ok(user);
        }

    }
}
