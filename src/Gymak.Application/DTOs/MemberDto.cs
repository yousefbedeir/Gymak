using Gymak.Domain.Enums;

namespace Gymak.Application.DTOs;

public record MemberDto(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    MembershipType MembershipType,
    bool IsActive
);

public record CreateMemberRequest(
    string FirstName,
    string LastName,
    string Email,
    MembershipType MembershipType
);
