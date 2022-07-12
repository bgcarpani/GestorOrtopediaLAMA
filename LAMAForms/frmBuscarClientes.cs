using LAMADatabase;
using LAMAModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LAMAForms
{
    public partial class frmBuscarClientes : Form
    {
        public frmBuscarClientes()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            btnOk.Enabled = false;
        }

        private List<Cliente> clientes = ClientesBd.GetLista();

        private void btnOk_Click(object sender, EventArgs e)
        {
            DataGridViewRow row = dgPedido.SelectedRows[0];
            Cliente cli = (Cliente)row.Tag;
            cliente = ClientesBd.GetObjeto(cli.ClienteId);
            DialogResult = DialogResult.OK;
        }

 

        private void MostrarDatosGrilla(List<Cliente> lista)
        {
            dgPedido.Rows.Clear();
            foreach (Cliente item in lista)
            {
                DataGridViewRow r = new DataGridViewRow();
                r.CreateCells(dgPedido);
                SetearFila(r, item);
                AgregarFila(r);
            }
        }

        private void AgregarFila(DataGridViewRow r)
        {
            dgPedido.Rows.Add(r);
        }

        private void SetearFila(DataGridViewRow r, Cliente item)
        {
            r.Cells[cmnId.Index].Value = item.ClienteId;
            r.Cells[cmnNombre.Index].Value = item.NombreCompleto;
            r.Cells[cmnDNI.Index].Value = item.DNI;
        
            r.Tag = item;
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string value = textBox1.Text;
            clientes = clientes.OrderBy(c => c.NombreCompleto).ToList();
            List<Cliente> listaProvisoria = null;
            if (int.TryParse(textBox1.Text, out int dni))
            {
                listaProvisoria = clientes.Where(c => c.DNI.ToUpper().Contains(dni.ToString())).ToList();
                listaProvisoria = listaProvisoria.Where(c => c.ClienteId != int.Parse(ConfigurationManager.ConnectionStrings["ConsumidorFinal"].ToString())).ToList();
                listaProvisoria = listaProvisoria.Where(c => c.Eliminado == false).ToList();
            }
            else
            {
                listaProvisoria = clientes.Where(c => c.NombreCompleto.ToUpper().Contains(value.ToUpper())).ToList();
                listaProvisoria = listaProvisoria.Where(c => c.ClienteId != int.Parse(ConfigurationManager.ConnectionStrings["ConsumidorFinal"].ToString())).ToList();
                listaProvisoria = listaProvisoria.Where(c => c.Eliminado == false).ToList();
            }
            if (listaProvisoria.Any())
            {
                btnOk.Enabled = true;
            }
            else
            {
                btnOk.Enabled = false;
            }
            MostrarDatosGrilla(listaProvisoria);
        }
        private Cliente cliente;
        private void button2_Click(object sender, EventArgs e)
        {
            cliente = null;
            DialogResult = DialogResult.Cancel;
        }

        internal Cliente GetCliente()
        {
            return cliente;
        }
    }
}
