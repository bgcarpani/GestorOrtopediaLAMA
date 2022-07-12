using LAMADatabase;
using LAMAModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LAMAForms
{
    public partial class frmLocalidades : Form
    {
        private static frmLocalidades frm = null;
        public static frmLocalidades GetInstancia()
        {
            if (frm == null)
            {
                frm = new frmLocalidades();
                frm.FormClosed += frm_FormClosed;
            }
            return frm;
        }

        private static void frm_FormClosed(object sender, FormClosedEventArgs e)
        {
            frm = null;
        }

        private frmLocalidades()
        {
            InitializeComponent();
        }

        private void frmLocalidades_Load(object sender, EventArgs e)
        {
            this.Dock = DockStyle.Fill;
            Actualizar();

        }





        private void EditarMenuItem_Click(object sender, EventArgs e)
        {
            Editar();
        }

        private void BorrarMenuItem_Click(object sender, EventArgs e)
        {
            Borrar();
        }

        private void AgregarMenuItem_Click(object sender, EventArgs e)
        {
            Nuevo();
        }

        private void Actualizar()
        {
            try
            {
                lista = LocalidadesBd.GetLista();
                MostrarDatosGrilla(lista);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MostrarDatosGrilla(List<Localidad> lista)
        {
            dgvDatos.Rows.Clear();
            foreach (var localidad in lista)
            {
                DataGridViewRow row = new DataGridViewRow();
                row.CreateCells(dgvDatos);
                SetearFila(row, localidad);
                AgregarFila(row);
            }
        }

        private void SetearFila(DataGridViewRow row, Localidad localidad)
        {
            row.Cells[cmnProvincia.Index].Value = localidad.Provincia.NombreProvincia;
            row.Cells[cmnLocalidad.Index].Value = localidad.Descripcion;

            row.Tag = localidad;
        }

        private void AgregarFila(DataGridViewRow row)
        {
            dgvDatos.Rows.Add(row);
        }

        private List<Localidad> lista;
        private void tsbNuevo_Click(object sender, EventArgs e)
        {
            Nuevo();
        }

        private void Nuevo()
        {
            frmLocalidadesAE frm = new frmLocalidadesAE();
            frm.Text = "Agregar Localidad";
            DialogResult dr = frm.ShowDialog(this);
            if (dr == DialogResult.OK)
            {
                try
                {
                    Localidad localidad = frm.GetLocalidad();
                    LocalidadesBd.Agregar(localidad);
                    DataGridViewRow row = new DataGridViewRow();
                    row.CreateCells(dgvDatos);
                    SetearFila(row, localidad);
                    AgregarFila(row);
                    MessageBox.Show("Operación exitosa", "Mensaje",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void Editar()
        {
            if (dgvDatos.SelectedRows.Count > 0)
            {
                int posicion = dgvDatos.SelectedRows[0].Index;

                DataGridViewRow row = dgvDatos.SelectedRows[0];
                Localidad localidad = (Localidad)row.Tag;
                Localidad localidadAux = (Localidad)localidad.Clone();
                frmLocalidadesAE frm = new frmLocalidadesAE { Text = "Editar Localidad" };
                frm.SetLocalidad(localidad);
                DialogResult dr = frm.ShowDialog(this);
                if (dr == DialogResult.OK)
                {
                    try
                    {
                        localidad = frm.GetLocalidad();
                        LocalidadesBd.Editar(localidad);
                        SetearFila(row, localidad);
                        MessageBox.Show("Operación exitosa", "Mensaje",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        if (ex.Message.Contains("repetido"))
                        {
                            SetearFila(row, localidadAux);
                            lista.Remove(localidad);
                            lista.Insert(posicion, localidadAux);
                        }
                        else
                        {
                            var localidadInBd = LocalidadesBd.GetObjeto(localidad.LocalidadId);
                            if (localidadInBd != null)
                            {
                                SetearFila(row, localidadInBd);
                                lista.Remove(localidad);
                                lista.Insert(posicion, localidadInBd);
                            }
                            else
                            {
                                dgvDatos.Rows.Remove(row);
                                lista.Remove(localidad);
                            }
                        }
                    }
                }
            }
        }

        private void tsbBorrar_Click(object sender, EventArgs e)
        {
            Borrar();
        }

        private void Borrar()
        {
            if (dgvDatos.SelectedRows.Count > 0)
            {
                DataGridViewRow r = dgvDatos.SelectedRows[0];
                Localidad localidad = (Localidad)r.Tag;
                DialogResult dr = MessageBox.Show($"¿Desea borrar la localidad {localidad.Descripcion}?",
                    "Confirmar Borrado", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (dr == DialogResult.Yes)
                {
                    try
                    {
                        LocalidadesBd.Borrar(localidad);
                        dgvDatos.Rows.Remove(r);
                        //*****************************
                        //sacar el objeto de la lista
                        //*******************************
                        lista.Remove(localidad);

                        MessageBox.Show("Operación exitosa", "Mensaje",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void frmLocalidades_Load_1(object sender, EventArgs e)
        {
            this.Dock = DockStyle.Fill;
            Actualizar();
        }

        private void tsbBorrar_Click_1(object sender, EventArgs e)
        {
            Borrar();
        }

        private void tsbSalir_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void tsbActualizar_Click_1(object sender, EventArgs e)
        {
            Actualizar();
        }

        private void tsbEditar_Click_1(object sender, EventArgs e)
        {
            Editar();
        }
    }
}
