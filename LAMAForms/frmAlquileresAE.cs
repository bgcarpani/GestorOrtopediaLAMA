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
    public partial class frmAlquileresAE : Form
    {
        public frmAlquileresAE()
        {
            InitializeComponent();
        }

        private Alquiler alquiler;
        private List<DetalleAlquiler> detalles;
        private Cliente cliente;
        public ProductoStock ps;
        private bool devolucion = false;
        private bool esEntrega = false;

        public Alquiler GetObjeto()
        {
            return alquiler;
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            StocksBd.CargarComboVentaAlquileres(ref cboStocks);
            txtDireccion.Enabled = false;
            txtDNI.Enabled = false;
            txtLocalidad.Enabled = false;
            txtProvincia.Enabled = false;
            txtTelefono.Enabled = false;
            chkMes.Enabled = false;
            chkMes.Checked = true;
            chkQuincena.Enabled = false;
            txtObra.Enabled = false;
            button3.Enabled = false;
            cboProducto.Enabled = false;
            if (alquiler != null)
            {
               
                dtpDesde.Value = alquiler.FechaDesde;
                dtpHasta.Value = alquiler.FechaHasta;
                cboCliente.SelectedValue = alquiler.Cliente.ClienteId;
                txtDNI.Text = alquiler.Cliente.DNI;
                txtDNI.Enabled = false;
                txtDireccion.Text = alquiler.Cliente.Domicilio.Direccion;
                txtDireccion.Enabled = false;
                txtLocalidad.Text = alquiler.Cliente.Domicilio.Localidad.Descripcion;
                txtLocalidad.Enabled = false;
                txtProvincia.Text = alquiler.Cliente.Domicilio.Provincia.NombreProvincia;
                txtProvincia.Enabled = false;
                txtTelefono.Text = alquiler.Cliente.TelefonoMovil;
                txtTelefono.Enabled = false;
                if (esDetalle)
                {
                    chkCli.Enabled = false;
                    chkProd.Enabled = false;
                    txtObservacion.ReadOnly = true;
                    txtObservacion.Text = alquiler.Observacion;
                }
                ClientesBd.CargarCombo(ref cboCliente, alquiler.Cliente.ClienteId);
                detalles = new List<DetalleAlquiler>();
                foreach (var item in alquiler.Detalle)
                {
                    dv = new DetalleAlquiler
                    {
                        Producto = item.Producto,
                        Cantidad = item.Cantidad,
                    };
                    dv.Stock = item.Stock;
                    dv.Alquiler = new Alquiler(alquiler.AlquilerId);

                    if (!detalles.Contains(dv))
                    {
                        detalles.Add(dv);
                        row = new DataGridViewRow();
                        row.CreateCells(dgPedido);
                        SetearFila(row, dv);
                        AgregarFila(row);
                    }
                }

            }
            else
            {
                ClientesBd.CargarCombo(ref cboCliente);
                detalles = new List<DetalleAlquiler>();
                cboProducto.Enabled = false;
                cboProducto.Enabled = false;
                chkDesde.Checked = true;
            }
            if (!esEntrega)
            {
                labelEntrega.Visible = false;
                labelEntrega2.Visible = false;
                dgPedido.Enabled = false;
            }
            else
            {
                labelEntrega.Visible = true;
                labelEntrega2.Visible = true;
                dgPedido.Enabled = true;
            }
            if (devolucion == true || esDetalle == true || renovacion == true)
            {
                cboCliente.Enabled = false;
                cboProducto.Enabled = false;
                cboStocks.Enabled = false;
                nudCantidad.Enabled = false;
                chkDesde.Enabled = false;
                dtpDesde.Enabled = false;
                btnAceptarProducto.Enabled = false;
                btnCancelarProducto.Enabled = false;
                chkMes.Enabled = true;
                chkQuincena.Enabled = true;
                if (!renovacion)
                {
                    dtpHasta.Enabled = false;
                    gbObservacion.Visible = true;
                }
            }
            else
            {
                chkDesde.Enabled = true;
                cboCliente.Enabled = true;
                cboProducto.Enabled = true;
                cboStocks.Enabled = true;
                nudCantidad.Enabled = true;
                dtpHasta.Enabled = true;
                btnAceptarProducto.Enabled = true;
                btnCancelarProducto.Enabled = true;
                dgPedido.Enabled = true;
                gbObservacion.Visible = false;
            }
        }

        internal bool GetBoolReporte()
        {
            return generoReporte;
        }

        internal DetalleAlquiler GetDetalle()
        {
            return dv;
        }

        private bool generoReporte = false;
        private bool bajaCliente = false;
        private bool bajaProducto = false;
        private Alquiler nuevoAlquiler = null;
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (Validar())
            {
                if (detalles.Count > 0)
                {
                    if (alquiler == null || renovacion)
                    {
                        alquiler = new Alquiler()
                        {
                            Cliente = cliente,
                            FechaHasta = dtpHasta.Value,
                            Detalle = detalles,
                            Observacion = txtObservacion.Text,
                            EstaEnUso = true,
                        };
                        if (chkDesde.Checked == true)
                        {
                            alquiler.FechaDesde = DateTime.Now;
                        }
                        else
                        {
                            alquiler.FechaDesde = dtpDesde.Value;
                        }
                        if (chkMes.Checked)
                        {
                            alquiler.Importe = CalcularPrecioAlquiler(alquiler.Detalle);
                        }
                        else
                        {
                            alquiler.Importe = CalcularPrecioAlquiler(alquiler.Detalle);
                        }
                        if (checkBox1.Checked)
                        {
                            alquiler.ImporteOS = decimal.Parse(txtObra.Text);
                        }
                        else
                        {
                            alquiler.ImporteOS = 0;
                        }
                      
                        if (!renovacion)
                        {
                            DialogResult dr = MessageBox.Show("¿Desea generar el contrato de alquiler ahora?",
                       "Confirmar Alquiler", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                            if (dr == DialogResult.Yes)
                            {
                                generoReporte = true;
                            }
                        }
                        DialogResult = DialogResult.OK;
                    }
                    else
                    {
                        alquiler.Observacion = txtObservacion.Text;
                        if (chkProd.Checked)
                        {
                            bajaProducto = true;
                        }
                        if (chkCli.Checked)
                        {
                            bajaCliente = true;
                        }
                        if (huboEdicionEntrega)
                        {
                            frmDateTimePicker frm = new frmDateTimePicker();
                            DialogResult dr = frm.ShowDialog(this);
                            if (dr == DialogResult.OK)
                            {
                                nuevoAlquiler = alquiler;
                                nuevoAlquiler.FechaDesde = frm.GetFechaDesde();
                                nuevoAlquiler.FechaHasta = frm.GetFechaHasta();
                                List<DetalleAlquiler> detalleOriginal = alquiler.Detalle;
                                alquiler.Detalle = detallesProvisorios;

                                for (int i = 0; i < detalleOriginal.Count; i++)
                                {
                                    nuevoAlquiler.Detalle[i].Cantidad = detalleOriginal[i].Cantidad - alquiler.Detalle[i].Cantidad;
                                    if (nuevoAlquiler.Detalle[i].Cantidad == 0)
                                    {
                                        nuevoAlquiler.Detalle.Remove(nuevoAlquiler.Detalle[i]);
                                    }
                                }
                                decimal sumatoria = CalcularPrecioAlquiler(nuevoAlquiler.Detalle);
                                nuevoAlquiler.Importe = decimal.Parse(sumatoria.ToString("0.00"));


                                DialogResult = DialogResult.OK;
                            }
                            else
                            {
                                MessageBox.Show("No se puede mostrar la información porque uno o más productos han sido eliminados. Elimine la transacción.");

                            }
                        }
                        else
                        {
                            DialogResult = DialogResult.OK;
                        }                     

                    }
                }
                else
                {
                    errorProvider1.Clear();
                    errorProvider1.SetError(btnAceptarProducto, "Debe ingresar al menos un producto a la grilla");
                }
            }
        }

        internal Alquiler GetAlquilerPostEdicion()
        {
            return nuevoAlquiler;
        }

        private decimal CalcularPrecioAlquiler(List<DetalleAlquiler> detalle)
        {
            decimal sumatoria = 0;
            decimal precioProducto = 0;
    
            foreach (var item in detalle)
            {
                if (chkMes.Checked)
                {
                    precioProducto = item.Producto.PrecioAlquiler;
                }
                else
                {
                    precioProducto = item.Producto.PrecioAlquilerQuincena;
                }
                decimal precio = precioProducto * item.Cantidad;
                sumatoria = sumatoria + precio;
            }

            return sumatoria;
        }

        internal bool GetHuboEdicionEntrega()
        {
            return huboEdicionEntrega;
        }

        internal void SetEntrega(bool v)
        {
            this.esEntrega = v;
        }

        internal List<DetalleAlquiler> GetDetalleBaja()
        {
            return detalleDeBaja;
        }

        internal bool GetBajaCliente()
        {
            return bajaCliente;
        }

        internal bool GetBajaProducto()
        {
            return bajaProducto;
        }

        private bool Validar()
        {
            if (devolucion == false && renovacion == false)
            {
                errorProvider1.Clear();
                if (cboCliente.SelectedIndex == 0)
                {
                    errorProvider1.SetError(cboCliente, "Seleccione un cliente");
                    return false;
                }
                 if (cboCliente.SelectedIndex == 0)
                {
                    errorProvider1.SetError(cboCliente, "Seleccione un cliente");
                    return false;
                }
                if (cboProducto.SelectedIndex == 0)
                {
                    errorProvider1.SetError(cboProducto, "Seleccione un producto");
                    return false;
                }
                if (chkMes.Checked == false && chkQuincena.Checked == false)
                {
                    errorProvider1.SetError(chkMes, "Debes seleccionar una opción.");
                    return false;
                }
                if (chkDesde.Enabled == false)
                {
                    if (dtpHasta.Value.ToShortDateString() == dtpDesde.Value.ToShortDateString())
                    {
                        errorProvider1.SetError(dtpHasta, "No puedes realizar un alquiler por menos de un día");
                        return false;
                    }
                }
                else
                {
                    if (dtpHasta.Value.ToShortDateString() == DateTime.Now.ToShortDateString())
                    {
                        errorProvider1.SetError(dtpHasta, "No puedes realizar un alquiler por menos de un día");
                        return false;
                    }
                }
                if (dtpHasta.Value < dtpDesde.Value)
                {
                    errorProvider1.SetError(dtpHasta, "'Desde' no puede ser mayor que 'Hasta'");
                    return false;
                }
                return true;
            }
            return true;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void frmAlquileresAE_Load(object sender, EventArgs e)
        {

        }

        private void btnCancelarProducto_Click(object sender, EventArgs e)
        {
            InicializarControles();

        }

        private DetalleAlquiler dv;
        private bool editarDetalle = false;
        private DataGridViewRow row;
        int i = -1;
        private decimal totalActual;
        private void btnAceptarProducto_Click(object sender, EventArgs e)
        {
            if (ValidarDatos())
            {
                if (!editarDetalle)
                {
                    if (ps.StockDisponible > 0)
                    {
                        dv = new DetalleAlquiler
                        {
                            Producto = ps.Producto,
                            Cantidad = (int)nudCantidad.Value,
                            Stock = (Stock)cboStocks.SelectedItem                         
                        };

                        if (!detalles.Contains(dv))
                        {
                            detalles.Add(dv);
                            row = new DataGridViewRow();
                            row.CreateCells(dgPedido);
                            SetearFila(row, dv);
                            AgregarFila(row);
                        }
                        else
                        {
                            DialogResult dr =
                                MessageBox.Show("Producto repetido \nDesea sumar el precio del producto al ingresado?",
                                    "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                            if (dr == DialogResult.Yes)
                            {
                                foreach (var item in detalles)
                                {
                                    i++;
                                    if (item.Producto.ProductoId == dv.Producto.ProductoId)
                                    {
                                        row = dgPedido.Rows[i];
                                        dv = (DetalleAlquiler)row.Tag;
                                        dv.Cantidad = (int)nudCantidad.Value + item.Cantidad;
                                        SetearFila(row, item);

                                    }

                                }
                            }

                            else
                            {
                                InicializarControles();
                            }
                            i = -1;
                        }
                        totalActual = CalcularPrecioAlquiler(detalles);
                        txtTotal.Text = totalActual.ToString("C");
                        ;
                    }
                    else
                    {
                        MessageBox.Show("No hay stock disponible");
                    }
                }
                else
                {
                    dv.Cantidad = (int)nudCantidad.Value;
                    SetearFila(row, dv);
                    InicializarControles();
                    editarDetalle = false;

                }
            }
        }
        private bool esDetalle = false;
        internal void SetEsDetalle(bool v)
        {
            esDetalle = v;
        }

        internal void SetDevolucion(bool v)
        {
            devolucion = v;
        }

        internal Alquiler GetAlquiler()
        {
            return alquiler;
        }

        public void SetAlquiler(Alquiler alq)
        {
            this.alquiler = alq;
        }


        private void InicializarControles()
        {
            cboProducto.SelectedValue = 0;
            txtStock.Clear();
            nudCantidad.Value = 1;
            cboProducto.Enabled = true;
            txtPrecio.Clear();
            totalActual = CalcularPrecioAlquiler(detalles);
            txtTotal.Text = totalActual.ToString("C");
        }

        private bool ValidarDatos()
        {
            if (cboProducto.SelectedIndex <= 0)
            {
                errorProvider1.SetError(cboProducto, "Debe seleccionar un producto");
                return false;
            }
            if (ps.StockDisponible < (int)nudCantidad.Value)
            {
                errorProvider1.SetError(nudCantidad, "No hay suficiente stock");
                return false;
            }
            errorProvider1.Clear();

            return true;
        }


        private void AgregarFila(DataGridViewRow row)
        {
            dgPedido.Rows.Add(row);
        }

        private void SetearFila(DataGridViewRow row, DetalleAlquiler dv)
        {
            if (dv.Producto == null)
            {
                row.Cells[cmnProducto.Index].Value = "Producto eliminado";
                row.Cells[cmnPrecioAlq.Index].Value = "-";
                

            }
            else
            {
                row.Cells[cmnProducto.Index].Value = dv.ToString();
                row.Cells[cmnPrecioAlq.Index].Value = dv.Producto.PrecioAlquiler;

            }
            row.Cells[cmnCantidad.Index].Value = dv.Cantidad;

            row.Tag = dv;
        }

        private void cboCliente_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboCliente.SelectedIndex > 0)
            {
                cliente = (Cliente)cboCliente.SelectedItem;
                txtDireccion.Text = cliente.Domicilio.Direccion;
                txtLocalidad.Text = cliente.Domicilio.Localidad.Descripcion;
                txtProvincia.Text = cliente.Domicilio.Provincia.NombreProvincia;
                txtDNI.Text = cliente.DNI;
            }
            else
            {
                cliente = null;
                txtDireccion.Clear();
                txtLocalidad.Clear();
                txtProvincia.Clear();
                txtDNI.Clear();
                //txtCondIva.Clear();
            }
        }

        private bool renovacion = false;
        internal void SetRenovación(bool v)
        {
            this.renovacion = v;
        }

        private void cboProducto_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboProducto.SelectedIndex > 0)
            {
                ps = (ProductoStock)cboProducto.SelectedItem;
                txtStock.Text = ps.StockDisponible.ToString();
                chkMes.Enabled = true;
                chkQuincena.Enabled = true;
                if (chkMes.Checked)
                {
                    txtPrecio.Text = ps.Producto.PrecioAlquiler.ToString("C");
                }
                else if (chkQuincena.Checked)
                {
                    txtPrecio.Text = ps.Producto.PrecioAlquilerQuincena.ToString("C");
                }
            }
            else
            {
                ps = null;
                txtStock.Clear();
            }
        }

        private void chkDesde_CheckedChanged(object sender, EventArgs e)
        {
            if (chkDesde.Checked == true)
            {
                dtpDesde.Enabled = false;
            }
            else
            {
                dtpDesde.Enabled = true;
            }
        }

        private void cboStocks_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboStocks.SelectedIndex > 0)
            {
                cboProducto.Enabled = true;
                button3.Enabled = true;
                ProductoStocksBd.CargarCombo(ref cboProducto, cboStocks.SelectedIndex);
            }
            else
            {
                cboProducto.Enabled = false;
                button3.Enabled = false;
            }

        }

        private void dgPedido_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 3)
            {
                row = dgPedido.SelectedRows[0];
                DetalleAlquiler detalleBorrar = (DetalleAlquiler)row.Tag;
                dgPedido.Rows.Remove(row);
                detalles.Remove(detalleBorrar);
                InicializarControles();
            }
            if (e.ColumnIndex == 4)
            {
                row = dgPedido.SelectedRows[0];
                dv = (DetalleAlquiler)row.Tag;
                cboProducto.SelectedValue = dv.Producto.ProductoId;
                cboProducto.Enabled = false;
                editarDetalle = true;
            }
        }

        private List<DetalleAlquiler> detalleDeBaja;
        private void chkProd_CheckedChanged(object sender, EventArgs e)
        {
            if (alquiler.Detalle.Count > 1)
            {
                frmBajaAlquiler frmBaja = new frmBajaAlquiler();
                frmBaja.SetAlquiler(alquiler);
                DialogResult dr = frmBaja.ShowDialog(this);
                if (dr == DialogResult.OK)
                {
                    detalleDeBaja = frmBaja.GetDetalleDeBaja();
                }
            }
            else
            {
                detalleDeBaja = new List<DetalleAlquiler>();
                foreach (var item in alquiler.Detalle)
                {
                    detalleDeBaja.Add(item);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
        private List<DetalleAlquiler> detallesProvisorios = null;
        public bool huboEdicionEntrega { get; set; }
        private void dgPedido_KeyDown(object sender, KeyEventArgs e)
        {
            DataGridViewCell cell = null;
            if (dgPedido.EditMode == DataGridViewEditMode.EditOnKeystrokeOrF2)
            {
                int posicion = dgPedido.SelectedRows[0].Index;
                detallesProvisorios = detalles;
                cell = dgPedido.Rows[posicion].Cells[1];
                bool nonNumberEntered = false;
                if (e.KeyCode < Keys.D0 || e.KeyCode > Keys.D9)
                {
                    if (e.KeyCode < Keys.NumPad0 || e.KeyCode > Keys.NumPad9)
                    {
                        if (e.KeyCode != Keys.Back)
                        {
                            nonNumberEntered = true;
                        }
                    }
                }
                if (Control.ModifierKeys == Keys.Shift)
                {
                    nonNumberEntered = true;
                }
                if (!nonNumberEntered)
                {
                    int value = int.Parse(new string(e.KeyData.ToString().Where(c => char.IsDigit(c)).ToArray()));
                    if (value < detallesProvisorios[posicion].Cantidad && value >= 0)
                    {
                        detallesProvisorios[posicion].Cantidad = value;
                        cell.Value = value;
                        huboEdicionEntrega = true;
                    }
                }
                else
                {
                    e.Handled = true;
                }
                //dgPedido.CommitEdit(DataGridViewDataErrorContexts.Commit);
                //dgPedido.EndEdit();
            }
        }

        private void chkMes_CheckedChanged(object sender, EventArgs e)
        {
            if (chkMes.Checked)
            {
                chkQuincena.Checked = false;
                if (ps != null)
                {
                    txtPrecio.Text = (ps.Producto.PrecioAlquiler).ToString();
                }
                if (renovacion)
                {
                    txtTotal.Text = CalcularPrecioAlquiler(alquiler.Detalle).ToString();
                }
            }
        }

        private void chkQuincena_CheckedChanged(object sender, EventArgs e)
        {
            if (chkQuincena.Checked)
            {
                chkMes.Checked = false;
                if (ps != null)
                {
                    txtPrecio.Text = (ps.Producto.PrecioAlquilerQuincena).ToString();
                }
                if (renovacion)
                {
                    txtTotal.Text = CalcularPrecioAlquiler(alquiler.Detalle).ToString();
                }
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            txtObra.Enabled = !txtObra.Enabled;
            txtObra.ReadOnly = false;
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            frmBuscarClientes frm = new frmBuscarClientes();
            DialogResult dr = frm.ShowDialog(this);
            if (dr == DialogResult.OK)
            {
                Cliente cliente = frm.GetCliente();
                cboCliente.SelectedValue = cliente.ClienteId;

            }
        }

        private void nudCantidad_ValueChanged(object sender, EventArgs e)
        {
            if (ps != null)
            {
                if (chkMes.Checked)
                {
                    txtPrecio.Text = (ps.Producto.PrecioAlquiler * nudCantidad.Value).ToString("C");
                }
                else if (chkQuincena.Checked)
                {
                    txtPrecio.Text = (ps.Producto.PrecioAlquilerQuincena * nudCantidad.Value).ToString("C");
                }

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            frmBuscarProductos frm = new frmBuscarProductos();
            frm.SetProductos(ProductoStocksBd.GetLista(cboStocks.SelectedIndex), cboStocks.SelectedIndex);
            DialogResult dr = frm.ShowDialog(this);
            if (dr == DialogResult.OK)
            {
                ProductoStock ps = frm.GetProductoStock();
                cboProducto.SelectedValue = ps.ProductoStockId;
            }
        }
    }
}
