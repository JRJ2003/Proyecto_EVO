using GUI.Producto;
using System;
using System.Linq;
using System.Windows.Forms;

namespace GUI
{
    public partial class FormConfigPrecio : Form
    {
        private readonly ProductoService _service;

        public FormConfigPrecio()
        {
            InitializeComponent();
            _service = ServiceFactory.CrearProductoService(); // igual que en PanelProductos
        }

        private void FormConfigPrecio_Load(object sender, EventArgs e)
        {
            var productos = _service.Listar().ToList();
            cmbProductos.DataSource = productos;
            cmbProductos.DisplayMember = "IdProducto"; // Mostrar código
            cmbProductos.ValueMember = "IdProducto";

            // Inicialmente, radiobutton "Todos" no marcado
            rdbTodos.Checked = false;
            cmbProductos.Enabled = true;
        }
        private void label8_Click(object sender, EventArgs e)
        {

        }

        

        // Opcional: limpiar el textbox después de aplicar descuento
        private void LimpiarCampos()
        {
            txtPorcentaje.Clear();
        }

        private void btnAplicarTodos_Click_1(object sender, EventArgs e)
        {
            if (!decimal.TryParse(txtPorcentaje.Text, out decimal porcentaje))
            {
                MessageBox.Show("Ingrese un valor numérico válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (porcentaje < 10 || porcentaje > 70)
            {
                MessageBox.Show("El porcentaje debe estar entre 10 y 70.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirmar = MessageBox.Show(
                $"Se aplicará un descuento del {porcentaje}% a los productos seleccionados. ¿Desea continuar?",
                "Confirmar descuento",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirmar != DialogResult.Yes)
                return;

            // Si está marcado "Todos"
            if (rdbTodos.Checked)
            {
                var resultado = _service.AplicarDescuentoATodos(porcentaje);
                MessageBox.Show(resultado.Message,
                    resultado.Success ? "Éxito" : "Error",
                    MessageBoxButtons.OK,
                    resultado.Success ? MessageBoxIcon.Information : MessageBoxIcon.Error);
            }
            else
            {
                // Descuento solo a los productos en el listBox
                foreach (string id in listBoxProduc.Items)
                {
                    var producto = _service.Obtener(id);

                    // Respaldar precio original si aún no está en la lista estática
                    _service.RespaldarPrecioOriginal(producto);


                    producto.Precio -= producto.Precio * (porcentaje / 100);
                    if (producto.Precio < 0) producto.Precio = 0;
                    _service.Actualizar(producto);
                }

                MessageBox.Show("Descuento aplicado a los productos seleccionados.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnRestaurar_Click_1(object sender, EventArgs e)
        {
            try
            {
                var resultado = _service.RestaurarPreciosOriginales();

                MessageBox.Show(resultado.Message,
                    resultado.Success ? "Éxito" : "Error",
                    MessageBoxButtons.OK,
                    resultado.Success ? MessageBoxIcon.Information : MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al restaurar precios: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSeleccionar_Click(object sender, EventArgs e)
        {
            if (cmbProductos.SelectedItem == null) return;

            var seleccionado = ((ProductoC)cmbProductos.SelectedItem).IdProducto;

            if (!listBoxProduc.Items.Contains(seleccionado))
                listBoxProduc.Items.Add(seleccionado);
        }

        private void rdbTodos_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbTodos.Checked)
            {
                listBoxProduc.Items.Clear();
                cmbProductos.Enabled = false;
            }
            else
            {
                cmbProductos.Enabled = true;
            }
        }
    }
}
