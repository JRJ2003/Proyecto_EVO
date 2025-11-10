using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUI.Producto;
using MySql.Data.MySqlClient;

namespace GUI.Usuarios
{
    internal class VentaRepository : IVentaRepository
    {

        private readonly string _connectionString;

        public VentaRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public void Actualizar(VentaC venta)
        {
            using (var cn = new MySqlConnection(_connectionString))
            using (var cmd = cn.CreateCommand())
            {
                cn.Open();
                cmd.CommandText = @"UPDATE Venta SET idUsuario=@idUsuario, fecha=@fecha, total=@total, listaProductos=@listaProductos
                                    WHERE idVenta=@idVenta";
                cmd.Parameters.AddWithValue("@idUsuario", venta.IdUsuario);
                cmd.Parameters.AddWithValue("@fecha", venta.Fecha);
                cmd.Parameters.AddWithValue("@total", venta.Total);
                cmd.Parameters.AddWithValue("@listaProductos", venta.ListaProductos);
                cmd.Parameters.AddWithValue("@idVenta", venta.IdVenta);
                cmd.ExecuteNonQuery();
            }
        }

        public bool Eliminar(string id)
        {
            using (var cn = new MySqlConnection(_connectionString))
            using (var cmd = cn.CreateCommand())
            {
                cn.Open();
                cmd.CommandText = "DELETE FROM Venta WHERE idVenta=@idVenta";
                cmd.Parameters.AddWithValue("@idVenta", id);
                int filas = cmd.ExecuteNonQuery();
                return filas > 0;
            }
        }

        public void Insertar(VentaC venta)
        {
            using (var cn = new MySqlConnection(_connectionString))
            using (var cmd = cn.CreateCommand())
            {
                cn.Open();
                cmd.CommandText = @"INSERT INTO Venta (idVenta, idUsuario, fecha, total, listaProductos)
                                    VALUES (@idVenta, @idUsuario, @fecha, @total, @listaProductos)";
                cmd.Parameters.AddWithValue("@idVenta", venta.IdVenta);
                cmd.Parameters.AddWithValue("@idUsuario", venta.IdUsuario);
                cmd.Parameters.AddWithValue("@fecha", venta.Fecha);
                cmd.Parameters.AddWithValue("@total", venta.Total);
                cmd.Parameters.AddWithValue("@listaProductos", venta.ListaProductos);
                cmd.ExecuteNonQuery();
            }
        }

        public VentaC ObtenerPorId(string id)
        {
            VentaC p = null;
            using (var cn = new MySqlConnection(_connectionString))
            using (var cmd = cn.CreateCommand())
            {
                cn.Open();
                cmd.CommandText = "SELECT idVenta, idUsuario, fecha, total, listaProductos FROM Venta WHERE idVenta = @id";
                cmd.Parameters.AddWithValue("@id", id);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        p = new VentaC(
                            reader.GetString("idVenta"),
                            reader.GetString("idUsuario"),
                            reader.GetDateTime("fecha"),
                            reader.GetDecimal("total"),
                            reader.GetString("listaProductos")
                        );
                    }
                }
            }
            return p;
        }

        public IEnumerable<VentaC> ObtenerTodos(string filtro = "")
        {
            var lista = new List<VentaC>();

            using (var cn = new MySqlConnection(_connectionString))
            using (var cmd = cn.CreateCommand())
            {
                cn.Open();
                cmd.CommandText = @"
                    SELECT idVenta, idUsuario, fecha, total, listaProductos
                    FROM Venta
                    WHERE idUsuario LIKE @filtro
                    ORDER BY CAST(SUBSTRING(idVenta, 4) AS UNSIGNED) DESC";
                cmd.Parameters.AddWithValue("@filtro", "%" + filtro + "%");

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new VentaC(
                            reader.GetString("idVenta"),
                            reader.GetString("idUsuario"),
                            reader.GetDateTime("fecha"),
                            reader.GetDecimal("total"),
                            reader.GetString("listaProductos")
                        ));
                    }
                }
            }

            return lista;
        }

        public string ObtenerUltimoId()
        {
            using (var cn = new MySqlConnection(_connectionString))
            using (var cmd = cn.CreateCommand())
            {
                cn.Open();
                cmd.CommandText = "SELECT idVenta FROM Venta ORDER BY idVenta DESC LIMIT 1";
                object obj = cmd.ExecuteScalar();
                return obj == null ? string.Empty : obj.ToString();
            }
        }
    }
}
