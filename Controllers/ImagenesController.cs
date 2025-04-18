using System;
using System.IO;
using System.Threading.Tasks;
using InmobiliariaLopez.Models;
using InmobiliariaLopez.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InmobiliariaLopez.Controllers
{
    public class ImagenesController : Controller
    {
        private readonly IRepositorioImagen _repositorioImagen;
        private readonly IWebHostEnvironment _environment;

        public ImagenesController(
            IRepositorioImagen repositorioImagen,
            IWebHostEnvironment environment
        )
        {
            _repositorioImagen = repositorioImagen;
            _environment = environment;
        }

        [HttpPost]
        public async Task<IActionResult> SubirImagen(
            int idRelacionado,
            string tipoImagen,
            IFormFile archivo
        )
        {
            if (archivo == null || archivo.Length == 0)
            {
                return BadRequest("No se recibió ningún archivo.");
            }

            if (string.IsNullOrEmpty(tipoImagen))
            {
                return BadRequest("El tipo de imagen no puede estar vacío.");
            }

            string wwwPath = _environment.WebRootPath;
            string path = Path.Combine(
                wwwPath,
                "uploads",
                tipoImagen.ToLower(),
                idRelacionado.ToString()
            );

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var extension = Path.GetExtension(archivo.FileName);
            var nombreArchivo = $"{Guid.NewGuid()}{extension}";
            var rutaArchivo = Path.Combine(path, nombreArchivo);
            var rutaRelativa = $"/uploads/{tipoImagen.ToLower()}/{idRelacionado}/{nombreArchivo}";

            using (var stream = new FileStream(rutaArchivo, FileMode.Create))
            {
                await archivo.CopyToAsync(stream);
            }

            var imagen = new Imagen
            {
                Ruta = rutaRelativa,
                TipoImagen = tipoImagen,
                IdRelacionado = idRelacionado,
                Activo = true,
            };

            _repositorioImagen.Create(imagen);

            return Ok(new { RutaImagen = rutaRelativa, TipoImagen = tipoImagen });
        }
    }
}
