using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WidgetScript
{
    public static class ResizingHandler
    {
        

        public static void AdjustWindowAspect(IntPtr lParam, double aspectRatio)
        {
            // Retrieve the RECT structure from lParam
            var rect = System.Runtime.InteropServices.Marshal.PtrToStructure<RECT>(lParam);

            // Calculate the new width and height while maintaining the aspect ratio
            double width = rect.Right - rect.Left;
            double height = rect.Bottom - rect.Top;

            if (width / height > aspectRatio)
            {
                // Adjust width based on height
                width = height * aspectRatio;
                rect.Right = rect.Left + (int)width;
            }
            else
            {
                // Adjust height based on width
                height = width / aspectRatio;
                rect.Bottom = rect.Top + (int)height;
            }

            // Update the RECT structure
            System.Runtime.InteropServices.Marshal.StructureToPtr(rect, lParam, true);
        }

        // Define the RECT structure used in the WndProc
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }
    }
}
