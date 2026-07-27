using Gymak.Domain.Entities;

namespace Gymak.Domain.Interfaces;

public interface IDietPlanRepository
{
    Task<DietPlan?> GetByIdAsync(Guid dietPlanId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DietPlan>> GetByMemberIdAsync(Guid memberId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DietPlan>> GetByTrainerIdAsync(Guid trainerId, CancellationToken cancellationToken = default);
    Task AddAsync(DietPlan dietPlan, CancellationToken cancellationToken = default);
    void Update(DietPlan dietPlan);
    void Delete(DietPlan dietPlan);
}
