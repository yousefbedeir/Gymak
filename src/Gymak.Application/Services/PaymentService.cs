using FluentValidation;
using Gymak.Application.Common.Interfaces;
using Gymak.Application.DTOs;
using Gymak.Domain.Entities;
using Gymak.Domain.Interfaces;

namespace Gymak.Application.Services;

public class PaymentService : IPaymentService
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IApplicationDbContext _context;
    private readonly IValidator<RecordPaymentRequest> _recordPaymentValidator;

    public PaymentService(
        IPaymentRepository paymentRepository,
        IApplicationDbContext context,
        IValidator<RecordPaymentRequest> recordPaymentValidator)
    {
        _paymentRepository = paymentRepository;
        _context = context;
        _recordPaymentValidator = recordPaymentValidator;
    }

    public async Task<IReadOnlyList<PaymentDto>> GetAllPaymentsAsync(CancellationToken cancellationToken = default)
    {
        var payments = await _paymentRepository.GetAllAsync(cancellationToken);
        return payments.Select(MapToDto).ToList();
    }

    public async Task<PaymentDto?> GetPaymentByIdAsync(Guid paymentId, CancellationToken cancellationToken = default)
    {
        var payment = await _paymentRepository.GetByIdAsync(paymentId, cancellationToken);
        return payment is null ? null : MapToDto(payment);
    }

    public async Task<IReadOnlyList<PaymentDto>> GetPaymentsBySubscriptionAsync(Guid subscriptionId, CancellationToken cancellationToken = default)
    {
        var payments = await _paymentRepository.GetBySubscriptionIdAsync(subscriptionId, cancellationToken);
        return payments.Select(MapToDto).ToList();
    }

    public async Task<IReadOnlyList<PaymentDto>> GetPaymentsByUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var payments = await _paymentRepository.GetByUserIdAsync(userId, cancellationToken);
        return payments.Select(MapToDto).ToList();
    }

    public async Task<Guid> RecordPaymentAsync(RecordPaymentRequest request, CancellationToken cancellationToken = default)
    {
        await _recordPaymentValidator.ValidateAndThrowAsync(request, cancellationToken);

        var payment = new Payment
        {
            SubscriptionId = request.SubscriptionId,
            UserId = request.UserId,
            Amount = request.Amount,
            PaymentDate = DateTime.UtcNow,
            PaymentMethod = request.PaymentMethod,
            Status = request.Status,
            TransactionReference = request.TransactionReference,
            Notes = request.Notes
        };

        await _paymentRepository.AddAsync(payment, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return payment.Id;
    }

    private static PaymentDto MapToDto(Payment p) =>
        new(
            p.Id,
            p.SubscriptionId,
            p.UserId,
            p.User?.FullName ?? string.Empty,
            p.Amount,
            p.PaymentDate,
            p.PaymentMethod,
            p.Status,
            p.TransactionReference,
            p.Notes
        );
}
