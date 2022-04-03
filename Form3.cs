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
    public partial class frmAviones : Form
    {
        public frmAviones()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            dgvAviones.DataSource = ObtenerAviones();
        }
        public List<AvionesDto> ObtenerAviones()
        {
            try
            {
                using (BD_LATAM_Entities bd = new BD_LATAM_Entities())
                {
                    var consulta = (from n in bd.Avion
                                    orderby n.Modelo
                                    select new AvionesDto
                                    {
                                        Id = n.Id,
                                        Modelo = n.Modelo,
                                        Matricula = n.Matricula,
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
            txtModelo.Text = "";
            txtMatricula.Text = "";
            chkActivo.Checked = false;
            txtModelo.Focus();
        }
        private bool FormularioValido()
        {
            bool valido = false;

            string Modelo = txtModelo.Text.Trim();
            string Matricula = txtMatricula.Text.Trim();

            if (Modelo.Length > 0 && Matricula.Length > 0)
                valido = true;
            return valido;
        }
        public void InsertarPasajero(Avion pAvion)
        {
            try
            {
                using (BD_LATAM_Entities bd = new BD_LATAM_Entities())
                {
                    bd.Avion.Add(pAvion);
                    bd.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public AvionesDto ObtenerUltimoAvion()
        {
            try
            {
                using (BD_LATAM_Entities bd = new BD_LATAM_Entities())
                {
                    var consulta = (from n in bd.Avion
                                    orderby n.Id descending
                                    select new AvionesDto
                                    {
                                        Id = n.Id,
                                        Modelo = n.Modelo,
                                        Matricula = n.Matricula,
                                        Activo = n.Activo

                                    }).ToList();


                    AvionesDto avionDto = consulta.First();

                    return avionDto;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    
       
        public void ActualizarAviones(Avion aAvion)
        {
            try
            {
                using (BD_LATAM_Entities bd = new BD_LATAM_Entities())
                {
                    var aviones = from p in bd.Avion
                                  where p.Id == aAvion.Id
                                  select p;

                    foreach (Avion avi in aviones)
                    {
                        avi.Modelo = aAvion.Modelo;
                        avi.Matricula = aAvion.Matricula;
                        avi.Activo = aAvion.Activo;
                    }

                    bd.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void EliminarPasajero(Avion aAvion)
        {
            try
            {
                using (BD_LATAM_Entities bd = new BD_LATAM_Entities())
                {
                    var aviones = from p in bd.Avion
                                  where p.Id == aAvion.Id
                                  select p;

                    foreach (Avion avi in aviones)
                    {
                        bd.Avion.Remove(avi);
                    }

                    bd.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void btnGuardar_Click_1(object sender, EventArgs e)
        {
            if (FormularioValido())
            {
                Avion avion = new Avion();
                avion.Id = 0;
                avion.Modelo = txtModelo.Text.Trim();
                avion.Matricula = txtMatricula.Text.Trim();
                avion.Activo = chkActivo.Checked;

                //Modificar Pasajero
                if (txtId.Text.Trim().Length > 0)
                {
                    avion.Id = Convert.ToInt32(txtId.Text);
                    ActualizarAviones(avion);
                }
                //Nuevo Pasajero
                else
                {
                    InsertarPasajero(avion);
                    txtId.Text = ObtenerUltimoAvion().Id.ToString();
                }

                MessageBox.Show("Datos guardados satisfactoriamente", "Ok", MessageBoxButtons.OK, MessageBoxIcon.Information);

                dgvAviones.DataSource = ObtenerAviones();
            }
            else
            {
                MessageBox.Show("Todos los datos son obligatorios", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtModelo.Focus();
            }
        }

        private void btnNuevo_Click_1(object sender, EventArgs e)
        {
            LimpiarFormulario();
        }

        private void btnEliminar_Click_1(object sender, EventArgs e)
        {
            if (txtId.Text.Trim().Length > 0)
            {
                DialogResult respuestaAdvertencia = DialogResult.OK;
                respuestaAdvertencia = MessageBox.Show("¿Está se guro de eliminar el Avion?", "Eliminar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (respuestaAdvertencia == DialogResult.Yes)
                {
                    Avion avion = new Avion();
                    avion.Id = Convert.ToInt32(txtId.Text);

                    EliminarPasajero(avion);
                    dgvAviones.DataSource = ObtenerAviones();

                    LimpiarFormulario();
                    MessageBox.Show("Registro eliminado satisfactoriamente", "Ok", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Debe seleccionar el registro que desea eliminar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvAviones_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex > -1 && dgvAviones.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
            {
                dgvAviones.CurrentRow.Selected = true;

                txtId.Text = dgvAviones.Rows[e.RowIndex].Cells["Id"].FormattedValue.ToString();
                txtModelo.Text = dgvAviones.Rows[e.RowIndex].Cells["Modelo"].FormattedValue.ToString();
                txtMatricula.Text = dgvAviones.Rows[e.RowIndex].Cells["Matricula"].FormattedValue.ToString();

                chkActivo.Checked = false;
                if (dgvAviones.Rows[e.RowIndex].Cells["Activo"].FormattedValue.ToString() == "True")
                    chkActivo.Checked = true;
            }
        }
    }
}
