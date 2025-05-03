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
    public class EditProfileWindowViewModel
    {
        public ICommand CloseWindow { get; }

        public EditProfileWindowViewModel(EditProfileWindow editProfileWindow)
        {
            CloseWindow = new CommandVM(() =>
            {
                editProfileWindow.Close();
            }, () => true);
        }
    }
}
