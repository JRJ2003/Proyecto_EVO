using GUI.Producto;
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
    public partial class Carga : Form
    {

        public Carga()
        {
            InitializeComponent();
        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }

        private void Carga_Load(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (progressBar1.Value < 100)
            {
                progressBar1.Value += 1;
                label3.Text = progressBar1.Value.ToString() + "%";

                // Acelerar los primeros 90%
                if (progressBar1.Value < 90)
                {
                    timer1.Interval = 10;  // muy rápido
                }
                else
                {
                    if (progressBar1.Value <= 95)
                    {
                        timer1.Interval = 100;
                    }
                    else
                    {
                        timer1.Interval = 500; // más lento en los últimos 10%
                    }
                }
            }
            else
            {
                timer1.Stop();
                this.Hide();
                Form1 form1 = new Form1();
                form1.Show();
            }
        }

    }
}
