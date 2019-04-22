using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Management;

namespace IPchanger2000
{
    class Program
    {
        static void Main(string[] args)
        {
            programStep myProgramStep = new programStep();
            //DisplayDnsConfiguration
            //WMIQuery_NetworkAdapterConfiguration();
            //WMIQuery_NetworkAdapter;
            bool programActive = true;
            while (programActive)
            {
                switch (myProgramStep)
                {
                    case programStep.init:
                        Console.WriteLine("Welcome to IPchanger2000!");
                        break;
                    case programStep.chooseNetworkAdapter:
                        break;
                    case programStep.selectConfiguration:
                        break;
                    case programStep.applyConfiguration:
                        break;
                    default:
                        break;
                }
            }
            
            Console.ReadLine();

        }

        public static void WMIQuery_NetworkAdapterConfiguration()
        {
            SelectQuery query = new SelectQuery("SELECT * FROM Win32_NetworkAdapterConfiguration"); // WHERE ipEnabled = 1");
        ManagementObjectSearcher moSearch = new ManagementObjectSearcher(query);
        //ManagementObjectCollection moCollection = moSearch.Get();// Every record in this collection is a network interface
        
        foreach (ManagementObject objMO in moSearch.Get())
            {
                Console.WriteLine("Caption: {0}", objMO["Caption"]);
                Console.WriteLine("IpAddress: {0}", (string[])objMO["IPAddress"]);
            }
        }

        public static void WMIQuery_NetworkAdapter()
        {
            SelectQuery query = new SelectQuery("SELECT * FROM Win32_NetworkAdapter WHERE PhysicalAdapter = 1");
            ManagementObjectSearcher moSearch = new ManagementObjectSearcher(query);
            foreach (ManagementObject objMO in moSearch.Get())
            {
                Console.WriteLine("Caption: {0}" + "\n" + 
                    "Description: {1}\n",objMO["Caption"], objMO["Description"]);
            }
        }
        
        public static void DisplayDnsConfiguration()
        {
            NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface adapter in adapters)
            {
                IPInterfaceProperties properties = adapter.GetIPProperties();
                Console.WriteLine(adapter.Description);
                Console.WriteLine("  DNS suffix .............................. : {0}",
                    properties.DnsSuffix);
                Console.WriteLine("  DNS enabled ............................. : {0}",
                    properties.IsDnsEnabled);
                Console.WriteLine("  Dynamically configured DNS .............. : {0}",
                    properties.IsDynamicDnsEnabled);
            }
        }

        enum programStep
        {
            init,
            chooseNetworkAdapter,
            selectConfiguration,
            applyConfiguration,
        }
    }
}
