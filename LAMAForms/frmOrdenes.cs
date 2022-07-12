using LAMADatabase;
using LAMAModels;
using LAMAReportes1;
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
    public partial class frmOrdenes : Form
    {
        public frmOrdenes()
        {
            InitializeComponent();
        }


        public static frmOrdenes frm = null;
        public static frmOrdenes GetInstancia()
        {
            if (frm == null)
            {
                frm = new frmOrdenes();
                frm.FormClosed += frm_FormClosed;
            }
            return frm;
        }

        private static void frm_FormClosed(object sender, FormClosedEventArgs e)
        {
            frm = null;
        }

        private void frmOrdenes_Load(object sender, EventArgs e)
        {
            Dock = DockStyle.Fill;
            GrillaInicial();
        }

        List<Orden> lista;
        private void GrillaInicial()
        {
            try
            {
                lista = OrdenesBd.GetLista();
                MostrarDatosEnGrilla(lista);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }

        }

        private void MostrarDatosEnGrilla(List<Orden> lista)
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

        private void SetearFila(DataGridViewRow row, Orden item)
        {
            row.Cells[cmnNroAlquiler.Index].Value = item.OrdenId;
            if (item.Cliente == null)
            {
                row.Cells[cmnCliente.Index].Value = "Cliente eliminado";
                row.Cells[cmnDni.Index].Value = "-";
            }
            else
            {
                row.Cells[cmnCliente.Index].Value = item.Cliente.NombreCompleto;
                row.Cells[cmnDni.Index].Value = item.Cliente.DNI;
            }

            if (item.Protesis == null)
            {
                row.Cells[cmnNotas.Index].Value = "Prótesis eliminada";
            }
            else
            {
                row.Cells[cmnNotas.Index].Value = item.Protesis.NombreProtesis;
            }
            row.Cells[cmnFechaDesde.Index].Value = item.FechaInicio.ToShortDateString();
            row.Cells[cmnEstimados.Index].Value = item.DiasEstimados + " días";
            if (item.Entregado == false)
            {
                row.Cells[cmnFechaHasta.Index].Value = "Pendiente";
                row.Cells[cmnFechaHasta.Index].Style.BackColor = Color.LightYellow;
            }
            else
            {
                row.Cells[cmnFechaHasta.Index].Value = item.FechaEntrega.ToShortDateString();
                row.Cells[cmnFechaHasta.Index].Style.BackColor = Color.LightGreen;
            }
            row.Cells[cmnImporte.Index].Value = item.Senia.ToString("C");
            row.Cells[cmnCosto.Index].Value = item.Costo.ToString("C");
            row.Tag = item;
        }

        private void dgvDatos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void tsbSalir_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void tsbNuevo_Click(object sender, EventArgs e)
        {

            frmOrdenesAE frm = new frmOrdenesAE();
            frm.Text = "Nueva Orden de Trabajo";
            DialogResult dr = frm.ShowDialog(this);
            if (dr == DialogResult.OK)
            {
                try
                {
                    Orden orden = frm.GetObjeto();
                    int ordenId = OrdenesBd.Agregar(orden);
                    orden.OrdenId = ordenId;
                    DialogResult dr2 = MessageBox.Show("¿Desea generar el contrato de la orden ahora?",
                        "Confirmar Orden de Trabajo", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                    if (dr2 == DialogResult.Yes)
                    {
                        if (dgvDatos.SelectedRows.Count > 0)
                        {
                            rptContratoOrden rpt = Reportes.GetDatos(orden);
                            frmReportes1 frmRpt = new frmReportes1();
                            frmRpt.SetReporte(rpt);
                            frmRpt.Show();

                        }
                    }


                    DataGridViewRow row = new DataGridViewRow();
                    row.CreateCells(dgvDatos);
                    SetearFila(row, orden);
                    AgregarFila(row);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            GrillaInicial();
        }

        private void btnEntrega_Click(object sender, EventArgs e)
        {
            DialogResult dr1 = MessageBox.Show($"¿Seguro que desea realizar la entrega?",
                            "Confirmar pago", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (dr1 == DialogResult.Yes)
            {
                if (dgvDatos.SelectedRows.Count > 0)
                {
                    int posicion = dgvDatos.SelectedRows[0].Index;
                    DataGridViewRow row = dgvDatos.SelectedRows[0];
                    Orden ordi = (Orden)row.Tag;
                    Orden ord = OrdenesBd.GetObjeto(ordi.OrdenId);
                    if (ord.Entregado == false)
                    {
                        try
                        {
                            OrdenesBd.Entrega(ord);
                            if (ord.Cliente != null)
                            {
                                if (!(ord.Senia + ord.ImporteOS >= ord.Costo))
                                {
                                    DialogResult dr2 = MessageBox.Show($"¿Pagó el resto del importe?",
                                "Confirmar pago", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                                    if (dr2 == DialogResult.Yes)
                                    {
                                        Pago pago = new Pago();
                                        pago.Cliente = ord.Cliente;
                                        pago.Descripcion = $"ORDEN-{ord.OrdenId}";
                                        pago.FechaPago = DateTime.Now;
                                        pago.Importe = ord.Costo - ord.Senia;
                                        PagosBd.Agregar(pago, 4, pago.Descripcion);
                                    }
                                }
                                SetearFila(row, ord);
                            }
                            MessageBox.Show("Operación exitosa", "Mensaje",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Exclamation);
                        }
                        GrillaInicial();
                    }
                    else
                    {
                        MessageBox.Show("Este item ya ha sido entregado.", "Mensaje",
                               MessageBoxButtons.OK,
                               MessageBoxIcon.Warning);
                    }
                }
            }
            
        }

        private void tsbBorrar_Click(object sender, EventArgs e)
        {
            if (dgvDatos.SelectedRows.Count > 0)
            {
                DataGridViewRow r = dgvDatos.SelectedRows[0];
                Orden ord = (Orden)r.Tag;
                DialogResult dr = MessageBox.Show($"¿Desea borrar la orden Nro. {ord.OrdenId}?",
                    "Confirmar Borrado", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (dr == DialogResult.Yes)
                {
                    try
                    {
                        OrdenesBd.Borrar(ord);
                        CtaCte cc = CtaCteBd.GetObjeto(ord.OrdenId, "OT");
                        CtaCteBd.BorrarUna(cc.CtaCteId);
                        CtaCteBd.BorrarPorReferido("ORDEN", ord.OrdenId);
                        PagosBd.Borrar("ORDEN", ord.OrdenId);
                        TransaccionesBd.Borrar("ORDEN", ord.OrdenId);
                        dgvDatos.Rows.Remove(r);
                        lista.Remove(ord);

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

        private void toolStripTextBox1_TextChanged(object sender, EventArgs e)
        {
            string valor = toolStripTextBox1.Text;
            List<Orden> listaProvisoria = null;
            try
            {
                if (!int.TryParse(valor, out int dni))
                {
                    listaProvisoria = lista.Where(l => l.Cliente != null).ToList();
                    listaProvisoria = listaProvisoria.Where(l => l.Cliente.ToString().ToUpper().Contains(valor.ToUpper())).ToList();
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
            GrillaInicial();
        }

        private void contratoDeAlquilerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvDatos.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dgvDatos.SelectedRows[0];
                Orden ord = (Orden)row.Tag;
                int ordenId = ord.OrdenId;
                Orden orden = OrdenesBd.GetObjeto(ordenId);
                rptContratoOrden rpt = Reportes.GetDatos(orden);
                frmReportes1 frm = new frmReportes1();
                frm.SetReporte(rpt);
                frm.Show();
                
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            List<Orden> listaProvisoria = new List<Orden>();
            listaProvisoria = lista.Where(o=>o.Entregado == false).ToList();
            MostrarDatosEnGrilla(listaProvisoria);
        }
    }
}
