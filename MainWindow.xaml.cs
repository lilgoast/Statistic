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
using System.Text.RegularExpressions;

namespace Statistic
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    
    public partial class MainWindow : Window
    {
        int chosed = 0;

        public MainWindow()
        {
            InitializeComponent();
        }

<<<<<<< Updated upstream
=======
        //--------------ComboBox Handler--------------

        //Intervals
        private void comboBoxIntervals_Loaded(object sender, RoutedEventArgs e)
        {
            List<string> data = new List<string>();
            data.Add("Период"); //indx = 0
            data.Add("Разделить на"); //indx = 1

            var combo = sender as ComboBox;
            combo.ItemsSource = data;
            combo.SelectedIndex = 0;
        }

        private void comboBoxIntervals_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedComboItem = sender as ComboBox;
            chosedIntervals = selectedComboItem.SelectedIndex;
        }

        //Series
        private void comboBoxSeries_Loaded(object sender, RoutedEventArgs e)
        {
            List<string> data = new List<string>();
            data.Add("Ряд частот");  //indx = 0
            data.Add("Ряд относительных частот"); //indx = 1

            var combo = sender as ComboBox;
            combo.ItemsSource = data;
            combo.SelectedIndex = 0;
        }

        private void comboBoxSeries_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedComboItem = sender as ComboBox;
            chosedSeries = selectedComboItem.SelectedIndex;
        }

        //--------------Text In "periodTextBox" Handler--------------

>>>>>>> Stashed changes
        private void PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
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
            Button_Click_Series(sender, e);
        }

        //--------------Build Series Button Handler--------------

        private void Button_Click_Series(object sender, RoutedEventArgs e)
        {
            double[] amountOfNumbers = new double[0];
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

            amountLabel.Content = "Всего елементов: " + (amountOfNumbers.Length - 1);

            double[,] numbers = new double[amountOfNumbers.Distinct().ToArray().Length - 1, 2];
            Array.Sort(amountOfNumbers);
            double minValue = amountOfNumbers.Min();
            double maxValue = amountOfNumbers.Max();
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

<<<<<<< Updated upstream
            //Grid1
            DataGridTextColumn[] textColumn1 = new DataGridTextColumn[arrayLength + 1];
            Row1 row1 = new Row1();
            row1.numbers = new int[arrayLength];
            double amountOfAllNums = 0;
=======
            //variational series

            DataGridSeries.Columns.Clear();
            DataGridSeries.Items.Clear();
>>>>>>> Stashed changes

            DataGrid1.Columns.Clear();
            DataGrid1.Items.Clear();

            DataGridTextColumn tc1 = new DataGridTextColumn();
            tc1.Header = "Xi";
            tc1.Binding = new Binding("name");
            DataGrid1.Columns.Add(tc1);

            for (int i = 1; i < arrayLength + 1; i++)
            {
                textColumn1[i] = new DataGridTextColumn();
                textColumn1[i].Header = Math.Round(numbers[i - 1, 0], 2);
                textColumn1[i].Binding = new Binding($"numbers[{i - 1}]");
                DataGrid1.Columns.Add(textColumn1[i]);

                row1.numbers[i - 1] = Convert.ToInt32(numbers[i - 1, 1]) + 1;
                amountOfAllNums += Convert.ToInt32(numbers[i - 1, 1]) + 1;
            }

            row1.name = "Ni(Pi)";

            DataGrid1.Items.Add(row1);

            //Grid2
            DataGridTextColumn[] textColumn2 = new DataGridTextColumn[arrayLength + 1];
            Row2 row2 = new Row2();
            row2.numbers = new double[arrayLength];

            DataGrid2.Columns.Clear();
            DataGrid2.Items.Clear();

            DataGridTextColumn tc2 = new DataGridTextColumn();
            tc2.Header = "Xi";
            tc2.Binding = new Binding("name");
            DataGrid2.Columns.Add(tc2);



            for (int i = 1; i < arrayLength + 1; i++)
            {
                textColumn2[i] = new DataGridTextColumn();
                textColumn2[i].Header = Math.Round(numbers[i - 1, 0], 2);
                textColumn2[i].Binding = new Binding($"numbers[{i - 1}]");
                DataGrid2.Columns.Add(textColumn2[i]);

                row2.numbers[i - 1] = Math.Round((Convert.ToInt32(numbers[i - 1, 1]) + 1) / amountOfAllNums, 2);
            }

<<<<<<< Updated upstream
            row2.name = "W";

            DataGrid2.Items.Add(row2);
        }

        public class Row1
        {
            public int[] numbers { get; set; }
            public string name { get; set; }
        }
        public class Row2
        {
            public double[] numbers { get; set; }
            public string name { get; set; }
        }

        public class RowIntervals
        {
            public double[] numbers { get; set; }
            public string name { get; set; }
        }

        private void comboBox_Loaded(object sender, RoutedEventArgs e)
        {
            List<string> data = new List<string>();
            data.Add("Период");
            data.Add("Разделить на");

            var combo = sender as ComboBox;
            combo.ItemsSource = data;
            combo.SelectedIndex = 0;
        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedComboItem = sender as ComboBox;
            chosed = selectedComboItem.SelectedIndex;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            double[] amountOfNumbers = new double[0];
            string line;
            string nums = numsTextBox.Text.ToString();
            StringReader strReader = new StringReader(nums);

            while ((line = strReader.ReadLine()) != null)
            {
                string[] items = line.Split(' ');

                foreach (string item in items)
                {
                    if (item.Length > 0)
                    {
                        Array.Resize(ref amountOfNumbers, amountOfNumbers.Length + 1);
                        amountOfNumbers[amountOfNumbers.Length - 1] = Convert.ToDouble(item);
                    }
                }
            }

            double[,] numbers = new double[amountOfNumbers.Distinct().ToArray().Length, 2];
            Array.Sort(amountOfNumbers);
            double minValue = amountOfNumbers.Min();
            double maxValue = amountOfNumbers.Max();
            int index = 0;
            int arrayLength = amountOfNumbers.Distinct().ToArray().Length - 1;
=======
            DataGridSeries.Items.Add(row);
>>>>>>> Stashed changes

            //intervals

            DataGridTextColumn[] textColumnIntervals = new DataGridTextColumn[arrayLength + 1];
            RowIntervals rowIntervals = new RowIntervals();
            rowIntervals.numbers = new double[arrayLength];
            double amountOfAllNums = 0;

            DataGridIntervals.Columns.Clear();
            DataGridIntervals.Items.Clear();

            DataGridTextColumn tcIntervals = new DataGridTextColumn();
            tcIntervals.Header = "Xi";
            tcIntervals.Binding = new Binding("name");
            DataGridIntervals.Columns.Add(tcIntervals);

            for (int i = 0; i < arrayLength; i++)
            {
                amountOfAllNums += Convert.ToInt32(numbers[i, 1]) + 1;
            }

<<<<<<< Updated upstream
            if (chosed == 0) // Период
=======
            if (chosedIntervals == 0) //period
>>>>>>> Stashed changes
            {
                periodLabel.Content = "";
                int period = Convert.ToInt32(periodTextBox.Text);
                int i = 0;
                double iterationValue = minValue;
                double absoluteInterval;
                int indexPeriod = 0;

                while (iterationValue <= maxValue)
                {
                    int periodValue = 0;

                    absoluteInterval = ((double)iterationValue * 2 + period - 1) / 2;
                    textColumnIntervals[i + 1] = new DataGridTextColumn();
                    textColumnIntervals[i + 1].Header = $"[{Math.Round(iterationValue, 2)}, {Math.Round(iterationValue + period, 2)}) ({Math.Round(absoluteInterval, 2)})";
                    textColumnIntervals[i + 1].Binding = new Binding($"numbers[{i}]");
                    DataGridIntervals.Columns.Add(textColumnIntervals[i + 1]);

                    while (indexPeriod < arrayLength
                        && numbers[indexPeriod, 0] >= iterationValue
                        && numbers[indexPeriod, 0] < (double)iterationValue + period)
                    {
                        periodValue += Convert.ToInt32(numbers[indexPeriod, 1]) + 1;
                        indexPeriod++;
                    }

                    rowIntervals.numbers[i] = periodValue;
                    iterationValue += period;
                    i++;
                }
            }
<<<<<<< Updated upstream
            else if(chosed == 1) // Разделить на
=======
            else if (chosedIntervals == 1) //devided
>>>>>>> Stashed changes
            {
                double period = (maxValue - minValue) / Convert.ToDouble(periodTextBox.Text);
                periodLabel.Content = "Период: " + Math.Round(period, 2);
                int i = 0;
                double iterationValue = minValue;
                double absoluteInterval;
                int indexPeriod = 0;
                int iterationAmount = Convert.ToInt32(periodTextBox.Text);

                while (iterationAmount > 0)
                {
                    int periodValue = 0;

                    absoluteInterval = ((double)iterationValue * 2 + period) / 2;
                    textColumnIntervals[i + 1] = new DataGridTextColumn();
                    if (iterationValue == minValue || iterationValue >= maxValue - period)
                    {
                        textColumnIntervals[i + 1].Header = $"[{Math.Round(iterationValue, 2)}, {Math.Round(iterationValue + period, 2)}] ({Math.Round(absoluteInterval, 2)})";
                    }
                    else
                    {
                        textColumnIntervals[i + 1].Header = $"({Math.Round(iterationValue, 2)}, {Math.Round(iterationValue + period, 2)}] ({Math.Round(absoluteInterval, 2)})";
                    }
                    textColumnIntervals[i + 1].Binding = new Binding($"numbers[{i}]");
                    DataGridIntervals.Columns.Add(textColumnIntervals[i + 1]);

                    while (indexPeriod < arrayLength
                        && numbers[indexPeriod, 0] >= iterationValue
                        && numbers[indexPeriod, 0] <= Math.Round((double)iterationValue + period, 5))
                    {
                        periodValue += Convert.ToInt32(numbers[indexPeriod, 1]) + 1;
                        indexPeriod++;
                    }

                    rowIntervals.numbers[i] = periodValue;
                    iterationValue += period;
                    i++;
                    iterationAmount--;
                }
            }
            rowIntervals.name = "Ni(Pi)";

            DataGridIntervals.Items.Add(rowIntervals);
        }

<<<<<<< Updated upstream
=======
        //Class for series columns
        public class Row
        {
            public double[] numbers { get; set; }
            public string name { get; set; }
        }      
        
        //Interval's class for columns
        public class RowIntervals
        {
            public double[] numbers { get; set; }
            public string name { get; set; }
        }
>>>>>>> Stashed changes
    }
}
