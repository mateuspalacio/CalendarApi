using AutoMapper;
using Calendar.Domain.Credentials;
using Calendar.Domain.Models.DTO;
using Calendar.Domain.Services;
using Calendar.Domain.Services.ServicesImpl;
using Calendar.Middleware;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.CustomSchemaIds(type => type.FullName);
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Version = "v1",
        Title = "Google Calendar API Scheduler",
        Description = "An ASP.NET Core Web API for scheduling and viewing events on Google Calendar"
    });
});
builder.Services.AddSingleton<IGoogleCalendarService, GoogleCalendarService>();
builder.Services.AddSingleton<Credentials>();
builder.Services.AddAutoMapper(typeof(Calendar.Domain.AutoMapper).Assembly);
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Google Calendar API Scheduler v1");
    });
}

app.UseHttpsRedirection();

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
