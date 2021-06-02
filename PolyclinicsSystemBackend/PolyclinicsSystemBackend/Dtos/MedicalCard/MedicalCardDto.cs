using System.Collections.Generic;
using PolyclinicsSystemBackend.Data.Entities.MedicalCard;
using PolyclinicsSystemBackend.HelperEntities;

namespace PolyclinicsSystemBackend.Dtos.MedicalCard
{
    public class MedicalCardDto
    {
        public int MedicalCardId { get; set; }

        public string? UserId { get; set; }

        public string? AdditionalInfo { get; set; }

        public Genders? Gender { get; set; }

        public int? Height { get; set; }

        public double? Weight { get; set; }

        public int? Age { get; set; }

        public List<DiagnoseDto>? Diagnoses { get; set; }
    }
}