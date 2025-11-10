using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.Producto
{
    internal class ProductoC
    {
        public string IdProducto { get; set; }    // e.g. "PRO0000001"
        public string Nombre { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public string Categoria { get; set; }

        public ProductoC() { }

        public ProductoC(string id, string nombre, decimal precio, int stock, string categoria)
        {
            IdProducto = id;
            Nombre = nombre;
            Precio = precio;
            Stock = stock;
            Categoria = categoria;
        }
    }
}
