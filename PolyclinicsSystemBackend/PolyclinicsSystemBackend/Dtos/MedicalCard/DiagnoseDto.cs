using System;
namespace PolyclinicsSystemBackend.Dtos.MedicalCard
{
    public class DiagnoseDto
    {
        public int DiagnoseId { get; set; }
        
        public string DiagnoseInfo { get; set; }

        public DateTime DiagnoseDate { get; set; }

        public TreatmentDto? Treatment { get; set; }
    }
}