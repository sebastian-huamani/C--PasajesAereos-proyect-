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
    public partial class frmVendedor : Form
    {
        public frmVendedor()
        {
            InitializeComponent();
        }

        private void frmVendedor_Load(object sender, EventArgs e)
        {
            dgvVendedores.DataSource = ObtenerVendedores("");
        }
        public List<SPLATAM_SEL_VENDEDOR_Result> ObtenerVendedores(string pNombre)//
        {
            try
            {
                using (BD_LATAM_Entities bd = new BD_LATAM_Entities())
                {
                    var consulta = bd.SPLATAM_SEL_VENDEDOR(null, pNombre).ToList();

                    return consulta;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void btnGuardar_1_Click(object sender, EventArgs e)
        {
            if (FormularioValido())
            {
                int id = 0;
                string nombres = txtNombres.Text.Trim();
                string apellidoPaterno = txtApellidoPaterno.Text.Trim();
                string apellidoMaterno = txtApellidoMaterno.Text.Trim();
                string dni = txtDni.Text.Trim();
                string salario = txtSalario.Text.Trim();
                bool activo = chkActivo.Checked;

                //Modificar Vendedor
                if (txtId.Text.Trim().Length > 0)
                {
                    id = Convert.ToInt32(txtId.Text);
                    ActualizarVendedor(id, nombres, apellidoPaterno, apellidoMaterno,
                                       dni, Convert.ToDecimal(salario), activo);
                }
                //Nuevo Vendedor
                else
                {
                    id = InsertarVendedor(nombres, apellidoPaterno, apellidoMaterno,
                                          dni, Convert.ToDecimal(salario), activo);
                    txtId.Text = id.ToString();
                }

                MessageBox.Show("Datos guardados satisfactoriamente", "Ok", MessageBoxButtons.OK, MessageBoxIcon.Information);

                dgvVendedores.DataSource = ObtenerVendedores("");
            }


        }
        private void LimpiarFormulario()
        {
            txtId.Text = "";
            txtNombres.Text = "";
            txtApellidoPaterno.Text = "";
            txtApellidoMaterno.Text = "";
            txtDni.Text = "";
            txtSalario.Text = "";
            chkActivo.Checked = false;

            txtNombres.Focus();
        }

        private void btnNuevo_1_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            dgvVendedores.DataSource = ObtenerVendedores(txtBuscar.Text.Trim());
        }
        private bool FormularioValido()
        {
            bool valido = false;

            string nombres = txtNombres.Text.Trim();
            string apellidoPaterno = txtApellidoPaterno.Text.Trim();
            string apellidoMaterno = txtApellidoMaterno.Text.Trim();
            string dni = txtDni.Text.Trim();
            string salario = txtSalario.Text.Trim();

            if (nombres.Length > 0 && apellidoPaterno.Length > 0 && apellidoMaterno.Length > 0 &&
                dni.Length > 0 && salario.Length > 0)
            {
                if (EsNumero(salario))
                {
                    valido = true;
                }
                else
                {
                    MessageBox.Show("El Salario debe ser un número.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtSalario.Text = "";
                    txtSalario.Focus();
                }
            }
            else
            {
                MessageBox.Show("Todos los datos son obligatorios.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtNombres.Focus();
            }

            return valido;
        }

        private bool EsNumero(string pNumero)
        {
            bool esNumero = true;

            try
            {
                decimal numero = Convert.ToDecimal(pNumero);
                return esNumero;
            }
            catch (Exception)
            {
                esNumero = false;
                return esNumero;
            }

        }
        public int InsertarVendedor(string pNombres, string pApellidoPaterno, string pApellidoMaterno,
                             string pDni, decimal pSalario, bool pActivo)
        {
            try
            {
                System.Data.Entity.Core.Objects.ObjectParameter idVendedorNuevo = new System.Data.Entity.Core.Objects.ObjectParameter("oIdVendedor", typeof(int));

                using (BD_LATAM_Entities bd = new BD_LATAM_Entities())
                {
                    bd.SPLATAM_INS_VENDEDOR(pNombres, pApellidoPaterno, pApellidoMaterno,
                                            pDni, pSalario, pActivo, idVendedorNuevo);
                }

                return Convert.ToInt32(idVendedorNuevo.Value.ToString());
            }
            catch (Exception e)
            {
                throw;
                //MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvVendedores_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex > -1 && dgvVendedores.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
            {
                dgvVendedores.CurrentRow.Selected = true;

                txtId.Text = dgvVendedores.Rows[e.RowIndex].Cells["Id"].FormattedValue.ToString();
                txtNombres.Text = dgvVendedores.Rows[e.RowIndex].Cells["Nombres"].FormattedValue.ToString();
                txtApellidoPaterno.Text = dgvVendedores.Rows[e.RowIndex].Cells["ApellidoPaterno"].FormattedValue.ToString();
                txtApellidoMaterno.Text = dgvVendedores.Rows[e.RowIndex].Cells["ApellidoMaterno"].FormattedValue.ToString();
                txtDni.Text = dgvVendedores.Rows[e.RowIndex].Cells["Dni"].FormattedValue.ToString();
                txtSalario.Text = dgvVendedores.Rows[e.RowIndex].Cells["Salario"].FormattedValue.ToString();

                chkActivo.Checked = false;
                if (dgvVendedores.Rows[e.RowIndex].Cells["Activo"].FormattedValue.ToString() == "True")
                    chkActivo.Checked = true;
            }
        }
        public void ActualizarVendedor(int pId, string pNombres, string pApellidoPaterno,
                               string pApellidoMaterno, string pDni, decimal pSalario, bool pActivo)
        {
            try
            {
                using (BD_LATAM_Entities bd = new BD_LATAM_Entities())
                {
                    bd.SPLATAM_UPD_VENDEDOR(pId, pNombres, pApellidoPaterno, pApellidoMaterno,
                                            pDni, pSalario, pActivo);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void EliminarVendedor(int pIdVendedor)
        {
            try
            {
                using (BD_LATAM_Entities bd = new BD_LATAM_Entities())
                {
                    bd.SPLATAM_DEL_VENDEDOR(pIdVendedor);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void btnEliminar_1_Click(object sender, EventArgs e)
        {
            if (txtId.Text.Trim().Length > 0)
            {
                DialogResult respuestaAdvertencia = DialogResult.OK;
                respuestaAdvertencia = MessageBox.Show("¿Está se guro de eliminar el vendedor?", "Eliminar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (respuestaAdvertencia == DialogResult.Yes)
                {
                    EliminarVendedor(Convert.ToInt32(txtId.Text));
                    dgvVendedores.DataSource = ObtenerVendedores("");

                    LimpiarFormulario();
                    MessageBox.Show("Registro eliminado satisfactoriamente", "Ok", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Debe seleccionar el registro que desea eliminar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
