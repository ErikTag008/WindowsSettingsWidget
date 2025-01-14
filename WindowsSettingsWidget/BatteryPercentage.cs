using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace WidgetScript
{
    public static class BatteryPercentage
    {
        

        public static int GetBatteryPercentage()
        {

            using var managmentClass = new ManagementClass("Win32_Battery")
            {
                Scope = new ManagementScope(@"\\.\root\cimv2")
            };
            using var instances = managmentClass.GetInstances();
            foreach (ManagementObject instance in instances)
            {
                var batteryPercentage = instance.GetPropertyValue("EstimatedChargeRemaining");
                return Convert.ToInt32(batteryPercentage);
            }
            return 0;
        }
    }
}
