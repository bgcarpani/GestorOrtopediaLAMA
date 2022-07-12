using LAMADatabase;
using LAMAModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LAMAForms
{
    public partial class frmProductosAE : Form
    {
        public frmProductosAE()
        {
            InitializeComponent();
        }

        private Producto producto;
        public Producto GetProducto()
        {
            return producto;
        }

        public void SetProducto(Producto producto)
        {
            this.producto = producto;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            //TiposDeProductosBd.CargarCombo(ref cboTipoProducto);
            MarcasBd.CargarCombo(ref cboMarca);
            TiposBd.CargarCombo(ref cmnTipo);

            //AlicuotasIvaBd.CargarCombo(ref cboAlicuotaIva);
            if (producto != null)
            {
               // btnAsignar.Enabled = true;
                txtDescripcion.Text = producto.Descripcion;
                cboMarca.SelectedValue = producto.Marca.MarcaId;
                txtPrecioVenta.Text = producto.Precio.ToString();
                textBox1.Text = producto.PrecioAlquiler.ToString();
                txtQuincena.Text = producto.PrecioAlquilerQuincena.ToString();
                //txtPrecioCompra.Text = producto.Precio.ToString();
                //Armo la ruta para que ponga la imagen no disponible
                //string imagenNoDisponible = Application.StartupPath + "\\Imágenes\\" + "ImagenNoDisponible.png";
                ////Pregunto si el objeto producto tiene seteada su propiedad Foto
                //if (producto.Foto != null || producto.Foto != string.Empty)
                //{
                //    archivoNombre = producto.Foto;//Tomo el nombre del archivo de la foto
                //    //le agrego la ruta para que busque el archivo
                //    string archivoNombreConRuta = Application.StartupPath + "\\Imágenes\\" + archivoNombre;
                //    //Veo si existe el archivo de imagen
                //    if (File.Exists(archivoNombreConRuta))
                //    {
                //        //Si existe la muestro
                //        picProducto.Image = Image.FromFile(@archivoNombreConRuta);
                //    }
                //    else
                //    {
                //        if (File.Exists(imagenNoDisponible))
                //            //Si no existe o no tiene muestro la imagen no disponible
                //            picProducto.Image = Image.FromFile(@imagenNoDisponible);
                //    }
                //}
                //else
                //{
                //    if (File.Exists(imagenNoDisponible))
                //        //Si no tiene imagen muestro la imagen no disponible
                //        picProducto.Image = Image.FromFile(@imagenNoDisponible);

                //}
                //chkSuspendido.Checked = producto.BajaProducto ? true : false;
               // ActualizarGrillaSucursales();
            }
        }

        public void SetEditar(bool editar)
        {
            this.editar = editar;
        }
        private bool editar = false;
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void InicializarControles()
        {
            txtDescripcion.Clear();
            txtPrecioVenta.Clear();
            cboMarca.SelectedIndex = 0;
            txtDescripcion.Focus();
        }

        private bool ValidarDatos()
        {
            bool Valido = true;
            errorProvider1.Clear();
            if (string.IsNullOrEmpty(txtDescripcion.Text))
            {
                Valido = false;
                errorProvider1.SetError(txtDescripcion, "Debe ingresar una descripción");
            }
            if (cboMarca.SelectedIndex == 0)
            {
                Valido = false;
                errorProvider1.SetError(cboMarca, "Debe seleccionar una marca");
            }
            if (cmnTipo.SelectedIndex == 0)
            {
                Valido = false;
                errorProvider1.SetError(cboMarca, "Debe seleccionar una marca");
            }
            decimal precioCosto;
            decimal precioVenta;

            if (!decimal.TryParse(txtPrecioVenta.Text, out precioVenta))
            {
                Valido = false;
                errorProvider1.SetError(txtPrecioVenta, "Debe ingresar un precio de venta válido");
            }
            if (!decimal.TryParse(textBox1.Text, out decimal precioAlq))
            {
                Valido = false;
                errorProvider1.SetError(txtPrecioVenta, "Debe ingresar un precio de alquiler válido");
            }
            if (!decimal.TryParse(txtQuincena.Text, out decimal precioQuin))
            {
                Valido = false;
                errorProvider1.SetError(txtQuincena, "Debe ingresar un precio de alquiler válido");
            }
            return Valido;
        }
        string archivoNombre = "";
        string archivoNombreConRuta = "";

        private List<ProductoSucursal> lista;
      
        private void frmProductosAE_Load(object sender, EventArgs e)
        {

        }

        private void picMarca_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            frmMarcasAE frm = new frmMarcasAE();
            frm.Text = "Agregar Marca";
            DialogResult dr = frm.ShowDialog(this);
            MarcasBd.CargarCombo(ref cboMarca);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (ValidarDatos())
            {
                if (producto == null)
                {
                    producto = new Producto();
                }
                producto.Descripcion = txtDescripcion.Text;
                producto.Marca = (Marca)cboMarca.SelectedItem;
                producto.Tipo = (Tipo)cmnTipo.SelectedItem;
                producto.Precio = decimal.Parse(txtPrecioVenta.Text);
                producto.PrecioAlquiler = decimal.Parse(textBox1.Text);
                producto.PrecioAlquilerQuincena = decimal.Parse(txtQuincena.Text);
                if (!editar)
                {
                    try
                    {
                        ProductosBd.Agregar(producto);
                        MessageBox.Show("Operación exitosa", "Mensaje", MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                        DialogResult dr = MessageBox.Show("¿Desea agregar otro producto?", "Continuar",
                            MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                        if (dr == DialogResult.Yes)
                        {
                            InicializarControles();
                        }
                        else
                        {
                            this.DialogResult = DialogResult.OK;
                        }
                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    this.DialogResult = DialogResult.OK;

                }
            }
        }

        private void btnCancelar_Click_1(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void btnBuscarImagen_Click(object sender, EventArgs e)
        {

            //Seteo el directorio donde se buscarán y grabarán las imágenes de los productos
            string directorioDeImagenes = Application.StartupPath + "\\Imágenes";
            //Veo si no existe la carpeta Imágenes en la carpeta donde corre el proyecto
            if (!Directory.Exists(directorioDeImagenes))
            {
                //De ser así la creo
                Directory.CreateDirectory(Application.StartupPath + "\\Imágenes");
            }
            //Abro un filedialog para buscar un archivo de imagen
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            //Seteo que no se pueda elegir más de uno a la vez
            openFileDialog1.Multiselect = false;
            //Veo si existe la carpeta imágenes
            if (Directory.Exists(directorioDeImagenes))
            {
                //De ser así seteo como lugar de búsqueda para el FileDialog
                openFileDialog1.InitialDirectory = directorioDeImagenes;

            }
            else
            {
                //Caso contrario busco en el disco C
                openFileDialog1.InitialDirectory = "C:\\";
            }
            //Pongo los filtros 
            openFileDialog1.Filter = " Imagenes(*.BMP;*.JPG;*.GIF;*.PGN)|*.BMP;*.JPG;*.GIF;*.PNG|All files (*.*)|*.*";
            //Por defecto busco bitmaps
            openFileDialog1.FilterIndex = 1;
            //Abro el cuadro de diálogo
            DialogResult dr = openFileDialog1.ShowDialog(this);
            //Si el dialogo termina OK
            if (dr == DialogResult.OK)
            {
                //Tomo el nombre del archivo de imagen con su ruta
                archivoNombreConRuta = openFileDialog1.FileName;
                //Obtengo el nombre del archivo solo sin ruta
                archivoNombre = archivoNombreConRuta.Substring(archivoNombreConRuta.LastIndexOf("\\") + 1);
                //Defino la ruta de guardado del archivo de imágen
                string archivoRutaDeGuardado = directorioDeImagenes + "\\" + archivoNombre;
                //Si el archivo no existe
                if (!File.Exists(archivoRutaDeGuardado))
                {
                    //lo guardo
                    File.Move(archivoNombreConRuta, archivoRutaDeGuardado);
                }
                //Muestro la imagen en el picture box
            }

        }

        private void btnAddTipo_Click(object sender, EventArgs e)
        {
            frmTiposAE frm = new frmTiposAE();
            frm.Text = "Agregar Tipo";
            DialogResult dr = frm.ShowDialog(this);
            TiposBd.CargarCombo(ref cmnTipo);
        }
    }

}
