using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ImageLoader
{
    public class ImageDownloadProgressChangedEventArgs : RoutedEventArgs
    {
        public int ProgressPercentage { get; protected set; }

        public ImageDownloadProgressChangedEventArgs(RoutedEvent routedEvent, int progressPercentage)
            : base(routedEvent)
        {
            ProgressPercentage = progressPercentage;
        }
    }
}
