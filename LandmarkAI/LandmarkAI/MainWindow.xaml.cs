using LandmarkAI.Classes;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft;
using Newtonsoft.Json;

namespace LandmarkAI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Image File (*.jpg; *.png)|*.jpg;*.png;*jpeg|All Files (*.*)|*.*";
            dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

            if (dialog.ShowDialog() == true) {
                string filePath = dialog.FileName;
                selectedImage.Source = new BitmapImage(new Uri(filePath));

                makePrediction(filePath);
            }
        }

        private async void makePrediction(string filePath)
        {
            string url = "https://eastus.api.cognitive.microsoft.com/customvision/v3.0/Prediction/c9729fd5-9eed-4bc6-8d01-a9f8aec6fae4/classify/iterations/Iteration3/image";
            string predicateKey = "301cd3ff84bd45d483f99d049c6bd85b";
            string contentType = "application/octet-stream";
            var file = File.ReadAllBytes(filePath);

            using (HttpClient client = new HttpClient()) {
                client.DefaultRequestHeaders.Add("Prediction-Key", predicateKey);


                using (HttpContent content = new ByteArrayContent(file)) {
                    content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(contentType);
                    var response = await client.PostAsync(url, content);

                    var temp = await response.Content.ReadAsStringAsync();

                    List<Prediction> predictions = (JsonConvert.DeserializeObject<CustomView>(temp)).predictions;

                    predictionResult.ItemsSource = predictions;
                }

            }
        }
    }
}
