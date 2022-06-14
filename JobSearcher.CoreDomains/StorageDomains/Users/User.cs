using JobSearcher.CoreDomains.BaseEntity;
using JobSearcher.CoreDomains.StorageDomains.Expertise;
using JobSearcher.CoreDomains.StorageDomains.Otp;
using JobSearcher.CoreDomains.StorageDomains.SafetyPermissions;

namespace JobSearcher.CoreDomains.ApiDomains;

public class User:Core
{
    public string Phonenumber { get; set; }
    public string Password { get; set; }
    public UserStatus UserStatus { get; set; }
    public bool IsRulesAccepted { get; set; }
    public int RoleId { get; set; }
    public Role Role { get; set; }
    public ICollection<OTP>Otps { get; set; }
    public ICollection<UserExpertise>UserExpertises { get; set; }
}