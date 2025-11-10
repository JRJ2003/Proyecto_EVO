using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.Producto
{
    internal class ProductoService
    {
        private readonly IProductoRepository _repo;

        public ProductoService(IProductoRepository repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        public IEnumerable<ProductoC> Listar(string filtro = "")
        {
            return _repo.ObtenerTodos(filtro ?? string.Empty);
        }

        public ProductoC Obtener(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("El id no puede estar vacío.", nameof(id));

            return _repo.ObtenerPorId(id);
        }

        public (bool Success, string Message) Crear(ProductoC p)
        {
            if (p == null) return (false, "Producto vacío.");
            if (string.IsNullOrWhiteSpace(p.Nombre)) return (false, "Nombre requerido.");
            if (p.Precio < 0) return (false, "Precio no puede ser negativo.");
            if (p.Stock < 0) return (false, "Stock no puede ser negativo.");

            p.IdProducto = GenerarNuevoId();

            _repo.Insertar(p);
            return (true, $"Producto creado con ID: {p.IdProducto}");
        }

        public (bool Success, string Message) Actualizar(ProductoC p)
        {
            if (p == null) return (false, "Producto vacío.");
            if (string.IsNullOrWhiteSpace(p.IdProducto)) return (false, "ID requerido.");
            if (string.IsNullOrWhiteSpace(p.Nombre)) return (false, "Nombre requerido.");
            if (p.Precio < 0) return (false, "Precio no puede ser negativo.");
            if (p.Stock < 0) return (false, "Stock no puede ser negativo.");

            _repo.Actualizar(p);
            return (true, "Producto actualizado correctamente.");
        }

        public (bool Success, string Message) Eliminar(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return (false, "ID requerido.");
            bool ok = _repo.Eliminar(id);
            return ok ? (true, "Producto eliminado.") : (false, "No se encontró el producto.");
        }
        public string GenerarNuevoId()
        {
            string ultimo = _repo.ObtenerUltimoId();

            if (string.IsNullOrEmpty(ultimo) || ultimo.Length < 10)
            {
                return "PRO0000001";
            }

            try
            {
                int numero = int.Parse(ultimo.Substring(3)) + 1;
                return "PRO" + numero.ToString("D7");
            }
            catch
            {
                // Si hay un error de formato, reinicia el conteo
                return "PRO0000001";
            }
        }
    }
}
