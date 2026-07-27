using Gymak.Domain.Enums;

namespace Gymak.Application.DTOs;

public record RegisterRequest(
    string FullName,
    string Email,
    string Password,
    string ConfirmPassword,
    string? PhoneNumber
);

public record LoginRequest(
    string Email,
    string Password
);

public record AuthResult(
    bool Succeeded,
    string? ErrorMessage,
    LoggedInUserDto? User
);

public record LoggedInUserDto(
    Guid UserId,
    string FullName,
    string Email,
    UserRole Role,
    bool IsPremium
);
