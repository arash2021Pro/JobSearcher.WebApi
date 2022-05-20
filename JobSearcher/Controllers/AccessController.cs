using JobSearcher.ApiModels.General;
using JobSearcher.ApiModels.Roles;
using JobSearcher.CoreDomains.ReposistoryPattern;
using JobSearcher.CoreDomains.StorageDomains;
using JobSearcher.CoreDomains.StorageDomains.SafetyPermissions;
using Microsoft.AspNetCore.Mvc;
namespace JobSearcher.Controllers;
[ApiController]
[Route("api/[controller]/[action]")]
public class AccessController
{
    public IUnitOfWork Work;
    public IAccessPermission AccessPermission;
    public AccessController(IUnitOfWork work, IAccessPermission accessPermission)
    {
        Work = work;
        AccessPermission = accessPermission;
    }
    [HttpPost("Rolename")]
    public async Task<Alarm> CreateRole( [FromBody] RoleCreationModel model)
    {
        var result = await AccessPermission.IsRoleContains(model.Rolename);
        if(result){return new Alarm(){Message = "نقش وجود داره",IsCompleted = false};}
        var role = new Role() {Rolename = model.Rolename};
        await AccessPermission.InsertRoleAsync(role);
        var row = await Work.SaveChangesAsync();
        if (row > 0)
        {
            return new Alarm() {IsCompleted = true, Message = "با موفقیت ثبت شد"};
        }
        return  new Alarm() {IsCompleted = false, Message = "خطا در پاسخگویی"};
    }

}