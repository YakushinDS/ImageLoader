using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace ImageLoader
{
    public partial class ImageControl : UserControl
    {
        private WebClient client;

        public delegate void ImageDownloadProgressChangedEventHandler(object sender, ImageDownloadProgressChangedEventArgs e);

        public static readonly RoutedEvent ImageDownloadCompletedEvent = EventManager.RegisterRoutedEvent("ImageDownloadCompleted",
            RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(ImageControl));

        public event RoutedEventHandler ImageDownloadCompleted
        {
            add { AddHandler(ImageDownloadCompletedEvent, value); }
            remove { RemoveHandler(ImageDownloadCompletedEvent, value); }
        }

        public static readonly RoutedEvent ImageDownloadProgressChangedEvent = EventManager.RegisterRoutedEvent("ImageDownloadProgressChanged",
            RoutingStrategy.Direct, typeof(ImageDownloadProgressChangedEventHandler), typeof(ImageControl));

        public event ImageDownloadProgressChangedEventHandler ImageDownloadProgressChanged
        {
            add { AddHandler(ImageDownloadProgressChangedEvent, value); }
            remove { RemoveHandler(ImageDownloadProgressChangedEvent, value); }
        }

        public ImageControl()
        {
            InitializeComponent();
            client = new WebClient();
            client.DownloadDataCompleted += client_DownloadDataCompleted;
            client.DownloadProgressChanged += client_DownloadProgressChanged;
            UpdateButtonState();
        }

        private void UpdateButtonState()
        {
            URLTextBox.IsEnabled = !client.IsBusy;
            StartButton.IsEnabled = !client.IsBusy;
            StopButton.IsEnabled = client.IsBusy;
        }

        public void StartDownload()
        {
            if (!client.IsBusy && URLTextBox.Text != String.Empty)
            {
                client.DownloadDataAsync(new Uri(URLTextBox.Text, UriKind.RelativeOrAbsolute));
                UpdateButtonState();
            }
        }

        private void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            RaiseEvent(new ImageDownloadProgressChangedEventArgs(ImageDownloadProgressChangedEvent, e.ProgressPercentage));
        }

        private void client_DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e)
        {
            if (!e.Cancelled)
                if (e.Error == null)
                {
                    try
                    {
                        BitmapImage image = new BitmapImage();

                        using (MemoryStream stream = new MemoryStream(e.Result))
                        {
                            image.BeginInit();
                            image.StreamSource = stream;
                            image.EndInit();
                            image.Freeze();
                        }

                        ImageBox.Source = image;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("The URL doesn't refer to an image", "Image Processing Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        Debug.WriteLine(ex.Message, "BitmapImage");
                    }
                }
                else
                {
                    MessageBox.Show("An error has occurred during a download", "Download Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    Debug.WriteLine(e.Error.Message, "WebClient");
                }

            RaiseEvent(new RoutedEventArgs(ImageDownloadCompletedEvent));
            UpdateButtonState();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            StartDownload();
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            client.CancelAsync();
            UpdateButtonState();
        }
    }
}
