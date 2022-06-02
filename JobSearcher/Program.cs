using ElmahCore.Mvc;
using Hangfire;
using JobSearcher.SysCore.Application;
using JobSearcher.SysCore.Binders;
using JobSearcher.SysCore.Elmah;
using JobSearcher.SysCore.Hangfire;
using JobSearcher.SysCore.SqlServer;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.RespectBrowserAcceptHeader = true;

});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();
builder.Services.UseApplicationService();
builder.Services.UseSqlService(builder.Configuration);
builder.Services.UseHangfireService(builder.Configuration);
builder.Services.UseElmahService(builder.Configuration);
builder.Services.UseBindService(builder.Configuration);
builder.Services.AddMediatR(typeof(Program).Assembly);
var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseHangfireDashboard();
app.UseAuthorization();
app.UseElmah();
app.MapControllers();

app.Run();