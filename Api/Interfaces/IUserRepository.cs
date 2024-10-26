using Api.Data.Entities;

namespace Api.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<AppUser>> GetUsers();
        Task<AppUser> GetUserById(int id);
        Task<AppUser> GetUserByName(string username);

        void Update(AppUser user);
        Task<bool> Save();
    }
}
