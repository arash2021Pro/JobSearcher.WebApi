using JobSearcher.ApiModels.General;
using JobSearcher.CoreDomains.ApiDomains;
using JobSearcher.CoreDomains.ReposistoryPattern;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace JobSearcher.CqrsOperations.Users.SetNewPassowrd;

public class NewPasswordQuery:IRequest<JsonResult>
{
   public SetPasswordModel Model { get; set; }
}

public class NewPasswordQueryHandler : IRequestHandler<NewPasswordQuery, JsonResult>
{
   private IUserService _userService;
   private IUnitOfWork _work;
   public NewPasswordQueryHandler(IUserService userService, IUnitOfWork work)
   {
      _userService = userService;
      _work = work;
   }
   public async Task<JsonResult> Handle(NewPasswordQuery request, CancellationToken cancellationToken)
   {
      var user = await _userService.GetUserAsync(request.Model.Phonenumber);
      user.Password = request.Model.Password;
      var row = await _work.SaveChangesAsync();
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
   
}
