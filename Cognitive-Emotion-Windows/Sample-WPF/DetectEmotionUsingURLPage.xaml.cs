
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

// -----------------------------------------------------------------------
// parte 1
// insira o namesapce para EmotionServiceClient
// -----------------------------------------------------------------------
 

// -----------------------------------------------------------------------
// Parte 1
// -----------------------------------------------------------------------

namespace EmotionAPI_WPF_Samples
{
    /// <summary>
    /// Inicio URL PAGE
    /// </summary>
    public partial class DetectEmotionUsingURLPage : Page
    {
        public DetectEmotionUsingURLPage()
        {
            InitializeComponent();
        }

        
        private async Task<Emotion[]> UploadAndDetectEmotions(string url)
        {
            MainWindow window = (MainWindow)Application.Current.MainWindow;
            string subscriptionKey = window.ScenarioControl.SubscriptionKey;

            // -----------------------------------------------------------------------
            // Parte 2
            // -----------------------------------------------------------------------

            window.Log("EmotionServiceClient iniciado");

            //
            // Crie uma chamada para Project Oxford Emotion API Service client
            //
            

            window.Log("Iniciando EmotionServiceClient.RecognizeAsync()...");
            try
            {
                //
                // Crie URL para detectar Emotions
                //
                
            }
            catch (Exception exception)
            {
                window.Log("Analise falhou. Por favor verifique a sua assinatura ou o log");
                window.Log(exception.ToString());
                return null;
            }
            // -----------------------------------------------------------------------
            // Parte 2
            // -----------------------------------------------------------------------

        }
        private async void LoadImageButton_Click(object sender, RoutedEventArgs e)
        {
            string urlString = URLTextBox.Text;
            Uri uri = new Uri(urlString, UriKind.Absolute);
            MainWindow window = (MainWindow)Application.Current.MainWindow;
            string subscriptionKey = window.ScenarioControl.SubscriptionKey;

            window.ScenarioControl.ClearLog();

            //
            // Load Imagem URL 
            //
            var bitmapSource = new BitmapImage();
            bitmapSource.BeginInit();
            bitmapSource.UriSource = uri;
            bitmapSource.EndInit();

            _emotionDetectionUserControl.ImageUri = uri;
            _emotionDetectionUserControl.Image = bitmapSource;

            _detectionStatus.Text = "Detecting...";

            // Crie o Load da imagem
            

            _detectionStatus.Text = "Detection Done";
            //
            // Log detection result in the log window
            //
            window.Log("");
            window.Log("Detection Result:");
            window.LogEmotionResult(emotionResult);

            _emotionDetectionUserControl.Emotions = emotionResult;
        }
    }
}
