using System.Text;
using H2_Trainning.Data;
using H2_Trainning.Interfaces;
using H2_Trainning.Models;
using H2_Trainning.Repositories;
using H2_Trainning.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// --- [DARORI L-RAILWAY] ---
// Railway kaye3tik l-Port f Variable smiytha PORT. 0.0.0.0 hiya l-IP dyal d-deployement.
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

// ── EF Core + Identity ──
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// ── JWT Authentication ──
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
// L-SecretKey khodha men env variable ila kant kayna (Security)
var secretKey = Environment.GetEnvironmentVariable("JWT_SECRET") ?? jwtSettings["SecretKey"]!;
var key = Encoding.UTF8.GetBytes(secretKey);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

// ── Repositories ──
builder.Services.AddScoped<IProgramRepository, ProgramRepository>();
builder.Services.AddScoped<IAssignmentRepository, AssignmentRepository>();
builder.Services.AddScoped<IAvailabilityRepository, AvailabilityRepository>();
builder.Services.AddScoped<IReservationRepository, ReservationRepository>();
builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<ICheckInRepository, CheckInRepository>();

// ── Services ──
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IProgramService, ProgramService>();
builder.Services.AddScoped<IAssignmentService, AssignmentService>();
builder.Services.AddScoped<IAvailabilityService, AvailabilityService>();
builder.Services.AddScoped<IReservationService, ReservationService>();
builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<ICheckInService, CheckInService>();

// ── Controllers + JSON ──
builder.Services.AddControllers(options =>
    {
        options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
    })
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    });

// ── OpenAPI / Swagger ──
builder.Services.AddOpenApi();

// ── CORS (Updated l-Cloud) ──
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.WithOrigins(
                  "http://localhost:3000", 
                  "http://localhost:5173", 
                  "http://localhost:5174",
                  "https://h2-trainning.vercel.app",
                  "https://www.h2-coaching.com",
                  "https://h2-coaching.com"
              )
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
// Khalli OpenApi khdam hta f production ila bghiti t-testi l-API direct men Railway
app.MapOpenApi();

app.UseCors("AllowAll"); // Sta3mel l-policy l-jdida

app.UseAuthentication();
app.UseAuthorization();

app.Use(async (context, next) =>
{
    var sw = System.Diagnostics.Stopwatch.StartNew();
    await next();
    sw.Stop();
    Console.WriteLine($"[TIME] {context.Request.Method} {context.Request.Path} took {sw.ElapsedMilliseconds}ms");
});

app.MapControllers();

app.Run();