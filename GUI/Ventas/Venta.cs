using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.Usuarios
{
    internal class VentaC
    {
        public string IdVenta { get; set; }    // e.g. "USU0000001"
        public string IdUsuario { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Total { get; set; }
        public string ListaProductos { get; set; }

        public VentaC() { }

        public VentaC(string id, string idusuario, DateTime fecha, decimal total, string listaProductos)
        {
            IdVenta = id;
            IdUsuario = idusuario;
            Fecha = fecha;
            Total = total;
            ListaProductos = listaProductos;
        }
    }
}

