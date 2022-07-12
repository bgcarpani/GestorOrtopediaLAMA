
namespace LAMAForms
{
    partial class frmOrdenes
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgvDatos = new System.Windows.Forms.DataGridView();
            this.cmnNroAlquiler = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cmnCliente = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cmnDni = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cmnNotas = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cmnFechaDesde = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cmnFechaHasta = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cmnEstimados = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cmnImporte = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cmnCosto = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tsbNuevo = new System.Windows.Forms.ToolStripButton();
            this.tsbBorrar = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripTextBox1 = new System.Windows.Forms.ToolStripTextBox();
            this.tsbActualizar = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbSalir = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnEntrega = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripComboBox1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.contratoDeAlquilerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDatos)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvDatos
            // 
            this.dgvDatos.AllowUserToAddRows = false;
            this.dgvDatos.AllowUserToDeleteRows = false;
            this.dgvDatos.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Silver;
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.dgvDatos.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvDatos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDatos.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cmnNroAlquiler,
            this.cmnCliente,
            this.cmnDni,
            this.cmnNotas,
            this.cmnFechaDesde,
            this.cmnFechaHasta,
            this.cmnEstimados,
            this.cmnImporte,
            this.cmnCosto});
            this.dgvDatos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDatos.Location = new System.Drawing.Point(0, 27);
            this.dgvDatos.Margin = new System.Windows.Forms.Padding(4);
            this.dgvDatos.MultiSelect = false;
            this.dgvDatos.Name = "dgvDatos";
            this.dgvDatos.ReadOnly = true;
            this.dgvDatos.RowHeadersVisible = false;
            this.dgvDatos.RowHeadersWidth = 51;
            this.dgvDatos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDatos.Size = new System.Drawing.Size(800, 423);
            this.dgvDatos.TabIndex = 15;
            this.dgvDatos.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDatos_CellContentClick);
            // 
            // cmnNroAlquiler
            // 
            this.cmnNroAlquiler.HeaderText = "Nro.";
            this.cmnNroAlquiler.MinimumWidth = 6;
            this.cmnNroAlquiler.Name = "cmnNroAlquiler";
            this.cmnNroAlquiler.ReadOnly = true;
            this.cmnNroAlquiler.Width = 75;
            // 
            // cmnCliente
            // 
            this.cmnCliente.HeaderText = "Cliente";
            this.cmnCliente.MinimumWidth = 6;
            this.cmnCliente.Name = "cmnCliente";
            this.cmnCliente.ReadOnly = true;
            this.cmnCliente.Width = 223;
            // 
            // cmnDni
            // 
            this.cmnDni.HeaderText = "DNI";
            this.cmnDni.MinimumWidth = 6;
            this.cmnDni.Name = "cmnDni";
            this.cmnDni.ReadOnly = true;
            this.cmnDni.Width = 125;
            // 
            // cmnNotas
            // 
            this.cmnNotas.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.cmnNotas.HeaderText = "Protesis";
            this.cmnNotas.MinimumWidth = 6;
            this.cmnNotas.Name = "cmnNotas";
            this.cmnNotas.ReadOnly = true;
            // 
            // cmnFechaDesde
            // 
            this.cmnFechaDesde.HeaderText = "Fecha de inicio";
            this.cmnFechaDesde.MinimumWidth = 6;
            this.cmnFechaDesde.Name = "cmnFechaDesde";
            this.cmnFechaDesde.ReadOnly = true;
            this.cmnFechaDesde.Width = 75;
            // 
            // cmnFechaHasta
            // 
            this.cmnFechaHasta.HeaderText = "Fecha de entrega";
            this.cmnFechaHasta.MinimumWidth = 6;
            this.cmnFechaHasta.Name = "cmnFechaHasta";
            this.cmnFechaHasta.ReadOnly = true;
            this.cmnFechaHasta.Width = 75;
            // 
            // cmnEstimados
            // 
            this.cmnEstimados.HeaderText = "Estimados";
            this.cmnEstimados.MinimumWidth = 6;
            this.cmnEstimados.Name = "cmnEstimados";
            this.cmnEstimados.ReadOnly = true;
            this.cmnEstimados.Width = 75;
            // 
            // cmnImporte
            // 
            this.cmnImporte.HeaderText = "Seña";
            this.cmnImporte.MinimumWidth = 6;
            this.cmnImporte.Name = "cmnImporte";
            this.cmnImporte.ReadOnly = true;
            this.cmnImporte.Width = 75;
            // 
            // cmnCosto
            // 
            this.cmnCosto.HeaderText = "Costo";
            this.cmnCosto.MinimumWidth = 6;
            this.cmnCosto.Name = "cmnCosto";
            this.cmnCosto.ReadOnly = true;
            this.cmnCosto.Width = 75;
            // 
            // tsbNuevo
            // 
            this.tsbNuevo.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbNuevo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbNuevo.Name = "tsbNuevo";
            this.tsbNuevo.Size = new System.Drawing.Size(56, 24);
            this.tsbNuevo.Text = "Nuevo";
            this.tsbNuevo.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsbNuevo.Click += new System.EventHandler(this.tsbNuevo_Click);
            // 
            // tsbBorrar
            // 
            this.tsbBorrar.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbBorrar.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbBorrar.Name = "tsbBorrar";
            this.tsbBorrar.Size = new System.Drawing.Size(54, 24);
            this.tsbBorrar.Text = "Borrar";
            this.tsbBorrar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsbBorrar.Click += new System.EventHandler(this.tsbBorrar_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
            // 
            // toolStripTextBox1
            // 
            this.toolStripTextBox1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.toolStripTextBox1.Name = "toolStripTextBox1";
            this.toolStripTextBox1.Size = new System.Drawing.Size(100, 27);
            this.toolStripTextBox1.TextChanged += new System.EventHandler(this.toolStripTextBox1_TextChanged);
            // 
            // tsbActualizar
            // 
            this.tsbActualizar.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbActualizar.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbActualizar.Name = "tsbActualizar";
            this.tsbActualizar.Size = new System.Drawing.Size(79, 24);
            this.tsbActualizar.Text = "Actualizar";
            this.tsbActualizar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsbActualizar.Click += new System.EventHandler(this.tsbActualizar_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
            // 
            // tsbSalir
            // 
            this.tsbSalir.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbSalir.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbSalir.Name = "tsbSalir";
            this.tsbSalir.Size = new System.Drawing.Size(42, 24);
            this.tsbSalir.Text = "Salir";
            this.tsbSalir.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsbSalir.Click += new System.EventHandler(this.tsbSalir_Click_1);
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbNuevo,
            this.btnEntrega,
            this.tsbBorrar,
            this.toolStripSeparator1,
            this.toolStripTextBox1,
            this.toolStripButton1,
            this.tsbActualizar,
            this.toolStripComboBox1,
            this.toolStripSeparator2,
            this.tsbSalir});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(800, 27);
            this.toolStrip1.TabIndex = 14;
            this.toolStrip1.Text = "toolStrip1";
            this.toolStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.toolStrip1_ItemClicked);
            // 
            // btnEntrega
            // 
            this.btnEntrega.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnEntrega.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnEntrega.Name = "btnEntrega";
            this.btnEntrega.Size = new System.Drawing.Size(121, 24);
            this.btnEntrega.Text = "Realizar entrega";
            this.btnEntrega.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnEntrega.Click += new System.EventHandler(this.btnEntrega_Click);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(84, 24);
            this.toolStripButton1.Text = "Pendientes";
            this.toolStripButton1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripComboBox1
            // 
            this.toolStripComboBox1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.contratoDeAlquilerToolStripMenuItem});
            this.toolStripComboBox1.Name = "toolStripComboBox1";
            this.toolStripComboBox1.Size = new System.Drawing.Size(80, 24);
            this.toolStripComboBox1.Text = "Imprimir";
            this.toolStripComboBox1.ToolTipText = "Imprimir";
            // 
            // contratoDeAlquilerToolStripMenuItem
            // 
            this.contratoDeAlquilerToolStripMenuItem.Name = "contratoDeAlquilerToolStripMenuItem";
            this.contratoDeAlquilerToolStripMenuItem.Size = new System.Drawing.Size(216, 26);
            this.contratoDeAlquilerToolStripMenuItem.Text = "Contrato de Orden";
            this.contratoDeAlquilerToolStripMenuItem.Click += new System.EventHandler(this.contratoDeAlquilerToolStripMenuItem_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(657, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(136, 17);
            this.label1.TabIndex = 16;
            this.label1.Text = "Ordenes de Trabajo";
            // 
            // frmOrdenes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.ControlBox = false;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dgvDatos);
            this.Controls.Add(this.toolStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmOrdenes";
            this.Text = "frmOrdenes";
            this.Load += new System.EventHandler(this.frmOrdenes_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDatos)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.DataGridView dgvDatos;
        private System.Windows.Forms.ToolStripButton tsbNuevo;
        private System.Windows.Forms.ToolStripButton tsbBorrar;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBox1;
        private System.Windows.Forms.ToolStripButton tsbActualizar;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton tsbSalir;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnEntrega;
        private System.Windows.Forms.ToolStripDropDownButton toolStripComboBox1;
        private System.Windows.Forms.ToolStripMenuItem contratoDeAlquilerToolStripMenuItem;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridViewTextBoxColumn cmnNroAlquiler;
        private System.Windows.Forms.DataGridViewTextBoxColumn cmnCliente;
        private System.Windows.Forms.DataGridViewTextBoxColumn cmnDni;
        private System.Windows.Forms.DataGridViewTextBoxColumn cmnNotas;
        private System.Windows.Forms.DataGridViewTextBoxColumn cmnFechaDesde;
        private System.Windows.Forms.DataGridViewTextBoxColumn cmnFechaHasta;
        private System.Windows.Forms.DataGridViewTextBoxColumn cmnEstimados;
        private System.Windows.Forms.DataGridViewTextBoxColumn cmnImporte;
        private System.Windows.Forms.DataGridViewTextBoxColumn cmnCosto;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
    }
}