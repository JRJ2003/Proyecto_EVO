using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using GUI.Usuarios;  // VentaService y VentaRepository
using GUI.Producto;  // VentaC

namespace GUI
{
    public partial class FrmReportes : Form
    {
        private readonly VentaService _ventaService;

        public FrmReportes()
        {
            InitializeComponent();

            // Conexión a tu base de datos real
            _ventaService = ServiceFactory.CrearVentaService();

        }

        private void FrmReportes_Load(object sender, EventArgs e)
        {
            cmbTipoReporte.Items.AddRange(new[] { "Ventas por día", "Más vendidos", "Totales" });
            cmbLapso.Items.AddRange(new[] { "Hoy", "3 días", "7 días", "15 días", "30 días", "Todos" });

            cmbTipoReporte.SelectedIndex = 0;
            cmbLapso.SelectedIndex = 0;
        }


        private void btnGenerarReporte_Click(object sender, EventArgs e)
        {
            if (cmbTipoReporte.SelectedItem == null || cmbLapso.SelectedItem == null)
            {
                MessageBox.Show("Seleccione un tipo de reporte y un lapso de tiempo.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string tipo = cmbTipoReporte.SelectedItem.ToString();
            string lapso = cmbLapso.SelectedItem.ToString();

            var ventas = _ventaService.Listar().ToList();
            DateTime desde = CalcularFechaDesde(lapso);
            var ventasFiltradas = ventas.Where(v => v.Fecha >= desde).ToList();

            switch (tipo)
            {
                case "Ventas por día":
                    GenerarVentasPorDia(ventasFiltradas);
                    break;
                case "Más vendidos":
                    GenerarMasVendidos(ventasFiltradas);
                    break;
                case "Totales":
                    GenerarTotales(ventasFiltradas);
                    break;
            }
        }

        private DateTime CalcularFechaDesde(string lapso)
        {
            int dias = 0;

            if (lapso == "Hoy")
                dias = 0;
            else if (lapso == "3 días")
                dias = 3;
            else if (lapso == "7 días")
                dias = 7;
            else if (lapso == "15 días")
                dias = 15;
            else if (lapso == "30 días")
                dias = 30;
            else if (lapso == "Todos")
                return DateTime.MinValue; // incluye todas las ventas

            return DateTime.Now.AddDays(-dias);
        }



        private void GenerarVentasPorDia(List<VentaC> ventas)
        {
            var reporte = ventas
                .GroupBy(v => v.Fecha.Date)
                .Select(g => new
                {
                    Fecha = g.Key.ToString("dd/MM/yyyy"),
                    TotalVentas = g.Sum(v => v.Total)
                })
                .OrderBy(r => r.Fecha)
                .ToList();

            dgvReportes.DataSource = reporte;
            lblTotalVentas.Text = "$ " + reporte.Sum(r => r.TotalVentas).ToString("N2");
        }

        private void GenerarMasVendidos(List<VentaC> ventas)
        {
            // Suponiendo que ListaProductos guarda nombres separados por coma
            var productos = ventas
                .SelectMany(v => v.ListaProductos.Split(','))
                .Select(p => p.Trim())
                .Where(p => !string.IsNullOrEmpty(p))
                .GroupBy(p => p)
                .Select(g => new
                {
                    Producto = g.Key,
                    CantidadVendida = g.Count()
                })
                .OrderByDescending(r => r.CantidadVendida)
                .ToList();

            dgvReportes.DataSource = productos;
            lblTotalVentas.Text = "$ " + ventas.Sum(v => v.Total).ToString("N2");
        }

        private void GenerarTotales(List<VentaC> ventas)
        {
            int totalRegistros = ventas.Count;
            decimal totalVentas = ventas.Sum(v => v.Total);

            var resumen = new[]
            {
                new { Descripción = "Total de ventas registradas", Total = totalRegistros.ToString() },
                new { Descripción = "Monto total vendido", Total = totalVentas.ToString("N2") }
            };

            dgvReportes.DataSource = resumen;
            lblTotalVentas.Text = "$ " + totalVentas.ToString("N2");
        }
    }
}
