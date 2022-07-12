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
    public partial class frmBuscarProductos : Form
    {
        public frmBuscarProductos()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            btnOk.Enabled = false;
        }
        private ProductoStock pstock = null;
        private void btnOk_Click(object sender, EventArgs e)
        {
            DataGridViewRow row = dgPedido.SelectedRows[0];
            if (productos != null)
            {
                producto = (Producto)row.Tag;
            }
            else if (ps != null)
            {
                pstock = (ProductoStock)row.Tag;
            }
            else if (proteList != null)
            {
                protesis = (Protesis)row.Tag;
            }
            DialogResult = DialogResult.OK;
        }

        private List<Producto> productos = null;
        private Producto producto;

        internal void SetProducto(List<Producto> prod)
        {
            this.productos = prod;
        }
        internal Producto GetProducto()
        {
            return producto;
        }
        internal ProductoStock GetProductoStock()
        {
            return pstock;
        }
        private void MostrarDatosGrilla(List<Producto> lista)
        {
            dgPedido.Rows.Clear();
            foreach (Producto item in lista)
            {
                DataGridViewRow r = new DataGridViewRow();
                r.CreateCells(dgPedido);
                SetearFila(r, item);
                AgregarFila(r);
            }
        }

        private void AgregarFila(DataGridViewRow r)
        {
            dgPedido.Rows.Add(r);
        }

        private void SetearFila(DataGridViewRow r, Producto item)
        {
            r.Cells[cmnId.Index].Value = item.ProductoId;
            r.Cells[cmnNombre.Index].Value = item.NombreProducto;

            r.Tag = item;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string value = textBox1.Text;
            if (productos != null)
            {
                productos = productos.OrderBy(c => c.NombreProducto).ToList();
                List<Producto> listaProvisoria = null;
                listaProvisoria = productos.Where(c => c.NombreProducto.ToUpper().Contains(value.ToUpper())).ToList();
                if (listaProvisoria.Any())
                {
                    btnOk.Enabled = true;
                }
                else
                {
                    btnOk.Enabled = false;
                }
                MostrarDatosGrilla(listaProvisoria);
            }
            else if (ps != null)
            {
                ps = ps.OrderBy(c => c.Producto.NombreProducto).ToList();
                List<ProductoStock> listaProvisoria = null;
                listaProvisoria = ps.Where(c => c.Producto.NombreProducto.ToUpper().Contains(value.ToUpper())).ToList();
                if (listaProvisoria.Any())
                {
                    btnOk.Enabled = true;
                }
                else
                {
                    btnOk.Enabled = false;
                }
                MostrarDatosGrillaPS(listaProvisoria);
            }
            else if (proteList != null)
            {
                proteList = proteList.OrderBy(c => c.NombreProtesis).ToList();
                List<Protesis> listaProvisoria = null;
                listaProvisoria = proteList.Where(c => c.NombreProtesis.ToUpper().Contains(value.ToUpper())).ToList();
                if (listaProvisoria.Any())
                {
                    btnOk.Enabled = true;
                }
                else
                {
                    btnOk.Enabled = false;
                }
                MostrarDatosGrillaProte(listaProvisoria);
            }
           
        }

        private void MostrarDatosGrillaProte(List<Protesis> lista)
        {
            dgPedido.Rows.Clear();
            foreach (Protesis item in lista)
            {
                DataGridViewRow r = new DataGridViewRow();
                r.CreateCells(dgPedido);
                SetearFilaProte(r, item);
                AgregarFila(r);
            }
        }

        private void SetearFilaProte(DataGridViewRow r, Protesis item)
        {
            r.Cells[cmnId.Index].Value = item.ProtesisId;
            r.Cells[cmnNombre.Index].Value = item.NombreProtesis;
            dgPedido.Columns[cmnNombre.Index].HeaderText = "Protesis";
            r.Tag = item;
        }

        private void MostrarDatosGrillaPS(List<ProductoStock> lista)
        {
            dgPedido.Rows.Clear();
            foreach (ProductoStock item in lista)
            {
                DataGridViewRow r = new DataGridViewRow();
                r.CreateCells(dgPedido);
                SetearFilaPS(r, item);
                AgregarFila(r);
            }
        }

        private void SetearFilaPS(DataGridViewRow r, ProductoStock item)
        {
            r.Cells[cmnId.Index].Value = item.Producto.ProductoId;
            r.Cells[cmnNombre.Index].Value = item.Producto.NombreProducto;
            r.Tag = item;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            producto = null;
            DialogResult = DialogResult.Cancel;
        }
        private int stockIndex = 0;

        private List<ProductoStock> ps = null;
        internal void SetProductos(List<ProductoStock> productoStocks, int stockIndex)
        {
            this.ps = productoStocks;
            this.stockIndex = stockIndex;
        }

        private List<Protesis> proteList = null;
        private Protesis protesis = null;
        internal Protesis GetProtesis()
        {
            return protesis;
        }

        internal void SetProtesis(List<Protesis> proteses)
        {
            this.proteList = proteses;
        }
    }
}
