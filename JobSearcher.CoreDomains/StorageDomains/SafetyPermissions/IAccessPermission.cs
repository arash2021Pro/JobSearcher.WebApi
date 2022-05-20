using JobSearcher.CoreDomains.StorageDomains.SafetyPermissions;

namespace JobSearcher.CoreDomains.StorageDomains;

public interface IAccessPermission
{
    Task InsertRoleAsync(Role? role);
    Task<bool> IsRoleContains(string rolename);
    public int SearchRoleIdAsync(string rolename);
}