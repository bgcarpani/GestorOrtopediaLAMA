using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LAMAModels;
using LAMADatabase;

namespace LAMAForms
{
    public partial class frmClientesAE : Form
    {
        public frmClientesAE()
        {
            InitializeComponent();
        }

        Cliente cliente;
        private bool verDetalle = false;
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            ProvinciasBd.CargarCombo(ref cboProvincia);

            if (cliente != null)
            {
                txtApellido.Text = cliente.Apellido;
                txtNombres.Text = cliente.Nombre;
                txtDNI.Text = cliente.DNI;
                txtCalle.Text = cliente.Domicilio.Direccion;       
                txtCodigoPostal.Text = cliente.Domicilio.CodigoPostal;
                txtTelefonoMovil.Text = cliente.TelefonoMovil;
                txtEmail.Text = cliente.CorreoElectronico;
                txtObservaciones.Text = cliente.Observaciones;
                cboProvincia.SelectedIndex = cliente.Domicilio.Provincia.ProvinciaId;
                LocalidadesBd.CargarCombo(ref cboLocalidad, cliente.Domicilio.Provincia);
                if (verDetalle)
                {

                    Font miFuente = new Font("Times New Roman", 10.0f, FontStyle.Bold);
                    foreach (Control control in this.Controls)
                    {

                        if (control is TextBox)
                        {
                            ((TextBox)control).ReadOnly = true;
                            ((TextBox)control).Font = miFuente;
                        }
                        else if (control is ComboBox)
                        {
                            ((ComboBox)control).Enabled = false;
                            ((ComboBox)control).Font = miFuente;
                        }
                        else if (control is GroupBox)
                        {
                            foreach (Control control2 in control.Controls)
                            {
                                if (control2 is TextBox)
                                {
                                    ((TextBox)control2).ReadOnly = true;
                                    ((TextBox)control2).Font = miFuente;
                                }
                                else if (control2 is ComboBox)
                                {
                                    ((ComboBox)control2).Enabled = false;
                                    ((ComboBox)control2).Font = miFuente;
                                }
                                else if (control2 is Button)
                                {
                                    ((Button)control2).Enabled = false;
                                }
                            }
                        }
                        else if (control is Button)
                        {
                            ((Button)control).Enabled = false;
                        }
                    }
                }
            }
        }



        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (ValidarDatos())
            {
                if (cliente == null)
                {
                    cliente = new Cliente();
                }
                cliente.Apellido = txtApellido.Text;
                cliente.Nombre = txtNombres.Text;
                cliente.DNI = txtDNI.Text;
                cliente.Domicilio = new Domicilio
                {
                    Direccion = ConvertirEmpty(txtCalle.Text),
                    Localidad = (Localidad)cboLocalidad.SelectedItem,
                    Provincia = (Provincia)cboProvincia.SelectedItem,
                    CodigoPostal = ConvertirEmpty(txtCodigoPostal.Text)
                };
                cliente.TelefonoMovil = ConvertirEmpty(txtTelefonoMovil.Text);
                cliente.CorreoElectronico = ConvertirEmpty(txtEmail.Text);
                cliente.Observaciones = ConvertirEmpty(txtObservaciones.Text);

                if (!editar)
                {
                    try
                    {
                        int nuevoId = ClientesBd.AgregarConReturn(cliente);
                        MessageBox.Show("Operación exitosa", "Mensaje", MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                        DialogResult dr = MessageBox.Show("¿Desea agregar otro cliente?", "Continuar",
                            MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                        if (dr == DialogResult.Yes)
                        {
                            InicializarControles();
                        }
                        else
                        {
                            this.DialogResult = DialogResult.OK;
                        }
                    }
                    catch (Exception ex)
                    {
                        string errorMessage = "";
                        if (ex.Message.ToUpper().Contains("PK_CLIENTE"))
                        {
                            errorMessage = "Ya existe un cliente con el DNI ingresado.";
                        }
                        else
                        {
                            errorMessage = ex.Message;
                        }
                        MessageBox.Show(errorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    this.DialogResult = DialogResult.OK;
                }

            }
        }

        private string ConvertirEmpty(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return " ";
            }
            else
            {
                return text;
            }
        }

        private void InicializarControles()
        {
            txtApellido.Text = string.Empty;
            txtNombres.Text = string.Empty;
            txtDNI.Text = string.Empty;
            txtCalle.Text = string.Empty;  
            cboLocalidad.SelectedIndex = 0;
            cboProvincia.SelectedIndex = 0;
            txtCodigoPostal.Text = string.Empty;
            txtTelefonoMovil.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtObservaciones.Text = string.Empty;
            txtApellido.Focus();
        }
        bool editar = false;
        internal void SetEditar(bool p)
        {
            editar = p;
        }

        private bool ValidarDatos()
        {
            bool valido = true;
            errorProvider1.Clear();
            if (string.IsNullOrEmpty(txtApellido.Text))
            {
                valido = false;
                errorProvider1.SetError(txtApellido, "Debe ingresar un apellido");
            }
            if (string.IsNullOrEmpty(txtNombres.Text))
            {
                valido = false;
                errorProvider1.SetError(txtNombres, "Debe ingresar un nombre");
            }
            if (string.IsNullOrEmpty(txtDNI.Text))
            {
                valido = false;
                errorProvider1.SetError(txtDNI, "Debe ingresar un DNI");
            }

            if (cboLocalidad.SelectedIndex == 0)
            {
                valido = false;
                errorProvider1.SetError(cboLocalidad, "Debe seleccionar una Localidad");
            }
            if (cboProvincia.SelectedIndex == 0)
            {
                valido = false;
                errorProvider1.SetError(cboProvincia, "Debe seleccionar una Provincia");
            }

            return valido;
        }


        public void SetCliente(Cliente cliente)
        {
            this.cliente = cliente;
        }


        public Cliente GetCliente()
        {
            return cliente;
        }

        private void cboProvincia_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboProvincia.SelectedIndex > 0)
            {
                Provincia provincia = (Provincia)cboProvincia.SelectedItem;
                LocalidadesBd.CargarCombo(ref cboLocalidad, provincia);
                cboLocalidad.Enabled = true;
            }
            else
            {
                cboLocalidad.DataSource = null;
                cboLocalidad.Enabled = true;
            }
        }

        public void SetDetalle(bool b)
        {
            verDetalle = b;
        }

        private void btnAgregarLocalidad_Click(object sender, EventArgs e)
        {
            frmLocalidadesAE frm = new frmLocalidadesAE();
            frm.Text = "Agregar Localidad";
            frm.ShowDialog(this);
            ProvinciasBd.CargarCombo(ref cboProvincia);
        }

        private void frmClientesAE_Load(object sender, EventArgs e)
        {

        }
    }
}
