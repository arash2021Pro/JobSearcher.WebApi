using JobSearcher.CoreDomains.BaseEntity;
using JobSearcher.CoreDomains.StorageDomains.SafetyPermissions;

namespace JobSearcher.CoreDomains.StorageDomains;

public class Permission:Core
{
    public string? PermissionName { get; set; }
    public ICollection<RolePermission>RolePermissions { get; set; }
}