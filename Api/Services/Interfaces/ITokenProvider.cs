using Api.Data.Entities;

namespace Api.Services.Interfaces
{
    public interface ITokenProvider
    {
        Task<string> CreateToken(AppUser user);
    }
}
