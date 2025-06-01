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
    public partial class EnterMasterKeyForm : Form
    {
        public string EnteredKey => tbMasterKey.Text;

        public EnterMasterKeyForm()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void EnterMasterKeyForm_Load(object sender, EventArgs e)
        {

        }

        private void bMasterKey_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
