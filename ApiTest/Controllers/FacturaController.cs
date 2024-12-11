using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

[ApiController]
[Route("api/[controller]")]
public class FacturaController : ControllerBase
{
    private readonly FacturaService _facturaService;

    public FacturaController(FacturaService facturaService)
    {
        _facturaService = facturaService;
    }

    // Obtener todas las facturas 1
    [HttpGet]
    public IActionResult GetAllFacturas()
    {
        var facturas = _facturaService.ObtenerFacturas();
        return Ok(facturas);
    }

    // Obtener facturas por el RUT del comprador 2
    [HttpGet("comprador/{rut}")]
    public IActionResult GetFacturasPorRut(int rut)
    {
        var facturas = _facturaService.ObtenerFacturasPorRut(rut);
        return Ok(facturas);
    }

    // Obtener el comprador con más compras 3
    [HttpGet("comprador-con-mas-compras")]
    public IActionResult GetCompradorConMasCompras()
    {
        var comprador = _facturaService.ObtenerCompradorConMasCompras();
        return Ok(comprador);
    }

    // Obtener lista de compradores con el monto total de compras realizadas 4
    [HttpGet("compradores")]
    public IActionResult GetCompradoresConMontoTotal()
    {
        var compradores = _facturaService.ObtenerCompradoresConMontoTotal();
        return Ok(compradores);
    }
    

 // Método para obtener facturas agrupadas por comuna o filtradas por comuna específica 5
    [HttpGet("comunas")]
    public IActionResult GetInvoicesGroupedOrByComuna([FromQuery] double? comuna = null)
    {
    // Obtener todas las facturas
        var invoices = _facturaService.ObtenerFacturas();

            if (comuna.HasValue)
        {
            // Si se proporciona una comuna, filtrar las facturas por esa comuna
            var facturasComuna = invoices.Where(f => f.ComunaComprador == comuna.Value).ToList();

            if (facturasComuna.Any())
            {
                return Ok(facturasComuna); // Devolver las facturas de la comuna específica
            }
            else
            {
                return NotFound($"No se encontraron facturas para la comuna {comuna.Value}");
            }
        }
        else
        {
            // Si no se proporciona comuna, agrupar las facturas por comuna
            var facturasPorComuna = invoices
            .GroupBy(f => f.ComunaComprador) // Agrupar las facturas por la comuna
                .Select(g => new
                    {
                        Comuna = g.Key,
                        Facturas = g.ToList() // Devolver la lista de facturas agrupadas por comuna
                    })
                    .ToList();

                return Ok(facturasPorComuna);
            }
        }
    }