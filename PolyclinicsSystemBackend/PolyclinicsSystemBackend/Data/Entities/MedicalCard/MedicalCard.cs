using System.Collections.Generic;

namespace PolyclinicsSystemBackend.Data.Entities.MedicalCard
{
    public class MedicalCard
    {
        public int MedicalCardId { get; set; }

        public string UserId { get; set; }

        public User User { get; set; }

        public string? AdditionalInfo { get; set; }

        public Genders? Gender { get; set; }

        public int? Height { get; set; }

        public double? Weight { get; set; }

        public int? Age { get; set; }

        public List<Diagnose> Diagnoses { get; set; }
    }
}