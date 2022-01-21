using Domain.Users;

namespace Services.Jwt
{
    public interface IJWTService
    {
        string GenerateJwtToken(User user);
    }
}
