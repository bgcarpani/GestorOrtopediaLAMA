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
    public partial class frmVentas : Form
    {
        public frmVentas()
        {
            InitializeComponent();
        }

        private void tsbSalir_Click(object sender, EventArgs e)
        {
            Close();
        }

        public static frmVentas frm = null;

        public static frmVentas GetInstancia()
        {
            if (frm == null)
            {
                frm = new frmVentas();
                frm.FormClosed += frm_FormClose;
            }
            return frm;
        }

        private static void frm_FormClose(object sender, FormClosedEventArgs e)
        {
            frm = null;
        }

        List<Venta> lista = null;
        private void GrillaInicial(bool devoluciones = false)
        {
            try
            {
                lista = VentasBd.GetLista();
                List<Venta> listaProvisoria = null;
                if (devoluciones)
                {
                    listaProvisoria = lista.Where(v => v.Devuelto).ToList();
                }
                else
                {
                    listaProvisoria = lista;
                }
                MostrarDatosEnGrilla(listaProvisoria);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }

        }

        private void MostrarDatosEnGrilla(List<Venta> lista)
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

        private void SetearFila(DataGridViewRow row, Venta item)
        {
            if (item.Devuelto)
            {
                row.Cells[cmnDevuelto.Index].Style.BackColor = Color.LightGoldenrodYellow;
                row.Cells[cmnDevuelto.Index].Value = "Devuelto";
            }
            else
            {
                row.Cells[cmnDevuelto.Index].Value = "Completado";
            }
            row.Cells[cmnNroVenta.Index].Value = item.VentaId;
            row.Cells[cmnFechaVenta.Index].Value = item.FechaVenta.ToShortDateString();

            if (item.EsConsumidorFinal)
            {
                row.Cells[cmnCliente.Index].Value = "CONSUMIDOR FINAL";
                row.Cells[cmnDNI.Index].Value = "-";
            }
            else
            {
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
            }
            row.Cells[cmnImporte.Index].Value = item.Total.ToString("C");
            row.Tag = item;
        }
        private void tsbNuevo_Click_1(object sender, EventArgs e)
        {

            frmVentasAE frm = new frmVentasAE();
            frm.Text = "Nueva venta";
            DialogResult dr = frm.ShowDialog(this);
            if (dr == DialogResult.OK)
            {
                try
                {
                    Venta venta = frm.GetObjeto();
                    bool esConsumidorFinal = frm.GetConsumidorFinal();
                    VentasBd.Agregar(venta, esConsumidorFinal);
                    if (!esConsumidorFinal)
                    {
                        DialogResult dr2 = MessageBox.Show($"¿Realizar el pago?",
                 "Confirmar Pago", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                        if (dr2 == DialogResult.Yes)
                        {
                            Pago pago = new Pago();
                            pago.Cliente = venta.Cliente;
                            pago.Descripcion = $"VENTA-{venta.VentaId}";
                            pago.FechaPago = venta.FechaVenta;
                            pago.Importe = venta.Total;
                            pago.ImporteOS = venta.ImporteOS;
                            PagosBd.Agregar(pago, 1, pago.Descripcion);
                        }
                    }              
                    DataGridViewRow row = new DataGridViewRow();
                    row.CreateCells(dgvDatos);
                    SetearFila(row, venta);
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
            GrillaInicial();
        }

        private void frmVentas_Load_1(object sender, EventArgs e)
        {
            Dock = DockStyle.Fill;
            GrillaInicial();
        }

        private void tsbSalir_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void tsbVerDetalle_Click_1(object sender, EventArgs e)
        {
            if (dgvDatos.SelectedRows.Count > 0)
            {
                DataGridViewRow r = dgvDatos.SelectedRows[0];
                Venta venta = (Venta)r.Tag;
                venta.Detalle = DetallesVentasBd.GetDetalles(venta);
                frmVerDetalleVenta frm = new frmVerDetalleVenta();
                frm.SetObjeto(venta);
                DialogResult dr = frm.ShowDialog(this);
            }
        }

        private void dgvDatos_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (dgvDatos.SelectedRows.Count > 0)
            {
                DataGridViewRow r = dgvDatos.SelectedRows[0];
                Venta venta = (Venta)r.Tag;
                venta.Detalle = DetallesVentasBd.GetDetalles(venta);
                frmVerDetalleVenta frm = new frmVerDetalleVenta();
                frm.SetObjeto(venta);
                DialogResult dr = frm.ShowDialog(this);
            }
        }

        private void dgvDatos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        //devolucion
        private void tsbEditar_Click(object sender, EventArgs e)
        {
            if (dgvDatos.SelectedRows.Count > 0)
            {
                int posicion = dgvDatos.SelectedRows[0].Index;
                DataGridViewRow row = dgvDatos.SelectedRows[0];
                Venta venta = (Venta)row.Tag;
                venta.Detalle = DetallesVentasBd.GetDetalles(venta);
             
                if (PermiteDevolucion(venta))
                {
                    string mensaje = $"¿Seguro desea realizar la devolución de:";
                    foreach (var item in venta.Detalle)
                    {
                        mensaje = mensaje + $"\n -({item.Cantidad}) {item.Producto.Marca.Descripcion} {item.Producto.Descripcion}";
                    }
                    mensaje = mensaje + "?";
                    DialogResult dr = MessageBox.Show(mensaje,
                      "Confirmar Devolución", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                    if (dr == DialogResult.Yes)
                    {
                        DevolucionVenta devolucion = new DevolucionVenta();
                        devolucion.FechaDevolucion = DateTime.Now;
                        devolucion.Venta = venta;
                        devolucion.Total = venta.Total;
                        List<DetalleDevolucionVenta> detDev = new List<DetalleDevolucionVenta>();
                        foreach (var item in venta.Detalle)
                        {
                            DetalleDevolucionVenta det = new DetalleDevolucionVenta();
                            det.Producto = item.Producto;
                            det.Cantidad = item.Cantidad;
                            det.PrecioUnitario = item.PrecioUnidad;
                            det.Kardex = item.Kardex;
                            det.Total = item.Total;
                            det.DevolucionVenta = devolucion;
                            detDev.Add(det);
                        }
                        devolucion.DetalleDevolucionVentas = detDev;
                        try
                        {
                            DialogResult dr2 = MessageBox.Show("¿Desea mover el producto al stock de Alquileres?",
                           "Confirmar Devolución", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                            bool mover = false;
                            if (dr2 == DialogResult.Yes)
                            {
                                mover = true;
                            }

                            DevolucionVentaBd.Agregar(devolucion, mover);
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

                    }
                    GrillaInicial();

                }
                else
                {
                    MessageBox.Show("El contenido de esta venta ya fue devuelto.", "Error",
                               MessageBoxButtons.OK,
                               MessageBoxIcon.Exclamation);
                }
            }
        }

        private bool PermiteDevolucion(Venta venta)
        {
            if (venta.Devuelto)
            {
                return false;
            }
            return true;
        }


        //Editar
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (dgvDatos.SelectedRows.Count > 0)
            {
                int posicion = dgvDatos.SelectedRows[0].Index;
                DataGridViewRow row = dgvDatos.SelectedRows[0];
                Venta venta = (Venta)row.Tag;
                Venta ventaAUx = (Venta)venta.Clone();
                venta.Detalle = DetallesVentasBd.GetDetalles(venta);
                frmVentasAE frm = new frmVentasAE();
                frm.Text = "Editar Venta";
                frm.SetCliente(venta);
                frm.SetEditar(true);
                DialogResult dr = frm.ShowDialog(this);
                if (dr == DialogResult.OK)
                {
                    bool edicionCtaCte = false;
                    DialogResult dr2 = MessageBox.Show($"¿Desea actualizar la Cuenta Corriente?",
                    "Confirmar Edición", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                    if (dr2 == DialogResult.Yes)
                    {
                        edicionCtaCte = true;
                    }
                    try
                    {
                        venta = frm.GetVenta();
                        VentasBd.Editar(venta, edicionCtaCte);
                        SetearFila(row, venta);
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
                            SetearFila(row, ventaAUx);
                            lista.Remove(venta);
                            lista.Insert(posicion, ventaAUx);
                        }
                        else
                        {
                            var ventaInBd = VentasBd.GetObjeto(venta.VentaId);
                            if (ventaInBd != null)
                            {
                                SetearFila(row, ventaInBd);
                                lista.Remove(venta);
                                lista.Insert(posicion, ventaInBd);
                            }
                            else
                            {
                                dgvDatos.Rows.Remove(row);
                                lista.Remove(venta);
                            }
                        }
                    }
                }
                GrillaInicial();
            }
        }

        private void toolStripTextBox1_TextChanged(object sender, EventArgs e)
        {
            string valor = toolStripTextBox1.Text;
            List<Venta> listaProvisoria = null;
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

        private void tsbBorrar_Click_1(object sender, EventArgs e)
        {
            if (dgvDatos.SelectedRows.Count > 0)
            {
                DataGridViewRow r = dgvDatos.SelectedRows[0];
                Venta venta = (Venta)r.Tag;
                bool edicionCta = false;
                DialogResult dr1 = MessageBox.Show($"¿Seguro que desea eliminar la venta {venta.VentaId}?",
                  "Confirmar Borrado", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (dr1 == DialogResult.Yes)
                {
                    DialogResult dr2 = MessageBox.Show($"¿Desea actualizar cuenta corriente y pagos?",
                 "Confirmar Borrado", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                    if (dr2 == DialogResult.Yes)
                    {
                        edicionCta = true;
                    }
                    try
                    {
                        VentasBd.Borrar(venta, edicionCta);
                        dgvDatos.Rows.Remove(r);
                        lista.Remove(venta);

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
            GrillaInicial();
        }

        private void tsbActualizar_Click(object sender, EventArgs e)
        {
            GrillaInicial();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            GrillaInicial(true);
        }

        private void toolStripButton1_Click_1(object sender, EventArgs e)
        {
            try
            {
                lista = VentasBd.GetLista();
                List<Venta> listaProvisoria = null;
                listaProvisoria = lista.Where(v => v.Devuelto == false).ToList();
                MostrarDatosEnGrilla(listaProvisoria);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        //private void tsbImprimir_Click(object sender, EventArgs e)
        //{
        //    if (dgvDatos.SelectedRows.Count >0)
        //    {
        //        DataGridViewRow row = dgvDatos.SelectedRows[0];
        //        Venta venta = (Venta) row.Tag;
        //        venta.Detalle = DetallesVentasBd.GetDetalles(venta);
        //        VentasRpt rpt = Reportes.GetDatos(venta);
        //    }
        //}
    }
}
