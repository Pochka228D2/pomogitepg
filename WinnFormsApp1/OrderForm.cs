using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace WinnFormsApp1
{
    internal sealed class OrderLine
    {
        public int StockItemId { get; }
        public string Name { get; }
        public decimal Size { get; }
        public int Quantity { get; set; }
        public decimal Price { get; }
        public int Available { get; }

        public OrderLine(int stockItemId, string name, decimal size, int quantity, decimal price, int available)
        {
            StockItemId = stockItemId;
            Name = name;
            Size = size;
            Quantity = quantity;
            Price = price;
            Available = available;
        }
    }

    internal sealed class OrderForm : Form
    {
        private readonly int userId;
        private readonly List<OrderLine> lines;
        private readonly ListView items = new ListView
        {
            Dock = DockStyle.Fill,
            View = View.Details,
            FullRowSelect = true,
            MultiSelect = false
        };
        private readonly Label total = new Label { AutoSize = true, Padding = new Padding(6) };
        private readonly Button confirm = new Button { Text = "Подтвердить заказ", AutoSize = true, Enabled = false };
        private readonly NumericUpDown quantity = new NumericUpDown { Minimum = 1, Maximum = 1, Width = 80, Enabled = false };
        private readonly Button updateQuantity = new Button { Text = "Изменить количество", AutoSize = true, Enabled = false };

        public bool OrderPlaced { get; private set; }

        public OrderForm(int userId, List<OrderLine> lines)
        {
            this.userId = userId;
            this.lines = lines;
            Text = "Чудо Обувь — заказ";
            StartPosition = FormStartPosition.CenterParent;
            ClientSize = new Size(820, 385);
            MinimumSize = new Size(836, 424);

            items.Columns.Add("Модель", 370);
            items.Columns.Add("Размер", 75);
            items.Columns.Add("Пар", 55);
            items.Columns.Add("Цена за пару", 120);
            items.Columns.Add("Сумма", 120);

            var back = new Button { Text = "Назад", AutoSize = true, DialogResult = DialogResult.Cancel };
            var remove = new Button { Text = "Убрать позицию", AutoSize = true, Enabled = false };
            var clear = new Button { Text = "Отменить заказ", AutoSize = true };
            CancelButton = back;

            items.SelectedIndexChanged += (s, e) =>
            {
                bool selected = items.SelectedIndices.Count > 0;
                remove.Enabled = selected;
                quantity.Enabled = selected;
                updateQuantity.Enabled = selected;
                if (selected)
                {
                    OrderLine line = lines[items.SelectedIndices[0]];
                    quantity.Maximum = line.Available;
                    quantity.Value = line.Quantity;
                }
            };

            updateQuantity.Click += (s, e) =>
            {
                if (items.SelectedIndices.Count == 0)
                    return;
                int index = items.SelectedIndices[0];
                lines[index].Quantity = (int)quantity.Value;
                ShowLines();
                items.Items[index].Selected = true;
            };

            remove.Click += (s, e) =>
            {
                if (items.SelectedIndices.Count == 0)
                    return;
                lines.RemoveAt(items.SelectedIndices[0]);
                ShowLines();
            };

            clear.Click += (s, e) =>
            {
                lines.Clear();
                DialogResult = DialogResult.Cancel;
            };

            confirm.Click += (s, e) => ConfirmOrder();

            var actions = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true };
            actions.Controls.AddRange(new Control[] { back, remove, clear, confirm, total });

            var quantityActions = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true };
            quantityActions.Controls.Add(new Label { Text = "Количество пар", AutoSize = true, Padding = new Padding(0, 5, 0, 0) });
            quantityActions.Controls.Add(quantity);
            quantityActions.Controls.Add(updateQuantity);

            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3,
                Padding = new Padding(12)
            };
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            layout.Controls.Add(items, 0, 0);
            layout.Controls.Add(quantityActions, 0, 1);
            layout.Controls.Add(actions, 0, 2);
            Controls.Add(layout);
            ShowLines();
        }

        private void ShowLines()
        {
            items.Items.Clear();
            foreach (OrderLine line in lines)
            {
                items.Items.Add(new ListViewItem(new[]
                {
                    line.Name,
                    line.Size.ToString("0.##"),
                    line.Quantity.ToString(),
                    $"{line.Price:N2} ₽",
                    $"{line.Price * line.Quantity:N2} ₽"
                }));
            }
            total.Text = $"Итого: {lines.Sum(line => line.Price * line.Quantity):N2} ₽";
            confirm.Enabled = lines.Count > 0;
            if (items.SelectedIndices.Count == 0)
            {
                quantity.Enabled = false;
                updateQuantity.Enabled = false;
            }
        }

        private void ConfirmOrder()
        {
            if (lines.Count == 0)
                return;

            try
            {
                using (var connection = Database.CreateConnection())
                {
                    connection.Open();
                    using (var transaction = connection.BeginTransaction())
                    {
                        long orderId;
                        const string insertOrder = "INSERT INTO orders (order_date, user_id) VALUES (@date, @userId)";
                        using (var command = new MySqlCommand(insertOrder, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@date", DateTime.Today);
                            command.Parameters.AddWithValue("@userId", userId);
                            command.ExecuteNonQuery();
                            orderId = command.LastInsertedId;
                        }

                        const string updateStock = "UPDATE stock_items SET quantity = quantity - @quantity WHERE stock_item_id = @id AND quantity >= @quantity";
                        const string insertItem = "INSERT INTO order_items (order_id, stock_item_id, quantity, unit_price) VALUES (@orderId, @id, @quantity, @price)";

                        foreach (OrderLine line in lines)
                        {
                            using (var stockCommand = new MySqlCommand(updateStock, connection, transaction))
                            {
                                stockCommand.Parameters.AddWithValue("@quantity", line.Quantity);
                                stockCommand.Parameters.AddWithValue("@id", line.StockItemId);
                                if (stockCommand.ExecuteNonQuery() != 1)
                                {
                                    transaction.Rollback();
                                    MessageBox.Show(this, $"Недостаточно пар на складе для модели «{line.Name}» ({line.Size:0.##}).",
                                        "Недостаточно товара", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return;
                                }
                            }

                            using (var itemCommand = new MySqlCommand(insertItem, connection, transaction))
                            {
                                itemCommand.Parameters.AddWithValue("@orderId", orderId);
                                itemCommand.Parameters.AddWithValue("@id", line.StockItemId);
                                itemCommand.Parameters.AddWithValue("@quantity", line.Quantity);
                                itemCommand.Parameters.AddWithValue("@price", line.Price);
                                itemCommand.ExecuteNonQuery();
                            }
                        }

                        transaction.Commit();
                        lines.Clear();
                        OrderPlaced = true;
                        MessageBox.Show(this, $"Заказ № {orderId} оформлен.", "Чудо Обувь",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        DialogResult = DialogResult.OK;
                    }
                }
            }
            catch (MySqlException)
            {
                MessageBox.Show(this, "Не удалось сохранить заказ. Проверьте соединение с MySQL.",
                    "Ошибка оформления заказа", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
