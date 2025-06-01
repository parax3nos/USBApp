namespace USBApp
{
    partial class EnterForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EnterForm));
            this.buttonDevices = new System.Windows.Forms.Button();
            this.buttonUsers = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.radioButtonBlock = new System.Windows.Forms.RadioButton();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonDevices
            // 
            this.buttonDevices.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.buttonDevices.Font = new System.Drawing.Font("Yu Gothic UI Semibold", 16.2F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonDevices.Location = new System.Drawing.Point(133, 99);
            this.buttonDevices.Name = "buttonDevices";
            this.buttonDevices.Size = new System.Drawing.Size(245, 93);
            this.buttonDevices.TabIndex = 0;
            this.buttonDevices.Text = "Управление устройствами";
            this.buttonDevices.UseVisualStyleBackColor = true;
            this.buttonDevices.Click += new System.EventHandler(this.buttonDevices_Click);
            // 
            // buttonUsers
            // 
            this.buttonUsers.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.buttonUsers.Font = new System.Drawing.Font("Yu Gothic UI Semibold", 16.2F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.buttonUsers.Location = new System.Drawing.Point(96, 229);
            this.buttonUsers.Name = "buttonUsers";
            this.buttonUsers.Size = new System.Drawing.Size(315, 109);
            this.buttonUsers.TabIndex = 1;
            this.buttonUsers.Text = "Управление учетными записями";
            this.buttonUsers.UseVisualStyleBackColor = true;
            this.buttonUsers.Click += new System.EventHandler(this.buttonUsers_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.panel1.Controls.Add(this.radioButtonBlock);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.buttonDevices);
            this.panel1.Controls.Add(this.buttonUsers);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(486, 368);
            this.panel1.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.label1.Font = new System.Drawing.Font("Yu Gothic UI", 28.2F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(158, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(185, 62);
            this.label1.TabIndex = 2;
            this.label1.Text = "ВЫБОР";
            // 
            // radioButtonBlock
            // 
            this.radioButtonBlock.AutoSize = true;
            this.radioButtonBlock.Font = new System.Drawing.Font("Yu Gothic UI Light", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.radioButtonBlock.Location = new System.Drawing.Point(146, 196);
            this.radioButtonBlock.Name = "radioButtonBlock";
            this.radioButtonBlock.Size = new System.Drawing.Size(232, 24);
            this.radioButtonBlock.TabIndex = 3;
            this.radioButtonBlock.TabStop = true;
            this.radioButtonBlock.Text = "Блокировать новые устройства";
            this.radioButtonBlock.UseVisualStyleBackColor = true;
            // 
            // EnterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(486, 368);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "EnterForm";
            this.Text = "Выбор режима работы";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.EnterForm_FormClosing);
            this.Load += new System.EventHandler(this.EnterForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonDevices;
        private System.Windows.Forms.Button buttonUsers;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton radioButtonBlock;
    }
}