using Gymak.Application.DTOs;

namespace Gymak.Application.Services;

public interface IMemberService
{
    Task<IReadOnlyList<MemberDto>> GetMembersAsync(CancellationToken cancellationToken = default);
    Task<MemberDto?> GetMemberByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Guid> CreateMemberAsync(CreateMemberRequest request, CancellationToken cancellationToken = default);
}
