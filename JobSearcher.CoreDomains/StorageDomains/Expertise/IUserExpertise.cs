namespace JobSearcher.CoreDomains.StorageDomains.Expertise;

public interface IUserExpertise
{
    Task AddNewExpertiseAsync(UserExpertise expertise);
}