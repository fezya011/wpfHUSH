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

        public string Name { get; set; }

        public ICommand SaveProfile { get; set; }
        public ICommand OpenAddPhotoWindow { get; set; }
        public CommandVM<Button> ClickMan {  get; set; }


        public AddProfileWindowViewModel()
        {
            Image = "/Pictures/Group 21.png";
            OpenAddPhotoWindow = new CommandVM(() =>
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

            SaveProfile = new CommandVM(() =>
            {

            }, () => true);

            ClickMan = new CommandVM<Button>((button) =>
            {
                Style style = button.Style;
                button.Style = null;
                button.Style = style;
            },
            () => true);
        }

        
    }

}
