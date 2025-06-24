using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Data.SqlClient; // SQL Server bağlantısı için

namespace WindowsFormsApp1
{
    public partial class LoginForm : Form
    {
        private TextBox txtUsername;
        private TextBox txtPassword;
        private Button btnLogin;
        private Button btnRegister;

        // SQL Server bağlantı dizesi
        private readonly string connectionString = "Server=BEYZA\\SQLEXPRESS;Database=AdisyonDB;Integrated Security=True;";


        public LoginForm()
        {
            InitializeLoginControls();
        }

        private void InitializeLoginControls()
        {
            // Form ayarları
            this.BackColor = Color.FromArgb(40, 30, 20); // Koyu ahşap tema
            this.Font = new Font("Georgia", 11);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.Text = "Restoran Giriş";
            this.ClientSize = new Size(400, 300);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Başlık etiketi
            Label lblTitle = new Label
            {
                Text = "Restoran Yönetim Sistemi",
                Location = new Point(50, 20),
                Size = new Size(300, 30),
                Font = new Font("Georgia", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(200, 160, 0),
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.MiddleCenter
            };
            this.Controls.Add(lblTitle);

            // Kullanıcı adı etiketi
            Label lblUsername = new Label
            {
                Text = "Kullanıcı Adı:",
                Location = new Point(50, 70),
                Size = new Size(100, 30),
                ForeColor = Color.FromArgb(200, 160, 0),
                BackColor = Color.Transparent,
                Font = new Font("Georgia", 12)
            };
            this.Controls.Add(lblUsername);

            // Kullanıcı adı giriş alanı
            txtUsername = new TextBox
            {
                Location = new Point(160, 70),
                Size = new Size(180, 30),
                BackColor = Color.FromArgb(240, 230, 200),
                Font = new Font("Georgia", 11),
                BorderStyle = BorderStyle.None
            };
            txtUsername.Paint += (s, pe) =>
            {
                pe.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (Pen pen = new Pen(Color.FromArgb(200, 160, 0), 2))
                {
                    pe.Graphics.DrawRectangle(pen, 0, 0, txtUsername.Width - 1, txtUsername.Height - 1);
                }
            };
            this.Controls.Add(txtUsername);

            // Şifre etiketi
            Label lblPassword = new Label
            {
                Text = "Şifre:",
                Location = new Point(50, 120),
                Size = new Size(100, 30),
                ForeColor = Color.FromArgb(200, 160, 0),
                BackColor = Color.Transparent,
                Font = new Font("Georgia", 12)
            };
            this.Controls.Add(lblPassword);

            // Şifre giriş alanı
            txtPassword = new TextBox
            {
                Location = new Point(160, 120),
                Size = new Size(180, 30),
                BackColor = Color.FromArgb(240, 230, 200),
                Font = new Font("Georgia", 11),
                BorderStyle = BorderStyle.None,
                UseSystemPasswordChar = true
            };
            txtPassword.Paint += (s, pe) =>
            {
                pe.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (Pen pen = new Pen(Color.FromArgb(200, 160, 0), 2))
                {
                    pe.Graphics.DrawRectangle(pen, 0, 0, txtPassword.Width - 1, txtPassword.Height - 1);
                }
            };
            this.Controls.Add(txtPassword);

            // Giriş butonu
            btnLogin = new Button
            {
                Text = "Giriş Yap",
                Location = new Point(160, 170),
                Size = new Size(100, 35),
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 0 },
                BackColor = Color.FromArgb(30, 80, 60),
                ForeColor = Color.FromArgb(240, 230, 200),
                Font = new Font("Georgia", 11)
            };
            btnLogin.MouseEnter += (s, args) =>
            {
                btnLogin.BackColor = Color.FromArgb(50, 100, 80);
                btnLogin.Size = new Size(105, 37);
            };
            btnLogin.MouseLeave += (s, args) =>
            {
                btnLogin.BackColor = Color.FromArgb(30, 80, 60);
                btnLogin.Size = new Size(100, 35);
            };
            btnLogin.Click += BtnLogin_Click;
            this.Controls.Add(btnLogin);

            // Kayıt butonu
            btnRegister = new Button
            {
                Text = "Kayıt Ol",
                Location = new Point(270, 170),
                Size = new Size(100, 35),
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 0 },
                BackColor = Color.FromArgb(180, 90, 50),
                ForeColor = Color.FromArgb(240, 230, 200),
                Font = new Font("Georgia", 11)
            };
            btnRegister.MouseEnter += (s, args) =>
            {
                btnRegister.BackColor = Color.FromArgb(200, 110, 70);
                btnRegister.Size = new Size(105, 37);
            };
            btnRegister.MouseLeave += (s, args) =>
            {
                btnRegister.BackColor = Color.FromArgb(180, 90, 50);
                btnRegister.Size = new Size(100, 35);
            };
            btnRegister.Click += BtnRegister_Click;
            this.Controls.Add(btnRegister);
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            // Giriş doğrulama
            if (string.IsNullOrEmpty(txtUsername.Text) || string.IsNullOrEmpty(txtPassword.Text))
            {
                MessageBox.Show("Lütfen kullanıcı adı ve şifreyi girin!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Kullanıcı adı ve şifreyi kontrol et
                    string query = "SELECT COUNT(*) FROM adisyon WHERE username = @username AND password = @password";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", txtUsername.Text);
                        cmd.Parameters.AddWithValue("@password", txtPassword.Text); // Not: Gerçek uygulamada şifre hash'lenmeli
                        int userCount = Convert.ToInt32(cmd.ExecuteScalar());

                        if (userCount > 0)
                        {
                            // Giriş başarılı, Form1'i aç
                            Form1 mainForm = new Form1();
                            mainForm.Show();
                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show("Geçersiz kullanıcı adı veya şifre!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Giriş sırasında hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnRegister_Click(object sender, EventArgs e)
        {
            RegisterForm registerForm = new RegisterForm();
            registerForm.ShowDialog();
        }
    }
}