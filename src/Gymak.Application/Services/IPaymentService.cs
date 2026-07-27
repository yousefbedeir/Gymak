using Gymak.Application.DTOs;

namespace Gymak.Application.Services;

public interface IPaymentService
{
    Task<IReadOnlyList<PaymentDto>> GetAllPaymentsAsync(CancellationToken cancellationToken = default);
    Task<PaymentDto?> GetPaymentByIdAsync(Guid paymentId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<PaymentDto>> GetPaymentsBySubscriptionAsync(Guid subscriptionId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<PaymentDto>> GetPaymentsByUserAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<Guid> RecordPaymentAsync(RecordPaymentRequest request, CancellationToken cancellationToken = default);
}
