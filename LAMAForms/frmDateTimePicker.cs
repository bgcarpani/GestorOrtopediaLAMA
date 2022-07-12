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
    public partial class frmDateTimePicker : Form
    {
        public frmDateTimePicker()
        {
            InitializeComponent();
        }

        private DateTime fechaDesde;
        private DateTime fechaHasta;
        private void button1_Click(object sender, EventArgs e)
        {
            if (Validar())
            {
                fechaDesde = dtpDesde.Value;
                fechaHasta = dtpHasta.Value;
                DialogResult = DialogResult.OK;
            }
        }

        private bool Validar()
        {
            errorProvider1.Clear();
            if (dtpDesde.Value > dtpHasta.Value)
            {
                errorProvider1.SetError(dtpHasta, "Fecha inválida.");
                return false;
            }
            if (dtpDesde.Value < DateTime.Now)
            {
                errorProvider1.SetError(dtpHasta, "Fecha inválida.");
                return false;
            }
            if (dtpHasta.Value < DateTime.Now)
            {
                errorProvider1.SetError(dtpHasta, "Fecha inválida.");
                return false;
            }
            if (dtpHasta.Value.ToShortDateString() == dtpDesde.Value.ToShortDateString())
            {
                errorProvider1.SetError(dtpHasta, "No puedes realizar un alquiler por menos de un día");
                return false;
            }
            if (dtpHasta.Value.ToShortDateString() == DateTime.Now.ToShortDateString())
            {
                errorProvider1.SetError(dtpHasta, "No puedes realizar un alquiler por menos de un día");
                return false;
            }
            if (dtpHasta.Value.ToShortDateString() == dtpDesde.Value.ToShortDateString())
            {
                errorProvider1.SetError(dtpHasta, "No puedes realizar un alquiler por menos de un día");
                return false;
            }
            return true;
        }

        internal DateTime GetFechaDesde()
        {
            return this.fechaDesde;
        }

        internal DateTime GetFechaHasta()
        {
            return this.fechaHasta;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
