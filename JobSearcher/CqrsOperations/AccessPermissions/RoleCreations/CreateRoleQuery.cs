using JobSearcher.ApiModels.General;
using JobSearcher.ApiModels.Roles;
using JobSearcher.CoreDomains.ReposistoryPattern;
using JobSearcher.CoreDomains.StorageDomains;
using JobSearcher.CoreDomains.StorageDomains.SafetyPermissions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace JobSearcher.CqrsOperations.AccessPermissions.RoleCreations;

public class CreateRoleQuery:IRequest<JsonResult>
{
    public RoleCreationModel Model{get; set;}
}

public class CreateRoleHandler : IRequestHandler<CreateRoleQuery, JsonResult>
{
    private IUnitOfWork work;
    private IAccessPermission _permission;
    public CreateRoleHandler(IUnitOfWork work, IAccessPermission permission)
    {
        this.work = work;
        _permission = permission;
    }
    public async Task<JsonResult> Handle(CreateRoleQuery request, CancellationToken cancellationToken)
    {
        var result = await _permission.IsRoleContains(request.Model.Rolename);
        if(result){return new JsonResult(new Alarm(){Message = "نقش وجود داره",IsCompleted = false});}
        var role = new Role() {Rolename = request.Model.Rolename};
        await _permission.InsertRoleAsync(role);
        var row = await work.SaveChangesAsync();
        if (row > 0)
        {
            return new JsonResult(new Alarm() {IsCompleted = true, Message = "با موفقیت ثبت شد"});
        }
        return new JsonResult( new Alarm() {IsCompleted = false, Message = "خطا در پاسخگویی"});
    }
}