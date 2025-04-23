using System;
using System.Globalization;
using InmobiliariaLopez.Data; // Para DatabaseConnection
using InmobiliariaLopez.Models; // Para Propietario e Inquilino
using InmobiliariaLopez.Repositories; // Para IRepositorio, PropietarioRepository, InquilinoRepository

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
app.UseAuthorization();

// Definimos la ruta predeterminada para las vistas Razor
app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

// Iniciamos la aplicación
app.Run();
