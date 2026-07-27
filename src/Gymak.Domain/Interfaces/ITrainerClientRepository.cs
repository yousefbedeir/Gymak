using Gymak.Domain.Entities;
using Gymak.Domain.Enums;

namespace Gymak.Domain.Interfaces;

public interface ITrainerClientRepository
{
    Task<TrainerClient?> GetByIdAsync(Guid assignmentId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<TrainerClient>> GetClientsForTrainerAsync(Guid trainerId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<TrainerClient>> GetTrainersForClientAsync(Guid clientId, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Guid trainerId, Guid clientId, CancellationToken cancellationToken = default);
    Task AddAsync(TrainerClient assignment, CancellationToken cancellationToken = default);
    void Update(TrainerClient assignment);
}
