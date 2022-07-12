
namespace LAMAForms
{
    partial class frmMantenimiento
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
            this.components = new System.ComponentModel.Container();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chk3meses = new System.Windows.Forms.CheckBox();
            this.chk6meses = new System.Windows.Forms.CheckBox();
            this.chk1anio = new System.Windows.Forms.CheckBox();
            this.chk3anios = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.btnTodo = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.btnOrdenes = new System.Windows.Forms.Button();
            this.btnAlquileres = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chk3meses);
            this.groupBox1.Controls.Add(this.chk6meses);
            this.groupBox1.Controls.Add(this.chk1anio);
            this.groupBox1.Controls.Add(this.chk3anios);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(13, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(616, 88);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Selecciona:";
            // 
            // chk3meses
            // 
            this.chk3meses.AutoSize = true;
            this.chk3meses.Location = new System.Drawing.Point(495, 41);
            this.chk3meses.Name = "chk3meses";
            this.chk3meses.Size = new System.Drawing.Size(83, 21);
            this.chk3meses.TabIndex = 5;
            this.chk3meses.Text = "3 meses";
            this.chk3meses.UseVisualStyleBackColor = true;
            this.chk3meses.CheckedChanged += new System.EventHandler(this.chk3meses_CheckedChanged);
            // 
            // chk6meses
            // 
            this.chk6meses.AutoSize = true;
            this.chk6meses.Location = new System.Drawing.Point(354, 41);
            this.chk6meses.Name = "chk6meses";
            this.chk6meses.Size = new System.Drawing.Size(83, 21);
            this.chk6meses.TabIndex = 4;
            this.chk6meses.Text = "6 meses";
            this.chk6meses.UseVisualStyleBackColor = true;
            this.chk6meses.CheckedChanged += new System.EventHandler(this.chk6meses_CheckedChanged);
            // 
            // chk1anio
            // 
            this.chk1anio.AutoSize = true;
            this.chk1anio.Location = new System.Drawing.Point(216, 41);
            this.chk1anio.Name = "chk1anio";
            this.chk1anio.Size = new System.Drawing.Size(66, 21);
            this.chk1anio.TabIndex = 3;
            this.chk1anio.Text = "1 año";
            this.chk1anio.UseVisualStyleBackColor = true;
            this.chk1anio.CheckedChanged += new System.EventHandler(this.chk1anio_CheckedChanged);
            // 
            // chk3anios
            // 
            this.chk3anios.AutoSize = true;
            this.chk3anios.Location = new System.Drawing.Point(91, 41);
            this.chk3anios.Name = "chk3anios";
            this.chk3anios.Size = new System.Drawing.Size(73, 21);
            this.chk3anios.TabIndex = 2;
            this.chk3anios.Text = "3 años";
            this.chk3anios.UseVisualStyleBackColor = true;
            this.chk3anios.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Desde:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.btnTodo);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.button1);
            this.groupBox2.Controls.Add(this.btnOrdenes);
            this.groupBox2.Controls.Add(this.btnAlquileres);
            this.groupBox2.Controls.Add(this.button2);
            this.groupBox2.Location = new System.Drawing.Point(13, 107);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(616, 269);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Limpieza de:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(161, 226);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(207, 17);
            this.label9.TabIndex = 11;
            this.label9.Text = "Elimina todo lo detallado arriba.";
            // 
            // btnTodo
            // 
            this.btnTodo.Location = new System.Drawing.Point(16, 217);
            this.btnTodo.Name = "btnTodo";
            this.btnTodo.Size = new System.Drawing.Size(130, 40);
            this.btnTodo.TabIndex = 10;
            this.btnTodo.Text = "Todo";
            this.btnTodo.UseVisualStyleBackColor = true;
            this.btnTodo.Click += new System.EventHandler(this.btnTodo_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(163, 195);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(217, 17);
            this.label7.TabIndex = 9;
            this.label7.Text = "al rango de tiempo seleccionado.";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(163, 178);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(443, 17);
            this.label6.TabIndex = 8;
            this.label6.Text = "Elimina todas las ordenes de trabajo y clientes temporales anteriores";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(163, 151);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(95, 17);
            this.label5.TabIndex = 7;
            this.label5.Text = "seleccionado.";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(163, 134);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(445, 17);
            this.label4.TabIndex = 6;
            this.label4.Text = "Elimina todos los alquileres entregados anteriores al rango de tiempo";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(163, 91);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(453, 17);
            this.label3.TabIndex = 5;
            this.label3.Text = "Elimina todas las compras anteriores al rango de tiempo seleccionado.";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(163, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(441, 17);
            this.label2.TabIndex = 2;
            this.label2.Text = "Elimina todas las ventas anteriores al rango de tiempo seleccionado.";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(16, 79);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(130, 40);
            this.button1.TabIndex = 4;
            this.button1.Text = "Compras";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnOrdenes
            // 
            this.btnOrdenes.Location = new System.Drawing.Point(16, 171);
            this.btnOrdenes.Name = "btnOrdenes";
            this.btnOrdenes.Size = new System.Drawing.Size(130, 40);
            this.btnOrdenes.TabIndex = 3;
            this.btnOrdenes.Text = "Ord. de Trabajo";
            this.btnOrdenes.UseVisualStyleBackColor = true;
            this.btnOrdenes.Click += new System.EventHandler(this.btnOrdenes_Click);
            // 
            // btnAlquileres
            // 
            this.btnAlquileres.Location = new System.Drawing.Point(16, 125);
            this.btnAlquileres.Name = "btnAlquileres";
            this.btnAlquileres.Size = new System.Drawing.Size(130, 40);
            this.btnAlquileres.TabIndex = 2;
            this.btnAlquileres.Text = "Alquileres";
            this.btnAlquileres.UseVisualStyleBackColor = true;
            this.btnAlquileres.Click += new System.EventHandler(this.btnAlquileres_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(16, 33);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(130, 40);
            this.button2.TabIndex = 1;
            this.button2.Text = "Ventas";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(501, 382);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(126, 32);
            this.button6.TabIndex = 12;
            this.button6.Text = "Cerrar";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // frmMantenimiento
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(639, 421);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "frmMantenimiento";
            this.Text = "Mantenimiento";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnOrdenes;
        private System.Windows.Forms.Button btnAlquileres;
        private System.Windows.Forms.CheckBox chk3meses;
        private System.Windows.Forms.CheckBox chk6meses;
        private System.Windows.Forms.CheckBox chk1anio;
        private System.Windows.Forms.CheckBox chk3anios;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button btnTodo;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.ErrorProvider errorProvider1;
    }
}