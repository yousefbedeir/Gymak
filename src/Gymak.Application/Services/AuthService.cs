using FluentValidation;
using Gymak.Application.Common.Interfaces;
using Gymak.Application.DTOs;
using Gymak.Domain.Entities;
using Gymak.Domain.Enums;
using Gymak.Domain.Interfaces;

namespace Gymak.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IMemberProfileRepository _profileRepository;
    private readonly ISubscriptionRepository _subscriptionRepository;
    private readonly IApplicationDbContext _context;
    private readonly IPasswordHasher _passwordHasher;

    private readonly IValidator<RegisterRequest> _registerValidator;
    private readonly IValidator<LoginRequest> _loginValidator;
    private readonly IValidator<ForgotPasswordRequest> _forgotPasswordValidator;
    private readonly IValidator<ResetPasswordRequest> _resetPasswordValidator;
    private readonly IValidator<ChangePasswordRequest> _changePasswordValidator;

    public AuthService(
        IUserRepository userRepository,
        IMemberProfileRepository profileRepository,
        ISubscriptionRepository subscriptionRepository,
        IApplicationDbContext context,
        IPasswordHasher passwordHasher,
        IValidator<RegisterRequest> registerValidator,
        IValidator<LoginRequest> loginValidator,
        IValidator<ForgotPasswordRequest> forgotPasswordValidator,
        IValidator<ResetPasswordRequest> resetPasswordValidator,
        IValidator<ChangePasswordRequest> changePasswordValidator)
    {
        _userRepository = userRepository;
        _profileRepository = profileRepository;
        _subscriptionRepository = subscriptionRepository;
        _context = context;
        _passwordHasher = passwordHasher;
        _registerValidator = registerValidator;
        _loginValidator = loginValidator;
        _forgotPasswordValidator = forgotPasswordValidator;
        _resetPasswordValidator = resetPasswordValidator;
        _changePasswordValidator = changePasswordValidator;
    }

    public async Task<AuthResult> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default)
    {
        var validationResult = await _registerValidator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            var errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
            return new AuthResult(false, errors, null);
        }

        var existingUser = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
        if (existingUser != null)
        {
            return new AuthResult(false, $"An account with email '{request.Email}' already exists.", null);
        }

        var user = new User
        {
            FullName = request.FullName.Trim(),
            Email = request.Email.Trim().ToLowerInvariant(),
            PhoneNumber = request.PhoneNumber?.Trim(),
            Role = UserRole.Member
        };

        user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);

        await _userRepository.AddAsync(user, cancellationToken);

        // Auto-create MemberProfile for Member role
        var profile = new MemberProfile
        {
            UserId = user.Id,
            FitnessGoal = "General Fitness"
        };
        await _profileRepository.AddAsync(profile, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        var loggedInDto = new LoggedInUserDto(
            user.Id,
            user.FullName,
            user.Email,
            user.Role,
            IsPremium: false
        );

        return new AuthResult(true, null, loggedInDto);
    }

    public async Task<AuthResult> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        var validationResult = await _loginValidator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            var errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
            return new AuthResult(false, errors, null);
        }

        var emailLower = request.Email.Trim().ToLowerInvariant();
        var user = await _userRepository.GetByEmailAsync(emailLower, cancellationToken);
        if (user == null)
        {
            return new AuthResult(false, "Invalid email or password.", null);
        }

        var isValidPassword = _passwordHasher.VerifyPassword(user, user.PasswordHash, request.Password);
        if (!isValidPassword)
        {
            return new AuthResult(false, "Invalid email or password.", null);
        }

        // Check if user has an active premium subscription
        var activeSub = await _subscriptionRepository.GetActiveSubscriptionByUserIdAsync(user.Id, cancellationToken);
        bool isPremium = activeSub != null && activeSub.Status == SubscriptionStatus.Active && activeSub.EndDate > DateTime.UtcNow;

        var loggedInDto = new LoggedInUserDto(
            user.Id,
            user.FullName,
            user.Email,
            user.Role,
            isPremium
        );

        return new AuthResult(true, null, loggedInDto);
    }

    public async Task<LoggedInUserDto?> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
        if (user == null) return null;

        var activeSub = await _subscriptionRepository.GetActiveSubscriptionByUserIdAsync(userId, cancellationToken);
        bool isPremium = activeSub != null && activeSub.Status == SubscriptionStatus.Active && activeSub.EndDate > DateTime.UtcNow;

        return new LoggedInUserDto(user.Id, user.FullName, user.Email, user.Role, isPremium);
    }

    public async Task<ForgotPasswordResult> ForgotPasswordAsync(ForgotPasswordRequest request, CancellationToken cancellationToken = default)
    {
        var validationResult = await _forgotPasswordValidator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            var errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
            return new ForgotPasswordResult(false, errors, null);
        }

        var emailLower = request.Email.Trim().ToLowerInvariant();
        var user = await _userRepository.GetByEmailAsync(emailLower, cancellationToken);
        if (user == null)
        {
            // For security, do not disclose whether user exists or not, but return success message or token preview
            return new ForgotPasswordResult(true, null, "If an account exists, a reset code was generated.");
        }

        // Generate 6-digit reset code (or secure token)
        var resetCode = Random.Shared.Next(100000, 999999).ToString();
        user.PasswordResetToken = resetCode;
        user.ResetTokenExpiresAt = DateTime.UtcNow.AddHours(1);

        _userRepository.Update(user);
        await _context.SaveChangesAsync(cancellationToken);

        return new ForgotPasswordResult(true, null, resetCode);
    }

    public async Task<AuthResult> ResetPasswordAsync(ResetPasswordRequest request, CancellationToken cancellationToken = default)
    {
        var validationResult = await _resetPasswordValidator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            var errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
            return new AuthResult(false, errors, null);
        }

        var emailLower = request.Email.Trim().ToLowerInvariant();
        var user = await _userRepository.GetByEmailAsync(emailLower, cancellationToken);
        if (user == null)
        {
            return new AuthResult(false, "Invalid email address or reset token.", null);
        }

        if (string.IsNullOrWhiteSpace(user.PasswordResetToken) || user.PasswordResetToken != request.Token.Trim())
        {
            return new AuthResult(false, "Invalid reset token.", null);
        }

        if (!user.ResetTokenExpiresAt.HasValue || user.ResetTokenExpiresAt.Value < DateTime.UtcNow)
        {
            return new AuthResult(false, "Reset token has expired. Please request a new one.", null);
        }

        user.PasswordHash = _passwordHasher.HashPassword(user, request.NewPassword);
        user.PasswordResetToken = null;
        user.ResetTokenExpiresAt = null;

        _userRepository.Update(user);
        await _context.SaveChangesAsync(cancellationToken);

        var activeSub = await _subscriptionRepository.GetActiveSubscriptionByUserIdAsync(user.Id, cancellationToken);
        bool isPremium = activeSub != null && activeSub.Status == SubscriptionStatus.Active;

        var loggedInDto = new LoggedInUserDto(user.Id, user.FullName, user.Email, user.Role, isPremium);
        return new AuthResult(true, null, loggedInDto);
    }

    public async Task<AuthResult> ChangePasswordAsync(ChangePasswordRequest request, CancellationToken cancellationToken = default)
    {
        var validationResult = await _changePasswordValidator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            var errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
            return new AuthResult(false, errors, null);
        }

        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
        if (user == null)
        {
            return new AuthResult(false, "User not found.", null);
        }

        var isCurrentValid = _passwordHasher.VerifyPassword(user, user.PasswordHash, request.CurrentPassword);
        if (!isCurrentValid)
        {
            return new AuthResult(false, "Current password is incorrect.", null);
        }

        user.PasswordHash = _passwordHasher.HashPassword(user, request.NewPassword);
        _userRepository.Update(user);
        await _context.SaveChangesAsync(cancellationToken);

        var activeSub = await _subscriptionRepository.GetActiveSubscriptionByUserIdAsync(user.Id, cancellationToken);
        bool isPremium = activeSub != null && activeSub.Status == SubscriptionStatus.Active;

        var loggedInDto = new LoggedInUserDto(user.Id, user.FullName, user.Email, user.Role, isPremium);
        return new AuthResult(true, null, loggedInDto);
    }
}
