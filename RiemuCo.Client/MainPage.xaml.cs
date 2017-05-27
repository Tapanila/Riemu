using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Net.Http;
using Windows.Media.Capture;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace RiemuCo.Client
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void Button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var cred = new StorageCredentials("riemu", "rsKKZg4bMaToTm+C7ELEFu6brMYMYVC9C9jWebGE+UJOfGVnY94wz2z/0jT+QnF1dqwLv3avBX3RprEhrtoaJQ==");
            var container = new CloudBlobContainer(new Uri("http://riemu.blob.core.windows.net/images/"), cred);

            CameraCaptureUI captureUI = new CameraCaptureUI();
            captureUI.PhotoSettings.Format = CameraCaptureUIPhotoFormat.Jpeg;

            StorageFile photo = await captureUI.CaptureFileAsync(CameraCaptureUIMode.Photo);

            var myUniqueFileName = string.Format("{0}.jpeg", DateTime.Now.Ticks);

            var blob = container.GetBlockBlobReference(myUniqueFileName);

            await blob.UploadFromFileAsync(photo);

            var subscriptionKey = "c55ecc393a2b4613b568279387cd8569";

            var client = new HttpClient();
            var queryString = blob.StorageUri.PrimaryUri.AbsoluteUri;

            // Request headers 
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);

            var uri = "https://riemu.azure-api.net/smile/api/Happy?imageUrl=" + queryString;

            var response = await client.GetAsync(uri);
            var repsonseString = await response.Content.ReadAsStringAsync();
            if (repsonseString == "true")
            {
                MessageDialog dialog = new MessageDialog("Person smiled");
                await dialog.ShowAsync();
            }
            else
            {
                MessageDialog dialog = new MessageDialog("Person didn't smiled");
                await dialog.ShowAsync();
            }
        }
    }
}
