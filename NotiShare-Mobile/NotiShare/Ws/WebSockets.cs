using System.Diagnostics;
using Websockets;

namespace NotiShare.Ws
{
    public class WebSocket
    {
        private readonly string DefaultUrl = "ws://192.168.43.89";

        private string url;

        private int port;

    
        private string userDeviceDbId;

        private string userDbId;//todo change db id

        private int deviceType;//todo change device type

        private IWebSocketConnection connection;


        public WebSocket(string url, int port, string userDeviceDbId, string userDbId, int deviceType)
        {
            this.url = url;
            this.port = port;
            this.userDeviceDbId = userDeviceDbId;
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
            connection.Open($"{DefaultUrl}:{port}/{url}?deviceId={userDeviceDbId}&userId={userDbId}&type={deviceType}");
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
            if (connection.IsOpen)
            {
                connection.Close();
            }
        }


        public bool IsConnected()
        {
            return connection.IsOpen;
        }

    }
}
