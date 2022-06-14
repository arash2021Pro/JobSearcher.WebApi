using JobSearcher.CoreStorage.SqlContext;
using Microsoft.EntityFrameworkCore;

namespace JobSearcher.SysCore.SqlServer;

public static class SqlService
{
    public static void UseSqlService(this IServiceCollection service, IConfiguration configuration)
    {
        var StorageConnection = configuration.GetConnectionString("DefaultConnection");
        service.AddDbContextPool<ApplicationContext>(option =>
        {
            option.UseSqlServer(StorageConnection, x => x.UseNodaTime()); 
            option.AddInterceptors();
            option.LogTo(Console.WriteLine);
        });
    }
}