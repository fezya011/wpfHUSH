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
using wpfHUSH.ViewModel;

namespace wpfHUSH.View
{
    /// <summary>
    /// Логика взаимодействия для MutualSympathyWindow.xaml
    /// </summary>
    public partial class LikedWindow : Window
    {
        public LikedWindow()
        {
            InitializeComponent();
            DataContext = new LikedWindowViewModel();
        }
    }
}
