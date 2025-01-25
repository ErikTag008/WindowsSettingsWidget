using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WidgetScript
{
    public static class TimeGetter
    {
        
        public static string GetTime()
        {
            DateTime currentTime = DateTime.Now;
            
            return $"{currentTime.Hour:D2} : {currentTime.Minute:D2}";
        }
    }
}
