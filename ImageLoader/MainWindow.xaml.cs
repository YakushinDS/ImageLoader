using System.Collections.Generic;
using System.Windows;
using System.Linq;

namespace ImageLoader
{
    public partial class MainWindow : Window
    {
        private Dictionary<object, int> progress = new Dictionary<object, int>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ImageControl_ImageDownloadProgressChanged(object sender, ImageDownloadProgressChangedEventArgs e)
        {
            if (progress.ContainsKey(sender))
                progress[sender] = e.ProgressPercentage;
            else
                progress.Add(sender, e.ProgressPercentage);

            UpdateProgressBar();
        }

        private void ImageControl_ImageDownloadCompleted(object sender, RoutedEventArgs e)
        {
            if (progress.ContainsKey(sender))
                progress.Remove(sender);

            UpdateProgressBar();
        }

        private void UpdateProgressBar()
        {
            DownloadProgressBar.Maximum = progress.Count == 0 ? 100 : 100 * progress.Count;
            DownloadProgressBar.Value = progress.Sum((p) => p.Value);
        }

        private void StartAllButton_Click(object sender, RoutedEventArgs e)
        {
            ImageControl1.StartDownload();
            ImageControl2.StartDownload();
            ImageControl3.StartDownload();
        }
    }
}
