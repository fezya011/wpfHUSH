using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private ObservableCollection<Item> _items;
        public ObservableCollection<Item> Items
        {
            get => _items;
            set
            {
                _items = value;
                Signal();
            }
        }

        private Visibility linksButtonsVisible = Visibility.Collapsed;
        private Visibility userButtonsVisible;
        private Visibility reportWindowVisible = Visibility.Collapsed;

        public ICommand LinkVisible { get; }
        public ICommand ReportVisible { get; }
        public ICommand ReturnUsersButtons { get; }
        public ICommand CloseWindow { get; }
        public ICommand OpenEditWindow { get; }

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
        public LikedWindowViewModel(LikedWindow likedWindow)
        {
            LinkVisible = new CommandVM(() =>
            {
                UserButtonsVisible = Visibility.Collapsed;
                LinksButtonsVisible = Visibility.Visible;
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

            Items = new ObservableCollection<Item>
            {
                new Item { Id = 1, Name = "Элемент 1" },
                new Item { Id = 2, Name = "Элемент 2" },
                new Item { Id = 3, Name = "Элемент 3" }
            };

        }
    }
}
public class Item
{
    public int Id { get; set; }
    public string Name { get; set; }
}