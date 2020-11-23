using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;


namespace SocketServer
{
    class ChatClient
    {
        //contains a list of all clients
        public static Hashtable AllClients = new Hashtable();

        //ifnormation about the client
        private Socket _client;
        private string _clientIP;
        private string _clientNick;

        //used for sending/recieving data
        private byte[] data;
        //is the nickname being sent?
        private bool RecieveNick = true;

        public ChatClient(Socket client)
        {
            TcpClient ds;
            //get client ip
            _clientIP = client.RemoteEndPoint.ToString();
            //add to hash table
            AllClients.Add(_clientIP, this);

            //start reading data from the client in seperate thread;
            data = new byte[_client.ReceiveBufferSize];
            NetworkStream network = new NetworkStream(client);
            network.BeginRead(data, 0, _client.ReceiveBufferSize, RecieveMessage(), null);
        }

        public  AsyncCallback RecieveMessage()
        {
            throw new NotImplementedException();
        }
    }

}
