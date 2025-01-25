using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
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
        private const int TIME_UPDATE_TIME_IN_MILISECONDS = 5000;
        private const double WINDOW_ASPECT_RATIO = 6D/5.5D; // Example aspect ratio
        private readonly CancellationTokenSource _cts = new();
        public MainWindow()
        {
            InitializeComponent();
            SetInitialValues();
            _ = UpdateBatteryPercentage(_cts.Token);
            _ = UpdateTime(_cts.Token);

            this.Closing += MainWindow_Closing;
            Loaded += MainWindow_Loaded;
        }


        private void MainWindow_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            _cts.Cancel();
            _cts.Dispose();
            this.Closing -= MainWindow_Closing;
            Loaded -= MainWindow_Loaded;
        }
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Attach the HwndSource hook after the window is loaded
            var hwndSource = PresentationSource.FromVisual(this) as HwndSource;
            if (hwndSource != null)
            {
                hwndSource.AddHook(WndProc);
            }
        }

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            const int WM_SIZING = 0x0214;

            // Handle the sizing message
            if (msg == WM_SIZING)
            {
                // Adjust the window size based on the desired aspect ratio
                ResizingHandler.AdjustWindowAspect(lParam, WINDOW_ASPECT_RATIO);
            }

            return IntPtr.Zero;
        }

        private void SetInitialValues()
        {
            
            var volume = VolumeChanger.GetVolume();
            var brightness = BrightnessChanger.GetBrightness();
            var battery = BatteryPercentage.GetBatteryPercentage();
            var time = TimeGetter.GetTime();
            VolumeSlider.Value = volume;
            VolumePercentageText.Text = $"{volume}%";
            BrightnessSlider.Value = brightness;
            BrightnessPercentageText.Text = $"{brightness}%";
            BatteryPercentageText.Text = $"{battery}%";
            TimeText.Text = time;

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


        private async Task UpdateTime(CancellationToken token)
        {
            try
            {
                while (!token.IsCancellationRequested)
                {
                    var time = TimeGetter.GetTime();
                    Dispatcher.Invoke(() =>
                    {
                        TimeText.Text = time;
                    });
                    await Task.Delay(TIME_UPDATE_TIME_IN_MILISECONDS, cancellationToken: token);
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