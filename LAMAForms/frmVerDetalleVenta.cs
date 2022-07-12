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
    public partial class frmVerDetalleVenta : Form
    {
        public frmVerDetalleVenta()
        {
            InitializeComponent();
        }

        private Venta venta;
        private void frmVerDetalleVenta_Load(object sender, EventArgs e)
        {
            if (venta != null)
            {
                txtCliente.Text = venta.Cliente.ToString();
                txtDireccion.Text = venta.Cliente.Domicilio.Direccion;
                txtProvincia.Text = venta.Cliente.Domicilio.Provincia.NombreProvincia;
                txtLocalidad.Text = venta.Cliente.Domicilio.Localidad.Descripcion;
                txtDNI.Text = venta.Cliente.DNI;
                dtpFechaPedido.Value = venta.FechaVenta;
                foreach (var detalleVenta in venta.Detalle)
                {
                    DataGridViewRow row = new DataGridViewRow();
                    row.CreateCells(dgPedido);
                    SetearFila(row, detalleVenta);
                    AgregarFila(row);
                }
                txtTotalPedido.Text = venta.Detalle.Sum(dt => dt.Total).ToString("C");
            }
        }

        private void SetearFila(DataGridViewRow row, DetalleVenta detalleVenta)
        {
            if (detalleVenta.Producto == null)
            {
                row.Cells[cmnProducto.Index].Value = "Eliminado";
            }
            else
            {
                row.Cells[cmnProducto.Index].Value = detalleVenta.ToString();
            }
            row.Cells[cmnPrecioUnitario.Index].Value = detalleVenta.PrecioUnidad.ToString("C");
            row.Cells[cmnCantidad.Index].Value = detalleVenta.Cantidad;
            row.Cells[cmnTotal.Index].Value = detalleVenta.Total.ToString("C");
        }

        private void AgregarFila(DataGridViewRow row)
        {
            dgPedido.Rows.Add(row);
        }

        public void SetObjeto(Venta venta1)
        {
            venta = venta1;
        }
    }
}
