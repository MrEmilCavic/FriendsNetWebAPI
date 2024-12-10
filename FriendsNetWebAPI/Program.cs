using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using FriendsNetWebAPI.Models;
using System.Text;
using dotenv.net;


var builder = WebApplication.CreateBuilder(args);

DotEnv.Load();

builder.Configuration.AddEnvironmentVariables();

var dbConnectionString = $"Data Source={builder.Configuration["DB_DATA_SOURCE"]};Initial Catalog={builder.Configuration["DB_INITIAL_CATALOG"]};Persist Security Info=True;User ID={builder.Configuration["DB_USER_ID"]};Password={builder.Configuration["DB_PASSWORD"]};Multiple Active Result Sets=True;Trust Server Certificate=True";

// Add services to the container.
builder.Services.AddDbContext<FriendsNetContext>(options => options.UseSqlServer(dbConnectionString));
builder.Services.AddScoped<TokenService>();

builder.Services.AddAuthorization();

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", builder =>
    {
        builder.WithOrigins("http://localhost:3000", "https://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JWT_ISSUER"],
            ValidAudience = builder.Configuration["JWT_AUDIENCE"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT_SECRET"]))
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowReactApp");

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();