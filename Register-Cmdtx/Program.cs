using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DCM = Microsoft.ConfigurationManagement.AdminConsole.DesiredConfigurationManagement;
using Microsoft.ConfigurationManagement.ManagementProvider;
using Microsoft.ConfigurationManagement.ManagementProvider.WqlQueryEngine;
using Microsoft.ConfigurationManagement.ApplicationManagement;
namespace Register_Cmdtx
{
    class Program
    {
        static void Main(string[] args)
        {
            ConnectionManagerBase connectionManager = new WqlConnectionManager();
            // Check whether a correct number of arguments is passed, 4 for extend, 2 for unextend
            // if not show Usage lines
            if ((args.Length != 5) && (args.Length != 4))
            {
                Console.WriteLine(args.Length);
                Console.WriteLine("Usage: Register-Cmdtx pathtocmdtx siteservername sitecode extend technologyid");
                Console.WriteLine("Usage: Register-Cmdtx technologyid siteservername sitecode unextend");
            }

            else
            {
                Console.WriteLine("Action to perform is {0}",args[3]);
                Console.WriteLine(args.Length);
                Console.WriteLine(args[3]);
                if ((args.Length == 5) && (args[3].ToLower()  == "extend"))
                { 
                    // run extend code
                    Console.WriteLine("Checking whether deployment technology has already been registered.");
                    if (DeploymentTechnology.Find(args[4]) != null)
                    {
                        // run unextend first, extending the same deployment technology or cmdtx multiple times does not automatically overwrite the older one
                        Console.WriteLine(string.Format(@"Found registered technology id {0} is already registered, unregistering it first!", args[4]));
                        connectionManager.Connect(Convert.ToString(args[1]));
                        DeploymentTypeExtender.Unextend(args[0], new DCM.ConsoleDcmConnection(connectionManager, null));
                    }
                    else
                    {
                        // Start executing extend code
                        Console.WriteLine(string.Format(@"\\{0}\root\sms\site_{1}", args[1], args[2]));
                        connectionManager.Connect(Convert.ToString(args[1]));
                        DeploymentTypeExtender.Extend(args[0], new DCM.ConsoleDcmConnection(connectionManager, null), string.Format(@"\\{0}\root\sms\site_{1}", args[1], args[2]));
                    }

                }
                
                if ((args.Length == 4) && (args[3] == "unextend"))
                {
                    // run unextend code
                    Console.WriteLine(String.Format("Unextending deployment technology {0}", args[0]));
                    connectionManager.Connect(Convert.ToString(args[1]));
                    DeploymentTypeExtender.Unextend(args[0], new DCM.ConsoleDcmConnection(connectionManager, null));
                }
                
            }
        }
    }
}
