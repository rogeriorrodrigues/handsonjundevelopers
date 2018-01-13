
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using SampleUserControlLibrary;
using Microsoft.ProjectOxford.Common.Contract;

namespace EmotionAPI_WPF_Samples
{
    public class EmotionResultDisplayItem
    {
        public Uri ImageSource { get; set; }

        public System.Windows.Int32Rect UIRect { get; set; }
        public string Emotion1 { get; set; }
        public string Emotion2 { get; set; }
        public string Emotion3 { get; set; }
    }

    /// <summary>
    /// Tela inicial
    /// </summary>
    public partial class MainWindow : Window
    {
        public SampleScenarios ScenarioControl
        {
            get
            {
                return _scenariosControl;
            }
        }

        public MainWindow()
        {
            InitializeComponent();

            //
            // Cenários
            //
            _scenariosControl.SampleTitle = "Emotion API";
            _scenariosControl.SampleScenarioList = new Scenario[]
            {
                new Scenario { Title = "Testar a partir de imagem local", PageClass = typeof(DetectEmotionUsingStreamPage) },
                new Scenario { Title = "Testar a partir de URL", PageClass = typeof(DetectEmotionUsingURLPage)}
            };
        }

        public void Log(string message)
        {
            _scenariosControl.Log(message);
        }

        public void LogEmotionResult(Emotion[] emotionResult)
        {
            int emotionResultCount = 0;
            if (emotionResult != null && emotionResult.Length > 0)
            {
                foreach (Emotion emotion in emotionResult)
                {
                    Log("Emotion[" + emotionResultCount + "]");
                    Log("  .FaceRectangle = left: " + emotion.FaceRectangle.Left
                             + ", top: " + emotion.FaceRectangle.Top
                             + ", width: " + emotion.FaceRectangle.Width
                             + ", height: " + emotion.FaceRectangle.Height);

                    Log("  Raiva    : " + emotion.Scores.Anger.ToString());
                    Log("  Desprezo : " + emotion.Scores.Contempt.ToString());
                    Log("  Desgosto  : " + emotion.Scores.Disgust.ToString());
                    Log("  Medo     : " + emotion.Scores.Fear.ToString());
                    Log("  Felicidade: " + emotion.Scores.Happiness.ToString());
                    Log("  Neutro  : " + emotion.Scores.Neutral.ToString());
                    Log("  Tristeza  : " + emotion.Scores.Sadness.ToString());
                    Log("  Surpreso  : " + emotion.Scores.Surprise.ToString());
                    Log("");
                    emotionResultCount++;
                }
            }
            else
            {
                Log("Sem deteçao. Motivos:\n" +
                    "    Imagem com rosto pequeno \n" +
                    "    Sem rosto na imagem \n" +
                    "    Angulo dificil de detectar \n" +
                    "    ou outros fatores");
            }
        }

        public void DrawFaceRectangle(Image image, BitmapImage bitmapSource, Emotion[] emotionResult)
        {
            if (emotionResult != null && emotionResult.Length > 0)
            {
                DrawingVisual visual = new DrawingVisual();
                DrawingContext drawingContext = visual.RenderOpen();

                drawingContext.DrawImage(bitmapSource,
                    new Rect(0, 0, bitmapSource.Width, bitmapSource.Height));

                double dpi = bitmapSource.DpiX;
                double resizeFactor = 96 / dpi;

                foreach (var emotion in emotionResult)
                {
                    Microsoft.ProjectOxford.Common.Rectangle faceRect = emotion.FaceRectangle;

                    drawingContext.DrawRectangle(
                        Brushes.Transparent,
                        new Pen(Brushes.Cyan, 4),
                        new Rect(
                            faceRect.Left * resizeFactor,
                            faceRect.Top * resizeFactor,
                            faceRect.Width * resizeFactor,
                            faceRect.Height * resizeFactor)
                    );
                }

                drawingContext.Close();

                RenderTargetBitmap faceWithRectBitmap = new RenderTargetBitmap(
                    (int)(bitmapSource.PixelWidth * resizeFactor),
                    (int)(bitmapSource.PixelHeight * resizeFactor),
                    96,
                    96,
                    PixelFormats.Pbgra32);

                faceWithRectBitmap.Render(visual);

                image.Source = faceWithRectBitmap;
            }
        }


        public void ListEmotionResult(Uri imageUri, ListBox resultListBox, Emotion[] emotionResult)
        {
            if (emotionResult != null)
            {
                EmotionResultDisplay[] resultDisplay = new EmotionResultDisplay[8];
                List<EmotionResultDisplayItem> itemSource = new List<EmotionResultDisplayItem>();
                for (int i = 0; i < emotionResult.Length; i++)
                {
                    Emotion emotion = emotionResult[i];
                    resultDisplay[0] = new EmotionResultDisplay { EmotionString = "Raiva", Score = emotion.Scores.Anger };
                    resultDisplay[1] = new EmotionResultDisplay { EmotionString = "Desprezo", Score = emotion.Scores.Contempt };
                    resultDisplay[2] = new EmotionResultDisplay { EmotionString = "Desgosto", Score = emotion.Scores.Disgust };
                    resultDisplay[3] = new EmotionResultDisplay { EmotionString = "Medo", Score = emotion.Scores.Fear };
                    resultDisplay[4] = new EmotionResultDisplay { EmotionString = "Felicidade", Score = emotion.Scores.Happiness };
                    resultDisplay[5] = new EmotionResultDisplay { EmotionString = "Neutro", Score = emotion.Scores.Neutral };
                    resultDisplay[6] = new EmotionResultDisplay { EmotionString = "Tristeza", Score = emotion.Scores.Sadness };
                    resultDisplay[7] = new EmotionResultDisplay { EmotionString = "Surpresa", Score = emotion.Scores.Surprise };

                    Array.Sort(resultDisplay, (result1, result2) => result2.Score.CompareTo(result1.Score));

                    String[] emotionStrings = new String[3];
                    for (int j = 0; j < 3; j++)
                    {
                        emotionStrings[j] = resultDisplay[j].EmotionString + ":" + resultDisplay[j].Score.ToString("0.000000"); ;
                    }

                    itemSource.Add(new EmotionResultDisplayItem
                    {
                        ImageSource = imageUri,
                        UIRect = new Int32Rect(emotion.FaceRectangle.Left, emotion.FaceRectangle.Top, emotion.FaceRectangle.Width, emotion.FaceRectangle.Height),
                        Emotion1 = emotionStrings[0],
                        Emotion2 = emotionStrings[1],
                        Emotion3 = emotionStrings[2]
                    });
                }
                resultListBox.ItemsSource = itemSource;
            }
        }      
    }
}
