using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace SocketClient2
{
    class Program
    {
        public static Dictionary<string,Socket> _clientList = new Dictionary<string, Socket>();
        public static int _clientNumber = 0;
        public const int BYTENUM = 1024;
        public static byte[] _buffer = new byte[BYTENUM];
        public static int _numRecievedBuff = 0;
        public static int _port;
        public static Socket  _server = null;
        //public static Socket _currentClient = null;
        static void Main(string[] args)
        {
            Console.Title = "Server";
            ServerInitialiser(25000);
            Console.ReadLine();
        }

        public static void ServerInitialiser(int port)
        {
            _port = port;
            _server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Console.WriteLine("Generated Socket that will accept client connections");
            _server.Bind(new IPEndPoint(IPAddress.Any, _port));
            Console.WriteLine("Bound the socket instance to any IP address that is using the 25000 port no");
            //Console.WriteLine($"We are here!1");
            _server.Listen(5);
            _server.BeginAccept(new AsyncCallback(AcceptCallback), null);
            //thread that adds any clients to the clientlist

        }

        private static void AcceptCallback(IAsyncResult ar)
        {
            Socket _currentClient = _server.EndAccept(ar);
            Console.WriteLine("Client has connected!");
            _currentClient.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(InitialRecieveNameCallback), _currentClient);
            _server.BeginAccept(new AsyncCallback(AcceptCallback), null);
        }

        private static void ReceiveCallBack(IAsyncResult ar)
        {
            string userName = (string)ar.AsyncState;
            Socket _currentClient = _clientList[userName];
            try
            {
                int received = _currentClient.EndReceive(ar);
                byte[] dataBuf = new byte[received];
                Array.Copy(_buffer, dataBuf, received);

                string text = Encoding.ASCII.GetString(dataBuf);
                Console.WriteLine($"Text received:{text}");

                if (text.ToLower() == "get time")
                {
                    SendText(DateTime.Now.ToLongTimeString());
                }
                else
                {
                    BroadcastAll(userName, text);
                }
                _currentClient.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallBack), userName);
            }catch(SocketException e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine($"{userName} has left the chat!");
                _currentClient.Close();
                _clientList.Remove(userName);
                BroadcastMessage($"{userName} has left the chat!");
                return;
            }

        }

        private static void InitialRecieveNameCallback(IAsyncResult ar)
        {
            Socket _currentClient = (Socket)ar.AsyncState;
            int received = _currentClient.EndReceive(ar);
            byte[] dataBuf = new byte[received];
            Array.Copy(_buffer, dataBuf, received);

            string userName = Encoding.ASCII.GetString(dataBuf);
            Console.WriteLine($"New Client Name: {userName}");
            _clientList.Add(userName, _currentClient);
            BroadcastMessage($"{userName} has entered the chat!");
            _currentClient.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallBack), userName);
        }

        private static void SendText(string text)
        {
            Socket currentClient = null;
            byte[] data = Encoding.ASCII.GetBytes(text);
            currentClient.Send(data);
        }

        private static void BroadcastAll(string username, string text)
        {
            foreach(var client in _clientList)
            {
                client.Value.Send(Encoding.ASCII.GetBytes($"[{DateTime.Now.ToLongTimeString()}] {username}: {text}"));
                //Console.WriteLine("Sent off!");
            }
        }

        private static void BroadcastMessage(string text)
        {
            foreach (var client in _clientList)
            {
                client.Value.Send(Encoding.ASCII.GetBytes($"{text}"));
                //Console.WriteLine("Sent off!");
            }
        }

    }
}