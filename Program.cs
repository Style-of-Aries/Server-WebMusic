using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using MusicAPI.Models;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.IdentityModel.Tokens;
using MusicAPI.Repositories;
using MusicAPI.Interfaces;
using MusicAPI.Middlewares;
using MusicAPI.Services;

using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Lấy connection string từ appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Đăng ký dịch vụ Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));
// builder.Services.AddDbContext<AppDbContext>(options =>
//     options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 100 * 1024 * 1024; // Cho phép 100MB
});
builder.Services.AddScoped<TokenService>();

// Ví dụ cách lấy Key trong Program.cs
var jwtKey = builder.Configuration["Jwt:Key"];

if (string.IsNullOrEmpty(jwtKey))
{
    throw new InvalidOperationException("JWT Key is missing in appsettings.json");
}

// Sau đó truyền vào cấu hình Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            ValidateIssuer = false,
            ValidateAudience = false,

            // THÊM 2 DÒNG NÀY VÀO:
            ValidateLifetime = true,        // Bật kiểm tra hết hạn
            ClockSkew = TimeSpan.Zero       // Tắt thời gian chờ cộng thêm (mặc định là 5 phút)
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddScoped<ISongRepository, SongRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
// builder.Services.AddScoped<IUserRepositoryNew, UserRepositoryNew>();
// builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IFavoriteRepository, FavoriteRepository>();
builder.Services.AddScoped<SongService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<AuthService>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddAutoMapper(typeof(Program).Assembly);
// Add services
builder.Services.AddControllers()
    .AddJsonOptions(x =>
        x.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles);

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
app.UseMiddleware<ExceptionMiddleware>();
// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();



// Apply CORS
app.UseCors("AllowAll");
app.UseStaticFiles();
// Authorization (nếu có login sau này)
app.UseAuthentication();

app.UseAuthorization();

// Map controllers
app.MapControllers();

app.Run();