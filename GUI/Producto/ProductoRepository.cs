using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.Producto
{
    internal class ProductoRepository : IProductoRepository
    {
        private readonly string _connectionString;

        public ProductoRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IEnumerable<ProductoC> ObtenerTodos(string filtro = "")
        {
            var lista = new List<ProductoC>();

            using (var cn = new MySqlConnection(_connectionString))
            using (var cmd = cn.CreateCommand())
            {
                cn.Open();
                cmd.CommandText = @"
                    SELECT idProducto, nombre, precio, stock, categoria
                    FROM Producto
                    WHERE nombre LIKE @filtro
                    ORDER BY CAST(SUBSTRING(idProducto, 4) AS UNSIGNED) DESC";
                cmd.Parameters.AddWithValue("@filtro", "%" + filtro + "%");

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new ProductoC(
                            reader.GetString("idProducto"),
                            reader.GetString("nombre"),
                            reader.GetDecimal("precio"),
                            reader.GetInt32("stock"),
                            reader.GetString("categoria")
                        ));
                    }
                }
            }

            return lista;
        }

        public ProductoC ObtenerPorId(string id)
        {
            ProductoC p = null;
            using (var cn = new MySqlConnection(_connectionString))
            using (var cmd = cn.CreateCommand())
            {
                cn.Open();
                cmd.CommandText = "SELECT idProducto, nombre, precio, stock, categoria FROM Producto WHERE idProducto = @id";
                cmd.Parameters.AddWithValue("@id", id);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        p = new ProductoC(
                            reader.GetString("idProducto"),
                            reader.GetString("nombre"),
                            reader.GetDecimal("precio"),
                            reader.GetInt32("stock"),
                            reader.GetString("categoria")
                        );
                    }
                }
            }
            return p;
        }

        public void Insertar(ProductoC producto)
        {
            using (var cn = new MySqlConnection(_connectionString))
            using (var cmd = cn.CreateCommand())
            {
                cn.Open();
                cmd.CommandText = @"INSERT INTO Producto (idProducto, nombre, precio, stock, categoria)
                                    VALUES (@id, @nombre, @precio, @stock, @categoria)";
                cmd.Parameters.AddWithValue("@id", producto.IdProducto);
                cmd.Parameters.AddWithValue("@nombre", producto.Nombre);
                cmd.Parameters.AddWithValue("@precio", producto.Precio);
                cmd.Parameters.AddWithValue("@stock", producto.Stock);
                cmd.Parameters.AddWithValue("@categoria", producto.Categoria);
                cmd.ExecuteNonQuery();
            }
        }

        public void Actualizar(ProductoC producto)
        {
            using (var cn = new MySqlConnection(_connectionString))
            using (var cmd = cn.CreateCommand())
            {
                cn.Open();
                cmd.CommandText = @"UPDATE Producto SET nombre=@nombre, precio=@precio, stock=@stock, categoria=@categoria
                                    WHERE idProducto=@id";
                cmd.Parameters.AddWithValue("@nombre", producto.Nombre);
                cmd.Parameters.AddWithValue("@precio", producto.Precio);
                cmd.Parameters.AddWithValue("@stock", producto.Stock);
                cmd.Parameters.AddWithValue("@categoria", producto.Categoria);
                cmd.Parameters.AddWithValue("@id", producto.IdProducto);
                cmd.ExecuteNonQuery();
            }
        }

        public bool Eliminar(string id)
        {
            using (var cn = new MySqlConnection(_connectionString))
            using (var cmd = cn.CreateCommand())
            {
                cn.Open();
                cmd.CommandText = "DELETE FROM Producto WHERE idProducto=@id";
                cmd.Parameters.AddWithValue("@id", id);
                int filas = cmd.ExecuteNonQuery();
                return filas > 0;
            }
        }

        public string ObtenerUltimoId()
        {
            using (var cn = new MySqlConnection(_connectionString))
            using (var cmd = cn.CreateCommand())
            {
                cn.Open();
                cmd.CommandText = "SELECT idProducto FROM Producto ORDER BY idProducto DESC LIMIT 1";
                object obj = cmd.ExecuteScalar();
                return obj == null ? string.Empty : obj.ToString();
            }
        }
    }
}
