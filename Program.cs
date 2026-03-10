using System;
using System.Diagnostics;
using System.Management;

namespace PatrolEDR
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string query = "SELECT * FROM Win32_ProcessStartTrace";
            ManagementEventWatcher watcher = new ManagementEventWatcher(query);

            watcher.EventArrived += (sender, e) =>
            {
                string processName = e.NewEvent.Properties["ProcessName"].Value.ToString();
                int pid = Convert.ToInt32(e.NewEvent.Properties["ProcessID"].Value);
                int parentId = Convert.ToInt32(e.NewEvent.Properties["ParentProcessID"].Value);

                string parentName = "unknown";
                try
                {
                    parentName = Process.GetProcessById(parentId).ProcessName;
                }
                catch { }

                Console.WriteLine($"[PROCESS SPAWN] {processName} (PID: {pid}) started by {parentName}");
            };

            watcher.Start();
            Console.WriteLine("Press Enter to exit...");
            Console.ReadLine();
        }
    }
}
