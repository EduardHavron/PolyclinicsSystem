using System;

namespace PolyclinicsSystemBackend.Data.Entities.MedicalCard
{
    public class Diagnose
    {
        public int DiagnoseId { get; set; }
        public string DiagnoseInfo { get; set; }

        public DateTime DiagnoseDate { get; set; }

        public Treatment? Treatment { get; set; }

        public int MedicalCardId { get; set; }

        public MedicalCard MedicalCard { get; set; }
    }
}