namespace TP___Reflexion
{
    partial class Form1
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnAbrir = new System.Windows.Forms.Button();
            this.btnGenerarSQL = new System.Windows.Forms.Button();
            this.cmbClases = new System.Windows.Forms.ComboBox();
            this.txtSQL = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // btnAbrir
            // 
            this.btnAbrir.Location = new System.Drawing.Point(69, 75);
            this.btnAbrir.Name = "btnAbrir";
            this.btnAbrir.Size = new System.Drawing.Size(75, 23);
            this.btnAbrir.TabIndex = 0;
            this.btnAbrir.Text = "Abrir";
            this.btnAbrir.UseVisualStyleBackColor = true;
            this.btnAbrir.Click += new System.EventHandler(this.btnAbrir_Click);
            // 
            // btnGenerarSQL
            // 
            this.btnGenerarSQL.Location = new System.Drawing.Point(69, 151);
            this.btnGenerarSQL.Name = "btnGenerarSQL";
            this.btnGenerarSQL.Size = new System.Drawing.Size(75, 23);
            this.btnGenerarSQL.TabIndex = 1;
            this.btnGenerarSQL.Text = "Generar";
            this.btnGenerarSQL.UseVisualStyleBackColor = true;
            // 
            // cmbClases
            // 
            this.cmbClases.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbClases.FormattingEnabled = true;
            this.cmbClases.Location = new System.Drawing.Point(181, 75);
            this.cmbClases.Name = "cmbClases";
            this.cmbClases.Size = new System.Drawing.Size(121, 21);
            this.cmbClases.TabIndex = 2;
            // 
            // txtSQL
            // 
            this.txtSQL.FormattingEnabled = true;
            this.txtSQL.Location = new System.Drawing.Point(181, 151);
            this.txtSQL.Name = "txtSQL";
            this.txtSQL.Size = new System.Drawing.Size(592, 147);
            this.txtSQL.TabIndex = 3;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.txtSQL);
            this.Controls.Add(this.cmbClases);
            this.Controls.Add(this.btnGenerarSQL);
            this.Controls.Add(this.btnAbrir);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnAbrir;
        private System.Windows.Forms.Button btnGenerarSQL;
        private System.Windows.Forms.ComboBox cmbClases;
        private System.Windows.Forms.ListBox txtSQL;
    }
}

