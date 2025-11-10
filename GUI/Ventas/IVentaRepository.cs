using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUI.Producto;

namespace GUI.Usuarios
{
    internal interface IVentaRepository
    {
        IEnumerable<VentaC> ObtenerTodos(string filtro = "");
        VentaC ObtenerPorId(string id);
        void Insertar(VentaC venta);
        void Actualizar(VentaC venta);
        bool Eliminar(string id);
        string ObtenerUltimoId();
    }
}
