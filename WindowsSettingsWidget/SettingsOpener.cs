using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace WidgetScript
{
    public static class SettingsOpener
    {
        public static void OpenSystemSettings()
        {
            try
            {
                // Opening the system settings (Windows 10/11)
                Process.Start(new ProcessStartInfo("ms-settings:") { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error opening system settings: {ex.Message}");
            }
        }
    }
}
