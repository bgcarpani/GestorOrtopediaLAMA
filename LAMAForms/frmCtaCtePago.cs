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
    public partial class frmCtaCtePago : Form
    {
        public frmCtaCtePago()
        {
            InitializeComponent();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private ConsultaCtaCte consulta;
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (detalle)
            {
                btnIngresarPago.Enabled = false;
                txtTotalAPagar.Enabled = false;
                dtpFecha.Enabled = false;
                txtDesc.Enabled = false;
                cboClientes.Enabled = false;
                button2.Enabled = false;
            }
            if (consulta != null)
            {
                txtCliente.Visible = true;
                cboClientes.Visible = false;
                cboClientes.Enabled = false;
                Cliente cliente = consulta.Cliente;
                txtCliente.Text = cliente.ToString();
                txtDireccion.Text = cliente.Domicilio.Direccion;
                txtLocalidad.Text = cliente.Domicilio.Localidad.Descripcion;
                txtDni.Text = cliente.DNI;
                List<Pago> lista = PagosBd.GetPagos(cliente);
                button2.Enabled = false;
                if (lista.Count > 0)
                {
                    MostrarDatosEnGrilla(lista);
                }
                ActualizarSaldo(cliente);
            }
            else
            {
                if (pago != null)
                {
                    ClientesBd.CargarCombo(ref cboClientes);
                    txtCliente.Visible = false;
                    cboClientes.Visible = true;
                    cboClientes.Enabled = true;
                    Cliente cliente = pago.Cliente;
                    cboClientes.SelectedValue = cliente.ClienteId;
                    txtCliente.Text = cliente.ToString();
                    txtDireccion.Text = cliente.Domicilio.Direccion;
                    txtLocalidad.Text = cliente.Domicilio.Localidad.Descripcion;
                    txtDni.Text = cliente.DNI;
                    ActualizarSaldo(cliente);
                }
                else
                {
                    txtTotalAPagar.Text = "0";
                    textBox1.Text = "0";
                    txtCliente.Visible = false;
                    cboClientes.Visible = true;
                    cboClientes.Enabled = true;
                    ClientesBd.CargarCombo(ref cboClientes);
                }     
            }
        }

        private void MostrarDatosEnGrilla(List<Pago> lista)
        {
            dgDatos.Rows.Clear();
            foreach (var item in lista)
            {
                DataGridViewRow row = new DataGridViewRow();
                row.CreateCells(dgDatos);
                SetearFila(row, item);
                AgregarFila(row);
            }
        }

        private void AgregarFila(DataGridViewRow row)
        {
            dgDatos.Rows.Add(row);
        }

        private void SetearFila(DataGridViewRow row, Pago item)
        {
            int id = PagosBd.GetUltimo(item.Cliente.ClienteId);
            if (item.PagoId == 0)
            {
                row.Cells[cmnNroPago.Index].Value = id+1;
            }
            else
            {
                row.Cells[cmnNroPago.Index].Value = item.PagoId;
            }
            row.Cells[cmnDescripcion.Index].Value = item.Descripcion;
            row.Cells[cmnImporte.Index].Value = item.Importe;
        }

        public void SetConsulta(ConsultaCtaCte consultaCtaCte)
        {
            consulta = consultaCtaCte;
        }

        private int numComp;


        Cliente cliente;
        private void cboClientes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboClientes.SelectedIndex > 0)
            {
                cliente = (Cliente)cboClientes.SelectedItem;
                txtDireccion.Text = cliente.Domicilio.Direccion;
                txtLocalidad.Text = cliente.Domicilio.Localidad.Descripcion;
                txtDni.Text = cliente.DNI;
                ActualizarSaldo(cliente);
                if (!editar)
                {
                    List<Pago> lista = PagosBd.GetPagos(cliente);
                    if (lista.Count > 0)
                    {
                        MostrarDatosEnGrilla(lista);
                    }
                 
                }

            }
            else
            {
                cliente = null;
                txtDireccion.Clear();
                txtLocalidad.Clear();
                txtDni.Clear();
            }
        }

        private void LimpiarPagos()
        {
            txtTotalAPagar.Clear();
            txtDesc.Clear();
            //cboForma.SelectedValue = 0;
        }

        private void ActualizarSaldo(Cliente cliente)
        {
            decimal saldo = 0;

            saldo = CtaCteBd.GetSaldo(cliente);
      
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
            return valido;
        }

        private void frmCtaCtePago_Load(object sender, EventArgs e)
        {

        }

        private void btnCancelar_Click_1(object sender, EventArgs e)
        {
            pago = null;
            cuenta = null;
            DialogResult = DialogResult.Cancel;
        }

        private void btnOK_Click_1(object sender, EventArgs e)
        {
            if (!editar && !detalle)
            {
                PagosBd.Agregar(pago);
            }
            DialogResult = DialogResult.OK;
        }

        private CtaCte cuenta;
        private Pago pago;

        private void btnIngresarPago_Click(object sender, EventArgs e)
        {

            if (ValidarPago())
            {
                var nroComprobante = $"{DateTime.Now.Year}{DateTime.Now.Month}{DateTime.Now.Day}";
                Cliente tmpCliente;
                if (consulta != null)
                    tmpCliente = consulta.Cliente;
                else
                    tmpCliente = cliente;

                cuenta = new CtaCte();
                cuenta.Cliente = tmpCliente;
                cuenta.FechaMovimiento = dtpFecha.Value;
                cuenta.Debe = 0;
                cuenta.Haber = decimal.Parse(txtTotalAPagar.Text);
                decimal saldo = CtaCteBd.GetSaldo(tmpCliente);
                cuenta.Saldo = saldo;

                pago = new Pago();
                pago.Descripcion = $"PAGO-{numComp + 1}";
                pago.FechaPago = dtpFecha.Value;
                pago.Cliente = tmpCliente;
                pago.CtaCte = cuenta;
                if (string.IsNullOrEmpty(textBox1.Text))
                {
                    textBox1.Text = "0";
                }
                pago.ImporteOS = decimal.Parse(textBox1.Text);
                pago.Importe = decimal.Parse(txtTotalAPagar.Text)+pago.ImporteOS;

                numComp = PagosBd.GetUltimo(tmpCliente.ClienteId);
                cuenta.Movimiento = $"PA {numComp + 1}";

                DataGridViewRow row = new DataGridViewRow();
                row.CreateCells(dgDatos);
                SetearFila(row, pago);
                AgregarFila(row);

                ActualizarSaldo(tmpCliente);
                LimpiarPagos();

            }
        }
        bool detalle = false;

        internal void SetDetalle(bool v)
        {
            this.detalle = v;
        }

        bool editar = false;

        internal void SetEditar(bool v)
        {
            this.editar = v;
        }

        internal Pago GetPago()
        {
            return pago;
        }

        internal void SetPago(Pago pago)
        {
            this.pago = pago;
        }

        private void btnImprimirCtaCte_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            frmBuscarClientes frm = new frmBuscarClientes();
            DialogResult dr = frm.ShowDialog(this);
            if (dr == DialogResult.OK)
            {
                Cliente cliente = frm.GetCliente();
                cboClientes.SelectedValue = cliente.ClienteId;

            }
        }
    }

}
