namespace WinnFormsApp1
{
    partial class Form1
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

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.idTitle = new System.Windows.Forms.Label();
            this.idLogin = new System.Windows.Forms.Label();
            this.txtLogin = new System.Windows.Forms.TextBox();
            this.btnLogin = new System.Windows.Forms.Button();
            this.btnGuest = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // idTitle
            // 
            this.idTitle.AutoSize = true;
            this.idTitle.Location = new System.Drawing.Point(119, 127);
            this.idTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.idTitle.Name = "idTitle";
            this.idTitle.Size = new System.Drawing.Size(91, 15);
            this.idTitle.TabIndex = 0;
            this.idTitle.Text = "Вход в систему";
            // 
            // idLogin
            // 
            this.idLogin.AutoSize = true;
            this.idLogin.Location = new System.Drawing.Point(304, 127);
            this.idLogin.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.idLogin.Name = "idLogin";
            this.idLogin.Size = new System.Drawing.Size(42, 15);
            this.idLogin.TabIndex = 1;
            this.idLogin.Text = "Логин";
            // 
            // txtLogin
            // 
            this.txtLogin.Location = new System.Drawing.Point(122, 192);
            this.txtLogin.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtLogin.Name = "txtLogin";
            this.txtLogin.Size = new System.Drawing.Size(116, 22);
            this.txtLogin.TabIndex = 2;
            // 
            // btnLogin
            // 
            this.btnLogin.BackColor = System.Drawing.Color.MediumSpringGreen;
            this.btnLogin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogin.Location = new System.Drawing.Point(260, 187);
            this.btnLogin.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(88, 27);
            this.btnLogin.TabIndex = 3;
            this.btnLogin.Text = "Войти";
            this.btnLogin.UseVisualStyleBackColor = false;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // btnGuest
            // 
            this.btnGuest.BackColor = System.Drawing.Color.MediumSpringGreen;
            this.btnGuest.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGuest.Location = new System.Drawing.Point(400, 187);
            this.btnGuest.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnGuest.Name = "btnGuest";
            this.btnGuest.Size = new System.Drawing.Size(204, 27);
            this.btnGuest.TabIndex = 4;
            this.btnGuest.Text = "Смотреть катало без кода";
            this.btnGuest.UseVisualStyleBackColor = false;
            this.btnGuest.Click += new System.EventHandler(this.btnGuest_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(933, 519);
            this.Controls.Add(this.btnGuest);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.txtLogin);
            this.Controls.Add(this.idLogin);
            this.Controls.Add(this.idTitle);
            this.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label idTitle;
        private System.Windows.Forms.Label idLogin;
        private System.Windows.Forms.TextBox txtLogin;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Button btnGuest;
    }
}

