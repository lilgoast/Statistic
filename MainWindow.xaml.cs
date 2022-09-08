using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;

namespace Statistic
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            int arrayLength = 0;
            string line;
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            dlg.DefaultExt = ".txt";
            dlg.Filter = "TXT Documents|*.txt";

            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                string filename = dlg.FileName;

                double[] numbers = new double[0];
                
                StreamReader reader = File.OpenText(filename);

                while ((line = reader.ReadLine()) != null)
                {
                    string[] items = line.Split('\t', ' ');

                    foreach(string item in items)
                    {
                        if(item.Length > 0)
                        {
                            Array.Resize(ref numbers, numbers.Length + 1);

                            if(!double.TryParse(item, NumberStyles.Any, CultureInfo.InvariantCulture, out numbers[arrayLength]))
                                numbers[arrayLength] = 0;
                            arrayLength++;
                        }
                    }
                }

                string nums = "";

                foreach(double item in numbers)
                {
                    nums = nums + " " + item.ToString();
                }
                numsTextBox.Text = nums;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            double[] amountOfNumbers = new double[1];
            string line;
            string nums = numsTextBox.Text.ToString();
            StringReader strReader = new StringReader(nums);

            while ((line = strReader.ReadLine()) != null)
            {
                string[] items = line.Split(' ');

                foreach (string item in items)
                {
                    if(item.Length > 0)
                    {
                        Array.Resize(ref amountOfNumbers, amountOfNumbers.Length + 1);
                        amountOfNumbers[amountOfNumbers.Length - 1] = Convert.ToDouble(item);
                    }
                }
            }

            double[,] numbers = new double[amountOfNumbers.Distinct().ToArray().Length - 1, 2];
            Array.Sort(amountOfNumbers);
            int index = 0;
            int arrayLength = amountOfNumbers.Distinct().ToArray().Length - 1;

            for (int i = 1; i < amountOfNumbers.Length; i++)
            {
                bool numberFinded = false;
                for (int d = 0; d < arrayLength; d++)
                {
                    if (numbers[d, 0] == amountOfNumbers[i])
                    {
                        numbers[d, 1] += 1;
                        numberFinded = true;
                        break;
                    }
                }
                if(!numberFinded)
                {
                    numbers[index, 0] = amountOfNumbers[i];
                    index++;
                }
            }

            DataGridTextColumn[] textColumn = new DataGridTextColumn[arrayLength + 1];
            Row row = new Row();
            row.numbers = new int[arrayLength];

            DataGrid1.Columns.Clear();
            DataGrid1.Items.Clear();

            DataGridTextColumn tc = new DataGridTextColumn();
            tc.Header = "Xi";
            tc.Binding = new Binding("name");
            DataGrid1.Columns.Add(tc);

            for (int i = 1; i < arrayLength + 1; i++)
            {
                textColumn[i] = new DataGridTextColumn();
                textColumn[i].Header = numbers[i - 1, 0];
                textColumn[i].Binding = new Binding($"numbers[{i - 1}]");
                DataGrid1.Columns.Add(textColumn[i]);

                row.numbers[i - 1] = Convert.ToInt32(numbers[i - 1, 1]) + 1;
            }

            row.name = "Ni(Pi)";

            DataGrid1.Items.Add(row);
        }

        public class Row
        {
            public int[] numbers { get; set; }
            public string name { get; set; }
        }
    }
}
