using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUI.Producto;

namespace GUI.Usuarios
{
    internal interface IUsuarioRepository
    {
        IEnumerable<UsuarioC> ObtenerTodos(string filtro = "");
        UsuarioC ObtenerPorId(string id);
        void Insertar(UsuarioC usuario);
        void Actualizar(UsuarioC usuario);
        bool Eliminar(string id);
        string ObtenerUltimoId();
    }
}
