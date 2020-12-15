namespace HealthChecker.Models.Settings
{
    public class BasicAuthSettings
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public bool RequireAuth { get; set; }
        public bool Enabled { get; set; }
    }
}