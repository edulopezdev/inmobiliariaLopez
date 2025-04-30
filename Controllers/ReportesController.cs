using System;
using System.Collections.Generic;
using System.Linq;
using InmobiliariaLopez.Models;
using InmobiliariaLopez.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace InmobiliariaLopez.Controllers
{
    public class ReportesController : Controller
    {
        private readonly IRepositorioReporte _repositorioReporte;

        public ReportesController(IRepositorioReporte repositorioReporte)
        {
            _repositorioReporte = repositorioReporte;
        }

        // Vista principal - Index
        public IActionResult Index()
        {
            var inmuebles = _repositorioReporte.ObtenerInmueblesConPropietarios(true);
            var contratos = _repositorioReporte.ObtenerContratosVigentes();

            var reportesViewModel = new ReportesViewModel
            {
                InmueblesConPropietarios = inmuebles,
                ContratosVigentes = contratos,
            };

            return View(reportesViewModel);
        }

        public IActionResult Inmuebles(int page = 1, int pageSize = 10)
        {
            var inmuebles = _repositorioReporte.ObtenerInmueblesConPropietarios(true);
            var totalItems = inmuebles.Count;
            var pagedInmuebles = inmuebles.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var viewModel = new ReportesViewModel
            {
                InmueblesConPropietarios = pagedInmuebles,
                TotalItems = totalItems,
                CurrentPage = page,
                PageSize = pageSize,
            };

            return PartialView("_Inmuebles", viewModel); // Renderizar parcialmente
        }

        public IActionResult Contratos(int page = 1, int pageSize = 10)
        {
            var contratos = _repositorioReporte.ObtenerContratosVigentes();
            Console.WriteLine($"Total contratos obtenidos: {contratos.Count}");

            var totalItems = contratos.Count;
            var pagedContratos = contratos.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            Console.WriteLine($"Mostrando contratos de la página {page}");
            foreach (var contrato in pagedContratos)
            {
                Console.WriteLine(
                    $"Contrato ID: {contrato.IdContrato}, Inquilino: {contrato.InquilinoNombre}, Inmueble: {contrato.InmuebleDireccion}"
                );
            }

            var viewModel = new ReportesViewModel
            {
                ContratosVigentes = pagedContratos,
                TotalItems = totalItems,
                CurrentPage = page,
                PageSize = pageSize,
            };

            return PartialView("_Contratos", viewModel);
        }
    }
}
