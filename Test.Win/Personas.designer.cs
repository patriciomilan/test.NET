namespace Test.Win
{
    partial class Personas
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.agregarBoton = new System.Windows.Forms.Button();
            this.personasGridView = new System.Windows.Forms.DataGridView();
            this.personaViewBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.nombreDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.apellidoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.razonRegistroDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.personasGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.personaViewBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.agregarBoton);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.personasGridView);
            this.splitContainer1.Size = new System.Drawing.Size(800, 450);
            this.splitContainer1.SplitterDistance = 44;
            this.splitContainer1.TabIndex = 0;
            // 
            // agregarBoton
            // 
            this.agregarBoton.Location = new System.Drawing.Point(12, 12);
            this.agregarBoton.Name = "agregarBoton";
            this.agregarBoton.Size = new System.Drawing.Size(75, 23);
            this.agregarBoton.TabIndex = 0;
            this.agregarBoton.Text = "Agregar";
            this.agregarBoton.UseVisualStyleBackColor = true;
            this.agregarBoton.Click += new System.EventHandler(this.agregarBoton_Click);
            // 
            // personasGridView
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Azure;
            this.personasGridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.personasGridView.AutoGenerateColumns = false;
            this.personasGridView.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SunkenHorizontal;
            this.personasGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.personasGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.nombreDataGridViewTextBoxColumn,
            this.apellidoDataGridViewTextBoxColumn,
            this.razonRegistroDataGridViewTextBoxColumn});
            this.personasGridView.DataSource = this.personaViewBindingSource;
            this.personasGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.personasGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.personasGridView.Location = new System.Drawing.Point(0, 0);
            this.personasGridView.Name = "personasGridView";
            this.personasGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.personasGridView.Size = new System.Drawing.Size(800, 402);
            this.personasGridView.TabIndex = 0;
            // 
            // personaViewBindingSource
            // 
            this.personaViewBindingSource.DataSource = typeof(Test.Entidad.PersonaView);
            // 
            // nombreDataGridViewTextBoxColumn
            // 
            this.nombreDataGridViewTextBoxColumn.DataPropertyName = "Nombre";
            this.nombreDataGridViewTextBoxColumn.HeaderText = "Nombre";
            this.nombreDataGridViewTextBoxColumn.Name = "nombreDataGridViewTextBoxColumn";
            // 
            // apellidoDataGridViewTextBoxColumn
            // 
            this.apellidoDataGridViewTextBoxColumn.DataPropertyName = "Apellido";
            this.apellidoDataGridViewTextBoxColumn.HeaderText = "Apellido";
            this.apellidoDataGridViewTextBoxColumn.Name = "apellidoDataGridViewTextBoxColumn";
            // 
            // razonRegistroDataGridViewTextBoxColumn
            // 
            this.razonRegistroDataGridViewTextBoxColumn.DataPropertyName = "RazonRegistro";
            this.razonRegistroDataGridViewTextBoxColumn.HeaderText = "RazonRegistro";
            this.razonRegistroDataGridViewTextBoxColumn.Name = "razonRegistroDataGridViewTextBoxColumn";
            this.razonRegistroDataGridViewTextBoxColumn.Width = 400;
            // 
            // Personas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.splitContainer1);
            this.Name = "Personas";
            this.Text = "Personas";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.personasGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.personaViewBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView personasGridView;
        private System.Windows.Forms.BindingSource personaViewBindingSource;
        private System.Windows.Forms.Button agregarBoton;
        private System.Windows.Forms.DataGridViewTextBoxColumn nombreDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn apellidoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn razonRegistroDataGridViewTextBoxColumn;
    }
}

