using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace wpfHUSH.Model
{
    public static class WindowManager
    {
        
        public static void ReturnToMainWindow()
        {
            
            var openWindows = Application.Current.Windows.Cast<Window>().ToList();

            foreach (var window in openWindows)
            {
                
                if (window != Application.Current.MainWindow)
                {
                    window.Close();
                }
            }

            
            if (Application.Current.MainWindow != null && !Application.Current.MainWindow.IsVisible)
            {
                Application.Current.MainWindow.Show();
            }

            Application.Current.MainWindow?.Focus();
        }

        
        public static void OpenWindow<T>(Window currentWindow) where T : Window, new()
        {
            var newWindow = new T();
            newWindow.Show();
            currentWindow.Close();
        }
    }
}
