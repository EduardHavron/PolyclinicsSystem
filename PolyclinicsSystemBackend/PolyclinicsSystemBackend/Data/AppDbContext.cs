using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PolyclinicsSystemBackend.Data.Entities;
using PolyclinicsSystemBackend.Data.Entities.Appointment;
using PolyclinicsSystemBackend.Data.Entities.Chat;
using PolyclinicsSystemBackend.Data.Entities.MedicalCard;
using PolyclinicsSystemBackend.Data.Entities.User;
using PolyclinicsSystemBackend.Dtos.Appointment;
using PolyclinicsSystemBackend.Dtos.Chat;

namespace PolyclinicsSystemBackend.Data
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Appointment> Appointments { get; set; }

        public DbSet<Message> Messages { get; set; }

        public DbSet<MedicalCard> MedicalCards { get; set; }

        public DbSet<Diagnose> Diagnoses { get; set; }

        public DbSet<Treatment> Treatments { get; set; }
    }
}