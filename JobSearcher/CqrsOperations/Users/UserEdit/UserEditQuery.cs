using System.Globalization;
using JobSearcher.ApiModels.General;
using JobSearcher.ApiModels.Users;
using JobSearcher.CoreDomains.ApiDomains;
using JobSearcher.CoreDomains.ReposistoryPattern;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace JobSearcher.CqrsOperations.Users.UserEdit;

public class UserEditQuery:IRequest<JsonResult>
{
    public UserEditModel Model { get; set; }
}

public class UserEditQueryHandler : IRequestHandler<UserEditQuery, JsonResult>
{
    public IUnitOfWork Work;
    public IMapper Mapper;
    public IUserService UserService;
   
    public UserEditQueryHandler(IUnitOfWork work, IMapper mapper, IUserService userService)
    {
        Work = work;
        Mapper = mapper;
        UserService = userService;
    }

    public async Task<JsonResult> Handle(UserEditQuery request, CancellationToken cancellationToken)
    {
        var user = await UserService.GetUserAsync(request.Model.Phonenumber);
        user.Password = request.Model.Password;
       // user.Phonenumber = request.Model.Phonenumber;
        var row = await Work.SaveChangesAsync();
        if (row > 0)
        {
            return new JsonResult(new Alarm(){status = 200,Message = "Successful",IsCompleted = true});
        }
        return new JsonResult(new Alarm(){status = 201,Message = "damn",IsCompleted = false});
    }
}