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
    public partial class frmAumentosProductos : Form
    {
        public frmAumentosProductos()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            TiposBd.CargarCombo(ref comboBox1);
            textBox1.Text = "5";
            chkVenta.Checked = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private Aumento aumento;
        private void button1_Click_1(object sender, EventArgs e)
        {
            if (ValidarDatos())
            {
                if (aumento == null)
                {
                    aumento = new Aumento();
                }
                aumento.tipo = (Tipo)comboBox1.SelectedItem;
                aumento.porcentaje = decimal.Parse(textBox1.Text);
                if (chkVenta.Checked)
                {
                    aumento.precioVenta = true;
                }
                if (checkBox2.Checked)
                {
                    aumento.precioAlquiler = true;
                }
                DialogResult = DialogResult.OK;
            }
        }

        private bool ValidarDatos()
        {
            errorProvider1.Clear();
            if (comboBox1.SelectedIndex == 0)
            {
                errorProvider1.SetError(comboBox1, "Debe seleccionar un tipo de producto.");
                return false;
            }
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                errorProvider1.SetError(textBox1, "No puede enviar este campo vacío.");
                return false;
            }
            if (!int.TryParse(textBox1.Text, out int result))
            {
                errorProvider1.SetError(textBox1, "Caracter inválido.");
                return false;
            }
            if (checkBox2.Checked == false && chkVenta.Checked == false)
            {
                errorProvider1.SetError(chkVenta, "Debe seleccionar al menos un precio.");
                return false;
            }
            return true;
        }

        internal Aumento GetAumento()
        {
            return this.aumento;
        }
    }
}
