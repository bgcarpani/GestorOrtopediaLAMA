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
    public partial class frmComprasAE : Form
    {
        private Compra compra;
        public frmComprasAE()
        {
            InitializeComponent();
        }

        private DataGridViewRow row;
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            ProveedoresBd.CargarCombo(ref cboProveedores);
            ProductosBd.CargarCombo(ref cboProducto);
            detalle = new List<DetalleCompra>();
            if (compra != null)
            {
                detalle = compra.DetallesCompras;
                cboProveedores.SelectedValue = compra.Proveedor.ProveedorId;
                txtDireccion.Text = compra.Proveedor.Direccion;
                txtProvincia.Text = compra.Proveedor.Localidad.Provincia.NombreProvincia;
                txtLocalidad.Text = compra.Proveedor.Localidad.Descripcion;
                txtTelefono.Text = compra.Proveedor.Telefono;
                txtEmail.Text = compra.Proveedor.Email;
                txtSitio.Text = compra.Proveedor.Web;
                
                foreach (var item in compra.DetallesCompras)
                {
                    row = new DataGridViewRow();
                    row.CreateCells(dgPedido);
                    SetearFila(row, item);
                    AgregarFila(row);
                    ActualizarTotalCompra();
                 
                }
            }

        }
        private Proveedor proveedor;
        private Producto pd;
        private ProductoStock ps;



        private decimal neto;

        private void InicializarControles()
        {
            cboProducto.SelectedIndex = 0;
            cboProducto.Enabled = true;
            errorProvider1.Clear();
            txtPrecioUnit.Clear();
            txtStock.Clear();
            nudCantidad.Value = 1;
            txtPrecioTotal.Clear();
            cboProducto.Focus();
        }

        private List<DetalleCompra> detalle;

        private void ActualizarTotalCompra()
        {
            txtTotalPedido.Text = detalle.Sum(dt => dt.Total).ToString("C");
        }

        private void SetearFila(DataGridViewRow r, DetalleCompra detalleCompra)
        {
            r.Cells[cmnProducto.Index].Value = detalleCompra.ToString();
            r.Cells[cmnCantidad.Index].Value = detalleCompra.Cantidad;
            r.Cells[cmnTotal.Index].Value = detalleCompra.Total.ToString("C");
            //r.Cells[cmnTotal.Index].Value = Convert.ToString($"$ {detalleCompra.Total}");
            r.Tag = detalleCompra;
        }

        private void AgregarFila(DataGridViewRow r)
        {
            dgPedido.Rows.Add(r);
        }

        private bool ValidarDatos()
        {
            errorProvider1.Clear();
            if (cboProducto.SelectedIndex == 0)
            {
                errorProvider1.SetError(cboProducto, "Debe seleccionar un producto.");
                return false;
            }
            if (cboProveedores.SelectedIndex == 0)
            {
                errorProvider1.SetError(cboProveedores, "Debe seleccionar un proveedor.");
                return false;
            }
            if ((int)nudCantidad.Value <= 0)
            {
                errorProvider1.SetError(nudCantidad, "El valor ingresado debe ser mayor a 0.");
                return false;
            }
            return true;
        }
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (ValidarCompra())
            {
                if (compra == null)
                {
                    compra = new Compra();
                }
                compra.FechaCompra = dtpFechaPedido.Value;
                compra.Proveedor = proveedor;
                compra.Total = detalle.Sum(dt => dt.Total);
                compra.DetallesCompras = detalle;
                this.DialogResult = DialogResult.OK;
            }
        }

        private bool ValidarCompra()
        {
            errorProvider1.Clear();
            if (cboProveedores.SelectedIndex == 0)
            {
                errorProvider1.SetError(cboProveedores, "Seleccione un proveedor");
                return false;
            }
            
            if (detalle.Count == 0 && cboProducto.SelectedIndex == 0)
            {
                errorProvider1.SetError(cboProducto, "Seleccione un producto");
                return false;
            }
            return true;
        }

        public Compra GetObjeto()
        {
            return compra;
        }

        private bool editarDetalle = false;
        private DataGridViewRow r;
        private DetalleCompra detalleCompra;

        private void nudCantidad_ValueChanged(object sender, EventArgs e)
        {

            neto = nudCantidad.Value * decimal.Parse(txtPrecioUnit.Text);
            txtPrecioTotal.Text = Convert.ToString(neto);
            
        }

        private void btnAceptarProducto_Click_1(object sender, EventArgs e)
        {
            if (ValidarDatos())
            {
                if (!editarDetalle)
                {
                    //Crear el item a la compra
                    detalleCompra = new DetalleCompra
                    {
                        Producto = ps.Producto,
                        Cantidad = (int)nudCantidad.Value,
                        PrecioUnidad = decimal.Parse(txtPrecioUnit.Text),
                        Total = decimal.Parse(txtPrecioTotal.Text)
                    };
                    if (!detalle.Contains(detalleCompra))
                    {
                        //Agregarlo a la lista de detalles
                        detalle.Add(detalleCompra);
                        //Mostrarlo en la grilla
                        DataGridViewRow r = new DataGridViewRow();
                        r.CreateCells(dgPedido);
                        SetearFila(r, detalleCompra);
                        AgregarFila(r);
                        ActualizarTotalCompra();
                    }
                    else
                    {
                        errorProvider1.SetError(cboProducto, "Producto ya ingresado");
                    }
                }
                else
                {
                    detalleCompra.Cantidad = (int)nudCantidad.Value;
                    detalleCompra.PrecioUnidad = decimal.Parse(txtPrecioUnit.Text);
                    detalleCompra.Total = (int)nudCantidad.Value * decimal.Parse(txtPrecioUnit.Text);

                    SetearFila(r, detalleCompra);
                    ActualizarTotalCompra();
                    editarDetalle = false;
                }
            }
        }

        internal Compra GetCompra()
        {
            return compra;
        }

        bool editar = false;
        internal void SetEditar(bool v)
        {
            editar = v;
        }

        internal void SetCliente(Compra compra)
        {
            this.compra = compra;
        }

        private void dgPedido_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 3)
            {
                r = dgPedido.SelectedRows[0];
                DetalleCompra detalleBorrar = (DetalleCompra)r.Tag;
                dgPedido.Rows.Remove(r);
                detalle.Remove(detalleBorrar);
                ActualizarTotalCompra();
            }
            if (e.ColumnIndex == 4)
            {
                r = dgPedido.SelectedRows[0];
                detalleCompra = (DetalleCompra)r.Tag;
                cboProducto.SelectedValue = detalleCompra.Producto.ProductoId;
                txtPrecioUnit.Text = detalleCompra.PrecioUnidad.ToString();
                txtPrecioTotal.Text = detalleCompra.Total.ToString();
                nudCantidad.Value = detalleCompra.Cantidad;
                cboProducto.Enabled = false;
                editarDetalle = true;
            }
        }

        private void frmComprasAE_Load(object sender, EventArgs e)
        {

        }

        private void cboProveedores_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (cboProveedores.SelectedIndex > 0)
            {
                proveedor = (Proveedor)cboProveedores.SelectedItem;
                //Ver de sobreescribir metodo toString
                txtDireccion.Text = proveedor.Direccion;
                txtLocalidad.Text = proveedor.Localidad.Descripcion;
                txtProvincia.Text = proveedor.Localidad.Provincia.NombreProvincia;
                txtTelefono.Text = proveedor.Telefono;
                txtEmail.Text = proveedor.Email;
                txtSitio.Text = proveedor.Web;
            }
            else
            {
                proveedor = null;
                txtDireccion.Clear();
                txtTelefono.Clear();
                txtLocalidad.Clear();
                txtProvincia.Clear();
                txtEmail.Clear();
                txtSitio.Clear();
            }
        }

        private void cboProducto_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (cboProducto.SelectedIndex > 0)
            {
                ps = new ProductoStock();
                ps.Producto = (Producto)cboProducto.SelectedItem;
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

        private void btnCancelarProducto_Click_1(object sender, EventArgs e)
        {
            InicializarControles();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            frmProveedoresAE frm = new frmProveedoresAE();
            frm.Text = "Nuevo proveedor";
            DialogResult dr = frm.ShowDialog(this);
            if (dr == DialogResult.OK)
            {
                ProveedoresBd.CargarCombo(ref cboProveedores);
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (cboProveedores.SelectedIndex > 0)
            {
                Proveedor proveedor = (Proveedor)cboProveedores.SelectedItem;
                DialogResult dr2 = MessageBox.Show($"¿Está seguro que desea eliminar el proveedor {proveedor.RazonSocial}?",
                   "Confirmar Borrado", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (dr2 == DialogResult.Yes)
                {
                    ProveedoresBd.Borrar(proveedor);
                    MessageBox.Show("Operación exitosa", "Mensaje",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    ProveedoresBd.CargarCombo(ref cboProveedores);
                }
            }        
        }

        private void button3_Click(object sender, EventArgs e)
        {
            frmBuscarProductos frm = new frmBuscarProductos();
            frm.SetProducto(ProductosBd.GetLista());
            DialogResult dr = frm.ShowDialog(this);
            if (dr == DialogResult.OK)
            {
                Producto producto = frm.GetProducto();
                cboProducto.SelectedValue = producto.ProductoId;

            }
        }
    }

}
