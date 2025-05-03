using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using wpfHUSH.View;
using wpfHUSH.VMTools;

namespace wpfHUSH.ViewModel
{
    class NotificationCenterWindowViewModel : BaseVM
    {
        public ICommand OpenLikedWindow { get; }

        public NotificationCenterWindowViewModel() 
        {
            OpenLikedWindow = new CommandVM(() =>
            {
                LikedWindow likedWindow = new LikedWindow();
                likedWindow.ShowDialog();
            }, () => true);
        }
    }
}
