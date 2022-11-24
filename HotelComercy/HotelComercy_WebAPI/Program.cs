using HotelComercy_WebAPI.Data;
using HotelComercy_WebAPI.Repository;
using HotelComercy_WebAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    //options.ReturnHttpNotAcceptable = true;
}).AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Add Context
builder.Services.AddDbContext<ApiVillaContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection")));
//Add Simple log
Log.Logger = new LoggerConfiguration().MinimumLevel.Debug()
    .WriteTo.File("/log/villaLogs.txt", rollingInterval: RollingInterval.Day).CreateLogger();
builder.Host.UseSerilog();
//Add UnitOfWork
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

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
