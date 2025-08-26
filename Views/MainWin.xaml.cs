using FinalAttemptWPF.ViewModels;
using System.Windows;
using System.Windows.Media;

namespace FinalAttemptWPF.Views
{

    public partial class MainWin : Window
    {
        private DevicesViewModel vm = new DevicesViewModel();
        private enum DevicePower
        {
            On ,
            Off 
        }
        public MainWin()
        {
            InitializeComponent();
            DataContext = vm;
            Butt1.Content = "Start";
            Butt2.Content = "Start";

        }



        // I know this is not the best way to do this but I had problems binding the button content to the VM
        private void Butt1_Click(object sender, RoutedEventArgs e)
        {
            if (vm.Power1)
            {
                Butt1.Content = "Start";
                PowerLight1.Fill = Brushes.Red;

            }
            else
            {
                Butt1.Content = "Stop";
                PowerLight1.Fill = Brushes.Green;

            }


        }

        private void Butt2_Click(object sender, RoutedEventArgs e)
        {
            if (vm.Power2)
            {
                Butt2.Content = "Start";
                PowerLight2.Fill = Brushes.Red;

            }
            else
            {
                Butt2.Content = "Stop";
                PowerLight2.Fill = Brushes.Green;

            }
        }

        private void SettingButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void dbVIew_Click(object sender, RoutedEventArgs e)
        {
            DisplayDB displayDB = new DisplayDB();
            displayDB.Show();
        }
    }
}
