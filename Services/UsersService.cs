using Microsoft.AspNetCore.Identity;

namespace BIBLIOTECA_API.Services;

public class UsersService:IServiceUsers
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IHttpContextAccessor _contextAccessor;

    public UsersService(UserManager<IdentityUser> userManager, IHttpContextAccessor contextAccessor)
    {
        _userManager = userManager;
        _contextAccessor = contextAccessor;
    }

    public async Task<IdentityUser> GetUser()
    {
        var emailClaim = _contextAccessor.HttpContext!.User.Claims.FirstOrDefault(x => x.Type == "email");
        if (emailClaim is null)
        {
            return null;
        }

        var email = emailClaim.Value;
        return await _userManager.FindByEmailAsync(email);
    }
}