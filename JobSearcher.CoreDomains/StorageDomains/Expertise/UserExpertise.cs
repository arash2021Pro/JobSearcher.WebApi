using JobSearcher.CoreDomains.ApiDomains;
using JobSearcher.CoreDomains.BaseEntity;

namespace JobSearcher.CoreDomains.StorageDomains.Expertise;

public class UserExpertise:Core
{
    public int userId { get; set; }
    public User User { get; set; }
    
    public int Gender { get; set; }
    public string? NationalSerial { get; set; }
    public string? FullName { get; set; }
    public string? JobField { get; set; }
    public int Age { get; set; }
    public string? Skills { get; set; }
    public string? HistoryTime { get; set; }
    public string ? Place { get; set; }
    public string ? Position { get; set; }
}