using Api.Data;
using Api.Data.Entities;
using Api.DTOs;
using Api.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace Api.Controllers
{
    
    public class AccountController : BaseApiController
    {
        private readonly DateContext _context;
        private readonly ITokenProvider _tokenProvider;
        public AccountController(DateContext context, ITokenProvider tokenProvider)
        {
            _context=context;
            _tokenProvider=tokenProvider;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register([FromBody]RegisterDto registerDto)
        {
            if(await IsUsernameTaken(registerDto.Username)) 
               return  BadRequest("UserName Is Taken");

            using var hmac = new HMACSHA512();
            var user = new AppUser
            {
                Username = registerDto.Username.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt = hmac.Key
            };
            await _context.AddAsync(user);
            await _context.SaveChangesAsync();
            var token = await _tokenProvider.CreateToken(user);

            return Ok(new UserDto(){ Username = user.Username, Token = token });
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == dto.Username.ToLower());
            if(user is null)
                return Unauthorized("Username is Unvalid");

            if (!IsPasswordValid(user, dto.Password))
                return Unauthorized("password is invalid");

            var token = await _tokenProvider.CreateToken(user);

            return Ok(new UserDto(){ Username = user.Username , Token = token});
        }





        private async Task<bool> IsUsernameTaken(string username)
            => await _context.Users.AnyAsync(u => u.Username.ToLower() == username.ToLower());


        private bool IsPasswordValid(AppUser user,string password)
        {
            using var hmac = new HMACSHA512(user.PasswordSalt);
            var ComputedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            for (int i = 0; i < ComputedHash.Length; i++)
            {
                if (ComputedHash[i] != user.PasswordHash[i]) return false;
            }
            return true;
        }
    }
}
