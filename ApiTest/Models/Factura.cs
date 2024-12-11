public class Factura
{
    public double NumeroDocumento { get; set; }
    public double RUTVendedor { get; set; }
    public string DvVendedor { get; set; }
    public double RUTComprador { get; set; }
    public string DvComprador { get; set; }
    public string DireccionComprador { get; set; }
    public double ComunaComprador { get; set; }
    public double RegionComprador { get; set; }
    public double TotalFactura { get; set; }
    public List<DetalleFactura> DetalleFactura { get; set; }


    public void CalcularTotalFactura()
    {
        TotalFactura = DetalleFactura.Sum(d => d.TotalProducto);
    }
}

public class DetalleFactura
{
    public double CantidadProducto { get; set; }
    public Producto Producto { get; set; }
    public double TotalProducto { get; set; }
}

public class Producto
{
    public string Descripcion { get; set; }
    public double Valor { get; set; }
}

public class BuyerTotal
{
    public double RUTComprador { get; set; }
    public double TotalCompras { get; set; }
}
