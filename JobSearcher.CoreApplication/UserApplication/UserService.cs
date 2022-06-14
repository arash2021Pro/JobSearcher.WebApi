using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JobSearcher.CoreDomains.ApiDomains;
using JobSearcher.CoreDomains.ReposistoryPattern;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
 
namespace JobSearcher.CoreApplication.UserApplication;

public class UserService:IUserService
{
    public DbSet<User> _users;
    public UserService(IUnitOfWork work)
    {
        _users = work.Set<User>();
    }
    public async Task InsertUserAsync(User user) => await _users.AddAsync(user);

    public async Task<bool> IsContainsUserAsync(string Phonenumber) => await _users.AsNoTracking().AnyAsync(u => u.Phonenumber == Phonenumber);

    
    public async Task<User?> GetUserAsync(string phonenumber) =>
        await _users.AsNoTracking().FirstOrDefaultAsync(u => u.Phonenumber == phonenumber);
    public async Task<User?> GetUserAsync(int userId) => await _users.AsNoTracking().FirstOrDefaultAsync(u => u.id == userId);
    public async Task<User?> LoginAsync(string Phonenumber, string Password)=>await _users.FirstOrDefaultAsync(user => user.Phonenumber == Phonenumber && user.Password == Password);

}