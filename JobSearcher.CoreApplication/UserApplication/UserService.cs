using JobSearcher.CoreDomains.ApiDomains;
using JobSearcher.CoreDomains.ReposistoryPattern;
using Microsoft.EntityFrameworkCore;

namespace JobSearcher.CoreApplication.UserApplication;

public class UserService:IUserService
{
    public DbSet<User> _users;
    public UserService(IUnitOfWork work)
    {
        _users = work.Set<User>();
    }
    public async Task InsertUserAsync(User user) => await _users.AddAsync(user);

    public async Task<bool> IsContainsUserAsync(string Phonenumber) => await _users.AnyAsync(u => u.Phonenumber == Phonenumber);

    public async Task<User?> GetUserAsync(string phonenumber) =>
        await _users.FirstOrDefaultAsync(u => u.Phonenumber == phonenumber);
    public async Task<User?> GetUserAsync(int userId) => await _users.FirstOrDefaultAsync(u => u.id == userId);
    
}