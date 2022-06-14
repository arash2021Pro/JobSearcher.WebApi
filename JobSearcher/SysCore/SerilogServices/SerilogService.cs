using Serilog;

namespace JobSearcher.SysCore.SerilogServices;

public static class SerilogService
{
    public static void UseSerilogService(this IHostBuilder hostBuilder, IConfiguration configuration)
    {
        string SerilogUrl = configuration.GetConnectionString(("Serilog"));
        hostBuilder.UseSerilog((context, config) =>
        {
            config.WriteTo.MSSqlServer(SerilogUrl, "Logs", autoCreateSqlTable: true).MinimumLevel.Information();
        });
    }
}