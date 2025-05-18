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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using BCrypt.Net; // Библиотека хеширования

namespace USBApp
{
    public partial class Users : Form
    {
        private readonly string connectionString = "Server=EVGENY;Database=USBApp;Trusted_Connection=True;"; // Строка подключения
        public Users()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
        }
        private void Users_Load(object sender, EventArgs e)
        {
            
        }

        // Проверка мастер-ключа
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
                            // Если это не хэш (например, открытый пароль), сравниваем напрямую
                            if (!storedPassword.StartsWith("$2a$") && !storedPassword.StartsWith("$2b$"))
                            {
                                return masterKeyPassword == storedPassword;
                            }
                            // Иначе проверяем как хэш
                            return BCrypt.Net.BCrypt.Verify(masterKeyPassword, storedPassword);
                        }
                    }
                }
            }
            return false;
        }

        // Кнопка добавления пользователя
        private void bAdd_Click(object sender, EventArgs e)
        {
            string masterKeyPassword = tbMasterPassword.Text; // Поле для ввода мастер-ключа
            string login = tbLogin.Text; // Поле для ввода логина
            string password = tbPassword.Text; // Поле для ввода пароля

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(masterKeyPassword))
            {
                MessageBox.Show("Все поля должны быть заполнены!");
                return;
            }

            if (!VerifyMasterKey(masterKeyPassword))
            {
                MessageBox.Show("Неправильно введён мастер-ключ!");
                return;
            }

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(password); // Хэшируем пароль

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string insertQuery = "INSERT INTO Users (Login, Password) VALUES (@Login, @Password)";
                using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@Login", login);
                    cmd.Parameters.AddWithValue("@Password", passwordHash);
                    try
                    {
                        cmd.ExecuteNonQuery();
                        tbLogin.Clear();
                        tbPassword.Clear();
                        tbNewPassword.Clear();
                        tbMasterPassword.Clear();
                        MessageBox.Show("Пользователь успешно добавлен!");
                    }
                    catch (SqlException ex)
                    {
                        if (ex.Number == 2627) // Нарушение уникальности (логин уже существует)
                        {
                            tbLogin.Clear();
                            tbPassword.Clear();
                            tbNewPassword.Clear();
                            tbMasterPassword.Clear();
                            MessageBox.Show("Пользователь с таким логином уже существует!");
                        }
                        else
                            MessageBox.Show("Ошибка добавления: " + ex.Message);
                    }
                }
            }
        }

        // Кнопка удаления пользователя
        private void bDelete_Click(object sender, EventArgs e)
        {
            string masterKeyPassword = tbMasterPassword.Text;
            string login = tbLogin.Text;
            string password = tbPassword.Text;

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(masterKeyPassword))
            {
                MessageBox.Show("Все поля должны быть заполнены!");
                return;
            }

            if (!VerifyMasterKey(masterKeyPassword))
            {
                MessageBox.Show("Неправильно введён мастер-ключ!");
                return;
            }

            if (login == "master_key")
            {
                MessageBox.Show("Нельзя удалить пользователя с логином master_key!");
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string selectQuery = "SELECT Password FROM Users WHERE Login = @Login";
                string storedHash = null;
                using (SqlCommand cmd = new SqlCommand(selectQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@Login", login);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            storedHash = reader["Password"].ToString();
                        }
                    }
                }

                if (storedHash != null && BCrypt.Net.BCrypt.Verify(password, storedHash))
                {
                    string deleteQuery = "DELETE FROM Users WHERE Login = @Login";
                    using (SqlCommand cmd = new SqlCommand(deleteQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@Login", login);
                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            tbLogin.Clear();
                            tbPassword.Clear();
                            tbNewPassword.Clear();
                            tbMasterPassword.Clear();
                            MessageBox.Show("Пользователь успешно удалён!");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Пользователь с таким логином и паролем не найден!");
                }
            }
        }

        // Кнопка изменения пароля
        private void bChange_Click(object sender, EventArgs e)
        {
            string masterKeyPassword = tbMasterPassword.Text;
            string login = tbLogin.Text;
            string oldPassword = tbPassword.Text;
            string newPassword = tbNewPassword.Text;

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(oldPassword) || string.IsNullOrEmpty(newPassword) || string.IsNullOrEmpty(masterKeyPassword))
            {
                MessageBox.Show("Все поля должны быть заполнены!");
                return;
            }

            if (!VerifyMasterKey(masterKeyPassword))
            {
                MessageBox.Show("Неправильно введён мастер-ключ!");
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string selectQuery = "SELECT Password FROM Users WHERE Login = @Login";
                string storedPassword = null;
                using (SqlCommand cmd = new SqlCommand(selectQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@Login", login);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            storedPassword = reader["Password"].ToString();
                        }
                    }
                }

                if (storedPassword != null)
                {
                    bool isPasswordValid = storedPassword.StartsWith("$2a$") || storedPassword.StartsWith("$2b$")
                        ? BCrypt.Net.BCrypt.Verify(oldPassword, storedPassword)
                        : oldPassword == storedPassword;

                    if (isPasswordValid)
                    {
                        string newPasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
                        string updateQuery = "UPDATE Users SET Password = @NewPassword WHERE Login = @Login";
                        using (SqlCommand cmd = new SqlCommand(updateQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@Login", login);
                            cmd.Parameters.AddWithValue("@NewPassword", newPasswordHash);
                            int rowsAffected = cmd.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                tbLogin.Clear();
                                tbPassword.Clear();
                                tbNewPassword.Clear();
                                tbMasterPassword.Clear();
                                MessageBox.Show("Пароль успешно изменён!");
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Пользователь с таким логином и старым паролем не найден!");
                    }
                }
                else
                {
                    MessageBox.Show("Пользователь с таким логином не найден!");
                }
            }
        }

        // Кнопка просмотра пользователей (только проверка мастер-ключа)
        private void bAllUsers_Click(object sender, EventArgs e)
        {
            string masterKeyPassword = tbMasterPassword.Text; // Поле для ввода мастер-ключа

            if (string.IsNullOrEmpty(masterKeyPassword))
            {
                MessageBox.Show("Введите мастер-ключ!");
                return;
            }

            if (!VerifyMasterKey(masterKeyPassword))
            {
                MessageBox.Show("Неправильно введён мастер-ключ!");
                return;
            }

            tbLogin.Clear();
            tbPassword.Clear();
            tbNewPassword.Clear();
            tbMasterPassword.Clear();
            AllUsers.allUsers.ShowForm();
        }

        private static Users usr;
        public static Users users
        {
            get
            {
                if (usr == null || usr.IsDisposed) usr = new Users();
                return usr;
            }
        }
        public void ShowForm()
        {
            Show();
            Activate();
        }

        private void Users_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = MessageBox.Show("Вы хотите закрыть программу?", "Внимание", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes;
        }
    }
}
