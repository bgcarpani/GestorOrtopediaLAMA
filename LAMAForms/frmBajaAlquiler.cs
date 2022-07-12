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
    public partial class frmBajaAlquiler : Form
    {
        public frmBajaAlquiler()
        {
            InitializeComponent();
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            MostrarDatosEnGrilla();
   
        }
        private DataGridViewRow row;
        private void MostrarDatosEnGrilla()
        {
            foreach (var item in alquiler.Detalle)
            {
                row = new DataGridViewRow();
                row.CreateCells(dgPedido);
                SetearFila(row, item);
                AgregarFila(row);
            }
        }

        private void AgregarFila(DataGridViewRow row)
        {
            dgPedido.Rows.Add(row);
        }

        private void SetearFila(DataGridViewRow row, DetalleAlquiler item)
        {
            row.Cells[cmnProducto.Index].Value = item.ToString();
            row.Cells[cmnCantidad.Index].Value = item.Cantidad;
            row.Tag = item;
        }

        private void frmBajaAlquiler_Load(object sender, EventArgs e)
        {

        }

        private Alquiler alquiler;
        internal void SetAlquiler(Alquiler alq)
        {
            this.alquiler = alq;
        }

        private List<DetalleAlquiler> bajaDetalle;
        private void btnOK_Click(object sender, EventArgs e)
        {
            bajaDetalle = new List<DetalleAlquiler>();
            foreach (DataGridViewRow row in dgPedido.Rows)
            {
                if (row.Cells[cmnChk.Index].Value != null)
                {
                    if (bool.Parse(row.Cells[cmnChk.Index]?.Value?.ToString()) == true)
                    {
                        DetalleAlquiler da = (DetalleAlquiler)row.Tag;
                        bajaDetalle.Add(da);
                    }
                }
                
            }
            DialogResult = DialogResult.OK;
        }

        internal List<DetalleAlquiler> GetDetalleDeBaja()
        {
            return bajaDetalle;
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void dgPedido_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                DataGridViewCheckBoxCell chkSeleccionar =
                    (DataGridViewCheckBoxCell)dgPedido.Rows[e.RowIndex].Cells[cmnChk.Index];
                chkSeleccionar.Value = !Convert.ToBoolean(chkSeleccionar.Value);
            }
        }
    }
}
