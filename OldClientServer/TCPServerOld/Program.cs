using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace TCPServerOld
{
    class Program
    {
        //public List<Socket> clientList = new List<Socket>();
        static void Main(string[] args)
        {
            //Create new socket 
            Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Console.WriteLine("Generated Socket that will accept client connections");
            server.Bind(new IPEndPoint(IPAddress.Any, 25000));
            Console.WriteLine("Bound the socket instance to any IP address that is using the 25000 port no");
            server.Listen(5);
            Console.WriteLine("start listening for connections! Clients can now connect to the server");
            //start the server listening
            //clientList = clientList.Add(server.Accept());
            Socket client = server.Accept();
            Console.WriteLine("Assign the client that we are listening in on to a new Socket instance!");
            byte[] recieveBuffer = new byte[255];
            int numRecievedBytes = 0;
            int i = 0;
            while (true)
            {
                numRecievedBytes = client.Receive(recieveBuffer);
                Console.WriteLine(numRecievedBytes);
                string recievedData = Encoding.ASCII.GetString(recieveBuffer,0, numRecievedBytes);
                Console.WriteLine($"Recieved {recievedData}");
                i++;
                Console.WriteLine(i);
                //Send data back to sende
                byte[] sendBuffer = Encoding.ASCII.GetBytes(recievedData);
                client.Send(sendBuffer);
                Array.Clear(sendBuffer, 0, sendBuffer.Length);
                Array.Clear(recieveBuffer, 0, recieveBuffer.Length);
                Console.WriteLine("Sending data back to user!");


            }
           
            Console.ReadLine();
        }
    }
}
