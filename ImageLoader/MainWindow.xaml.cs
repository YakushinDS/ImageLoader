using System.Collections.Generic;
using System.Windows;
using System.Linq;
using System;

namespace ImageLoader
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowVM();;
        }
    }
}
