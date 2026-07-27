using FluentValidation;
using Gymak.Application.Common.Interfaces;
using Gymak.Application.DTOs;
using Gymak.Domain.Entities;
using Gymak.Domain.Enums;
using Gymak.Domain.Interfaces;

namespace Gymak.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMemberProfileRepository _profileRepository;
    private readonly ITrainerClientRepository _trainerClientRepository;
    private readonly IApplicationDbContext _context;
    private readonly IValidator<CreateUserRequest> _createUserValidator;
    private readonly IValidator<UpsertMemberProfileRequest> _profileValidator;
    private readonly IValidator<AssignTrainerRequest> _assignTrainerValidator;

    public UserService(
        IUserRepository userRepository,
        IMemberProfileRepository profileRepository,
        ITrainerClientRepository trainerClientRepository,
        IApplicationDbContext context,
        IValidator<CreateUserRequest> createUserValidator,
        IValidator<UpsertMemberProfileRequest> profileValidator,
        IValidator<AssignTrainerRequest> assignTrainerValidator)
    {
        _userRepository = userRepository;
        _profileRepository = profileRepository;
        _trainerClientRepository = trainerClientRepository;
        _context = context;
        _createUserValidator = createUserValidator;
        _profileValidator = profileValidator;
        _assignTrainerValidator = assignTrainerValidator;
    }

    public async Task<UserDto?> GetByIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var u = await _userRepository.GetByIdAsync(userId, cancellationToken);
        return u is null ? null : new UserDto(u.Id, u.FullName, u.Email, u.PhoneNumber, u.Role, u.CreatedAt);
    }

    public async Task<UserDto?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var u = await _userRepository.GetByEmailAsync(email, cancellationToken);
        return u is null ? null : new UserDto(u.Id, u.FullName, u.Email, u.PhoneNumber, u.Role, u.CreatedAt);
    }

    public async Task<IReadOnlyList<UserDto>> GetUsersByRoleAsync(UserRole role, CancellationToken cancellationToken = default)
    {
        var users = await _userRepository.GetAllByRoleAsync(role, cancellationToken);
        return users.Select(u => new UserDto(u.Id, u.FullName, u.Email, u.PhoneNumber, u.Role, u.CreatedAt)).ToList();
    }

    public async Task<Guid> CreateUserAsync(CreateUserRequest request, CancellationToken cancellationToken = default)
    {
        await _createUserValidator.ValidateAndThrowAsync(request, cancellationToken);

        var existingUser = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
        if (existingUser != null)
        {
            throw new InvalidOperationException($"A user with email '{request.Email}' already exists.");
        }

        var user = new User
        {
            FullName = request.FullName,
            Email = request.Email,
            PasswordHash = request.Password,
            PhoneNumber = request.PhoneNumber,
            Role = request.Role
        };

        await _userRepository.AddAsync(user, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return user.Id;
    }

    public async Task<MemberProfileDto?> GetProfileByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var p = await _profileRepository.GetByUserIdAsync(userId, cancellationToken);
        return p is null ? null : new MemberProfileDto(p.ProfileId, p.UserId, p.Gender, p.DateOfBirth, p.Height, p.CurrentWeight, p.FitnessGoal);
    }

    public async Task UpsertProfileAsync(UpsertMemberProfileRequest request, CancellationToken cancellationToken = default)
    {
        await _profileValidator.ValidateAndThrowAsync(request, cancellationToken);

        var existingProfile = await _profileRepository.GetByUserIdAsync(request.UserId, cancellationToken);
        if (existingProfile is null)
        {
            var profile = new MemberProfile
            {
                UserId = request.UserId,
                Gender = request.Gender,
                DateOfBirth = request.DateOfBirth,
                Height = request.Height,
                CurrentWeight = request.CurrentWeight,
                FitnessGoal = request.FitnessGoal
            };
            await _profileRepository.AddAsync(profile, cancellationToken);
        }
        else
        {
            existingProfile.Gender = request.Gender;
            existingProfile.DateOfBirth = request.DateOfBirth;
            existingProfile.Height = request.Height;
            existingProfile.CurrentWeight = request.CurrentWeight;
            existingProfile.FitnessGoal = request.FitnessGoal;
            _profileRepository.Update(existingProfile);
        }

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Guid> AssignTrainerAsync(AssignTrainerRequest request, CancellationToken cancellationToken = default)
    {
        await _assignTrainerValidator.ValidateAndThrowAsync(request, cancellationToken);

        var trainer = await _userRepository.GetByIdAsync(request.TrainerId, cancellationToken);
        if (trainer is null || trainer.Role != UserRole.Trainer)
        {
            throw new InvalidOperationException("Specified trainer does not exist or does not have Trainer role.");
        }

        var client = await _userRepository.GetByIdAsync(request.ClientId, cancellationToken);
        if (client is null)
        {
            throw new InvalidOperationException("Specified client user does not exist.");
        }

        var assignment = new TrainerClient
        {
            TrainerId = request.TrainerId,
            ClientId = request.ClientId,
            StartDate = DateTime.UtcNow,
            Status = AssignmentStatus.Active
        };

        await _trainerClientRepository.AddAsync(assignment, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return assignment.AssignmentId;
    }

    public async Task<IReadOnlyList<TrainerClientDto>> GetTrainerClientsAsync(Guid trainerId, CancellationToken cancellationToken = default)
    {
        var list = await _trainerClientRepository.GetClientsForTrainerAsync(trainerId, cancellationToken);
        return list.Select(tc => new TrainerClientDto(
            tc.AssignmentId,
            tc.TrainerId,
            tc.Trainer?.FullName ?? string.Empty,
            tc.ClientId,
            tc.Client?.FullName ?? string.Empty,
            tc.StartDate,
            tc.EndDate,
            tc.Status
        )).ToList();
    }
    public async Task<Guid?> AutoAssignTrainerAsync(Guid memberId, CancellationToken cancellationToken = default)
    {
        // Get all trainers
        var trainers = await _userRepository.GetAllByRoleAsync(UserRole.Trainer, cancellationToken);
        if (!trainers.Any()) return null;

        // Find trainer with fewest active clients
        var trainerClientCounts = new List<(Guid TrainerId, int Count)>();
        foreach (var trainer in trainers)
        {
            var clients = await _trainerClientRepository.GetClientsForTrainerAsync(trainer.Id, cancellationToken);
            var activeCount = clients.Count(c => c.Status == AssignmentStatus.Active);
            trainerClientCounts.Add((trainer.Id, activeCount));
        }

        var bestTrainer = trainerClientCounts.MinBy(x => x.Count);

        var assignment = new TrainerClient
        {
            TrainerId = bestTrainer.TrainerId,
            ClientId = memberId,
            StartDate = DateTime.UtcNow,
            Status = AssignmentStatus.Active
        };

        await _trainerClientRepository.AddAsync(assignment, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return assignment.AssignmentId;
    }
}
