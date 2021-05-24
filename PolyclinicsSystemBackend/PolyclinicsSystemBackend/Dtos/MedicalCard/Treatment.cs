﻿namespace PolyclinicsSystemBackend.Dtos.MedicalCard
{
    public class Treatment
    {
        public int TreatmentId { get; set; }
        
        public int DiagnoseId { get; set; }
        
        public Diagnose Diagnose { get; set; }
        
        public string? TreatmentInstructions { get; set; }
    }
}