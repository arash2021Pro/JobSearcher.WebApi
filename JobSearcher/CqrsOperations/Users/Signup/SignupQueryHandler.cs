using JobSearcher.ApiModels.General;
using JobSearcher.ApiModels.Users;
using JobSearcher.CoreDomains.ApiDomains;
using JobSearcher.CoreDomains.ReposistoryPattern;
using JobSearcher.CoreDomains.StorageDomains;
using JobSearcher.CoreDomains.StorageDomains.Otp;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace JobSearcher.CqrsOperations.Users;

public class SignupQuery:IRequest<JsonResult>
{
    public UserSignupModel Model { get; set; }
    public JsonResult DisplayPhone(string? Phonenumber) => new(Phonenumber);
}

public class SignupQueryHandler : IRequestHandler<SignupQuery, JsonResult>
{
    private IUserService _userService;
    private IOtpService _otpService;
    private IAccessPermission AccessPermission;
    private IUnitOfWork work;
    private IMapper _mapper;
    private IMediator _mediator;
    public SignupQueryHandler(IUserService userService, IOtpService otpService, IAccessPermission accessPermission, IUnitOfWork work, IMapper mapper, IMediator mediator)
    {
        _userService = userService;
        _otpService = otpService;
        AccessPermission = accessPermission;
        this.work = work;
        _mapper = mapper;
        _mediator = mediator;
    }

    public async Task<JsonResult> Handle(SignupQuery request, CancellationToken cancellationToken)
    {
        var result = await _userService.IsContainsUserAsync(request.Model.Phonenumber);
        var user = await _userService.GetUserAsync(request.Model.Phonenumber);
        if(result && user.UserStatus != UserStatus.None) 
        {
            var alarm = new Alarm() {IsCompleted = false, Message ="این کاربر قبلا صبت نام کرده" , status = 201}; 
            var json = new JsonResult(alarm);
            return json;
        }
        var code = _otpService.GenerateOtpCodeAsync(6);
        var otp = new OTP();
        otp.code = code;
        if (user == null)
        {
            user = new User();
            var roleId =  AccessPermission.SearchRoleIdAsync(request.Model.Rolename);
            user.RoleId = roleId ;
            user.UserStatus = UserStatus.None;
            _mapper.Map(request, user);
            await _userService.InsertUserAsync(user);
        }
        otp.User = user;
        await _otpService.InsertOtpAsync(otp);
        var row = await work.SaveChangesAsync();
        if (row > 0 )
        {
          //  await  _mediator.Publish(new SignupEventNotification() {Message = "welcome to app", Phonenumber = user.Phonenumber});
            return request.DisplayPhone(user.Phonenumber);
        }
        var alert = new Alarm() {Message = "خطا در پاسخگویی", IsCompleted = false, status = 500};
        var Json = new JsonResult(alert);
        return Json;
    }
}