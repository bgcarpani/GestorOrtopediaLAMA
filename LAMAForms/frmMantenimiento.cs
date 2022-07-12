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
    public partial class frmMantenimiento : Form
    {
        public frmMantenimiento()
        {
            InitializeComponent();
        }

        public static frmMantenimiento frm = null;
        public static frmMantenimiento GetInstancia()
        {
            if (frm == null)
            {
                frm = new frmMantenimiento();
                frm.FormClosed += frm_FormClosed;
            }
            return frm;
        }

        private static void frm_FormClosed(object sender, FormClosedEventArgs e)
        {
            frm = null;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            chk3anios.Checked = true;
        }

        public int tiempoEliminacion { get; set; }

        private DateTime GetFechaEliminacion(int tiempoEliminacion)
        {
            switch (tiempoEliminacion)
            {
                case 0:
                    return DateTime.Now.AddYears(-3);
                    break;
                case 1:
                    return DateTime.Now.AddYears(-1);
                    break;
                case 2:
                    return DateTime.Now.AddMonths(-6);
                    break;
                case 3:
                    return DateTime.Now.AddMonths(-3);
                    break;
                default:
                    return DateTime.Now.AddDays(1);
                    break;
            };
        }

        //Ventas
        private void button2_Click(object sender, EventArgs e)
        {
            MantenimientoVentas();
        }

        private void MantenimientoVentas()
        {
            if (ValidarChks())
            {
                try
                {
                    List<Venta> lista = VentasBd.GetLista();
                    lista = lista.Where(v => v.FechaVenta < GetFechaEliminacion(tiempoEliminacion)).ToList();
                    if (lista.Any())
                    {
                        foreach (var item in lista)
                        {
                            VentasBd.Borrar(item);
                            DetallesVentasBd.Eliminar(item.VentaId, true);
                        }
                        MessageBox.Show("Operación exitosa", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("No hay nada para eliminar", "Ventas", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                }
            }
        }

        private bool ValidarChks()
        {
            errorProvider1.Clear();
            if (chk3anios.Checked == false && chk1anio.Checked == false && chk6meses.Checked == false && chk3meses.Checked == false)
            {
                errorProvider1.SetError(chk3anios, "Debes seleccionar un check box.");
                return false;
            }
            return true;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (chk3anios.Checked == true)
            {
                chk1anio.Checked = false;
                chk6meses.Checked = false;
                chk3meses.Checked = false;
                tiempoEliminacion = 0;
            }
        }

        private void chk1anio_CheckedChanged(object sender, EventArgs e)
        {
            if (chk1anio.Checked == true)
            {
                chk3anios.Checked = false;
                chk6meses.Checked = false;
                chk3meses.Checked = false;
                tiempoEliminacion = 1;
            }
        }

        private void chk6meses_CheckedChanged(object sender, EventArgs e)
        {
            if (chk6meses.Checked == true)
            {
                chk1anio.Checked = false;
                chk3anios.Checked = false;
                chk3meses.Checked = false;
                tiempoEliminacion = 2;
            }
        }

        private void chk3meses_CheckedChanged(object sender, EventArgs e)
        {
            if (chk3meses.Checked == true)
            {
                chk1anio.Checked = false;
                chk3anios.Checked = false;
                chk6meses.Checked = false;
                tiempoEliminacion = 3;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //COMPRAS
        private void button1_Click(object sender, EventArgs e)
        {
            MantenimientoCompras();

        }

        private void MantenimientoCompras()
        {
            if (ValidarChks())
            {
                try
                {
                    List<Compra> lista = ComprasBd.GetLista();
                    lista = lista.Where(v => v.FechaCompra < GetFechaEliminacion(tiempoEliminacion)).ToList();
                    if (lista.Any())
                    {
                        foreach (var item in lista)
                        {
                            ComprasBd.Borrar(item);
                            DetallesComprasBd.Eliminar(item.CompraId, true);
                        }
                        MessageBox.Show($"{lista.Count} registros borrados.", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("No hay nada para eliminar", "Compras", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                }
            }
        }

        private void btnAlquileres_Click(object sender, EventArgs e)
        {
            MantenimientoAlquileres();
        }

        private void MantenimientoAlquileres()
        {
            if (ValidarChks())
            {
                try
                {

                    List<Alquiler> lista = AlquileresBd.GetLista();
                    lista = lista.Where(v => v.FechaDesde < GetFechaEliminacion(tiempoEliminacion)).ToList();
                    lista = lista.Where(v => v.EstaEnUso == false).ToList();
                    if (lista.Any())
                    {


                        foreach (var item in lista)
                        {
                            AlquileresBd.Borrar(item);
                            DetallesAlquileresBd.Eliminar(item.AlquilerId, true);
                        }
                        MessageBox.Show($"{lista.Count} registros borrados.", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("No hay nada para eliminar", "Alquileres", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                }
            }
        }

        private void btnOrdenes_Click(object sender, EventArgs e)
        {
            MantenimientoOrdenes();
        }

        private void MantenimientoOrdenes()
        {
            if (ValidarChks())
            {
                try
                {
                    List<Orden> lista = OrdenesBd.GetLista();
                    lista = lista.Where(v => v.FechaInicio < GetFechaEliminacion(tiempoEliminacion)).ToList();
                    lista = lista.Where(v => v.Entregado == true).ToList();
                    if (lista.Any())
                    {
                        foreach (var item in lista)
                        {
                            OrdenesBd.Borrar(item);
                            if (item.Cliente.Eliminado)
                            {
                                ClientesBd.Borrar(item.Cliente);
                            }
                        }
                        MessageBox.Show($"{lista.Count} registros borrados.", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("No hay nada para eliminar", "Ordenes", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                }
            }
        }

        private void btnTodo_Click(object sender, EventArgs e)
        {
            MantenimientoVentas(); //Ventas y detallesVentas - kardex?
            MantenimientoCompras(); //Compras y Detalles compras - kardex?
            MantenimientoAlquileres(); //alquileres y detalles Alquileres
            MantenimientoOrdenes(); //ordenes, protesis y clientes.eliminado = 1
            MantenimientoTransacciones(); //transacciones
        }

        private int contador;
        private void MantenimientoTransacciones()
        {
            if (ValidarChks())
            {
                try
                {
                    List<Transaccion> lista = TransaccionesBd.GetLista();
                    lista = lista.Where(v => v.FechaTransaccion < GetFechaEliminacion(tiempoEliminacion)).ToList();
                    if (lista.Any())
                    {
                        foreach (var item in lista)
                        {
                            TransaccionesBd.Borrar(item);
                        }
                        MessageBox.Show($"{lista.Count} registros borrados.", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("No hay nada para eliminar", "Ordenes", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

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
}
