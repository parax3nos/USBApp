using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace USBApp
{
    public partial class EnterForm : Form
    {
        public bool BlockNewDevices { get; set; } = true; // По умолчанию блокировать новые устройства
        public EnterForm()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(0, 0); // X=100, Y=200
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

        private void EnterForm_Load(object sender, EventArgs e)
        {

        }

        private void EnterForm_FormClosing(object sender, FormClosingEventArgs e)
        {

        }
    }
}
