using PolyclinicsSystemBackend.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PolyclinicsSystemBackend.Dtos.Appointment;
using PolyclinicsSystemBackend.Dtos.Chat;
using PolyclinicsSystemBackend.Dtos.MedicalCard;

namespace PolyclinicsSystemBackend.Data
{
  public class AppDbContext : IdentityDbContext<User>
  {
    public DbSet<Appointment> Appointments {get; set; }

    public DbSet<Message> Messages { get; set; }
    
    public DbSet<MedicalCard> MedicalCards { get; set; }
    
    public DbSet<Diagnose> Diagnoses { get; set; }
    
    public DbSet<Treatment> Treatments { get; set; }
    public AppDbContext(DbContextOptions options) : base(options)
    {
      
    }
  }
}