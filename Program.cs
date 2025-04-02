using System;
using InmobiliariaLopez.Data; // Para DatabaseConnection
using InmobiliariaLopez.Models; // Para Propietario e Inquilino
using InmobiliariaLopez.Repositories; // Para IRepositorio, PropietarioRepository, InquilinoRepository
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Agregamos servicios al contenedor de dependencias
builder.Services.AddControllersWithViews();

// 1. Registrar DatabaseConnection como un servicio
builder.Services.AddTransient<DatabaseConnection>();

// 2. Registrar los repositorios para Propietario e Inquilino
builder.Services.AddScoped<IRepositorioPropietario, PropietarioRepository>();
builder.Services.AddScoped<IRepositorioInquilino, InquilinoRepository>();
builder.Services.AddScoped<IRepositorioInmueble, InmuebleRepository>();

// Creamos la aplicación web
var app = builder.Build();

// Configuramos el pipeline de solicitudes HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// Definimos la ruta predeterminada para las vistas Razor
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Iniciamos la aplicación
app.Run();