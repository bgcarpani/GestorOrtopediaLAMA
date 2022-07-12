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
    public partial class frmOrdenesAE : Form
    {
        public frmOrdenesAE()
        {
            InitializeComponent();
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }
        private Protesis protesis;
        private Cliente cliente;
        private Provincia provincia;
        private decimal importeOs = 0;
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (ValidarOrden())
            {
                if (orden == null)
                {
                    orden = new Orden();
                }
                if (protesis == null)
                {
                    protesis = new Protesis();
                }               
                orden.Cliente = (Cliente)cboCliente.SelectedItem;
                orden.Protesis = protesis;
                orden.DiasEstimados = int.Parse(txtDiasEstimados.Text);
                orden.Notas = protesis.Tipo + " " + protesis.Descripcion;
                if (string.IsNullOrEmpty(txtSenia.Text))
                {
                    txtSenia.Text = "0";
                }
                if (chkObra.Checked)
                {
                    orden.ImporteOS = decimal.Parse(txtObra.Text);
                }
                else
                {
                    orden.ImporteOS = 0;
                }
                orden.Senia = decimal.Parse(txtSenia.Text);
                orden.Costo = protesis.Importe;
                orden.FechaInicio = dtpFechaVenta.Value;
                orden.FechaEntrega = DateTime.Now.AddDays(orden.DiasEstimados);
                orden.Eliminado = false;
                orden.Entregado = false;
                orden.FechaEliminacion = new DateTime(1900, 1, 1);
                DialogResult = DialogResult.OK;
            }
        }

        private bool ValidarOrden()
        {
            errorProvider1.Clear();
            if (checkBox1.Checked == false)
            {
                if (cboCliente.SelectedIndex == 0)
                {
                    errorProvider1.SetError(cboCliente, "Seleccione un cliente");
                    return false;
                }
                if (cboProtesis.SelectedIndex == 0)
                {
                    errorProvider1.SetError(comboBox2, "Seleccione una Protesis");
                    return false;
                }
            }
            if (string.IsNullOrEmpty(txtDiasEstimados.Text))
            {
                errorProvider1.SetError(txtDiasEstimados, "Este campo no puede estar vacío.");
                return false;
            }
            if (string.IsNullOrEmpty(txtObra.Text))
            {
                errorProvider1.SetError(txtObra, "Este campo no puede estar vacío.");
                return false;
            }
            if (!int.TryParse(txtDiasEstimados.Text, out int resultEstimados))
            {
                errorProvider1.SetError(txtDiasEstimados, "Valor inválido.");
                return false;
            }
            if (!decimal.TryParse(txtSenia.Text, out decimal result))
            {
                errorProvider1.SetError(txtSenia, "Valor inválido.");
                return false;
            }
            return true;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private Orden orden;
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            ClientesBd.CargarCombo(ref cboCliente);
            ProtesisBd.CargarCombo(ref cboProtesis);
            checkBox1.Enabled = false;
            checkBox1.Checked = false;
            checkBox1.Visible = false;
            comboBox1.Visible = false;
            comboBox1.Enabled = false;
            comboBox2.Visible = false;
            txtDescripcion.ReadOnly = true;
            txtObra.Enabled = false;
            txtCosto.ReadOnly = true;
            txtObra.Text = "0";
            txtSenia.Text = "0";
            txtDiasEstimados.Text = "7";
            if (orden != null)
            {
                button2.Enabled = false;
                ClientesBd.CargarCombo(ref cboCliente, orden.Cliente.ClienteId);
                ProtesisBd.CargarCombo(ref cboProtesis, orden.Protesis.ProtesisId);
                cboCliente.SelectedValue = orden.Cliente.ClienteId;
                txtDireccion.Text = orden.Cliente.Domicilio.Direccion;
                txtDNI.Text = orden.Cliente.DNI;
                txtProvincia.Text = orden.Cliente.Domicilio.Provincia.NombreProvincia;
                txtLocalidad.Text = orden.Cliente.Domicilio.Localidad.Descripcion;
                txtTelefono.Text = orden.Cliente.TelefonoMovil;
                txtCP.Text = orden.Cliente.Domicilio.CodigoPostal;
                cboProtesis.SelectedValue = orden.Protesis.ProtesisId;
                txtDescripcion.Text = orden.Protesis.Descripcion;
                txtSenia.Text = orden.Senia.ToString();
                txtCosto.Text = orden.Costo.ToString();
                txtDiasEstimados.Text = orden.DiasEstimados.ToString();
            }
        }

        internal Orden GetObjeto()
        {
            return orden;
        }
        private void frmOrdenesAE_Load(object sender, EventArgs e)
        {

        }

        private void cboCliente_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboCliente.SelectedIndex > 0)
            {
                cliente = (Cliente)cboCliente.SelectedItem;
                txtDireccion.Text = cliente.Domicilio.Direccion;
                txtLocalidad.Text = cliente.Domicilio.Localidad.Descripcion;
                txtProvincia.Text = cliente.Domicilio.Provincia.NombreProvincia;
                txtDNI.Text = cliente.DNI;
                txtCP.Text = cliente.Domicilio.CodigoPostal;
                txtTelefono.Text = cliente.TelefonoMovil;
            }
            else
            {
                cliente = null;
                txtDireccion.Clear();
                txtLocalidad.Clear();
                txtProvincia.Clear();
                txtDNI.Clear();
                txtCP.Clear();
                txtTelefono.Clear();
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
           
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            frmProtesisAE frm = new frmProtesisAE();
            DialogResult dr = frm.ShowDialog(this);
            if (dr == DialogResult.OK)
            {
                Protesis prot = frm.GetProtesis();
                ProtesisBd.Agregar(prot);
            }
            ProtesisBd.CargarCombo(ref cboProtesis);
        }

        private void cboProtesis_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboProtesis.SelectedIndex > 0)
            {
                comboBox1.Enabled = true;
                protesis = (Protesis)cboProtesis.SelectedItem;
                txtCosto.Text = protesis.Importe.ToString();
                txtDescripcion.Text = protesis.Descripcion;
            }
        }

        private void chkObra_CheckedChanged(object sender, EventArgs e)
        {
            txtObra.Enabled = !txtObra.Enabled;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            frmBuscarClientes frm = new frmBuscarClientes();
            DialogResult dr = frm.ShowDialog(this);
            if (dr == DialogResult.OK)
            {
                Cliente cliente = frm.GetCliente();
                cboCliente.SelectedValue = cliente.ClienteId;

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            frmBuscarProductos frm = new frmBuscarProductos();
            frm.SetProtesis(ProtesisBd.GetLista());
            DialogResult dr = frm.ShowDialog(this);
            if (dr == DialogResult.OK)
            {
                Protesis protesis= frm.GetProtesis();
                cboProtesis.SelectedValue = protesis.ProtesisId;

            }
        }
    }
}

