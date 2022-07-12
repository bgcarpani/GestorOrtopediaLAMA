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
    public partial class frmInicio : Form
    {
        public frmInicio()
        {
            InitializeComponent();
        }

        private static frmInicio frm = null;

        public static frmInicio GetInstancia()
        {
            if (frm == null)
            {
                frm = new frmInicio();
                frm.FormClosed += new FormClosedEventHandler(form_FormClosed);
            }
            return frm;
        }

        private static void form_FormClosed(object sender, FormClosedEventArgs e)
        {
            frm = null;
        }

        private void timerHora_Tick(object sender, EventArgs e)
        {
            lblHora.Text = DateTime.Now.ToLongTimeString();
        }

        private void frmInicio_Load(object sender, EventArgs e)
        {
            this.Dock = DockStyle.Fill;
            lblFecha.Text = DateTime.Now.ToLongDateString();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            GrillaInicial();
            GrillaInicialStocks();
        }

        private void GrillaInicialStocks()
        {
            try
            {
                dataGridView2.Rows.Clear();
                List<ProductoStock> ps = ProductoStocksBd.GetLista();
                MostrarDatosGrillaStock(ps);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void MostrarDatosGrillaStock(List<ProductoStock> ps)
        {
            dataGridView2.Rows.Clear();
            foreach (var item in ps)
            {
                DataGridViewRow row = new DataGridViewRow();
                row.CreateCells(dataGridView2);
                SetearFilaStocks(row, item);
                AgregarFilaStock(row);
            }
        }

        private void AgregarFilaStock(DataGridViewRow row)
        {
            dataGridView2.Rows.Add(row);
        }

        private void SetearFilaStocks(DataGridViewRow row, ProductoStock item)
        {
            row.Cells[cmnStockDe.Index].Value = item.Stock.Descripcion;
            row.Cells[cmnProducto.Index].Value = item.Producto.NombreProducto;
            row.Cells[cmnDisponible.Index].Value = item.StockDisponible;
            if (item.StockDisponible <= 5)
            {
                row.Cells[cmnDisponible.Index].Style.BackColor = Color.LightYellow;
            }
            if (item.StockDisponible <= 0)
            {
                row.Cells[cmnDisponible.Index].Style.BackColor = Color.Red;
            }
          
            row.Tag = item;
        }

        public void GrillaInicial()
        {
            try
            {
                List<Alquiler> alquileres = AlquileresBd.GetLista();
                alquileres = alquileres.Where(a=>a.EstaEnUso == true).ToList();

                MostrarDatosEnGrillaAlquileres(alquileres);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }

        }

        private void MostrarDatosEnGrillaAlquileres(List<Alquiler> alquileres)
        {
            dataGridView1.Rows.Clear();
            foreach (var item in alquileres)
            {
                DataGridViewRow row = new DataGridViewRow();
                row.CreateCells(dataGridView1);
                SetearFilaAlq(row, item);
                AgregarFilaAlq(row);
            }
        }

        private void AgregarFilaAlq(DataGridViewRow row)
        {
            dataGridView1.Rows.Add(row);
        }

        private void SetearFilaAlq(DataGridViewRow row, Alquiler item)
        {
            if (!item.EstaEnUso)
            {
                row.DefaultCellStyle.BackColor = Color.LightGray;
            }
            row.Cells[cmnIdAlquiler.Index].Value = item.AlquilerId; 
            if (item.Cliente == null)
            {
                row.Cells[cmnCliente.Index].Value = "Cliente eliminado";
            }
            else
            {
                row.Cells[cmnCliente.Index].Value = item.Cliente.ToString();
            }
            row.Cells[cmnFechaD.Index].Value = item.FechaDesde.ToShortDateString();
            row.Cells[cmnFechaH.Index].Value = item.FechaHasta.ToShortDateString();
            if (item.EstaEnUso == true)
            {

                if (item.FechaHasta.AddDays(5) < DateTime.Now)
                {
                    row.Cells[cmnFechaH.Index].Style.BackColor = Color.LightYellow;
                }
                else if (item.FechaHasta < DateTime.Now)
                {
                    row.Cells[cmnFechaH.Index].Style.BackColor = Color.Red;
                }
                row.Cells[cmnEnUso.Index].Value = "En uso";
                row.Cells[cmnEnUso.Index].Style.BackColor = Color.Green;
            }
            else
            {
                row.Cells[cmnEnUso.Index].Value = item.FechaDevolucion;
            }
            row.Tag = item;
        }

        List<Caja> lista;

        List<Alquiler> alquileres;
      
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {

        }


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            GrillaInicial();
            GrillaInicialStocks();
        }

        private void btnCaja_Click(object sender, EventArgs e)
        {
            frmCaja frm = frmCaja.GetInstancia();
            frm.Show();
        }

        private void frmInicio_MouseClick(object sender, MouseEventArgs e)
        {
        }
    }
}
