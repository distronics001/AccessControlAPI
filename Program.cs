using AccessControlAPI.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ✅ 1. Lire les variables d’environnement Render
builder.Configuration.AddEnvironmentVariables();

// ✅ 2. Ajouter les services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ✅ 3. Récupérer la chaîne de connexion depuis Render
var connStr = builder.Configuration["CONNSTR_MYSQL"];
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connStr, ServerVersion.AutoDetect(connStr)));

var app = builder.Build();

// ✅ 4. Configuration de Swagger
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "AccessControlAPI v1");
    c.RoutePrefix = "swagger";
});

// ✅ 5. Middlewares
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
