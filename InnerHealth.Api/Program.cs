using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using InnerHealth.Api.Data;
using InnerHealth.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();

// Banco SQLite — arquivo local, criado automaticamente
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Versionamento
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

// AutoMapper
builder.Services.AddAutoMapper(typeof(Program).Assembly);

// Serviços da aplicação
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IWaterService, WaterService>();
builder.Services.AddScoped<ISunlightService, SunlightService>();
builder.Services.AddScoped<IMeditationService, MeditationService>();
builder.Services.AddScoped<ISleepService, SleepService>();
builder.Services.AddScoped<IPhysicalActivityService, PhysicalActivityService>();
builder.Services.AddScoped<ITaskService, TaskService>();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "InnerHealth API v1",
        Description = "Endpoints básicos de hidratação, sono, tarefas, atividades e métricas diárias."
    });

    options.SwaggerDoc("v2", new OpenApiInfo
    {
        Version = "v2",
        Title = "InnerHealth API v2",
        Description = "Versão estendida com recomendações e resumos mais completos."
    });
});

var app = builder.Build();

// Cria o banco na primeira execução
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.EnsureCreated();
}

// Pipelines
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "InnerHealth API v1");
    c.SwaggerEndpoint("/swagger/v2/swagger.json", "InnerHealth API v2");
});

app.UseAuthorization();
app.MapControllers();
app.Run();