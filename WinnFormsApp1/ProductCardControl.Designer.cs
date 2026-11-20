namespace WinnFormsApp1
{
    partial class ProductCardControl
    {
        /// <summary> 
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором компонентов

        /// <summary> 
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.picProduct = new System.Windows.Forms.PictureBox();
            this.lblProductTitle = new System.Windows.Forms.Label();
            this.lblCategory = new System.Windows.Forms.Label();
            this.lblQuantity = new System.Windows.Forms.Label();
            this.lblMaterial = new System.Windows.Forms.Label();
            this.lblPrice = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.picProduct)).BeginInit();
            this.SuspendLayout();
            // 
            // picProduct
            // 
            this.picProduct.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picProduct.Location = new System.Drawing.Point(14, 14);
            this.picProduct.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.picProduct.Name = "picProduct";
            this.picProduct.Size = new System.Drawing.Size(149, 127);
            this.picProduct.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picProduct.TabIndex = 0;
            this.picProduct.TabStop = false;
            // 
            // lblProductTitle
            // 
            this.lblProductTitle.Location = new System.Drawing.Point(182, 14);
            this.lblProductTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblProductTitle.Name = "lblProductTitle";
            this.lblProductTitle.Size = new System.Drawing.Size(531, 44);
            this.lblProductTitle.TabIndex = 1;
            this.lblProductTitle.Text = "Производство и название";
            this.lblProductTitle.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblCategory
            // 
            this.lblCategory.AutoSize = true;
            this.lblCategory.Location = new System.Drawing.Point(182, 60);
            this.lblCategory.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCategory.Name = "lblCategory";
            this.lblCategory.Size = new System.Drawing.Size(64, 15);
            this.lblCategory.TabIndex = 2;
            this.lblCategory.Text = "Категория";
            // 
            // lblQuantity
            // 
            this.lblQuantity.AutoSize = true;
            this.lblQuantity.Location = new System.Drawing.Point(182, 85);
            this.lblQuantity.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblQuantity.Name = "lblQuantity";
            this.lblQuantity.Size = new System.Drawing.Size(71, 15);
            this.lblQuantity.TabIndex = 3;
            this.lblQuantity.Text = "Количество";
            // 
            // lblMaterial
            // 
            this.lblMaterial.Location = new System.Drawing.Point(182, 60);
            this.lblMaterial.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMaterial.Name = "lblMaterial";
            this.lblMaterial.Size = new System.Drawing.Size(531, 92);
            this.lblMaterial.TabIndex = 4;
            this.lblMaterial.Text = "Состав";
            this.lblMaterial.UseWaitCursor = true;
            // 
            // lblPrice
            // 
            this.lblPrice.AutoSize = true;
            this.lblPrice.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblPrice.Location = new System.Drawing.Point(752, 54);
            this.lblPrice.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPrice.Name = "lblPrice";
            this.lblPrice.Size = new System.Drawing.Size(35, 15);
            this.lblPrice.TabIndex = 5;
            this.lblPrice.Text = "Цена";
            // 
            // ProductCardControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.lblPrice);
            this.Controls.Add(this.lblMaterial);
            this.Controls.Add(this.lblQuantity);
            this.Controls.Add(this.lblCategory);
            this.Controls.Add(this.lblProductTitle);
            this.Controls.Add(this.picProduct);
            this.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "ProductCardControl";
            this.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Size = new System.Drawing.Size(931, 211);
            ((System.ComponentModel.ISupportInitialize)(this.picProduct)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox picProduct;
        private System.Windows.Forms.Label lblProductTitle;
        private System.Windows.Forms.Label lblCategory;
        private System.Windows.Forms.Label lblQuantity;
        private System.Windows.Forms.Label lblMaterial;
        private System.Windows.Forms.Label lblPrice;
    }
}
