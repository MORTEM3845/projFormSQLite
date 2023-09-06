using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _21212121
{
    public partial class Form3 : Form
    {
        public string Password { get; private set; }

        public Form3()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }
        //ввод пароля и отправка его в Form2
        private void button1_Click(object sender, EventArgs e)
        {
            Password = textBox1.Text;
            if (Password == "123456789") //вписать пароль
            {
                DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show("Неверный пароль. Попробуйте снова.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox1.Text = "";
            }
        }

     
    }
}
