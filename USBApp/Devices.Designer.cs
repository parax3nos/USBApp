namespace USBApp
{
    partial class Devices
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Devices));
            this.dataGridViewDevices = new System.Windows.Forms.DataGridView();
            this.bSave = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDevices)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridViewDevices
            // 
            this.dataGridViewDevices.AllowUserToAddRows = false;
            this.dataGridViewDevices.AllowUserToDeleteRows = false;
            this.dataGridViewDevices.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewDevices.Location = new System.Drawing.Point(12, 12);
            this.dataGridViewDevices.Name = "dataGridViewDevices";
            this.dataGridViewDevices.RowHeadersWidth = 51;
            this.dataGridViewDevices.RowTemplate.Height = 24;
            this.dataGridViewDevices.Size = new System.Drawing.Size(1911, 664);
            this.dataGridViewDevices.TabIndex = 0;
            // 
            // bSave
            // 
            this.bSave.Font = new System.Drawing.Font("Times New Roman", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.bSave.ForeColor = System.Drawing.Color.Black;
            this.bSave.Location = new System.Drawing.Point(12, 682);
            this.bSave.Name = "bSave";
            this.bSave.Size = new System.Drawing.Size(303, 54);
            this.bSave.TabIndex = 1;
            this.bSave.Text = "Сохранить изменения";
            this.bSave.UseVisualStyleBackColor = true;
            this.bSave.Click += new System.EventHandler(this.bSave_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.panel1.Controls.Add(this.bSave);
            this.panel1.Controls.Add(this.dataGridViewDevices);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1935, 746);
            this.panel1.TabIndex = 2;
            // 
            // Devices
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1935, 746);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Devices";
            this.Text = "Устройства";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Devices_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDevices)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewDevices;
        private System.Windows.Forms.Button bSave;
        private System.Windows.Forms.Panel panel1;
    }
}