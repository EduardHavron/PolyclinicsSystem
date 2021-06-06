namespace PolyclinicsSystemBackend.Dtos.Account.AuthorizedUser
{
    public class AuthorizedUser
    {
        public string Email { get; set; }
        
        public string Id { get; set; }
        
        public string[] Roles { get; set; }
        
        public string Token { get; set; }
    }
}