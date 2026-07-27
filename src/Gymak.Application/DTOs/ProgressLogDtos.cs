namespace Gymak.Application.DTOs;

public record ProgressLogDto(
    Guid Id,
    Guid UserId,
    string UserName,
    DateTime LogDate,
    decimal Weight,
    decimal Height,
    decimal? BodyFatPercentage,
    decimal? MuscleMassPercentage,
    decimal? ChestCm,
    decimal? WaistCm,
    decimal? HipsCm,
    decimal? BicepsCm,
    decimal? ThighsCm,
    decimal BMI,
    string? Notes
);

public record CreateProgressLogRequest(
    Guid UserId,
    DateTime LogDate,
    decimal Weight,
    decimal Height,
    decimal? BodyFatPercentage,
    decimal? MuscleMassPercentage,
    decimal? ChestCm,
    decimal? WaistCm,
    decimal? HipsCm,
    decimal? BicepsCm,
    decimal? ThighsCm,
    string? Notes
);

public record UpdateProgressLogRequest(
    Guid LogId,
    DateTime LogDate,
    decimal Weight,
    decimal Height,
    decimal? BodyFatPercentage,
    decimal? MuscleMassPercentage,
    decimal? ChestCm,
    decimal? WaistCm,
    decimal? HipsCm,
    decimal? BicepsCm,
    decimal? ThighsCm,
    string? Notes
);
