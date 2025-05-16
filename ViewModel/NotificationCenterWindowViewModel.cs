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
        private User swiper;
        private Swipes selectedItem;

        public List<Swipes> Notifications
        {
            get => notifications;
            set
            {
                notifications = value;
                Signal();
            }
        }

        public User Swiper 
        { 
            get => swiper; 
            set
            {
                swiper = value;
                Signal();
            }
        }

        public Swipes SelectedItem 
        { 
            get => selectedItem; 
            set
            {
                selectedItem = value;
                Signal();
            }
        }

        public ICommand MarkAsReadCommand { get; }

        public NotificationCenterWindowViewModel()
        {
            LoadNotifications();
           
            MarkAsReadCommand = new CommandVM(MarkAllAsRead, () => true);
        }

       

        private void MarkAllAsRead()
        {
            var unreadNotifications = Notifications?.Where(n => !n.IsNotificated).ToList();
            if (unreadNotifications == null || !unreadNotifications.Any())
                return;

            foreach (var notification in unreadNotifications)
            {
                notification.IsNotificated = true;
                SwipeDB.GetDb().Update(notification);
            }

            // Обновляем список уведомлений
            LoadNotifications();
        }

        private void MarkNotificationsAsRead(IEnumerable<Swipes> swipes)
        {
            foreach (var swipe in swipes.Where(s => !s.IsNotificated))
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

                // Получаем только лайки, направленные текущему пользователю
                var receivedLikes = allSwipes
                    .Where(s => s.SwipedId == currentUserId && s.Action)
                    .ToList();

                // Обновляем информацию о пользователях, которые лайкнули
                foreach (var swipe in receivedLikes)
                {
                    if (swipe.Swiper == null)
                    {
                        swipe.Swiper = UserDB.GetDb().SelectById(swipe.SwiperId);
                        Swiper = swipe.Swiper;
                    }
                }

                Notifications = receivedLikes
                    .OrderByDescending(s => s.IsNotificated) // Сначала непрочитанные
                    .ThenByDescending(s => s.Swiper?.Name)   // Затем сортируем по имени
                    .ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки уведомлений: {ex.Message}");
            }
        }

        internal void DoubleClickList()
        {
            if (SelectedItem != null)
            {
                LikedWindow likedWindow = new LikedWindow(SelectedItem);
                likedWindow.ShowDialog();
            }
        }
    }
}
