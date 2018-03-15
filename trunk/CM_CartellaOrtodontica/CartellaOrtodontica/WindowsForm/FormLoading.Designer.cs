namespace CartellaOrtodontica.WindowsForm
{
    partial class FormLoading
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormLoading));
            this.uiProgressBar_loading = new Janus.Windows.EditControls.UIProgressBar();
            this.label_loading = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // uiProgressBar_loading
            // 
            this.uiProgressBar_loading.Location = new System.Drawing.Point(12, 44);
            this.uiProgressBar_loading.Name = "uiProgressBar_loading";
            this.uiProgressBar_loading.Size = new System.Drawing.Size(430, 23);
            this.uiProgressBar_loading.TabIndex = 0;
            // 
            // label_loading
            // 
            this.label_loading.AutoSize = true;
            this.label_loading.Location = new System.Drawing.Point(13, 13);
            this.label_loading.Name = "label_loading";
            this.label_loading.Size = new System.Drawing.Size(16, 13);
            this.label_loading.TabIndex = 1;
            this.label_loading.Text = "...";
            // 
            // FormLoading
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(454, 74);
            this.Controls.Add(this.label_loading);
            this.Controls.Add(this.uiProgressBar_loading);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormLoading";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "caricamento...";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        public System.Windows.Forms.Label label_loading;
        public Janus.Windows.EditControls.UIProgressBar uiProgressBar_loading;
    }
}