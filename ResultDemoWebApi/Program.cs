using Microsoft.EntityFrameworkCore;
using ResultDemoWebApi.Controllers;
using ResultDemoWebApi.Data;
using ResultDemoWebApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ResultDemoWebApiContext>(options =>
    options.UseSqlServer(
        connectionString: builder.Configuration.GetConnectionString(
            name: "ResultDemoWebApiContext")
                ?? throw new InvalidOperationException(
                    message: "Connection string 'ResultDemoWebApiContext' not found.")));

// Add services to the container.
builder.Services.AddTransient<ResultDemoWebApiContext>();
builder.Services.AddTransient<WeatherForecastService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

app.MapWeatherForecastEndpoints();

app.Run();
