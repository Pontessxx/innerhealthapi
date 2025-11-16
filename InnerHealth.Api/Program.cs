using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using InnerHealth.Api.Data;
using System.Linq;

var builder = WebApplication.CreateBuilder(args);

// Adiciona os controllers na parada.
builder.Services.AddControllers();

// Aqui a gente registra o DbContext usando SQLite (simples e ótimo pra projeto acadêmico).
// A connection string tá lá no appsettings.json. Com SQLite, o EF Core cria o arquivo do banco
// se não existir, então não precisa ficar instalando SQL Server Express ou LocalDB.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configuração de versionamento da API
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
});

// Adiciona o API Explorer pra ajudar no versionamento do Swagger
builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

// Registra o AutoMapper pra mapear DTOs
builder.Services.AddAutoMapper(typeof(Program).Assembly);

// Registra os serviços do domínio
builder.Services.AddScoped<InnerHealth.Api.Services.IUserService, InnerHealth.Api.Services.UserService>();
builder.Services.AddScoped<InnerHealth.Api.Services.IWaterService, InnerHealth.Api.Services.WaterService>();
builder.Services.AddScoped<InnerHealth.Api.Services.ISunlightService, InnerHealth.Api.Services.SunlightService>();
builder.Services.AddScoped<InnerHealth.Api.Services.IMeditationService, InnerHealth.Api.Services.MeditationService>();
builder.Services.AddScoped<InnerHealth.Api.Services.ISleepService, InnerHealth.Api.Services.SleepService>();
builder.Services.AddScoped<InnerHealth.Api.Services.IPhysicalActivityService, InnerHealth.Api.Services.PhysicalActivityService>();
builder.Services.AddScoped<InnerHealth.Api.Services.ITaskService, InnerHealth.Api.Services.TaskService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // Cria uma doc básica do Swagger pra cada versão da API. Os grupos dinâmicos rolam depois.
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "InnerHealth API v1",
        Description = "InnerHealth API v1 oferece endpoints para acompanhar hidratação diária, exposição ao sol, meditação, sono, atividade física, tarefas e informações do perfil do usuário."
    });
    options.SwaggerDoc("v2", new OpenApiInfo
    {
        Version = "v2",
        Title = "InnerHealth API v2",
        Description = "InnerHealth API v2 expande a v1 com recomendações automáticas e resumos diários aprimorados."
    });
});

var app = builder.Build();

// Swagger liberado no ambiente de desenvolvimento pra facilitar testes e debug
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

// Swagger sempre ligado, independente do ambiente. Assim a documentação fica acessível
// em produção e evita erro 404 quando acessar /swagger.
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "InnerHealth API v1");
    c.SwaggerEndpoint("/swagger/v2/swagger.json", "InnerHealth API v2");
});

// app.UseHttpsRedirection();

app.UseAuthorization();


app.MapControllers();


// Aplica automaticamente as migrações pendentes ou cria o esquema se não houver migrations.
//
// O EF Core marca as migrações aplicadas na tabela __EFMigrationsHistory. Em cenários em que
// o banco foi criado manualmente ou as migrações ficaram fora de sincronia (por exemplo,
// ao renomear namespaces ou mover arquivos), pode ocorrer de o banco existir mas as
// tabelas não estarem presentes. Para contornar isso, usamos a seguinte lógica:
//   1. Obtemos as migrações pendentes via GetPendingMigrations().
//   2. Se houver pendentes, tentamos aplicar via Migrate().
//   3. Se não houver pendentes ou o Migrate() falhar, usamos EnsureCreated() para
//      garantir que todas as tabelas definidas no DbContext sejam criadas.
//
// Esse fallback faz com que a API funcione mesmo em ambientes onde o dotnet-ef não está
// disponível ou quando o banco ficou em um estado inconsistente. Caso esteja usando um
// provedor relacional diferente de SQLite em produção, recomende-se rodar as migrations
// via CLI para um controle mais granular.
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    try
    {
        var pending = db.Database.GetPendingMigrations();
        // Se houver migrations pendentes, aplicamos normalmente
        if (pending.Any())
        {
            db.Database.Migrate();
        }
        else
        {
            // Caso não haja nenhuma pendente, garantimos que as tabelas existam.
            db.Database.EnsureCreated();
        }
    }
    catch
    {
        // Em caso de erro (por exemplo, migrations inconsistentes), garantimos que
        // o esquema seja criado a partir do modelo atual. Isso evita erros
        // “no such table” na primeira execução.
        db.Database.EnsureCreated();
    }
}


app.Run();