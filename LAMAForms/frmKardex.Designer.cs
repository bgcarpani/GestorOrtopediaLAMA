
namespace LAMAForms
{
    partial class frmKardex
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.label1 = new System.Windows.Forms.Label();
            this.cboProductos = new System.Windows.Forms.ComboBox();
            this.dgvDatos = new System.Windows.Forms.DataGridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cmnFecha = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cmnMovimiento = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cmnEntrada = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cmnSalida = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cmnSaldo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cmnUltimoCosto = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cmnCostoPromedio = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDatos)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(28, 40);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 17);
            this.label1.TabIndex = 19;
            this.label1.Text = "Producto:";
            // 
            // cboProductos
            // 
            this.cboProductos.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboProductos.FormattingEnabled = true;
            this.cboProductos.Location = new System.Drawing.Point(118, 37);
            this.cboProductos.Margin = new System.Windows.Forms.Padding(4);
            this.cboProductos.Name = "cboProductos";
            this.cboProductos.Size = new System.Drawing.Size(345, 24);
            this.cboProductos.TabIndex = 17;
            this.cboProductos.SelectedIndexChanged += new System.EventHandler(this.cboProductos_SelectedIndexChanged);
            // 
            // dgvDatos
            // 
            this.dgvDatos.AllowUserToAddRows = false;
            this.dgvDatos.AllowUserToDeleteRows = false;
            this.dgvDatos.AllowUserToResizeRows = false;
            this.dgvDatos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDatos.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cmnFecha,
            this.cmnMovimiento,
            this.cmnEntrada,
            this.cmnSalida,
            this.cmnSaldo,
            this.cmnUltimoCosto,
            this.cmnCostoPromedio});
            this.dgvDatos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDatos.Location = new System.Drawing.Point(0, 0);
            this.dgvDatos.Margin = new System.Windows.Forms.Padding(4);
            this.dgvDatos.MultiSelect = false;
            this.dgvDatos.Name = "dgvDatos";
            this.dgvDatos.ReadOnly = true;
            this.dgvDatos.RowHeadersVisible = false;
            this.dgvDatos.RowHeadersWidth = 51;
            this.dgvDatos.Size = new System.Drawing.Size(875, 494);
            this.dgvDatos.TabIndex = 20;
            this.dgvDatos.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDatos_CellContentClick);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cboProductos);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(492, 89);
            this.groupBox1.TabIndex = 21;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Consulta de item";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.dgvDatos);
            this.panel1.Location = new System.Drawing.Point(3, 107);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(875, 494);
            this.panel1.TabIndex = 22;
            // 
            // cmnFecha
            // 
            this.cmnFecha.HeaderText = "Fecha";
            this.cmnFecha.MinimumWidth = 6;
            this.cmnFecha.Name = "cmnFecha";
            this.cmnFecha.ReadOnly = true;
            this.cmnFecha.Width = 150;
            // 
            // cmnMovimiento
            // 
            this.cmnMovimiento.HeaderText = "Movimiento";
            this.cmnMovimiento.MinimumWidth = 6;
            this.cmnMovimiento.Name = "cmnMovimiento";
            this.cmnMovimiento.ReadOnly = true;
            this.cmnMovimiento.Width = 70;
            // 
            // cmnEntrada
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.cmnEntrada.DefaultCellStyle = dataGridViewCellStyle1;
            this.cmnEntrada.HeaderText = "Entrada";
            this.cmnEntrada.MinimumWidth = 6;
            this.cmnEntrada.Name = "cmnEntrada";
            this.cmnEntrada.ReadOnly = true;
            this.cmnEntrada.Width = 70;
            // 
            // cmnSalida
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.cmnSalida.DefaultCellStyle = dataGridViewCellStyle2;
            this.cmnSalida.HeaderText = "Salida";
            this.cmnSalida.MinimumWidth = 6;
            this.cmnSalida.Name = "cmnSalida";
            this.cmnSalida.ReadOnly = true;
            this.cmnSalida.Width = 65;
            // 
            // cmnSaldo
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.cmnSaldo.DefaultCellStyle = dataGridViewCellStyle3;
            this.cmnSaldo.HeaderText = "Saldo";
            this.cmnSaldo.MinimumWidth = 6;
            this.cmnSaldo.Name = "cmnSaldo";
            this.cmnSaldo.ReadOnly = true;
            this.cmnSaldo.Width = 65;
            // 
            // cmnUltimoCosto
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.cmnUltimoCosto.DefaultCellStyle = dataGridViewCellStyle4;
            this.cmnUltimoCosto.HeaderText = "Último Costo";
            this.cmnUltimoCosto.MinimumWidth = 6;
            this.cmnUltimoCosto.Name = "cmnUltimoCosto";
            this.cmnUltimoCosto.ReadOnly = true;
            this.cmnUltimoCosto.Width = 125;
            // 
            // cmnCostoPromedio
            // 
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.cmnCostoPromedio.DefaultCellStyle = dataGridViewCellStyle5;
            this.cmnCostoPromedio.HeaderText = "Costo Promedio";
            this.cmnCostoPromedio.MinimumWidth = 6;
            this.cmnCostoPromedio.Name = "cmnCostoPromedio";
            this.cmnCostoPromedio.ReadOnly = true;
            this.cmnCostoPromedio.Width = 125;
            // 
            // frmKardex
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(881, 603);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox1);
            this.Name = "frmKardex";
            this.Text = "Consulta de kardex";
            this.Load += new System.EventHandler(this.frmKardex_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDatos)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboProductos;
        private System.Windows.Forms.DataGridView dgvDatos;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridViewTextBoxColumn cmnFecha;
        private System.Windows.Forms.DataGridViewTextBoxColumn cmnMovimiento;
        private System.Windows.Forms.DataGridViewTextBoxColumn cmnEntrada;
        private System.Windows.Forms.DataGridViewTextBoxColumn cmnSalida;
        private System.Windows.Forms.DataGridViewTextBoxColumn cmnSaldo;
        private System.Windows.Forms.DataGridViewTextBoxColumn cmnUltimoCosto;
        private System.Windows.Forms.DataGridViewTextBoxColumn cmnCostoPromedio;
    }
}