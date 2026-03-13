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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Mapeo de todas las tablas a minúsculas para compatibilidad con Linux/Render
        modelBuilder.Entity<UserAccount>().ToTable("useraccounts");
        modelBuilder.Entity<Worker>().ToTable("workers");
        modelBuilder.Entity<Resident>().ToTable("residents");
        modelBuilder.Entity<Role>().ToTable("roles");
        modelBuilder.Entity<Shift>().ToTable("shifts");
        modelBuilder.Entity<Incident>().ToTable("incidents");
        modelBuilder.Entity<VitalSign>().ToTable("vitalsigns");
        modelBuilder.Entity<Attendance>().ToTable("attendance");
        modelBuilder.Entity<DietType>().ToTable("diettypes");
        modelBuilder.Entity<ResidentDiet>().ToTable("residentdiets");

        // Configuración de precisión para decimales en VitalSign
        modelBuilder.Entity<VitalSign>(entity =>
        {
            entity.Property(e => e.GlucoseLevel).HasPrecision(18, 2);
            entity.Property(e => e.OxygenSaturation).HasPrecision(18, 2);
            entity.Property(e => e.Temperature).HasPrecision(18, 2);
            entity.Property(e => e.Weight).HasPrecision(18, 2);
        });
    }
}