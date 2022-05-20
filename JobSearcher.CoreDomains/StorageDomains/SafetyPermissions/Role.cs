using JobSearcher.CoreDomains.ApiDomains;
using JobSearcher.CoreDomains.BaseEntity;

namespace JobSearcher.CoreDomains.StorageDomains.SafetyPermissions;

public class Role:Core
{
    public string? Rolename { get; set; }
    public ICollection<User>Users { get; set; }
    public ICollection<RolePermission>RolePermissions { get; set; }
}