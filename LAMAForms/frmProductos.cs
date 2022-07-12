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
    public partial class frmProductos : Form
    {
        public frmProductos()
        {
            InitializeComponent();
        }
        private static frmProductos frm = null;
        public static frmProductos GetInstancia()
        {
            if (frm == null)
            {
                frm = new frmProductos();
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
            frmProductosAE frm = new frmProductosAE();
            frm.Text = "Agregar Producto";
            DialogResult dr = frm.ShowDialog(this);
            if (dr == DialogResult.OK)
            {
                ActualizarGrilla();
            }
        }

        private List<Producto> lista;
        private void ActualizarGrilla()
        {
            try
            {
                lista = ProductosBd.GetLista();
                MostrarDatosGrilla(lista);

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private ContextMenuStrip cm;
        private void frmProductos_Load(object sender, EventArgs e)
        {
            this.Dock = DockStyle.Fill;
            ActualizarGrilla();
            ArmarMenuContextual();
        }

        private void ArmarMenuContextual()
        {
            cm = LAMAHelp1.Help.CrearMenuContextual();
            foreach (ToolStripMenuItem item in cm.Items)
            {
                switch (item.Text)
                {
                    case "Nuevo":
                        item.Click += AgregarMenuItem_Click;
                        break;
                    case "Borrar":
                        item.Click += BorrarMenuItem_Click;
                        break;
                    case "Editar":
                        item.Click += EditarMenuItem_Click;
                        break;
                    case "Ver Detalles":
                        item.Click += VerDetallesMenuItem_Click;
                        break;
                }
            }
        }

        private void VerDetallesMenuItem_Click(object sender, EventArgs e)
        {
            VerDetalle();
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

        private void MostrarDatosGrilla(List<Producto> lista)
        {
            dgvDatos.Rows.Clear();
            foreach (Producto item in lista)
            {
                DataGridViewRow r = new DataGridViewRow();
                r.CreateCells(dgvDatos);
                SetearFila(r, item);
                AgregarFila(r);
            }
        }

        private List<ProductoStock> listaPS = ProductoStocksBd.GetListaCompleta();

        private void SetearFila(DataGridViewRow r, Producto item)
        {
            r.Cells[cmnDescripcion.Index].Value = item.Descripcion;
            r.Cells[cmnMarca.Index].Value = item.Marca.Descripcion;
            r.Cells[cmnTipo.Index].Value = item.Tipo.Descripcion;
            r.Cells[cmnPrecioCosto.Index].Value = item.Precio.ToString("C");
            r.Cells[cmnAlquiler.Index].Value = item.PrecioAlquiler.ToString("C");
            r.Cells[cmnQuincena.Index].Value = item.PrecioAlquilerQuincena.ToString("C");
            ProductoStock psVen = listaPS.Where(p => p.Producto.ProductoId == item.ProductoId && p.Stock.StockId == 1).FirstOrDefault();
            if (psVen != null)
            {
                if (psVen.StockDisponible <= 5)
                {
                    r.Cells[cmnStockVen.Index].Style.BackColor = Color.LightYellow;
                }
                if (psVen.StockDisponible <= 0)
                {
                    r.Cells[cmnStockVen.Index].Style.BackColor = Color.Red;
                }
                r.Cells[cmnStockVen.Index].Value = psVen.StockDisponible;
            }
            else
            {
                r.Cells[cmnStockVen.Index].Value = "0";
                r.Cells[cmnStockVen.Index].Style.BackColor = Color.Red;

            }
            ProductoStock psAlq = listaPS.Where(p => p.Producto.ProductoId == item.ProductoId && p.Stock.StockId == 2).FirstOrDefault();
            if (psAlq != null)
            {
                r.Cells[cmnStockAlq.Index].Value = psAlq.StockDisponible;
                if (psAlq.StockDisponible <= 5)
                {
                    r.Cells[cmnStockAlq.Index].Style.BackColor = Color.LightYellow;
                }
                if (psAlq.StockDisponible <= 0)
                {
                    r.Cells[cmnStockAlq.Index].Style.BackColor = Color.Red;
                }
            }
            else
            {
                r.Cells[cmnStockAlq.Index].Value = "0";
                r.Cells[cmnStockAlq.Index].Style.BackColor = Color.Red;

            }
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
                Producto producto = (Producto)r.Tag;
                DialogResult dr = MessageBox.Show($"¿Desea borrar al Producto {producto.Descripcion}?",
                    "Confirmar Borrado", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (dr == DialogResult.Yes)
                {
                    try
                    {
                        ProductosBd.Borrar(producto);
                        dgvDatos.Rows.Remove(r);
                        MessageBox.Show("Producto borrado", "Mensaje",
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
                Producto producto = (Producto)row.Tag;
                Producto productoAux = (Producto)producto.Clone();
                frmProductosAE frm = new frmProductosAE();
                frm.Text = "Editar Producto";
                frm.SetProducto(producto);
                frm.SetEditar(true);
                DialogResult dr = frm.ShowDialog(this);
                if (dr == DialogResult.OK)
                {
                    try
                    {
                        producto = frm.GetProducto();
                        ProductosBd.Editar(producto);
                        SetearFila(row, producto);
                        MessageBox.Show("Producto editado", "Mensaje",
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
                            SetearFila(row, productoAux);
                            lista.Remove(producto);
                            lista.Insert(posicion, productoAux);

                        }
                        else
                        {
                            var productoInBd = ProductosBd.GetObjeto(producto.ProductoId);
                            if (productoInBd != null)
                            {
                                SetearFila(row, productoInBd);
                                lista.Remove(producto);
                                lista.Insert(posicion, productoInBd);
                            }
                            else
                            {
                                dgvDatos.Rows.Remove(row);
                                lista.Remove(producto);

                            }
                        }
                    }
                }
            }
        }

        private void VerDetalle()
        {
            DataGridViewRow row = dgvDatos.SelectedRows[0];
            Producto producto = (Producto)row.Tag;
            frmProductosAE frm = new frmProductosAE();
            frm.Text = "Detalle Producto";
            frm.SetProducto(producto);
            //frm.SetDetalle(true);
            frm.ShowDialog(this);
        }


        private void dgvDatos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                DataGridViewCheckBoxCell chkSeleccionar =
                    (DataGridViewCheckBoxCell)dgvDatos.Rows[e.RowIndex].Cells[cmdSeleccion.Index];
                chkSeleccionar.Value = !Convert.ToBoolean(chkSeleccionar.Value);
            }
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void tsbSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void tsbNuevo_Click_1(object sender, EventArgs e)
        {
            Nuevo();
        }

        private void tsbBorrar_Click_1(object sender, EventArgs e)
        {
            if (dgvDatos.SelectedRows.Count > 0)
            {
                DataGridViewRow r = dgvDatos.SelectedRows[0];
                Producto prod = (Producto)r.Tag;
                DialogResult dr = MessageBox.Show($"¿Desea borrar {prod.Marca.Descripcion} {prod.Descripcion}?",
                    "Confirmar Borrado", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (dr == DialogResult.Yes)
                {
                    try
                    {
                        ProductosBd.Borrar(prod);
                        ProductoStocksBd.EliminarPorProducto(prod);
                        dgvDatos.Rows.Remove(r);
                        lista.Remove(prod);
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

        private void tsbEditar_Click_1(object sender, EventArgs e)
        {
            if (dgvDatos.SelectedRows.Count > 0)
            {
                int posicion = dgvDatos.SelectedRows[0].Index;
                DataGridViewRow row = dgvDatos.SelectedRows[0];
                Producto prod = (Producto)row.Tag;
                Producto prodAux = (Producto)prod.Clone();
                frmProductosAE frm = new frmProductosAE();
                frm.Text = "Editar Producto";
                frm.SetProducto(prod);
                frm.SetEditar(true);
                DialogResult dr = frm.ShowDialog(this);
                if (dr == DialogResult.OK)
                {
                    try
                    {
                        prod = frm.GetProducto();
                        ProductosBd.Editar(prod);
                        SetearFila(row, prod);
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
                            SetearFila(row, prodAux);
                            lista.Remove(prod);
                            lista.Insert(posicion, prodAux);
                        }
                        else
                        {
                            var prodInBd = ProductosBd.GetObjeto(prod.ProductoId);
                            if (prodInBd != null)
                            {
                                SetearFila(row, prodInBd);
                                lista.Remove(prod);
                                lista.Insert(posicion, prodInBd);
                            }
                            else
                            {
                                dgvDatos.Rows.Remove(row);
                                lista.Remove(prod);
                            }
                        }
                    }
                }
            }
        }

        private void toolStripTextBox1_TextChanged(object sender, EventArgs e)
        {
            string valor = toolStripTextBox1.Text;
            List<Producto> listaProvisoria = null;
            try
            {

                listaProvisoria = lista.Where(l => l.ToString().ToUpper().Contains(valor.ToUpper())).ToList();

                MostrarDatosGrilla(listaProvisoria);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        private void tsbActualizar_Click(object sender, EventArgs e)
        {
            ActualizarGrilla();
        }

        private void toolStrip1_ItemClicked_1(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            frmAumentosProductos frm = new frmAumentosProductos();
            DialogResult dr = frm.ShowDialog(this);
            if (dr == DialogResult.OK)
            {
                Aumento aumento = frm.GetAumento();
                ProductosBd.GenerarAumento(aumento);
                MessageBox.Show("Aumento realizado con éxito.", "Mensaje",
                          MessageBoxButtons.OK, MessageBoxIcon.Information);
                ActualizarGrilla();
            }
        }
    }
}
