using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Drawing;
using System.Windows.Forms;

namespace Client
{
    class Client : Form
    {
        static void Main(string[] args)
        {
            NetNamedPipeBinding binding = new NetNamedPipeBinding();

            EndpointAddress address = new EndpointAddress(
                "net.pipe://localhost/ServiceModelSamples/Service/CalculatorService");

            // Step 1: Create instance of the WCF Client.
            CalculatorClient client = new CalculatorClient(binding, address);

            try
            {
                Application.Run(new CalculatorForm(client));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                Application.Exit();
                client.Close();
            }
        }
    }
}
