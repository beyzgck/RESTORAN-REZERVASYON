using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private FlowLayoutPanel tablePanel;
        private TextBox txtTableCount;
        private Button btnAddTables;
        private Button btnMenuManagement;
        private Dictionary<string, bool> tableStatus;
        private Dictionary<string, List<DataGridViewRow>> tableOrders;
        private Dictionary<string, decimal> tableTotals;

        public Form1()
        {
            InitializeComponent();
            InitializeMainControls();
            tableStatus = new Dictionary<string, bool>();
            tableOrders = new Dictionary<string, List<DataGridViewRow>>();
            tableTotals = new Dictionary<string, decimal>();
        }

        private void InitializeMainControls()
        {
            this.BackColor = Color.FromArgb(40, 30, 20); // Dark wood
            this.Font = new Font("Georgia", 10);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.Text = "Restoran Adisyon Sistemi";
            this.ClientSize = new Size(1000, 600);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Header Panel
            Panel headerPanel = new Panel
            {
                Size = new Size(960, 80),
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
                Text = "Restoran Yönetim Paneli",
                Location = new Point(20, 20),
                Size = new Size(300, 40),
                Font = new Font("Georgia", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(200, 160, 0),
                BackColor = Color.Transparent
            };
            headerPanel.Controls.Add(lblTitle);

            // Table Count Input
            Label lblTableCount = new Label
            {
                Text = "Masa Sayısı:",
                Location = new Point(650, 25),
                Size = new Size(100, 30),
                Font = new Font("Georgia", 12),
                ForeColor = Color.FromArgb(200, 160, 0),
                BackColor = Color.Transparent
            };
            headerPanel.Controls.Add(lblTableCount);

            txtTableCount = new TextBox
            {
                Location = new Point(760, 25),
                Size = new Size(80, 30),
                Text = "1",
                BackColor = Color.FromArgb(240, 230, 200),
                Font = new Font("Georgia", 10),
                BorderStyle = BorderStyle.None
            };
            txtTableCount.Paint += (s, pe) =>
            {
                pe.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (Pen pen = new Pen(Color.FromArgb(200, 160, 0), 2))
                {
                    pe.Graphics.DrawRectangle(pen, 0, 0, txtTableCount.Width - 1, txtTableCount.Height - 1);
                }
            };
            headerPanel.Controls.Add(txtTableCount);

            // Add Tables Button
            btnAddTables = new Button
            {
                Text = "Masaları Ekle",
                Location = new Point(860, 25),
                Size = new Size(100, 30),
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 0 },
                BackColor = Color.FromArgb(30, 80, 60),
                ForeColor = Color.FromArgb(240, 230, 200),
                Font = new Font("Georgia", 10)
            };
            btnAddTables.MouseEnter += (s, args) =>
            {
                btnAddTables.BackColor = Color.FromArgb(50, 100, 80);
                btnAddTables.Size = new Size(105, 32);
            };
            btnAddTables.MouseLeave += (s, args) =>
            {
                btnAddTables.BackColor = Color.FromArgb(30, 80, 60);
                btnAddTables.Size = new Size(100, 30);
            };
            btnAddTables.Click += BtnAddTables_Click;
            headerPanel.Controls.Add(btnAddTables);

            // Menu Management Button
            btnMenuManagement = new Button
            {
                Text = "Menü Yönetimi",
                Location = new Point(540, 25),
                Size = new Size(100, 30),
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 0 },
                BackColor = Color.FromArgb(30, 80, 60),
                ForeColor = Color.FromArgb(240, 230, 200),
                Font = new Font("Georgia", 10)
            };
            btnMenuManagement.MouseEnter += (s, args) =>
            {
                btnMenuManagement.BackColor = Color.FromArgb(50, 100, 80);
                btnMenuManagement.Size = new Size(105, 32);
            };
            btnMenuManagement.MouseLeave += (s, args) =>
            {
                btnMenuManagement.BackColor = Color.FromArgb(30, 80, 60);
                btnMenuManagement.Size = new Size(100, 30);
            };
            btnMenuManagement.Click += BtnMenuManagement_Click;
            headerPanel.Controls.Add(btnMenuManagement);

            // Table Panel
            tablePanel = new FlowLayoutPanel
            {
                Location = new Point(20, 110),
                Size = new Size(960, 470),
                AutoScroll = true,
                BackColor = Color.FromArgb(40, 30, 20),
                BorderStyle = BorderStyle.FixedSingle
            };
            this.Controls.Add(tablePanel);
        }

        private void BtnAddTables_Click(object sender, EventArgs ev)
        {
            if (!int.TryParse(txtTableCount.Text, out int tableCount) || tableCount < 1)
            {
                MessageBox.Show("Geçerli bir masa sayısı giriniz!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            tablePanel.Controls.Clear();
            tableStatus.Clear();
            tableOrders.Clear();
            tableTotals.Clear();

            for (int i = 1; i <= tableCount; i++)
            {
                string tableName = $"Masa {i}";
                tableStatus[tableName] = false;
                tableOrders[tableName] = new List<DataGridViewRow>();
                tableTotals[tableName] = 0m;

                Button btnTable = new Button
                {
                    Text = $"{tableName}\nSipariş: 0\nToplam: 0 TL",
                    Size = new Size(120, 80),
                    FlatStyle = FlatStyle.Flat,
                    FlatAppearance = { BorderSize = 0 },
                    BackColor = Color.FromArgb(240, 230, 200),
                    ForeColor = Color.FromArgb(40, 30, 20),
                    Font = new Font("Georgia", 10, FontStyle.Bold),
                    Margin = new Padding(10),
                    Tag = tableName,
                    TextAlign = ContentAlignment.MiddleCenter
                };
                btnTable.Paint += (s, pe) =>
                {
                    Button btn = (Button)s;
                    Graphics g = pe.Graphics;
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    Color baseColor = tableStatus[btn.Tag.ToString()] ? Color.FromArgb(180, 90, 50) : Color.FromArgb(240, 230, 200);
                    Color hoverColor = tableStatus[btn.Tag.ToString()] ? Color.FromArgb(200, 110, 70) : Color.FromArgb(255, 245, 220);

                    using (GraphicsPath path = new GraphicsPath())
                    {
                        RectangleF rect = new RectangleF(2, 2, btn.Width - 5, btn.Height - 5);
                        float radius = 10;
                        path.AddArc(rect.X, rect.Y, radius * 2, radius * 2, 180, 90);
                        path.AddArc(rect.Width - radius * 2, rect.Y, radius * 2, radius * 2, 270, 90);
                        path.AddArc(rect.Width - radius * 2, rect.Height - radius * 2, radius * 2, radius * 2, 0, 90);
                        path.AddArc(rect.X, rect.Height - radius * 2, radius * 2, radius * 2, 90, 90);
                        path.CloseFigure();

                        using (LinearGradientBrush brush = new LinearGradientBrush(btn.ClientRectangle, baseColor, hoverColor, 45F))
                        {
                            g.FillPath(brush, path);
                        }
                        using (Pen pen = new Pen(Color.FromArgb(200, 160, 0), 2))
                        {
                            g.DrawPath(pen, path);
                        }
                    }

                    if (tableOrders[btn.Tag.ToString()].Count > 0)
                    {
                        using (SolidBrush badgeBrush = new SolidBrush(Color.FromArgb(150, 30, 30)))
                        {
                            g.FillEllipse(badgeBrush, btn.Width - 25, 5, 20, 20);
                        }
                        TextRenderer.DrawText(g, tableOrders[btn.Tag.ToString()].Count.ToString(), new Font("Georgia", 8, FontStyle.Bold), new Rectangle(btn.Width - 25, 5, 20, 20), Color.White, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                    }
                    TextRenderer.DrawText(g, btn.Text, btn.Font, new Rectangle(0, 0, btn.Width, btn.Height), btn.ForeColor, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                };
                btnTable.MouseEnter += (s, args) =>
                {
                    btnTable.Size = new Size(125, 85);
                    btnTable.Invalidate();
                };
                btnTable.MouseLeave += (s, args) =>
                {
                    btnTable.Size = new Size(120, 80);
                    btnTable.Invalidate();
                };
                btnTable.Click += (s, args) =>
                {
                    TableOrderForm orderForm = new TableOrderForm(btnTable.Tag.ToString(), tableOrders[btnTable.Tag.ToString()], tableTotals[btnTable.Tag.ToString()]);
                    orderForm.OrderChanged += (isOccupied, orders, total) =>
                    {
                        tableStatus[btnTable.Tag.ToString()] = isOccupied;
                        tableOrders[btnTable.Tag.ToString()] = orders;
                        tableTotals[btnTable.Tag.ToString()] = total;
                        btnTable.Text = $"{tableName}\nSipariş: {orders.Count}\nToplam: {total:C2}";
                        btnTable.Invalidate();
                    };
                    orderForm.ShowDialog();
                };
                tablePanel.Controls.Add(btnTable);
            }
        }

        private void BtnMenuManagement_Click(object sender, EventArgs e)
        {
            MenuManagementForm menuForm = new MenuManagementForm();
            menuForm.ShowDialog();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            txtTableCount.Text = "5";
            BtnAddTables_Click(null, null);
        }
    }

    public partial class TableOrderForm : Form
    {
        private string tableName;
        private DataGridView orderGrid;
        private Label lblTotal;
        private ComboBox cmbMenuItems;
        private NumericUpDown nudQuantity;
        private Button btnAddItem;
        private Button btnRemoveItem;
        private Button btnPrintBill;
        private TextBox txtBill;
        private List<DataGridViewRow> orders;
        private decimal total;
        private Dictionary<string, decimal> menuItems;

        public delegate void OrderChangedEventHandler(bool isOccupied, List<DataGridViewRow> orders, decimal total);
        public event OrderChangedEventHandler OrderChanged;

        public TableOrderForm(string tableName, List<DataGridViewRow> existingOrders, decimal existingTotal)
        {
            this.tableName = tableName;
            this.orders = existingOrders;
            this.total = existingTotal;
            this.menuItems = DatabaseHelper.GetMenuItems();
            InitializeOrderControls();
            LoadExistingOrders();
        }

        private void InitializeOrderControls()
        {
            this.BackColor = Color.FromArgb(40, 30, 20);
            this.Font = new Font("Georgia", 10);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.Text = $"{tableName} - Adisyon";
            this.ClientSize = new Size(800, 500);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Header Panel
            Panel headerPanel = new Panel
            {
                Size = new Size(760, 60),
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

            // Table Name Label
            Label lblTable = new Label
            {
                Text = $"Masa: {tableName}",
                Location = new Point(20, 15),
                Size = new Size(200, 30),
                Font = new Font("Georgia", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(200, 160, 0),
                BackColor = Color.Transparent
            };
            headerPanel.Controls.Add(lblTable);

            // Order DataGridView
            orderGrid = new DataGridView
            {
                Location = new Point(20, 90),
                Size = new Size(760, 200),
                BackgroundColor = Color.FromArgb(240, 230, 200),
                BorderStyle = BorderStyle.None,
                ColumnHeadersDefaultCellStyle = { Font = new Font("Georgia", 10, FontStyle.Bold), BackColor = Color.FromArgb(60, 40, 20), ForeColor = Color.FromArgb(200, Color.White), Alignment = DataGridViewContentAlignment.MiddleLeft },
                DefaultCellStyle = { Font = new Font("Georgia", 10), SelectionBackColor = Color.FromArgb(180, 90, 50), SelectionForeColor = Color.White },
                EnableHeadersVisualStyles = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                RowHeadersVisible = false
            };
            orderGrid.Columns.Add("Item", "Ürün");
            orderGrid.Columns.Add("Quantity", "Adet");
            orderGrid.Columns.Add("Price", "Fiyat");
            orderGrid.Columns["Item"].Width = 450;
            orderGrid.Columns["Quantity"].Width = 100;
            orderGrid.Columns["Price"].Width = 150;
            orderGrid.Columns["Price"].DefaultCellStyle.Format = "C2";
            orderGrid.Columns["Price"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            this.Controls.Add(orderGrid);

            // Menu Selection Panel
            Panel menuPanel = new Panel
            {
                Size = new Size(760, 70),
                Location = new Point(20, 300),
                BackColor = Color.FromArgb(60, 40, 20)
            };
            this.Controls.Add(menuPanel);

            // Menu Selection
            Label lblMenu = new Label
            {
                Text = "Menü:",
                Location = new Point(20, 20),
                Size = new Size(80, 30),
                Font = new Font("Georgia", 12),
                ForeColor = Color.FromArgb(200, 160, 0),
                BackColor = Color.Transparent
            };
            menuPanel.Controls.Add(lblMenu);

            cmbMenuItems = new ComboBox
            {
                Location = new Point(100, 15),
                Size = new Size(400, 35),
                DropDownStyle = ComboBoxStyle.DropDownList,
                BackColor = Color.FromArgb(240, 230, 200),
                Font = new Font("Georgia", 10),
                DropDownHeight = 200
            };
            foreach (var item in menuItems.Keys)
            {
                cmbMenuItems.Items.Add(item);
            }
            menuPanel.Controls.Add(cmbMenuItems);

            // Quantity Input
            Label lblQuantity = new Label
            {
                Text = "Adet:",
                Location = new Point(520, 20),
                Size = new Size(80, 30),
                Font = new Font("Georgia", 12),
                ForeColor = Color.FromArgb(200, 160, 0),
                BackColor = Color.Transparent
            };
            menuPanel.Controls.Add(lblQuantity);

            nudQuantity = new NumericUpDown
            {
                Location = new Point(600, 15),
                Size = new Size(80, 35),
                Minimum = 1,
                Maximum = 100,
                Font = new Font("Georgia", 10),
                BackColor = Color.FromArgb(240, 230, 200)
            };
            menuPanel.Controls.Add(nudQuantity);

            // Bill Display
            txtBill = new TextBox
            {
                Location = new Point(20, 380),
                Size = new Size(380, 70),
                Multiline = true,
                ReadOnly = true,
                BackColor = Color.FromArgb(240, 230, 200),
                Font = new Font("Georgia", 10),
                ScrollBars = ScrollBars.Vertical
            };
            this.Controls.Add(txtBill);

            // Total Label
            lblTotal = new Label
            {
                Text = "Toplam: 0 TL",
                Location = new Point(400, 380),
                Size = new Size(200, 30),
                Font = new Font("Georgia", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(200, 160, 0)
            };
            this.Controls.Add(lblTotal);

            // Add Item Button
            btnAddItem = new Button
            {
                Text = "Ekle",
                Location = new Point(500, 415),
                Size = new Size(80, 35),
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 0 },
                BackColor = Color.FromArgb(30, 80, 60),
                ForeColor = Color.FromArgb(240, 230, 200),
                Font = new Font("Georgia", 10)
            };
            btnAddItem.MouseEnter += (s, args) =>
            {
                btnAddItem.BackColor = Color.FromArgb(50, 100, 80);
                btnAddItem.Size = new Size(85, 37);
            };
            btnAddItem.MouseLeave += (s, args) =>
            {
                btnAddItem.BackColor = Color.FromArgb(30, 80, 60);
                btnAddItem.Size = new Size(80, 35);
            };
            btnAddItem.Click += BtnAddItem_Click;
            this.Controls.Add(btnAddItem);

            // Remove Item Button
            btnRemoveItem = new Button
            {
                Text = "Sil",
                Location = new Point(590, 415),
                Size = new Size(80, 35),
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 0 },
                BackColor = Color.FromArgb(150, 30, 30),
                ForeColor = Color.FromArgb(240, 230, 200),
                Font = new Font("Georgia", 10)
            };
            btnRemoveItem.MouseEnter += (s, args) =>
            {
                btnRemoveItem.BackColor = Color.FromArgb(170, 50, 50);
                btnRemoveItem.Size = new Size(85, 37);
            };
            btnRemoveItem.MouseLeave += (s, args) =>
            {
                btnRemoveItem.BackColor = Color.FromArgb(150, 30, 30);
                btnRemoveItem.Size = new Size(80, 35);
            };
            btnRemoveItem.Click += BtnRemoveItem_Click;
            this.Controls.Add(btnRemoveItem);

            // Print Bill Button
            btnPrintBill = new Button
            {
                Text = "Adisyon Yazdır",
                Location = new Point(680, 415),
                Size = new Size(100, 35),
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 0 },
                BackColor = Color.FromArgb(180, 90, 50),
                ForeColor = Color.FromArgb(240, 230, 200),
                Font = new Font("Georgia", 10)
            };
            btnPrintBill.MouseEnter += (s, args) =>
            {
                btnPrintBill.BackColor = Color.FromArgb(200, 110, 70);
                btnPrintBill.Size = new Size(105, 37);
            };
            btnPrintBill.MouseLeave += (s, args) =>
            {
                btnPrintBill.BackColor = Color.FromArgb(180, 90, 50);
                btnPrintBill.Size = new Size(100, 35);
            };
            btnPrintBill.Click += BtnPrintBill_Click;
            this.Controls.Add(btnPrintBill);

            // Form closing event
            this.FormClosing += (s, e) => SaveTableOrders();
        }

        private void LoadExistingOrders()
        {
            foreach (DataGridViewRow row in orders)
            {
                DataGridViewRow newRow = (DataGridViewRow)row.Clone();
                for (int i = 0; i < row.Cells.Count; i++)
                {
                    newRow.Cells[i].Value = row.Cells[i].Value;
                }
                orderGrid.Rows.Add(newRow);
            }
            UpdateTotal();
        }

        private void BtnAddItem_Click(object sender, EventArgs e)
        {
            if (cmbMenuItems.SelectedItem != null)
            {
                string item = cmbMenuItems.SelectedItem.ToString();
                decimal price = menuItems[item];
                decimal quantity = nudQuantity.Value;

                bool itemExists = false;
                foreach (DataGridViewRow row in orderGrid.Rows)
                {
                    if (!row.IsNewRow && row.Cells["Item"].Value.ToString() == item)
                    {
                        decimal currentQuantity = Convert.ToDecimal(row.Cells["Quantity"].Value);
                        row.Cells["Quantity"].Value = currentQuantity + quantity;
                        itemExists = true;
                        break;
                    }
                }

                if (!itemExists)
                {
                    orderGrid.Rows.Add(item, quantity, price);
                }

                UpdateTotal();
                nudQuantity.Value = 1;
            }
            else
            {
                MessageBox.Show("Lütfen bir menü öğesi seçiniz!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnRemoveItem_Click(object sender, EventArgs e)
        {
            if (orderGrid.SelectedRows.Count > 0)
            {
                foreach (DataGridViewRow row in orderGrid.SelectedRows)
                {
                    if (!row.IsNewRow)
                    {
                        orderGrid.Rows.Remove(row);
                    }
                }
                UpdateTotal();
            }
            else
            {
                MessageBox.Show("Lütfen silmek için bir ürün seçiniz!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnPrintBill_Click(object sender, EventArgs e)
        {
            StringBuilder bill = new StringBuilder();
            bill.AppendLine($"=== {tableName} Adisyon ===");
            bill.AppendLine($"Tarih: {DateTime.Now:dd/MM/yyyy HH:mm}");
            bill.AppendLine();
            foreach (DataGridViewRow row in orderGrid.Rows)
            {
                if (!row.IsNewRow)
                {
                    bill.AppendLine($"{row.Cells["Item"].Value,-20} {row.Cells["Quantity"].Value,2} x {row.Cells["Price"].Value:C2}");
                }
            }
            bill.AppendLine();
            bill.AppendLine($"Toplam: {CalculateTotal():C2}");
            txtBill.Text = bill.ToString();

            orderGrid.Rows.Clear();
            orders.Clear();
            UpdateTotal();
            SaveTableOrders();
        }

        private void UpdateTotal()
        {
            total = CalculateTotal();
            lblTotal.Text = $"Toplam: {total:C2}";
        }

        private decimal CalculateTotal()
        {
            decimal total = 0;
            foreach (DataGridViewRow row in orderGrid.Rows)
            {
                if (!row.IsNewRow)
                {
                    total += Convert.ToDecimal(row.Cells["Quantity"].Value) * Convert.ToDecimal(row.Cells["Price"].Value);
                }
            }
            return total;
        }

        private void SaveTableOrders()
        {
            orders.Clear();
            foreach (DataGridViewRow row in orderGrid.Rows)
            {
                if (!row.IsNewRow)
                {
                    DataGridViewRow newRow = (DataGridViewRow)row.Clone();
                    for (int i = 0; i < row.Cells.Count; i++)
                    {
                        newRow.Cells[i].Value = row.Cells[i].Value;
                    }
                    orders.Add(newRow);
                }
            }
            bool isOccupied = orders.Any();
            OrderChanged?.Invoke(isOccupied, orders, total);
        }
    }
}