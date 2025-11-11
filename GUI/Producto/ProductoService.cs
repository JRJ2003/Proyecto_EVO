using System;
using System.Collections.Generic;
using System.Linq;

namespace GUI.Producto
{
    internal class ProductoService
    {
        private readonly IProductoRepository _repo;

        // Lista temporal para guardar los precios originales
        private static List<ProductoC> _preciosOriginales = new List<ProductoC>();


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
                return "PRO0000001";
            }
        }

        // ------------------- NUEVOS MÉTODOS RFC ESTÁNDAR -------------------

        // Aplicar descuento a todos los productos
        public (bool Success, string Message) AplicarDescuentoATodos(decimal porcentaje)
        {
            if (porcentaje < 10 || porcentaje > 70)
                return (false, "El porcentaje debe estar entre 10 y 70.");

            var productos = _repo.ObtenerTodos("").ToList();

            // Guardar precios originales si es la primera vez
            if (_preciosOriginales.Count == 0)
            {
                foreach (var p in productos)
                {
                    _preciosOriginales.Add(new ProductoC
                    {
                        IdProducto = p.IdProducto,
                        Nombre = p.Nombre,
                        Precio = p.Precio,
                        Stock = p.Stock
                    });
                }
            }

            // Aplicar descuento
            foreach (var p in productos)
            {
                p.Precio -= p.Precio * (porcentaje / 100);
                if (p.Precio < 0) p.Precio = 0; // evitar precios negativos
                _repo.Actualizar(p);
            }

            return (true, $"Descuento del {porcentaje}% aplicado a todos los productos.");
        }

        // Restaurar precios originales
        public (bool Success, string Message) RestaurarPreciosOriginales()
        {
            if (_preciosOriginales.Count == 0)
                return (false, "No hay precios originales guardados.");

            foreach (var original in _preciosOriginales)
            {
                var producto = _repo.ObtenerPorId(original.IdProducto);
                if (producto != null)
                {
                    producto.Precio = original.Precio;
                    _repo.Actualizar(producto);
                }
            }

            _preciosOriginales.Clear(); // limpiar lista temporal
            return (true, "Precios originales restaurados correctamente.");
        }

        // Respaldar un producto específico si no está ya respaldado
        public void RespaldarPrecioOriginal(ProductoC producto)
        {
            if (!_preciosOriginales.Any(p => p.IdProducto == producto.IdProducto))
            {
                _preciosOriginales.Add(new ProductoC
                {
                    IdProducto = producto.IdProducto,
                    Nombre = producto.Nombre,
                    Precio = producto.Precio,
                    Stock = producto.Stock
                });
            }
        }

    }
}
