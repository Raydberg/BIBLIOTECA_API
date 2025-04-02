using Microsoft.AspNetCore.Identity;

namespace BIBLIOTECA_API.Services;

public interface IServiceUsers
{
    Task<IdentityUser> GetUser();
}