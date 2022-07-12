using LAMADatabase;
using LAMAModels;
using LAMAReportes1;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LAMAForms
{
    public partial class frmCaja : Form
    {
        public frmCaja()
        {
            InitializeComponent();
        }
        private static frmCaja frm = null;
        public static frmCaja GetInstancia()
        {
            if (frm == null)
            {
                frm = new frmCaja();
                frm.FormClosed += new FormClosedEventHandler(form_FormClosed);
            }
            return frm;
        }

        private static void form_FormClosed(object sender, FormClosedEventArgs e)
        {
            frm = null;
        }


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            TipoTransaccionesBd.CargarCombo(ref comboBox1);
            textBox1.ReadOnly = true;
            tipo = (TipoTransaccion)comboBox1.Items[1];
            MostrarGrilla(DateTime.Now, DateTime.Now);
        }
        private TipoTransaccion tipo;
        private void button1_Click(object sender, EventArgs e)
        {
            decimal total = transaccionesAMostrar.Sum(a => a.Importe);
            rptCaja2 rpt = Reportes.GetDatos(transaccionesAMostrar, dateTimePicker1.Value, dateTimePicker2.Value, total);
            frmReportes1 frm = new frmReportes1();
            frm.SetReporte(rpt);
            frm.Show();
        }

        private void frmCaja_Load(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex > 0)
            {
                tipo = (TipoTransaccion)comboBox1.SelectedItem;
            }
            else
            {
                tipo = null;
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            if (Validar())
            {
                MostrarGrilla(dateTimePicker1.Value, dateTimePicker2.Value);
            }
        }

        private bool Validar()
        {
            errorProvider1.Clear();
            if (DateTime.Parse(dateTimePicker2.Value.ToShortDateString()) < DateTime.Parse(dateTimePicker1.Value.ToShortDateString()))
            {
                errorProvider1.SetError(dateTimePicker2, "El rango de fechas es incorrecto.");
                return false;
            }
            return true;
        }

        private List<Transaccion> transaccionesAMostrar = null;
        private void MostrarGrilla(DateTime fecha, DateTime fechaHasta)
        {
            try
            {
                List<Transaccion> tran = TransaccionesBd.GetLista();
                if (tipo.TipoTransaccionId != 5)
                {
                    tran = tran.Where(a => a.TipoTransaccion.TipoTransaccionId == tipo.TipoTransaccionId
                      && (DateTime.Parse(a.FechaTransaccion.ToShortDateString()) >= DateTime.Parse(fecha.ToShortDateString()) 
                      && DateTime.Parse(a.FechaTransaccion.ToShortDateString()) <= DateTime.Parse(fechaHasta.ToShortDateString()))).ToList();
                }
                else
                {
                    tran = tran.Where(a => a.TipoTransaccion.TipoTransaccionId != 2
                   && (DateTime.Parse(a.FechaTransaccion.ToShortDateString()) >= DateTime.Parse(fecha.ToShortDateString())
                   && DateTime.Parse(a.FechaTransaccion.ToShortDateString()) <= DateTime.Parse(fechaHasta.ToShortDateString()))).ToList();
                }
                transaccionesAMostrar = tran;
                textBox1.Text = tran.Sum(a => a.Importe).ToString("C");
                MostrarDatosEnGrilla(tran);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void MostrarDatosEnGrilla(List<Transaccion> tran)
        {
            dgvDatos.Rows.Clear();
            foreach (var item in tran)
            {
                DataGridViewRow row = new DataGridViewRow();
                row.CreateCells(dgvDatos);
                SetearFilaAlq(row, item);
                AgregarFilaAlq(row);
            }
        }

        private void AgregarFilaAlq(DataGridViewRow row)
        {
            dgvDatos.Rows.Add(row);
        }

        private void SetearFilaAlq(DataGridViewRow row, Transaccion item)
        {
            row.Cells[cmnNro.Index].Value = item.TransaccionId;
            if (item.Cliente == null || item.Cliente.ClienteId == int.Parse(ConfigurationManager.ConnectionStrings["ConsumidorFinal"].ToString()))
            {
                if (item.Importe < 0)
                {
                    row.Cells[cmnCliente.Index].Value = "Devolución";
                }
                else
                {
                    row.Cells[cmnCliente.Index].Value = "";
                }
            }
            else
            {
                if (item.Importe < 0)
                {
                    row.Cells[cmnCliente.Index].Value = item.Cliente + " - Devolución";
                }
                else
                {
                    row.Cells[cmnCliente.Index].Value = item.Cliente;
                }

            }
            row.Cells[cmnFecha.Index].Value = item.FechaTransaccion.ToShortDateString();
            row.Cells[cmnImporte.Index].Value = item.Importe.ToString("C");
        }

        private void btnAct_Click(object sender, EventArgs e)
        {
            if (Validar())
            {
                MostrarGrilla(dateTimePicker1.Value, dateTimePicker2.Value);
            }
        }
    }
}
