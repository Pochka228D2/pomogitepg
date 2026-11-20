using MySqlConnector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace WinnFormsApp1
{
    internal sealed class OrderListForm : Form
    {
        private readonly bool canManage;
        private readonly ComboBox clients = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = 280 };
        private readonly TextBox search = new TextBox { Width = 280 };
        private readonly ListView orders = new ListView
        {
            Dock = DockStyle.Fill,
            View = View.Details,
            FullRowSelect = true,
            MultiSelect = false
        };
        private readonly ListView items = new ListView
        {
            Dock = DockStyle.Fill,
            View = View.Details,
            FullRowSelect = true,
            MultiSelect = false
        };
        private readonly DateTimePicker orderDate = new DateTimePicker { Format = DateTimePickerFormat.Short, Width = 130, Enabled = false };
        private readonly Button saveDate = new Button { Text = "Сохранить дату", AutoSize = true, Enabled = false };
        private readonly Button removeItem = new Button { Text = "Удалить позицию", AutoSize = true, Enabled = false };
        private readonly Button removeOrder = new Button { Text = "Удалить заказ", AutoSize = true, Enabled = false };
        private readonly Label summary = new Label { AutoSize = true, Padding = new Padding(6) };

        private sealed class OrderHeader
        {
            public int Id { get; }
            public DateTime Date { get; set; }
            public string Client { get; }
            public decimal Total { get; set; }
            public int Pairs { get; set; }
            public string Summary { get; set; }

            public OrderHeader(int id, DateTime date, string client)
            {
                Id = id;
                Date = date;
                Client = client;
            }
        }

        private sealed class OrderPosition
        {
            public int ItemId { get; }
            public string Product { get; }
            public decimal Size { get; }
            public int Quantity { get; }
            public decimal Price { get; }

            public OrderPosition(int itemId, string product, decimal size, int quantity, decimal price)
            {
                ItemId = itemId;
                Product = product;
                Size = size;
                Quantity = quantity;
                Price = price;
            }
        }

        private readonly List<OrderHeader> allOrders = new List<OrderHeader>();
        private readonly Dictionary<int, List<OrderPosition>> positions = new Dictionary<int, List<OrderPosition>>();

        public bool StockChanged { get; private set; }

        public OrderListForm(bool canManage)
        {
            this.canManage = canManage;
            Text = "Чудо Обувь — список заказов";
            StartPosition = FormStartPosition.CenterParent;
            ClientSize = new Size(1040, 560);
            MinimumSize = new Size(1056, 599);

            orders.Columns.Add("№ заказа", 85);
            orders.Columns.Add("Дата", 95);
            orders.Columns.Add("Клиент", 260);
            orders.Columns.Add("Позиции", 390);
            orders.Columns.Add("Пар", 55);
            orders.Columns.Add("Сумма", 120);

            items.Columns.Add("Модель", 520);
            items.Columns.Add("Размер", 80);
            items.Columns.Add("Пар", 60);
            items.Columns.Add("Цена за пару", 140);
            items.Columns.Add("Сумма", 140);

            var back = new Button { Text = "Назад", AutoSize = true, DialogResult = DialogResult.Cancel };
            CancelButton = back;

            var filters = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true };
            filters.Controls.Add(new Label { Text = "Клиент", AutoSize = true, Padding = new Padding(0, 5, 0, 0) });
            filters.Controls.Add(clients);
            filters.Controls.Add(new Label { Text = "Поиск по номеру, клиенту или модели", AutoSize = true, Padding = new Padding(12, 5, 0, 0) });
            filters.Controls.Add(search);

            var actions = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true };
            actions.Controls.Add(back);
            if (canManage)
            {
                actions.Controls.Add(new Label { Text = "Дата заказа", AutoSize = true, Padding = new Padding(12, 5, 0, 0) });
                actions.Controls.Add(orderDate);
                actions.Controls.Add(saveDate);
                actions.Controls.Add(removeItem);
                actions.Controls.Add(removeOrder);
            }
            actions.Controls.Add(summary);

            var split = new SplitContainer
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Horizontal,
                SplitterDistance = 240
            };
            split.Panel1.Controls.Add(orders);
            split.Panel2.Controls.Add(items);

            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3,
                Padding = new Padding(12)
            };
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            layout.Controls.Add(filters, 0, 0);
            layout.Controls.Add(split, 0, 1);
            layout.Controls.Add(actions, 0, 2);
            Controls.Add(layout);

            clients.SelectedIndexChanged += (s, e) => ShowOrders();
            search.TextChanged += (s, e) => ShowOrders();
            orders.SelectedIndexChanged += (s, e) => ShowOrderItems();
            items.SelectedIndexChanged += (s, e) => removeItem.Enabled = canManage && items.SelectedItems.Count > 0;
            saveDate.Click += (s, e) => SaveOrderDate();
            removeItem.Click += (s, e) => RemoveSelectedItem();
            removeOrder.Click += (s, e) => RemoveSelectedOrder();

            LoadOrders();
        }

        private OrderHeader SelectedOrder => orders.SelectedItems.Count > 0 ? orders.SelectedItems[0].Tag as OrderHeader : null;
        private OrderPosition SelectedPosition => items.SelectedItems.Count > 0 ? items.SelectedItems[0].Tag as OrderPosition : null;

        private void LoadOrders(int? selectOrderId = null)
        {
            string selectedClient = clients.SelectedIndex > 0 ? clients.SelectedItem?.ToString() : null;
            allOrders.Clear();
            positions.Clear();

            using (var connection = Database.CreateConnection())
            {
                connection.Open();
                const string ordersSql = @"
                    SELECT o.order_id, o.order_date,
                           CONCAT_WS(' ', u.last_name, u.first_name, NULLIF(TRIM(u.patronymic), ''))
                    FROM orders AS o
                    JOIN users AS u ON u.user_id = o.user_id
                    ORDER BY o.order_date DESC, o.order_id DESC";

                using (var command = new MySqlCommand(ordersSql, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        allOrders.Add(new OrderHeader(reader.GetInt32(0), reader.GetDateTime(1), reader.GetString(2)));
                    }
                }

                const string itemsSql = @"
                    SELECT oi.order_item_id, oi.order_id, p.product_name, s.size_value, oi.quantity, oi.unit_price
                    FROM order_items AS oi
                    JOIN stock_items AS si ON si.stock_item_id = oi.stock_item_id
                    JOIN products AS p ON p.product_id = si.product_id
                    JOIN sizes AS s ON s.size_id = si.size_id
                    ORDER BY oi.order_item_id";

                using (var command = new MySqlCommand(itemsSql, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int orderId = reader.GetInt32(1);
                        if (!positions.TryGetValue(orderId, out List<OrderPosition> list))
                        {
                            list = new List<OrderPosition>();
                            positions[orderId] = list;
                        }
                        list.Add(new OrderPosition(reader.GetInt32(0), reader.GetString(2), reader.GetDecimal(3),
                            reader.GetInt32(4), reader.GetDecimal(5)));
                    }
                }
            }

            foreach (OrderHeader order in allOrders)
            {
                List<OrderPosition> list = positions.TryGetValue(order.Id, out List<OrderPosition> found) ? found : new List<OrderPosition>();
                order.Pairs = list.Sum(item => item.Quantity);
                order.Total = list.Sum(item => item.Price * item.Quantity);
                order.Summary = list.Count == 0
                    ? "Нет позиций"
                    : string.Join("; ", list.Select(item => $"{item.Product} ({item.Size:0.##}) x{item.Quantity}"));
            }

            clients.Items.Clear();
            clients.Items.Add("Все клиенты");
            foreach (string client in allOrders.Select(order => order.Client).Distinct().OrderBy(name => name))
            {
                clients.Items.Add(client);
            }

            int clientIndex = selectedClient != null ? clients.Items.IndexOf(selectedClient) : 0;
            clients.SelectedIndex = clientIndex >= 0 ? clientIndex : 0;
            ShowOrders(selectOrderId);
        }

        private void ShowOrders(int? selectOrderId = null)
        {
            string selectedClient = clients.SelectedIndex > 0 ? clients.SelectedItem?.ToString() : null;
            string filter = search.Text.Trim();
            orders.Items.Clear();
            IEnumerable filtered = allOrders.Where(order =>
            (selectedClient == null || order.Client == selectedClient)
            && (filter.Length == 0
            || order.Id.ToString().IndexOf(filter, StringComparison.CurrentCultureIgnoreCase) >= 0
            || order.Client.IndexOf(filter, StringComparison.CurrentCultureIgnoreCase) >= 0
            || order.Summary.IndexOf(filter, StringComparison.CurrentCultureIgnoreCase) >= 0));
            foreach (OrderHeader order in filtered)
            {
                var row = new ListViewItem(new[]
                {
order.Id.ToString(),
order.Date.ToString("dd.MM.yyyy"),
order.Client,
order.Summary,
order.Pairs.ToString(),
$"{order.Total:N2} ₽"
})
                {
                    Tag = order
                };
                orders.Items.Add(row);
                if (order.Id == selectOrderId)
                    row.Selected = true;
            }
            if (orders.SelectedItems.Count == 0)
            {
                if (orders.Items.Count > 0)
                    orders.Items[0].Selected = true;
                else
                    ShowOrderItems();
            }
            summary.Text = $"Заказов: {orders.Items.Count}";
        }
        private void ShowOrderItems()
        {
            items.Items.Clear();
            OrderHeader order = SelectedOrder;
            bool hasOrder = order != null;
            orderDate.Enabled = canManage && hasOrder;
            saveDate.Enabled = canManage && hasOrder;
            removeOrder.Enabled = canManage && hasOrder;
            removeItem.Enabled = false;
            if (!hasOrder)
                return;
            orderDate.Value = order.Date;
            if (positions.TryGetValue(order.Id, out List<OrderPosition> list))
            {
                foreach (OrderPosition item in list)
                {
                    items.Items.Add(new ListViewItem(new[]
                    {
item.Product,
item.Size.ToString("0.##"),
item.Quantity.ToString(),
$"{item.Price:N2} ₽",
$"{item.Price * item.Quantity:N2} ₽"
})
                    {
                        Tag = item
                    });
                }
            }
        }
        private void SaveOrderDate()
        {
            OrderHeader order = SelectedOrder;
            if (!canManage || order == null)
                return;
            try
            {
                using (var connection = Database.CreateConnection())
                {
                    connection.Open();
                    const string sql = "UPDATE orders SET order_date = @date WHERE order_id = @id";
                    using (var command = new MySqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@date", orderDate.Value.Date);
                        command.Parameters.AddWithValue("@id", order.Id);
                        command.ExecuteNonQuery();
                    }
                }
                LoadOrders(order.Id);
            }
            catch (MySqlException)
            {
                MessageBox.Show(this, "Не удалось сохранить дату заказа.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void RemoveSelectedItem()
        {
            OrderHeader order = SelectedOrder;
            OrderPosition position = SelectedPosition;
            if (!canManage || order == null || position == null)
                return;
            if (MessageBox.Show(this, $"Удалить позицию «{position.Product}» ({position.Size:0.##}) из заказа № {order.Id}?",
            "Подтверждение удаления", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }
            try
            {
                using (var connection = Database.CreateConnection())
                {
                    connection.Open();
                    using (var transaction = connection.BeginTransaction())
                    {
                        int stockItemId;
                        int returnedQuantity;
                        const string selectSql = "SELECT stock_item_id, quantity FROM order_items WHERE order_item_id = @id AND order_id = @orderId FOR UPDATE";
                        using (var command = new MySqlCommand(selectSql, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@id", position.ItemId);
                            command.Parameters.AddWithValue("@orderId", order.Id);
                            using (var reader = command.ExecuteReader())
                            {
                                if (!reader.Read())
                                {
                                    transaction.Rollback();
                                    LoadOrders(order.Id);
                                    return;
                                }
                                stockItemId = reader.GetInt32(0);
                                returnedQuantity = reader.GetInt32(1);
                            }
                        }
                        const string returnStockSql = "UPDATE stock_items SET quantity = quantity + @qty WHERE stock_item_id = @id";
                        using (var command = new MySqlCommand(returnStockSql, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@qty", returnedQuantity);
                            command.Parameters.AddWithValue("@id", stockItemId);
                            command.ExecuteNonQuery();
                        }
                        const string deleteItemSql = "DELETE FROM order_items WHERE order_item_id = @id AND order_id = @orderId";
                        using (var command = new MySqlCommand(deleteItemSql, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@id", position.ItemId);
                            command.Parameters.AddWithValue("@orderId", order.Id);
                            command.ExecuteNonQuery();
                        }
                        const string countSql = "SELECT COUNT(*) FROM order_items WHERE order_id = @orderId";
                        long remaining;
                        using (var command = new MySqlCommand(countSql, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@orderId", order.Id);
                            remaining = Convert.ToInt64(command.ExecuteScalar());
                        }
                        if (remaining == 0)
                        {
                            const string deleteOrderSql = "DELETE FROM orders WHERE order_id = @orderId";
                            using (var command = new MySqlCommand(deleteOrderSql, connection, transaction))
                            {
                                command.Parameters.AddWithValue("@orderId", order.Id);
                                command.ExecuteNonQuery();
                            }
                        }
                        transaction.Commit();
                        StockChanged = true;
                        LoadOrders(remaining > 0 ? (int?)order.Id : null);
                    }
                }
            }
            catch (MySqlException)
            {
                MessageBox.Show(this, "Не удалось удалить позицию из заказа.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void RemoveSelectedOrder()
        {
            OrderHeader order = SelectedOrder;
            if (!canManage || order == null)
                return;
            if (MessageBox.Show(this, $"Удалить заказ № {order.Id} целиком и вернуть все пары на склад?",
            "Подтверждение удаления", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }
            try
            {
                using (var connection = Database.CreateConnection())
                {
                    connection.Open();
                    using (var transaction = connection.BeginTransaction())
                    {
                        var itemsToReturn = new List<KeyValuePair<int, int>>();
                        const string selectSql = "SELECT stock_item_id, quantity FROM order_items WHERE order_id = @orderId FOR UPDATE";
                        using (var command = new MySqlCommand(selectSql, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@orderId", order.Id);
                            using (var reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    itemsToReturn.Add(new KeyValuePair<int, int>(reader.GetInt32(0), reader.GetInt32(1)));
                                }
                            }
                        }
                        const string returnStockSql = "UPDATE stock_items SET quantity = quantity + @qty WHERE stock_item_id = @id";
                        foreach (var item in itemsToReturn)
                        {
                            using (var command = new MySqlCommand(returnStockSql, connection, transaction))
                            {
                                command.Parameters.AddWithValue("@qty", item.Value);
                                command.Parameters.AddWithValue("@id", item.Key);
                                command.ExecuteNonQuery();
                            }
                        }
                        const string deleteItemsSql = "DELETE FROM order_items WHERE order_id = @orderId";
                        using (var command = new MySqlCommand(deleteItemsSql, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@orderId", order.Id);
                            command.ExecuteNonQuery();
                        }
                        const string deleteOrderSql = "DELETE FROM orders WHERE order_id = @orderId";
                        using (var command = new MySqlCommand(deleteOrderSql, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@orderId", order.Id);
                            command.ExecuteNonQuery();
                        }
                        transaction.Commit();
                        StockChanged = true;
                        LoadOrders();
                    }
                }
            }
            catch (MySqlException)
            {
                MessageBox.Show(this, "Не удалось удалить заказ.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}