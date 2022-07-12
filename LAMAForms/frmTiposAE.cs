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
    public partial class frmTiposAE : Form
    {
        public frmTiposAE()
        {
            InitializeComponent();
        }

        private Tipo tipo;
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (ValidarDatos())
            {
                if (tipo == null)
                {
                    tipo = new Tipo();
                }
                tipo.Descripcion = txtMarca.Text;
                try
                {
                    TiposBd.Agregar(tipo);
                    MessageBox.Show("Registro agregado", "Mensaje",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult dr = MessageBox.Show("Desea agregar otro registro?", "Mensaje",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                    if (dr == DialogResult.Yes)
                    {
                        tipo = null;
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
                errorProvider1.SetError(txtMarca, "Debe ingresar un tipo de producto.");
            }
            return valido;
        }
    }
}
