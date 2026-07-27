using Gymak.Domain.Entities;

namespace Gymak.Application.Common.Interfaces;

public interface IPasswordHasher
{
    string HashPassword(User user, string password);
    bool VerifyPassword(User user, string hashedPassword, string providedPassword);
}
