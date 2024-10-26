using Api.Data.Entities;
using Api.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Api.Data
{
    public class UserRepository:IUserRepository
    {
        private readonly DateContext _context;
        private readonly IMapper _mapper;

        public UserRepository(DateContext context)
        {
            _context=context;
         
        }

        public async Task<AppUser> GetUserById(int id)
          => await _context.Users.FindAsync(id);

        public async Task<AppUser> GetUserByName(string username)
            => await _context.Users.Include(u => u.Photos).FirstOrDefaultAsync(u => u.Username == username);

        public async Task<IEnumerable<AppUser>> GetUsers()
            => await _context.Users.Include(u => u.Photos).ToListAsync();

        public async Task<bool> Save()
            => await _context.SaveChangesAsync() > 0;

        public void Update(AppUser user)
        {
            _context.Entry(user).State = EntityState.Modified;
            //_context.Users.Update(user);
        }
    }
}
