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
    });

// 4. Configuración del logging
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
app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

// Iniciamos la aplicación
app.Run();
