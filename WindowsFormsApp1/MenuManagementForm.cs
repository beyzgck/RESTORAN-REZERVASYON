
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class MenuManagementForm : Form
    {
        private DataGridView menuGrid;
        private TextBox txtItemName;
        private NumericUpDown nudPrice;
        private Button btnAddMenuItem;
        private Button btnUpdateMenuItem;
        private Button btnRemoveMenuItem;

        public MenuManagementForm()
        {
            DatabaseHelper.InitializeDatabase();
            InitializeMenuControls();
            LoadMenuItems();
        }

        private void InitializeMenuControls()
        {
            this.BackColor = Color.FromArgb(40, 30, 20);
            this.Font = new Font("Georgia", 10);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.Text = "Menü Yönetimi";
            this.ClientSize = new Size(600, 500);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Header Panel
            Panel headerPanel = new Panel
            {
                Size = new Size(560, 60),
                Location = new Point(20, 20),
                BackColor = Color.Transparent
            };
            headerPanel.Paint += (s, pe) =>
            {
                using (LinearGradientBrush brush = new LinearGradientBrush(headerPanel.ClientRectangle, Color.FromArgb(60, 40, 20), Color.FromArgb(80, 50, 30), 90F))
                {
                    pe.Graphics.FillRectangle(brush, headerPanel.ClientRectangle);
                }
            };
            this.Controls.Add(headerPanel);

            // Title Label
            Label lblTitle = new Label
            {
                Text = "Menü Yönetim Paneli",
                Location = new Point(20, 15),
                Size = new Size(200, 30),
                Font = new Font("Georgia", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(200, 160, 0),
                BackColor = Color.Transparent
            };
            headerPanel.Controls.Add(lblTitle);

            // Menu DataGridView
            menuGrid = new DataGridView
            {
                Location = new Point(20, 90),
                Size = new Size(560, 200),
                BackgroundColor = Color.FromArgb(240, 230, 200),
                BorderStyle = BorderStyle.None,
                ColumnHeadersDefaultCellStyle = { Font = new Font("Georgia", 10, FontStyle.Bold), BackColor = Color.FromArgb(60, 40, 20), ForeColor = Color.FromArgb(200, 160, 0), Alignment = DataGridViewContentAlignment.MiddleLeft },
                DefaultCellStyle = { Font = new Font("Georgia", 10), SelectionBackColor = Color.FromArgb(180, 90, 50), SelectionForeColor = Color.FromArgb(240, 230, 200) },
                EnableHeadersVisualStyles = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                RowHeadersVisible = false,
                AllowUserToAddRows = false
            };
            menuGrid.Columns.Add("ItemName", "Ürün Adı");
            menuGrid.Columns.Add("Price", "Fiyat");
            menuGrid.Columns["ItemName"].Width = 350;
            menuGrid.Columns["Price"].Width = 150;
            menuGrid.Columns["Price"].DefaultCellStyle.Format = "C2";
            menuGrid.Columns["Price"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            menuGrid.SelectionChanged += MenuGrid_SelectionChanged;
            this.Controls.Add(menuGrid);

            // Item Input Panel
            Panel itemPanel = new Panel
            {
                Size = new Size(560, 70),
                Location = new Point(20, 300),
                BackColor = Color.FromArgb(60, 40, 20)
            };
            this.Controls.Add(itemPanel);

            Label lblItemName = new Label
            {
                Text = "Ürün Adı:",
                Location = new Point(20, 20),
                Size = new Size(80, 30),
                Font = new Font("Georgia", 12),
                ForeColor = Color.FromArgb(200, 160, 0)
            };
            itemPanel.Controls.Add(lblItemName);

            txtItemName = new TextBox
            {
                Location = new Point(100, 15),
                Size = new Size(200, 35),
                BackColor = Color.FromArgb(240, 230, 200),
                Font = new Font("Georgia", 10)
            };
            itemPanel.Controls.Add(txtItemName);

            Label lblPrice = new Label
            {
                Text = "Fiyat:",
                Location = new Point(320, 20),
                Size = new Size(80, 30),
                Font = new Font("Georgia", 12),
                ForeColor = Color.FromArgb(200, 160, 0)
            };
            itemPanel.Controls.Add(lblPrice);

            nudPrice = new NumericUpDown
            {
                Location = new Point(400, 15),
                Size = new Size(80, 35),
                Minimum = 0.01m,
                Maximum = 1000,
                DecimalPlaces = 2,
                Font = new Font("Georgia", 10),
                BackColor = Color.FromArgb(240, 230, 200)
            };
            itemPanel.Controls.Add(nudPrice);

            // Buttons Panel
            Panel buttonsPanel = new Panel
            {
                Size = new Size(560, 70),
                Location = new Point(20, 390),
                BackColor = Color.FromArgb(60, 40, 20)
            };
            this.Controls.Add(buttonsPanel);

            btnAddMenuItem = new Button
            {
                Text = "Ekle",
                Location = new Point(120, 15),
                Size = new Size(100, 35),
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 0 },
                BackColor = Color.FromArgb(30, 80, 60),
                ForeColor = Color.FromArgb(240, 230, 200),
                Font = new Font("Georgia", 10)
            };
            btnAddMenuItem.Click += BtnAddMenuItem_Click;
            buttonsPanel.Controls.Add(btnAddMenuItem);

            btnUpdateMenuItem = new Button
            {
                Text = "Güncelle",
                Location = new Point(240, 15),
                Size = new Size(100, 35),
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 0 },
                BackColor = Color.FromArgb(180, 90, 50),
                ForeColor = Color.FromArgb(240, 230, 200),
                Font = new Font("Georgia", 10)
            };
            btnUpdateMenuItem.Click += BtnUpdateMenuItem_Click;
            buttonsPanel.Controls.Add(btnUpdateMenuItem);

            btnRemoveMenuItem = new Button
            {
                Text = "Sil",
                Location = new Point(360, 15),
                Size = new Size(100, 35),
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 0 },
                BackColor = Color.FromArgb(150, 30, 30),
                ForeColor = Color.FromArgb(240, 230, 200),
                Font = new Font("Georgia", 10)
            };
            btnRemoveMenuItem.Click += BtnRemoveMenuItem_Click;
            buttonsPanel.Controls.Add(btnRemoveMenuItem);
        }

        private void LoadMenuItems()
        {
            menuGrid.Rows.Clear();
            var menuItems = DatabaseHelper.GetMenuItems();
            foreach (var item in menuItems)
            {
                menuGrid.Rows.Add(item.Key, item.Value);
            }
        }

        private void MenuGrid_SelectionChanged(object sender, EventArgs e)
        {
            if (menuGrid.SelectedRows.Count > 0)
            {
                var selectedRow = menuGrid.SelectedRows[0];
                txtItemName.Text = selectedRow.Cells["ItemName"].Value.ToString();
                nudPrice.Value = Convert.ToDecimal(selectedRow.Cells["Price"].Value);
            }
            else
            {
                txtItemName.Clear();
                nudPrice.Value = 0.01m;
            }
        }

        private void BtnAddMenuItem_Click(object sender, EventArgs e)
        {
            string newItem = txtItemName.Text.Trim();
            decimal newPrice = nudPrice.Value;

            if (string.IsNullOrEmpty(newItem))
            {
                MessageBox.Show("Lütfen bir ürün adı giriniz!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                DatabaseHelper.AddMenuItem(newItem, newPrice);
                LoadMenuItems();
                txtItemName.Clear();
                nudPrice.Value = 0.01m;
                MessageBox.Show($"{newItem} menüye eklendi!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627) // Unique constraint violation
                {
                    MessageBox.Show("Bu ürün zaten menüde mevcut!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show($"Ürün eklenirken hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnUpdateMenuItem_Click(object sender, EventArgs e)
        {
            if (menuGrid.SelectedRows.Count == 0)
            {
                MessageBox.Show("Lütfen güncellemek için bir ürün seçiniz!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string originalItemName = menuGrid.SelectedRows[0].Cells["ItemName"].Value.ToString();
            string newItemName = txtItemName.Text.Trim();
            decimal newPrice = nudPrice.Value;

            if (string.IsNullOrEmpty(newItemName))
            {
                MessageBox.Show("Lütfen bir ürün adı giriniz!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // If the item name has changed, remove the old item and add the new one
                if (originalItemName != newItemName)
                {
                    DatabaseHelper.RemoveMenuItem(originalItemName);
                    DatabaseHelper.AddMenuItem(newItemName, newPrice);
                }
                else
                {
                    DatabaseHelper.UpdateMenuItem(newItemName, newPrice);
                }
                LoadMenuItems();
                txtItemName.Clear();
                nudPrice.Value = 0.01m;
                MessageBox.Show($"{newItemName} güncellendi!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627) // Unique constraint violation
                {
                    MessageBox.Show("Bu ürün adı zaten mevcut!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show($"Ürün güncellenirken hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnRemoveMenuItem_Click(object sender, EventArgs e)
        {
            if (menuGrid.SelectedRows.Count == 0)
            {
                MessageBox.Show("Lütfen silmek için bir ürün seçiniz!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string itemName = menuGrid.SelectedRows[0].Cells["ItemName"].Value.ToString();
            if (MessageBox.Show($"{itemName} menüden silinsin mi?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    DatabaseHelper.RemoveMenuItem(itemName);
                    LoadMenuItems();
                    txtItemName.Clear();
                    nudPrice.Value = 0.01m;
                    MessageBox.Show($"{itemName} menüden silindi!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (SqlException ex)
                {
                    MessageBox.Show($"Ürün silinirken hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }

    public static class DatabaseHelper
    {
        private static readonly string connectionString = "Server=BEYZA\\SQLEXPRESS;Database=AdisyonDB;Integrated Security=True;";

        public static void InitializeDatabase()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Create MenuItems table
                    string createTableQuery = @"
                        IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'MenuItems')
                        BEGIN
                            CREATE TABLE MenuItems (
                                Id INT PRIMARY KEY IDENTITY(1,1),
                                ItemName NVARCHAR(100) NOT NULL UNIQUE,
                                Price DECIMAL(18,2) NOT NULL
                            )
                        END";
                    using (var command = new SqlCommand(createTableQuery, connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    // Insert initial menu items if table is empty
                    string checkDataQuery = "SELECT COUNT(*) FROM MenuItems";
                    using (var command = new SqlCommand(checkDataQuery, connection))
                    {
                        int count = (int)command.ExecuteScalar();
                        if (count == 0)
                        {
                            string insertInitialData = @"
                                INSERT INTO MenuItems (ItemName, Price) VALUES
                                (N' Adana Kebap', 150.00),
                                (N' Meze Tabağı', 80.00),
                                (N' Izgara Köfte', 120.00),
                                (N' Cola', 25.00),
                                (N' Su', 10.00)";
                            using (var insertCommand = new SqlCommand(insertInitialData, connection))
                            {
                                insertCommand.ExecuteNonQuery();
                            }
                        }
                    }
                }
                catch (SqlException ex)
                {
                    MessageBox.Show($"Veritabanı başlatma hatası: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public static Dictionary<string, decimal> GetMenuItems()
        {
            var menu = new Dictionary<string, decimal>();
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT ItemName, Price FROM MenuItems";
                    using (var command = new SqlCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                menu.Add(reader["ItemName"].ToString(), Convert.ToDecimal(reader["Price"]));
                            }
                        }
                    }
                }
                catch (SqlException ex)
                {
                    MessageBox.Show($"Menü öğeleri yüklenirken hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    connection.Close();
                }
            }
            return menu;
        }

        public static void AddMenuItem(string itemName, decimal price)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "INSERT INTO MenuItems (ItemName, Price) VALUES (@ItemName, @Price)";
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ItemName", itemName);
                        command.Parameters.AddWithValue("@Price", price);
                        command.ExecuteNonQuery();
                    }
                }
                catch (SqlException)
                {
                    throw; // Propagate the original exception
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public static void UpdateMenuItem(string itemName, decimal price)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "UPDATE MenuItems SET Price = @Price WHERE ItemName = @ItemName";
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ItemName", itemName);
                        command.Parameters.AddWithValue("@Price", price);
                        command.ExecuteNonQuery();
                    }
                }
                catch (SqlException)
                {
                    throw; // Propagate the original exception
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public static void RemoveMenuItem(string itemName)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "DELETE FROM MenuItems WHERE ItemName = @ItemName";
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ItemName", itemName);
                        command.ExecuteNonQuery();
                    }
                }
                catch (SqlException)
                {
                    throw; // Propagate the original exception
                }
                finally
                {
                    connection.Close();
                }
            }
        }
    }
}