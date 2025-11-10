using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUI.Producto;

namespace GUI.Usuarios
{
    internal class UsuarioService
    {
        private readonly IUsuarioRepository _repo;

        public UsuarioService(IUsuarioRepository repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        public IEnumerable<UsuarioC> Listar(string filtro = "")
        {
            return _repo.ObtenerTodos(filtro ?? string.Empty);
        }

        public UsuarioC Obtener(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("El id no puede estar vacío.", nameof(id));

            return _repo.ObtenerPorId(id);
        }

        public (bool Success, string Message) Crear(UsuarioC p)
        {
            if (p == null) return (false, "Producto vacío.");
            if (string.IsNullOrWhiteSpace(p.NombreUsuario)) return (false, "Nombre requerido.");
            if (string.IsNullOrWhiteSpace(p.ContraUsuario)) return (false, "Contraseña requerido.");
            if (string.IsNullOrWhiteSpace(p.RolUsuario)) return (false, "Rol requerido.");

            p.IdUsuario = GenerarNuevoId();

            _repo.Insertar(p);
            return (true, $"Usuario creado con ID: {p.IdUsuario}");
        }

        public (bool Success, string Message) Actualizar(UsuarioC p)
        {
            if (p == null) return (false, "Producto vacío.");
            if (string.IsNullOrWhiteSpace(p.IdUsuario)) return (false, "ID requerido.");
            if (string.IsNullOrWhiteSpace(p.NombreUsuario)) return (false, "Nombre requerido.");
            if (string.IsNullOrWhiteSpace(p.ContraUsuario)) return (false, "Contraseña requerido.");
            if (string.IsNullOrWhiteSpace(p.RolUsuario)) return (false, "Rol requerido.");
            _repo.Actualizar(p);
            return (true, "Usuario actualizado correctamente.");
        }

        public (bool Success, string Message) Eliminar(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return (false, "ID requerido.");
            bool ok = _repo.Eliminar(id);
            return ok ? (true, "Usuario eliminado.") : (false, "No se encontró el usuario.");
        }
        public string GenerarNuevoId()
        {
            string ultimo = _repo.ObtenerUltimoId();

            if (string.IsNullOrEmpty(ultimo) || ultimo.Length < 10)
            {
                return "USU0000001";
            }

            try
            {
                int numero = int.Parse(ultimo.Substring(3)) + 1;
                return "USU" + numero.ToString("D7");
            }
            catch
            {
                // Si hay un error de formato, reinicia el conteo
                return "USU0000001";
            }
        }
    }
}
