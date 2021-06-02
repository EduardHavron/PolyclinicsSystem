using System;

namespace PolyclinicsSystemBackend.Dtos.MedicalCard
{
    public class TreatmentDto
    {
        public int TreatmentId { get; set; }

        public DateTime TreatmentDate { get; set; }

        public string TreatmentInstructions { get; set; }
    }
}