using JobSearcher.ApiModels.General;
using JobSearcher.CoreApplication.OtpApplication;
using JobSearcher.CoreApplication.UserApplication;
using JobSearcher.CoreDomains.ApiDomains;
using JobSearcher.CoreDomains.ReposistoryPattern;
using JobSearcher.CoreDomains.StorageDomains.Otp;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace JobSearcher.CqrsOperations.Users.OptVerification;

public class VerifyQuery:IRequest<JsonResult>
{
    public string code { get; set; }
}

public class VerifyOtpHandler : IRequestHandler<VerifyQuery, JsonResult>
{
    private IUserService _userService;
    private IUnitOfWork _work;
    private IOtpService _otpService;
    public VerifyOtpHandler(IUserService userService, IUnitOfWork work, IOtpService otpService)
    {
        _userService = userService;
        _work = work;
        _otpService = otpService;
    }

    public async Task<JsonResult> Handle(VerifyQuery request, CancellationToken cancellationToken)
    {
        var otp = await _otpService.GetOtpAsync(request.code);
        var user = await _userService.GetUserAsync(otp.userId);
        if (otp.IsAuthentic)
        {
            user.UserStatus = UserStatus.Active;
            otp.IsUsed = true;
            var row = await _work.SaveChangesAsync();
            if (row > 0)
            {
                var alarm = new VerifyAlarmModel() {Message = "حساب شما با موفقیت وریفای شد", IsCompleted = true, Status = 200};
                return new JsonResult(alarm);
            }
            var alert= new VerifyAlarmModel() {Message = "خطا در سرور", IsCompleted = true, Status = 200};
            return new JsonResult(alert);
        }
        var message = new VerifyAlarmModel() {Message = "کد وارد شده اعتبار ندارد", IsCompleted = true, Status = 200};
        return new JsonResult(message);
    }
}