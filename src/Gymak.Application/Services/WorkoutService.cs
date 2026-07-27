using FluentValidation;
using Gymak.Application.Common.Interfaces;
using Gymak.Application.DTOs;
using Gymak.Domain.Entities;
using Gymak.Domain.Interfaces;

namespace Gymak.Application.Services;

public class WorkoutService : IWorkoutService
{
    private readonly IMuscleGroupRepository _muscleGroupRepository;
    private readonly IExerciseRepository _exerciseRepository;
    private readonly IWorkoutPlanRepository _workoutPlanRepository;
    private readonly IWorkoutLogRepository _workoutLogRepository;
    private readonly IApplicationDbContext _context;

    private readonly IValidator<CreateMuscleGroupRequest> _muscleGroupValidator;
    private readonly IValidator<CreateExerciseRequest> _exerciseValidator;
    private readonly IValidator<CreateWorkoutPlanRequest> _planValidator;
    private readonly IValidator<CreateWorkoutLogRequest> _logValidator;

    public WorkoutService(
        IMuscleGroupRepository muscleGroupRepository,
        IExerciseRepository exerciseRepository,
        IWorkoutPlanRepository workoutPlanRepository,
        IWorkoutLogRepository workoutLogRepository,
        IApplicationDbContext context,
        IValidator<CreateMuscleGroupRequest> muscleGroupValidator,
        IValidator<CreateExerciseRequest> exerciseValidator,
        IValidator<CreateWorkoutPlanRequest> planValidator,
        IValidator<CreateWorkoutLogRequest> logValidator)
    {
        _muscleGroupRepository = muscleGroupRepository;
        _exerciseRepository = exerciseRepository;
        _workoutPlanRepository = workoutPlanRepository;
        _workoutLogRepository = workoutLogRepository;
        _context = context;
        _muscleGroupValidator = muscleGroupValidator;
        _exerciseValidator = exerciseValidator;
        _planValidator = planValidator;
        _logValidator = logValidator;
    }

    public async Task<IReadOnlyList<MuscleGroupDto>> GetMuscleGroupsAsync(CancellationToken cancellationToken = default)
    {
        var groups = await _muscleGroupRepository.GetAllAsync(cancellationToken);
        return groups.Select(g => new MuscleGroupDto(g.MuscleGroupId, g.Name)).ToList();
    }

    public async Task<Guid> CreateMuscleGroupAsync(CreateMuscleGroupRequest request, CancellationToken cancellationToken = default)
    {
        await _muscleGroupValidator.ValidateAndThrowAsync(request, cancellationToken);
        var group = new MuscleGroup { Name = request.Name };
        await _muscleGroupRepository.AddAsync(group, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return group.MuscleGroupId;
    }

    public async Task<IReadOnlyList<ExerciseDto>> GetAllExercisesAsync(CancellationToken cancellationToken = default)
    {
        var exercises = await _exerciseRepository.GetAllAsync(cancellationToken);
        return exercises.Select(e => new ExerciseDto(
            e.ExerciseId,
            e.MuscleGroupId,
            e.MuscleGroup?.Name ?? string.Empty,
            e.Name,
            e.Description,
            e.ExerciseType,
            e.MediaUrl
        )).ToList();
    }

    public async Task<IReadOnlyList<ExerciseDto>> GetExercisesByMuscleGroupAsync(Guid muscleGroupId, CancellationToken cancellationToken = default)
    {
        var exercises = await _exerciseRepository.GetByMuscleGroupIdAsync(muscleGroupId, cancellationToken);
        return exercises.Select(e => new ExerciseDto(
            e.ExerciseId,
            e.MuscleGroupId,
            e.MuscleGroup?.Name ?? string.Empty,
            e.Name,
            e.Description,
            e.ExerciseType,
            e.MediaUrl
        )).ToList();
    }

    public async Task<Guid> CreateExerciseAsync(CreateExerciseRequest request, CancellationToken cancellationToken = default)
    {
        await _exerciseValidator.ValidateAndThrowAsync(request, cancellationToken);
        var exercise = new Exercise
        {
            MuscleGroupId = request.MuscleGroupId,
            Name = request.Name,
            Description = request.Description,
            ExerciseType = request.ExerciseType,
            MediaUrl = request.MediaUrl
        };
        await _exerciseRepository.AddAsync(exercise, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return exercise.ExerciseId;
    }

    public async Task<WorkoutPlanDto?> GetWorkoutPlanByIdAsync(Guid planId, CancellationToken cancellationToken = default)
    {
        var plan = await _workoutPlanRepository.GetByIdAsync(planId, cancellationToken);
        if (plan is null) return null;

        var exercisesDto = plan.PlanExercises.Select(pe => new WorkoutPlanExerciseDto(
            pe.PlanExerciseId,
            pe.ExerciseId,
            pe.Exercise?.Name ?? string.Empty,
            pe.DayOfWeek,
            pe.Sets,
            pe.Reps,
            pe.RestTimeSeconds
        )).ToList();

        return new WorkoutPlanDto(
            plan.WorkoutPlanId,
            plan.TrainerId,
            plan.Trainer?.FullName ?? string.Empty,
            plan.MemberId,
            plan.Member?.FullName ?? string.Empty,
            plan.Title,
            plan.Description,
            exercisesDto
        );
    }

    public async Task<IReadOnlyList<WorkoutPlanDto>> GetWorkoutPlansByMemberAsync(Guid memberId, CancellationToken cancellationToken = default)
    {
        var plans = await _workoutPlanRepository.GetByMemberIdAsync(memberId, cancellationToken);
        return plans.Select(plan => new WorkoutPlanDto(
            plan.WorkoutPlanId,
            plan.TrainerId,
            plan.Trainer?.FullName ?? string.Empty,
            plan.MemberId,
            plan.Member?.FullName ?? string.Empty,
            plan.Title,
            plan.Description,
            plan.PlanExercises.Select(pe => new WorkoutPlanExerciseDto(
                pe.PlanExerciseId,
                pe.ExerciseId,
                pe.Exercise?.Name ?? string.Empty,
                pe.DayOfWeek,
                pe.Sets,
                pe.Reps,
                pe.RestTimeSeconds
            )).ToList()
        )).ToList();
    }

    public async Task<Guid> CreateWorkoutPlanAsync(CreateWorkoutPlanRequest request, CancellationToken cancellationToken = default)
    {
        await _planValidator.ValidateAndThrowAsync(request, cancellationToken);

        var plan = new WorkoutPlan
        {
            TrainerId = request.TrainerId,
            MemberId = request.MemberId,
            Title = request.Title,
            Description = request.Description
        };

        foreach (var ex in request.Exercises)
        {
            plan.PlanExercises.Add(new WorkoutPlanExercise
            {
                ExerciseId = ex.ExerciseId,
                DayOfWeek = ex.DayOfWeek,
                Sets = ex.Sets,
                Reps = ex.Reps,
                RestTimeSeconds = ex.RestTimeSeconds
            });
        }

        await _workoutPlanRepository.AddAsync(plan, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return plan.WorkoutPlanId;
    }

    public async Task<IReadOnlyList<WorkoutLogDto>> GetUserWorkoutLogsAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var logs = await _workoutLogRepository.GetByUserIdAsync(userId, cancellationToken);
        return logs.Select(l => new WorkoutLogDto(
            l.LogId,
            l.UserId,
            l.ExerciseId,
            l.Exercise?.Name ?? string.Empty,
            l.LogDate,
            l.SetsCompleted,
            l.RepsCompleted,
            l.WeightUsed,
            l.CaloriesBurned
        )).ToList();
    }

    public async Task<Guid> CreateWorkoutLogAsync(CreateWorkoutLogRequest request, CancellationToken cancellationToken = default)
    {
        await _logValidator.ValidateAndThrowAsync(request, cancellationToken);

        var log = new WorkoutLog
        {
            UserId = request.UserId,
            ExerciseId = request.ExerciseId,
            LogDate = request.LogDate,
            SetsCompleted = request.SetsCompleted,
            RepsCompleted = request.RepsCompleted,
            WeightUsed = request.WeightUsed,
            CaloriesBurned = request.CaloriesBurned
        };

        await _workoutLogRepository.AddAsync(log, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return log.LogId;
    }
}
