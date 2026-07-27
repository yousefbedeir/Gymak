using Gymak.Domain.Enums;
using Gymak.Domain.ValueObjects;

namespace Gymak.Domain.Entities;

public class Member : BaseEntity
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public MembershipType MembershipType { get; set; } = MembershipType.Standard;
    public Address? Address { get; set; }
    public bool IsActive { get; set; } = true;
}
