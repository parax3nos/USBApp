namespace USBApp
{
    partial class Users
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Users));
            this.tbLogin = new System.Windows.Forms.TextBox();
            this.tbPassword = new System.Windows.Forms.TextBox();
            this.tbMasterPassword = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.bDelete = new System.Windows.Forms.Button();
            this.bChange = new System.Windows.Forms.Button();
            this.bAdd = new System.Windows.Forms.Button();
            this.bAllUsers = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tbNewPassword = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbLogin
            // 
            this.tbLogin.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbLogin.Location = new System.Drawing.Point(58, 71);
            this.tbLogin.Name = "tbLogin";
            this.tbLogin.Size = new System.Drawing.Size(405, 43);
            this.tbLogin.TabIndex = 0;
            // 
            // tbPassword
            // 
            this.tbPassword.Font = new System.Drawing.Font("Segoe UI", 16.2F);
            this.tbPassword.Location = new System.Drawing.Point(58, 164);
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.Size = new System.Drawing.Size(405, 43);
            this.tbPassword.TabIndex = 1;
            // 
            // tbMasterPassword
            // 
            this.tbMasterPassword.Font = new System.Drawing.Font("Segoe UI", 16.2F);
            this.tbMasterPassword.Location = new System.Drawing.Point(27, 68);
            this.tbMasterPassword.Name = "tbMasterPassword";
            this.tbMasterPassword.Size = new System.Drawing.Size(405, 43);
            this.tbMasterPassword.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 16.2F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(51, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 38);
            this.label1.TabIndex = 4;
            this.label1.Text = "Логин";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 16.2F, System.Drawing.FontStyle.Bold);
            this.label2.Location = new System.Drawing.Point(51, 123);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(118, 38);
            this.label2.TabIndex = 5;
            this.label2.Text = "Пароль";
            // 
            // bDelete
            // 
            this.bDelete.Font = new System.Drawing.Font("Segoe UI Semibold", 16.2F, System.Drawing.FontStyle.Bold);
            this.bDelete.Location = new System.Drawing.Point(499, 148);
            this.bDelete.Name = "bDelete";
            this.bDelete.Size = new System.Drawing.Size(227, 95);
            this.bDelete.TabIndex = 6;
            this.bDelete.Text = "Удалить пользователя";
            this.bDelete.UseVisualStyleBackColor = true;
            this.bDelete.Click += new System.EventHandler(this.bDelete_Click);
            // 
            // bChange
            // 
            this.bChange.Font = new System.Drawing.Font("Segoe UI Semibold", 16.2F, System.Drawing.FontStyle.Bold);
            this.bChange.Location = new System.Drawing.Point(499, 273);
            this.bChange.Name = "bChange";
            this.bChange.Size = new System.Drawing.Size(227, 95);
            this.bChange.TabIndex = 7;
            this.bChange.Text = "Изменить пароль";
            this.bChange.UseVisualStyleBackColor = true;
            this.bChange.Click += new System.EventHandler(this.bChange_Click);
            // 
            // bAdd
            // 
            this.bAdd.Font = new System.Drawing.Font("Segoe UI Semibold", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.bAdd.Location = new System.Drawing.Point(499, 47);
            this.bAdd.Name = "bAdd";
            this.bAdd.Size = new System.Drawing.Size(227, 95);
            this.bAdd.TabIndex = 8;
            this.bAdd.Text = "Добавить пользователя";
            this.bAdd.UseVisualStyleBackColor = true;
            this.bAdd.Click += new System.EventHandler(this.bAdd_Click);
            // 
            // bAllUsers
            // 
            this.bAllUsers.Font = new System.Drawing.Font("Segoe UI Semibold", 16.2F, System.Drawing.FontStyle.Bold);
            this.bAllUsers.Location = new System.Drawing.Point(458, 27);
            this.bAllUsers.Name = "bAllUsers";
            this.bAllUsers.Size = new System.Drawing.Size(262, 101);
            this.bAllUsers.TabIndex = 9;
            this.bAllUsers.Text = "Посмотреть всех пользователей";
            this.bAllUsers.UseVisualStyleBackColor = true;
            this.bAllUsers.Click += new System.EventHandler(this.bAllUsers_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.tbNewPassword);
            this.panel1.Controls.Add(this.bDelete);
            this.panel1.Controls.Add(this.bChange);
            this.panel1.Controls.Add(this.bAdd);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.tbLogin);
            this.panel1.Controls.Add(this.tbPassword);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(796, 594);
            this.panel1.TabIndex = 10;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.panel2.Controls.Add(this.bAllUsers);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.tbMasterPassword);
            this.panel2.Location = new System.Drawing.Point(32, 421);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(735, 150);
            this.panel2.TabIndex = 13;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 16.2F, System.Drawing.FontStyle.Bold);
            this.label3.Location = new System.Drawing.Point(20, 27);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(198, 38);
            this.label3.TabIndex = 10;
            this.label3.Text = "Мастер-ключ";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 16.2F, System.Drawing.FontStyle.Bold);
            this.label4.Location = new System.Drawing.Point(51, 258);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(213, 38);
            this.label4.TabIndex = 12;
            this.label4.Text = "Новый пароль";
            // 
            // tbNewPassword
            // 
            this.tbNewPassword.Font = new System.Drawing.Font("Segoe UI", 16.2F);
            this.tbNewPassword.Location = new System.Drawing.Point(58, 299);
            this.tbNewPassword.Name = "tbNewPassword";
            this.tbNewPassword.Size = new System.Drawing.Size(405, 43);
            this.tbNewPassword.TabIndex = 11;
            // 
            // Users
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(796, 594);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Users";
            this.Text = "Управление учетными записями";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Users_FormClosing);
            this.Load += new System.EventHandler(this.Users_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox tbLogin;
        private System.Windows.Forms.TextBox tbPassword;
        private System.Windows.Forms.TextBox tbMasterPassword;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button bDelete;
        private System.Windows.Forms.Button bChange;
        private System.Windows.Forms.Button bAdd;
        private System.Windows.Forms.Button bAllUsers;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbNewPassword;
        private System.Windows.Forms.Panel panel2;
    }
}