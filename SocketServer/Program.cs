using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SocketServer
{
    class Program
    {
        static void Main(string[] args)
        {
            //Create an object of socket class
            Socket listenerSocket = new Socket(AddressFamily.InterNetwork,SocketType.Stream, ProtocolType.Tcp);
            //Create an object of an IP Address.socket listning on any ip address.
            IPAddress ipaddr = IPAddress.Any;
            // Define IP ENDP POINT
            IPEndPoint ipep = new IPEndPoint(ipaddr, 25000);
            //Bind socket to ip end point.
            try
            {
                listenerSocket.Bind(ipep);
                // Call listen method on the Listener socket object, pass the a
                //number to signify how many
                //clients can wait for a co
                //nnection while the system is busy
                //handling another connection
            listenerSocket.Listen(5);
                // Call Accept method on the listener socket.
                //Accept is a synchrouous method (Blocking method) will not move
                //forward until operation completed--Very Bad Method
            Socket client = listenerSocket.Accept();
                //Print out client ip end point
                Console.WriteLine("Client connected: " + client.ToString() + "-Ip End Point: " + client.RemoteEndPoint.ToString());
                // define a buffer as byte array of 128 byte length.
                byte[] buff = new byte[128];
                // define number of received bytes as an int
                int numberOfReceivedBytes = 0;
                while (true)
                {
                    //ChatClient user = new ChatClient(
                    numberOfReceivedBytes = client.Receive(buff);
                    Console.WriteLine("Number of received bytes: " + numberOfReceivedBytes);
                    //convert from byte array to ascii, using
                    //Encoding.ASCII.GetString
                    string receivedText = Encoding.ASCII.GetString(buff, 0,numberOfReceivedBytes);
                    //print out text
                    Console.WriteLine("Data sent by client is: " + receivedText);
                    //Send data back to sender
                    client.Send(buff);
                    if (receivedText == "x")
                    {
                        break;
                    }
                    Array.Clear(buff, 0, buff.Length);
                    numberOfReceivedBytes = 0;
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.ToString());
            }
        }
    }
}