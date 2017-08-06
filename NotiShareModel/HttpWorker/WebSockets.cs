using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Websockets;

namespace NotiShareModel.HttpWorker
{
    public class WebSocket
    {
        private readonly string DefaultUrl = UrlResource.WebSocketUrl;

        private string url;

        private int port;

        private string id;

        private string deviceDbId;

        private string userDbId;

        private string deviceType;

        private IWebSocketConnection connection;


        public WebSocket(string url, int port, string id, string deviceDbId, string userDbId, string deviceType)
        {
            this.url = url;
            this.port = port;
            this.id = id;
            this.deviceDbId = deviceDbId;
            this.userDbId = userDbId;
            this.deviceType = deviceType;
        }



        public void Init()
        {
            connection = WebSocketFactory.Create();
            connection.OnOpened += ConnectionOnOpened;
            connection.OnMessage += ConnectionOnMessage;
            connection.OnError += ConnectionOnError;
            connection.OnClosed += ConnectionOnClosed;
            connection.Open($"{DefaultUrl}:{port}/{url}?id={id}&deviceId={deviceDbId}&userId={userDbId}&type={deviceType}");
        }

        private void ConnectionOnClosed()
        {
            Debug.WriteLine("Closed");
        }


        public void Send(string message)
        {
            connection.Send(message);
        }


        private void ConnectionOnError(string s)
        {
            Debug.WriteLine(s);
        }

        private void ConnectionOnMessage(string s)
        {
            
        }


        
        private void ConnectionOnOpened()
        {
            Debug.WriteLine("opened");
        }


        public void Close()
        {
            connection.Close();
        }


        public bool IsConnected()
        {
            return connection.IsOpen;
        }

    }
}
