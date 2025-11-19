using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

using InnerHealth.Api.Data;
using InnerHealth.Api.Services;
using InnerHealth.Api.Domain.Entities;      // User
using InnerHealth.Api.Domain.Enums;         // UserRole
using InnerHealth.Api.Auth;                 // JwtService
using InnerHealth.Api.Infrastructure.Data; // UserRepository

var builder = WebApplication.CreateBuilder(args);

// =============================================
// CONTROLLERS
// =============================================
builder.Services.AddControllers();


// =============================================
// DATABASE (SQLite + EF)
// =============================================

// ? Para utilizar o mysql quando for para o DOCKER, caso queria rodar localmente deixe comentado
// builder.Services.AddDbContext<ApplicationDbContext>(options =>
// {
//     var cs = builder.Configuration.GetConnectionString("DefaultConnection");
//     options.UseMySql(cs, ServerVersion.AutoDetect(cs));
// });
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var cs = builder.Configuration.GetConnectionString("SQLite");
    options.UseSqlite(cs);
});

// =============================================
// SERVICES (DOMAIN SERVICES)
// =============================================
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IWaterService, WaterService>();
builder.Services.AddScoped<ISunlightService, SunlightService>();
builder.Services.AddScoped<IMeditationService, MeditationService>();
builder.Services.AddScoped<ISleepService, SleepService>();
builder.Services.AddScoped<IPhysicalActivityService, PhysicalActivityService>();
builder.Services.AddScoped<ITaskService, TaskService>();

// =============================================
// AUTH & SECURITY SERVICES
// =============================================
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<JwtService>();


// =============================================
// AUTOMAPPER
// =============================================
builder.Services.AddAutoMapper(typeof(Program).Assembly);


// =============================================
// API VERSIONING
// =============================================
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
});

builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});


// =============================================
// JWT CONFIGURATION
// =============================================

var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!);
var issuer = builder.Configuration["Jwt:Issuer"];
var audience = builder.Configuration["Jwt:Audience"];

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
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
        ValidIssuer = issuer,
        ValidAudience = audience,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization();


// =============================================
// SWAGGER CONFIG (COM JWT)
// =============================================
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "InnerHealth API v1",
        Description = "Versão inicial da API"
    });

    options.SwaggerDoc("v2", new OpenApiInfo
    {
        Version = "v2",
        Title = "InnerHealth API v2",
        Description = "Versão avançada da API"
    });

    // JWT AUTH NO SWAGGER
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Insira o token JWT no formato: Bearer {seu_token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer"
    });

    
    options.OperationFilter<InnerHealth.Api.Swagger.AuthOperationFilter>();

    options.EnableAnnotations();
});


// =============================================
// BUILD APP
// =============================================
var app = builder.Build();


// =============================================
// SWAGGER ALWAYS ENABLED
// =============================================
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "InnerHealth API v1");
    c.SwaggerEndpoint("/swagger/v2/swagger.json", "InnerHealth API v2");
});

app.UseMiddleware<InnerHealth.Api.Middleware.GlobalExceptionMiddleware>();
app.MapGet("/test-exception", () =>
{
    throw new Exception("Erro de teste do middleware!");
});


// =============================================
// AUTH & AUTHORIZATION
// =============================================
app.UseAuthentication();
app.UseAuthorization();


// =============================================
// CONTROLLERS
// =============================================
app.MapControllers();


// =============================================
// DATABASE (AUTO CREATE) + SEED ADMIN
// =============================================
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    // Cria o banco e as tabelas
    db.Database.EnsureCreated();

    try
    {
        // Seed apenas se Users existir e estiver vazio
        if (db.Users.Any() == false)
        {
            db.Users.Add(new User
            {
                Name = "Admin",
                Email = "admin@innerhealth.com",
                Role = UserRole.Admin,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!")
            });

            db.SaveChanges();
        }
    }
    catch
    {
        // Quando Users ainda não existe → ignora, na próxima execução funciona
    }
}




// =============================================
// RUN APP
// =============================================
app.Run();
