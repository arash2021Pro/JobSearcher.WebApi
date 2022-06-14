using JobSearcher.CoreDomains.ReposistoryPattern;
using JobSearcher.CoreDomains.StorageDomains.Expertise;
using Microsoft.EntityFrameworkCore;

namespace JobSearcher.CoreApplication.ExpertisesApplication;

public class ExpertiseService:IUserExpertise
{
    public DbSet<UserExpertise> UserExpertises;

    public ExpertiseService(IUnitOfWork work)
    {
        UserExpertises = work.Set<UserExpertise>();
    }

    public async Task AddNewExpertiseAsync(UserExpertise expertise) => await UserExpertises.AddAsync(expertise);


}