// 
// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license.
// 
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

// -----------------------------------------------------------------------
// Parte 1
// Insira os namespaces EmotionServiceClient
// -----------------------------------------------------------------------


// -----------------------------------------------------------------------
// Parte 1
// -----------------------------------------------------------------------

namespace EmotionAPI_WPF_Samples
{
    internal class EmotionResultDisplay
    {
        public string EmotionString
        {
            get;
            set;
        }
        public float Score
        {
            get;
            set;
        }

        public int OriginalIndex
        {
            get;
            set;
        }
    }

    /// <summary>
    /// Iniciando
    /// </summary>
    public partial class DetectEmotionUsingStreamPage : Page
    {
        public DetectEmotionUsingStreamPage()
        {
            InitializeComponent();
        }

     
        private async Task<Emotion[]> UploadAndDetectEmotions(string imageFilePath)
        {
            MainWindow window = (MainWindow)Application.Current.MainWindow;
            string subscriptionKey = window.ScenarioControl.SubscriptionKey;

            window.Log("EmotionServiceClient iniciado");

            // -----------------------------------------------------------------------
            // Parte 3
            // -----------------------------------------------------------------------

            //
            // Crie o cliente service
            //
            

            window.Log("Calling EmotionServiceClient.RecognizeAsync()...");
            try
            {
              //Crie Load da imagem

            }
            catch (Exception exception)
            {
                window.Log(exception.ToString());
                return null;
            }
            // -----------------------------------------------------------------------
            // Parte 3
            // -----------------------------------------------------------------------

        }

        private async void LoadImageButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = (MainWindow)Application.Current.MainWindow;

            Microsoft.Win32.OpenFileDialog openDlg = new Microsoft.Win32.OpenFileDialog();
            openDlg.Filter = "JPEG Image(*.jpg)|*.jpg";
            bool? result = openDlg.ShowDialog(window);

            if (!(bool)result)
            {
                return;
            }

            string imageFilePath = openDlg.FileName;
            Uri fileUri = new Uri(imageFilePath);

            BitmapImage bitmapSource = new BitmapImage();
            bitmapSource.BeginInit();
            bitmapSource.CacheOption = BitmapCacheOption.None;
            bitmapSource.UriSource = fileUri;
            bitmapSource.EndInit();

            _emotionDetectionUserControl.ImageUri = fileUri;
            _emotionDetectionUserControl.Image = bitmapSource;

            //
            // Enviando a imagem
            //
            window.ScenarioControl.ClearLog();
            _detectionStatus.Text = "Detectando...";

            //Crie envio da imagem
            

            _detectionStatus.Text = "Finalizada...";

            //
            // Log 
            //
            window.Log("");
            window.Log("Detection Result:");
            window.LogEmotionResult(emotionResult);

            _emotionDetectionUserControl.Emotions = emotionResult;
        }
    }
}
