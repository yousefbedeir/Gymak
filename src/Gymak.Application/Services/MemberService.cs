using FluentValidation;
using Gymak.Application.Common.Interfaces;
using Gymak.Application.DTOs;
using Gymak.Domain.Entities;
using Gymak.Domain.Interfaces;

namespace Gymak.Application.Services;

public class MemberService : IMemberService
{
    private readonly IMemberRepository _memberRepository;
    private readonly IApplicationDbContext _context;
    private readonly IValidator<CreateMemberRequest> _validator;

    public MemberService(
        IMemberRepository memberRepository,
        IApplicationDbContext context,
        IValidator<CreateMemberRequest> validator)
    {
        _memberRepository = memberRepository;
        _context = context;
        _validator = validator;
    }

    public async Task<IReadOnlyList<MemberDto>> GetMembersAsync(CancellationToken cancellationToken = default)
    {
        var members = await _memberRepository.GetAllAsync(cancellationToken);
        return members.Select(m => new MemberDto(
            m.Id,
            m.FirstName,
            m.LastName,
            m.Email,
            m.MembershipType,
            m.IsActive
        )).ToList();
    }

    public async Task<MemberDto?> GetMemberByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var m = await _memberRepository.GetByIdAsync(id, cancellationToken);
        if (m is null) return null;

        return new MemberDto(m.Id, m.FirstName, m.LastName, m.Email, m.MembershipType, m.IsActive);
    }

    public async Task<Guid> CreateMemberAsync(CreateMemberRequest request, CancellationToken cancellationToken = default)
    {
        await _validator.ValidateAndThrowAsync(request, cancellationToken);

        var member = new Member
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            MembershipType = request.MembershipType
        };

        await _memberRepository.AddAsync(member, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return member.Id;
    }
}
