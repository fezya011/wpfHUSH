using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using wpfHUSH.ViewModel;

namespace wpfHUSH.View
{
    /// <summary>
    /// Логика взаимодействия для SearchWindow.xaml
    /// </summary>
    public partial class SearchWindow : Window
    {
        public SearchWindow()
        {
            InitializeComponent();
            DataContext = new SearchWindowViewModel();
            // Загрузка тестовых изображений (замените на свои)
            var photos = new List<ImageSource>
            {
                new System.Windows.Media.Imaging.BitmapImage(new Uri("photo1.jpg", UriKind.Relative)),
                new System.Windows.Media.Imaging.BitmapImage(new Uri("photo2.jpg", UriKind.Relative)),
                new System.Windows.Media.Imaging.BitmapImage(new Uri("photo3.jpg", UriKind.Relative)),
                new System.Windows.Media.Imaging.BitmapImage(new Uri("photo4.jpg", UriKind.Relative)),
                new System.Windows.Media.Imaging.BitmapImage(new Uri("photo5.jpg", UriKind.Relative))
            };

            PhotosContainer.ItemsSource = photos;

            // Подписываемся на событие нажатия
            PhotosContainer.PreviewMouseDown += (s, e) => StartLadderAnimation();
        }
        private void StartLadderAnimation()
        {
            // Находим все изображения в ItemsControl
            var items = PhotosContainer.Items;
            var generator = PhotosContainer.ItemContainerGenerator;

            for (int i = 0; i < items.Count; i++)
            {
                var container = generator.ContainerFromIndex(i) as ContentPresenter;
                if (container == null) continue;

                var image = VisualTreeHelper.GetChild(container, 0) as Image;
                if (image == null) continue;

                // Создаем анимацию для каждого изображения
                var transformGroup = image.RenderTransform as TransformGroup;
                var translateTransform = transformGroup.Children[0] as TranslateTransform;
                var rotateTransform = transformGroup.Children[1] as RotateTransform;
                var scaleTransform = transformGroup.Children[2] as ScaleTransform;

                // Настройка анимации с задержкой для каждого элемента
                double delay = i * 0.2; // Задержка увеличивается для каждого следующего изображения

                // Анимация перемещения
                var translateAnim = new DoubleAnimation
                {
                    To = -50 + i * 20, // Смещение по X для эффекта лесенки
                    Duration = TimeSpan.FromSeconds(1),
                    BeginTime = TimeSpan.FromSeconds(delay),
                    EasingFunction = new ElasticEase { Oscillations = 1, Springiness = 5 }
                };

                // Анимация вращения
                var rotateAnim = new DoubleAnimation
                {
                    To = -10 + i * 5, // Поворот для эффекта лесенки
                    Duration = TimeSpan.FromSeconds(1),
                    BeginTime = TimeSpan.FromSeconds(delay),
                    EasingFunction = new BackEase { Amplitude = 0.5, EasingMode = EasingMode.EaseOut }
                };

                // Анимация масштабирования
                var scaleAnim = new DoubleAnimation
                {
                    To = 1.1,
                    Duration = TimeSpan.FromSeconds(0.5),
                    AutoReverse = true,
                    BeginTime = TimeSpan.FromSeconds(delay),
                    EasingFunction = new BounceEase { Bounces = 2, Bounciness = 2 }
                };

                // Запуск анимаций
                translateTransform.BeginAnimation(TranslateTransform.XProperty, translateAnim);
                rotateTransform.BeginAnimation(RotateTransform.AngleProperty, rotateAnim);
                scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, scaleAnim);
                scaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, scaleAnim);
            }

            // Возврат в исходное положение через 3 секунды
            var timer = new System.Windows.Threading.DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(3)
            };

            timer.Tick += (s, e) =>
            {
                timer.Stop();
                ResetAnimation();
            };

            timer.Start();
        }

        private void ResetAnimation()
        {
            var items = PhotosContainer.Items;
            var generator = PhotosContainer.ItemContainerGenerator;

            for (int i = 0; i < items.Count; i++)
            {
                var container = generator.ContainerFromIndex(i) as ContentPresenter;
                if (container == null) continue;

                var image = VisualTreeHelper.GetChild(container, 0) as Image;
                if (image == null) continue;

                var transformGroup = image.RenderTransform as TransformGroup;
                var translateTransform = transformGroup.Children[0] as TranslateTransform;
                var rotateTransform = transformGroup.Children[1] as RotateTransform;

                // Анимация возврата
                var translateAnim = new DoubleAnimation
                {
                    To = 0,
                    Duration = TimeSpan.FromSeconds(0.5),
                    BeginTime = TimeSpan.FromSeconds(i * 0.1),
                    EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
                };

                var rotateAnim = new DoubleAnimation
                {
                    To = 0,
                    Duration = TimeSpan.FromSeconds(0.5),
                    BeginTime = TimeSpan.FromSeconds(i * 0.1),
                    EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
                };

                translateTransform.BeginAnimation(TranslateTransform.XProperty, translateAnim);
                rotateTransform.BeginAnimation(RotateTransform.AngleProperty, rotateAnim);
            }
        }
    }
}
   
    }
}
