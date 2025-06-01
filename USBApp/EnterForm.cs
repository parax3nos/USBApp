using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using BCrypt.Net;

namespace USBApp
{
    public partial class EnterForm : Form
    {
        private readonly string connectionString = "Server=EVGENY;Database=USBApp;Trusted_Connection=True;";

        public bool BlockNewDevices { get; set; } = true; // По умолчанию блокировать новые устройства
        public EnterForm()
        {
            InitializeComponent();
            cbBlock.Checked = BlockNewDevices; // Set initial state (true by default)
            cbBlock.CheckedChanged += cbBlock_CheckedChanged;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(0, 0); 
        }

        private void buttonDevices_Click(object sender, EventArgs e)
        {
            using (var devicesForm = new Devices(this))
            {
                Devices form = new Devices(this);
                this.Hide();
                var formMain = new Devices(this);
                formMain.Closed += (s, args) => this.Close();
                formMain.Show();
            }
        }

        private void buttonUsers_Click(object sender, EventArgs e)
        {
            Users form = new Users();
            this.Hide();
            var formMain = new Users();
            formMain.Closed += (s, args) => this.Close();
            formMain.Show();
        }

        private bool VerifyMasterKey(string masterKeyPassword)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT Password FROM Users WHERE Login = 'master_key'";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string storedPassword = reader["Password"].ToString();
                            if (!storedPassword.StartsWith("$2a$") && !storedPassword.StartsWith("$2b$"))
                            {
                                return masterKeyPassword == storedPassword;
                            }
                            return BCrypt.Net.BCrypt.Verify(masterKeyPassword, storedPassword);
                        }
                    }
                }
            }
            return false;
        }

        private void cbBlock_CheckedChanged(object sender, EventArgs e)
        {
            if (!cbBlock.Checked)
            {
                using (var masterKeyForm = new EnterMasterKeyForm())
                {
                    if (masterKeyForm.ShowDialog() == DialogResult.OK)
                    {
                        string enteredKey = masterKeyForm.EnteredKey;
                        if (VerifyMasterKey(enteredKey))
                        {
                            BlockNewDevices = false;
                        }
                        else
                        {
                            cbBlock.Checked = true;
                            MessageBox.Show("Неправильно введён мастер-ключ!");
                        }
                    }
                    else
                    {
                        cbBlock.Checked = true;
                    }
                }
            }
            else
            {
                BlockNewDevices = true;
            }
        }

        private void EnterForm_Load(object sender, EventArgs e)
        {

        }

        private void EnterForm_FormClosing(object sender, FormClosingEventArgs e)
        {

        }
    }
}
