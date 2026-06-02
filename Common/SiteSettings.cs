namespace Common
{
    public class SiteSettings
    {
        public string ElmahPath { get; set; }
        public JwtSettings JwtSettings { get; set; }
        public IdentitySetting IdentitySetting { get; set; } // PascalCase

    }
}
