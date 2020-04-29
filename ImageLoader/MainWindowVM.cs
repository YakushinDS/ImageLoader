using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ImageLoader
{
    public class MainWindowVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<ImageControlVM> Images {get; set;}

        private RelayComand startAllCommand;

        public RelayComand StartAll
        {
            protected set { startAllCommand = value; }

            get
            {
                return startAllCommand ??
                    (startAllCommand = new RelayComand((p) =>
                    {
                        foreach (ImageControlVM image in Images)
                        {
                            image.StartDownload();
                        }
                    }, (p) => true));
            }
        }

        public int ProgressPercentage { get { return Images.Sum((m) => m.IsBusy ? m.ProgressPercentage : 0); } }

        public int TotalValue
        {
            get
            {
                int total = Images.Sum((m) => m.IsBusy ? 100 : 0);
                return total == 0 ? 100 : total;
            }
        }

        public MainWindowVM()
        {
            Images = new ObservableCollection<ImageControlVM>();
            Images.CollectionChanged += Images_CollectionChanged;
            for (int i = 0; i < 3; i++)
                Images.Add(new ImageControlVM());
        }

        void Images_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (INotifyPropertyChanged item in e.OldItems)
                    item.PropertyChanged -= item_PropertyChanged;
            }
            if (e.NewItems != null)
            {
                foreach (INotifyPropertyChanged item in e.NewItems)
                    item.PropertyChanged += item_PropertyChanged;
            }
        }

        private void InvokePropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        void item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            InvokePropertyChanged("TotalValue");
            InvokePropertyChanged("ProgressPercentage");
        }
    }
}
