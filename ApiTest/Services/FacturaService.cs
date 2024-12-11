using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

public class FacturaService
{
    private readonly string _filePath;

    public FacturaService()
    {
        _filePath = Path.Combine(Directory.GetCurrentDirectory(), "JsonEjemplo.json");
    }

    public List<Factura> ObtenerFacturas()
    {
        var jsonString = File.ReadAllText(_filePath);
        var facturas = JsonSerializer.Deserialize<List<Factura>>(jsonString);
        facturas?.ForEach(f => f.CalcularTotalFactura());
        return facturas ?? new List<Factura>();
    }

    public List<Factura> ObtenerFacturasPorRut(int rutComprador)
    {
        var facturas = ObtenerFacturas();
        return facturas.FindAll(f => f.RUTComprador == rutComprador);
    }

    public Factura ObtenerCompradorConMasCompras()
    {
        var facturas = ObtenerFacturas();
        var comprasPorComprador = facturas.GroupBy(f => f.RUTComprador)
                                          .OrderByDescending(g => g.Sum(f => f.TotalFactura))
                                          .FirstOrDefault();
        return comprasPorComprador?.FirstOrDefault();
    }

    public List<(double RUTComprador, double TotalCompras)> ObtenerCompradoresConMontoTotal()
    {
        var facturas = ObtenerFacturas();
        var comprasPorComprador = facturas.GroupBy(f => f.RUTComprador)
                                          .Select(g => new
                                          {
                                              RUTComprador = g.Key,
                                              TotalCompras = g.Sum(f => f.TotalFactura)
                                          })
                                          .ToList();

        return comprasPorComprador.Select(c => (c.RUTComprador, c.TotalCompras)).ToList();
    }

    public Dictionary<double, List<Factura>> ObtenerFacturasPorComuna()
    {
        var facturas = ObtenerFacturas();
        return facturas.GroupBy(f => f.ComunaComprador)
                       .ToDictionary(g => g.Key, g => g.ToList());
    }

    public List<Factura> ObtenerFacturasPorComunaEspecifica(double comuna)
    {
        var facturas = ObtenerFacturas();
        return facturas.Where(f => f.ComunaComprador == comuna).ToList();
    }
}
