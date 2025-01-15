using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WidgetScript
{
    public static class FolderOpener
    {
        public static bool OpenFolder(string folderPath)
        {
            try
            {
                // Get the user's profile directory if the folder path contains a user name
                string userDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

                // Replace placeholder with actual user profile path
                if (folderPath.Contains("{user}"))
                {
                    folderPath = folderPath.Replace("{user}", userDirectory);
                    Debug.WriteLine($"User directory is {userDirectory}");
                }

                // Ensure the path exists before opening it
                if (Directory.Exists(folderPath))
                {
                    Process.Start(new ProcessStartInfo("explorer.exe", folderPath) { UseShellExecute = true });
                    return true;
                }
                else
                {
                    
                    Debug.WriteLine($"The specified folder {folderPath} does not exist.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error opening folder: {ex.Message}");
                return false;
            }
        }
    }
}
