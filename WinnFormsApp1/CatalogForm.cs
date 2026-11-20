using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace WinnFormsApp1
{
    public partial class CatalogForm : Form
    {
        private readonly int userId;
        public string UserRole { get; }

        private readonly List<CatalogEntry> products = new List<CatalogEntry>();
        private readonly List<OrderLine> orderLines = new List<OrderLine>();

        private sealed class CatalogEntry
        {
            public ProductCardControl Card { get; }
            public string Name { get; }
            public string Description { get; }
            public int CategoryId { get; }
            public decimal Price { get; }

            public CatalogEntry(ProductCardControl card, string name, string description, int categoryId, decimal price)
            {
                Card = card;
                Name = name;
                Description = description;
                CategoryId = categoryId;
                Price = price;
            }
        }

        private sealed class CategoryOption
        {
            public int? Id { get; }
            public string Name { get; }

            public CategoryOption(int? id, string name)
            {
                Id = id;
                Name = name;
            }

            public override string ToString() => Name;
        }

        public CatalogForm()
        {
            InitializeComponent();
            btnOrder.Visible = false;
        }

        public CatalogForm(string displayName, string role, int userId) : this()
        {
            lblUserName.Text = displayName;
            UserRole = role != null ? role.Trim() : null;
            this.userId = userId;
            btnOrder.Visible = userId > 0;
            btnOrder.Enabled = userId > 0;

            bool canViewOrders = string.Equals(UserRole, "Manager", StringComparison.OrdinalIgnoreCase)
                || string.Equals(UserRole, "Administrator", StringComparison.OrdinalIgnoreCase);
            btnOrderList.Visible = canViewOrders;
            btnOrderList.Enabled = canViewOrders;
        }

        internal static CatalogForm ForGuest()
        {
            var catalog = new CatalogForm();
            catalog.lblUserName.Text = "Гость";
            return catalog;
        }

        private void LoadCategories(MySqlConnection connection)
        {
            const string sql = "SELECT category_id, category_name FROM categories ORDER BY category_name";
            using (var command = new MySqlCommand(sql, connection))
            using (var reader = command.ExecuteReader())
            {
                cboCategory.Items.Add(new CategoryOption(null, "Все категории"));
                while (reader.Read())
                {
                    cboCategory.Items.Add(new CategoryOption(reader.GetInt32(0), reader.GetString(1)));
                }
            }
        }

        private void LoadProducts(bool loadCategories = true)
        {
            const string sql = ProductDetailsForm.ProductQuery + " ORDER BY p.product_name, p.product_id";
            using (var connection = Database.CreateConnection())
            {
                connection.Open();
                if (loadCategories)
                    LoadCategories(connection);

                using (var command = new MySqlCommand(sql, connection))
                {
                    ProductDetailsForm.SetMonthParameters(command);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var card = new ProductCardControl();
                            decimal finalPrice = ProductDetailsForm.FillCard(card, reader);

                            int productId = reader.GetInt32(reader.GetOrdinal("product_id"));
                            if (userId > 0)
                                card.EnableSelection((s, e) => OpenProduct(productId));

                            string productName = reader.GetString(1);
                            int descriptionColumn = reader.GetOrdinal("description");
                            string description = reader.IsDBNull(descriptionColumn)
                                ? string.Empty
                                : reader.GetString(descriptionColumn);

                            products.Add(new CatalogEntry(card, productName, description,
                                reader.GetInt32(reader.GetOrdinal("category_id")), finalPrice));
                            flpProduct.Controls.Add(card);
                        }
                    }
                }
            }
        }

        private void OpenProduct(int productId)
        {
            try
            {
                using (var details = new ProductDetailsForm(productId))
                {
                    if (details.ShowDialog(this) == DialogResult.OK && details.SelectedLine is OrderLine line)
                    {
                        int index = orderLines.FindIndex(item => item.StockItemId == line.StockItemId);
                        if (index >= 0)
                        {
                            int previousQuantity = orderLines[index].Quantity;
                            if (line.Quantity > line.Available - previousQuantity)
                            {
                                MessageBox.Show(this, "С учётом позиций в заказе доступных пар недостаточно. Уменьшите количество.",
                                    "Недостаточно товара", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                            orderLines[index].Quantity = previousQuantity + line.Quantity;
                        }
                        else
                        {
                            orderLines.Add(line);
                        }
                        UpdateOrderButton();
                    }
                }
            }
            catch (MySqlException)
            {
                MessageBox.Show(this, "Не удалось загрузить товар. Проверьте соединение и попробуйте ещё раз.",
                    "Ошибка загрузки товара", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateOrderButton()
        {
            btnOrder.Text = $"Заказ ({orderLines.Count})";
        }

        private void btnOrder_Click(object sender, EventArgs e)
        {
            if (userId <= 0)
                return;
            using (var order = new OrderForm(userId, orderLines))
            {
                order.ShowDialog(this);
                UpdateOrderButton();
                if (order.OrderPlaced)
                {
                    products.Clear();
                    flpProduct.Controls.Clear();
                    LoadProducts(loadCategories: false);
                    ApplyCatalogOptions();
                }
            }
        }
        private void btnOrderList_Click(object sender, EventArgs e)
        {
            bool canManage = string.Equals(UserRole, "Administrator", StringComparison.OrdinalIgnoreCase);
            try
            {
                using (var orderList = new OrderListForm(canManage))
                {
                    orderList.ShowDialog(this);
                    if (orderList.StockChanged)
                    {
                        products.Clear();
                        flpProduct.Controls.Clear();
                        LoadProducts(loadCategories: false);
                        ApplyCatalogOptions();
                    }
                }
            }
            catch (MySqlException)
            {
                MessageBox.Show(this, "Не удалось загрузить заказы. Проверьте соединение и попробуйте ещё раз.",
                    "Ошибка загрузки заказов", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CatalogOptionsChanged(object sender, EventArgs e)
        {
            ApplyCatalogOptions();
        }

        private void ApplyCatalogOptions()
        {
            string search = txtSearch.Text.Trim();
            int? categoryId = (cboCategory.SelectedItem as CategoryOption)?.Id;
            string sortText = cboPriceSort.SelectedItem != null ? cboPriceSort.SelectedItem.ToString() : string.Empty;
            IEnumerable<CatalogEntry> orderedProducts;

            if (sortText.IndexOf("дешевле", StringComparison.OrdinalIgnoreCase) >= 0)
                orderedProducts = products.OrderBy(product => product.Price);
            else if (sortText.IndexOf("дороже", StringComparison.OrdinalIgnoreCase) >= 0)
                orderedProducts = products.OrderByDescending(product => product.Price);
            else
                orderedProducts = products;

            flpProduct.SuspendLayout();
            try
            {
                int index = 0;
                foreach (CatalogEntry product in orderedProducts)
                {
                    bool categoryMatches = categoryId == null || product.CategoryId == categoryId;
                    bool searchMatches = search.Length == 0
                        || product.Name.IndexOf(search, StringComparison.CurrentCultureIgnoreCase) >= 0
                        || product.Description.IndexOf(search, StringComparison.CurrentCultureIgnoreCase) >= 0;
                    flpProduct.Controls.SetChildIndex(product.Card, index++);
                    product.Card.Visible = categoryMatches && searchMatches;
                }
            }
            finally
            {
                flpProduct.ResumeLayout();
            }
            flpProduct.AutoScrollPosition = Point.Empty;
        }


        private void CatalogForm_Load(object sender, EventArgs e)
        {
            try
            {
                LoadProducts();
                cboCategory.SelectedIndex = 0;
                cboPriceSort.SelectedIndex = 0;
            }
            catch (MySqlException)
            {
                MessageBox.Show(
                    "Не удалось загрузить товары. Проверьте подключение к MySQL и названия столбцов.",
                    "Ошибка загрузки каталога",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
