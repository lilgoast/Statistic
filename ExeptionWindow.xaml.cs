using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace Statistic
{
    /// <summary>
    /// Логика взаимодействия для ExeptionWindow.xaml
    /// </summary>
    public partial class ExeptionWindow : Window
    {
        public ExeptionWindow()
        {
            InitializeComponent();
        }

        private string exeption;

        public void ExeptionString(string exeptionString)
        {
            exeption = exeptionString;
            label.Content = exeption;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
