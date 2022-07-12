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
    public partial class frmStocks : Form
    {
        public frmStocks()
        {
            InitializeComponent();
        }

        public static frmStocks frm = null;
        public static frmStocks GetInstancia()
        {
            if (frm == null)
            {
                frm = new frmStocks();
                frm.FormClosed += frm_FormClosed;
            }
            return frm;
        }

        private static void frm_FormClosed(object sender, FormClosedEventArgs e)
        {
            frm = null;
        }

        public ProductoStock ps;
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            StocksBd.CargarCombo(ref cboStocks1);
            StocksBd.CargarCombo(ref cboStockBaja);
            StocksBd.CargarCombo(ref cboStocks2);
            cboStocks1.SelectedIndex = 1;
            cboStocks2.SelectedIndex = 2;
            ProductoStocksBd.CargarCombo(ref cboProducto, cboStocks1.SelectedIndex);
            ProductoStocksBd.CargarCombo(ref cboProductoBaja, cboStockBaja.SelectedIndex);
        }

        private void btnAceptarProducto_Click(object sender, EventArgs e)
        {
            if (ValidarDatos())
            {
                if (tabControl1.SelectedTab == tabPage2)
                {
                    if (ps.StockDisponible > 0)
                    {
                        Stock stockDesde = (Stock)cboStocks1.SelectedItem;
                        Stock stockHasta = (Stock)cboStocks2.SelectedItem;
                        ProductoStocksBd.MoverStock(stockDesde.StockId, stockHasta.StockId, (int)nudCantidad.Value, ps.Producto.ProductoId);
                        MessageBox.Show("Operación exitosa", "Mensaje", MessageBoxButtons.OK,
                               MessageBoxIcon.Information);
                        txtStock.Text = ps.StockDisponible.ToString();
                    }
                }
                else
                {
                    Stock stock = (Stock)cboStockBaja.SelectedItem;
                    ProductoStocksBd.BajarStock(stock.StockId, (int)nudCantidadBaja.Value, ps.Producto.ProductoId);
                    MessageBox.Show("Operación exitosa", "Mensaje", MessageBoxButtons.OK,
                       MessageBoxIcon.Information);
                    txtStockBaja.Text = ps.StockDisponible.ToString();
                           
                }

            }
        }

        private bool ValidarDatos()
        {
            errorProvider1.Clear();
            if (tabControl1.SelectedTab == tabPage2)
            {
                if (ps == null)
                {
                    errorProvider1.SetError(cboProducto, "Debe seleccionar un producto");
                    return false;
                }
                if (cboStocks1.SelectedIndex == cboStocks2.SelectedIndex)
                {
                    errorProvider1.SetError(cboStocks1, "Estos valores no pueden ser iguales");
                    errorProvider1.SetError(cboStocks2, "Estos valores no pueden ser iguales");
                    return false;
                }
                if (nudCantidad.Value <= 0)
                {
                    errorProvider1.SetError(nudCantidad, "Cantidad no puede ser menor o igual a 0");
                    return false;
                }
                if (ps.StockDisponible < nudCantidad.Value)
                {
                    errorProvider1.SetError(nudCantidad, "No hay stock suficiente");
                    return false;
                }
            }
            else
            {
                if (ps == null)
                {
                    errorProvider1.SetError(cboProductoBaja, "Debe seleccionar un producto");
                    return false;
                }
                if (nudCantidadBaja.Value <= 0)
                {
                    errorProvider1.SetError(nudCantidadBaja, "Cantidad no puede ser menor o igual a 0");
                    return false;
                }
                if (ps.StockDisponible < nudCantidadBaja.Value)
                {
                    errorProvider1.SetError(nudCantidadBaja, "No hay stock suficiente");
                    return false;
                }
            }
           
            return true;
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void frmStocks_Load(object sender, EventArgs e)
        {

        }

        private void cboProducto_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboProducto.SelectedIndex > 0)
            {
                ps = (ProductoStock)cboProducto.SelectedItem;
                txtStock.Text = ps.StockDisponible.ToString();
            }
            else
            {
                ps = null;
                txtStock.Clear();
            }
        }

        private void btnCancelarProducto_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cboStocks1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ProductoStocksBd.CargarCombo(ref cboProducto, cboStocks1.SelectedIndex);
        }

        private void cboProductoBaja_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboProductoBaja.SelectedIndex > 0)
            {
                ps = (ProductoStock)cboProductoBaja.SelectedItem;
                txtStockBaja.Text = ps.StockDisponible.ToString();
            }
            else
            {
                ps = null;
                txtStockBaja.Clear();
            }
        }

        private void cboStockBaja_SelectedIndexChanged(object sender, EventArgs e)
        {
            ProductoStocksBd.CargarCombo(ref cboProductoBaja, cboStockBaja.SelectedIndex);

        }
    }
}
