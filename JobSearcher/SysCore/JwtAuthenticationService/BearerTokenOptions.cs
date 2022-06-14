namespace JobSearcher.SysCore.JwtAuthenticationService;

public class BearerTokenOptions
{
    public string Key { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public int ExpireDuration { get; set; }
}