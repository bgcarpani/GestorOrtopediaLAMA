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
    public partial class frmAlquileres : Form
    {
        public frmAlquileres()
        {
            InitializeComponent();
        }

        public static frmAlquileres frm = null;
        public static frmAlquileres GetInstancia()
        {
            if (frm == null)
            {
                frm = new frmAlquileres();
                frm.FormClosed += frm_FormClosed;
            }
            return frm;
        }

        private static void frm_FormClosed(object sender, FormClosedEventArgs e)
        {
            frm = null;
        }

        private void tsbSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void tsbNuevo_Click(object sender, EventArgs e)
        {
            frmAlquileresAE frm = new frmAlquileresAE();
            frm.Text = "Nueva alquiler";
            DialogResult dr = frm.ShowDialog(this);
            if (dr == DialogResult.OK)
            {
                try
                {
                    bool crearReporte = frm.GetBoolReporte(); 
                    Alquiler alquiler = frm.GetObjeto();
                    alquiler = AgregarAlquilerPagoYReporte(frm, alquiler, crearReporte);
                    DataGridViewRow row = new DataGridViewRow();
                    row.CreateCells(dgvDatos);
                    SetearFila(row, alquiler);
                    AgregarFila(row);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            GrillaInicial();
        }

        private static Alquiler AgregarAlquilerPagoYReporte(frmAlquileresAE frm, Alquiler alquiler, bool crearReporte)
        {
            DetalleAlquiler dv = frm.GetDetalle();
            int alquilerId = AlquileresBd.Agregar(alquiler);
            DialogResult dr2 = MessageBox.Show($"¿Paga el total del alquiler?",
            "Confirmar pago", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (dr2 == DialogResult.Yes)
            {
                Pago pago = new Pago();
                pago.Cliente = alquiler.Cliente;
                pago.Descripcion = $"ALQUILER-{alquiler.AlquilerId}";
                pago.FechaPago = DateTime.Now;
                pago.Importe = alquiler.Importe;
                pago.ImporteOS = alquiler.ImporteOS;
                PagosBd.Agregar(pago, 3, pago.Descripcion);
            }
            if (crearReporte)
            {
                alquiler.AlquilerId = alquilerId;
                foreach (var item in alquiler.Detalle)
                {
                    rptContratoAlquiler rpt = Reportes.GetDatos(alquiler, item);
                    frmReportes1 frmRpt = new frmReportes1();
                    frmRpt.SetReporte(rpt);
                    frmRpt.Show();
                }
            }

            return alquiler;
        }

        private void AgregarFila(DataGridViewRow row)
        {
            dgvDatos.Rows.Add(row);
        }

        private void SetearFila(DataGridViewRow row, Alquiler item)
        {
            if (!item.EstaEnUso)
            {
                row.DefaultCellStyle.BackColor = Color.LightGray;
            }
            row.Cells[cmnNroAlquiler.Index].Value = item.AlquilerId;
            if (item.Cliente == null)
            {
                row.Cells[cmnCliente.Index].Value = "Cliente eliminado";
                row.Cells[cmnDni.Index].Value = "-";

            }
            else
            {
                row.Cells[cmnCliente.Index].Value = item.Cliente.ToString();
                row.Cells[cmnDni.Index].Value = item.Cliente.DNI.ToString();
            }
            row.Cells[cmnFechaDesde.Index].Value = item.FechaDesde.ToShortDateString();
            row.Cells[cmnFechaHasta.Index].Value = item.FechaHasta.ToShortDateString();
            row.Cells[cmnImporte.Index].Value = item.Importe.ToString("C");
            if (item.EstaEnUso == true)
            {

                if (item.FechaHasta.AddDays(5) < DateTime.Now)
                {
                    row.Cells[cmnFechaHasta.Index].Style.BackColor = Color.LightYellow;
                }
                else if (item.FechaHasta < DateTime.Now)
                {
                    row.Cells[cmnFechaHasta.Index].Style.BackColor = Color.Red;
                }
            }

            if (item.EstaEnUso)
            {
                row.Cells[cmnEnUso.Index].Value = "En uso";
                row.Cells[cmnEnUso.Index].Style.BackColor = Color.Green;
            }
            else
            {
                row.Cells[cmnEnUso.Index].Value = item.FechaDevolucion;
            }

            row.Tag = item;
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void dgvDatos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void frmAlquileres_Load(object sender, EventArgs e)
        {
            Dock = DockStyle.Fill;
            GrillaInicial();
        }

        List<Alquiler> lista = null;
        private void GrillaInicial()
        {
            try
            {
                lista = AlquileresBd.GetLista();
                MostrarDatosEnGrilla(lista);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }

        }

        private void MostrarDatosEnGrilla(List<Alquiler> lista)
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

        private void btnEntrega_Click(object sender, EventArgs e)
        {
            if (dgvDatos.SelectedRows.Count > 0)
            {
                int posicion = dgvDatos.SelectedRows[0].Index;
                DataGridViewRow row = dgvDatos.SelectedRows[0];
                Alquiler alq = (Alquiler)row.Tag;
                if (alq.EstaEnUso == true)
                {
                    frmAlquileresAE frm = new frmAlquileresAE();
                    frm.Text = "Realizar entrega de alquiler";
                    alq.Detalle = DetallesAlquileresBd.GetDetalles(alq);
                    if (EsProductoNull(alq))
                    {
                        MessageBox.Show("No se puede mostrar la información porque uno o más productos han sido eliminados. Elimine la transacción.");
                    }
                    else
                    {
                        frm.SetAlquiler(alq);
                        frm.SetEntrega(true);
                        frm.SetDevolucion(true);
                        if (alq.Cliente == null)
                        {
                            AlquileresBd.Devolucion(alq);
                            MessageBox.Show("No se puede mostrar la información porque el cliente ha sido eliminado. Entrega realizada.");
                        }
                        else
                        {
                            DialogResult dr = frm.ShowDialog(this);
                            if (dr == DialogResult.OK)
                            {
                                try
                                {
                                    alq = frm.GetAlquiler();
                                    AlquileresBd.Devolucion(alq);
                                    BajaClienteYProducto(alq, frm);
                                    if (frm.GetHuboEdicionEntrega())
                                    {
                                        Alquiler alquiler = frm.GetAlquilerPostEdicion();
                                        alquiler = AgregarAlquilerPagoYReporte(frm, alquiler, true);

                                    }
                                    SetearFila(row, alq);
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
                        }
                        GrillaInicial();
                    }
                }
                else
                {
                    MessageBox.Show("Este item ya ha sido entregado.", "Mensaje",
                           MessageBoxButtons.OK,
                           MessageBoxIcon.Warning);
                }
            }
        }

        private void BajaClienteYProducto(Alquiler alq, frmAlquileresAE frm)
        {
            if (BajaCliente(frm))
            {
                ClientesBd.Borrar(alq.Cliente);
            }
            if (BajaProducto(frm))
            {
                List<DetalleAlquiler> da = frm.GetDetalleBaja();

                foreach (var item in da)
                {
                    ProductoStocksBd.BajarStock(item.Stock.StockId, item.Cantidad, item.Producto.ProductoId);
                }
            }
        }

        private static bool EsProductoNull(Alquiler alq)
        {
            foreach (var item in alq.Detalle)
            {
                if (item.Producto == null)
                {
                    return true;
                }
            }
            return false;
        }

        private bool BajaCliente(frmAlquileresAE frm)
        {
            return frm.GetBajaCliente();
        }

        private bool BajaProducto(frmAlquileresAE frm)
        {
            return frm.GetBajaProducto();
        }
        private void tsbBorrar_Click(object sender, EventArgs e)
        {
            if (dgvDatos.SelectedRows.Count > 0)
            {
                DataGridViewRow r = dgvDatos.SelectedRows[0];
                Alquiler alq = (Alquiler)r.Tag;
                DialogResult dr = MessageBox.Show($"¿Desea borrar el Alquiler Nro. {alq.AlquilerId}?",
                    "Confirmar Borrado", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (dr == DialogResult.Yes)
                {
                    bool edicionCtaCte = false;
                    if (alq.EstaEnUso)
                    {
                        DialogResult dr2 = MessageBox.Show($"¿Desea actualizar cuenta corriente y stock?",
                     "Confirmar borrado", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                        if (dr2 == DialogResult.Yes)
                        {
                            edicionCtaCte = true;
                            alq.Detalle = DetallesAlquileresBd.GetDetalles(alq);
                        }
                    }
                    try
                    {
                        AlquileresBd.Borrar(alq, edicionCtaCte);
                        dgvDatos.Rows.Remove(r);
                        lista.Remove(alq);

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

        private void contratoDeAlquilerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvDatos.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dgvDatos.SelectedRows[0];
                Alquiler alq = (Alquiler)row.Tag;
                if (alq.Cliente == null)
                {
                    MessageBox.Show("No se puede mostrar la información porque el cliente ha sido eliminado.");
                }
                else
                {
                    alq.Detalle = DetallesAlquileresBd.GetDetalles(alq);
                    bool productoNull = false;
                    foreach (var item in alq.Detalle)
                    {
                        if (item.Producto == null)
                        {
                            productoNull = true;
                            break;
                        }
                        rptContratoAlquiler rpt = Reportes.GetDatos(alq, item);
                        frmReportes1 frm = new frmReportes1();
                        frm.SetReporte(rpt);
                        frm.Show();
                    }
                    if (productoNull)
                    {
                        MessageBox.Show("No se puede generar el reporte porque uno o más productos han sido eliminados.");
                    }
                }
            }
        }

        private void listaDeAlquileresToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void toolStripTextBox1_TextChanged(object sender, EventArgs e)
        {
            string valor = toolStripTextBox1.Text;
            List<Alquiler> listaProvisoria = null;
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

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (dgvDatos.SelectedRows.Count > 0)
            {
                DataGridViewRow r = dgvDatos.SelectedRows[0];
                Alquiler alquiler = (Alquiler)r.Tag;
                if (alquiler.Cliente == null)
                {
                    MessageBox.Show("No se puede mostrar la información porque el cliente ha sido eliminado.");
                }
                else
                {
                    alquiler.Detalle = DetallesAlquileresBd.GetDetalles(alquiler);
                    bool productoNull = false;
                    foreach (var item in alquiler.Detalle)
                    {
                        if (item.Producto == null)
                        {
                            productoNull = true;
                            break;
                        }
                    }
                    if (productoNull)
                    {
                        MessageBox.Show("No se puede mostrar la información porque uno o más productos han sido eliminados.");
                    }
                    else
                    {
                        frmAlquileresAE frm = new frmAlquileresAE();
                        frm.SetAlquiler(alquiler);
                        frm.SetEsDetalle(true);
                        DialogResult dr = frm.ShowDialog(this);

                    }
                }    
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            List<Alquiler> listaProvisoria = new List<Alquiler>();
            listaProvisoria = lista.Where(a => a.EstaEnUso == true).ToList();
            MostrarDatosEnGrilla(listaProvisoria);
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            if (dgvDatos.SelectedRows.Count > 0)
            {
                int posicion = dgvDatos.SelectedRows[0].Index;
                DataGridViewRow row = dgvDatos.SelectedRows[0];
                Alquiler alq = (Alquiler)row.Tag;
                if (alq.EstaEnUso == true)
                {
                    frmAlquileresAE frm = new frmAlquileresAE();
                    frm.Text = "Realizar renovación de alquiler";
                    alq.Detalle = DetallesAlquileresBd.GetDetalles(alq);
                    int alquilerId = alq.AlquilerId;
                    Cliente cli = alq.Cliente;
                    frm.SetAlquiler(alq);
                    frm.SetRenovación(true);
                    if (alq.Cliente == null)
                    {
                        MessageBox.Show("No se puede mostrar la información porque el cliente ha sido eliminado. Realice la devolución o elimine el alquiler.");
                    }
                    else
                    {
                        if (EsProductoNull(alq))
                        {
                            MessageBox.Show("No se puede mostrar la información porque uno o más productos han sido eliminado. Elimine el alquiler.");
                        }
                        else
                        {
                            DialogResult dr = frm.ShowDialog(this);
                            if (dr == DialogResult.OK)
                            {
                                try
                                {
                                    alq = frm.GetAlquiler();
                                    alq.AlquilerId = alquilerId;
                                    alq.Cliente = cli;
                                    decimal diferencia = AlquileresBd.Renovacion(alq);
                                    DialogResult dr2 = MessageBox.Show($"¿Paga el total de la renovación?",
                                    "Confirmar pago", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                                    if (dr2 == DialogResult.Yes)
                                    {
                                        Pago pago = new Pago();
                                        pago.Cliente = alq.Cliente;
                                        pago.Descripcion = $"RENOVACION-{alq.AlquilerId}";
                                        pago.FechaPago = DateTime.Now;
                                        pago.Importe = diferencia;
                                        PagosBd.Agregar(pago, 3, pago.Descripcion);
                                    }
                                    Alquiler alquilerParaReporte = alq;
                                    alquilerParaReporte.FechaDesde = DateTime.Now;
                                    alquilerParaReporte.Importe = diferencia;
                                    foreach (var item in alq.Detalle)
                                    {
                                        rptContratoAlquiler rpt = Reportes.GetDatos(alquilerParaReporte, item);
                                        frmReportes1 frmRpt = new frmReportes1();
                                        frmRpt.SetReporte(rpt);
                                        frmRpt.Show();
                                    }
                                    SetearFila(row, alq);
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
                        }
                  
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
}
