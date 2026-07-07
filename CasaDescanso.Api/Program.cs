using CasaDescanso.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using CasaDescanso.Infrastructure.Services;
using CasaDescanso.Domain.Interfaces;
using CasaDescanso.Application.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.SetIsOriginAllowed(origin => true) // Permite cualquier origen dinámicamente
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials(); // REQUERIDO POR SIGNALR
    });
});

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Registro de servicios
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IWorkerService, WorkerService>();
builder.Services.AddScoped<IResidentService, ResidentService>();
builder.Services.AddScoped<IIncidentService, IncidentService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IVitalSignService, VitalSignService>();
builder.Services.AddScoped<IEventsService, EventsService>();
builder.Services.AddScoped<IEmailService, SendGridEmailService>();

var cloudinarySection = builder.Configuration.GetSection("CloudinarySettings");
builder.Services.Configure<CloudinarySettings>(options => {
    options.CloudName = cloudinarySection["CloudName"] ?? "";
    options.ApiKey = cloudinarySection["ApiKey"] ?? "";
    options.ApiSecret = cloudinarySection["ApiSecret"] ?? "";
});
builder.Services.AddScoped<ICloudinaryService, CloudinaryService>();

// 1. Obtener la cadena de conexión
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// 2. Configurar el DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseMySql(connectionString, ServerVersion.Parse("8.0.0-mysql"));
});

// 3. Configurar Autenticación JWT
var jwtKey = builder.Configuration["JwtSettings:Secret"] ?? "EstaEsUnaSuperClaveSecretaMuyLargaParaJWT123456789";
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            ValidateIssuer = false, // En un entorno real se recomienda validar el Emisor
            ValidateAudience = false,
            ClockSkew = TimeSpan.Zero // Para que la expiración sea exacta
        };
        
        // Extraer el token de la query string para SignalR
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];
                var path = context.HttpContext.Request.Path;
                if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs/notifications"))
                {
                    context.Token = accessToken;
                }
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddSignalR();
builder.Services.AddControllers();

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(5195);
});

var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.UseUrls($"http://*:{port}");

var app = builder.Build();

app.UseHttpsRedirection();
app.UseRouting();

// Middleware global de excepciones
app.UseMiddleware<CasaDescanso.Api.Middleware.GlobalExceptionMiddleware>();

app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.MapHub<CasaDescanso.Api.Hubs.NotificationHub>("/hubs/notifications");

app.Run();
