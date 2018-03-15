namespace CartellaOrtodontica.WindowsForm
{
    partial class FormTrattamentoRefertazione
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormTrattamentoRefertazione));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.uiComboBox_DescrizioneDato = new Janus.Windows.EditControls.UIComboBox();
            this.editBox_NumeroTrattamento = new Janus.Windows.GridEX.EditControls.EditBox();
            this.editBox_Valore = new Janus.Windows.GridEX.EditControls.EditBox();
            this.editBox_Osservazioni = new Janus.Windows.GridEX.EditControls.EditBox();
            this.btn_paziente_daticlinici_save = new Janus.Windows.EditControls.UIButton();
            this.btn_paziente_daticlinici_annulla = new Janus.Windows.EditControls.UIButton();
            this.label5 = new System.Windows.Forms.Label();
            this.editBox_Procedura = new Janus.Windows.GridEX.EditControls.EditBox();
            this.editBox_Sottoprocedura = new Janus.Windows.GridEX.EditControls.EditBox();
            this.label6 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(427, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Numero Trattamento";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(23, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Dato Clinico";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(23, 115);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(48, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Risultato";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(20, 147);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(67, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Osservazioni";
            // 
            // uiComboBox_DescrizioneDato
            // 
            this.uiComboBox_DescrizioneDato.Location = new System.Drawing.Point(107, 5);
            this.uiComboBox_DescrizioneDato.Name = "uiComboBox_DescrizioneDato";
            this.uiComboBox_DescrizioneDato.Size = new System.Drawing.Size(280, 20);
            this.uiComboBox_DescrizioneDato.TabIndex = 4;
            this.uiComboBox_DescrizioneDato.Text = "uiComboBox1";
            // 
            // editBox_NumeroTrattamento
            // 
            this.editBox_NumeroTrattamento.Location = new System.Drawing.Point(537, 5);
            this.editBox_NumeroTrattamento.Name = "editBox_NumeroTrattamento";
            this.editBox_NumeroTrattamento.Size = new System.Drawing.Size(79, 20);
            this.editBox_NumeroTrattamento.TabIndex = 5;
            // 
            // editBox_Valore
            // 
            this.editBox_Valore.Location = new System.Drawing.Point(107, 112);
            this.editBox_Valore.Name = "editBox_Valore";
            this.editBox_Valore.Size = new System.Drawing.Size(508, 20);
            this.editBox_Valore.TabIndex = 8;
            // 
            // editBox_Osservazioni
            // 
            this.editBox_Osservazioni.Location = new System.Drawing.Point(20, 163);
            this.editBox_Osservazioni.Multiline = true;
            this.editBox_Osservazioni.Name = "editBox_Osservazioni";
            this.editBox_Osservazioni.Size = new System.Drawing.Size(603, 127);
            this.editBox_Osservazioni.TabIndex = 9;
            // 
            // btn_paziente_daticlinici_save
            // 
            this.btn_paziente_daticlinici_save.Image = global::CartellaOrtodontica.Properties.Resources.save_48;
            this.btn_paziente_daticlinici_save.Location = new System.Drawing.Point(226, 318);
            this.btn_paziente_daticlinici_save.Name = "btn_paziente_daticlinici_save";
            this.btn_paziente_daticlinici_save.Size = new System.Drawing.Size(75, 23);
            this.btn_paziente_daticlinici_save.TabIndex = 10;
            this.btn_paziente_daticlinici_save.Text = "salva";
            this.btn_paziente_daticlinici_save.Click += new System.EventHandler(this.btn_paziente_daticlinici_save_Click);
            // 
            // btn_paziente_daticlinici_annulla
            // 
            this.btn_paziente_daticlinici_annulla.Image = global::CartellaOrtodontica.Properties.Resources.cancel_2_48;
            this.btn_paziente_daticlinici_annulla.Location = new System.Drawing.Point(322, 318);
            this.btn_paziente_daticlinici_annulla.Name = "btn_paziente_daticlinici_annulla";
            this.btn_paziente_daticlinici_annulla.Size = new System.Drawing.Size(75, 23);
            this.btn_paziente_daticlinici_annulla.TabIndex = 11;
            this.btn_paziente_daticlinici_annulla.Text = "annulla";
            this.btn_paziente_daticlinici_annulla.Click += new System.EventHandler(this.btn_paziente_daticlinici_annulla_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(23, 39);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(56, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "Procedura";
            // 
            // editBox_Procedura
            // 
            this.editBox_Procedura.Location = new System.Drawing.Point(107, 39);
            this.editBox_Procedura.Name = "editBox_Procedura";
            this.editBox_Procedura.ReadOnly = true;
            this.editBox_Procedura.Size = new System.Drawing.Size(508, 20);
            this.editBox_Procedura.TabIndex = 13;
            // 
            // editBox_Sottoprocedura
            // 
            this.editBox_Sottoprocedura.Location = new System.Drawing.Point(107, 73);
            this.editBox_Sottoprocedura.Name = "editBox_Sottoprocedura";
            this.editBox_Sottoprocedura.ReadOnly = true;
            this.editBox_Sottoprocedura.Size = new System.Drawing.Size(508, 20);
            this.editBox_Sottoprocedura.TabIndex = 15;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(20, 76);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(81, 13);
            this.label6.TabIndex = 14;
            this.label6.Text = "SottoProcedura";
            // 
            // FormTrattamentoRefertazione
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(637, 353);
            this.Controls.Add(this.editBox_Sottoprocedura);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.editBox_Procedura);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btn_paziente_daticlinici_annulla);
            this.Controls.Add(this.btn_paziente_daticlinici_save);
            this.Controls.Add(this.editBox_Osservazioni);
            this.Controls.Add(this.editBox_Valore);
            this.Controls.Add(this.editBox_NumeroTrattamento);
            this.Controls.Add(this.uiComboBox_DescrizioneDato);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormTrattamentoRefertazione";
            this.Text = "Refertazione dato clinico";
            this.Load += new System.EventHandler(this.FormTrattamentoRefertazione_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private Janus.Windows.EditControls.UIButton btn_paziente_daticlinici_save;
        private Janus.Windows.EditControls.UIButton btn_paziente_daticlinici_annulla;
        public Janus.Windows.EditControls.UIComboBox uiComboBox_DescrizioneDato;
        public Janus.Windows.GridEX.EditControls.EditBox editBox_NumeroTrattamento;
        public Janus.Windows.GridEX.EditControls.EditBox editBox_Valore;
        public Janus.Windows.GridEX.EditControls.EditBox editBox_Osservazioni;
        private System.Windows.Forms.Label label5;
        public Janus.Windows.GridEX.EditControls.EditBox editBox_Procedura;
        public Janus.Windows.GridEX.EditControls.EditBox editBox_Sottoprocedura;
        private System.Windows.Forms.Label label6;
    }
}