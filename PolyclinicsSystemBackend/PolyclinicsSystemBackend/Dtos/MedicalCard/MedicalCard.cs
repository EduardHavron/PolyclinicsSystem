using System.Collections.Generic;
using PolyclinicsSystemBackend.Data.Entities;

namespace PolyclinicsSystemBackend.Dtos.MedicalCard
{
    public class MedicalCard
    {
        public string MedicalCardId { get; set; }
        
        public virtual IList<Diagnose>? Diagnoses { get; set; }
        
        public virtual User User { get; set; }
    }
}