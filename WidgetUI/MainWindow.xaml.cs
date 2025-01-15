using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WidgetScript;


namespace WidgetUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int BATTERY_UPDATE_TIME_IN_MILISECONDS = 15000;
        private readonly CancellationTokenSource _cts = new();
        public MainWindow()
        {
            InitializeComponent();
            SetInitialValues();
            Task.Run(() => UpdateBatteryPercentage(_cts.Token));

            this.Closing += MainWindow_Closing;
        }

        private void MainWindow_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            _cts.Cancel();
            _cts.Dispose();
            this.Closing -= MainWindow_Closing;
        }

        private void SetInitialValues()
        {
            
            var volume = VolumeChanger.GetVolume();
            var brightness = BrightnessChanger.GetBrightness();
            var battery = BatteryPercentage.GetBatteryPercentage();
            VolumeSlider.Value = volume;
            VolumePercentageText.Text = $"{volume}%";
            BrightnessSlider.Value = brightness;
            BrightnessPercentageText.Text = $"{brightness}%";
            BatteryPercentageText.Text = $"{battery}%";
        }

        private async Task UpdateBatteryPercentage(CancellationToken token)
        {
            try
            {
                while (!token.IsCancellationRequested)
                {
                    var battery = BatteryPercentage.GetBatteryPercentage();
                    Dispatcher.Invoke(() =>
                    {
                        BatteryPercentageText.Text = $"{battery}%";
                    });
                    await Task.Delay(BATTERY_UPDATE_TIME_IN_MILISECONDS, cancellationToken: token);
                }
            }
            catch (OperationCanceledException) { }
            

        }

        private void VolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int percentage = (int)e.NewValue;
            VolumeChanger.SetVolume(percentage);
            VolumePercentageText.Text = $"{percentage}%";
        }

        private void BrightnessSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int percentage = (int)e.NewValue;
            BrightnessChanger.SetBrightness(percentage);
            BrightnessPercentageText.Text = $"{percentage}%";
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            SettingsOpener.OpenSystemSettings();
        }

        private void ControlPanelButton_Click(object sender, RoutedEventArgs e)
        {
            ControlPanelOpener.OpenControlPanel();
        }

        private void AlwaysOnTopMenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Topmost = !this.Topmost;

            // Update the checked state of the menu item based on Topmost property
            if (sender is MenuItem menuItem)
            {
                menuItem.IsChecked = this.Topmost;
            }
        }

        private void AppDataButton_Click(object sender, RoutedEventArgs e)
        {
            FolderOpener.OpenFolder("{user}\\AppData");
        }

        private void DownloadsButton_Click(object sender, RoutedEventArgs e)
        {
            if(FolderOpener.OpenFolder("{user}\\Downloads") == false)
            {
                FolderOpener.OpenFolder("{user}\\Загрузки");
            }
        }
    }
}