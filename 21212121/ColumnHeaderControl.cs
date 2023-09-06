using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _21212121
{
    //пользовательский элемент панель с элементами
    public partial class ColumnHeaderControl : UserControl
    {
        //Для сортировки в этой панеле есть DataGridView в который я копирую оригинальную таблицу сортирую все в этой панеле, а потом 
        //вывожу выделенные строки в оригинальную DataGridView
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox textBox;
        private List<DataGridViewCell> highlightedCells = new List<DataGridViewCell>();
        private DataGridView mainDataGridView;

        public ColumnHeaderControl(DataGridView mainDgv)
        {
            InitializeComponent();
            button2.Click += Button2_Click;
            button4.Click += Button4_Click;
            button1.Click += Button1_Click;
            mainDataGridView = mainDgv; // Оригинальная таблица

        }

        public event EventHandler Button2Click; //поиск
        public event EventHandler Button4Click; //по возрастанию
        public event EventHandler Button1Click; //по убыванию

        //вызов функций поиска и сортировки
        private void Button2_Click(object sender, EventArgs e)
        {
            Button2Click?.Invoke(this, EventArgs.Empty);
        }
        private void Button4_Click(object sender, EventArgs e)
        {
            Button4Click?.Invoke(this, EventArgs.Empty);
        }
        private void Button1_Click(object sender, EventArgs e)
        {
            Button1Click?.Invoke(this, EventArgs.Empty);
        }
        //________________________________________________________________________________________________________________________
        //Сортировка по убыванию для столбцов ( тут только вызов функции SortDataGridViewColumnDescending к определенным столбцам, для даты отделаьная функция )
        //из-за другого типа данных
        public void FunctionDownForHeader1()
        {
            SortDataGridViewByDate(dataGridView1, "Дата", ListSortDirection.Descending);
            this.Visible = false;
        }
        public void FunctionDownForHeader2()
        {
            SortDataGridViewColumnDescending(dataGridView1, "Номер_наряда");
            this.Visible = false;
        }
        public void FunctionDownForHeader3()
        {
            SortDataGridViewColumnDescending(dataGridView1, "Заказ_наряд");
            this.Visible = false;
        }
        public void FunctionDownForHeader4()
        {
            SortDataGridViewColumnDescending(dataGridView1, "Исполнитель");
        }
        public void FunctionDownForHeader5()
        {
            SortDataGridViewColumnDescending(dataGridView1, "Описание_работ");
        }
        public void FunctionDownForHeader6()
        {
            SortDataGridViewColumnDescending(dataGridView1, "Основание");
        }
        //функция сортировки по убыванию
        private void SortDataGridViewColumnDescending(DataGridView dataGridView, string columnName)
        {
            if (dataGridView.Columns.Contains(columnName))
            {
                // Получаем индекс столбца по имени
                int columnIndex = dataGridView.Columns[columnName].Index;

                // Устанавливаем Comparer для сортировки по убыванию
                dataGridView.Sort(dataGridView.Columns[columnIndex], ListSortDirection.Descending);
            }
            
        }
        //Сортировка по датам, но не работают года
        private void SortDataGridViewByDate(DataGridView dataGridView, string columnName, ListSortDirection sortDirection)
        {

            if (dataGridView.Columns.Contains(columnName))
            {
                DataGridViewColumn column = dataGridView.Columns[columnName];
                DataTable dataTable = (DataTable)dataGridView.DataSource;

                if (dataTable != null)
                {
                    string sortExpression = $"{columnName} {(sortDirection == ListSortDirection.Ascending ? "ASC" : "DESC")}";
                    dataTable.DefaultView.Sort = sortExpression;

                    dataGridView.DataSource = dataTable;
                }
            }
        }
        //________________________________________________________________________________________________________________________
        public void FunctionUpForHeader1()
        {
            SortDataGridViewByDate(dataGridView1, "Дата", ListSortDirection.Ascending);
            this.Visible = !this.Visible;
        }
        public void FunctionUpForHeader2()
        {
            SortDataGridViewColumnAscending(dataGridView1, "Номер_наряда");
            this.Visible = !this.Visible;
        }
        public void FunctionUpForHeader3()
        {
            SortDataGridViewColumnAscending(dataGridView1, "Заказ_наряд");
            this.Visible = !this.Visible;
        }
        public void FunctionUpForHeader4()
        {
            SortDataGridViewColumnAscending(dataGridView1, "Исполнитель");
            this.Visible = !this.Visible;
        }
        public void FunctionUpForHeader5()
        {
            SortDataGridViewColumnAscending(dataGridView1, "Описание_работ");
            this.Visible = !this.Visible;
        }
        public void FunctionUpForHeader6()
        {
            SortDataGridViewColumnAscending(dataGridView1, "Основание");
            this.Visible = !this.Visible;
        }
        private void SortDataGridViewColumnAscending(DataGridView dataGridView, string columnName)
        {
            if (dataGridView.Columns.Contains(columnName))
            {
                // Получаем индекс столбца по имени
                int columnIndex = dataGridView.Columns[columnName].Index;

                // Устанавливаем Comparer для сортировки по возрастанию
                dataGridView.Sort(dataGridView.Columns[columnIndex], ListSortDirection.Ascending);
            }
            
        }
        //________________________________________________________________________________________________________________________
        public void FunctionForHeader1()
        {
            string inputText = textBox.Text.Trim();


            if (DateTime.TryParseExact(inputText, "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime date))
            {
                // Введена одна дата
                SearchAndHighlightByDate(date, date);
            }
            else if (inputText.Contains("-"))
            {
                // Введен диапазон дат
                string[] dateRange = inputText.Split('-');

                if (dateRange.Length == 2 && DateTime.TryParseExact(dateRange[0].Trim(), "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime startDate) &&
                    DateTime.TryParseExact(dateRange[1].Trim(), "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime endDate))
                {
                    SearchAndHighlightByDate(startDate, endDate);
                }
            }
            this.Visible = !this.Visible;
            ApplyHighlightToMainDataGridView();
        }

        public void FunctionForHeader2()
        {
            string searchValue = textBox.Text.Trim();

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    if (cell.Value != null && cell.Value.ToString().Equals(searchValue))
                    {
                        row.DefaultCellStyle.BackColor = Color.Yellow;
                        highlightedCells.AddRange(row.Cells.Cast<DataGridViewCell>());
                        dataGridView1.FirstDisplayedScrollingRowIndex = row.Index;
                        ApplyHighlightToMainDataGridView();
                        return; // Выходим из функции после первого совпадения
                    }
                    highlightedCells.Clear();
                }
            }
            this.Visible = !this.Visible;

        }
        public void FunctionForHeader3()
        {
            string searchValue = textBox.Text.Trim();

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    if (cell.Value != null && cell.Value.ToString().Equals(searchValue))
                    {
                        row.DefaultCellStyle.BackColor = Color.Yellow;
                        highlightedCells.AddRange(row.Cells.Cast<DataGridViewCell>());
                        dataGridView1.FirstDisplayedScrollingRowIndex = row.Index;
                        ApplyHighlightToMainDataGridView();
                        return; 
                    }
                    highlightedCells.Clear();
                }
            }
            this.Visible = !this.Visible;

        }
        public void FunctionForHeader4()
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                DataGridViewCell descriptionCell = row.Cells["Исполнитель"];
                if (descriptionCell != null && descriptionCell.Value != null &&
                    descriptionCell.Value.ToString().IndexOf(textBox.Text, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    row.DefaultCellStyle.BackColor = Color.Yellow;
                    highlightedCells.AddRange(row.Cells.Cast<DataGridViewCell>());
                    mainDataGridView.FirstDisplayedScrollingRowIndex = row.Index;
                    ApplyHighlightToMainDataGridView();
                }
                else
                {
                    row.DefaultCellStyle.BackColor = dataGridView1.DefaultCellStyle.BackColor;
                    ApplyHighlightToMainDataGridView();
                }
                
            }
            this.Visible = !this.Visible;
        }
        public void FunctionForHeader5()
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                DataGridViewCell descriptionCell = row.Cells["Описание_работ"];
                if (descriptionCell != null && descriptionCell.Value != null &&
                    descriptionCell.Value.ToString().IndexOf(textBox.Text, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    row.DefaultCellStyle.BackColor = Color.Yellow;
                    highlightedCells.AddRange(row.Cells.Cast<DataGridViewCell>());
                    mainDataGridView.FirstDisplayedScrollingRowIndex = row.Index;
                    ApplyHighlightToMainDataGridView();
                }
                else 
                {
                    row.DefaultCellStyle.BackColor = dataGridView1.DefaultCellStyle.BackColor;
                    ApplyHighlightToMainDataGridView();
                }
                
            }
            this.Visible = !this.Visible;
        }
        public void FunctionForHeader6()
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                DataGridViewCell descriptionCell = row.Cells["Основание"];
                if (descriptionCell != null && descriptionCell.Value != null &&
                    descriptionCell.Value.ToString().IndexOf(textBox.Text, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    row.DefaultCellStyle.BackColor = Color.Yellow;
                    highlightedCells.AddRange(row.Cells.Cast<DataGridViewCell>());
                    mainDataGridView.FirstDisplayedScrollingRowIndex = row.Index;
                    ApplyHighlightToMainDataGridView();
                }
                else
                {
                    row.DefaultCellStyle.BackColor = dataGridView1.DefaultCellStyle.BackColor;
                    
                }            
            }
            this.Visible = !this.Visible;
        }
     
        public void CopyDataFromMainDataGridView(DataGridView mainDataGridView)
        {
            dataGridView1.DataSource = mainDataGridView.DataSource;
        }
        public void ApplyHighlightToMainDataGridView()
        {
            foreach (DataGridViewCell cell in highlightedCells)
            {
                mainDataGridView.Rows[cell.RowIndex].Cells[cell.ColumnIndex].Style.BackColor = Color.Yellow;
            }

            if (highlightedCells.Count > 0)
            {
                int rowIndex = highlightedCells[0].RowIndex;
                if (rowIndex >= 0 && rowIndex < mainDataGridView.Rows.Count)
                {
                    mainDataGridView.FirstDisplayedScrollingRowIndex = rowIndex;
                }
            }
        }
        private void ClearHighlightInMainDataGridView()
        {
            foreach (DataGridViewRow row in mainDataGridView.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    cell.Style.BackColor = mainDataGridView.DefaultCellStyle.BackColor;
                }
            }
        }
        private void ClearHighlightInDataGridView()
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    cell.Style.BackColor = dataGridView1.DefaultCellStyle.BackColor;
                }
            }
        }

        private void SearchAndHighlightByDate(DateTime startDate, DateTime endDate)
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells["Дата"].Value is string dateString)
                {
                    if (DateTime.TryParseExact(dateString, "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime dateValue))
                    {
                        if (dateValue.Date >= startDate.Date && dateValue.Date <= endDate.Date)
                        {
                            row.DefaultCellStyle.BackColor = Color.Yellow;
                            highlightedCells.AddRange(row.Cells.Cast<DataGridViewCell>());
                        }
                        else
                        {
                            row.DefaultCellStyle.BackColor = dataGridView1.DefaultCellStyle.BackColor;
                        }
                    }
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SortDataGridViewColumnAscending(dataGridView1, "Номер_наряда");
            highlightedCells.Clear();
            ClearHighlightInMainDataGridView();
            ClearHighlightInDataGridView();
            this.Visible = !this.Visible;
            this.Focus();
        }

    }
}
