using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUI.Producto;

namespace GUI.Usuarios
{
    internal class VentaService
    {
        private readonly IVentaRepository _repo;

        public VentaService(IVentaRepository repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        public IEnumerable<VentaC> Listar(string filtro = "")
        {
            return _repo.ObtenerTodos(filtro ?? string.Empty);
        }

        public VentaC Obtener(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("El id no puede estar vacío.", nameof(id));

            return _repo.ObtenerPorId(id);
        }

        public (bool Success, string Message) Crear(VentaC p)
        {
            if (p == null) return (false, "Venta vacío.");
            if (string.IsNullOrWhiteSpace(p.IdUsuario)) return (false, "Id del usuario requerido.");
            if (p.Total <= 0) return (false, "Total requerido.");
            if (string.IsNullOrWhiteSpace(p.ListaProductos)) return (false, "Lista de productos requerido.");

            p.IdVenta = GenerarNuevoId();

            _repo.Insertar(p);
            return (true, $"Venta creado con ID: {p.IdUsuario}");
        }

        public (bool Success, string Message) Actualizar(VentaC p)
        {
            if (p == null) return (false, "Venta vacío.");
            if (string.IsNullOrWhiteSpace(p.IdVenta)) return (false, "ID requerido.");
            if (string.IsNullOrWhiteSpace(p.IdUsuario)) return (false, "Id del usuario requerido.");
            if (p.Total <= 0) return (false, "Total requerido.");
            if (string.IsNullOrWhiteSpace(p.ListaProductos)) return (false, "Lista de productos requerido.");
            _repo.Actualizar(p);
            return (true, "Venta actualizado correctamente.");
        }

        public (bool Success, string Message) Eliminar(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return (false, "ID requerido.");
            bool ok = _repo.Eliminar(id);
            return ok ? (true, "Venta eliminado.") : (false, "No se encontró la venta.");
        }
        public string GenerarNuevoId()
        {
            string ultimo = _repo.ObtenerUltimoId();

            if (string.IsNullOrEmpty(ultimo) || ultimo.Length < 10)
            {
                return "VEN0000001";
            }

            try
            {
                int numero = int.Parse(ultimo.Substring(3)) + 1;
                return "VEN" + numero.ToString("D7");
            }
            catch
            {
                // Si hay un error de formato, reinicia el conteo
                return "VEN0000001";
            }
        }
    }
}
