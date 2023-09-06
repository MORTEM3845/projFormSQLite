using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Globalization;

namespace _21212121
{
    public partial class Form4 : Form
    {
        //FilesToShow это список который мы показываем в этом окне
        public List<string> FilesToShow { get; set; }
        //папка со сканами
        public string scansFolderPath = @"E:/work2/scans/";
        public Form4()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;

        }
        //при запуске формы открываетсялистбокс, со списком подходящих файлов, при выборе любого откроется сам файл
        //Split(\t) нужна так как сюда приходит имя файла типа 1(221).pdf много пробелов 18.08.2023, отделяем часть с именем файла, чтобы открыть его
        private void Form4_Load(object sender, EventArgs e)
        {
            if (FilesToShow != null)
            {
                listBox1.Items.AddRange(FilesToShow.ToArray());
            }
            if (listBox1.Items.Count == 1)
            {
                string selectedLine = listBox1.Items[0].ToString();
                string[] parts = selectedLine.Split('\t');
                if (parts.Length > 0)
                {
                    string selectedFile = parts[0];
                    string targetFilePath = Path.Combine(scansFolderPath, selectedFile);
                    OpenPdfFile(targetFilePath); // функция, открывает выбранный файл
                }
                this.Close();
            }
        }
        // функция ниже, просто открывает выбранный файл
        private void OpenPdfFile(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    Process.Start(filePath);
                }
                else
                {
                    MessageBox.Show($"Файл не найден: {filePath}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при открытии файла: {ex.Message}");
            }
        }
        //чтобы пользователь 2 раза по файлу нажал для открытия, удобнее в этом случае

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                string selectedLine = listBox1.SelectedItem.ToString();
                string[] parts = selectedLine.Split('\t');
                if (parts.Length > 0)
                {
                    string selectedFile = parts[0];
                    string targetFilePath = Path.Combine(scansFolderPath, selectedFile);
                    OpenPdfFile(targetFilePath);
                }
            }
        }
       
        
    }
}
