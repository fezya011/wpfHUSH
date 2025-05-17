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
using MaterialDesignThemes.Wpf;

namespace wpfHUSH.ViewModel
{
    class SearchWindowViewModel : BaseVM
    {
        private Visibility reportWindowVisible = Visibility.Collapsed;
        private List<User> profileToShow = new List<User>();
        private Random random = new Random();
        private User currentProfile;
        private List<User> allProfiles;
        private Visibility areOver = Visibility.Hidden;
        private Visibility hiddenLocationAndPhoto;
        private User user;
        private List<Reason> reasons;
        private Reason selectedReason;
        private string reportText;

        public Visibility ReportWindowVisible
        {
            get => reportWindowVisible; 
            set
            {
                reportWindowVisible = value;
                Signal();
            }
        }

        public User CurrentProfile 
        { 
            get => currentProfile; 
            set
            {
                currentProfile = value;
                Signal();
            }
        }

        public List<User> AllProfiles 
        { 
            get => allProfiles;
            set
            {
                allProfiles = value;
                Signal();
            }
        }

        public Visibility AreOver 
        { 
            get => areOver; 
            set
            {
                areOver = value;
                Signal();
            }
        }

        public Visibility HiddenLocationAndPhoto 
        { 
            get => hiddenLocationAndPhoto; 
            set
            {
                hiddenLocationAndPhoto = value;
                Signal();
            }
        }

        public User User 
        { 
            get => user; 
            set
            {
                user = value;
                Signal();
            }
        }

        public List<Reason> Reasons 
        { 
            get => reasons; 
            set
            {
                reasons = value;
                Signal();
            }
        }

        public Reason SelectedReason 
        { 
            get => selectedReason; 
            set
            {
                selectedReason = value;
                Signal();
            }
        }

        public string ReportText 
        { 
            get => reportText; 
            set
            {
                reportText = value;
                Signal();
            }
        }

        public ICommand OpenEditWindow { get; set; }
        public ICommand OpenNotificationWindow { get; set; }
        public ICommand ReportVisible { get; }
        public ICommand ReportHidden { get; }
        public ICommand LikeCommand { get; }
        public ICommand DislikeCommand { get; }
        public ICommand SendReport { get; }


        public SearchWindowViewModel(SearchWindow searchWindow)
        {           
            User = UserStatic.CurrentUser;

            SearchSelectAll();
            ReasonsSelectAll();
            
            profileToShow = AllProfiles
                .OrderBy(x => random.Next())
                .ToList();
           
            if (profileToShow.Count > 0)
            {
                CurrentProfile = profileToShow.First();
            }

            SendReport = new CommandVM(() =>
            {
                if (SelectedReason == null)
                {
                    MessageBox.Show("Сначала выбери причину жалобы");
                }
                else
                {
                    Report report = new Report();
                    report.ReportedId = CurrentProfile.Id;
                    report.ReporterId = UserStatic.CurrentUser.Id;
                    report.Text = ReportText;
                    report.Reason = new Reason();
                    report.Reason.Type = SelectedReason.Type;
                    report.ReasonId = SelectedReason.Id;
                    ReportDB.GetDb().InsertReport(report);
                    ReportWindowVisible = Visibility.Hidden;
                }                              
            }, () => !string.IsNullOrWhiteSpace(ReportText));

            LikeCommand = new CommandVM(() =>
            {
                ProcessLikeDislike(true);
            }, () => true);

            DislikeCommand = new CommandVM(() =>
            {
                ProcessLikeDislike(false);
            }, () => true);
            OpenEditWindow = new CommandVM(() =>
            {
                EditProfileWindow editProfileWindow = new EditProfileWindow();
                editProfileWindow.ShowDialog();
            }, () => true);

            OpenEditWindow = new CommandVM(() =>
            {
                EditProfileWindow editProfileWindow = new EditProfileWindow();
                searchWindow.Hide();
                editProfileWindow.ShowDialog();
                searchWindow.Visibility = Visibility.Visible;
                User = null;
                User = UserStatic.CurrentUser;
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

        private void SearchSelectAll()
        {
            AllProfiles = new List<User>(UserDB.GetDb().SearchSelectAll().Where(user => user.Id != UserStatic.CurrentUser.Id && !SwipeDB.GetDb().SelectAll().Any(swipe => swipe.SwiperId == UserStatic.CurrentUser.Id && swipe.SwipedId == user.Id)).ToList());
            if (AllProfiles.Count == 0)
            {
                HideAndVisability();
            }
        }

        private void ReasonsSelectAll()
        {
            Reasons = ReportDB.GetDb().SelectAll();        
        }

        private async void ShowNextProfile(bool isLike)
        {
            if (CurrentProfile == null) return;

            Application.Current.Dispatcher.Invoke(() =>
            {
                var window = Application.Current.Windows.OfType<SearchWindow>().FirstOrDefault();
                window?.StartProfileAnimation(isLike ? "SlideOutToRight" : "SlideOutToLeft");
            });

            await Task.Delay(200); 

            profileToShow.Remove(CurrentProfile);

            if (profileToShow.Count > 0)
            {
                CurrentProfile = profileToShow.First();

                Application.Current.Dispatcher.Invoke(() =>
                {
                    var window = Application.Current.Windows.OfType<SearchWindow>().FirstOrDefault();
                    window?.StartProfileAnimation(isLike ? "SlideInFromLeft" : "SlideInFromRight");
                });
            }
            else
            {
                HideAndVisability();
                CurrentProfile = null;
            }
        }

        private void ProcessLikeDislike(bool isLike)
        {
            if (CurrentProfile == null) return;

            var swipe = new Swipes
            {
                SwiperId = UserStatic.CurrentUser.Id,
                SwipedId = CurrentProfile.Id,
                Action = isLike,
                IsNotificated = false
            };
           
            SwipeDB.GetDb().InsertSwipe(swipe);
                          
            ShowNextProfile(isLike);
        }

        private void HideAndVisability()
        {
            AreOver = Visibility.Visible;
            HiddenLocationAndPhoto = Visibility.Hidden;            
        }
    }
}
