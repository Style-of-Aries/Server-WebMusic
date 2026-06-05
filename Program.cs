using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using MyApi.Data;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.IdentityModel.Tokens;
using MyApi.Repositories;
using MyApi.Services;

using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 100 * 1024 * 1024; // Cho phép 100MB
});

var jwtKey =
builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key is missing in appsettings.json");

var key = Encoding.UTF8.GetBytes(jwtKey);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)

.AddJwtBearer(options =>
{

    options.TokenValidationParameters =
    new TokenValidationParameters
    {

        ValidateIssuer = true,

        ValidateAudience = true,

        ValidateLifetime = true,

        ValidateIssuerSigningKey = true,

        ValidIssuer =
    builder.Configuration["Jwt:Issuer"],

        ValidAudience =
    builder.Configuration["Jwt:Audience"],

        IssuerSigningKey =
    new SymmetricSecurityKey(
    key
    )

    };

});

builder.Services.AddAuthorization();
builder.Services.AddScoped<SongRepository>();
builder.Services.AddScoped<SongService>();
// Add services
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

// Apply CORS
app.UseCors("AllowAll");

// Authorization (nếu có login sau này)
app.UseAuthentication();

app.UseAuthorization();

// Map controllers
app.MapControllers();

app.Run();