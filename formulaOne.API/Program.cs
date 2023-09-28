using formulaOne.DataService.Context;
using formulaOne.DataService.Repositories.Implementations;
using formulaOne.DataService.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
string appDbConnectionString = builder.Configuration.GetConnectionString("AppDb")!;
string redisDbConnectionString = builder.Configuration.GetConnectionString("RedisDb")!;

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetc
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<APIDbContext>(options => options.UseNpgsql(appDbConnectionString));
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = redisDbConnectionString;
    options.InstanceName = "FormulaOneAPI_";

});

builder.Services.AddScoped<IDriverRepository, DriverRepository>();
builder.Services.AddScoped<IAchievementRepository, AchievementRepository>();

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
