namespace PolyclinicsSystemBackend.Config.Options
{
    public class AuthOptions
    {
        public string Key { get; set; } = null!;
        public string Issuer { get; set; } = null!;
        public int ExpireDays { get; set; }
    }
}