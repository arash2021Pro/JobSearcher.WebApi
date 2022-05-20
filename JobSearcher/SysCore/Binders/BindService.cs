using JobSearcher.CoreStructure;

namespace JobSearcher.SysCore.Binders;

public static class BindService
{
    public static void UseBindService(this IServiceCollection service, IConfiguration configuration)
    {
        service.Configure<MessageOption>(o => configuration.GetSection("KaveNegar:ApiKey").Bind(o));
    }
}