using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.Producto
{
    internal interface IProductoRepository
    {
        IEnumerable<ProductoC> ObtenerTodos(string filtro = "");
        ProductoC ObtenerPorId(string id);
        void Insertar(ProductoC producto);
        void Actualizar(ProductoC producto);
        bool Eliminar(string id);
        string ObtenerUltimoId();
    }
}
