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
        /// <summary>
        /// Закрывает все окна, кроме MainWindow, и показывает MainWindow.
        /// </summary>
        public static void ReturnToMainWindow()
        {
            // Получаем все открытые окна
            var openWindows = Application.Current.Windows.Cast<Window>().ToList();

            foreach (var window in openWindows)
            {
                // Если это не MainWindow — закрываем
                if (window != Application.Current.MainWindow)
                {
                    window.Close();
                }
            }

            // Если MainWindow скрыто — показываем
            if (Application.Current.MainWindow != null && !Application.Current.MainWindow.IsVisible)
            {
                Application.Current.MainWindow.Show();
            }

            // Фокусируем MainWindow
            Application.Current.MainWindow?.Focus();
        }

        /// <summary>
        /// Открывает новое окно и закрывает текущее.
        /// </summary>
        /// <typeparam name="T">Тип нового окна (например, MainWindow, SettingsWindow)</typeparam>
        /// <param name="currentWindow">Текущее окно, которое нужно закрыть</param>
        public static void OpenWindow<T>(Window currentWindow) where T : Window, new()
        {
            var newWindow = new T();
            newWindow.Show();
            currentWindow.Close();
        }
    }
}
