using Microsoft.Build.Framework;

namespace BIBLIOTECA_API.DTOs.Auth;

public class CredentialsUsersDTO
{
    [Required]
    public required string Email { get; set; }
    [Required]
    public string? Password { get; set; }
}