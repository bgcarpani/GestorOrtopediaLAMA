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
    public partial class frmProveedoresAE : Form
    {
        public frmProveedoresAE()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e); 
            ProvinciasBd.CargarCombo(ref cboProvincia);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Validar())
            {
                if (proveedor == null)
                {
                    proveedor = new Proveedor();
                }
                proveedor.RazonSocial = txtRzon.Text;
                proveedor.Localidad = (Localidad)cboLocalidad.SelectedItem;
                if (!string.IsNullOrEmpty(txtTelefono.Text))
                    proveedor.Telefono = txtTelefono.Text;
                else
                    proveedor.Telefono = "";
                if (!string.IsNullOrEmpty(txtDireccion.Text))
                    proveedor.Direccion = txtDireccion.Text;
                else
                    proveedor.Direccion = "";
                if (!string.IsNullOrEmpty(txtEmail.Text))
                    proveedor.Email = txtEmail.Text;
                else
                    proveedor.Email = "";
                if (!string.IsNullOrEmpty(txtSitio.Text))
                    proveedor.Web = txtSitio.Text;
                else
                    proveedor.Web= "";

                ProveedoresBd.Agregar(proveedor);

                DialogResult = DialogResult.OK;
            }
        }

        private bool Validar()
        {
            errorProvider1.Clear();
            if (string.IsNullOrEmpty(txtRzon.Text))
            {
                errorProvider1.SetError(txtRzon, "Debe ingresar una razón social.");
                return false;
            }
            if (cboLocalidad.SelectedIndex == 0)
            {
                errorProvider1.SetError(cboLocalidad, "Debe seleccionar una Localidad");
                return false;

            }
            if (cboProvincia.SelectedIndex == 0)
            {
                errorProvider1.SetError(cboProvincia, "Debe seleccionar una Provincia");
                return false;
            }

            return true;
        }

        private Proveedor proveedor;

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

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
