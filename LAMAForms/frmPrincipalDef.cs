using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;

namespace LAMAForms
{
    public partial class frmPrincipalDef : Form
    {
        public frmPrincipalDef()
        {
            Thread t = new Thread(new ThreadStart(SplashStart));
            t.Start();
            Thread.Sleep(1500);
            InitializeComponent();
            t.Abort();
            this.Focus();
        }

        public void SplashStart()
        {
            Application.Run(new Splash1());
        }

        private void btnRestore_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            btnRestore.Visible = false;
            btnMaximize.Visible = true;

        }

        private void btnMaximize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            btnRestore.Visible = true;
            btnMaximize.Visible = false;

        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

   

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

        private void pnlBarraTitulo_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112,0xf012,0);
        }

        private void btnArchivos_Click(object sender, EventArgs e)
        {
            panelMovimiento.Visible = false;
            panelArchivos.Visible = !panelArchivos.Visible;
        }

        private void frmPrincipalDef_Load(object sender, EventArgs e)
        {
            btnRestore.Visible = false;
            btnMaximize.Visible = true;
            frmInicio frm = frmInicio.GetInstancia();
            frm.MdiParent = this;
            frm.Show();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            panelArchivos.Visible = false;
            frmClientes frm = frmClientes.GetInstancia();
            frm.MdiParent = this;
            frm.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            panelArchivos.Visible = false;
            frmLocalidades frm = frmLocalidades.GetInstancia();
            frm.MdiParent = this;
            frm.Show();
        }

        private void btnVentas_Click(object sender, EventArgs e)
        {

        }

        private void btnPagos_Click(object sender, EventArgs e)
        {
            frmPagos frm = frmPagos.GetInstancia();
            frm.MdiParent = this;
            frm.Show();
        }

        private void btnMovimientos_Click(object sender, EventArgs e)
        {
            panelArchivos.Visible = false;
            panelMovimiento.Visible = !panelMovimiento.Visible;
        }

        private void button12_Click(object sender, EventArgs e)
        {
            panelMovimiento.Visible = false;
            frmVentas frm = frmVentas.GetInstancia();
            frm.MdiParent = this;
            frm.Show();

        }

        private void button11_Click(object sender, EventArgs e)
        {
            panelMovimiento.Visible = false;
            frmCompras frm = frmCompras.GetInstancia();
            frm.MdiParent = this;
            frm.Show();
        }


        private void bnCalcu_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process p = System.Diagnostics.Process.Start("calc.exe");
            p.WaitForInputIdle();
            //NativeMethods.SetParent(p.MainWindowHandle, this.Handle);
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void pnlBarraTitulo_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            frmProductos frm = frmProductos.GetInstancia();
            frm.MdiParent = this;
            frm.Show();
        }

        private void btnInicio_Click(object sender, EventArgs e)
        {
            foreach (Form form in this.MdiChildren)
            {
                form.Close();
            }
            frmInicio frm = frmInicio.GetInstancia();
            frm.MdiParent = this;
            frm.Show();
        }
        private void button13_Click_1(object sender, EventArgs e)
        {
            frmAlquileres frm = frmAlquileres.GetInstancia();
            frm.MdiParent = this;
            frm.Show();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            frmStocks frm = frmStocks.GetInstancia();
            frm.Show();
        }


        private void btnKardex_Click_1(object sender, EventArgs e)
        {
            frmKardex frm = frmKardex.GetInstancia();
            frm.Show();
        }

        private void panelArchivos_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnOrdenes_Click_1(object sender, EventArgs e)
        {
            panelMovimiento.Visible = false;
            frmOrdenes frm = frmOrdenes.GetInstancia();
            frm.MdiParent = this;
            frm.Show();
        }

        private void btnCaja_Click_1(object sender, EventArgs e)
        {
            frmCtasCtes frm = frmCtasCtes.GetInstancia();
            frm.MdiParent = this;
            frm.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            frmMantenimiento frm = frmMantenimiento.GetInstancia();
            frm.Show();
        }

        private void button29_Click(object sender, EventArgs e)
        {
            frmProtesis frm = frmProtesis.GetInstancia();
            frm.MdiParent = this;
            frm.Show();
        }

        private void button14_Click_1(object sender, EventArgs e)
        {
            frmStocks frm = frmStocks.GetInstancia();
            frm.Show();
        }
    }
}
