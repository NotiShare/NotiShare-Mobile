using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Websockets;

namespace NotiShareModel.HttpWorker
{
    public class WebSocket
    {
        private string url;

        private int port;

        private string id;

        private IWebSocketConnection connection;

        public WebSocket(string url, int port, string id)
        {
            this.url = url;
            this.port = port;
            this.id = id;
        }



        public void Init()
        {
            connection = WebSocketFactory.Create();
            connection.OnOpened += ConnectionOnOnOpened;
            connection.OnMessage += ConnectionOnOnMessage;
            connection.OnError += ConnectionOnOnError;
            connection.Open($"ws://192.168.100.6:{port}/{url}");
        }


        public void Send(string message)
        {
            connection.Send(message);
        }


        private void ConnectionOnOnError(string s)
        {
            
        }

        private void ConnectionOnOnMessage(string s)
        {
            
        }

        private void ConnectionOnOnOpened()
        {
            connection.Send(id);
        }


        

    }
}
