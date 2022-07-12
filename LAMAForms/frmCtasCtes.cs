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
    public partial class frmCtasCtes : Form
    {
        public frmCtasCtes()
        {
            InitializeComponent();
        }

        public static frmCtasCtes frm = null;
        public static frmCtasCtes GetInstancia()
        {
            if (frm == null)
            {
                frm = new frmCtasCtes();
                frm.FormClosed += frm_FormClosed;
            }
            return frm;
        }

        private static void frm_FormClosed(object sender, FormClosedEventArgs e)
        {
            frm = null;
        }

        private void frmCtasCtes_Load(object sender, EventArgs e)
        {
            Dock = DockStyle.Fill;
            ActualizarGrilla();
        }

        private ConsultaCtaCte consulta;

        List<ConsultaCtaCte> lista = null;

        private void ActualizarGrilla()
        {
            try
            {
                lista = CtaCteBd.GetSaldos();
                lista = lista.OrderBy(c => c.Cliente.NombreCompleto).ToList();
                MostrarDatosEnGrilla(lista);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void MostrarDatosEnGrilla(List<ConsultaCtaCte> lista)
        {
            dgvDatos.Rows.Clear();
            foreach (var item in lista)
            {
                DataGridViewRow r = new DataGridViewRow();
                r.CreateCells(dgvDatos);
                SetearFila(r, item);
                AgregarFila(r);
            }
        }

        private void AgregarFila(DataGridViewRow r)
        {
            dgvDatos.Rows.Add(r);
        }

        private void SetearFila(DataGridViewRow r, ConsultaCtaCte item)
        {
            r.Cells[cmnCliente.Index].Value = item.Cliente.ToString();
            r.Cells[cmnDNI.Index].Value = item.Cliente.DNI;
            r.Cells[cmnSaldo.Index].Value = item.Saldo;
            if (item.Saldo < 0)
            {
                r.Cells[cmnSaldo.Index].Style.BackColor = Color.Red;
            }
            else if (item.Saldo >= 0)
            {
                r.Cells[cmnSaldo.Index].Style.BackColor = Color.Green;

            }
            r.Tag = item;
        }

        private void tsbSalir_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void tsbVerDetalle_Click(object sender, EventArgs e)
        {
            if (dgvDatos.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dgvDatos.SelectedRows[0];
                ConsultaCtaCte consulta = (ConsultaCtaCte)row.Tag;
                frmDetalleCtaCte frm = new frmDetalleCtaCte();
                frm.Text = "Detalle de cuenta corriente";
                frm.SetCtaCte(consulta);
                DialogResult dr = frm.ShowDialog(this);
                if (dr == DialogResult.OK)
                {
                    ActualizarGrilla();
                }
            }
        }

        //private void tsbImprimir_Click_1(object sender, EventArgs e)
        //{
        //    if (dgvDatos.SelectedRows.Count > 0)
        //    {
        //        DataGridViewRow row = dgvDatos.SelectedRows[0];
        //        ConsultaCtaCte consulta = (ConsultaCtaCte)row.Tag;
        //        consulta.DetalleCtaCte = CtaCteBd.GetDetalle(consulta.Cliente);
        //        rpt_CtaCte rpt = Reportes.GetDatos(consulta);
        //        frmReportes frm = new frmReportes();
        //        frm.SetReporte(rpt);
        //        frm.Show();
        //    }
        //}

        private void toolStripSeparator1_Click(object sender, EventArgs e)
        {

        }

        private void tsbNuevo_Click(object sender, EventArgs e)
        {

        }

        private void tsbPagos_Click_1(object sender, EventArgs e)
        {
            if (dgvDatos.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dgvDatos.SelectedRows[0];
                ConsultaCtaCte consulta = (ConsultaCtaCte)row.Tag;
                frmCtaCtePago frm = new frmCtaCtePago();
                frm.Text = "Ingresar pago";
                frm.SetConsulta(consulta);
                DialogResult dr = frm.ShowDialog(this);
                if (dr == DialogResult.OK)
                {
                    ActualizarGrilla();
                }
            }
        }

        private void tsbSalir_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgvDatos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void tsbNuevo_Click_1(object sender, EventArgs e)
        {
        }

        //BORRAR
        private void tsbVerDetalle_Click_1(object sender, EventArgs e)
        {
            if (dgvDatos.SelectedRows.Count > 0)
            {
                DataGridViewRow r = dgvDatos.SelectedRows[0];
                
                ConsultaCtaCte cta = (ConsultaCtaCte)r.Tag;
                DialogResult dr = MessageBox.Show($"¿Está seguro que desea borrar la cuenta corriente de {cta.Cliente.NombreCompleto}? Se borrará la cuenta por completo.",
                    "Confirmar Borrado", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (dr == DialogResult.Yes)
                {
                    try
                    {
                        CtaCteBd.BorrarCompleta(cta.Cliente.ClienteId);
                        dgvDatos.Rows.Remove(r);

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
            ActualizarGrilla();
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (dgvDatos.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dgvDatos.SelectedRows[0];
                ConsultaCtaCte consulta = (ConsultaCtaCte)row.Tag;
                frmDetalleCtaCte frm = new frmDetalleCtaCte();
                frm.Text = "Detalle de cuenta corriente";
                frm.SetCtaCte(consulta);
                DialogResult dr = frm.ShowDialog(this);
                if (dr == DialogResult.OK)
                {
                    ActualizarGrilla();
                }
            }
        }

        private void toolStripTextBox1_TextChanged(object sender, EventArgs e)
        {
            string valor = toolStripTextBox1.Text;
            List<ConsultaCtaCte> listaProvisoria = null;
            try
            {
                if (!int.TryParse(valor, out int dni))
                {
                    listaProvisoria = lista.Where(l => l.Cliente.ToString().ToUpper().Contains(valor.ToUpper())).ToList();
                }
                else
                {
                    listaProvisoria = lista.Where(l => l.Cliente.DNI.ToString().Contains(dni.ToString())).ToList();
                }
                MostrarDatosEnGrilla(listaProvisoria);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void tsbActualizar_Click(object sender, EventArgs e)
        {
            ActualizarGrilla();
        }
    }

}
