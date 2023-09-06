using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite; //SQLITE
using System.IO;
using System.Diagnostics;
using Microsoft.Office.Interop.Excel; //Excel
using System.Windows.Media.Media3D;
using System.Windows.Forms.VisualStyles;
using System.Data.Common;
using System.Windows.Controls;
using System.Text.RegularExpressions;



namespace _21212121
{
    public partial class Form1 : Form
    {
        private bool isButton3Enabled = true;
        private int receivedNumberOfChoice; 
        private List<string> receivedProjectNumberOfChoice;
        //private string filePathforzakasi = @"//Dsp/почта_ту/БМРИ/Документооборот/J/zakasi.txt";
        private AutoCompleteStringCollection autoCompleteCollection = new AutoCompleteStringCollection();

        public Form1(int NumberOfChoice, List<string> ProjectNumberOfChoice)
        {
            receivedNumberOfChoice = NumberOfChoice;                    // переменные получаемые из Form2(самая первая форма) - receivedNumberOfChoice для создания
            receivedProjectNumberOfChoice = ProjectNumberOfChoice;      // таблиц по типу my_table1, my_table2 
            InitializeComponent();                                      // a receivedProjectNumberOfChoice  для сортировки файлов в scans, передаются значение типа
            CreateDatabaseAndTable();                                   // 221,222,223 и тп
            LoadDataToDataGridView();

            if (dataGridView1.Columns.Count > 0 & dataGridView1 != null)
            {
                SetColumnWidths();
            }
            // установка стартовых значений для формы, какие элементы скрыты, какие кнопки не активны и тп
            this.MinimumSize = new Size(800, 280);
            panel1.Visible = true;                                          
            textBox1.Text = DateTime.Now.ToString("dd.MM.yyyy");
            button3.Enabled = false;
            StartPosition = FormStartPosition.CenterScreen;
            //_____________________________________________________________
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            //_____________________________________________________________

            // Путь к текстовому файлу со списком заказов ZakSuggestions.txt

            string filePath = @"E:\work2\ZakSuggestions.txt";               
            // string filePath = @"//Dsp/почта_ту/БМРИ/Документооборот/J/ZakSuggestions.txt";
            List<string> suggestions = new List<string>();
            if (File.Exists(filePath))
            {
                suggestions.AddRange(File.ReadAllLines(filePath));
            }
            listBox1.Items.AddRange(suggestions.ToArray());
            autoCompleteCollection.AddRange(suggestions.ToArray());

            textBox2.AutoCompleteMode = AutoCompleteMode.Suggest;
            textBox2.AutoCompleteSource = AutoCompleteSource.CustomSource;
            textBox2.AutoCompleteCustomSource = autoCompleteCollection;


        }
        //Заказ наряд из листбокса в текстбокс
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1) // Проверяем, выбран ли элемент
            {
                textBox2.Text = listBox1.SelectedItem.ToString();
            }
        }
        //Установка ширины столбцов таблицы, в процентах от всей ширины
        private void SetColumnWidths()
        {
            if (dataGridView1.Rows.Count > 0 && dataGridView1.Columns.Count > 0)
            {
                int totalWidth = dataGridView1.Width - dataGridView1.RowHeadersWidth;
                dataGridView1.Columns["Дата"].Width = (int)(totalWidth * 0.075);
                dataGridView1.Columns["Номер_наряда"].Width = (int)(totalWidth * 0.1);
                dataGridView1.Columns["Заказ_наряд"].Width = (int)(totalWidth * 0.09);
                dataGridView1.Columns["Исполнитель"].Width = (int)(totalWidth * 0.15);
                dataGridView1.Columns["Описание_работ"].Width = (int)(totalWidth * 0.370);
                dataGridView1.Columns["Основание"].Width = (int)(totalWidth * 0.185);
            }
        }
        //Выбор базы данных и формирование таблиц из основы my_table и числа которое получено из Form2(первая форма при запуске), получаются таблицы с именами my_table1, my_table2 и тп
        private void CreateDatabaseAndTable()
        {
            //string connectionString = @"Data Source=//Dsp/почта_ту/БМРИ/Документооборот/J/database.db;Version=3;";
            string connectionString = @"Data Source=E:/work2/database.db;Version=3;"; //Выбор базы данных
            string tableintobdName = "my_table" + receivedNumberOfChoice;             //формирование таблиц из основы my_table и числа которое получено из Form2

            // Создаем базу данных
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();


                // Создаем таблицу
                using (SQLiteCommand cmd = new SQLiteCommand("CREATE TABLE IF NOT EXISTS " + tableintobdName + " ( Дата TEXT, Номер_наряда INTEGER PRIMARY KEY, Заказ_наряд TEXT, Исполнитель TEXT, Описание_работ TEXT, Основание TEXT)", connection))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }
        // Загрузка базы данных в DataGridView1 (таблицы в самом приложении)
        private void LoadDataToDataGridView()
        {
            //string connectionString = @"Data Source=//Dsp/почта_ту/БМРИ/Документооборот/J/database.db;Version=3;";
            string connectionString = @"Data Source=E:/work2/database.db;Version=3;";
            string tableintobdName = "my_table" + receivedNumberOfChoice;

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                // Выборка данных из таблицы
                string selectQuery = "SELECT * FROM " + tableintobdName;
                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(selectQuery, connection))
                {
                    System.Data.DataTable dataTable = new System.Data.DataTable();
                    adapter.Fill(dataTable);

                    // Очищаем существующие столбцы (если есть)
                    dataGridView1.Columns.Clear();

                    // Добавляем столбцы к DataGridView
                    dataGridView1.DataSource = dataTable;

                    // Автоскролл вниз таблицы
                    ScrollToBottom(dataGridView1);
                    dataGridView1.ColumnHeaderMouseClick += DataGridView_ColumnHeaderMouseClick;
                }
            }
        }
        //открытие панели с поиском и сортировкой столбцов. columnHeaderControl это созданная панель, у нее отдельный файл ColumnHeaderControl.cs, по факту
        //это панель, с элементами, скрывается и появляется при нажатии по заголовку  таблицы
        private void DataGridView_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.RowIndex == -1 && e.ColumnIndex >= 0)
            {
                DataGridViewColumn column = dataGridView1.Columns[e.ColumnIndex];

                if (column.HeaderCell.Tag == null || !(column.HeaderCell.Tag is ColumnHeaderControl))
                {
                    ColumnHeaderControl columnHeaderControl = new ColumnHeaderControl(dataGridView1);

                    // Привязываем пользовательский элемент к тегу заголовка
                    column.HeaderCell.Tag = columnHeaderControl;

                    columnHeaderControl.CopyDataFromMainDataGridView(dataGridView1);

                    // Устанавливаем координаты для отображения пользовательского элемента
                    int xOffset = 10; // Горизонтальное смещение
                    int yOffset = 22; // Вертикальное смещение
                    columnHeaderControl.Location = new System.Drawing.Point(dataGridView1.GetColumnDisplayRectangle(e.ColumnIndex, false).Left + xOffset, yOffset);

                    // Добавляем пользовательский элемент в DataGridView
                    dataGridView1.Controls.Add(columnHeaderControl);

                    columnHeaderControl.Button2Click += (s, args) =>
                    {
                        string columnHeader = column.HeaderText;

                        // В зависимости от выбранного заголовка, вызываем соответствующую функцию
                        // FunctionForHeader - это функция поиска, описана в columnHeaderControl, FunctionForHeader1 для первого заголовка "Дата" и далее аналогично
                        switch (columnHeader)
                        {
                            case "Дата":
                                columnHeaderControl.FunctionForHeader1();
                                break;
                            // Вызываем функцию для Заголовка2 - номер наряда
                            case "Номер_наряда":
                                columnHeaderControl.FunctionForHeader2();
                                break;
                            case "Заказ_наряд":
                                columnHeaderControl.FunctionForHeader3();
                                break;
                            case "Исполнитель":
                                columnHeaderControl.FunctionForHeader4();
                                break;
                            case "Описание_работ":
                                columnHeaderControl.FunctionForHeader5();
                                break;
                            case "Основание":
                                columnHeaderControl.FunctionForHeader6();
                                break;

                        }
                    };
                    columnHeaderControl.Button1Click += (s, args) =>
                    {
                        string columnHeader = column.HeaderText;

                        // В зависимости от выбранного заголовка, вызываем соответствующую функцию
                        // FunctionDownForHeader - функция для сортировки по убыванию, аналогично описана в columnHeaderControl
                        switch (columnHeader)
                        {
                            case "Дата":
                                columnHeaderControl.FunctionDownForHeader1();
                                break;
                            // Вызываем функцию для Заголовка2
                            case "Номер_наряда":
                                columnHeaderControl.FunctionDownForHeader2();
                                break;
                            case "Заказ_наряд":
                                columnHeaderControl.FunctionDownForHeader3();
                                break;
                            case "Исполнитель":
                                columnHeaderControl.FunctionDownForHeader4();
                                break;
                            case "Описание_работ":
                                columnHeaderControl.FunctionDownForHeader5();
                                break;
                            case "Основание":
                                columnHeaderControl.FunctionDownForHeader6();
                                break;

                        }
                    };
                    columnHeaderControl.Button4Click += (s, args) =>
                    {
                        string columnHeader = column.HeaderText;

                        // В зависимости от выбранного заголовка, вызываем соответствующую функцию
                        // FunctionDownForHeader - функция для сортировки по возрастанию, аналогично описана в columnHeaderControl
                        switch (columnHeader)
                        {
                            case "Дата":
                                columnHeaderControl.FunctionUpForHeader1();
                                break;
                            // Вызываем функцию для Заголовка2
                            case "Номер_наряда":
                                columnHeaderControl.FunctionUpForHeader2();
                                break;
                            case "Заказ_наряд":
                                columnHeaderControl.FunctionUpForHeader3();
                                break;
                            case "Исполнитель":
                                columnHeaderControl.FunctionUpForHeader4();
                                break;
                            case "Описание_работ":
                                columnHeaderControl.FunctionUpForHeader5();
                                break;
                            case "Основание":
                                columnHeaderControl.FunctionUpForHeader6();
                                break;

                        }
                    };
                    dataGridView1.MouseClick += (s, args) =>
                    {
                        if (!columnHeaderControl.ClientRectangle.Contains(columnHeaderControl.PointToClient(args.Location)))
                        {
                            dataGridView1.Controls.Remove(columnHeaderControl);

                            column.HeaderCell.Tag = null;
                        }
                    };
                }
            }
        }

        // запрещаем вводить больше 6 символов в заказ наряд
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            string inputText = textBox2.Text;

            if (inputText.Length >= 6)
            {
                textBox2.Text = textBox2.Text.Substring(0, 6);
                textBox2.SelectionStart = 6;
            }

        }
        // функция для того, чтобы при запуске приложения показывался последний элемент таблицы
        private void ScrollToBottom(DataGridView dgv)
        {
            if (dgv.Rows.Count > 0)
            {
                dgv.FirstDisplayedScrollingRowIndex = dgv.Rows.Count - 1;
                dgv.CurrentCell = dgv[0, dgv.Rows.Count - 1];
            }
        }
        // Кнопка Создать запись. также подключаемся к базе данных, выбираем таблицу
        private void button1_Click(object sender, EventArgs e)
        {
            //string connectionString = @"Data Source=//Dsp/почта_ту/БМРИ/Документооборот/J/database.db;Version=3;";
            string connectionString = @"Data Source=E:/work2/database.db;Version=3;";
            string tableintobdName = "my_table" + receivedNumberOfChoice;

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string insertQuery = "INSERT INTO " + tableintobdName + " (Дата, Заказ_наряд, Исполнитель, Описание_работ, Основание) VALUES (@column1, @column2, @column3, @column4, @column5)";

                using (SQLiteCommand cmd = new SQLiteCommand(insertQuery, connection))
                {
                    cmd.Parameters.AddWithValue("@column1", textBox1.Text);
                    cmd.Parameters.AddWithValue("@column2", textBox2.Text);
                    cmd.Parameters.AddWithValue("@column3", textBox3.Text);
                    cmd.Parameters.AddWithValue("@column4", textBox4.Text);
                    cmd.Parameters.AddWithValue("@column5", textBox5.Text);

                    cmd.ExecuteNonQuery();
                }
            }
            //после создания записи обнуляем значения в тексбоксах
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            MessageBox.Show("Новая запись создана");
            LoadDataToDataGridView(); // Обновить DataGridView после добавления записи
        }
        // Кнопка редактировать. Открывает форму 3, где нужно ввести пароль, после чего можно редактировать таблицу и созранить изменения по кнопке
        private void button2_Click(object sender, EventArgs e)
        {
            Form3 passwordForm = new Form3();
            if (passwordForm.ShowDialog() == DialogResult.OK)
            {
                if (passwordForm.Password == "123456789") // выбираете пароль здесь
                {
                    isButton3Enabled = true;
                    dataGridView1.AllowUserToAddRows = true;
                    dataGridView1.ReadOnly = false;
                    button3.Enabled = true;
                    textBox1.ReadOnly = false;
                }
            }
        }
        // Кнопка сохранить изменения. Сохранит все изменения в таблице
        private void button3_Click(object sender, EventArgs e)
        {
            //string connectionString = @"Data Source=//Dsp/почта_ту/БМРИ/Документооборот/J/database.db;Version=3;";
            string connectionString = @"Data Source=E:/work2/database.db;Version=3;";
            string tableintobdName = "my_table" + receivedNumberOfChoice;

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    using (SQLiteDataAdapter adapter = new SQLiteDataAdapter("SELECT * FROM " + tableintobdName, connection))
                    {
                        using (SQLiteCommandBuilder commandBuilder = new SQLiteCommandBuilder(adapter))
                        {
                            System.Data.DataTable dataTable = ((System.Data.DataTable)dataGridView1.DataSource);
                            adapter.Update(dataTable); // Сохранение изменений в базу данных
                        }
                    }
                    MessageBox.Show("Изменения сохранены");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при сохранении изменений: {ex.Message}");
                }
            }

        }
        // Кнопка вернуться к выбору проектов
        private void button5_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Show();
            this.Hide();
        }
        // Вывод в эксель, можно добавить форму ( ссылаться на существующий Excel файл а не создавать новый как тут ) 
        // Была проведена аналогичная работа в предыдущем проекте, можно взять логику работы с эксель от туда, пока что создает новый файл, и тупо копирует
        // заголовки и значения в ексель
        private void ExportSelectedRowsToExcel(List<int> selectedIds)
        {
            Microsoft.Office.Interop.Excel.Application excelApp = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook workbook = excelApp.Workbooks.Add();
            Microsoft.Office.Interop.Excel.Worksheet worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Sheets[1];

            // Создаем заголовки
            for (int col = 1; col <= dataGridView1.Columns.Count; col++)
            {
                worksheet.Cells[1, col] = dataGridView1.Columns[col - 1].HeaderText;
            }
            int row = 2; // Начинаем со второй строки
            // Добавляем выбранные строки в Excel
            foreach (int id in selectedIds)
            {
                DataGridViewRow dgvRow = dataGridView1.Rows
                    .Cast<DataGridViewRow>()
                    .FirstOrDefault(r => Convert.ToInt32(r.Cells["Номер_наряда"].Value) == id);

                if (dgvRow != null)
                {
                    for (int col = 1; col <= dataGridView1.Columns.Count; col++)
                    {
                        worksheet.Cells[row, col] = dgvRow.Cells[col - 1].Value;
                    }
                    row++;
                }
            }
            excelApp.Visible = true; // Открываем Excel для просмотра
        }
        //Кнопка вызывающая функцию открытия екселя. Можно вводить числа через запяту, вводить числа через -, или просто выбрать нужные строки вручную
        //так например можно выбрать 1-3, 7,8 -> в екселе будут строчки с номерами наряда 1,2,3,7,8
        private void button4_Click(object sender, EventArgs e)
        {

            List<int> selectedIds = new List<int>();

            string[] idRanges = textBox7.Text.Split(',');

            foreach (string idRange in idRanges)
            {
                if (idRange.Contains("-"))
                {
                    string[] rangeLimits = idRange.Split('-');
                    if (rangeLimits.Length == 2 && int.TryParse(rangeLimits[0], out int start) && int.TryParse(rangeLimits[1], out int end))
                    {
                        for (int i = start; i <= end; i++)
                        {
                            selectedIds.Add(i);
                        }
                    }
                }
            }

            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                if (int.TryParse(row.Cells["Номер_наряда"].Value.ToString(), out int id))
                {
                    selectedIds.Insert(0, id); // Вставляем id в начало списка
                }
            }

            if (selectedIds.Count > 0)
            {
                ExportSelectedRowsToExcel(selectedIds);
            }
        }
        //копирование любого файла в файл пдф, в котором имя файла это номер наряда в выбранной строке при нажатии на кнопку. Также можно вписать номер наряда
        //в текстовое поле, работает аналогично
        private void button7_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Получаем выбранный файл
                    string selectedFilePath = openFileDialog.FileName;

                    bool hasSelectedRows = dataGridView1.SelectedRows.Count > 0;

                    if (!hasSelectedRows && int.TryParse(textBox7.Text, out int orderNumberFromTextBox))
                    {
                        // Создаем путь для целевого файла с новым именем, сохраняя оригинальное расширение
                        //string targetFilePath = $@"\\Dsp\почта_ту\БМРИ\Документооборот\J\scans\{orderNumberFromTextBox}{Path.GetExtension(selectedFilePath)}";
                        string targetFilePath = $@"E:/work2/scans/{orderNumberFromTextBox}{Path.GetExtension(selectedFilePath)}";
                        // Копируем файл
                        File.Copy(selectedFilePath, targetFilePath, true); // true для перезаписи файла, если он уже существует

                        MessageBox.Show($"Файл успешно скопирован с именем {orderNumberFromTextBox}{Path.GetExtension(selectedFilePath)}.\nПуть: {targetFilePath}");
                    }
                    else if (hasSelectedRows)
                    {
                        DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
                        string orderNumber = selectedRow.Cells["Номер_наряда"].Value.ToString();

                        // Создаем путь для целевого файла с новым именем, сохраняя оригинальное расширение
                        //string targetFilePath = $@"\\Dsp\почта_ту\БМРИ\Документооборот\J\scans\{orderNumber}{Path.GetExtension(selectedFilePath)}";
                        string targetFilePath = $@"E:/work2/scans/{orderNumber}{Path.GetExtension(selectedFilePath)}";
                        // Копируем файл
                        File.Copy(selectedFilePath, targetFilePath, true); // true для перезаписи файла, если он уже существует

                        MessageBox.Show($"Файл успешно скопирован с именем {orderNumber}{Path.GetExtension(selectedFilePath)}.\nПуть: {targetFilePath}");
                    }
                    else
                    {
                        MessageBox.Show("Выберите строку или введите номер наряда в поле.");
                    }
                }
            }
        }



        //_________________________________________________________________________________________________________

        // string fileDate = File.GetLastWriteTime(filePath).ToString("dd.MM.yyyy HH:mm:ss");
        //string fileDate = File.GetCreationTime(filePath).ToString("dd.MM.yyyy HH:mm:ss");
        //Получения файла по выделенной строке
        private void button9_Click(object sender, EventArgs e)
        {
            int orderNumberFromTextBox = 0; // Присвоить значение по умолчанию

            if (dataGridView1.SelectedRows.Count > 0 || int.TryParse(textBox7.Text, out orderNumberFromTextBox))
            {
                string orderNumber = "";

                if (dataGridView1.SelectedRows.Count > 0)
                {
                    DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
                    orderNumber = selectedRow.Cells["Номер_наряда"].Value.ToString();
                }
                else
                {
                    orderNumber = orderNumberFromTextBox.ToString();
                }
                //папка со сканами
                string scansFolderPath = @"E:/work2/scans/";

                string[] allFiles = Directory.GetFiles(scansFolderPath);

                List<string> matchingFiles = new List<string>();
                List<string> additionalMatchingFiles = new List<string>();

                DateTime comparisonDate = new DateTime(2013, 1, 1); // Дата до которой файлы не будут учитываться при поиске

                foreach (string filePath in allFiles)
                {
                    string fileName = Path.GetFileName(filePath);

                    if (fileName.StartsWith(orderNumber))
                    {
                        DateTime fileModifiedDate = File.GetLastWriteTime(filePath); //вместо GetLastWriteTime можно использовать GetCreationTime, смотря какая дата нужна
                        //additionalMatchingFiles.Add(fileName);
                        if (fileModifiedDate >= comparisonDate) // Сравнение даты изменения файла
                        {
                            foreach (string projectNumber in receivedProjectNumberOfChoice)
                            {
                                string projectSubstring = $"({projectNumber}";

                                if (fileName.Contains(projectSubstring))
                                {
                                    string fileDate = fileModifiedDate.ToString("dd.MM.yyyy"); // как будет выгядеть выведенная дата, можно менять на "dd.MM.yyyy HH:mm:ss" для добавления времени

                                    // ниже код для табуляции, чтобы дата была в правой части окна при выводе списка файлов, предполагается что длина файла не больше 50
                                    // можно менять, но тогда не забыть поменять размеры окна вывода
                                    int maxFileNameLength = 50; // Максимальная длина имени файла
                                    int dotsCount = maxFileNameLength - fileName.Length;

                                    if (dotsCount < 0)
                                    {
                                        fileName = fileName.Substring(0, maxFileNameLength);
                                        dotsCount = 0;
                                    }

                                    string dots = new string(' ', dotsCount);
                                    string formattedText = $"{fileName}{dots}\t{fileDate}"; // Используем \t для табуляции
                                    string fileWithDate = $"{formattedText}";
                                    additionalMatchingFiles.Add(formattedText);
                                }

                            }
                        }
                    }
                }

                if (additionalMatchingFiles.Count > 0)
                {
                    // Вывод списка файлов, подходящих по всем критериям
                    string filesList = string.Join(", ", additionalMatchingFiles);
                    List<string> filesListArray = filesList.Split(new string[] { ", " }, StringSplitOptions.None).ToList();

                    Form4 Formchoicefile = new Form4();
                    Formchoicefile.FilesToShow = filesListArray;
                    Formchoicefile.Show();
                }
                else if (matchingFiles.Count > 0)
                {
                    // Вывод списка файлов, подходящих по orderNumber и receivedProjectNumberOfChoice
                    string filesList = string.Join(", ", matchingFiles);
                    List<string> filesListArray = filesList.Split(new string[] { ", " }, StringSplitOptions.None).ToList();

                    Form4 Formchoicefile = new Form4();
                    Formchoicefile.FilesToShow = filesListArray;
                    Formchoicefile.Show();
                }
                else
                {
                    MessageBox.Show($"Файлы с номером {orderNumber} и проектами из списка не найдены.");
                }
            }
        }

        //Кнопка рядом с редактировать, чтобы скрывать и показывать окно с доп функциями
        private void button6_Click(object sender, EventArgs e)
        {
            panel1.Visible = !panel1.Visible;
        }
        //Кнопка рядом с Заказ наряд, чтобы скрывать и показывать окно листом заказов
        private void button10_Click(object sender, EventArgs e)
        {
            listBox1.Visible = !listBox1.Visible;
        }
        // можно скопировать из одной папки(sourceFolderPath) все пдф в другую папку (destinationFolderPath)
        // Идея в том, что файлы будут не просто из папки скопированы, но и из всех подпапок в этой папке, можно одним кликом все файлы из подпапок с датами, получить все файлы
        //кнопка скрыта
        private void button11_Click(object sender, EventArgs e)
        {
            string sourceFolderPath = @"E:\work2"; //папка из которой берем
            string destinationFolderPath = @"E:\publishedone"; //в которую копируем

            if (!Directory.Exists(destinationFolderPath))
            {
                Directory.CreateDirectory(destinationFolderPath);
            }

            string[] pdfFiles = Directory.GetFiles(sourceFolderPath, "*.pdf", SearchOption.AllDirectories);

            foreach (string filePath in pdfFiles)
            {
                string fileName = Path.GetFileName(filePath);
                string destinationFilePath = Path.Combine(destinationFolderPath, fileName);

                File.Copy(filePath, destinationFilePath, true); // Копирование файла

            }
            MessageBox.Show("COMPLITED");
        }
    }
   
}
