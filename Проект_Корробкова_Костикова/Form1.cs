using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
using System.IO;


namespace Проект_Корробкова_Костикова
{
    public partial class Form1 : Form
    {

        private SQLiteConnection SQLiteConn;    //содержит информацию о подключении к БД
        private DataTable STable;               //таблица данных

        public Form1()
        {
            InitializeComponent();
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            textBox1.Enabled = false;
            textBox3.Enabled = false;
            comboBox1.Enabled = false;
            comboBox2.Enabled = false;
            comboBox3.Enabled = false;
            button6.Enabled = false;
            button7.Enabled = false;
            button8.Enabled = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            SQLiteConn = new SQLiteConnection();
            STable = new DataTable();
        }

        string fileName; //переменные хранящие путь и имя для  открытия БД 
        string filePath;

        //функция запрашивает путь к файлу БД и осуществляет подключение
        private bool OpenDBFile()
        {
            //создание диалогового окна
            OpenFileDialog openFileDialog = new OpenFileDialog();
            //установка начальной папки в диалогово окне
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            //установка фильтра файлов по расширению
            openFileDialog.Filter = "Файлы БД (*.db; *.sqlite)|*.db; *sqlite|Все файлы (*.*)|*.*";
            //отображение диалогового окна и проверка выбора файла
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                //создание подключения к БД
                fileName = Path.GetFileNameWithoutExtension(openFileDialog.FileName);
                filePath = Path.GetDirectoryName(openFileDialog.FileName);
                SQLiteConn = new SQLiteConnection("Data Source=" + openFileDialog.FileName + ";Version=3;");
                SQLiteConn.Open();
                SQLiteCommand command = new SQLiteCommand();
                command.Connection = SQLiteConn;
                return true;
            }
            else return false;
        }

        //функция заполнения comboBox1 названиями таблиц из БД
        private void GetTableNames()
        {
            //создание запроса к служебной таблице для получения списка таблиц
            string SQLQuery = "SELECT name FROM sqlite_master WHERE type='table' ORDER BY name;";
            SQLiteCommand command = new SQLiteCommand(SQLQuery, SQLiteConn);
            //выполнение запроса и получение списка
            SQLiteDataReader reader = command.ExecuteReader();
        
        }

        //функция заполнения dTable данными из БД
        private void ZapolTable()
        {
            STable.Clear();
            //выполнение SQL-запроса и заполнение таблицы dTable
            SQLiteDataAdapter adapter = new SQLiteDataAdapter("SELECT * FROM [" + "Запросы" + "] order by 1", SQLiteConn);
            adapter.Fill(STable);
        }


        //функция для отображения таблицы данных dTable в компоненте dataGridView
        private void ShowTable(DataTable BDTable, DataGridView Table)
        {
            //очистка dataGridView
            Table.Columns.Clear();
            Table.Rows.Clear();

            //создание столбцов в табличном компоненте dataGridView
            for (int col = 0; col < BDTable.Columns.Count; col++)
            {
                string ColName = BDTable.Columns[col].ColumnName;
                Table.Columns.Add(ColName, ColName);
                //автоширина столбцов по содержимому
                Table.Columns[col].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }

            //заполнение строк в табличном компоненте dataGridView
            for (int row = 0; row < BDTable.Rows.Count; row++)
            {
                Table.Rows.Add(BDTable.Rows[row].ItemArray);
            }
            Table.AutoResizeRows();
            Table.AutoResizeColumns();
        }


        //проверка ввода Введите сумму - textbox3
        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {

            if ((e.KeyChar >= '0') && (e.KeyChar <= '9')) //разрешен ввод чисел от 0 до 9
            {
                return;
            }

            if (e.KeyChar == '.') //если пользователь случайно ввел точку - исправляем на запятую
            {
                e.KeyChar = ',';
            }

            if (e.KeyChar == ',') //запрет на ввод двух запятых
            {
                if (textBox3.Text.IndexOf(',') != -1)
                {
                    e.Handled = true;
                }
                return;
            }

            if (Char.IsControl(e.KeyChar))  //Enter, Backspace, Escape - разрешены для нажатия
            {
                return;
            }

            e.Handled = true; //запрет на ввод других символов не предусмотренный в этой функции
        }

        //проверка ввода Введите сумму - textbox1 
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {

            if ((e.KeyChar >= '0') && (e.KeyChar <= '9')) //разрешен ввод чисел от 0 до 9
            {
                return;
            }

            if (e.KeyChar == '.') //если пользователь случайно ввел точку - исправляем на запятую
            {
                e.KeyChar = ',';
            }

            if (e.KeyChar == ',') //запрет на ввод двух запятых
            {
                if (textBox1.Text.IndexOf(',') != -1)
                {
                    e.Handled = true;
                }
                return;
            }

            if (Char.IsControl(e.KeyChar))  //Enter, Backspace, Escape - разрешены для нажатия
            {
                return;
            }

            e.Handled = true; //запрет на ввод других символов не предусмотренный в этой функции
        }

        public void perevod()
        {
            double sum1, rez, kurs;

            sum1 = Convert.ToDouble(textBox1.Text);
            kurs = Convert.ToDouble(textBox3.Text);
            rez = sum1 * kurs;

            label6.Text = Convert.ToDouble(textBox1.Text) + Convert.ToString(comboBox1.SelectedItem) + " = " + rez
                + Convert.ToString(comboBox2.SelectedItem);

            listBox1.Items.Add(Convert.ToDouble(textBox1.Text) + Convert.ToString(comboBox1.SelectedItem) + " = " + rez
                + Convert.ToString(comboBox2.SelectedItem));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            comboBox3.Enabled = true;
            button6.Enabled = true;
            button7.Enabled = true;
            button8.Enabled = true;

            string from, to;

            from = comboBox1.SelectedItem.ToString();
            to = comboBox2.SelectedItem.ToString();


            if (textBox1.Text == "")
            {
                MessageBox.Show("Сумма для конвертации не введена!", "Ошибка! ", MessageBoxButtons.OK, MessageBoxIcon.Error); return;
            }


            else if (comboBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Валюта, из которой будет проводиться конвертация не выбрана!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            else if (comboBox2.SelectedIndex == -1)
            {
                MessageBox.Show("Валюта, из которой будет проводиться конвертация не выбрана!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            else if (from == to)
            {
                MessageBox.Show("Выбрана одна и та же валюта!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            perevod();

            NewEra();
            ShowTable(STable, dataGridView1); //отображение в dataGridView1  новой строки
           

        }


        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox3.Clear();
            label6.Text = "";
            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
        }

        private void button2_Click_1(object sender, EventArgs e)
        {

            if (comboBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Валюта, из которой будет проводиться конвертация не выбрана!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            else if (comboBox2.SelectedIndex == -1)
            {
                MessageBox.Show("Валюта, из которой будет проводиться конвертация не выбрана!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            button1.Enabled = true;
            button3.Enabled = true;          

            string from, to;

            from = comboBox1.SelectedItem.ToString();
            to = comboBox2.SelectedItem.ToString();

            webBrowser1.Navigate("https://www.calc.ru/kurs-" + from + "-" + to + ".html");

        }

        private void button5_Click(object sender, EventArgs e)
        { 
            button2.Enabled = true;
            button4.Enabled = true;
            textBox1.Enabled = true;
            textBox3.Enabled = true;
            comboBox1.Enabled = true;
            comboBox2.Enabled = true;

            if (OpenDBFile() == true)
            {
                ZapolTable();   //заполнение dTable данными из таблицы БД
                ShowTable(STable, dataGridView1);      //отображение таблицы в dataGridView1
            }

            else
            {
                MessageBox.Show("Файл не выбран.", "Ошибка! ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }


        private void NewEra()
        {
           
            DataRow NewStroka = STable.NewRow();                       //создание новой строки 
            NewStroka[0] = Convert.ToInt32(STable.Rows.Count);         //присвоение номера эпохи для новой строки

            NewStroka[1] = Convert.ToString(comboBox1.SelectedItem); //новое значение метки
            NewStroka[2] = Convert.ToString(comboBox2.SelectedItem);
            NewStroka[0] = Convert.ToDouble(textBox1.Text);
            NewStroka[3] = Convert.ToString(label6.Text);
          
            STable.Rows.Add(NewStroka);   //добавление заполненной строки в dTable    
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (comboBox3.SelectedIndex == -1)
            {
                MessageBox.Show("Выбepите поле для расчета", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            else if (comboBox3.SelectedIndex == 0)
            {
                int s1 = 0;
                int s2 = 0;
                int s3 = 0;
                int s4 = 0;
                int s5 = 0;
                int s6 = 0;
                int s7 = 0;
                int s8 = 0;
                int s9 = 0;

                for (int row = 0; row < STable.Rows.Count; row++)
                {

                    if (Convert.ToString(STable.Rows[row][1]) == "RUB")
                    {
                        s1 = s1 + 1;
                    }

                    if (Convert.ToString(STable.Rows[row][1]) == "USD")
                    {
                        s2 = s2 + 1;
                    }
                    if (Convert.ToString(STable.Rows[row][1]) == "EUR")
                    {
                        s3 = s3 + 1;
                    }
                    if (Convert.ToString(STable.Rows[row][1]) == "UAH")
                    {
                        s4 = s4  + 1;
                    }
                    if (Convert.ToString(STable.Rows[row][1]) == "KZT")
                    {
                        s5 = s5 + 1;
                    }
                    if (Convert.ToString(STable.Rows[row][1]) == "BYN")
                    {
                        s6 = s6 + 1;
                    }
                    if (Convert.ToString(STable.Rows[row][1]) == "GBR")
                    {
                        s7 = s7 + 1;
                    }
                    if (Convert.ToString(STable.Rows[row][1]) == "JPY")
                    {
                        s8 = s8 + 1;
                    }
                    if (Convert.ToString(STable.Rows[row][1]) == "CNY")
                    {
                        s9 = s9 + 1;
                    }
                }
                int[] nums = new int[9];
                nums[0] = s1;
                nums[1] = s2;
                nums[2] = s3;
                nums[3] = s4;
                nums[4] = s5;
                nums[5] = s6;
                nums[6] = s7;
                nums[7] = s8;
                nums[8] = s9;
                int max = nums[0];        
                for (int i = 0; i < 9; i++)
                {
                    if (max < nums[i])   
                    {
                        max = nums[i]; 
                    }
                }
                for (int i = 0; i < 9; i++)
                {
                    if (max == s1 && max == s2 || max == s1 && max == s3 || max == s1 && max == s4 || max == s1 && max == s5 ||
                        max == s1 && max == s6 || max == s1 && max == s7 || max == s1 && max == s8 || max == s1 && max == s9 ||
                        max == s2 && max == s3 || max == s2 && max == s4 || max == s2 && max == s5 || max == s2 && max == s6 ||
                        max == s2 && max == s7 || max == s2 && max == s8 || max == s2 && max == s9 || max == s3 && max == s4 ||
                        max == s3 && max == s5 || max == s3 && max == s6 || max == s3 && max == s7 || max == s3 && max == s8 ||
                        max == s3 && max == s9 || max == s4 && max == s5 || max == s4 && max == s6 || max == s4 && max == s7 ||
                        max == s4 && max == s8 || max == s4 && max == s9 || max == s5 && max == s6 || max == s5 && max == s7 ||
                        max == s5 && max == s8 || max == s5 && max == s9 || max == s6 && max == s7 || max == s6 && max == s8 ||
                        max == s6 && max == s9 || max == s7 && max == s8 || max == s7 && max == s9 || max == s8 && max == s9)
                    {
                        string MyMessage = "";
                        MyMessage = "Преобладающей валюты нет!";
                        MessageBox.Show(MyMessage, "Статистика", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    else if (max == s1)
                    {
                        string MyMessage = "";
                        MyMessage = "Популярная валюта, из которой конвертируют это RUB";
                        MessageBox.Show(MyMessage, "Статистика", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    else if (max == s2)
                    {
                        string MyMessage = "";
                        MyMessage = "Популярная валюта, из которой конвертируют это USD";
                        MessageBox.Show(MyMessage, "Статистика", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    else if(max == s3)
                    {
                        string MyMessage = "";
                        MyMessage = "Популярная валюта, из которой конвертируют это EUR";
                        MessageBox.Show(MyMessage, "Статистика", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    else if(max == s4)
                    {
                        string MyMessage = "";
                        MyMessage = "Популярная валюта, из которой конвертируют это UAH";
                        MessageBox.Show(MyMessage, "Статистика", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    else if(max == s5)
                    {
                        string MyMessage = "";
                        MyMessage = "Популярная валюта, из которой конвертируют это KZT";
                        MessageBox.Show(MyMessage, "Статистика", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    else if(max == s6)
                    {
                        string MyMessage = "";
                        MyMessage = "Популярная валюта, из которой конвертируют это BYN";
                        MessageBox.Show(MyMessage, "Статистика", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    else if(max == s7)
                    {
                        string MyMessage = "";
                        MyMessage = "Популярная валюта, из которой конвертируют это GBP";
                        MessageBox.Show(MyMessage, "Статистика", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    else if(max == s8)
                    {
                        string MyMessage = "";
                        MyMessage = "Популярная валюта, из которой конвертируют это JPY";
                        MessageBox.Show(MyMessage, "Статистика", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    else if(max == s9)
                    {
                        string MyMessage = "";
                        MyMessage = "Популярная валюта, из которой конвертируют это CNY";
                        MessageBox.Show(MyMessage, "Статистика", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                   
                } 
            }
            else if (comboBox3.SelectedIndex == 1)
            {
                int s1 = 0;
                int s2 = 0;
                int s3 = 0;
                int s4 = 0;
                int s5 = 0;
                int s6 = 0;
                int s7 = 0;
                int s8 = 0;
                int s9 = 0;
                for (int row = 0; row < STable.Rows.Count; row++)
                {

                    if (Convert.ToString(STable.Rows[row][2]) == "RUB")
                    {
                        s1 = s1 + 1;
                    }

                   if (Convert.ToString(STable.Rows[row][2]) == "USD")
                    {
                        s2 = s2 + 1;
                    }
                   if (Convert.ToString(STable.Rows[row][2]) == "EUR")
                    {
                        s3 = s3 + 1;
                    }
                    if (Convert.ToString(STable.Rows[row][2]) == "UAH")
                    {
                        s4 = s4  + 1;
                    }
                    if (Convert.ToString(STable.Rows[row][2]) == "KZT")
                    {
                        s5 = s5 + 1;
                    }
                    if (Convert.ToString(STable.Rows[row][2]) == "BYN")
                    {
                        s6 = s6 + 1;
                    }
                    if (Convert.ToString(STable.Rows[row][2]) == "GBR")
                    {
                        s7 = s7 + 1;
                    }
                    if (Convert.ToString(STable.Rows[row][2]) == "JPY")
                    {
                        s8 = s8 + 1;
                    }
                    if (Convert.ToString(STable.Rows[row][2]) == "CNY")
                    {
                        s9 = s9 + 1;
                    }
                }

                int[] nums = new int[9];
                nums[0] = s1;
                nums[1] = s2;
                nums[2] = s3;
                nums[3] = s4;
                nums[4] = s5;
                nums[5] = s6;
                nums[6] = s7;
                nums[7] = s8;
                nums[8] = s9;

                int max = nums[0];        
                for (int i = 0; i < 9; i++)
                {
                    if (max < nums[i])   
                    {
                        max = nums[i]; 
                    }
                }

                for (int i = 0; i < 9; i++)
                {
                    if (max == s1 && max == s2 || max == s1 && max == s3 || max == s1 && max == s4 || max == s1 && max == s5 ||
                        max == s1 && max == s6 || max == s1 && max == s7 || max == s1 && max == s8 || max == s1 && max == s9 ||
                        max == s2 && max == s3 || max == s2 && max == s4 || max == s2 && max == s5 || max == s2 && max == s6 ||
                        max == s2 && max == s7 || max == s2 && max == s8 || max == s2 && max == s9 || max == s3 && max == s4 ||
                        max == s3 && max == s5 || max == s3 && max == s6 || max == s3 && max == s7 || max == s3 && max == s8 ||
                        max == s3 && max == s9 || max == s4 && max == s5 || max == s4 && max == s6 || max == s4 && max == s7 ||
                        max == s4 && max == s8 || max == s4 && max == s9 || max == s5 && max == s6 || max == s5 && max == s7 ||
                        max == s5 && max == s8 || max == s5 && max == s9 || max == s6 && max == s7 || max == s6 && max == s8 ||
                        max == s6 && max == s9 || max == s7 && max == s8 || max == s7 && max == s9 || max == s8 && max == s9)
                    {
                        string MyMessage = "";
                        MyMessage = "Преобладающей валюты нет!";
                        MessageBox.Show(MyMessage, "Статистика", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                     if (max == s1)
                    {
                        string MyMessage = "";
                        MyMessage = "Популярная валюта, в которую конвертируют это RUB";
                        MessageBox.Show(MyMessage, "Статистика", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                     if (max == s2)
                    {
                        string MyMessage = "";
                        MyMessage = "Популярная валюта, в которую конвертируют это USD";
                        MessageBox.Show(MyMessage, "Статистика", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    if(max == s3)
                    {
                        string MyMessage = "";
                        MyMessage = "Популярная валюта, в которую конвертируют это EUR";
                        MessageBox.Show(MyMessage, "Статистика", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                   if(max == s4)
                    {
                        string MyMessage = "";
                        MyMessage = "Популярная валюта, в которую конвертируют это UAH";
                        MessageBox.Show(MyMessage, "Статистика", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    if(max == s5)
                    {
                        string MyMessage = "";
                        MyMessage = "Популярная валюта, в которую конвертируют это KZT";
                        MessageBox.Show(MyMessage, "Статистика", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                   if(max == s6)
                    {
                        string MyMessage = "";
                        MyMessage = "Популярная валюта, в которую конвертируют это BYN";
                        MessageBox.Show(MyMessage, "Статистика", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                     if(max == s7)
                    {
                        string MyMessage = "";
                        MyMessage = "Популярная валюта, в которую конвертируют это GBP";
                        MessageBox.Show(MyMessage, "Статистика", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    if(max == s8)
                    {
                        string MyMessage = "";
                        MyMessage = "Популярная валюта, в которую конвертируют это JPY";
                        MessageBox.Show(MyMessage, "Статистика", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    if(max == s9)
                    {
                        string MyMessage = "";
                        MyMessage = "Популярная валюта, в которую конвертируют это CNY";
                        MessageBox.Show(MyMessage, "Статистика", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                   
                }  
            }

        }

        private void button7_Click(object sender, EventArgs e)
        {
            double max = 0;
            for (int row = 0; row < STable.Rows.Count; row++)
            {

                if (Convert.ToDouble(STable.Rows[row][0]) > max)
                {
                    max = Convert.ToDouble(STable.Rows[row][0]);
                }
            }
            string MyMessage = "";
            MyMessage = "Наибольшая сумма всех запроса = " + max;
            MessageBox.Show(MyMessage, "Статистика", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return; 
        }

        private void button8_Click(object sender, EventArgs e)
        {
            double min;
            min = Convert.ToDouble(STable.Rows[0][0]);
            for (int row = 0; row < STable.Rows.Count; row++)
            {
                if (Convert.ToDouble(STable.Rows[row][0]) < min)
                {
                    min = Convert.ToDouble(STable.Rows[row][0]);
                }
            }
            string MyMessage = "";
            MyMessage = "Наименьшая сумма всех запроса = " + min;
            MessageBox.Show(MyMessage, "Статистика", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string MyMessage = "";
            MyMessage = "RUB - Российский рубль" + "\n" +
                "USD - Доллар США" + "\n" +
                "EUR - ЕВРО" + "\n" +
                "UAH - Украинская гривна" + "\n" +
                "KZT - Казахстанский тенге" + "\n" +
                "BYN - Белорусский рубль" + "\n" +
                "GBP - Фунт стерлингов" + "\n" +
                "JPY - Японская иена" + "\n" +
                "CNY - Китайский юань";
            MessageBox.Show(MyMessage, "Расшировка валют", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
    }
}

