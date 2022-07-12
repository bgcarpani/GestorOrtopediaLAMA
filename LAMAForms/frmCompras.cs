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

    public partial class frmCompras : Form
    {
        private static frmCompras frm = null;

        public static frmCompras GetInstancia()
        {
            if (frm == null)
            {
                frm = new frmCompras();
                frm.FormClosed += new FormClosedEventHandler(form_FormClosed);

            }
            return frm;
        }

        private static void form_FormClosed(object sender, FormClosedEventArgs e)
        {
            frm = null;
        }

        private frmCompras()
        {
            InitializeComponent();
        }
        List<Compra> lista;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

        }
        private void frmCompras_Load(object sender, EventArgs e)
        {
            this.Dock = DockStyle.Fill;
            ActualizarGrilla();
        }

        private void ActualizarGrilla()
        {
            try
            {
                lista = ComprasBd.GetLista();
                MostrarDatosGrilla(lista);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void MostrarDatosGrilla(List<Compra> lista)
        {
            dgvDatos.Rows.Clear();
            foreach (var compra in lista)
            {
                DataGridViewRow r = new DataGridViewRow();
                r.CreateCells(dgvDatos);
                SetearFila(r, compra);
                AgregarFila(r);
            }
        }

        private void SetearFila(DataGridViewRow r, Compra compra)
        {
            r.Cells[cmnNroCompra.Index].Value = compra.CompraId;
            r.Cells[cmnFechaCompra.Index].Value = compra.FechaCompra;
            if (compra.Proveedor == null)
            {
                r.Cells[cmnProveedor.Index].Value = "Proveedor eliminado";
            }
            else
            {
                r.Cells[cmnProveedor.Index].Value = compra.Proveedor.RazonSocial;
            }
            r.Cells[cmnTotal.Index].Value = compra.Total.ToString("C");

            r.Tag = compra;
        }

        private void AgregarFila(DataGridViewRow r)
        {
            dgvDatos.Rows.Add(r);
        }


        private void tsbVerDetalle_Click(object sender, EventArgs e)
        {
            if (dgvDatos.SelectedRows.Count > 0)
            {
                DataGridViewRow r = dgvDatos.SelectedRows[0];
                Compra compra = (Compra)r.Tag;
                compra.DetallesCompras = DetallesComprasBd.GetDetalles(compra);
                frmVerDetalleCompra frm = new frmVerDetalleCompra();
                frm.SetObjeto(compra);
                DialogResult dr = frm.ShowDialog(this);
            }

        }

        private void tsbBorrar_Click(object sender, EventArgs e)
        {

        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void tsbNuevo_Click_1(object sender, EventArgs e)
        {
            frmComprasAE frm = new frmComprasAE();
            frm.Text = "Nueva compra";
            DialogResult dr = frm.ShowDialog(this);
            if (dr == DialogResult.OK)
            {
                try
                {
                    Compra compra = frm.GetObjeto();
                    ComprasBd.Agregar(compra);
                    DataGridViewRow r = new DataGridViewRow();
                    r.CreateCells(dgvDatos);
                    SetearFila(r, compra);
                    AgregarFila(r);

                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message, "Error");
                }
                ActualizarGrilla();
            }
        }

        private void tsbVerDetalle_Click_1(object sender, EventArgs e)
        {
            if (dgvDatos.SelectedRows.Count > 0)
            {
                DataGridViewRow r = dgvDatos.SelectedRows[0];
                Compra compra = (Compra)r.Tag;
                compra.DetallesCompras = DetallesComprasBd.GetDetalles(compra);
                frmVerDetalleCompra frm = new frmVerDetalleCompra();
                frm.SetObjeto(compra);
                DialogResult dr = frm.ShowDialog(this);
            }
        }

        private void dgvDatos_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (dgvDatos.SelectedRows.Count > 0)
            {
                DataGridViewRow r = dgvDatos.SelectedRows[0];
                Compra compra = (Compra)r.Tag;
                compra.DetallesCompras = DetallesComprasBd.GetDetalles(compra);
                frmVerDetalleCompra frm = new frmVerDetalleCompra();
                frm.SetObjeto(compra);
                DialogResult dr = frm.ShowDialog(this);
            }
        }

        private void tsbSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void tsbEditar_Click(object sender, EventArgs e)
        {
            if (dgvDatos.SelectedRows.Count > 0)
            {
                int posicion = dgvDatos.SelectedRows[0].Index;
                DataGridViewRow row = dgvDatos.SelectedRows[0];
                Compra compra = (Compra)row.Tag;
                Compra compraAux = (Compra)compra.Clone();
                compra.DetallesCompras = DetallesComprasBd.GetDetalles(compra);
                frmComprasAE frm = new frmComprasAE();
                frm.Text = "Editar Compra";
                frm.SetCliente(compra);
                frm.SetEditar(true);
                DialogResult dr = frm.ShowDialog(this);
                if (dr == DialogResult.OK)
                {
                    try
                    {
                        compra = frm.GetCompra();
                        ComprasBd.Editar(compra);
                        SetearFila(row, compra);
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
                            SetearFila(row, compraAux);
                            lista.Remove(compra);
                            lista.Insert(posicion, compraAux);
                        }
                        else
                        {
                            var compraInBd = ComprasBd.GetObjeto(compra.CompraId);
                            if (compraInBd != null)
                            {
                                SetearFila(row, compraInBd);
                                lista.Remove(compra);
                                lista.Insert(posicion, compraInBd);
                            }
                            else
                            {
                                dgvDatos.Rows.Remove(row);
                                lista.Remove(compra);
                            }
                        }
                    }
                }
            }
        }

        private void tsbActualizar_Click(object sender, EventArgs e)
        {
            ActualizarGrilla();
        }

        private void tsbBorrar_Click_1(object sender, EventArgs e)
        {
            if (dgvDatos.SelectedRows.Count > 0)
            {
                DataGridViewRow r = dgvDatos.SelectedRows[0];
                Compra compra = (Compra)r.Tag;
                DialogResult dr = MessageBox.Show($"¿Desea borrar la compra Nro. {compra.CompraId}?",
                    "Confirmar Borrado", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (dr == DialogResult.Yes)
                {
                    try
                    {
                        ComprasBd.Borrar(compra);
                        dgvDatos.Rows.Remove(r);
                        lista.Remove(compra);

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
            List<Compra> listaProvisoria = null;
            try
            {
                if (!int.TryParse(valor, out int ventaId))
                {
                    listaProvisoria = lista.Where(l => l.Proveedor != null).ToList();
                    listaProvisoria = listaProvisoria.Where(l => l.Proveedor.RazonSocial.ToString().ToUpper().Contains(valor.ToUpper())).ToList();
                }
                else
                {
                    listaProvisoria = lista.Where(l => l.CompraId.ToString().Contains(ventaId.ToString())).ToList();
                }
                MostrarDatosGrilla(listaProvisoria);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }

}
