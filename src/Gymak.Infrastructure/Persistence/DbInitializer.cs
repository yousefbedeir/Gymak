using Gymak.Application.Common.Interfaces;
using Gymak.Domain.Entities;
using Gymak.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Gymak.Infrastructure.Persistence;

public static class DbInitializer
{
    public static async Task SeedAsync(AppDbContext context, IPasswordHasher passwordHasher)
    {
        // Ensure Database is Created & Migrations applied
        await context.Database.MigrateAsync();

        // Seed Users if none exist
        if (!await context.Users.AnyAsync())
        {
            var adminUser = new User
            {
                FullName = "Gym Administrator",
                Email = "admin@gymak.com",
                PhoneNumber = "+201000000001",
                Role = UserRole.Admin,
                CreatedAt = DateTime.UtcNow
            };
            adminUser.PasswordHash = passwordHasher.HashPassword(adminUser, "Admin123!");

            var trainerUser = new User
            {
                FullName = "Captain Ahmed",
                Email = "trainer@gymak.com",
                PhoneNumber = "+201000000002",
                Role = UserRole.Trainer,
                CreatedAt = DateTime.UtcNow
            };
            trainerUser.PasswordHash = passwordHasher.HashPassword(trainerUser, "Trainer123!");

            var memberUser = new User
            {
                FullName = "Mohamed Ali",
                Email = "member@gymak.com",
                PhoneNumber = "+201000000003",
                Role = UserRole.Member,
                CreatedAt = DateTime.UtcNow
            };
            memberUser.PasswordHash = passwordHasher.HashPassword(memberUser, "Member123!");

            await context.Users.AddRangeAsync(adminUser, trainerUser, memberUser);
            await context.SaveChangesAsync();

            // Create MemberProfile for Member user
            var memberProfile = new MemberProfile
            {
                UserId = memberUser.Id,
                Gender = "Male",
                DateOfBirth = new DateTime(1995, 5, 15),
                Height = 178,
                CurrentWeight = 76.5m,
                FitnessGoal = "Hypertrophy & Strength"
            };
            await context.MemberProfiles.AddAsync(memberProfile);

            // Assign Trainer to Member
            var assignment = new TrainerClient
            {
                TrainerId = trainerUser.Id,
                ClientId = memberUser.Id,
                StartDate = DateTime.UtcNow,
                Status = AssignmentStatus.Active
            };
            await context.TrainerClients.AddAsync(assignment);

            await context.SaveChangesAsync();
        }

        // Seed Subscription Plans if none exist
        if (!await context.SubscriptionPlans.AnyAsync())
        {
            var plans = new List<SubscriptionPlan>
            {
                new()
                {
                    Name = "Basic Fitness",
                    Description = "Access to basic gym facilities, cardio section & locker room.",
                    Price = 350.00m,
                    BillingCycle = BillingCycle.Monthly,
                    DurationDays = 30,
                    Features = "Gym floor access, Locker usage, Water station",
                    IsActive = true
                },
                new()
                {
                    Name = "Pro VIP Transformation",
                    Description = "Full access to gym, custom diet plan, workout routine & 1-on-1 trainer.",
                    Price = 950.00m,
                    BillingCycle = BillingCycle.Monthly,
                    DurationDays = 30,
                    Features = "Unlimited Gym Access, Dedicated Trainer, Customized Diet & Workout Plans, Progress Tracking",
                    IsActive = true
                },
                new()
                {
                    Name = "Annual Elite Legend",
                    Description = "Full year VIP membership with all premium perks included.",
                    Price = 7990.00m,
                    BillingCycle = BillingCycle.Annually,
                    DurationDays = 365,
                    Features = "Year-round Gym Access, Trainer Assigned, Diet Plans, Free Supplements Starter Kit, Guest Passes",
                    IsActive = true
                }
            };
            await context.SubscriptionPlans.AddRangeAsync(plans);
            await context.SaveChangesAsync();

            // Add sample active subscription for memberUser
            var memberUser = await context.Users.FirstOrDefaultAsync(u => u.Email == "member@gymak.com");
            var proPlan = await context.SubscriptionPlans.FirstOrDefaultAsync(p => p.Name == "Pro VIP Transformation");
            if (memberUser != null && proPlan != null)
            {
                var sub = new Subscription
                {
                    UserId = memberUser.Id,
                    PlanId = proPlan.Id,
                    StartDate = DateTime.UtcNow.AddDays(-5),
                    EndDate = DateTime.UtcNow.AddDays(25),
                    AutoRenew = true,
                    Status = SubscriptionStatus.Active
                };
                await context.Subscriptions.AddAsync(sub);
                await context.SaveChangesAsync();
            }
        }
    }
}
