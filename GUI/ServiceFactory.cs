using GUI.Producto;
using GUI.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI
{
    internal class ServiceFactory
    {
        private static readonly string _connectionString =
            "server=localhost; database=db_empresa; user id=root; password=@xelevV3nt_5960;";

        // PRODUCTOS
        public static IProductoRepository CrearProductoRepository()
        {
            return new ProductoRepository(_connectionString);
        }

        public static ProductoService CrearProductoService()
        {
            return new ProductoService(CrearProductoRepository());
        }

        // Agregar los demas...

        public static IUsuarioRepository CrearUsuarioRepository()
        {
            return new UsuarioRepository(_connectionString);
        }

        public static UsuarioService CrearUsuarioService()
        {
            return new UsuarioService(CrearUsuarioRepository());
        }

        public static IVentaRepository CrearVentaRepository()
        {
            return new VentaRepository(_connectionString);
        }

        public static VentaService CrearVentaService()
        {
            return new VentaService(CrearVentaRepository());
        }
    }
}
