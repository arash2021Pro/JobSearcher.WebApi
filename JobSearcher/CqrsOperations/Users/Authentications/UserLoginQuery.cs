using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JobSearcher.CoreApplication.UserApplication;
using JobSearcher.CoreDomains.ApiDomains;
using JobSearcher.SysCore.JwtAuthenticationService;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace JobSearcher.CqrsOperations.Users.Authentications;

public class UserLoginQuery:IRequest<JsonResult>
{
    public BearerTokenOptions _Options;
    public UserLoginQuery(BearerTokenOptions Options)
    {
      
        _Options = Options;
    }
    public string Phonenumber { get; set; }
    public string Password { get; set; }
    public string GenerateJSONWebToken(User user)
    {
        var claims = new List<Claim>();
        claims.Add(new Claim(ClaimTypes.NameIdentifier, user.id.ToString()));
        claims.Add(new Claim(ClaimTypes.Name,user.Phonenumber));
        claims.Add(new Claim("Password",user.Password));
        switch (user.RoleId)
        {
            case 1:
                claims.Add(new Claim(ClaimTypes.Role,"admin"));
                break;
                
            case 2:
                claims.Add(new Claim(ClaimTypes.Role,"Bussiness"));
                break;
                
            case 3:
                claims.Add(new Claim(ClaimTypes.Role,"Applicant"));
                break;
        }
        var SecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_Options.Key));
        var credentials = new SigningCredentials(SecurityKey,SecurityAlgorithms.HmacSha256);
        var Token = new JwtSecurityToken(
            _Options.Issuer,
            _Options.Audience,
            claims,
            expires: DateTime.Now.AddMinutes(_Options.ExpireDuration),
            signingCredentials:credentials);
        return new JwtSecurityTokenHandler().WriteToken(Token);
    }
}
public class UserLoginHandler:IRequestHandler<UserLoginQuery,JsonResult>
{
    private IUserService _userService;


    public UserLoginHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<JsonResult> Handle(UserLoginQuery request, CancellationToken cancellationToken)
    {
        var user = await _userService.LoginAsync(request.Phonenumber, request.Password);
        if (user == null)
            return new JsonResult("Not found");
        var token = request.GenerateJSONWebToken(user);
        return new JsonResult(token);
    }
}