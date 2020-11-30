using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SocketClient2
{
    class Program
    {
        private static Socket _client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private static string hostIP = "127.0.0.1";
        private static int portNo = 25000;
        static void Main(string[] args)
        {
            Console.Title = "Client";
            LoopConnect();
            RecieveLoopAsync();
            SendLoop();
            Console.ReadLine();
        }

        private static async Task RecieveLoopAsync()
        {
            while (true)
            {   
                byte[] recievedBuffer = new byte[1024];
                int rec = await Task.Run(()=>_client.Receive(recievedBuffer));
                byte[] data = new byte[rec];
                Array.Copy(recievedBuffer, data, rec);
                Console.WriteLine($"{Encoding.ASCII.GetString(data)}");
            }
        }

        private static void LoopConnect()
        {
            int attempts = 0;

            while (!_client.Connected)
            {
                try
                {
                    _client.Connect(IPAddress.Loopback, portNo);
                }
                catch (SocketException)
                {
                    Console.Clear();
                    Console.WriteLine($"Connection attempts:{attempts.ToString()} ");
                }
            }
            Console.Clear();
            Console.WriteLine("Connected");
            UsernameRequest();
        }  

        private static void UsernameRequest()
        {
            Console.Write("Enter a username: ");
            string req = Console.ReadLine();
            byte[] buffer = Encoding.ASCII.GetBytes(req);
            _client.Send(buffer);
            Console.Title = $"Client: {req}";
        }
        private static void SendLoop()
    {
        while (true)
        {
            //Console.Write("Enter a request: ");
            string req = Console.ReadLine();
            byte[] buffer = Encoding.ASCII.GetBytes(req);
            _client.Send(buffer);

        }
    }
    }
  
}
