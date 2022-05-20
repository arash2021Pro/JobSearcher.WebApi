namespace JobSearcher.CoreDomains.ApiDomains;

public interface IUserService
{
    Task InsertUserAsync(User user);
    Task<bool> IsContainsUserAsync(string Phonenumber);
    Task<User?> GetUserAsync(string phonenumber);
    Task<User?> GetUserAsync(int userId);
}