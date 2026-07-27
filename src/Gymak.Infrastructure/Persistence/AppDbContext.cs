using Gymak.Application.Common.Interfaces;
using Gymak.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Gymak.Infrastructure.Persistence;

public class AppDbContext : DbContext, IApplicationDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Member> Members => Set<Member>();
    public DbSet<User> Users => Set<User>();
    public DbSet<MemberProfile> MemberProfiles => Set<MemberProfile>();
    public DbSet<TrainerClient> TrainerClients => Set<TrainerClient>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
