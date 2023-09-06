using System.Windows.Forms;

namespace _21212121
{
    partial class ColumnHeaderControl
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
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.textBox = new System.Windows.Forms.TextBox();
            this.panel = new System.Windows.Forms.FlowLayoutPanel();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(3, 32);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(120, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "По убыванию";
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(3, 116);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(120, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "Поиск";
            this.button2.Click += new System.EventHandler(this.Button2_Click);
            // 
            // textBox
            // 
            this.textBox.Location = new System.Drawing.Point(3, 90);
            this.textBox.Name = "textBox";
            this.textBox.Size = new System.Drawing.Size(120, 20);
            this.textBox.TabIndex = 2;
            // 
            // panel
            // 
            this.panel.Controls.Add(this.button4);
            this.panel.Controls.Add(this.button1);
            this.panel.Controls.Add(this.button3);
            this.panel.Controls.Add(this.textBox);
            this.panel.Controls.Add(this.button2);
            this.panel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.panel.Location = new System.Drawing.Point(3, 5);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(127, 153);
            this.panel.TabIndex = 0;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(3, 3);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(120, 23);
            this.button4.TabIndex = 4;
            this.button4.Text = "По возрастанию";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(3, 61);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(120, 23);
            this.button3.TabIndex = 3;
            this.button3.Text = "Очистить";
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(217, 11);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(252, 110);
            this.dataGridView1.TabIndex = 1;
            // 
            // ColumnHeaderControl
            // 
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.panel);
            this.Name = "ColumnHeaderControl";
            this.Size = new System.Drawing.Size(133, 152);
            this.panel.ResumeLayout(false);
            this.panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private FlowLayoutPanel panel;
        private DataGridView dataGridView1;
        private Button button3;
        private Button button4;
    }
}
