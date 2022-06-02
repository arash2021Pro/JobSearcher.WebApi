using JobSearcher.ApiModels.Users;
using JobSearcher.CoreDomains.ApiDomains;
using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace JobSearcher.CqrsOperations.Users.GetUserThroughParam;

public class GetUserQueryByPhone:IRequest<JsonResult>
{
    public string Phonenumber { get; set; }
}
public class GetUserQueryByIdHandler : IRequestHandler<GetUserQueryByPhone, JsonResult>
{
    private IUserService _userService;
    private IMapper _mapper;
    public GetUserQueryByIdHandler(IUserService userService, IMapper mapper)
    {
        _userService = userService;
        _mapper = mapper;
    }
    public async Task<JsonResult> Handle(GetUserQueryByPhone request, CancellationToken cancellationToken)
    {
        var user = await _userService.GetUserAsync(request.Phonenumber);
        var User = user.Adapt<UserSignupModel>();
        return new JsonResult(User);
    }
}