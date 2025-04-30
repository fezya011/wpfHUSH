using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using wpfHUSH.VMTools;

namespace wpfHUSH.ViewModel
{
    public class LikedWindowViewModel : BaseVM
    {
        private Visibility linksButtonsVisible = Visibility.Collapsed;
        private Visibility userButtonsVisible;
        private Visibility reportWindowVisible = Visibility.Collapsed;

        public ICommand LinkVisible { get; }
        public ICommand ReportVisible { get; }

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
        public LikedWindowViewModel()
        {
            LinkVisible = new CommandVM(() =>
            {
                UserButtonsVisible = Visibility.Collapsed;
                LinksButtonsVisible = Visibility.Visible;
            }, () => true);

            ReportVisible = new CommandVM(() =>
            {
                ReportWindowVisible = Visibility.Visible;
            }, () => true);

            
        }
    }
}
