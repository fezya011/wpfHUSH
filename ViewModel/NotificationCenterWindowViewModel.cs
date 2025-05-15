using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using wpfHUSH.Model;
using wpfHUSH.View;
using wpfHUSH.VMTools;

namespace wpfHUSH.ViewModel
{
    class NotificationCenterWindowViewModel : BaseVM
    {
        private List<Swipes> notifications;
        public List<Swipes> Notifications
        {
            get => notifications;
            set
            {
                notifications = value;
                Signal();
            }
        }

        public ICommand OpenLikedWindow { get; }

        public NotificationCenterWindowViewModel() 
        {
            OpenLikedWindow = new CommandVM(() =>
            {
                LikedWindow likedWindow = new LikedWindow();
                likedWindow.ShowDialog();
            }, () => true);
        }

        private void MarkNotificationsAsRead(IEnumerable<Swipes> swipes)
        {
            var currentUserId = UserStatic.CurrentUser.Id;
            var allSwipes = SwipeDB.GetDb().SelectAll();

            foreach (var swipe in allSwipes.Where(s =>
                s.SwipedId == currentUserId &&
                !s.IsNotificated))
            {
                swipe.IsNotificated = true;
                SwipeDB.GetDb().Update(swipe);
            }
        }
        private void LoadNotifications()
        {
            try
            {
                var currentUserId = UserStatic.CurrentUser.Id;
                var allSwipes = SwipeDB.GetDb().SelectAll();
                var receivedLikes = allSwipes
                    .Where(s => s.SwipedId == currentUserId && s.Action)
                    .ToList();


                MarkNotificationsAsRead(receivedLikes.Where(s => !s.IsNotificated));

                Notifications = receivedLikes
                    .OrderByDescending(s => s.IsNotificated)
                    .ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки уведомлений: {ex.Message}");
            }
        }
    }
}
