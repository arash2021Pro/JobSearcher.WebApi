using JobSearcher.ApiModels;
using JobSearcher.CoreDomains.ApiDomains;
using JobSearcher.CoreDomains.ReposistoryPattern;
using JobSearcher.CoreDomains.StorageDomains.Otp;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace JobSearcher.CqrsOperations.Users.ForgotPassword;

public class ForgetPasswordQuery:IRequest<JsonResult>
{
    public string Phonenumber { get; set; }
}

public class ForgetPasswordQueryHandler : IRequestHandler<ForgetPasswordQuery, JsonResult>
{
    private IUserService _userService;
    private IOtpService _otpService;
    private IUnitOfWork _work;
    private IMediator _mediator;
    public ForgetPasswordQueryHandler(IUserService userService, IOtpService otpService, IUnitOfWork work, IMediator mediator)
    {
        _userService = userService;
        _otpService = otpService;
        _work = work;
        _mediator = mediator;
    }
    public async Task<JsonResult> Handle(ForgetPasswordQuery request, CancellationToken cancellationToken)
    {
        var user = await _userService.GetUserAsync(request.Phonenumber);
        if (user == null)
        {
            return null;
        }
        var code = _otpService.GenerateOtpCodeAsync(5);
        var otp = new OTP();
        otp.userId = user.id;
        otp.code = code;
        await _otpService.InsertOtpAsync(otp);
        var row = await _work.SaveChangesAsync();
        if (row > 0)
        {
           // await _mediator.Publish(new SignupEventNotification() {Message = "کد تایید شما", Phonenumber = user.Phonenumber});
            var alert = new ForgetPassModel() {Message = "کد تایید برای شما مسیج شد ", IsCompleted = true, Status = 200};
            return new JsonResult(alert);
        }
        var json = new ForgetPassModel()
            {Message = "خطا در سرور", IsCompleted = true, Status = 500};
        return new JsonResult(json);   
    }
}