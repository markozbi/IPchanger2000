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
            ProgramStep myProgramStep = new ProgramStep();

            bool programActive = true;
            List<ProgramStep> nextSteps = new List<ProgramStep>();
            while (programActive)
            {
                switch (myProgramStep)
                {
                    case ProgramStep.init:
                        nextSteps.Clear();

                        Console.WriteLine("Welcome to IPchanger2000!");

                        nextSteps.Add(ProgramStep.chooseNetworkAdapter);
                        nextSteps.Add(ProgramStep.End);
                        break;
                    case ProgramStep.chooseNetworkAdapter:
                        nextSteps.Clear();

                        Console.WriteLine("Choosing the network adapter...");
                        WMIQuery_NetworkAdapter();
                        nextSteps.Add(ProgramStep.selectConfiguration);
                        nextSteps.Add(ProgramStep.End);
                        break;
                    case ProgramStep.selectConfiguration:
                        nextSteps.Clear();

                        Console.WriteLine("Selecting desired configuration...");

                        nextSteps.Add(ProgramStep.applyConfiguration);
                        nextSteps.Add(ProgramStep.End);
                        break;
                    case ProgramStep.applyConfiguration:
                        nextSteps.Clear();

                        Console.WriteLine("Applying choosen configuration...");

                        nextSteps.Add(ProgramStep.End);
                        break;
                    case ProgramStep.End:
                        Console.WriteLine("Goodbye");
                        programActive = false;
                        break;
                    default:
                        break;
                }
                myProgramStep = CaseSelector(nextSteps,myProgramStep);
            }

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

        public enum ProgramStep
        {
            init,
            chooseNetworkAdapter,
            selectConfiguration,
            applyConfiguration,
            End,
        }

        public static  ProgramStep CaseSelector(List<ProgramStep> stepChoices, ProgramStep currentStep)
        {
            if (currentStep != ProgramStep.End)
            {
                Console.WriteLine();
                Console.WriteLine("What is the next step?");
                foreach (int step in stepChoices)
                {
                    Console.WriteLine("[{0}] - {1}", step, Enum.GetName(typeof(ProgramStep), step));
                }
                Console.Write ("Your choice: ");
                int  i = Int32.Parse(Console.ReadLine());
                return (ProgramStep) i;
            }
            return ProgramStep.End;
        }
    }
}
