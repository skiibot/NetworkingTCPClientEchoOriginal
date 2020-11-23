using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Sockets_programme
{
    class Program
    {
        static void Main(string[] args)
        {
            Socket client = null;

            client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            
            IPAddress ipAdress = null;

            try
            {
                Console.WriteLine("Welcome to the Socket Client starter!");
                Console.WriteLine("Please type in a valid Server IP address and press enter:");

                string stringIPAddress = Console.ReadLine();

                Console.WriteLine("Please write valid port");
                string portNoString = Console.ReadLine();

                int portNo = 0;

                if (stringIPAddress == "" ||stringIPAddress == " ") stringIPAddress = "127.0.0.1";
                if (portNoString == "" || portNoString == " ") portNoString = "25000";

                if(!IPAddress.TryParse(stringIPAddress, out ipAdress))
                {
                    Console.WriteLine("Invalid IP address given.");
                    Console.ReadLine();
                    return;
                }
                if (!int.TryParse(portNoString, out portNo))
                {
                    Console.WriteLine("Invalid port number");
                    Console.ReadLine();
                    return;
                }
                if (portNo <= 0 || portNo > 65535)
                {
                    Console.WriteLine("Port number must be between 0 and 65535");
                    Console.ReadLine();
                    return;
                }

                Console.WriteLine($"{stringIPAddress}:{portNo}");

                client.Connect(ipAdress, portNo);
                Console.WriteLine("Connected to the server!");
                string inputCommand = string.Empty;

                while (true)
                {
                    inputCommand = Console.ReadLine();

                    if (inputCommand.Equals("<EXIT>"))
                    {
                        break;
                    }

                    byte[] buffSend = Encoding.ASCII.GetBytes(inputCommand);

                    client.Send(buffSend);

                    byte[] buffReceived = new byte[128];

                    int nRecieved = client.Receive(buffReceived);

                    Console.WriteLine($"Data reeived: {Encoding.ASCII.GetString(buffReceived, 0, nRecieved)}");
                    ;
                }
            }
            catch (Exception excp)
            {
                Console.WriteLine(excp.ToString());
            }
            finally
            {
                if (client != null)
                {
                    if (client.Connected)
                    {
                        client.Shutdown(SocketShutdown.Both);
                    }
                    client.Close();
                    client.Dispose();
                }
            }
            Console.WriteLine("Press a key to exit...");
            Console.ReadKey();
        }
    }
}
