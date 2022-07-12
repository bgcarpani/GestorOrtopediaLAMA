using LAMADatabase;
using LAMAModels;
using LAMAReportes1;
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
    public partial class frmDetalleCtaCte : Form
    {
        public frmDetalleCtaCte()
        {
            InitializeComponent();
        }

        private ConsultaCtaCte consulta;


        public void SetCtaCte(ConsultaCtaCte consulta)
        {
            this.consulta = consulta;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private List<CtaCte> lista;
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            txtCliente.Enabled = false;
            txtDesc.Enabled = false;
            txtTotalAPagar.Enabled = false;
            dtpFecha.Enabled = false;
            btnIngresarPago.Enabled = false;

            if (consulta != null)
            {
                Cliente cliente = consulta.Cliente;
                txtCliente.Text = cliente.ToString();
                txtDireccion.Text = cliente.Domicilio.Direccion;
                txtLocalidad.Text = cliente.Domicilio.Localidad.Descripcion;
                txtProvincia.Text = cliente.Domicilio.Provincia.NombreProvincia;
                txtCP.Text = cliente.Domicilio.CodigoPostal;
                lista = CtaCteBd.GetMovimientos(cliente);
                if (lista.Count > 0)
                {
                    MostrarDatosEnGrilla(lista);
                }
                //FormasDePagoBd.CargarCombo(ref cboForma);

                ActualizarSaldo();
            }
        }

        private void MostrarDatosEnGrilla(List<CtaCte> lista)
        {
            dgDatos.Rows.Clear();
            foreach (var item in lista)
            {
                DataGridViewRow row = new DataGridViewRow();
                row.CreateCells(dgDatos);
                SetearFila(item, row);
                AgregarFila(row);
            }
        }

        private void AgregarFila(DataGridViewRow row)
        {
            dgDatos.Rows.Add(row);
        }
        private Pago pago;
        private CtaCte cuenta;

        private void SetearFila(CtaCte item, DataGridViewRow row)
        {
            row.Cells[cmnFecha.Index].Value = item.FechaMovimiento;
            row.Cells[cmnMovimiento.Index].Value = item.Movimiento;
            row.Cells[cmnDebe.Index].Value = item.Debe;
            row.Cells[cmnHaber.Index].Value = item.Haber;
        }

        private int numComp;

        private void LimpiarPagos()
        {
            txtTotalAPagar.Clear();
            txtDesc.Clear();
           // cboForma.SelectedValue = 0;
        }

        private void ActualizarSaldo()
        {
            decimal saldo = CtaCteBd.GetSaldo(consulta.Cliente);
            txtSaldo.Text = saldo.ToString("C");
        }

        private bool ValidarPago()
        {
            bool valido = true;
            errorProvider1.Clear();
            decimal pago;
            if (!decimal.TryParse(txtTotalAPagar.Text, out pago))
            {
                valido = false;
                errorProvider1.SetError(txtTotalAPagar, "Datos incorrectos");
            }
            if (pago < 0)
            {
                valido = false;
                errorProvider1.SetError(txtTotalAPagar, "El monto ingresado debe ser mayor a 0");
            }
            //if (cboForma.SelectedIndex == 0)
            //{
            //    valido = false;
            //    errorProvider1.SetError(cboForma, "Seleccione una forma de pago");
            //}
            if (string.IsNullOrEmpty(txtDesc.Text))
            {
                valido = false;
                errorProvider1.SetError(txtDesc, "Ingrese una descripcion");
            }
            return valido;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;

        }


        //private void btnImprimirCtaCte_Click(object sender, EventArgs e)
        //{
        //  
        //}

        private void frmDetalleCtaCte_Load(object sender, EventArgs e)
        {

        }

        private void btnIngresarPago_Click_1(object sender, EventArgs e)
        {

        }

        private void btnImprimirCtaCte_Click(object sender, EventArgs e)
        {
            //consulta.DetalleCtaCte = CtaCteBd.GetDetalle(consulta.Cliente);
            //rptContratoAlqiuler1 rpt = Reportes.GetDatos(consulta);
            //frmReportes frm = new frmReportes();
            //frm.SetReporte(rpt);
            //frm.Show();

        }
    }

}
