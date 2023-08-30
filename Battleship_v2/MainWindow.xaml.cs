using Battleship_v2.Services;
using System;
using System.ComponentModel;
using System.Windows;

namespace Battleship_v2
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

        protected override void OnClosing(CancelEventArgs e)
        {
            NetworkService.Instance.Close();

            base.OnClosing(e);
        }
    }
}
