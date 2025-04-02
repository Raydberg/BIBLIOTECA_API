namespace BIBLIOTECA_API.DTOs.Auth;

public class AuthenticationResponseDTO
{
    public required string Token { get; set; }
    public DateTime Expiracion { get; set; }
}