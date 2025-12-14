using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using FitPulse.Models;

namespace FitPulse.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<Gym> Gyms => Set<Gym>();
    public DbSet<Service> Services => Set<Service>();
    public DbSet<Trainer> Trainers => Set<Trainer>();
    public DbSet<TrainerService> TrainerServices => Set<TrainerService>();
    public DbSet<TrainerAvailability> TrainerAvailabilities => Set<TrainerAvailability>();
    public DbSet<Appointment> Appointments => Set<Appointment>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // TrainerServices (Many-to-Many)
        builder.Entity<TrainerService>()
            .HasKey(ts => new { ts.TrainerId, ts.ServiceId });

        builder.Entity<TrainerService>()
            .HasOne(ts => ts.Trainer)
            .WithMany(t => t.TrainerServices)
            .HasForeignKey(ts => ts.TrainerId)
            .OnDelete(DeleteBehavior.Restrict);   // ✅ مهم

        builder.Entity<TrainerService>()
            .HasOne(ts => ts.Service)
            .WithMany()
            .HasForeignKey(ts => ts.ServiceId)
            .OnDelete(DeleteBehavior.Restrict);   // ✅ مهم

        // Appointments (منع multiple cascade paths)
        builder.Entity<Appointment>()
            .HasOne(a => a.Trainer)
            .WithMany()
            .HasForeignKey(a => a.TrainerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Appointment>()
            .HasOne(a => a.Service)
            .WithMany()
            .HasForeignKey(a => a.ServiceId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
