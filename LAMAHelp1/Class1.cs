using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LAMAHelp1
{
    public static class Help
    {
        public static ContextMenuStrip CrearMenuContextual()
        {

            ContextMenuStrip cm = new ContextMenuStrip();
            ToolStripMenuItem botonAgregar = new ToolStripMenuItem
            {
             //   Image = Image.FromFile(@"../../Resources/Actions-document-new-icon.png"),
                Name = "agregarToolStripMenuItem",
                Size = new System.Drawing.Size(130, 22),
                Text = "Nuevo",
            };
            cm.Items.Add(botonAgregar);

            ToolStripMenuItem botonBorrar = new ToolStripMenuItem
            {
              //  Image = Image.FromFile(@"../../Resources/Actions-document-close-icon.png"),
                Name = "borrarToolStripMenuItem",
                Size = new System.Drawing.Size(130, 22),
                Text = "Borrar"
            };
            cm.Items.Add(botonBorrar);

            ToolStripMenuItem botonEditar = new ToolStripMenuItem
            {
             //   Image = Image.FromFile(@"../../Resources/edit-file-icon.png"),
                Name = "editarToolStripMenuItem",
                Size = new System.Drawing.Size(130, 22),
                Text = "Editar"
            };
            cm.Items.Add(botonEditar);

            ToolStripMenuItem botonVerDetalles = new ToolStripMenuItem
            {
               // Image = Image.FromFile(@"../../Resources/newVerDetallesAbierto.png"),
                Name = "verDetallesToolStripMenuItem",
                Size = new System.Drawing.Size(130, 22),
                Text = "Ver Detalles"
            };
            cm.Items.Add(botonVerDetalles);

            return cm;


        }
        public static void DeshabilitarControles(Form form)
        {
            Font myFont = new Font("Times New Roman", 10.0f, FontStyle.Bold);
            foreach (Control c in form.Controls)
            {
                c.Enabled = false;
                var box = c as TextBox;
                var combo = c as ComboBox;
                var grupo = c as GroupBox;
                if (box != null)
                {
                    box.Font = myFont;
                }
                else if (combo != null)
                {
                    combo.Font = myFont;
                }
                else if (grupo != null)
                {
                    foreach (Control c2 in grupo.Controls)
                    {
                        if (c2 is TextBox || c2 is ComboBox)
                        {
                            c2.Font = myFont;
                        }

                    }
                }
                else if (c is Button)
                {
                    if (c.Name == "btnCancelar")
                    {
                        c.Visible = false;
                    }
                    else if (c.Name == "btnOK")
                    {
                        c.Enabled = true;
                    }
                }
            }


        }
    }
}
