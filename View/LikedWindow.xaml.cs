using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
    /// Логика взаимодействия для MutualSympathyWindow.xaml
    /// </summary>
    public partial class LikedWindow : Window
    {
        public LikedWindow(Swipes swiper)
        {
            InitializeComponent();
            DataContext = new LikedWindowViewModel(this, swiper);
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
