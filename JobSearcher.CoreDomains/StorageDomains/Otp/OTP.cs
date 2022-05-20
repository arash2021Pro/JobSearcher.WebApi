using JobSearcher.CoreDomains.ApiDomains;
using JobSearcher.CoreDomains.BaseEntity;

namespace JobSearcher.CoreDomains.StorageDomains.Otp;

public class OTP:Core
{
    public const int expirelimit = 15;

    public OTP()
    {
        expiretime = DateTimeOffset.Now.AddMinutes(expirelimit);
    }
    public int userId { get; set; }
    public User User { get; set; }
    public string code { get; set; }
    public bool IsUsed { get; set; }
    public DateTimeOffset expiretime { get; set; }
    public bool IsAuthentic => !IsUsed && DateTimeOffset.Now < expiretime;
}