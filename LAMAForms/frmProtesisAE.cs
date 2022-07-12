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
    public partial class frmProtesisAE : Form
    {
        public frmProtesisAE()
        {
            InitializeComponent();
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (protesis != null)
            {
                textBox1.Text = protesis.Tipo;
                txtDescripcion.Text = protesis.Descripcion;
                txtCosto.Text = protesis.Importe.ToString();
            }
        }
        private Protesis protesis;
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (ValidarDatos())
            {
                if (protesis == null)
                {
                    protesis = new Protesis();
                }
                protesis.Tipo = textBox1.Text;
                protesis.Descripcion = txtDescripcion.Text;
                protesis.Importe = decimal.Parse(txtCosto.Text);
                DialogResult = DialogResult.OK;
            }
        }

        private bool ValidarDatos()
        {
            errorProvider1.Clear();
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                errorProvider1.SetError(textBox1, "El campo no puede estar vacío.");
                return false;
            }
            if (string.IsNullOrEmpty(txtDescripcion.Text))
            {
                errorProvider1.SetError(txtDescripcion, "El campo no puede estar vacío.");
                return false;
            }
            if (!decimal.TryParse(txtCosto.Text, out decimal result))
            {
                errorProvider1.SetError(txtDescripcion, "Caracter inválido.");
                return false;
            }

            return true;
        }

        internal Protesis GetProtesis()
        {
            return this.protesis;
        }

        internal void SetProtesis(Protesis prot)
        {
            this.protesis = prot;
        }
    }
}
