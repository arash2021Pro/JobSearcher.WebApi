using Hangfire;
using JobSearcher.ApiModels;
using JobSearcher.ApiModels.General;
using JobSearcher.ApiModels.Users;
using JobSearcher.CoreDomains.ApiDomains;
using JobSearcher.CoreDomains.ReposistoryPattern;
using JobSearcher.CoreDomains.StorageDomains;
using JobSearcher.CoreDomains.StorageDomains.Otp;
using JobSearcher.CoreDomains.StorageDomains.SafetyPermissions;
using JobSearcher.CoreStorage.Migrations;
using JobSearcher.CoreStructure;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace JobSearcher.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class UserController:ControllerBase
{
    public IUserService UserService;
    public IUnitOfWork Work;
    public IAccessPermission AccessPermission;
    public IOtpService OtpService;
    public IMapper Mapper;
    public IMessageService MessageService;
    public UserController(IUserService userService, IUnitOfWork work, IAccessPermission accessPermission, IOtpService otpService, IMapper mapper)
    {
        UserService = userService;
        Work = work;
        AccessPermission = accessPermission;
        OtpService = otpService;
        Mapper = mapper;
    }
    
    [HttpPost("Phonenumber,Password")]
    public async Task<JsonResult> Signup([FromBody] UserSignupModel? model)
    {
        var result = await UserService.IsContainsUserAsync(model.Phonenumber);
        var user = await UserService.GetUserAsync(model.Phonenumber);
        if(result && user.UserStatus != UserStatus.None) 
        {
            var alarm = new Alarm() {IsCompleted = false, Message ="این کاربر قبلا صبت نام کرده" , status = 201}; 
            var json = new JsonResult(alarm);
            return json;
        }
        var code = OtpService.GenerateOtpCodeAsync(6);
        var otp = new OTP();
        otp.code = code;
        if (user == null)
        {
            user = new User();
            var roleId =  AccessPermission.SearchRoleIdAsync(model.Rolename);
            user.RoleId = roleId ;
            user.UserStatus = UserStatus.None;
            Mapper.Map(model, user);
            await UserService.InsertUserAsync(user);
        }
        otp.User = user;
        await OtpService.InsertOtpAsync(otp);
        var row = await Work.SaveChangesAsync();
        if (row > 0)
        {
            //BackgroundJob.Enqueue(() => SendMessage("JobSearcher Group",model.Phonenumber,code));
            return await DisplayPhone(model.Phonenumber);
        }
        var alert = new Alarm() {Message = "خطا در پاسخگویی", IsCompleted = false, status = 500};
        var Json = new JsonResult(alert);
        return Json;
    }
    
    [HttpGet("phonenumber")]
    public async Task<JsonResult> DisplayPhone(string?phonenumber)
    {
        var phone = new JsonResult(phonenumber);
        return phone;
    }

    [HttpPost("code")]
    public async Task<JsonResult> Verify( [FromBody]string code)
    {
        var otp = await OtpService.GetOtpAsync(code);
        var user = await UserService.GetUserAsync(otp.userId);
        if (otp.IsAuthentic)
        {
            user.UserStatus = UserStatus.Active;
            otp.IsUsed = true;
            var row = await Work.SaveChangesAsync();
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

    [HttpPost("Phonenumber")]
    public async Task<JsonResult> fotgetPassword([FromBody]string Phonenumber)
    {
        var user = await UserService.GetUserAsync(Phonenumber);
        if (user == null)
        {
            return null;
        }
        var code = OtpService.GenerateOtpCodeAsync(5);
        var otp = new OTP();
        otp.userId = user.id;
        otp.code = code;
        await OtpService.InsertOtpAsync(otp);
        var row = await Work.SaveChangesAsync();
        if (row > 0)
        {
            // BackgroundJob.Enqueue(() => MessageService.SendMessage("JobSearcher", Phonenumber, code));
            var alert = new ForgetPassModel()
                {Message = "کد تایید برای شما مسیج شد ", IsCompleted = true, Status = 200};
            return new JsonResult(alert);
        }
        var json = new ForgetPassModel()
            {Message = "خطا در سرور", IsCompleted = true, Status = 200};
        return new JsonResult(json);
    }
    
    [HttpPost("code")]
    public async Task<JsonResult> VerifyCode([FromBody] string code)
    {
        var otp = await OtpService.GetOtpAsync(code);
        var user = await UserService.GetUserAsync(otp.userId);
        if (otp.IsAuthentic)
        {
            user.UserStatus = UserStatus.Active;
            otp.IsUsed = true;
            var row = await Work.SaveChangesAsync();
            if (row > 0)
            {
               var alarm = new VerifyAlarmModel() {Message = "کد حله شما میتوتنید رمز جدید انتخاب کنید", IsCompleted = true, Status = 200};
                return new JsonResult(alarm);
             
            }
            var alert= new VerifyAlarmModel() {Message = "خطا در سرور", IsCompleted = true, Status = 200};
            return new JsonResult(alert);
        }
        var message = new VerifyAlarmModel() {Message = "کد وارد شده اعتبار ندارد", IsCompleted = true, Status = 200};
        return new JsonResult(message);
    }
    [HttpPost("phonenumber,password")]
    public async Task<JsonResult> SetNewPassword([FromBody] SetPasswordModel? model)
    {
        var user = await UserService.GetUserAsync(model.Phonenumber);
        user.Password = model.Password;
        var row = await Work.SaveChangesAsync();
        if (row > 0)
        {
            var json = new VerifyAlarmModel()
                {Message = "رمز عبور با موفیقیت تغییر کرد", IsCompleted = true, Status = 200};
            return new JsonResult(json);
        }
        var Json = new VerifyAlarmModel()
            {Message = "خطا در سرور", IsCompleted = true, Status = 200};
        return new JsonResult(Json);
    }
    //Jwt ...
    public async Task<JsonResult> Login()
    {
        return null;
    }
    
    public async Task<JsonResult> Logout()
    {
        return null;
    }
}