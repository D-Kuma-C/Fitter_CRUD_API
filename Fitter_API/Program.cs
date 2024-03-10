using Fitter_API.Controllers;
using Fitter_API.Controllers.Repository;
using Fitter_API.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<FitterDbContext>(options =>
{
    options.UseNpgsql(System.Environment.GetEnvironmentVariable("FitterAPI_DatabaseConnectionString"));
});

builder.Services.AddControllers(p =>
{
    p.Filters.Add<HttpExceptionFilter>();
})
    .AddJsonOptions(o => { o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles; });

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IFitterRepository, FitterRepository>();
builder.Services.AddScoped<ISeniorFitterRepository, SeniorFitterRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
