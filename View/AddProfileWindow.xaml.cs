using System;
using System.Collections.Generic;
using System.Globalization;
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
using wpfHUSH.Model;
using wpfHUSH.ViewModel;

namespace wpfHUSH.View
{
    /// <summary>
    /// Логика взаимодействия для AddProfileWindow.xaml
    /// </summary>
    public partial class AddProfileWindow : Window
    {
        public AddProfileWindow()
        {
            InitializeComponent();
            DataContext = new AddProfileWindowViewModel(this);  
        }
        private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }
        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

    }
}
