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
    public partial class Form2 : Form
    {
        
        public Form2()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            
        }
        //при запуске отдаем число и список в дальнейший код в Form1
        private void OpenForm1(int numberOfChoice, List<string> projectsOfChoice)
        {
            Form1 form1 = new Form1(numberOfChoice, projectsOfChoice);
            form1.Show();
        }
        
        private void button1_Click_1(object sender, EventArgs e)
        {
            List<string> listOfProjects = new List<string> { "221", "222", "223" }; // список проекта 12200
            OpenForm1(1, listOfProjects); // число один пойдет в my_table1, а список в код поиска файлов
            this.Hide();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            List<string> listOfProjects = new List<string> { "111", "111", "111" };// список проекта 10410
            OpenForm1(2, listOfProjects);
            this.Hide();

        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            List<string> listOfProjects = new List<string> { "333", "333", "333" };// список проекта 21180М
            OpenForm1(3, listOfProjects);
            this.Hide();
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            List<string> listOfProjects = new List<string> { "444", "445", "446" };// список проекта 22120
            OpenForm1(4, listOfProjects);
            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            List<string> listOfProjects = new List<string> { "555", "555", "555" }; // список проекта 02690
            OpenForm1(5, listOfProjects);
            this.Hide();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            List<string> listOfProjects = new List<string> { "666", "666", "666" };// список проекта ВЗО
            OpenForm1(6, listOfProjects); 
            this.Hide();
        }
        // Аналогично можно создавать новые кнопки-проекты
        //private void Нажатие на кнопку, легче всего создать кнопку и дважды на нее нажать откроется private void button_Click {}
        //{
        //    List<string> listOfProjects = new List<string> { список проекта через запятую в кавычках  };  
        //    OpenForm1(пишем новое число, для следующей кнопки 7, listOfProjects); 
        //    this.Hide();
        //}
    }
}
