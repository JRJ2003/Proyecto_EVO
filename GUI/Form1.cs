using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using MySql.Data.MySqlClient;

namespace GUI
{
    public partial class Form1 : Form
    {
        MySqlConnection conexion = new MySqlConnection("server=localhost; database=db_empresa; user id=root; password=;");

        public Form1()
        {
            InitializeComponent();
        }
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hwnd,int wmsg, int wparam, int lparam);

        private void btnslide_Click(object sender, EventArgs e)
        {
            if (MenuVertical.Width == 250)
            {
                MenuVertical.Width = 60;
                btninicio.Visible = false;
                btninicio2.Visible = true;
            }
            else
            {
                MenuVertical.Width = 250;
                btninicio.Visible = true;
                btninicio2.Visible = false;
            }
        }

        private void iconcerrar_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void iconrestaurar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            iconrestaurar.Visible = false;
            iconmaximizar.Visible = true;
        }

        private void iconmaximizar_Click(object sender, EventArgs e)
        {
            this.WindowState=FormWindowState.Maximized;
            iconrestaurar.Visible = true;
            iconmaximizar.Visible = false;
        }

        private void iconminimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void BarraTitulo_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnprod_Click(object sender, EventArgs e)
        {
            AbrirFromHija(new PanelProductos());
        }
        private void AbrirFromHija(object formhija)
        {
            if (this.panelContenedor.Controls.Count > 0)
                this.panelContenedor.Controls.RemoveAt(0);
            Form fh = formhija as Form;
            fh.TopLevel = false;
            fh.Dock=DockStyle.Fill;
            this.panelContenedor.Controls.Add(fh);
            this.panelContenedor.Tag = fh;
            fh.Show();

        }

        private void btninicio_Click(object sender, EventArgs e)
        {
            AbrirFromHija(new inicio());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AbrirFromHija(new PanelUsuarios());
        }

        private void button3_Click(object sender, EventArgs e)
        {
            AbrirFromHija(new PanelCompras());
        }

        private void button4_Click(object sender, EventArgs e)
        {
            AbrirFromHija(new PanelProveedores());
        }

        private void btninicio2_Click(object sender, EventArgs e)
        {
            AbrirFromHija(new inicio());
        }

        private void btnVentas_Click(object sender, EventArgs e)
        {
            AbrirFromHija(new PanelVentas());
        }

        private void btnReportes_Click(object sender, EventArgs e)
        {
            AbrirFromHija(new FrmReportes());
        }

        private void btnPanelPrecion_Click(object sender, EventArgs e)
        {
            AbrirFromHija(new FormConfigPrecio());
        }
    }
}
