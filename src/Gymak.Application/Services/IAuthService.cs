using Gymak.Application.DTOs;

namespace Gymak.Application.Services;

public interface IAuthService
{
    Task<AuthResult> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default);
    Task<AuthResult> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default);
    Task<LoggedInUserDto?> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<ForgotPasswordResult> ForgotPasswordAsync(ForgotPasswordRequest request, CancellationToken cancellationToken = default);
    Task<AuthResult> ResetPasswordAsync(ResetPasswordRequest request, CancellationToken cancellationToken = default);
    Task<AuthResult> ChangePasswordAsync(ChangePasswordRequest request, CancellationToken cancellationToken = default);
}
