using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using wpfHUSH.View;
using wpfHUSH.VMTools;

namespace wpfHUSH.ViewModel
{
    class AddProfileWindowViewModel
    {
        public ICommand Open { get; set; }

        public AddProfileWindowViewModel()
        {
            Open = new CommandVM(() =>
            {
                SearchWindow vm = new SearchWindow();
                vm.ShowDialog();
            }, () => true);
        }
    }
}
