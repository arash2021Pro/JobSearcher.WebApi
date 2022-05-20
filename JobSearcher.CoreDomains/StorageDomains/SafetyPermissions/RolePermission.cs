using JobSearcher.CoreDomains.BaseEntity;

namespace JobSearcher.CoreDomains.StorageDomains.SafetyPermissions;

public class RolePermission:Core
{
    public int RoleId { get; set; }
    public Role Role { get; set; }
    public int PermissionId { get; set; }
    public Permission Permission { get; set; }
}