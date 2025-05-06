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
    class LoginWindowViewModel : BaseVM
    {
        private User user;
        private List<User> users;

        public User User 
        { 
            get => user;
            set
            {
                user = value;
                Signal();
            }
        }
        public List<User> Users 
        { 
            get => users;
            set
            {
                users = value;
                Signal();
                
            }
        }

        public ICommand Login { get; }

        public LoginWindowViewModel()
        {
            SelectAll();
            Login = new CommandVM(() =>
            {
                User user = Users.FirstOrDefault(u => u.LoginPassword.Login == User.LoginPassword.Login && u.LoginPassword.Password == User.LoginPassword.Password);
                if (user != null)
                {
                    AddProfileWindow addProfileWindow = new AddProfileWindow(user);
                    addProfileWindow.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Неправильный ввод");
                }
            }, () => !string.IsNullOrWhiteSpace(User.LoginPassword.Login) && !string.IsNullOrWhiteSpace(User.LoginPassword.Password));
        }

        private void SelectAll()
        {
            Users = new List<User>(UserDB.GetDb().SelectAll());
        }
    }
}
