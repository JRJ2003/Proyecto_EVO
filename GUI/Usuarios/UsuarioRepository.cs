using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUI.Producto;
using MySql.Data.MySqlClient;

namespace GUI.Usuarios
{
    internal class UsuarioRepository : IUsuarioRepository
    {

        private readonly string _connectionString;

        public UsuarioRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public void Actualizar(UsuarioC usuario)
        {
            using (var cn = new MySqlConnection(_connectionString))
            using (var cmd = cn.CreateCommand())
            {
                cn.Open();
                cmd.CommandText = @"UPDATE Usuario SET nombreUsuario=@nombreUsuario, contraseña=@contraUsuario, rol=@rolUsuario
                                    WHERE idUsuario=@idUsuario";
                cmd.Parameters.AddWithValue("@nombreUsuario", usuario.NombreUsuario);
                cmd.Parameters.AddWithValue("@contraUsuario", usuario.ContraUsuario);
                cmd.Parameters.AddWithValue("@rolUsuario", usuario.RolUsuario);
                cmd.Parameters.AddWithValue("@idUsuario", usuario.IdUsuario);
                cmd.ExecuteNonQuery();
            }
        }

        public bool Eliminar(string id)
        {
            using (var cn = new MySqlConnection(_connectionString))
            using (var cmd = cn.CreateCommand())
            {
                cn.Open();
                cmd.CommandText = "DELETE FROM Usuario WHERE idUsuario=@idUsuario";
                cmd.Parameters.AddWithValue("@idUsuario", id);
                int filas = cmd.ExecuteNonQuery();
                return filas > 0;
            }
        }

        public void Insertar(UsuarioC usuario)
        {
            using (var cn = new MySqlConnection(_connectionString))
            using (var cmd = cn.CreateCommand())
            {
                cn.Open();
                cmd.CommandText = @"INSERT INTO Usuario (idUsuario, nombreUsuario, contraseña, rol)
                                    VALUES (@idUsuario, @nombreUsuario, @contraUsuario, @rolUsuario)";
                cmd.Parameters.AddWithValue("@idUsuario", usuario.IdUsuario);
                cmd.Parameters.AddWithValue("@nombreUsuario", usuario.NombreUsuario);
                cmd.Parameters.AddWithValue("@contraUsuario", usuario.ContraUsuario);
                cmd.Parameters.AddWithValue("@rolUsuario", usuario.RolUsuario);
                cmd.ExecuteNonQuery();
            }
        }

        public UsuarioC ObtenerPorId(string id)
        {
            UsuarioC p = null;
            using (var cn = new MySqlConnection(_connectionString))
            using (var cmd = cn.CreateCommand())
            {
                cn.Open();
                cmd.CommandText = "SELECT idUsuario, nombreUsuario, rol, contraseña FROM Usuario WHERE idUsuario = @id";
                cmd.Parameters.AddWithValue("@id", id);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        p = new UsuarioC(
                            reader.GetString("idUsuario"),
                            reader.GetString("nombreUsuario"),
                            reader.GetString("contraseña"),
                            reader.GetString("rolUsuario")
                        );
                    }
                }
            }
            return p;
        }

        public IEnumerable<UsuarioC> ObtenerTodos(string filtro = "")
        {
            var lista = new List<UsuarioC>();

            using (var cn = new MySqlConnection(_connectionString))
            using (var cmd = cn.CreateCommand())
            {
                cn.Open();
                cmd.CommandText = @"
                    SELECT idUsuario, nombreUsuario, contraseña, rol
                    FROM Usuario
                    WHERE nombreUsuario LIKE @filtro
                    ORDER BY CAST(SUBSTRING(idUsuario, 4) AS UNSIGNED) DESC";
                cmd.Parameters.AddWithValue("@filtro", "%" + filtro + "%");

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new UsuarioC(
                            reader.GetString("idUsuario"),
                            reader.GetString("nombreUsuario"),
                            reader.GetString("contraseña"),
                            reader.GetString("rol")
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
                cmd.CommandText = "SELECT idUsuario FROM Usuario ORDER BY idUsuario DESC LIMIT 1";
                object obj = cmd.ExecuteScalar();
                return obj == null ? string.Empty : obj.ToString();
            }
        }
    }
}
