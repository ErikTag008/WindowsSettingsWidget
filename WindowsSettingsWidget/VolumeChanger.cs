using System;
using System.Management;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.CoreAudioApi;


namespace WidgetScript
{
    public static class VolumeChanger
    {
        private static readonly MMDeviceEnumerator _enumerator = new MMDeviceEnumerator();
        private static readonly MMDevice _device = _enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Console);

        public static void SetVolume(int volume)
        {
            // Validate that the volume is between 0 and 100
            if (volume < 0 || volume > 100)
            {
                throw new ArgumentException("Volume must be between 0 and 100.");
            }

            try
            {
                float volumeLevel = volume / 100f;

                // Set the volume of the device
                _device.AudioEndpointVolume.MasterVolumeLevelScalar = volumeLevel;

                Console.WriteLine($"Volume set to {volume}%.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error setting volume: {ex.Message}");
            }
        }

        public static int GetVolume()
        {
            try
            {
                // Get the current volume level (0.0 - 1.0)
                float currentVolume = _device.AudioEndpointVolume.MasterVolumeLevelScalar;

                // Convert the volume level to a percentage (0 - 100)
                int volumePercentage = (int)(currentVolume * 100);

                Console.WriteLine($"Current Volume: {volumePercentage}%");
                return volumePercentage;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting volume: {ex.Message}");
                return 0; // Default value if there's an error
            }
        }
    }
}
