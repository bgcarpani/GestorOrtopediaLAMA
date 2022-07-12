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
    public partial class frmProtesis : Form
    {
        public frmProtesis()
        {
            InitializeComponent();
        }

        private static frmProtesis frm = null;
        public static frmProtesis GetInstancia()
        {
            if (frm == null)
            {
                frm = new frmProtesis();
                frm.FormClosed += frm_FormClosed;
            }
            return frm;
        }

        private static void frm_FormClosed(object sender, FormClosedEventArgs e)
        {
            frm = null;
        }

        private void tsbNuevo_Click(object sender, EventArgs e)
        {
            frmProtesisAE frm = new frmProtesisAE();
            DialogResult dr = frm.ShowDialog(this);
            if (dr == DialogResult.OK)
            {
                Protesis prot = frm.GetProtesis();
                ProtesisBd.Agregar(prot);
                MessageBox.Show("Operación exitosa", "Mensaje",
                     MessageBoxButtons.OK,
                     MessageBoxIcon.Information);
            }
            Actualizar();
        }
        private void frmProtesis_Load(object sender, EventArgs e)
        {
            this.Dock = DockStyle.Fill;
            Actualizar();
        }

        private List<Protesis> lista;
        private void Actualizar()
        {
            try
            {
                lista = ProtesisBd.GetLista();
                MostrarDatosGrilla(lista);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MostrarDatosGrilla(List<Protesis> lista)
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

        private void AgregarFila(DataGridViewRow row)
        {
            dgvDatos.Rows.Add(row);
        }

        private void SetearFila(DataGridViewRow row, Protesis prot)
        {
            row.Cells[cmnId.Index].Value = prot.ProtesisId;
            row.Cells[cmnProtesis.Index].Value = prot.NombreProtesis;
            row.Cells[cmnCosto.Index].Value = prot.Importe.ToString("C");

            row.Tag = prot;
        }

        private void toolStripTextBox1_TextChanged(object sender, EventArgs e)
        {
            string valor = toolStripTextBox1.Text;
            List<Protesis> listaProvisoria = null;
            try
            {
                if (!int.TryParse(valor, out int id))
                {
                    listaProvisoria = lista.Where(l => l.NombreProtesis.ToString().ToUpper().Contains(valor.ToUpper())).ToList();
                }
                else
                {
                    listaProvisoria = lista.Where(l => l.ProtesisId.ToString().Contains(id.ToString())).ToList();
                }
                MostrarDatosGrilla(listaProvisoria);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        private void tsbSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void tsbActualizar_Click(object sender, EventArgs e)
        {
            Actualizar();
        }

        private void tsbBorrar_Click(object sender, EventArgs e)
        {
            if (dgvDatos.SelectedRows.Count > 0)
            {
                DataGridViewRow r = dgvDatos.SelectedRows[0];
                Protesis prot = (Protesis)r.Tag;
                DialogResult dr = MessageBox.Show($"¿Desea borrar la protesis {prot.NombreProtesis}?",
                    "Confirmar Borrado", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (dr == DialogResult.Yes)
                {
                    try
                    {
                        ProtesisBd.Borrar(prot);
                        dgvDatos.Rows.Remove(r);
                        lista.Remove(prot);

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
                Actualizar();
            }
        }

        private void tsbEditar_Click(object sender, EventArgs e)
        {
            if (dgvDatos.SelectedRows.Count > 0)
            {
                int posicion = dgvDatos.SelectedRows[0].Index;

                DataGridViewRow row = dgvDatos.SelectedRows[0];
                Protesis prot = (Protesis)row.Tag;
                Protesis protAux = (Protesis)prot.Clone();
                frmProtesisAE frm = new frmProtesisAE { Text = "Editar Protesis" };
                frm.SetProtesis(prot);
                DialogResult dr = frm.ShowDialog(this);
                if (dr == DialogResult.OK)
                {
                    try
                    {
                        prot = frm.GetProtesis();
                        ProtesisBd.Editar(prot);
                        SetearFila(row, prot);
                        MessageBox.Show("Operación exitosa", "Mensaje",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        if (ex.Message.Contains("repetido"))
                        {
                            SetearFila(row, protAux);
                            lista.Remove(prot);
                            lista.Insert(posicion, protAux);
                        }
                        else
                        {
                            var protInBd = ProtesisBd.GetObjeto(prot.ProtesisId);
                            if (protInBd != null)
                            {
                                SetearFila(row, protInBd);
                                lista.Remove(prot);
                                lista.Insert(posicion, protInBd);
                            }
                            else
                            {
                                dgvDatos.Rows.Remove(row);
                                lista.Remove(prot);
                            }
                        }
                    }
                }
                Actualizar();
            }
        }
    }
    
}
