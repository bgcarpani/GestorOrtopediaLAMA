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
using System.Threading;
using System.Configuration;

namespace LAMAForms
{
    public partial class frmClientes : Form
    {
        public frmClientes()
        {
            InitializeComponent();
        }


        private static frmClientes frm = null;
        public static frmClientes GetInstancia()
        {
            if (frm == null)
            {
                frm = new frmClientes();
                frm.FormClosed += new FormClosedEventHandler(form_FormClosed);
            }
            return frm;
        }

        private static void form_FormClosed(object sender, FormClosedEventArgs e)
        {
            frm = null;
        }

        private void tsbCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void tsbNuevo_Click(object sender, EventArgs e)
        {
            Nuevo();
        }

        private void Nuevo()
        {
            frmClientesAE frm = new frmClientesAE();
            frm.Text = "Agregar Cliente";
            DialogResult dr = frm.ShowDialog();
            if (dr == DialogResult.OK)
            {
                ActualizarGrilla();
            }
        }


        private void ActualizarGrilla()
        {
            try
            {
                lista = ClientesBd.GetLista();
                lista = lista.Where(c => c.Eliminado == false).ToList();
                lista = lista.Where(c => c.ClienteId != int.Parse(ConfigurationManager.ConnectionStrings["ConsumidorFinal"].ToString())).ToList();
                MostrarDatosGrilla(lista);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void VerDetallesMenuItem_Click(object sender, EventArgs e)
        {
            Detalles();
        }

        private void EditarMenuItem_Click(object sender, EventArgs e)
        {
            Editar();
        }

        private void BorrarMenuItem_Click(object sender, EventArgs e)
        {
            Borrar();
        }

        private void AgregarMenuItem_Click(object sender, EventArgs e)
        {
            Nuevo();
        }

        private List<Cliente> lista; 
        private void frmClientes_Load(object sender, EventArgs e)
        {

           this.Dock = DockStyle.Fill;
            ActualizarGrilla();
        }

        



        private void MostrarDatosGrilla(List<Cliente> lista)
        {
            dgvDatos.Rows.Clear();
            foreach (Cliente item in lista)
            {
                DataGridViewRow r = new DataGridViewRow();
                r.CreateCells(dgvDatos);
                SetearFila(r, item);
                AgregarFila(r);
            }
        }

        private void SetearFila(DataGridViewRow r, Cliente item)
        {
            r.Cells[cmnApellido.Index].Value = item.Apellido;
            r.Cells[cmnNombres.Index].Value = item.Nombre;
            r.Cells[cmnDNI.Index].Value = item.DNI;
            r.Cells[cmnDireccion.Index].Value = item.Domicilio.Direccion == string.Empty ? string.Empty : item.Domicilio.Direccion;
            r.Cells[cmnLocalidad.Index].Value = item.Domicilio.Localidad.Descripcion;
            r.Cells[cmnProvincia.Index].Value = item.Domicilio.Provincia.NombreProvincia;
            r.Cells[cmnTelMovil.Index].Value = item.TelefonoMovil == string.Empty ? string.Empty : item.TelefonoMovil;

            r.Tag = item;
        }

        private void AgregarFila(DataGridViewRow r)
        {
            dgvDatos.Rows.Add(r);
        }

        private void tsbBorrar_Click(object sender, EventArgs e)
        {
            Borrar();
        }

        private void Borrar()
        {
            if (dgvDatos.SelectedRows.Count > 0)
            {
                DataGridViewRow r = dgvDatos.SelectedRows[0];
                Cliente cliente = (Cliente) r.Tag;
                DialogResult dr = MessageBox.Show($"¿Desea borrar al Cliente {cliente.Apellido} {cliente.Nombre}?",
                    "Confirmar Borrado", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (dr == DialogResult.Yes)
                {
                    try
                    {
                        ClientesBd.Borrar(cliente);
                        dgvDatos.Rows.Remove(r);
                        lista.Remove(cliente);

                        MessageBox.Show("Operación exitosa", "Mensaje",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    }
                }
            }
        }


        private void tsbEditar_Click(object sender, EventArgs e)
        {
            Editar();
        }

        private void Editar()
        {
            if (dgvDatos.SelectedRows.Count > 0)
            {
                int posicion = dgvDatos.SelectedRows[0].Index;
                DataGridViewRow row = dgvDatos.SelectedRows[0];
                Cliente cliente = (Cliente) row.Tag;
                Cliente clienteAux = (Cliente) cliente.Clone();
                frmClientesAE frm = new frmClientesAE();
                frm.Text = "Editar Cliente";
                frm.SetCliente(cliente);
                frm.SetEditar(true);
                DialogResult dr = frm.ShowDialog(this);
                if (dr == DialogResult.OK)
                {
                    try
                    {
                        cliente = frm.GetCliente();
                        ClientesBd.Editar(cliente);
                        SetearFila(row, cliente);
                        MessageBox.Show("Operación exitosa", "Mensaje",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Exclamation);
                        if (ex.Message.Contains("repetido"))
                        {
                            SetearFila(row, clienteAux);
                            lista.Remove(cliente);
                            lista.Insert(posicion, clienteAux);
                        }
                        else
                        {
                            var clienteInBd = ClientesBd.GetObjeto(cliente.ClienteId);
                            if (clienteInBd != null)
                            {
                                SetearFila(row, clienteInBd);
                                lista.Remove(cliente);
                                lista.Insert(posicion, clienteInBd);
                            }
                            else
                            {
                                dgvDatos.Rows.Remove(row);
                                lista.Remove(cliente);
                            }
                        }
                    }
                }
            }
        }


        private void tsbBuscar_Click(object sender, EventArgs e)
        {
            
        }

        private void tsbVerDetalle_Click(object sender, EventArgs e)
        {
            Detalles();
        }

        private void Detalles()
        {
            /*Pasa el objeto al formulario para mostrarlo
             * se deben deshabilitar la posibilidad de modificar
             * */
            DataGridViewRow row = dgvDatos.SelectedRows[0];
            Cliente cliente = (Cliente) row.Tag;
            frmClientesAE frm = new frmClientesAE();
            frm.Text = "Detalle Cliente";
            //Se setea para que el formulario sepa que va a mostrar datos
            //sin permitir la edición
            frm.SetCliente(cliente);
            frm.SetDetalle(true);
            frm.ShowDialog(this);
        }

        private void dgvDatos_MouseClick(object sender, MouseEventArgs e)
        {
            //Detectamos si se pulsó el boton derecho del mouse
            if (e.Button == MouseButtons.Right)
            {
                //Vemos si se hizo clic sobre una celda
                DataGridView.HitTestInfo hitTest = dgvDatos.HitTest(e.X, e.Y);
                //Si fue así posicionamos el cursor y mostramos el menú contextual
                if (hitTest.Type == DataGridViewHitTestType.Cell)
                {
                    dgvDatos.CurrentCell = dgvDatos.Rows[hitTest.RowIndex].Cells[hitTest.ColumnIndex];
                    cm.Show(dgvDatos, dgvDatos.PointToClient(Cursor.Position));

                }
            }
        }

        private void nuevoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Nuevo();
        }

        private void borrarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Borrar();
        }

        private void editarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Editar();
        }

        private void detallesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Detalles();
        }

        private void tsbActualizar_Click(object sender, EventArgs e)
        {
            ActualizarGrilla();
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void toolStripTextBox1_Click(object sender, EventArgs e)
        {

        }
        private void toolStripTextBox1_TextChanged(object sender, EventArgs e)
        {
            string valor = toolStripTextBox1.Text;
            List<Cliente> listaProvisoria = null;
            try
            {
                if (!int.TryParse(valor, out int dni))
                {
                    listaProvisoria = lista.Where(l => l.ToString().ToUpper().Contains(valor.ToUpper())).ToList();
                }
                else
                {
                    listaProvisoria = lista.Where(l => l.DNI.Contains(dni.ToString())).ToList();
                }
                MostrarDatosGrilla(listaProvisoria);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }
    }
}
