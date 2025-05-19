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
using BCrypt.Net; // Библиотека хеширования

namespace USBApp
{
    public partial class LoginForm : Form
    {
        private readonly string connectionString = "Server=EVGENY;Database=USBApp;Trusted_Connection=True;"; // Строка подключения

        public LoginForm()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void bEnter_Click(object sender, EventArgs e)
        {
            string login = tbLogin.Text.Trim(); // Поле для ввода логина
            string password = tbPassword.Text.Trim(); // Поле для ввода пароля, не хэшируем здесь

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Введите логин и пароль!");
                return;
            }

            if (login.ToLower() == "master_key")
            {
                MessageBox.Show("Вход под логином master_key запрещён!");
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT Password FROM Users WHERE Login = @Login"; // Берем хэш из базы
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Login", login);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string storedHash = reader["Password"].ToString();
                            if (BCrypt.Net.BCrypt.Verify(password, storedHash)) // Сравниваем введённый пароль с хэшем
                            {
                                this.DialogResult = DialogResult.OK; // Устанавливаем результат диалога
                                this.Close(); // Закрываем форму авторизации
                            }
                            else
                            {
                                MessageBox.Show("Неверный логин или пароль!");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Неверный логин или пароль!");
                        }
                    }
                }
            }
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {

        }
    }
}
