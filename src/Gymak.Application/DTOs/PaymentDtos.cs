using Gymak.Domain.Enums;

namespace Gymak.Application.DTOs;

public record PaymentDto(
    Guid PaymentId,
    Guid SubscriptionId,
    Guid UserId,
    string UserName,
    decimal Amount,
    DateTime PaymentDate,
    PaymentMethod PaymentMethod,
    PaymentStatus Status,
    string? TransactionReference,
    string? Notes
);

public record RecordPaymentRequest(
    Guid SubscriptionId,
    Guid UserId,
    decimal Amount,
    PaymentMethod PaymentMethod,
    PaymentStatus Status,
    string? TransactionReference,
    string? Notes
);
