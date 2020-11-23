using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TCPClientOld
{
    class Program
    {

        static void Main(string[] args)
        {

            //Socket client = null;
            string hostIP = "127.0.0.1";
            int portNo = 25000;

            Socket client =  new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Console.WriteLine("Generated socket instance");
            IPEndPoint endPoint = new IPEndPoint(Dns.GetHostAddresses(hostIP)[0], portNo);
            Console.WriteLine("Generated IPEndPoint instance that points to host IP socket");
            client.Connect(endPoint);
            while (true)
            {            
                Console.WriteLine("Connected to the server endpoint");
                Console.WriteLine("What do you want to say?: ");

                //start with the buffer 
                string input = Console.ReadLine();
                byte[] inputAsBytes = Encoding.ASCII.GetBytes(input);
                Console.WriteLine("Converted what was written into bytes!");
                client.Send(inputAsBytes, inputAsBytes.Length, SocketFlags.None);
                Console.WriteLine($"Sent {inputAsBytes.Length} to the server!");
                byte[] bytesFromServer = new byte[128];
                int recievedInt = client.Receive(bytesFromServer);
                string output = Encoding.ASCII.GetString(bytesFromServer, 0 ,recievedInt);
                Console.WriteLine("Recieving data from server!");
                Console.WriteLine($"Sever echoed back {output}");

            }


            Console.ReadLine();
        }
    }
}
