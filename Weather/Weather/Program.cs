using Microsoft.EntityFrameworkCore;
using Weather.Models;
using Weather.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();

var connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<WeatherContext>(options => options.UseSqlServer(connection));
builder.Services.AddMemoryCache();
builder.Services.AddTransient<IWorkWithFiles, WorkWithFiles>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
