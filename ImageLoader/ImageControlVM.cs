using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace ImageLoader
{
    public class ImageControlVM : INotifyPropertyChanged
    {
        private WebClient client;
        private RelayComand startCommand;
        private RelayComand stopCommand;
        private bool hasError;
        private string errorMessage;
        private int progressPercentage;
        private string url;
        private BitmapImage image;
        private bool isBusy;

        public RelayComand Start
        {
            protected set { startCommand = value; }

            get
            {
                return startCommand ??
                    (startCommand = new RelayComand((p) =>
                    {
                        StartDownload();
                    }, (p) => !IsBusy));
            }
        }

        public RelayComand Stop
        {
            protected set { stopCommand = value; }

            get
            {
                return stopCommand ??
                    (stopCommand = new RelayComand((p) =>
                    {
                        client.CancelAsync();
                    }, (p) => IsBusy));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ImageControlVM()
        {
            client = new WebClient();
            client.DownloadDataCompleted += client_DownloadDataCompleted;
            client.DownloadProgressChanged += client_DownloadProgressChanged;
        }

        public BitmapImage Image
        {
            get { return image; }

            set
            {
                image = value;
                InvokePropertyChanged("Image");
            }
        }

        public string Url
        {
            get { return url; }

            set
            {
                url = value;
                InvokePropertyChanged("Url");
            }
        }

        public int ProgressPercentage
        {
            get { return progressPercentage; }

            protected set
            {
                progressPercentage = value;
                InvokePropertyChanged("ProgressPercentage");
            }
        }

        public string ErrorMessage
        {
            get { return errorMessage; }

            set
            {
                errorMessage = value;
                InvokePropertyChanged("ErrorMessage");
            }
        }

        public bool HasError
        {
            get { return hasError; }

            set
            {
                hasError = value;
                InvokePropertyChanged("HasError");
            }
        }

        public bool IsBusy
        {
            get { return isBusy; }

            set
            {
                isBusy = value;
                InvokePropertyChanged("IsBusy");
                Start.RaiseCanExecuteChanged();
                Stop.RaiseCanExecuteChanged();
            }
        }

        public void StartDownload()
        {
            if (!client.IsBusy && Url != String.Empty && Url != null)
            {
                client.DownloadDataAsync(new Uri(Url, UriKind.RelativeOrAbsolute));
                IsBusy = client.IsBusy;
            }
        }

        private void InvokePropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        private void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            ProgressPercentage = e.ProgressPercentage;
        }

        private void client_DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e)
        {
            IsBusy = client.IsBusy;

            if (!e.Cancelled)
                if (e.Error == null)
                {
                    try
                    {
                        BitmapImage image = new BitmapImage();
                        image.BeginInit();
                        image.StreamSource = new MemoryStream(e.Result);
                        image.EndInit();
                        image.Freeze();
                        Image = image;
                        HasError = false;
                    }
                    catch (Exception ex)
                    {
                        ErrorMessage = "The URL doesn't refer to an image";
                        HasError = true;
                        MessageBox.Show("The URL doesn't refer to an image", "Image Processing Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        Debug.WriteLine(ex.Message, "BitmapImage");
                    }
                }
                else
                {
                    ErrorMessage = "An error has occurred during a download";
                    HasError = true;
                    MessageBox.Show("The URL doesn't refer to an image", "Image Processing Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    Debug.WriteLine(e.Error.Message, "WebClient");
                }
        }
    }
}
