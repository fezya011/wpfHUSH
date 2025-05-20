using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
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
        public CommandVM<Button> ClickWoman { get; set; }
        public CommandVM<Button> ClickMan { get; set; }
        public ICommand ResetButton { get; }


        public AddProfileWindowViewModel(AddProfileWindow addProfileWindow)
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
                var maxAllowedNameLenght = 13;
                var maxAllowedAboutLenght = 210;
                var maxAllowedCityLenght = 20;
                if (Age > 50)
                {
                    MessageBox.Show("Вы были перенаправлены на зеркало нашего приложения: gosuslugi.ru",
                                   "Перенаправление",
                                   MessageBoxButton.OK,
                                   MessageBoxImage.Information);

                    try
                    {
                        Process.Start(new ProcessStartInfo("https://gosuslugi.ru")
                        {
                            UseShellExecute = true
                        });
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Не удалось открыть сайт: {ex.Message}",
                                       "Ошибка",
                                       MessageBoxButton.OK,
                                       MessageBoxImage.Error);
                    }
                }
                else if (Age < 14)
                {
                    MessageBox.Show("Вы слишком малы для использования нашего приложения!");
                }
                else if (IsPhotoAdded == false)
                {
                    MessageBox.Show("Сначала добавь фото");
                    return;
                }
                else if (Name.Length >= maxAllowedNameLenght)
                {
                    MessageBox.Show("Лимит символов имени превышен!");
                }
                else if (City.Length >= maxAllowedNameLenght)
                {
                    MessageBox.Show("Лимит символов города превышен!");
                }
                else if (About.Length > maxAllowedAboutLenght)
                {
                    MessageBox.Show("Лимит символов описания превышен!");

                }
                else 
                {
                    User user = UserStatic.CurrentUser;
                    user.Contact = new Contact();
                    user.Contact.VK = VKLink;
                    user.Contact.Telegram = TelegramLink;
                    user.Name = Name;
                    user.About = About;
                    user.Age = Age;
                    user.Gender = Gender;
                    user.City = City;
                    user.RoleId = 1;
                    user.LoginPasswordId = UserStatic.CurrentUser.LoginPassword.Id;
                    UserDB.GetDb().Update(user);
                    user = UserStatic.CurrentUser;
                    SearchWindow searchWindow = new SearchWindow();
                    addProfileWindow.Hide();
                    searchWindow.ShowDialog();
                }
                
            }, () => !string.IsNullOrWhiteSpace(Name) && !string.IsNullOrWhiteSpace(About) && !string.IsNullOrWhiteSpace(City) && Age != 0 && !string.IsNullOrWhiteSpace(VKLink) && !string.IsNullOrWhiteSpace(TelegramLink));

            ResetButton = new CommandVM(() =>
            {
                IsPhotoAdded = false;
                Image = "/Pictures/Group 21.png";
                Name = null;
                Age = 0;
                About = null;
                City = null;
                VKLink = null;
                TelegramLink = null;
            }, () => true);

            ClickWoman = new CommandVM<Button>((button) =>
            {
                Style style = button.Style;
                button.Style = null;
                button.Style = style;
                Gender = true;
            },
            () => true);

            ClickMan = new CommandVM<Button>((button) =>
            {
                Style style = button.Style;
                button.Style = null;
                button.Style = style;
                Gender = false;
            },
            () => true);
        }

        
    }

}
