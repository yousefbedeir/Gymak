using FluentValidation;
using Gymak.Application.Common.Interfaces;
using Gymak.Application.DTOs;
using Gymak.Domain.Entities;
using Gymak.Domain.Interfaces;

namespace Gymak.Application.Services;

public class ProgressLogService : IProgressLogService
{
    private readonly IProgressLogRepository _progressLogRepository;
    private readonly IApplicationDbContext _context;
    private readonly IValidator<CreateProgressLogRequest> _createValidator;
    private readonly IValidator<UpdateProgressLogRequest> _updateValidator;

    public ProgressLogService(
        IProgressLogRepository progressLogRepository,
        IApplicationDbContext context,
        IValidator<CreateProgressLogRequest> createValidator,
        IValidator<UpdateProgressLogRequest> updateValidator)
    {
        _progressLogRepository = progressLogRepository;
        _context = context;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    public async Task<ProgressLogDto?> GetByIdAsync(Guid logId, CancellationToken cancellationToken = default)
    {
        var log = await _progressLogRepository.GetByIdAsync(logId, cancellationToken);
        return log is null ? null : MapToDto(log);
    }

    public async Task<IReadOnlyList<ProgressLogDto>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var logs = await _progressLogRepository.GetByUserIdAsync(userId, cancellationToken);
        return logs.Select(MapToDto).ToList();
    }

    public async Task<ProgressLogDto?> GetLatestByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var log = await _progressLogRepository.GetLatestByUserIdAsync(userId, cancellationToken);
        return log is null ? null : MapToDto(log);
    }

    public async Task<Guid> LogProgressAsync(CreateProgressLogRequest request, CancellationToken cancellationToken = default)
    {
        await _createValidator.ValidateAndThrowAsync(request, cancellationToken);

        var log = new ProgressLog
        {
            UserId = request.UserId,
            LogDate = request.LogDate,
            Weight = request.Weight,
            Height = request.Height,
            BodyFatPercentage = request.BodyFatPercentage,
            MuscleMassPercentage = request.MuscleMassPercentage,
            ChestCm = request.ChestCm,
            WaistCm = request.WaistCm,
            HipsCm = request.HipsCm,
            BicepsCm = request.BicepsCm,
            ThighsCm = request.ThighsCm,
            Notes = request.Notes
        };

        await _progressLogRepository.AddAsync(log, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return log.Id;
    }

    public async Task UpdateProgressLogAsync(UpdateProgressLogRequest request, CancellationToken cancellationToken = default)
    {
        await _updateValidator.ValidateAndThrowAsync(request, cancellationToken);

        var log = await _progressLogRepository.GetByIdAsync(request.LogId, cancellationToken)
            ?? throw new InvalidOperationException($"Progress log with ID '{request.LogId}' not found.");

        log.LogDate = request.LogDate;
        log.Weight = request.Weight;
        log.Height = request.Height;
        log.BodyFatPercentage = request.BodyFatPercentage;
        log.MuscleMassPercentage = request.MuscleMassPercentage;
        log.ChestCm = request.ChestCm;
        log.WaistCm = request.WaistCm;
        log.HipsCm = request.HipsCm;
        log.BicepsCm = request.BicepsCm;
        log.ThighsCm = request.ThighsCm;
        log.Notes = request.Notes;
        log.LastModifiedAt = DateTime.UtcNow;

        _progressLogRepository.Update(log);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteProgressLogAsync(Guid logId, CancellationToken cancellationToken = default)
    {
        var log = await _progressLogRepository.GetByIdAsync(logId, cancellationToken)
            ?? throw new InvalidOperationException($"Progress log with ID '{logId}' not found.");

        _progressLogRepository.Delete(log);
        await _context.SaveChangesAsync(cancellationToken);
    }

    private static ProgressLogDto MapToDto(ProgressLog p) =>
        new(
            p.Id,
            p.UserId,
            p.User?.FullName ?? string.Empty,
            p.LogDate,
            p.Weight,
            p.Height,
            p.BodyFatPercentage,
            p.MuscleMassPercentage,
            p.ChestCm,
            p.WaistCm,
            p.HipsCm,
            p.BicepsCm,
            p.ThighsCm,
            p.BMI,
            p.Notes
        );
}
