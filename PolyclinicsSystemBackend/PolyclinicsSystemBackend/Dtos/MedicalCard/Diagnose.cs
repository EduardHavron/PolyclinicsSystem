namespace PolyclinicsSystemBackend.Dtos.MedicalCard
{
    public class Diagnose
    {
        public int DiagnoseId { get; set; }
        
        public int MedicalCardId { get; set; }
        
        public MedicalCard MedicalCard { get; set; }

        public Treatment? Treatment { get; set; }
    }
}