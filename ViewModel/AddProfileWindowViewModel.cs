using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using wpfHUSH.Model;
using wpfHUSH.View;
using wpfHUSH.VMTools;

namespace wpfHUSH.ViewModel
{
    class AddProfileWindowViewModel : BaseVM
    {
        private bool isPhotoAdded;
        private object image;
        private bool isProfileNull;
        private string name;
        private string about;
        private string city;
        private int age;
        private bool gender;
        private string telegramLink;
        private string vKLink;

        public bool IsPhotoAdded 
        { 
            get => isPhotoAdded; 
            set
            {
                isPhotoAdded = value;
                Signal();
            }
        }

        public object Image 
        { 
            get => image; 
            set
            {
                image = value;
                Signal();
            }
        }

        public bool IsProfileNull 
        { 
            get => isProfileNull;
            set
            {
                isProfileNull = value;
                Signal();
            }
        }

        public string Name 
        { 
            get => name;
            set
            {
                name = value;
                Signal();
            }
        }
        public string About 
        { 
            get => about;
            set
            {
                about = value;
                Signal();
            }
        }
        public string City 
        { 
            get => city;
            set
            {
                city = value;
                Signal();
            }
        }
        public int Age 
        { 
            get => age; 
            set
            {
                age = value;
                Signal();
            }
        }
        public bool Gender 
        { 
            get => gender; 
            set
            {
                gender = value;
                Signal();
            }
        }

        public string TelegramLink 
        { 
            get => telegramLink;
            set
            {
                telegramLink = value;
                Signal();
            }
        }
        public string VKLink 
        { 
            get => vKLink; 
            set
            {
                vKLink = value;
                Signal();
            }
        }

        public ICommand SaveProfileButton { get; }
        public ICommand OpenAddPhotoWindowButton { get; }
        //public CommandVM<Button> ClickMan {  get; set; }
        public ICommand ResetButton { get; }


        public AddProfileWindowViewModel()
        {
            Image = "/Pictures/Group 21.png";
            OpenAddPhotoWindowButton = new CommandVM(() =>
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif;*.tiff";
                openFileDialog.Title = "Выберите изображение";
                openFileDialog.Multiselect = false;
                if (openFileDialog.ShowDialog() == true)
                {
                    UserStatic.CurrentUser.Photo = File.ReadAllBytes(openFileDialog.FileName);
                    IsPhotoAdded = true;
                    Image = UserStatic.CurrentUser.Photo;
                }
                
            }, () => true);

            SaveProfileButton = new CommandVM(() =>
            {
                User user = UserStatic.CurrentUser;
                user.Name = Name;
                user.About = About;
                user.Age = Age;
                //user.Gender = Gender;
                user.City = City;
                user.RoleId = 1;
                UserDB.GetDb().Update(user);
            }, () => true);

            ResetButton = new CommandVM(() =>
            {
                IsPhotoAdded = false;
                Image = "/Pictures/Group 21.png";
            }, () => true);

            //ClickMan = new CommandVM<Button>((button) =>
            //{
            //    Style style = button.Style;
            //    button.Style = null;
            //    button.Style = style;
            //},
            //() => true);
        }

        
    }

}
