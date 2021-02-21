namespace ApplicationHealthChecker.Models.Settings
{
    public record BasicAuthSettings
    {
        public string Username { get; init; } = default!;
        public string Password { get; init; } = default!;
        public bool RequireAuth { get; init; }
        public bool Enabled { get; init; }
    }
}