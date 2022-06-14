using JobSearcher.CoreApplication.ExpertisesApplication;
using JobSearcher.CoreApplication.OtpApplication;
using JobSearcher.CoreApplication.RoleApplication;
using JobSearcher.CoreApplication.UserApplication;
using JobSearcher.CoreDomains.ApiDomains;
using JobSearcher.CoreDomains.ReposistoryPattern;
using JobSearcher.CoreDomains.StorageDomains;
using JobSearcher.CoreDomains.StorageDomains.Expertise;
using JobSearcher.CoreDomains.StorageDomains.Otp;
using JobSearcher.CoreStorage.SqlContext;
using JobSearcher.CoreStructure;
using MapsterMapper;
using MediatR;

namespace JobSearcher.SysCore.Application;

public static class ApplicationService
{
    public static void UseApplicationService(this IServiceCollection service)
    {
        service.AddScoped<IUnitOfWork, ApplicationContext>();
        service.AddScoped<IMapper, Mapper>();
        service.AddScoped<IUserService, UserService>();
        service.AddScoped<IAccessPermission,RoleService>();
        service.AddScoped<IOtpService, OtpService>();
        service.AddScoped<IMessageService,MessageService>();
        service.AddScoped<IUserExpertise, ExpertiseService>();

    }
}