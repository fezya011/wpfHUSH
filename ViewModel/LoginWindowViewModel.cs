using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using wpfHUSH.Model;
using wpfHUSH.View;
using wpfHUSH.VMTools;

namespace wpfHUSH.ViewModel
{
    class LoginWindowViewModel : BaseVM
    {
        private User user;
        private List<User> users;
        private string userLogin;
        private string userPassword;


        public List<User> Users 
        { 
            get => users;
            set
            {
                users = value;
                Signal();
                
            }
        }

        public string UserLogin 
        { 
            get => userLogin; 
            set
            {
                userLogin = value;
                Signal();
            }
        }

        public string UserPassword 
        { 
            get => userPassword; 
            set
            {
                userPassword = value;
                Signal();
            }
        }

        public ICommand Login { get; }

        public LoginWindowViewModel(LoginWindow loginWindow)
        {
            SelectAll();
            Login = new CommandVM(() =>
            {
                User user = Users.FirstOrDefault(u => u.LoginPassword.Login == UserLogin && u.LoginPassword.Password == UserPassword);
                if (user != null)
                {
                    if (user.RoleId == 1)
                    {
                        UserStatic.CurrentUser = user;
                        SearchWindow searchWindow = new SearchWindow();
                        loginWindow.Hide();
                        searchWindow.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("Ваш профиль забанен!");
                    }
                    
                }
                else
                {
                    MessageBox.Show("Неправильный ввод");
                }

            }, () => !string.IsNullOrWhiteSpace(UserLogin) && !string.IsNullOrWhiteSpace(UserPassword));
        }

        private void SelectAll()
        {
            Users = new List<User>(UserDB.GetDb().SelectAll());
        }
    }
}
