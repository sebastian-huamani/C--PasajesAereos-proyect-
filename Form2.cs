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
    public partial class frmPasajeros : Form
    {
        public frmPasajeros()
        {
            InitializeComponent();
        }

        private void frmPasajeros_Load(object sender, EventArgs e)
        {
            dgvPasajeros.DataSource = ObtenerPasajeros();
        }
        public List<PasajeroDto> ObtenerPasajeros()
        {
            try
            {
                using (BD_LATAM_Entities bd = new BD_LATAM_Entities())
                {
                    var consulta = (from n in bd.Pasajero
                                    orderby n.Nombres
                                    select new PasajeroDto
                                    {
                                        Id = n.Id,
                                        Nombres = n.Nombres,
                                        ApellidoPaterno = n.ApellidoPaterno,
                                        ApellidoMaterno = n.ApellidoMaterno,
                                        Dni = n.Dni,
                                        Activo = n.Activo

                                    }).ToList();
                    return consulta;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void LimpiarFormulario()
        {
            txtId.Text = "";
            txtNombres.Text = "";
            txtApellidoPaterno.Text = "";
            txtApellidoMaterno.Text = "";
            txtDni.Text = "";
            chkActivo.Checked = false;

            txtNombres.Focus();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (FormularioValido())
            {
                Pasajero pasajero = new Pasajero();
                pasajero.Id = 0;
                pasajero.Nombres = txtNombres.Text.Trim();
                pasajero.ApellidoPaterno = txtApellidoPaterno.Text.Trim();
                pasajero.ApellidoMaterno = txtApellidoMaterno.Text.Trim();
                pasajero.Dni = txtDni.Text.Trim();
                pasajero.Activo = chkActivo.Checked;

                //Modificar Pasajero
                if (txtId.Text.Trim().Length > 0)
                {
                    pasajero.Id = Convert.ToInt32(txtId.Text);
                    ActualizarPasajero(pasajero);
                }
                //Nuevo Pasajero
                else
                {
                    InsertarPasajero(pasajero);
                    txtId.Text = ObtenerUltimoPasajero().Id.ToString();
                }

                MessageBox.Show("Datos guardados satisfactoriamente", "Ok", MessageBoxButtons.OK, MessageBoxIcon.Information);

                dgvPasajeros.DataSource = ObtenerPasajeros();
            }
            else
            {
                MessageBox.Show("Todos los datos son obligatorios", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtNombres.Focus();
            }

        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
        }
        private bool FormularioValido()
        {
            bool valido = false;

            string nombres = txtNombres.Text.Trim();
            string apellidoPaterno = txtApellidoPaterno.Text.Trim();
            string apellidoMaterno = txtApellidoMaterno.Text.Trim();
            string dni = txtDni.Text.Trim();

            if (nombres.Length > 0 && apellidoPaterno.Length > 0 && apellidoMaterno.Length > 0 && dni.Length > 0)
                valido = true;
            return valido;
        }
        public void InsertarPasajero(Pasajero pPasajero)
        {
            try
            {
                using (BD_LATAM_Entities bd = new BD_LATAM_Entities())
                {
                    bd.Pasajero.Add(pPasajero);
                    bd.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public PasajeroDto ObtenerUltimoPasajero()
        {
            try
            {
                using (BD_LATAM_Entities bd = new BD_LATAM_Entities())
                {
                    var consulta = (from n in bd.Pasajero
                                    orderby n.Id descending
                                    select new PasajeroDto
                                    {
                                        Id = n.Id,
                                        Nombres = n.Nombres,
                                        ApellidoPaterno = n.ApellidoPaterno,
                                        ApellidoMaterno = n.ApellidoMaterno,
                                        Dni = n.Dni,
                                        Activo = n.Activo

                                    }).ToList();


                    PasajeroDto pasajeroDto = consulta.First();

                    return pasajeroDto;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void dgvPasajeros_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex > -1 && dgvPasajeros.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
            {
                dgvPasajeros.CurrentRow.Selected = true;

                txtId.Text = dgvPasajeros.Rows[e.RowIndex].Cells["Id"].FormattedValue.ToString();
                txtNombres.Text = dgvPasajeros.Rows[e.RowIndex].Cells["Nombres"].FormattedValue.ToString();
                txtApellidoPaterno.Text = dgvPasajeros.Rows[e.RowIndex].Cells["ApellidoPaterno"].FormattedValue.ToString();
                txtApellidoMaterno.Text = dgvPasajeros.Rows[e.RowIndex].Cells["ApellidoMaterno"].FormattedValue.ToString();
                txtDni.Text = dgvPasajeros.Rows[e.RowIndex].Cells["Dni"].FormattedValue.ToString();

                chkActivo.Checked = false;
                if (dgvPasajeros.Rows[e.RowIndex].Cells["Activo"].FormattedValue.ToString() == "True")
                    chkActivo.Checked = true;
            }
        }
        public void ActualizarPasajero(Pasajero pPasajero)
        {
            try
            {
                using (BD_LATAM_Entities bd = new BD_LATAM_Entities())
                {
                    var pasajeros = from p in bd.Pasajero
                                    where p.Id == pPasajero.Id
                                    select p;

                    foreach (Pasajero pas in pasajeros)
                    {
                        pas.Nombres = pPasajero.Nombres;
                        pas.ApellidoPaterno = pPasajero.ApellidoPaterno;
                        pas.ApellidoMaterno = pPasajero.ApellidoMaterno;
                        pas.Dni = pPasajero.Dni;
                        pas.Activo = pPasajero.Activo;
                    }

                    bd.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void EliminarPasajero(Pasajero pPasajero)
        {
            try
            {
                using (BD_LATAM_Entities bd = new BD_LATAM_Entities())
                {
                    var pasajeros = from p in bd.Pasajero
                                    where p.Id == pPasajero.Id
                                    select p;

                    foreach (Pasajero pas in pasajeros)
                    {
                        bd.Pasajero.Remove(pas);
                    }

                    bd.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (txtId.Text.Trim().Length > 0)
            {
                DialogResult respuestaAdvertencia = DialogResult.OK;
                respuestaAdvertencia = MessageBox.Show("¿Está se guro de eliminar el pasajero?", "Eliminar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (respuestaAdvertencia == DialogResult.Yes)
                {
                    Pasajero pasajero = new Pasajero();
                    pasajero.Id = Convert.ToInt32(txtId.Text);

                    EliminarPasajero(pasajero);
                    dgvPasajeros.DataSource = ObtenerPasajeros();

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
