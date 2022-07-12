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
    public partial class frmLocalidadesAE : Form
    {
        public frmLocalidadesAE()
        {
            InitializeComponent();
        }

        private Localidad localidad;
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            ProvinciasBd.CargarCombo(ref cboProvincia);
            if (localidad != null)
            {
                cboProvincia.SelectedValue = localidad.Provincia.ProvinciaId;
                txtLocalidad.Text = localidad.Descripcion;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (ValidarDatos())
            {
                if (localidad == null)
                {
                    localidad = new Localidad();
                }
                localidad.Descripcion = txtLocalidad.Text;
                localidad.Provincia = (Provincia)cboProvincia.SelectedItem;
                this.DialogResult = DialogResult.OK;
            }
        }

        private bool ValidarDatos()
        {
            bool valido = true;
            errorProvider1.Clear();
            if (cboProvincia.SelectedIndex == 0)
            {
                valido = false;
                errorProvider1.SetError(cboProvincia, "Debe seleccionar una provincia");
            }
            if (string.IsNullOrEmpty(txtLocalidad.Text))
            {
                valido = false;
                errorProvider1.SetError(txtLocalidad, "Debe ingresar una localidad");
            }
            return valido;
        }

        public Localidad GetLocalidad()
        {
            return localidad;
        }

        //private void btnAgregarProvincia_Click(object sender, EventArgs e)
        //{
        //    frmProvinciasAE frm = new frmProvinciasAE();
        //    frm.Text = "Agregar Provincia";
        //    frm.ShowDialog(this);
        //    ProvinciasBd.CargarCombo(ref cboProvincia);
        //}

        public void SetLocalidad(Localidad localidad1)
        {
            localidad = localidad1;
        }
    }
}
