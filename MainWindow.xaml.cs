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

        public MainWindow()
        {
            InitializeComponent();
        }

        private int chosedIntervals = 0;
        private int chosedSeries = 0;

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

        private void PreviewTextInputPeriod(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        //--------------Select File Button Handler--------------

        private void button_Click_Select_File(object sender, RoutedEventArgs e)
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
                Button_Click_Series(sender, e);
            }
        }

        //--------------Build Series Button Handler--------------

        private void Button_Click_Series(object sender, RoutedEventArgs e)
        {
            try
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

                amountLabel.Content = "Всего елементов: " + (amountOfNumbers.Length);

                double[,] numbers = new double[amountOfNumbers.Distinct().ToArray().Length, 2];
                Array.Sort(amountOfNumbers);
                double minValue = amountOfNumbers.Min();
                double maxValue = amountOfNumbers.Max();
                int index = 0;
                int arrayLength = amountOfNumbers.Distinct().ToArray().Length;

                for (int i = 0; i < amountOfNumbers.Length; i++)
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
                    if (!numberFinded)
                    {
                        numbers[index, 0] = amountOfNumbers[i];
                        index++;
                    }
                }

                DataGridTextColumn[] textColumn = new DataGridTextColumn[arrayLength + 1];
                RowSeries row = new RowSeries();
                row.numbers = new double[arrayLength];

                //Variational Series

                DataGridSeries.Columns.Clear();
                DataGridSeries.Items.Clear();

                DataGridTextColumn textColumnTitle = new DataGridTextColumn();
                textColumnTitle.Header = "Xi";
                textColumnTitle.Binding = new Binding("name");
                DataGridSeries.Columns.Add(textColumnTitle);


                if (chosedSeries == 0) //frequency
                {
                    for (int i = 0; i < arrayLength; i++)
                    {
                        textColumn[i + 1] = new DataGridTextColumn();
                        textColumn[i + 1].Header = Math.Round(numbers[i, 0], 2);
                        textColumn[i + 1].Binding = new Binding($"numbers[{i}]");
                        DataGridSeries.Columns.Add(textColumn[i + 1]);

                        row.numbers[i] = Convert.ToInt32(numbers[i, 1]) + 1;
                    }

                    row.name = "Ni(Pi)";
                }
                else if (chosedSeries == 1) //relative frenquencies
                {
                    for (int i = 0; i < arrayLength; i++)
                    {
                        textColumn[i + 1] = new DataGridTextColumn();
                        textColumn[i + 1].Header = Math.Round(numbers[i, 0], 2);
                        textColumn[i + 1].Binding = new Binding($"numbers[{i}]");
                        DataGridSeries.Columns.Add(textColumn[i + 1]);

                        row.numbers[i] = Math.Round((Convert.ToInt32(numbers[i, 1]) + 1) / (double)amountOfNumbers.Length, 2);
                    }

                    row.name = "W";
                }

                DataGridSeries.Items.Add(row);
                amountLabel.Content = "Всего элементов: " + amountOfNumbers.Length;

                //Intervals

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

                if (chosedIntervals == 0) //period
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
                else if (chosedIntervals == 1) //devided
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
            catch(Exception exeption)
            {
                ExeptionWindow objExeptionWindow = new ExeptionWindow();
                objExeptionWindow.ExeptionString(exeption.ToString());
                objExeptionWindow.Show();
            }
        }

        //Class for serieses columns
        public class RowSeries
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
    }
}
