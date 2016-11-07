using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.MsmqIntegration;

namespace Microsoft.ServiceModel.Samples
{
    [ServiceContract(Namespace = "http://Microsoft.ServiceModel.Samples")]
    public interface ICalculator
    {
        [OperationContract]
        double Add(double n1, double n2);
        [OperationContract]
        double Subtract(double n1, double n2);
        [OperationContract]
        double Multiply(double n1, double n2);
        [OperationContract]
        double Divide(double n1, double n2);
        [OperationContract]
        double Modulo(double n1, double n2);
        [OperationContract]
        double Power(double n1, double n2);
    }

    public class CalculatorService : ICalculator
    {
        public double Add(double n1, double n2)
        {
            double result = n1 + n2;
            Console.WriteLine("Received Add({0},{1})", n1, n2);
            // Code added to write output to the console window.
            Console.WriteLine("Return: {0}", result);
            return result;
        }

        public double Subtract(double n1, double n2)
        {
            double result = n1 - n2;
            Console.WriteLine("Received Subtract({0},{1})", n1, n2);
            Console.WriteLine("Return: {0}", result);
            return result;
        }

        public double Multiply(double n1, double n2)
        {
            double result = n1 * n2;
            Console.WriteLine("Received Multiply({0},{1})", n1, n2);
            Console.WriteLine("Return: {0}", result);
            return result;
        }

        public double Divide(double n1, double n2)
        {
            double result = n1 / n2;
            Console.WriteLine("Received Divide({0},{1})", n1, n2);
            Console.WriteLine("Return: {0}", result);
            return result;
        }

        public double Modulo(double n1, double n2)
        {
            double result = n1 % n2;
            Console.WriteLine("Received Modulo({0},{1})", n1, n2);
            Console.WriteLine("Return: {0}", result);
            return result;
        }

        public double Power(double n1, double n2)
        {
            double result = Math.Pow(n1, n2);
            Console.WriteLine("Received Power({0},{1})", n1, n2);
            Console.WriteLine("Return: {0}", result);
            return result;
        }
    }

    class Service
    {
        static void Main(string[] args)
        {
            // Step 1: Create a URI to serve as the base address
            Uri baseAddress = new Uri("net.pipe://localhost/ServiceModelSamples/Service");

            // Step 2: Create service host
            ServiceHost selfHost = new ServiceHost(typeof(CalculatorService), baseAddress);

            try
            {
                // Step 3: Add an endpoint to the service
                selfHost.AddServiceEndpoint(typeof(ICalculator),
                    new NetNamedPipeBinding(),
                    "CalculatorService");

                // Step 4: Enable metadata exchange
                //ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
                //smb.ExternalMetadataLocation = new Uri("http://localhost:8000/ServiceModelSamples/Service");
                //selfHost.Description.Behaviors.Add(smb);

                // Step 5: Start (then stop) the service
                selfHost.Open();
                Console.WriteLine("The service is ready.");
                Console.WriteLine("Press <ENTER> to terminate the service\n");
                Console.ReadLine();

                // Shutdown the service
                selfHost.Close();
            }
            catch (CommunicationException ce)
            {
                Console.WriteLine("An exception was thrown: {0}", ce);
                selfHost.Abort();
            }
        }
    }
}
