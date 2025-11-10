using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.Usuarios
{
    internal class UsuarioC
    {
        public string IdUsuario { get; set; }    // e.g. "USU0000001"
        public string NombreUsuario { get; set; }
        public string ContraUsuario { get; set; }
        public string RolUsuario { get; set; }

        public UsuarioC() { }

        public UsuarioC(string id, string nombre, string contraUsuario, string rolUsuario)
        {
            IdUsuario = id;
            NombreUsuario = nombre;
            ContraUsuario = contraUsuario;
            RolUsuario = rolUsuario;
        }
    }
}

