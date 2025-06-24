using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Data.SqlClient; // SQL Server bağlantısı için

namespace WindowsFormsApp1
{
    public partial class RegisterForm : Form
    {
        private TextBox txtUsername;
        private TextBox txtPassword;
        private TextBox txtConfirmPassword;
        private Button btnRegister;
        private Button btnCancel;

        // SQL Server bağlantı dizesi
        private readonly string connectionString = "Server=BEYZA\\SQLEXPRESS;Database=AdisyonDB;Integrated Security=True;";

        public RegisterForm()
        {
            InitializeRegisterControls();
        }

        private void InitializeRegisterControls()
        {
            // Form ayarları
            this.BackColor = Color.FromArgb(40, 30, 20); // Koyu ahşap tema
            this.Font = new Font("Georgia", 11);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.Text = "Restoran Kayıt";
            this.ClientSize = new Size(400, 350);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Başlık etiketi
            Label lblTitle = new Label
            {
                Text = "Yeni Kullanıcı Kaydı",
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

            // Şifre doğrulama etiketi
            Label lblConfirmPassword = new Label
            {
                Text = "Şifre Doğrula:",
                Location = new Point(50, 170),
                Size = new Size(100, 30),
                ForeColor = Color.FromArgb(200, 160, 0),
                BackColor = Color.Transparent,
                Font = new Font("Georgia", 12)
            };
            this.Controls.Add(lblConfirmPassword);

            // Şifre doğrulama giriş alanı
            txtConfirmPassword = new TextBox
            {
                Location = new Point(160, 170),
                Size = new Size(180, 30),
                BackColor = Color.FromArgb(240, 230, 200),
                Font = new Font("Georgia", 11),
                BorderStyle = BorderStyle.None,
                UseSystemPasswordChar = true
            };
            txtConfirmPassword.Paint += (s, pe) =>
            {
                pe.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (Pen pen = new Pen(Color.FromArgb(200, 160, 0), 2))
                {
                    pe.Graphics.DrawRectangle(pen, 0, 0, txtConfirmPassword.Width - 1, txtConfirmPassword.Height - 1);
                }
            };
            this.Controls.Add(txtConfirmPassword);

            // Kayıt butonu
            btnRegister = new Button
            {
                Text = "Kayıt Ol",
                Location = new Point(160, 220),
                Size = new Size(100, 35),
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 0 },
                BackColor = Color.FromArgb(30, 80, 60),
                ForeColor = Color.FromArgb(240, 230, 200),
                Font = new Font("Georgia", 11)
            };
            btnRegister.MouseEnter += (s, args) =>
            {
                btnRegister.BackColor = Color.FromArgb(50, 100, 80);
                btnRegister.Size = new Size(105, 37);
            };
            btnRegister.MouseLeave += (s, args) =>
            {
                btnRegister.BackColor = Color.FromArgb(30, 80, 60);
                btnRegister.Size = new Size(100, 35);
            };
            btnRegister.Click += BtnRegister_Click;
            this.Controls.Add(btnRegister);

            // İptal butonu
            btnCancel = new Button
            {
                Text = "İptal",
                Location = new Point(270, 220),
                Size = new Size(100, 35),
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 0 },
                BackColor = Color.FromArgb(180, 90, 50),
                ForeColor = Color.FromArgb(240, 230, 200),
                Font = new Font("Georgia", 11)
            };
            btnCancel.MouseEnter += (s, args) =>
            {
                btnCancel.BackColor = Color.FromArgb(200, 110, 70);
                btnCancel.Size = new Size(105, 37);
            };
            btnCancel.MouseLeave += (s, args) =>
            {
                btnCancel.BackColor = Color.FromArgb(180, 90, 50);
                btnCancel.Size = new Size(100, 35);
            };
            btnCancel.Click += BtnCancel_Click;
            this.Controls.Add(btnCancel);
        }

        private void BtnRegister_Click(object sender, EventArgs e)
        {
            // Giriş doğrulama
            if (string.IsNullOrEmpty(txtUsername.Text) || string.IsNullOrEmpty(txtPassword.Text) || string.IsNullOrEmpty(txtConfirmPassword.Text))
            {
                MessageBox.Show("Lütfen tüm alanları doldurun!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (txtPassword.Text != txtConfirmPassword.Text)
            {
                MessageBox.Show("Şifreler eşleşmiyor!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (txtUsername.Text.Length < 4 || txtPassword.Text.Length < 4)
            {
                MessageBox.Show("Kullanıcı adı ve şifre en az 4 karakter olmalıdır!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Kullanıcı adının zaten var olup olmadığını kontrol et
                    string checkQuery = "SELECT COUNT(*) FROM adisyon WHERE username = @username";
                    using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@username", txtUsername.Text);
                        int userCount = Convert.ToInt32(checkCmd.ExecuteScalar());
                        if (userCount > 0)
                        {
                            MessageBox.Show("Bu kullanıcı adı zaten alınmış!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    // Kullanıcıyı kaydet
                    string insertQuery = "INSERT INTO adisyon (username, password) VALUES (@username, @password)";
                    using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", txtUsername.Text);
                        cmd.Parameters.AddWithValue("@password", txtPassword.Text); // Not: Gerçek uygulamada şifreyi hash'leyin
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show($"Kayıt başarılı! Kullanıcı adı: {txtUsername.Text}", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Kayıt sırasında hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}