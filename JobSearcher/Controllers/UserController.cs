using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
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
using JobSearcher.CqrsOperations.Users;
using JobSearcher.CqrsOperations.Users.Authentications;
using JobSearcher.CqrsOperations.Users.ForgotPassword;
using JobSearcher.CqrsOperations.Users.GetUserThroughParam;
using JobSearcher.CqrsOperations.Users.OptVerification;
using JobSearcher.CqrsOperations.Users.SetNewPassowrd;
using JobSearcher.CqrsOperations.Users.UserEdit;
using JobSearcher.SysCore.JwtAuthenticationService;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
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
    public ISender _mediator;
    public BearerTokenOptions _Options;
    public UserController(IUserService userService, IUnitOfWork work,IOptionsSnapshot<BearerTokenOptions>bearerOptions, IAccessPermission accessPermission, IOtpService otpService, IMapper mapper, ISender sender, ISender mediator)
    {
        UserService = userService;
        Work = work;
        AccessPermission = accessPermission;
        OtpService = otpService;
        Mapper = mapper;
        _mediator = mediator;
        _Options = bearerOptions.Value;
    }
        
    [HttpPost("Phonenumber,Password")]
    public async Task<JsonResult> Signup([FromBody] UserSignupModel? model)
    {
        var json = await _mediator.Send(new SignupQuery() {Model = model});
        return json;
    }
    //Jwt ...
    [HttpPost]
    public async Task<JsonResult> Login([FromBody] UserLoginModel model)
    {
        var json = await _mediator.Send(new UserLoginQuery(_Options){Password = model.Password,Phonenumber = model.Phonenumber});
        return json;
    }
    
    [HttpPost("code")]
    public async Task<JsonResult> Verify( [FromBody]string code)
    {
        var json = await _mediator.Send(new VerifyQuery() {code = code});
        return json;
    }
    
    [HttpPost("Phonenumber")]
    public async Task<JsonResult> fotgetPassword([FromBody]string Phonenumber)
    {
        var json = await _mediator.Send(new ForgetPasswordQuery() {Phonenumber = Phonenumber});
        return json;
    }
    
    [HttpPost("code")]
    public async Task<JsonResult> VerifyCode([FromBody] string code)
    {
        var json = await _mediator.Send(new VerifyQuery() {code = code});
        return json;
    }
    
    [HttpPost("phonenumber,password")]
    public async Task<JsonResult> SetNewPassword([FromBody] SetPasswordModel? model)
    {
        var json = await _mediator.Send(new NewPasswordQuery() {Model = model});
        return json;
    }
    
    [HttpGet("Phonenumber")]
    [Authorize]
    public async Task<JsonResult> GetUser(string? Phonenumber)
    {
        var json = await _mediator.Send(new GetUserQueryByPhone() {Phonenumber = Phonenumber});
        return json;
    }

    [HttpPost("Phonenumber,Password")]
    public async Task<JsonResult> EditUser([FromBody] UserEditModel model)
    {
        var json = await _mediator.Send(new UserEditQuery() {Model = model});
        return json;
    }
    

 












}