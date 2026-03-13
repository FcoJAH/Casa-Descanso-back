using CasaDescanso.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CasaDescanso.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Shift> Shifts => Set<Shift>();
    public DbSet<Worker> Workers => Set<Worker>();
    public DbSet<UserAccount> UserAccounts => Set<UserAccount>();
    public DbSet<Resident> Residents => Set<Resident>();
    public DbSet<Attendance> Attendances => Set<Attendance>();
    public DbSet<Incident> Incidents => Set<Incident>();
    public DbSet<VitalSign> VitalSigns => Set<VitalSign>();
    public DbSet<DietType> DietTypes => Set<DietType>();
    public DbSet<ResidentDiet> ResidentDiets => Set<ResidentDiet>();
}
