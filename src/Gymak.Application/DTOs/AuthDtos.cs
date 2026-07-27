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

public record ForgotPasswordRequest(
    string Email
);

public record ForgotPasswordResult(
    bool Succeeded,
    string? ErrorMessage,
    string? ResetToken
);

public record ResetPasswordRequest(
    string Email,
    string Token,
    string NewPassword,
    string ConfirmNewPassword
);

public record ChangePasswordRequest(
    Guid UserId,
    string CurrentPassword,
    string NewPassword,
    string ConfirmNewPassword
);
