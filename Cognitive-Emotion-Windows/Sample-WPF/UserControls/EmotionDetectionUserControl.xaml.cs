using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Microsoft.ProjectOxford.Common.Contract;

namespace EmotionAPI_WPF_Samples
{
    /// <summary>
    /// EmotionDetectionUserControl.xaml
    /// </summary>
    public partial class EmotionDetectionUserControl : UserControl
    {
        public static readonly DependencyProperty ImageProperty =
            DependencyProperty.Register(
                "Image",
                typeof(BitmapImage),
                typeof(EmotionDetectionUserControl),
                new FrameworkPropertyMetadata(new PropertyChangedCallback(ImageChangedCallback)));

        public static readonly DependencyProperty EmotionsProperty =
            DependencyProperty.Register(
                "Emotions",
                typeof(Emotion[]),
                typeof(EmotionDetectionUserControl),
                new FrameworkPropertyMetadata(new PropertyChangedCallback(EmotionsChangedCallback)));

        public EmotionDetectionUserControl()
        {
            InitializeComponent();
            DataContext = this;
        }

        public Uri ImageUri
        {
            get; set;
        }

        public BitmapImage Image
        {
            get
            {
                return (BitmapImage)GetValue(ImageProperty);
            }

            set
            {
                SetValue(ImageProperty, value);
            }
        }

        public Emotion[] Emotions
        {
            get
            {
                return (Emotion[])GetValue(EmotionsProperty);
            }

            set
            {
                SetValue(EmotionsProperty, value);
            }
        }

        private static void ImageChangedCallback(DependencyObject obj, DependencyPropertyChangedEventArgs eventArg)
        {
            EmotionDetectionUserControl userControl = obj as EmotionDetectionUserControl;

            if (userControl != null)
            {
                userControl._image.Source = (BitmapImage)eventArg.NewValue;
                // LIMPANDO RESULTADOS
                userControl._emotionList._resultListBox.ItemsSource = null;
            }
        }

        private static void EmotionsChangedCallback(DependencyObject obj, DependencyPropertyChangedEventArgs eventArg)
        {
            MainWindow window = (MainWindow)Application.Current.MainWindow;

            if (window != null)
            {
                EmotionDetectionUserControl userControl = obj as EmotionDetectionUserControl;

                if (userControl != null)
                {
                    //
                    //  rectangles
                    //
                    window.DrawFaceRectangle(userControl._image, userControl.Image, userControl.Emotions);

                    //
                    // 3 emotions / scores.
                    //
                    window.ListEmotionResult(userControl.ImageUri, userControl._emotionList._resultListBox, userControl.Emotions);
                }
            }
        }
    }
}
