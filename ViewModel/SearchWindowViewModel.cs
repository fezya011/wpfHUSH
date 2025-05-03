using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows;
using wpfHUSH.VMTools;
using System.Windows.Controls;
using System.Windows.Input;
using wpfHUSH.View;
using System.Collections.ObjectModel;
using wpfHUSH.Model;
using System.Windows.Media;

namespace wpfHUSH.ViewModel
{
    class SearchWindowViewModel : BaseVM
    {
        

        

        private Visibility reportWindowVisible = Visibility.Collapsed;


       
        public Visibility ReportWindowVisible
        {
            get => reportWindowVisible; 
            set
            {
                reportWindowVisible = value;
                Signal();
            }
        }
        private bool _isLiked;
        public bool IsLiked
        {
            get => _isLiked;
            set { _isLiked = value; Signal(); }
        }

        private bool _isDisliked;
        public bool IsDisliked
        {
            get => _isDisliked;
            set { _isDisliked = value; Signal(); }
        }
        public ICommand OpenEditWindow { get; set; }
        public ICommand OpenNotificationWindow { get; set; }
        public ICommand ReportVisible { get; }
        public ICommand ReportHidden { get; }

        public ICommand LikeCommand { get; }
        public ICommand DislikeCommand { get; }
        public SearchWindowViewModel(SearchWindow searchWindow)
        {
           

            OpenEditWindow = new CommandVM(() =>
            {
                EditProfileWindow editProfileWindow = new EditProfileWindow();
                editProfileWindow.ShowDialog();
            }, () => true);

            OpenNotificationWindow = new CommandVM(() =>
            {
                NotificationCenterWindow notificationCenterWindow = new NotificationCenterWindow();
                notificationCenterWindow.ShowDialog();
            }, () => true);

            ReportVisible = new CommandVM(() =>
            {
                ReportWindowVisible = Visibility.Visible;
            }, () => true);
            
            ReportHidden = new CommandVM(() =>
            {
                ReportWindowVisible = Visibility.Hidden;
            }, () => true);
        }
    }
}
