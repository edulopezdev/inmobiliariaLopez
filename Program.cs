using System;
using System.Globalization;
using InmobiliariaLopez.Data; // Para DatabaseConnection
using InmobiliariaLopez.Models; // Para Propietario e Inquilino
using InmobiliariaLopez.Repositories; // Para IRepositorio, PropietarioRepository, InquilinoRepository
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Agregamos servicios al contenedor de dependencias
builder.Services.AddControllersWithViews();

// 1. Registrar DatabaseConnection como un servicio
builder.Services.AddSingleton<DatabaseConnection>();

// 2. Registrar los repositorios para Propietario e Inquilino
builder.Services.AddScoped<IRepositorioPropietario, PropietarioRepository>();
builder.Services.AddScoped<IRepositorioInquilino, InquilinoRepository>();
builder.Services.AddScoped<IRepositorioInmueble, InmuebleRepository>();
builder.Services.AddScoped<IRepositorioContrato, ContratoRepository>();
builder.Services.AddScoped<IRepositorioImagen, ImagenRepository>();
builder.Services.AddScoped<IRepositorioPago, PagoRepository>();
builder.Services.AddScoped<IRepositorioUsuario, UsuarioRepository>();

// 3. Registrar el servicio de autenticación
builder
    .Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Usuarios/Login";
        options.LogoutPath = "/Usuarios/Logout";

        // Estas líneas aseguran que la cookie no sea persistente
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Opcional, puede ser lo que quieras
        options.SlidingExpiration = true;
        options.Cookie.IsEssential = true;
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.Cookie.SameSite = SameSiteMode.Strict;
        options.Cookie.Name = "InmobiliariaAuth";
    });

// 4. Configurar la política de autorización global
builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser() // Asegura que todas las rutas requieran autenticación
        .Build();
});

// 5. Configuración del logging
builder.Logging.AddConsole(); // Asegura que los logs se escriban en la consola
builder.Logging.AddDebug(); // Asegura que los logs se escriban en la salida de depuración

// Creamos la aplicación web
var app = builder.Build();

// Configurar la cultura
var cultureInfo = new CultureInfo("es-AR");
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

// Configuramos el pipeline de solicitudes HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Definimos la ruta predeterminada para las vistas Razor
app.MapControllerRoute(name: "default", pattern: "{controller=Usuarios}/{action=Login}/{id?}");

// Iniciamos la aplicación
app.Run();
