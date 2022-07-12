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
    public partial class frmMarcasAE : Form
    {
        private Marca marca;
        public frmMarcasAE()
        {
            InitializeComponent();
        }


        public Marca GetMarca()
        {
            return marca;
        }

        private bool editar = false;
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (ValidarDatos())
            {
                if (marca == null)
                {
                    marca = new Marca();
                }
                marca.Descripcion = txtMarca.Text;
                if (!editar)
                {
                    try
                    {
                        MarcasBd.Agregar(marca);
                        MessageBox.Show("Registro agregado", "Mensaje",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        DialogResult dr = MessageBox.Show("Desea agregar otro registro?", "Mensaje",
                            MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                        if (dr == DialogResult.Yes)
                        {
                            marca = null;
                            InicializarControles();
                        }
                        else
                        {
                            this.Close();
                        }

                    }
                    catch (Exception ex)
                    {

                        errorProvider1.SetError(txtMarca, "Tipo existente");
                    }
                }
                else
                {
                    this.DialogResult = DialogResult.OK;

                }
            }
        }

        private void InicializarControles()
        {
            txtMarca.Clear();
            txtMarca.Focus();
        }

        private bool ValidarDatos()
        {
            bool valido = true;
            errorProvider1.Clear();
            if (string.IsNullOrEmpty(txtMarca.Text))
            {
                valido = false;
                errorProvider1.SetError(txtMarca, "Debe ingresar una marca");
            }
            return valido;
        }

        public void SetEditar(bool editar)
        {
            this.editar = editar;
        }
        public void SetMarca(Marca marca)
        {
            this.marca = marca;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (marca != null)
            {
                txtMarca.Text = marca.Descripcion;
            }
        }

        private void btnCancelar_Click_1(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
