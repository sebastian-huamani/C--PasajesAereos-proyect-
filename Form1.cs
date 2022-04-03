using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace T11A
{
    public partial class frmMenu : Form
    {
        private frmPasajeros pasajeros;
        private frmAviones aviones;
        private frmVendedor vendedores;
        public frmMenu()
        {
            InitializeComponent();
        }

        private void pasajerosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pasajeros = new frmPasajeros();
            pasajeros.MdiParent = this;
            pasajeros.Show();
        }

        private void avionesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            aviones = new frmAviones();
            aviones.MdiParent = this;
            aviones.Show();
        }

        private void vendedorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            /*vendedores = new frmVendedor();
            vendedores.MdiParent = this;
            vendedores.Show();*/
            if (vendedores == null)
            {
                vendedores = new frmVendedor();
                vendedores.MdiParent = this;
                vendedores.FormClosed += new FormClosedEventHandler(CerrarFormularioDeVendedores);
                vendedores.Show();
            }
            else
            {
                vendedores.Activate();
            }

        }




        void CerrarFormularioDeVendedores(object sender, FormClosedEventArgs e)
        {
            vendedores = null;
        }








    }
}
