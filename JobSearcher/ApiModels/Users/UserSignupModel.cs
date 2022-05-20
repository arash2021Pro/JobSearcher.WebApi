using System.ComponentModel.DataAnnotations;
using DNTPersianUtils.Core;

namespace JobSearcher.ApiModels.Users;

public class UserSignupModel
{
 
   
    public string Phonenumber { get; set; }
    public string Password { get; set; }
    public string Rolename { get; set; }
}