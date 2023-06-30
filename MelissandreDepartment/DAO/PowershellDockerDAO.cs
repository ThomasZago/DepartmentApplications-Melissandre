using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Text;
using System.Threading.Tasks;


namespace MelissandreDepartment.DAO
{
    public class PowershellDockerDAO
    {
        
        private static PowershellDockerDAO _instance;
        private static readonly object _lockObject = new object();

        public static PowershellDockerDAO Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lockObject)
                    {
                        if (_instance == null)
                        {
                            _instance = new PowershellDockerDAO();
                        }
                    }
                }
                return _instance;
            }
        }

        private PowershellDockerDAO()
        {
            
        }

        public string GetContainerLog(string containerName)
        {
            string commandString = $"docker logs {containerName}";

            // Create the PowerShell runspace
            Runspace runspace = RunspaceFactory.CreateRunspace();
            runspace.Open();

            // Create a PowerShell instance within the runspace
            PowerShell powerShell = PowerShell.Create();
            powerShell.Runspace = runspace;

            // Add the command to the PowerShell instance
            powerShell.AddScript(commandString);

            // Execute the command and retrieve the output
            var output = powerShell.Invoke();
            string result = string.Join(Environment.NewLine, output.Select(o => o.ToString()));

            // Close the runspace and dispose of resources
            runspace.Close();
            runspace.Dispose();
            powerShell.Dispose();

            return result;
        }
    }
}
