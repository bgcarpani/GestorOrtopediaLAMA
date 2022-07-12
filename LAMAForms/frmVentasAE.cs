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
    public partial class frmVentasAE : Form
    {
        public frmVentasAE()
        {
            InitializeComponent();
        }

        private Venta venta;
        private List<DetalleVenta> detalles;
        private Cliente cliente;
        private Producto pd;
        public ProductoStock ps;
        public Venta GetObjeto()
        {
            return venta;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!HayConsumidorFinal())
            {
                ClientesBd.CargarCombo(ref cboCliente);
            }
            ProductoStocksBd.CargarCombo(ref cboProducto, 1);
            detalles = new List<DetalleVenta>();
            cboProducto.Enabled = true;
            txtObra.Enabled = false;
            txtObra.ReadOnly = false;
            txtObra.Text = "0";
            checkBox1.Checked = false;

            if (venta != null)
            {
                button2.Enabled = false;
                detalles = venta.Detalle;
                if (venta.EsConsumidorFinal)
                {
                    cboCliente.Enabled = false;
                    txtDireccion.Clear();
                    txtDNI.Clear();
                    txtProvincia.Clear();
                    txtLocalidad.Clear();
                    txtTelefono.Clear();
                    checkBox1.Enabled = false;
                    checkBox1.Checked = true;
                }
                else
                {
                    cboCliente.SelectedValue = venta.Cliente.ClienteId;
                    txtDireccion.Text = venta.Cliente.Domicilio.Direccion;
                    txtDNI.Text = venta.Cliente.DNI;
                    txtProvincia.Text = venta.Cliente.Domicilio.Provincia.NombreProvincia;
                    txtLocalidad.Text = venta.Cliente.Domicilio.Localidad.Descripcion;
                    txtTelefono.Text = venta.Cliente.TelefonoMovil;
                }
                foreach (var item in venta.Detalle)
                {
                    row = new DataGridViewRow();
                    row.CreateCells(dgPedido);
                    SetearFila(row, item);
                    AgregarFila(row);
                    ActualizarVentaTotal();
                }
            }

        }

        private bool HayConsumidorFinal()
        {
            return (venta != null && venta.EsConsumidorFinal);
        }

        private decimal neto;

        public void SetCliente(Venta venta)
        {
            this.venta = venta;
        }
        private bool ValidarDatosProductos()
        {
            bool valido = true;
            errorProvider1.Clear();
            if (detalles.Count == 0 && cboProducto.SelectedIndex == 0)
            {
                errorProvider1.SetError(cboProducto, "Seleccione un producto");
                valido = false;
            }

            if (ps != null)
            {
                if (nudCantidad.Value > ps.StockDisponible)
                {
                    valido = false;
                    btnAceptarProducto.Enabled = false;
                    errorProvider1.SetError(nudCantidad, "La cantidad debe ser menor o igual al stock disponible");
                }
                else
                {
                    btnAceptarProducto.Enabled = true;
                }
            }
            else
            {
                errorProvider1.SetError(cboProducto, "Seleccione un producto");
                valido = false;
            }
            return valido;
        }

        private void btnCancelarProducto_Click(object sender, EventArgs e)
        {
            InicializarControles();
        }
        private void InicializarControles()
        {

            cboProducto.SelectedValue = 0;
            txtStock.Clear();
            txtPrecioUnit.Clear();
            nudCantidad.Value = 1;
            txtPrecioTotal.Clear();
            cboProducto.Enabled = true;
        }

        private DetalleVenta dv;
        private bool editarDetalle = false;
        private bool esConsumidorFinal = false;
        internal bool GetConsumidorFinal()
        {
            return esConsumidorFinal;
        }

        private DataGridViewRow row;
        int i = -1;
        private void btnAceptarProducto_Click_1(object sender, EventArgs e)
        {
            if (ValidarDatos())
            {
                if (!editarDetalle)
                {
                    if (ps.StockDisponible > 0)
                    {
                        dv = new DetalleVenta
                        {
                            Producto = ps.Producto,
                            Cantidad = (int)nudCantidad.Value,
                            PrecioUnidad = decimal.Parse(txtPrecioUnit.Text),
                            Total = decimal.Parse(txtPrecioTotal.Text),
                            Devuelto = false
                        };

                        if (!detalles.Contains(dv))
                        {
                            detalles.Add(dv);
                            row = new DataGridViewRow();
                            row.CreateCells(dgPedido);
                            SetearFila(row, dv);
                            AgregarFila(row);
                            ActualizarVentaTotal();
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
                                        dv = (DetalleVenta)row.Tag;
                                        dv.Cantidad = (int)nudCantidad.Value + item.Cantidad;
                                        dv.Total = decimal.Parse(txtPrecioUnit.Text) + decimal.Parse(txtPrecioUnit.Text);
                                        SetearFila(row, item);

                                    }

                                }
                                ActualizarVentaTotal();
                            }

                            else
                            {
                                ActualizarVentaTotal();
                                InicializarControles();
                            }
                            i = -1;
                        }
                    }
                    else
                    {
                        MessageBox.Show("No hay stock disponible");
                    }
                }
                else
                {
                    dv.Cantidad = (int)nudCantidad.Value;
                    dv.PrecioUnidad = decimal.Parse(txtPrecioUnit.Text);
                    dv.Total = (int)nudCantidad.Value * decimal.Parse(txtPrecioUnit.Text);

                    SetearFila(row, dv);
                    ActualizarVentaTotal();
                    InicializarControles();
                    editarDetalle = false;

                }
            }
        }

        public Venta GetVenta()
        {
            return venta;
        }

        bool editar = false;
        internal void SetEditar(bool p)
        {
            editar = p;
        }


        private bool ValidarDatos()
        {
            bool valido = true;
            errorProvider1.Clear();
            return valido;
        }
        private void ActualizarVentaTotal()
        {
            txtTotalPedido.Text = detalles.Sum(d => d.Total).ToString("C");
        }

        private void AgregarFila(DataGridViewRow row)
        {
            dgPedido.Rows.Add(row);
        }

        private void SetearFila(DataGridViewRow row, DetalleVenta dv)
        {
            row.Cells[cmnProducto.Index].Value = dv.ToString();
            row.Cells[cmnPrecioUnitario.Index].Value = Convert.ToString($"$ {dv.PrecioUnidad}");
            row.Cells[cmnCantidad.Index].Value = dv.Cantidad;
            row.Cells[cmnTotal.Index].Value = Convert.ToString($"$ {dv.Total}");

            row.Tag = dv;
        }

        private bool ValidarVenta()
        {
            errorProvider1.Clear();
            if (checkBox1.Checked == false)
            {
                if (cboCliente.SelectedIndex == 0)
                {
                    errorProvider1.SetError(cboCliente, "Seleccione un cliente");
                    return false;
                }
            }
            if (detalles.Count == 0 && cboProducto.SelectedIndex == 0)
            {
                errorProvider1.SetError(cboProducto, "Seleccione un producto");
                return false;
            }
          
            return true;
        }

        private void frmVentasAE_Load(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void nudCantidad_ValueChanged(object sender, EventArgs e)
        {
            if (ValidarDatosProductos())
            {
                neto = nudCantidad.Value * decimal.Parse(txtPrecioUnit.Text);

                txtPrecioTotal.Text = Convert.ToString(neto);
            }
        }

        private void cboCliente_SelectedIndexChanged_1(object sender, EventArgs e)
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
            }
        }

        private void cboProducto_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (cboProducto.SelectedIndex > 0)
            {                
                ps = (ProductoStock)cboProducto.SelectedItem;
                txtStock.Text = ps.StockDisponible.ToString();
                txtPrecioUnit.Text = ps.Producto.Precio.ToString();
                txtPrecioTotal.Text = ps.Producto.Precio.ToString();

            }
            else
            {
                ps = null;
                txtStock.Clear();
            }
        }

        private void btnOK_Click_1(object sender, EventArgs e)
        {

            if (ValidarVenta())
            {
                if (detalles.Count > 0)
                {
                    if (venta == null)
                    {
                        venta = new Venta();
                    }
                    venta.FechaVenta = dtpFechaVenta.Value;
                    venta.Total = detalles.Sum(p => p.Total);
                    venta.Detalle = detalles;
                    if (chkObra.Checked)
                    {
                        venta.ImporteOS = decimal.Parse(txtObra.Text);
                    }
                    else
                    {
                        venta.ImporteOS = 0;
                    }

                    if (checkBox1.Checked == true)
                    {
                        esConsumidorFinal = true;
                        venta.EsConsumidorFinal = true;
                    }
                    else
                    {
                        venta.Cliente = (Cliente)cboCliente.SelectedItem;
                        venta.EsConsumidorFinal = false;
                    }
                   
                    DialogResult = DialogResult.OK;
                }
                else
                {
                    errorProvider1.Clear();
                    errorProvider1.SetError(btnAceptarProducto, "Debe ingresar al menos un producto a la grilla");
                }
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

            if (checkBox1.Checked == true)
            {
                cboCliente.Visible = true;
                cboCliente.Enabled = false;
                txtDireccion.Enabled = false;
                txtLocalidad.Enabled = false;
                txtProvincia.Enabled = false;
                txtDNI.Enabled = false;
                txtTelefono.Enabled = false;
                txtDireccion.Clear();
                txtLocalidad.Clear();
                txtProvincia.Clear();
                txtDNI.Clear();
                txtTelefono.Clear();
                txtDireccion.ReadOnly = false;
                txtLocalidad.ReadOnly = false;
                txtProvincia.ReadOnly = false;
                txtDNI.ReadOnly = false;
                txtTelefono.ReadOnly = false;
            }
            else
            {
                cboCliente.Visible = true;
                cboCliente.Enabled = true;
                txtDireccion.Enabled = false;
                txtLocalidad.Enabled = false;
                txtProvincia.Enabled = false;
                txtDNI.Enabled = false;
                txtTelefono.Enabled = false;
                txtDireccion.Clear();
                txtLocalidad.Clear();
                txtProvincia.Clear();
                txtDNI.Clear();
                txtTelefono.Clear();
                txtDireccion.ReadOnly = true;
                txtLocalidad.ReadOnly = true;
                txtProvincia.ReadOnly = true;
                txtDNI.ReadOnly = true;
                txtTelefono.ReadOnly = true;
            }
        }

        private void dgPedido_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 4)
            {
                row = dgPedido.SelectedRows[0];
                DetalleVenta detalleBorrar = (DetalleVenta)row.Tag;
                dgPedido.Rows.Remove(row);
                detalles.Remove(detalleBorrar);
                ActualizarVentaTotal();
                InicializarControles();
            }
            if (e.ColumnIndex == 5)
            {
                row = dgPedido.SelectedRows[0];
                dv = (DetalleVenta)row.Tag;
                cboProducto.SelectedValue = dv.Producto.ProductoId;
                txtPrecioUnit.Text = dv.PrecioUnidad.ToString();
                txtPrecioTotal.Text = dv.Total.ToString("C");
                cboProducto.Enabled = false;
                editarDetalle = true;
            }
        }

        private void btnCancelar_Click_1(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            txtObra.Enabled = !txtObra.Enabled;
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

        private void button1_Click(object sender, EventArgs e)
        {
            frmBuscarProductos frm = new frmBuscarProductos();
            frm.SetProductos(ProductoStocksBd.GetLista(1), 1);
            DialogResult dr = frm.ShowDialog(this);
            if (dr == DialogResult.OK)
            {
                ProductoStock ps = frm.GetProductoStock();
                cboProducto.SelectedValue = ps.ProductoStockId;

            }

        }
    }
}
