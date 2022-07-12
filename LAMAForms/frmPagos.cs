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
    public partial class frmPagos : Form
    {
        public frmPagos()
        {
            InitializeComponent();
        }

        private void tsbSalir_Click(object sender, EventArgs e)
        {
            Close();
        }

        private static frmPagos frm = null;
        public static frmPagos GetInstancia()
        {
            if (frm == null)
            {
                frm = new frmPagos();
                frm.FormClosed += frm_FormClose;
            }
            return frm;
        }

        private static void frm_FormClose(object sender, FormClosedEventArgs e)
        {
            frm = null;
        }

        private void frmPagos_Load(object sender, EventArgs e)
        {
            Dock = DockStyle.Fill;
            ActualizarGrilla();
        }

        List<Pago> lista;
        private void ActualizarGrilla()
        {
            try
            {
                lista = PagosBd.GetLista();
                MostrarDatosEnGrilla(lista);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MostrarDatosEnGrilla(List<Pago> lista)
        {
            dgvDatos.Rows.Clear();
            foreach (var item in lista)
            {
                DataGridViewRow row = new DataGridViewRow();
                row.CreateCells(dgvDatos);
                SetearFila(row, item);
                AgregarFila(row);
            }
        }

        private void AgregarFila(DataGridViewRow row)
        {
            dgvDatos.Rows.Add(row);
        }

        private void SetearFila(DataGridViewRow row, Pago item)
        {
            row.Cells[cmnNroPago.Index].Value = item.PagoId;
            if (item.CtaCte == null)
            {
                row.Cells[cmnFecha.Index].Value = "Cuenta eliminada";
            }
            else
            {
                row.Cells[cmnFecha.Index].Value = item.CtaCte.FechaMovimiento.ToShortDateString();
            }
            if (item.Cliente == null)
            {
                row.Cells[cmnCliente.Index].Value = "Cliente eliminado";
                row.Cells[cmnDNI.Index].Value = "-";
            }
            else
            {
                row.Cells[cmnCliente.Index].Value = item.Cliente.ToString();
                row.Cells[cmnDNI.Index].Value = item.Cliente.DNI;
            }
            row.Cells[cmnDesc.Index].Value = item.Descripcion;
            //   row.Cells[cmnForma.Index].Value = item.FormaDePago.Descripcion;
            row.Cells[cmnImporte.Index].Value = item.Importe;

            row.Tag = item;
        }

        private void tsbBorrar_Click(object sender, EventArgs e)
        {
            if (dgvDatos.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dgvDatos.SelectedRows[0];
                Pago pago = (Pago)row.Tag;
                DialogResult dr =
                    MessageBox.Show(
                        $"Desea dar de baja el pago del cliente {pago.Cliente.Nombre} {pago.Cliente.Apellido}",
                        "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (dr == DialogResult.Yes)
                {
                    PagosBd.Borrar(pago);
                    dgvDatos.Rows.Remove(row);
                    SetearFila(row, pago);
                    MessageBox.Show("Pago dado de baja", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
            }
        }

        private void tsbNuevo_Click_1(object sender, EventArgs e)
        {
            frmCtaCtePago frm = new frmCtaCtePago();
            frm.Text = "Ingresar pago";
            ConsultaCtaCte consulta = null;
            frm.SetConsulta(consulta);
            DialogResult dr = frm.ShowDialog(this);
            if (dr == DialogResult.OK)
            {
                ActualizarGrilla();
            }
        }

        private void tsbSalir_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void toolStripTextBox1_TextChanged(object sender, EventArgs e)
        {
            string valor = toolStripTextBox1.Text;
            List<Pago> listaProvisoria = null;
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

        private void tsbBorrar_Click_1(object sender, EventArgs e)
        {
            if (dgvDatos.SelectedRows.Count > 0)
            {
                DataGridViewRow r = dgvDatos.SelectedRows[0];
                Pago pago = (Pago)r.Tag;
                DialogResult dr = MessageBox.Show($"¿Desea borrar el pago Nro. {pago.PagoId}?",
                    "Confirmar Borrado", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (dr == DialogResult.Yes)
                {
                    try
                    {
                        PagosBd.Borrar(pago);
                        dgvDatos.Rows.Remove(r);
                        lista.Remove(pago);

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

        private void tsbEditar_Click(object sender, EventArgs e)
        {
            if (dgvDatos.SelectedRows.Count > 0)
            {
                int posicion = dgvDatos.SelectedRows[0].Index;
                DataGridViewRow row = dgvDatos.SelectedRows[0];
                Pago pago = (Pago)row.Tag;
                Pago pagoAux = (Pago)pago.Clone();
                int idAEditar = pago.PagoId;
                int idCta = pago.CtaCte.CtaCteId;
                frmCtaCtePago frm = new frmCtaCtePago();
                frm.Text = "Editar Pago";
                frm.SetPago(pago);
                frm.SetEditar(true);
                DialogResult dr = frm.ShowDialog(this);
                if (dr == DialogResult.OK)
                {
                    try
                    {
                        pago = frm.GetPago();
                        pago.CtaCte.CtaCteId = idCta;
                        pago.CtaCte.Saldo = pago.CtaCte.Saldo + pago.Importe;
                        PagosBd.Editar(pago, idAEditar, idCta);
                        SetearFila(row, pago);
                        MessageBox.Show("Operación exitosa", "Mensaje",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Exclamation);
                        if (ex.Message.Contains("repetido"))
                        {
                            SetearFila(row, pagoAux);
                            lista.Remove(pago);
                            lista.Insert(posicion, pagoAux);
                        }
                        else
                        {
                            var pagoInBd = PagosBd.GetObjeto(pago.PagoId);
                            if (pagoInBd != null)
                            {
                                SetearFila(row, pagoInBd);
                                lista.Remove(pago);
                                lista.Insert(posicion, pagoInBd);
                            }
                            else
                            {
                                dgvDatos.Rows.Remove(row);
                                lista.Remove(pago);
                            }
                        }
                    }
                }
                ActualizarGrilla();
            }
        }
    }
}
