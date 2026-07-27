using Gymak.Application.DTOs;
using Gymak.Domain.Enums;

namespace Gymak.Application.Services;

public interface IUserService
{
    Task<UserDto?> GetByIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<UserDto?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<UserDto>> GetUsersByRoleAsync(UserRole role, CancellationToken cancellationToken = default);
    Task<Guid> CreateUserAsync(CreateUserRequest request, CancellationToken cancellationToken = default);
    
    Task<MemberProfileDto?> GetProfileByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task UpsertProfileAsync(UpsertMemberProfileRequest request, CancellationToken cancellationToken = default);

    Task<Guid> AssignTrainerAsync(AssignTrainerRequest request, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<TrainerClientDto>> GetTrainerClientsAsync(Guid trainerId, CancellationToken cancellationToken = default);
}
