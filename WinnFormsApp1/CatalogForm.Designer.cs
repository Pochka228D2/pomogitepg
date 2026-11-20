namespace WinnFormsApp1
{
    partial class CatalogForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblCatalogTitle = new System.Windows.Forms.Label();
            this.lblUserName = new System.Windows.Forms.Label();
            this.btnBack = new System.Windows.Forms.Button();
            this.btnOrder = new System.Windows.Forms.Button();
            this.btnOrderList = new System.Windows.Forms.Button();
            this.cboCategory = new System.Windows.Forms.ComboBox();
            this.lblPriceSort = new System.Windows.Forms.Label();
            this.lblSearch = new System.Windows.Forms.Label();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.lblCategoryFilter = new System.Windows.Forms.Label();
            this.cboPriceSort = new System.Windows.Forms.ComboBox();
            this.flpProduct = new System.Windows.Forms.FlowLayoutPanel();
            this.SuspendLayout();
            // 
            // lblCatalogTitle
            // 
            this.lblCatalogTitle.AutoSize = true;
            this.lblCatalogTitle.Location = new System.Drawing.Point(8, 10);
            this.lblCatalogTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCatalogTitle.Name = "lblCatalogTitle";
            this.lblCatalogTitle.Size = new System.Drawing.Size(87, 15);
            this.lblCatalogTitle.TabIndex = 0;
            this.lblCatalogTitle.Text = "Каталог обуви";
            // 
            // lblUserName
            // 
            this.lblUserName.AutoSize = true;
            this.lblUserName.Location = new System.Drawing.Point(1130, 10);
            this.lblUserName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.Size = new System.Drawing.Size(38, 15);
            this.lblUserName.TabIndex = 1;
            this.lblUserName.Text = "Гость";
            // 
            // btnBack
            // 
            this.btnBack.BackColor = System.Drawing.Color.MediumSpringGreen;
            this.btnBack.Location = new System.Drawing.Point(14, 52);
            this.btnBack.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(88, 27);
            this.btnBack.TabIndex = 2;
            this.btnBack.Text = "Назад";
            this.btnBack.UseVisualStyleBackColor = false;
            // 
            // btnOrder
            // 
            this.btnOrder.BackColor = System.Drawing.Color.MediumSpringGreen;
            this.btnOrder.Enabled = false;
            this.btnOrder.Location = new System.Drawing.Point(396, 14);
            this.btnOrder.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnOrder.Name = "btnOrder";
            this.btnOrder.Size = new System.Drawing.Size(88, 27);
            this.btnOrder.TabIndex = 3;
            this.btnOrder.Text = "Заказ (0)";
            this.btnOrder.UseVisualStyleBackColor = false;
            this.btnOrder.Click += new System.EventHandler(this.btnOrder_Click);
            // 
            // btnOrderList
            // 
            this.btnOrderList.BackColor = System.Drawing.Color.MediumSpringGreen;
            this.btnOrderList.Enabled = false;
            this.btnOrderList.Location = new System.Drawing.Point(534, 14);
            this.btnOrderList.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnOrderList.Name = "btnOrderList";
            this.btnOrderList.Size = new System.Drawing.Size(88, 27);
            this.btnOrderList.TabIndex = 4;
            this.btnOrderList.Text = "Список заказов";
            this.btnOrderList.UseVisualStyleBackColor = false;
            this.btnOrderList.Visible = false;
            this.btnOrderList.Click += new System.EventHandler(this.btnOrderList_Click);
            // 
            // cboCategory
            // 
            this.cboCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCategory.FormattingEnabled = true;
            this.cboCategory.Location = new System.Drawing.Point(396, 117);
            this.cboCategory.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cboCategory.Name = "cboCategory";
            this.cboCategory.Size = new System.Drawing.Size(238, 23);
            this.cboCategory.TabIndex = 6;
            this.cboCategory.UseWaitCursor = true;
            this.cboCategory.SelectedIndexChanged += new System.EventHandler(this.CatalogOptionsChanged);
            // 
            // lblPriceSort
            // 
            this.lblPriceSort.AutoSize = true;
            this.lblPriceSort.Location = new System.Drawing.Point(687, 95);
            this.lblPriceSort.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPriceSort.Name = "lblPriceSort";
            this.lblPriceSort.Size = new System.Drawing.Size(119, 15);
            this.lblPriceSort.TabIndex = 7;
            this.lblPriceSort.Text = "Сортировка по цене";
            this.lblPriceSort.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblSearch
            // 
            this.lblSearch.AutoSize = true;
            this.lblSearch.Location = new System.Drawing.Point(14, 95);
            this.lblSearch.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(199, 15);
            this.lblSearch.TabIndex = 8;
            this.lblSearch.Text = "Поиск по названию или описанию";
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(14, 117);
            this.txtSearch.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(316, 22);
            this.txtSearch.TabIndex = 9;
            this.txtSearch.TextChanged += new System.EventHandler(this.CatalogOptionsChanged);
            // 
            // lblCategoryFilter
            // 
            this.lblCategoryFilter.AutoSize = true;
            this.lblCategoryFilter.Location = new System.Drawing.Point(511, 98);
            this.lblCategoryFilter.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCategoryFilter.Name = "lblCategoryFilter";
            this.lblCategoryFilter.Size = new System.Drawing.Size(64, 15);
            this.lblCategoryFilter.TabIndex = 10;
            this.lblCategoryFilter.Text = "Категория";
            // 
            // cboPriceSort
            // 
            this.cboPriceSort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPriceSort.FormattingEnabled = true;
            this.cboPriceSort.Items.AddRange(new object[] {
            "По умолчанию",
            "Сначала дешевле",
            "Сначала дороже"});
            this.cboPriceSort.Location = new System.Drawing.Point(652, 117);
            this.cboPriceSort.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cboPriceSort.Name = "cboPriceSort";
            this.cboPriceSort.Size = new System.Drawing.Size(238, 23);
            this.cboPriceSort.TabIndex = 11;
            this.cboPriceSort.SelectedIndexChanged += new System.EventHandler(this.CatalogOptionsChanged);
            // 
            // flpProduct
            // 
            this.flpProduct.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flpProduct.AutoScroll = true;
            this.flpProduct.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flpProduct.Location = new System.Drawing.Point(14, 151);
            this.flpProduct.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.flpProduct.Name = "flpProduct";
            this.flpProduct.Size = new System.Drawing.Size(1327, 577);
            this.flpProduct.TabIndex = 12;
            this.flpProduct.WrapContents = false;
            // 
            // CatalogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1370, 712);
            this.Controls.Add(this.flpProduct);
            this.Controls.Add(this.cboPriceSort);
            this.Controls.Add(this.lblCategoryFilter);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.lblSearch);
            this.Controls.Add(this.lblPriceSort);
            this.Controls.Add(this.cboCategory);
            this.Controls.Add(this.btnOrderList);
            this.Controls.Add(this.btnOrder);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.lblUserName);
            this.Controls.Add(this.lblCatalogTitle);
            this.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "CatalogForm";
            this.Text = "Чудо Обувь — каталог";
            this.Load += new System.EventHandler(this.CatalogForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblCatalogTitle;
        private System.Windows.Forms.Label lblUserName;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Button btnOrder;
        private System.Windows.Forms.Button btnOrderList;
        internal System.Windows.Forms.ComboBox cboCategory;
        private System.Windows.Forms.Label lblPriceSort;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label lblCategoryFilter;
        private System.Windows.Forms.ComboBox cboPriceSort;
        private System.Windows.Forms.FlowLayoutPanel flpProduct;
    }
}