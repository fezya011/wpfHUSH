using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using wpfHUSH.View;
using wpfHUSH.VMTools;

namespace wpfHUSH.ViewModel
{
    public class StartWindowViewModel
    {
        public ICommand OpenRegistration { get; set; }
        public ICommand OpenLogin { get; set; }
        public ICommand open { get; set; }

        public StartWindowViewModel(MainWindow startWindow)
        {
            OpenRegistration = new CommandVM(() =>
            {
                RegisterWindow registerWindow = new RegisterWindow();
                registerWindow.ShowDialog();
            }, () => true);

            OpenLogin = new CommandVM(() =>
            {
                LoginWindow loginWindow = new LoginWindow();
                loginWindow.ShowDialog();
            }, () => true);

            open = new CommandVM(() =>
            {
                AddProfileWindow addProfileWindow = new AddProfileWindow();
                addProfileWindow.ShowDialog();
            }, () => true);
        }
    }
}
