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
    public class RegisterWindowViewModel : BaseVM
    {
        private User user;
        private List<User> users;
        private string userLogin;
        private string userPassword;
        private string userRepeatPassword;


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

        public string UserRepeatPassword 
        { 
            get => userRepeatPassword; 
            set
            {
                userRepeatPassword = value;
                Signal();
            }
        }

        public ICommand SaveNewUserLoginPassword { get; }

        public RegisterWindowViewModel()
        {
            SelectAll();
            
            SaveNewUserLoginPassword = new CommandVM(() =>
            {
               
                if (Users.Any(u => u.LoginPassword.Login == UserLogin))
                {
                    MessageBox.Show("Пользователь с таким логином уже существует");
                }                
                else if (UserPassword != UserRepeatPassword)
                {
                    MessageBox.Show("Пароли не совпадают");
                }
                else
                {
                    User user = new User();     
                    user.LoginPassword = new LoginPassword();                    
                    user.LoginPassword.Login = UserLogin;
                    user.LoginPassword.Password = UserPassword;
                    UserDB.GetDb().Insert(user);
                    UserStatic.CurrentUser = user;
                    AddProfileWindow addProfileWindow = new AddProfileWindow();
                    addProfileWindow.ShowDialog();
                }
            }, () => !string.IsNullOrWhiteSpace(UserLogin) && !string.IsNullOrWhiteSpace(UserPassword) && !string.IsNullOrWhiteSpace(UserRepeatPassword));
        }

        private void SelectAll()
        {
            Users = new List<User>(UserDB.GetDb().SelectAll());
        }
    }
}
