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

namespace wpfHUSH.ViewModel
{
    class SearchWindowViewModel : BaseVM
    {
        public ICommand Open { get; set; }
        public ICommand OpenEditWindow { get; set; }
        public ICommand OpenNotificationWindow { get; set; }

        public SearchWindowViewModel()
        {
            Open = new CommandVM(() =>
            {
                LikedWindow vm = new LikedWindow();
                vm.ShowDialog();
            }, () => true);

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
        }
    }
}
