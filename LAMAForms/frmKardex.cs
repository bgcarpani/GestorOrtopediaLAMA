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
    public partial class frmKardex : Form
    {
        public frmKardex()
        {
            InitializeComponent();
        }

        private static frmKardex frm = null;
        public static frmKardex GetInstancia()
        {
            if (frm == null)
            {
                frm = new frmKardex();
                frm.FormClosed += new FormClosedEventHandler(frm_FormClose);

            }
            return frm;
        }

        private static void frm_FormClose(object sender, FormClosedEventArgs e)
        {
            frm = null;
        }

        private void btnConsultar_Click(object sender, EventArgs e)
        {
       
        }

        private void MostrarDatosGrilla(List<Kardex> lista)
        {
            dgvDatos.Rows.Clear();
            foreach (var item in lista)
            {
                DataGridViewRow r = new DataGridViewRow();
                r.CreateCells(dgvDatos);
                SetearFila(r, item);
                AgregarFila(r);
            }
        }

        private void AgregarFila(DataGridViewRow r)
        {
            dgvDatos.Rows.Add(r);
        }

        private void SetearFila(DataGridViewRow r, Kardex item)
        {
            r.Cells[cmnFecha.Index].Value = item.FechaMovimiento;
            r.Cells[cmnMovimiento.Index].Value = item.Movimiento;
            r.Cells[cmnEntrada.Index].Value = item.Entrada;
            r.Cells[cmnSalida.Index].Value = item.Salida;
            r.Cells[cmnSaldo.Index].Value = item.Saldo;
            r.Cells[cmnUltimoCosto.Index].Value = item.UltimoCosto;
            r.Cells[cmnCostoPromedio.Index].Value = item.CostoPromedio;
            r.Tag = item;
        }

        private bool ValidarDatos()
        {
            if (cboProductos.SelectedIndex!=0)
            {
                return true;
            }
            else
            {
                return false;
            }
            
        }

        private void frmKardex_Load(object sender, EventArgs e)
        {
            ProductoStocksBd.CargarCombo(ref cboProductos, 1);
        }

        private void cboProductos_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ValidarDatos())
            {
                try
                {
                    List<Kardex> lista = KardexBd.ConsultarKardex((ProductoStock)cboProductos.SelectedItem);
                    if (lista.Count > 0)
                    {
                        MostrarDatosGrilla(lista);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error");
                }
            }
        }

        private void dgvDatos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }

}
