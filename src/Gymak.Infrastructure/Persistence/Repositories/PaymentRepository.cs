using Gymak.Domain.Entities;
using Gymak.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gymak.Infrastructure.Persistence.Repositories;

public class PaymentRepository : IPaymentRepository
{
    private readonly AppDbContext _context;

    public PaymentRepository(AppDbContext context) => _context = context;

    public async Task<Payment?> GetByIdAsync(Guid paymentId, CancellationToken cancellationToken = default)
        => await _context.Payments
            .Include(p => p.User)
            .Include(p => p.Subscription)
                .ThenInclude(s => s.Plan)
            .FirstOrDefaultAsync(p => p.Id == paymentId, cancellationToken);

    public async Task<IReadOnlyList<Payment>> GetBySubscriptionIdAsync(Guid subscriptionId, CancellationToken cancellationToken = default)
        => await _context.Payments
            .AsNoTracking()
            .Include(p => p.User)
            .Where(p => p.SubscriptionId == subscriptionId)
            .OrderByDescending(p => p.PaymentDate)
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<Payment>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        => await _context.Payments
            .AsNoTracking()
            .Include(p => p.Subscription)
                .ThenInclude(s => s.Plan)
            .Where(p => p.UserId == userId)
            .OrderByDescending(p => p.PaymentDate)
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<Payment>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _context.Payments
            .AsNoTracking()
            .Include(p => p.User)
            .Include(p => p.Subscription)
                .ThenInclude(s => s.Plan)
            .OrderByDescending(p => p.PaymentDate)
            .ToListAsync(cancellationToken);

    public async Task AddAsync(Payment payment, CancellationToken cancellationToken = default)
        => await _context.Payments.AddAsync(payment, cancellationToken);

    public void Update(Payment payment) => _context.Payments.Update(payment);
}
