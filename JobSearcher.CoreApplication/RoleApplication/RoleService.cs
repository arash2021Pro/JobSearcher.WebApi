using JobSearcher.CoreDomains.ReposistoryPattern;
using JobSearcher.CoreDomains.StorageDomains;
using JobSearcher.CoreDomains.StorageDomains.SafetyPermissions;
using Microsoft.EntityFrameworkCore;

namespace JobSearcher.CoreApplication.RoleApplication;

public class RoleService:IAccessPermission
{
    public DbSet<Role?> _Roles;
    public RoleService(IUnitOfWork work)
    {
        _Roles = work.Set<Role>();
    }
    public async Task InsertRoleAsync(Role? role)
    {
        await _Roles.AddAsync(role);
    }

    public async Task<bool> IsRoleContains(string rolename) => await _Roles.AnyAsync(r => r.Rolename == rolename);
    public int SearchRoleIdAsync(string rolename)
    {
        return  _Roles.FirstOrDefault(r => r.Rolename == rolename).id;
    }

    public async Task<Role?> SearchRoleByName(int roleId)
    {
        return await _Roles.FirstOrDefaultAsync(r => r.id == roleId);
    }
}