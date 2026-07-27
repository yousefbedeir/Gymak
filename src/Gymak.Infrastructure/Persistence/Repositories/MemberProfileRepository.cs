using Gymak.Domain.Entities;
using Gymak.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gymak.Infrastructure.Persistence.Repositories;

public class MemberProfileRepository : IMemberProfileRepository
{
    private readonly AppDbContext _context;

    public MemberProfileRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<MemberProfile?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.MemberProfiles
            .FirstOrDefaultAsync(p => p.UserId == userId, cancellationToken);
    }

    public async Task AddAsync(MemberProfile profile, CancellationToken cancellationToken = default)
    {
        await _context.MemberProfiles.AddAsync(profile, cancellationToken);
    }

    public void Update(MemberProfile profile)
    {
        _context.MemberProfiles.Update(profile);
    }
}
