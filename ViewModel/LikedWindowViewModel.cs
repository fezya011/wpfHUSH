using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using wpfHUSH.Model;
using wpfHUSH.View;
using wpfHUSH.VMTools;
using static MaterialDesignThemes.Wpf.Theme.ToolBar;

namespace wpfHUSH.ViewModel
{
    public class LikedWindowViewModel : BaseVM
    {
        
        

        private Visibility linksButtonsVisible = Visibility.Collapsed;
        private Visibility userButtonsVisible;
        private Visibility reportWindowVisible = Visibility.Collapsed;
        private Swipes swiper;
        private User users;
        private User user;
        private List<Reason> reasons;
        private Reason selectedReason;
        private string reportText;


        public ICommand LinkVisible { get; }
        public ICommand ReportVisible { get; }
        public ICommand ReturnUsersButtons { get; }
        public ICommand CloseWindow { get; }
        public ICommand OpenEditWindow { get; }
        public ICommand SendReport { get; }
        public ICommand Dislike { get; }

        

        public Visibility ReportWindowVisible 
        {
            get => reportWindowVisible; 
            set
            {
                reportWindowVisible = value;
                Signal();
            }
        }

        public Visibility UserButtonsVisible 
        { 
            get => userButtonsVisible; 
            set
            {
                userButtonsVisible = value;
                Signal();
            }
        }
        public Visibility LinksButtonsVisible 
        { 
            get => linksButtonsVisible; 
            set
            {
                linksButtonsVisible = value;
                Signal();
            }
        }

        public Swipes Swiper 
        { 
            get => swiper; 
            set
            {
                swiper = value;
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



        public LikedWindowViewModel(LikedWindow likedWindow, Swipes swiper)
        {
            
            swiper.IsNotificated = true;
            SwipeDB.GetDb().Update(swiper);

            ReasonsSelectAll();
            Swiper = swiper;
            User = UserStatic.CurrentUser;

            List<User> users = UserDB.GetDb().SelectAll();
            var user = users.FirstOrDefault(u => u.ContactId == swiper.Swiper.ContactId)?.Contact;

            Swiper.Swiper.Contact = user;

            SendReport = new CommandVM(() =>
            {
                if (SelectedReason == null)
                {
                    MessageBox.Show("Сначала выбери причину жалобы");
                }
                else
                {
                    Report report = new Report();
                    report.ReportedId = Swiper.Swiper.Id;
                    report.ReporterId = UserStatic.CurrentUser.Id;
                    report.Text = ReportText;
                    report.Reason = new Reason();
                    report.Reason.Type = SelectedReason.Type;
                    report.ReasonId = SelectedReason.Id;
                    ReportDB.GetDb().InsertReport(report);
                    ReportWindowVisible = Visibility.Hidden;
                }
            }, () => !string.IsNullOrWhiteSpace(ReportText));

            

            LinkVisible = new CommandVM(() =>
            {                
                UserButtonsVisible = Visibility.Collapsed;
                LinksButtonsVisible = Visibility.Visible;
            }, () => true);

            Dislike = new CommandVM(() =>
            {              
                likedWindow.Close();
                SwipeDB.GetDb().Remove(Swiper);
            }, () => true);

            ReportVisible = new CommandVM(() =>
            {
                ReportWindowVisible = Visibility.Visible;
                UserButtonsVisible = Visibility.Hidden;
            }, () => true);

            ReturnUsersButtons = new CommandVM(() =>
            {
                UserButtonsVisible = Visibility.Visible;
                ReportWindowVisible = Visibility.Collapsed;
            }, () => true);

            CloseWindow = new CommandVM(() =>
            {
                likedWindow.Close();
            }, () => true);
            OpenEditWindow = new CommandVM(() =>
            {
                EditProfileWindow editProfileWindow = new EditProfileWindow();
                editProfileWindow.ShowDialog();
            }, () => true);

          
        }

        private void ReasonsSelectAll()
        {
            Reasons = ReportDB.GetDb().SelectAll();
        }
    }
}
