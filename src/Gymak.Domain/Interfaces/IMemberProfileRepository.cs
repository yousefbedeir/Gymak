using Gymak.Domain.Entities;

namespace Gymak.Domain.Interfaces;

public interface IMemberProfileRepository
{
    Task<MemberProfile?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task AddAsync(MemberProfile profile, CancellationToken cancellationToken = default);
    void Update(MemberProfile profile);
}
