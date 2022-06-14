using ElmahCore.Mvc;
using Hangfire;
using JobSearcher.CoreStorage.SqlContext;
using JobSearcher.SysCore.Application;
using JobSearcher.SysCore.Binders;
using JobSearcher.SysCore.Elmah;
using JobSearcher.SysCore.Hangfire;
using JobSearcher.SysCore.JwtAuthenticationService;
using JobSearcher.SysCore.SqlServer;
using MediatR;
using Microsoft.OpenApi.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.RespectBrowserAcceptHeader = true;

});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description =
            "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    }); 
});
builder.Services.UseApplicationService();
builder.Services.UseSqlService(builder.Configuration);
builder.Services.UseHangfireService(builder.Configuration);
builder.Services.UseElmahService(builder.Configuration);
builder.Services.UseBindService(builder.Configuration);
builder.Services.AddMediatR(typeof(Program).Assembly);
builder.Services.AddHttpContextAccessor();
builder.Host.UseSerilog();
builder.Services.AddResponseCompression(options => { options.EnableForHttps = true; });
builder.Services.UseJwtAuthenticationService(builder.Configuration);
builder.Services.AddHealthChecks().AddSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
builder.Services.AddHealthChecks().AddDbContextCheck<ApplicationContext>();

var app = builder.Build();
app.UseResponseCompression();
app.MapHealthChecks("/healths");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseHangfireDashboard();
app.UseAuthentication();
app.UseAuthorization();
app.UseElmah();
app.MapControllers();

app.Run();