using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace USBApp
{
    public partial class AllUsers : Form
    {
        private readonly string connectionString = "Server=EVGENY;Database=USBApp;Trusted_Connection=True;"; // Строка подключения
        private DataTable usersTable;

        public AllUsers()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.Manual;

            var screen = Screen.PrimaryScreen.WorkingArea; // Рабочая область (без панели задач)
            this.Location = new Point(
                screen.Right - this.Width,
                (screen.Height - this.Height) / 2);
            InitializeDataGridView();
            LoadUsers();
        }

        private void InitializeDataGridView()
        {
            dataGridViewUsers.AutoGenerateColumns = false;
            dataGridViewUsers.ReadOnly = true; // Только для просмотра

            // Создание столбцов
            var idColumn = new DataGridViewTextBoxColumn
            {
                Name = "Id_user",
                HeaderText = "ID",
                DataPropertyName = "Id_user",
                Width = 50
            };
            idColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            idColumn.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewUsers.Columns.Add(idColumn);

            var loginColumn = new DataGridViewTextBoxColumn
            {
                Name = "Login",
                HeaderText = "Логин",
                DataPropertyName = "Login",
                Width = 150
            };
            loginColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            loginColumn.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewUsers.Columns.Add(loginColumn);

            var passwordColumn = new DataGridViewTextBoxColumn
            {
                Name = "Password",
                HeaderText = "Пароль",
                DataPropertyName = "Password",
                Width = 150
            };
            passwordColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            passwordColumn.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewUsers.Columns.Add(passwordColumn);

            // Создание DataTable
            usersTable = new DataTable();
            usersTable.Columns.Add("Id_user", typeof(int));
            usersTable.Columns.Add("Login", typeof(string));
            usersTable.Columns.Add("Password", typeof(string));

            dataGridViewUsers.DataSource = usersTable;
        }

        private void LoadUsers()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string selectQuery = "SELECT Id_user, Login, Password FROM Users WHERE Login != 'master_key'";
                using (SqlDataAdapter adapter = new SqlDataAdapter(selectQuery, conn))
                {
                    usersTable.Clear();
                    adapter.Fill(usersTable);
                }
            }
        }

        private void AllUsers_Load(object sender, EventArgs e)
        {

        }

        private static AllUsers alusr;
        public static AllUsers allUsers
        {
            get
            {
                if (alusr == null || alusr.IsDisposed) alusr = new AllUsers();
                return alusr;
            }
        }
        public void ShowForm()
        {
            Show();
            Activate();
        }
    }
}
