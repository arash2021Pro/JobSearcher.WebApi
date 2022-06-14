using JobSearcher.ApiModels.General;
using JobSearcher.ApiModels.Roles;
using JobSearcher.CoreDomains.ReposistoryPattern;
using JobSearcher.CoreDomains.StorageDomains;
using JobSearcher.CoreDomains.StorageDomains.SafetyPermissions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace JobSearcher.CqrsOperations.AccessPermissions.RoleEdition;

public class EditRoleQuery:IRequest<IActionResult>
{
    public RoleEditModel Model { get; set; }
}

public class EditRoleQueryHandler : IRequestHandler<EditRoleQuery, IActionResult>
{
    public IAccessPermission Permission;
    public IUnitOfWork Work;

    public EditRoleQueryHandler(IAccessPermission permission, IUnitOfWork work)
    {
        Permission = permission;
        Work = work;
    }

    public async Task<IActionResult> Handle(EditRoleQuery request, CancellationToken cancellationToken)
    {
        var role =await  Permission.SearchRoleByName(request.Model.id);
        role.Rolename = request.Model.Rolename;
        var row = await Work.SaveChangesAsync();
        if (row > 0)
        {
            return new OkResult();
        }
        return new BadRequestResult();

    }
}