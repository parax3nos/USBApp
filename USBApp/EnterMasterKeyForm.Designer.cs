namespace USBApp
{
    partial class EnterMasterKeyForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EnterMasterKeyForm));
            this.tbMasterKey = new System.Windows.Forms.TextBox();
            this.bMasterKey = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbMasterKey
            // 
            this.tbMasterKey.Font = new System.Drawing.Font("Segoe UI", 16.2F);
            this.tbMasterKey.Location = new System.Drawing.Point(13, 14);
            this.tbMasterKey.Name = "tbMasterKey";
            this.tbMasterKey.Size = new System.Drawing.Size(600, 43);
            this.tbMasterKey.TabIndex = 0;
            // 
            // bMasterKey
            // 
            this.bMasterKey.Font = new System.Drawing.Font("Segoe UI Semibold", 16.2F, System.Drawing.FontStyle.Bold);
            this.bMasterKey.Location = new System.Drawing.Point(384, 69);
            this.bMasterKey.Name = "bMasterKey";
            this.bMasterKey.Size = new System.Drawing.Size(229, 54);
            this.bMasterKey.TabIndex = 1;
            this.bMasterKey.Text = "Подтвердить";
            this.bMasterKey.UseVisualStyleBackColor = true;
            this.bMasterKey.Click += new System.EventHandler(this.bMasterKey_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.panel1.Controls.Add(this.bMasterKey);
            this.panel1.Controls.Add(this.tbMasterKey);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(626, 162);
            this.panel1.TabIndex = 2;
            // 
            // EnterMasterKeyForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(647, 187);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "EnterMasterKeyForm";
            this.Text = "Проверка мастер-ключа";
            this.Load += new System.EventHandler(this.EnterMasterKeyForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox tbMasterKey;
        private System.Windows.Forms.Button bMasterKey;
        private System.Windows.Forms.Panel panel1;
    }
}