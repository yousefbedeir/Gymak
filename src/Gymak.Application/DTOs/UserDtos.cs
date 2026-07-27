using Gymak.Domain.Enums;

namespace Gymak.Application.DTOs;

public record UserDto(
    Guid UserId,
    string FullName,
    string Email,
    string? PhoneNumber,
    UserRole Role,
    DateTime CreatedAt
);

public record CreateUserRequest(
    string FullName,
    string Email,
    string Password,
    string? PhoneNumber,
    UserRole Role
);

public record MemberProfileDto(
    Guid ProfileId,
    Guid UserId,
    string? Gender,
    DateTime? DateOfBirth,
    decimal Height,
    decimal CurrentWeight,
    string? FitnessGoal
);

public record UpsertMemberProfileRequest(
    Guid UserId,
    string? Gender,
    DateTime? DateOfBirth,
    decimal Height,
    decimal CurrentWeight,
    string? FitnessGoal
);

public record TrainerClientDto(
    Guid AssignmentId,
    Guid TrainerId,
    string TrainerName,
    Guid ClientId,
    string ClientName,
    DateTime StartDate,
    DateTime? EndDate,
    AssignmentStatus Status
);

public record AssignTrainerRequest(
    Guid TrainerId,
    Guid ClientId
);
