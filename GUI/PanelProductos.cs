using GUI.Producto;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI
{
    public partial class PanelProductos : Form
    {
        private readonly ProductoService _service;

        public PanelProductos()
        {
            InitializeComponent();
            _service = ServiceFactory.CrearProductoService();
        }
        private void RefrescarGrilla(string filtro = "")
        {
            try
            {
                var lista = _service.Listar(filtro).ToList();
                dgvProductos.DataSource = lista;
                dgvProductos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                if (dgvProductos.Columns["IdProducto"] != null) dgvProductos.Columns["IdProducto"].HeaderText = "Código";
                if (dgvProductos.Columns["Nombre"] != null) dgvProductos.Columns["Nombre"].HeaderText = "Nombre";
                if (dgvProductos.Columns["Precio"] != null) dgvProductos.Columns["Precio"].HeaderText = "Precio (S/)";
                if (dgvProductos.Columns["Stock"] != null) dgvProductos.Columns["Stock"].HeaderText = "Stock";
                if (dgvProductos.Columns["Categoria"] != null) dgvProductos.Columns["Categoria"].HeaderText = "Categoría";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar productos: " + ex.Message);
            }
        }

        private void PanelProductos_Load(object sender, EventArgs e)
        {
            RefrescarGrilla();
        }

        private void btnEliminarProducto_Click(object sender, EventArgs e)
        {
            if (dgvProductos.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecciona un producto para eliminar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var fila = dgvProductos.SelectedRows[0];
            string id = fila.Cells["IdProducto"].Value.ToString();
            string nombre = fila.Cells["Nombre"].Value.ToString();

            var confirmar = MessageBox.Show(
                $"¿Seguro que deseas eliminar el producto '{nombre}' (ID: {id})?",
                "Confirmar eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirmar == DialogResult.Yes)
            {
                try
                {
                    _service.Eliminar(id);
                    RefrescarGrilla();
                    MessageBox.Show("Producto eliminado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al eliminar: " + ex.Message);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            RefrescarGrilla(txtBuscar.Text.Trim());
        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            RefrescarGrilla(txtBuscar.Text.Trim());
        }

        private void dgvProductos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var fila = dgvProductos.Rows[e.RowIndex];
                txtIdProducto.Text = fila.Cells["IdProducto"].Value.ToString();
                txtNombre.Text = fila.Cells["Nombre"].Value.ToString();
                txtPrecio.Text = fila.Cells["Precio"].Value.ToString();
                txtStock.Text = fila.Cells["Stock"].Value.ToString();
                txtCategoria.Text = fila.Cells["Categoria"].Value.ToString();
            }
        }

        private void btnAgregarProducto_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
                    string.IsNullOrWhiteSpace(txtPrecio.Text) ||
                    string.IsNullOrWhiteSpace(txtStock.Text) ||
                    string.IsNullOrWhiteSpace(txtCategoria.Text))
                {
                    MessageBox.Show("Completa todos los campos antes de agregar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                ProductoC nuevo = new ProductoC
                {
                    IdProducto = _service.GenerarNuevoId(),
                    Nombre = txtNombre.Text.Trim(),
                    Precio = decimal.Parse(txtPrecio.Text),
                    Stock = int.Parse(txtStock.Text),
                    Categoria = txtCategoria.Text.Trim()
                };

                _service.Crear(nuevo);
                RefrescarGrilla();
                LimpiarCampos();

                MessageBox.Show("Producto agregado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al agregar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEditarProducto_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtIdProducto.Text))
            {
                MessageBox.Show("Selecciona un producto de la lista para modificar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Confirmar acción
            DialogResult confirmar = MessageBox.Show(
                $"¿Deseas guardar los cambios del producto '{txtNombre.Text}'?",
                "Confirmar modificación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirmar != DialogResult.Yes)
                return;

            try
            {
                // Crear objeto actualizado
                ProductoC p = new ProductoC
                {
                    IdProducto = txtIdProducto.Text.Trim(),
                    Nombre = txtNombre.Text.Trim(),
                    Precio = decimal.Parse(txtPrecio.Text),
                    Stock = int.Parse(txtStock.Text),
                    Categoria = txtCategoria.Text.Trim()
                };

                var resultado = _service.Actualizar(p);

                if (resultado.Success)
                {
                    RefrescarGrilla();
                    LimpiarCampos();
                    MessageBox.Show(resultado.Message, "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(resultado.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Verifica los campos de precio y stock (deben ser numéricos).", "Error de formato", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al modificar producto: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void LimpiarCampos()
        {
            txtNombre.Clear();
            txtPrecio.Clear();
            txtStock.Clear();
            txtCategoria.Clear();
        }
    }
}
