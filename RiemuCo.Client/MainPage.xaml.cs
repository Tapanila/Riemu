using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Capture;
using Windows.Media.MediaProperties;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace RiemuCo.Client
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private bool _working = false;
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void Grid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (_working)
            {
                return;
            }
            _working = true;
            FacePicture.Visibility = Visibility.Collapsed;
            var cred = new StorageCredentials("riemu", "rsKKZg4bMaToTm+C7ELEFu6brMYMYVC9C9jWebGE+UJOfGVnY94wz2z/0jT+QnF1dqwLv3avBX3RprEhrtoaJQ==");
            var container = new CloudBlobContainer(new Uri("http://blob.riemu.co/images/"), cred);

            var test = new MediaCapture();
            await test.InitializeAsync();
            var file = await KnownFolders.SavedPictures.CreateFileAsync("temp.jpeg", CreationCollisionOption.ReplaceExisting);
            await test.CapturePhotoToStorageFileAsync(ImageEncodingProperties.CreateJpeg(), file);

            BitmapImage bitmapImage = new BitmapImage();
            FileRandomAccessStream stream = (FileRandomAccessStream)await file.OpenAsync(FileAccessMode.Read);

            bitmapImage.SetSource(stream);

            Picture.Source = bitmapImage;

            Picture.Source = bitmapImage;

            var myUniqueFileName = string.Format("{0}.jpeg", DateTime.Now.Ticks);

            var blob = container.GetBlockBlobReference(myUniqueFileName);

            await blob.UploadFromFileAsync(file);

            var subscriptionKey = "c55ecc393a2b4613b568279387cd8569";

            var client = new HttpClient();
            var queryString = blob.StorageUri.PrimaryUri.AbsoluteUri;

            // Request headers 
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);

            var uri = "https://riemu.azure-api.net/smile/api/Happy?imageUrl=" + queryString;

            var response = await client.GetAsync(uri);
            var repsonseString = await response.Content.ReadAsStringAsync();
            BitmapImage faceImage = null;
            if (repsonseString == "true")
            {
                faceImage = new BitmapImage(new Uri("ms-appx://riemuco.cleint/Assets/happy.png"));
            }
            else
            {
                faceImage = new BitmapImage(new Uri("ms-appx://riemuco.cleint/Assets/sad.png"));
            }
            FacePicture.Source = faceImage;
            FacePicture.Visibility = Visibility.Visible;
            _working = false;
        }
    }
}
