using JobSearcher.ApiModels.General;
using JobSearcher.ApiModels.Roles;
using JobSearcher.CoreDomains.ReposistoryPattern;
using JobSearcher.CoreDomains.StorageDomains;
using JobSearcher.CoreDomains.StorageDomains.SafetyPermissions;
using JobSearcher.CqrsOperations.AccessPermissions.RoleCreations;
using JobSearcher.CqrsOperations.AccessPermissions.RoleEdition;
using MediatR;
using Microsoft.AspNetCore.Mvc;
namespace JobSearcher.Controllers;
[ApiController]
[Route("api/[controller]/[action]")]
public class AccessController
{
    public IUnitOfWork Work;
    public IAccessPermission AccessPermission;
    public ISender mediator;
    public AccessController(IUnitOfWork work, IAccessPermission accessPermission, ISender mediator)
    {
        Work = work;
        AccessPermission = accessPermission;
        this.mediator = mediator;
    }
    [HttpPost("Rolename")]
    public async Task<JsonResult> CreateRole( [FromBody] RoleCreationModel model)
    {
        var json = await mediator.Send(new CreateRoleQuery() {Model = model});
        return json;
    }

    [HttpPost("Rolename")]
    public async Task<IActionResult> EditRole([FromBody] RoleEditModel model)
    {
        var result = await mediator.Send(new EditRoleQuery() {Model = model});
        return result;
    }


}