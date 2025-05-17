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
using System.Xml.Linq;
using wpfHUSH.Model;
using wpfHUSH.View;
using wpfHUSH.VMTools;

namespace wpfHUSH.ViewModel
{
    public class EditProfileWindowViewModel : BaseVM
    {
        private User editableUser;
        private object image;

        public User EditableUser 
        { 
            get => editableUser; 
            set
            {
                editableUser = value;
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

        public ICommand CloseWindow { get; }
        public ICommand Logout { get; }
        public ICommand SaveEdit { get; }
        public ICommand OpenAddPhotoWindowButton { get; }
        public ICommand Reset { get; }

        public EditProfileWindowViewModel(EditProfileWindow editProfileWindow)
        {
            Image = UserStatic.CurrentUser.Photo;
            OpenAddPhotoWindowButton = new CommandVM(() =>
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif;*.tiff";
                openFileDialog.Title = "Выберите изображение";
                openFileDialog.Multiselect = false;
                if (openFileDialog.ShowDialog() == true)
                {
                    UserStatic.CurrentUser.Photo = File.ReadAllBytes(openFileDialog.FileName);
                    Image = UserStatic.CurrentUser.Photo;
                }

            }, () => true);

            EditableUser = new User
            {
                Name = UserStatic.CurrentUser.Name,
                About = UserStatic.CurrentUser.About,
                Age = UserStatic.CurrentUser.Age,
                City = UserStatic.CurrentUser.City,                
                Contact = new Contact
                {
                    VK = UserStatic.CurrentUser.Contact.VK,
                    Telegram = UserStatic.CurrentUser.Contact.Telegram
                }

            };

            SaveEdit = new CommandVM(() =>
            {
                if (EditableUser.Age < 0)
                {
                    MessageBox.Show("Не рановато ли ты к нам?");
                }
                else if (EditableUser.Age > 45)
                {
                    MessageBox.Show("Ты либо слишком старый, либо введи свой реальный возраст!");
                }
                else
                {
                    UserStatic.CurrentUser.Name = EditableUser.Name;
                    UserStatic.CurrentUser.About = EditableUser.About;
                    UserStatic.CurrentUser.Age = EditableUser.Age;
                    UserStatic.CurrentUser.City = EditableUser.City;
                    UserStatic.CurrentUser.Contact.VK = EditableUser.Contact.VK;
                    UserStatic.CurrentUser.Contact.Telegram = EditableUser.Contact.Telegram;
                    UserDB.GetDb().Update(UserStatic.CurrentUser);
                }
                
            }, () => !string.IsNullOrWhiteSpace(EditableUser.Name) && !string.IsNullOrWhiteSpace(EditableUser.About) && !string.IsNullOrWhiteSpace(EditableUser.City) && EditableUser.Age != 0 && !string.IsNullOrWhiteSpace(EditableUser.Contact.VK) && !string.IsNullOrWhiteSpace(EditableUser.Contact.Telegram));

            CloseWindow = new CommandVM(() =>
            {
                editProfileWindow.Close();
            }, () => true);

            Logout = new CommandVM(() =>
            {
                WindowManager.ReturnToMainWindow();
            }, () => true);

            Reset = new CommandVM(() =>
            {
                Image = "/Pictures/EditPhoto.png";
                EditableUser = new User
                {
                    Name = null,
                    About = null,
                    Age = 0,
                    City = null,
                    Contact = new Contact
                    {
                        VK = null,
                        Telegram = null
                    },                   
                };
            }, () => true);
        }
    }
}
