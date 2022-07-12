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
    public partial class frmVerDetalleCompra : Form
    {
        public frmVerDetalleCompra()
        {
            InitializeComponent();
        }

        private Compra compra;
        public void SetObjeto(Compra compra)
        {
            this.compra = compra;
        }

        private void frmVerDetalleCompra_Load(object sender, EventArgs e)
        {
            if (compra != null)
            {
                if (compra.Proveedor != null)
                {
                    txtProveedor.Text = compra.Proveedor.RazonSocial;
                    txtDireccion.Text = compra.Proveedor.Direccion;
                    txtLocalidad.Text = compra.Proveedor.Localidad.Descripcion;
                    txtProvincia.Text = compra.Proveedor.Localidad.Provincia.NombreProvincia;
                    txtCP.Text = compra.Proveedor.Telefono;
                }
                else
                {
                    txtProveedor.Text = "Proveedor eliminado.";
                }
              
                foreach (var detallesCompra in compra.DetallesCompras)
                {
                    DataGridViewRow r = new DataGridViewRow();
                    r.CreateCells(dgPedido);
                    SetearFila(r, detallesCompra);
                    AgregarFila(r);
                }
                txtTotalPedido.Text = compra.DetallesCompras.Sum(dt => dt.Total).ToString("C");
              
            }
        }

        private void SetearFila(DataGridViewRow r, DetalleCompra detalleCompra)
        {
            if (detalleCompra.Producto == null)
            {
                r.Cells[cmnProducto.Index].Value = "Eliminado";

            }
            else
            {
                r.Cells[cmnProducto.Index].Value = detalleCompra.ToString();
            }
            r.Cells[cmnCantidad.Index].Value = detalleCompra.Cantidad;
            r.Cells[cmnPrecioUnitario.Index].Value = detalleCompra.PrecioUnidad.ToString("C");
            r.Cells[cmnTotal.Index].Value = detalleCompra.Total.ToString("C");
        }

        private void AgregarFila(DataGridViewRow dataGridViewRow)
        {
            dgPedido.Rows.Add(dataGridViewRow);
        }


    }

}
