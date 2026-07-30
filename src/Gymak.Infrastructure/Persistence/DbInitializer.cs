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

        // 1. Seed Users if none exist
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

        // 2. Seed Subscription Plans if none exist
        if (!await context.SubscriptionPlans.AnyAsync())
        {
            var plans = new List<SubscriptionPlan>
            {
                new()
                {
                    Name = "Basic Fitness Access",
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

        // 3. Seed Muscle Groups & Exercises if none exist
        if (!await context.MuscleGroups.AnyAsync())
        {
            var chestGroup = new MuscleGroup { Name = "Chest (الصناية/الصدر)" };
            var backGroup = new MuscleGroup { Name = "Back (الظهر)" };
            var legsGroup = new MuscleGroup { Name = "Legs (الأرجل)" };
            var shouldersGroup = new MuscleGroup { Name = "Shoulders (الأكتاف)" };
            var bicepsGroup = new MuscleGroup { Name = "Biceps (البايسپس)" };
            var tricepsGroup = new MuscleGroup { Name = "Triceps (الترايسپس)" };
            var absGroup = new MuscleGroup { Name = "Abs & Core (البطن والوسط)" };
            var cardioGroup = new MuscleGroup { Name = "Cardio & Fitness (الكارديو)" };

            await context.MuscleGroups.AddRangeAsync(chestGroup, backGroup, legsGroup, shouldersGroup, bicepsGroup, tricepsGroup, absGroup, cardioGroup);
            await context.SaveChangesAsync();

            var exercises = new List<Exercise>
            {
                // Chest
                new() { MuscleGroupId = chestGroup.MuscleGroupId, Name = "Barbell Bench Press (بنش بالبار)", Description = "الاسم الأساسي لبناء وتضخيم عضلة الصدر المتوسطة والعلوية.", ExerciseType = ExerciseType.Strength },
                new() { MuscleGroupId = chestGroup.MuscleGroupId, Name = "Incline Dumbbell Press (تجميع بالدمبل عالي)", Description = "يركز بشكل مباشر على الجزء العلوي من عضلة الصدر (Upper Chest).", ExerciseType = ExerciseType.Strength },
                new() { MuscleGroupId = chestGroup.MuscleGroupId, Name = "Cable Flyes (تفتيح كابل)", Description = "يعطي انقباض مستمر وممتاز لحواف الصدر وتحديد العضلة.", ExerciseType = ExerciseType.Strength },

                // Back
                new() { MuscleGroupId = backGroup.MuscleGroupId, Name = "Lat Pulldown (سحب عالي للظهر)", Description = "تمرين أساسي لتوسيع عضلات المجانيص (Lats) وإعطاء شكل الـ V-Taper.", ExerciseType = ExerciseType.Strength },
                new() { MuscleGroupId = backGroup.MuscleGroupId, Name = "Barbell Bent-Over Row (سحب بار للظهر)", Description = "يبني كثافة وسمك عضلات الظهر المنتصف والعليا.", ExerciseType = ExerciseType.Strength },
                new() { MuscleGroupId = backGroup.MuscleGroupId, Name = "Deadlift (الديدليفت)", Description = "ملك تمارين القوة، يمرن السلسلة الخلفية كاملة من الظهر حتى الأرجل.", ExerciseType = ExerciseType.Strength },

                // Legs
                new() { MuscleGroupId = legsGroup.MuscleGroupId, Name = "Barbell Back Squat (اسكوات بالبار)", Description = "التمرين الأول لعضلات الفخذ الأمامي (Quads) والجلوس.", ExerciseType = ExerciseType.Strength },
                new() { MuscleGroupId = legsGroup.MuscleGroupId, Name = "Leg Press (مكواة الأرجل)", Description = "ممتاز لتحميل أوزان عالية بثبات وأمان على عضلات الرجل.", ExerciseType = ExerciseType.Strength },
                new() { MuscleGroupId = legsGroup.MuscleGroupId, Name = "Romanian Deadlift (ديدليفت روماني)", Description = "يركز على الهامسترينج (عضلات الفخذ الخلفية) وعضلات الجلوتس.", ExerciseType = ExerciseType.Strength },

                // Shoulders
                new() { MuscleGroupId = shouldersGroup.MuscleGroupId, Name = "Overhead Dumbbell Press (ضغط أكتاف بالدمبل)", Description = "لبناء الكتف الأمامي والجانبي وإعطاء مظهر متناسق وقوي.", ExerciseType = ExerciseType.Strength },
                new() { MuscleGroupId = shouldersGroup.MuscleGroupId, Name = "Lateral Raises (رفرفة جانبي)", Description = "التمرين الأهم لتعريض الكتف الجانبي وإعطاء شكل الكورة.", ExerciseType = ExerciseType.Strength },

                // Biceps
                new() { MuscleGroupId = bicepsGroup.MuscleGroupId, Name = "Barbell Curl (تبادل بالبار للباي)", Description = "التمرين الرائد لبناء حجم البايسپس وقوة الذراعين.", ExerciseType = ExerciseType.Strength },
                new() { MuscleGroupId = bicepsGroup.MuscleGroupId, Name = "Hammer Curls (شواك بالدمبل)", Description = "يركز على عضلة Brachialis ويزيد من سمك الذراع والساعد.", ExerciseType = ExerciseType.Strength },

                // Triceps
                new() { MuscleGroupId = tricepsGroup.MuscleGroupId, Name = "Triceps Cable Pushdown (دفع كابل للتراي)", Description = "يركز على الرأس الخارجي والجانبي للترايسپس.", ExerciseType = ExerciseType.Strength },
                new() { MuscleGroupId = tricepsGroup.MuscleGroupId, Name = "Skull Crushers (سكال كراشر بالبار EZ)", Description = "بناء الرأس الطويل للتراي لإعطاء حجم ضخم للذراع.", ExerciseType = ExerciseType.Strength },

                // Abs & Cardio
                new() { MuscleGroupId = absGroup.MuscleGroupId, Name = "Hanging Leg Raises (رفع الأرجل عقلة)", Description = "تقوية وتقسيم عضلات البطن السفلى والجذع.", ExerciseType = ExerciseType.Strength },
                new() { MuscleGroupId = cardioGroup.MuscleGroupId, Name = "Treadmill HIIT Cardio (مشاية سريع)", Description = "حرق دهون مكثف وتحسين الكفاءة القلبية والبدنية.", ExerciseType = ExerciseType.Cardio }
            };

            await context.Exercises.AddRangeAsync(exercises);
            await context.SaveChangesAsync();
        }
    }
}
