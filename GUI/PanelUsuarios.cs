using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GUI.Producto;
using GUI.Usuarios;

namespace GUI
{
    public partial class PanelUsuarios : Form
    {
        private readonly UsuarioService _service;
        public PanelUsuarios()
        {
            InitializeComponent();
            _service = ServiceFactory.CrearUsuarioService();

        }

        private void RefrescarGrilla(string filtro = "")
        {
            try
            {
                var lista = _service.Listar(filtro).ToList();
                dgvUsuarios.DataSource = lista;
                dgvUsuarios.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                if (dgvUsuarios.Columns["IdUsuario"] != null) dgvUsuarios.Columns["IdUsuario"].HeaderText = "IdUsuario";
                if (dgvUsuarios.Columns["Nombre"] != null) dgvUsuarios.Columns["Nombre"].HeaderText = "Nombre";
                if (dgvUsuarios.Columns["Contraseña"] != null) dgvUsuarios.Columns["Contraseña"].HeaderText = "Contraseña";
                if (dgvUsuarios.Columns["RolUsuario"] != null) dgvUsuarios.Columns["RolUsuario"].HeaderText = "RolUsuario";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar usuarios: " + ex.Message);
            }
        }

        private void dgvUsuarios_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void PanelUsuarios_Load(object sender, EventArgs e)
        {
            RefrescarGrilla();

        }

        private void btnAgregarUsuario_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
                    string.IsNullOrWhiteSpace(txtContraseña.Text) ||
                    string.IsNullOrWhiteSpace(txtRol.Text))
                {
                    MessageBox.Show("Completa todos los campos antes de agregar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                UsuarioC nuevo = new UsuarioC
                {
                    IdUsuario = _service.GenerarNuevoId(),
                    NombreUsuario = txtNombre.Text.Trim(),
                    ContraUsuario = txtContraseña.Text.Trim(),
                    RolUsuario = txtRol.Text.Trim(),
                };

                _service.Crear(nuevo);
                RefrescarGrilla();

                MessageBox.Show("Usuario agregado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al agregar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEditarUsuario_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtIdUsuario.Text))
            {
                MessageBox.Show("Selecciona un usuario de la lista para modificar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Confirmar acción
            DialogResult confirmar = MessageBox.Show(
                $"¿Deseas guardar los cambios del usuario '{txtNombre.Text}'?",
                "Confirmar modificación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirmar != DialogResult.Yes)
                return;

            try
            {
                // Crear objeto actualizado
                UsuarioC p = new UsuarioC
                {
                    IdUsuario = txtIdUsuario.Text.Trim(),
                    NombreUsuario = txtNombre.Text.Trim(),
                    ContraUsuario = txtContraseña.Text.Trim(),
                    RolUsuario = txtRol.Text.Trim(),
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
                MessageBox.Show($"Error al modificar usuario: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnEliminarUsuario_Click(object sender, EventArgs e)
        {
            if (dgvUsuarios.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecciona un usuario para eliminar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var fila = dgvUsuarios.SelectedRows[0];
            string id = fila.Cells["IdUsuario"].Value.ToString();

            var confirmar = MessageBox.Show(
                $"¿Seguro que deseas eliminar el usuario (ID: {id})?",
                "Confirmar eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirmar == DialogResult.Yes)
            {
                try
                {
                    _service.Eliminar(id);
                    RefrescarGrilla();
                    MessageBox.Show("Usuario eliminado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void dgvUsuarios_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var fila = dgvUsuarios.Rows[e.RowIndex];
                txtIdUsuario.Text = fila.Cells["IdUsuario"].Value.ToString();
                txtNombre.Text = fila.Cells["NombreUsuario"].Value.ToString();
                txtContraseña.Text = fila.Cells["ContraUsuario"].Value.ToString();
                txtRol.Text = fila.Cells["RolUsuario"].Value.ToString();
            }
        }

        private void LimpiarCampos()
        {
            txtNombre.Clear();
            txtContraseña.Clear();
            txtRol.Clear();
        }
    }
}
