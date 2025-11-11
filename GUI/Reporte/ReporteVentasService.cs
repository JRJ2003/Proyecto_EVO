using GUI.Producto;
using GUI.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GUI.Reportes
{
    internal class ReporteVentasService
    {
        private readonly IEnumerable<VentaC> _ventas;

        public ReporteVentasService(IEnumerable<VentaC> ventas)
        {
            _ventas = ventas ?? throw new ArgumentNullException(nameof(ventas));
        }

        public IEnumerable<object> GenerarVentasPorDia()
        {
            return _ventas
                .GroupBy(v => v.Fecha.Date)
                .Select(g => new
                {
                    Fecha = g.Key.ToShortDateString(),
                    Ventas = g.Count(),
                    Total = g.Sum(x => x.Total)
                })
                .OrderBy(r => r.Fecha)
                .ToList();
        }

        public IEnumerable<object> GenerarMasVendidos()
        {
            return _ventas
                .SelectMany(v => v.ListaProductos.Split(','))
                .GroupBy(p => p.Trim())
                .Select(g => new
                {
                    Producto = g.Key,
                    Cantidad = g.Count()
                })
                .OrderByDescending(r => r.Cantidad)
                .ToList();
        }

        public IEnumerable<object> GenerarTotales()
        {
            var totalVentas = _ventas.Sum(v => v.Total);
            var totalRegistros = _ventas.Count();

            return new[]
{
    new { Descripción = "Total de ventas registradas", Total = totalRegistros.ToString() },
    new { Descripción = "Monto total vendido", Total = totalVentas.ToString("N2") }
};

        }
    }
}
