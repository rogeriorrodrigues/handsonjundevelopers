
using System.ComponentModel;
using System.IO;
using System.IO.IsolatedStorage;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace SampleUserControlLibrary
{
    
    public partial class SubscriptionKeyPage : Page, INotifyPropertyChanged
    {
        private readonly string _isolatedStorageSubscriptionKeyFileName = "Subscription.txt";
        private readonly string _isolatedStorageSubscriptionEndpointFileName = "SubscriptionEndpoint.txt";

        private readonly string _defaultSubscriptionKeyPromptMessage = "Coloque a sua chave aqui";
        private readonly string _defaultSubscriptionEndpointPromptMessage = "";

        private static string s_subscriptionKey;
        private static string s_subscriptionEndpoint;

        private SampleScenarios _sampleScenarios;
        public SubscriptionKeyPage(SampleScenarios sampleScenarios)
        {
            InitializeComponent();
            _sampleScenarios = sampleScenarios;

            DataContext = this;
            SubscriptionKey = GetSubscriptionKeyFromIsolatedStorage();
            SubscriptionEndpoint = GetSubscriptionEndpointFromIsolatedStorage();
        }

       
        public string SubscriptionKey
        {
            get
            {
                return s_subscriptionKey;
            }

            set
            {
                s_subscriptionKey = value;
                OnPropertyChanged<string>();
                _sampleScenarios.SubscriptionKey = s_subscriptionKey;
            }
        }

        
        public string SubscriptionEndpoint
        {
            get
            {
                return s_subscriptionEndpoint;
            }

            set
            {
                s_subscriptionEndpoint = value;
                OnPropertyChanged<string>();
                _sampleScenarios.SubscriptionEndpoint = s_subscriptionEndpoint;
            }
        }

        
        public event PropertyChangedEventHandler PropertyChanged;

     
        private void OnPropertyChanged<T>([CallerMemberName]string caller = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(caller));
            }
        }

        
        private string GetSubscriptionKeyFromIsolatedStorage()
        {
            string subscriptionKey = null;

            using (IsolatedStorageFile isoStore = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null))
            {
                try
                {
                    using (var iStream = new IsolatedStorageFileStream(_isolatedStorageSubscriptionKeyFileName, FileMode.Open, isoStore))
                    {
                        using (var reader = new StreamReader(iStream))
                        {
                            subscriptionKey = reader.ReadLine();
                        }
                    }
                }
                catch (FileNotFoundException)
                {
                    subscriptionKey = null;
                }
            }
            if (string.IsNullOrEmpty(subscriptionKey))
            {
                subscriptionKey = _defaultSubscriptionKeyPromptMessage;
            }
            return subscriptionKey;
        }

        
        private string GetSubscriptionEndpointFromIsolatedStorage()
        {
            string subscriptionEndpoint = null;

            using (IsolatedStorageFile isoStore = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null))
            {
                try
                {
                    using (var iStreamForEndpoint = new IsolatedStorageFileStream(_isolatedStorageSubscriptionEndpointFileName, FileMode.Open, isoStore))
                    {
                        using (var readerForEndpoint = new StreamReader(iStreamForEndpoint))
                        {
                            subscriptionEndpoint = readerForEndpoint.ReadLine();
                        }
                    }
                }
                catch (FileNotFoundException)
                {
                    subscriptionEndpoint = null;
                }
            }
            if (string.IsNullOrEmpty(subscriptionEndpoint))
            {
                subscriptionEndpoint = _defaultSubscriptionEndpointPromptMessage;
            }
            return subscriptionEndpoint;
        }


       
        private void SaveSubscriptionKeyToIsolatedStorage(string subscriptionKey)
        {
            using (IsolatedStorageFile isoStore = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null))
            {
                using (var oStream = new IsolatedStorageFileStream(_isolatedStorageSubscriptionKeyFileName, FileMode.Create, isoStore))
                {
                    using (var writer = new StreamWriter(oStream))
                    {
                        writer.WriteLine(subscriptionKey);
                    }
                }
            }
        }

       
        private void SaveSubscriptionEndpointToIsolatedStorage(string subscriptionEndpoint)
        {
            using (IsolatedStorageFile isoStore = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null))
            {
                using (var oStream = new IsolatedStorageFileStream(_isolatedStorageSubscriptionEndpointFileName, FileMode.Create, isoStore))
                {
                    using (var writer = new StreamWriter(oStream))
                    {
                        writer.WriteLine(subscriptionEndpoint);
                    }
                }
            }
        }

      
        public void SetSubscriptionEndpoint(string endpoint)
        {
            string subscriptionEndpoint = null;
            subscriptionEndpoint = GetSubscriptionEndpointFromIsolatedStorage();
            if(string.Equals(subscriptionEndpoint, _defaultSubscriptionEndpointPromptMessage))
            {
                SubscriptionEndpoint = endpoint;
            }
        }

        
        private void SaveSetting_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveSubscriptionKeyToIsolatedStorage(SubscriptionKey);
                SaveSubscriptionEndpointToIsolatedStorage(SubscriptionEndpoint);
                MessageBox.Show("Seus dados de API foram Salvos na máquina", "Configuraçoes");
            }
            catch (System.Exception exception)
            {
                MessageBox.Show("Falhar ao salvar: " + exception.Message,
                    "Configuraçoes", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteSetting_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SubscriptionKey = _defaultSubscriptionKeyPromptMessage;
                SubscriptionEndpoint = _defaultSubscriptionEndpointPromptMessage;
                SaveSubscriptionEndpointToIsolatedStorage("");
                SaveSubscriptionKeyToIsolatedStorage("");
                MessageBox.Show("Os dados foram apagados.", "Configuraçoes");
            }
            catch (System.Exception exception)
            {
                MessageBox.Show("Falha ao apagar os dados: " + exception.Message,
                    "Configuraçoes", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void GetKeyButton_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.microsoft.com/cognitive-services/en-us/sign-up");
        }
    }
}
