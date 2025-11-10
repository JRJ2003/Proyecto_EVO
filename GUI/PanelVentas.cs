using GUI.Usuarios;
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
    public partial class PanelVentas : Form
    {
        private readonly VentaService _service;
        public PanelVentas()
        {
            InitializeComponent();
            _service = ServiceFactory.CrearVentaService();

        }

        private void RefrescarGrilla(string filtro = "")
        {
            try
            {
                var lista = _service.Listar(filtro).ToList();
                dgvVenta.DataSource = lista;
                dgvVenta.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                if (dgvVenta.Columns["IdVenta"] != null) dgvVenta.Columns["IdVenta"].HeaderText = "IdVenta";
                if (dgvVenta.Columns["IdUsuario"] != null) dgvVenta.Columns["IdUsuario"].HeaderText = "IdUsuario";
                if (dgvVenta.Columns["Fecha"] != null) dgvVenta.Columns["Fecha"].HeaderText = "Fecha";
                if (dgvVenta.Columns["Total"] != null) dgvVenta.Columns["Total"].HeaderText = "Total";
                if (dgvVenta.Columns["Productos"] != null) dgvVenta.Columns["Productos"].HeaderText = "Productos";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar ventas: " + ex.Message);
            }
        }

        private void PanelVentas_Load(object sender, EventArgs e)
        {
            RefrescarGrilla();
        }

        private void btnAgregarCompras_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtIdUsuario.Text) ||
                    string.IsNullOrWhiteSpace(txtFecha.Text) ||
                    string.IsNullOrWhiteSpace(txtTotal.Text) ||
                    string.IsNullOrWhiteSpace(txtProductos.Text))
                {
                    MessageBox.Show("Completa todos los campos antes de agregar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                VentaC nuevo = new VentaC
                {
                    IdVenta = _service.GenerarNuevoId(),
                    IdUsuario = txtIdUsuario.Text.Trim(),
                    Fecha = DateTime.Parse(txtFecha.Text),
                    Total = decimal.Parse(txtTotal.Text.Trim()),
                    ListaProductos = txtProductos.Text.Trim(),
                };

                _service.Crear(nuevo);
                RefrescarGrilla();

                MessageBox.Show("Venta agregado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al agregar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEditarCompras_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtIdUsuario.Text))
            {
                MessageBox.Show("Selecciona una venta de la lista para modificar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Confirmar acción
            DialogResult confirmar = MessageBox.Show(
                $"¿Deseas guardar los cambios de la venta '{txtIdVenta.Text}'?",
                "Confirmar modificación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirmar != DialogResult.Yes)
                return;

            try
            {
                // Crear objeto actualizado
                VentaC p = new VentaC
                {
                    IdVenta = txtIdVenta.Text.Trim(),
                    IdUsuario = txtIdUsuario.Text.Trim(),
                    Fecha = DateTime.Parse(txtFecha.Text),
                    Total = decimal.Parse(txtTotal.Text.Trim()),
                    ListaProductos = txtProductos.Text.Trim(),
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
            catch (Exception ex)
            {
                MessageBox.Show($"Error al modificar venta: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEliminarCompras_Click(object sender, EventArgs e)
        {
            if (dgvVenta.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecciona una venta para eliminar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var fila = dgvVenta.SelectedRows[0];
            string id = fila.Cells["IdVenta"].Value.ToString();

            var confirmar = MessageBox.Show(
                $"¿Seguro que deseas eliminar el venta (ID: {id})?",
                "Confirmar eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirmar == DialogResult.Yes)
            {
                try
                {
                    _service.Eliminar(id);
                    RefrescarGrilla();
                    MessageBox.Show("Venta eliminado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al eliminar: " + ex.Message);
                }
            }

        }

        private void dgvCompras_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var fila = dgvVenta.Rows[e.RowIndex];
                txtIdVenta.Text = fila.Cells["IdVenta"].Value.ToString();
                txtIdUsuario.Text = fila.Cells["IdUsuario"].Value.ToString();
                txtFecha.Text = fila.Cells["Fecha"].Value.ToString();
                txtTotal.Text = fila.Cells["Total"].Value.ToString();
                txtProductos.Text = fila.Cells["ListaProductos"].Value.ToString();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            RefrescarGrilla(txtBuscar.Text.Trim());
        }

        private void LimpiarCampos()
        {
            txtIdUsuario.Clear();
            txtFecha.Clear();
            txtTotal.Clear();
            txtProductos.Clear();
        }
    }
}
