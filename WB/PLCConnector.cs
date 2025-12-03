using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace WB
{
    public class PLCConnector
    {
        private Socket socket;
        private bool isConnected;
        private bool isHeartbeatRunning;
        private Thread heartbeatThread;
        private const int heartbeatInterval = 5000; // Heartbeat interval in milliseconds

        public PLCConnector()
        {
            isConnected = false;
            isHeartbeatRunning = false;
        }

        public void Connect(string ipAddress, int port)
        {
            try
            {
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPAddress plcIPAddress = IPAddress.Parse(ipAddress);
                IPEndPoint plcEndPoint = new(plcIPAddress, port);
                socket.Connect(plcEndPoint);
                isConnected = true;
                Console.WriteLine("Connected to PLC");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to connect to PLC: " + ex.Message);
            }
        }

        public void Disconnect()
        {
            if (isConnected)
            {
                socket.Close();
                isConnected = false;
                Console.WriteLine("Disconnected from PLC");
            }
        }

        public void SendMessage(string message)
        {
            if (isConnected)
            {
                byte[] data = Encoding.ASCII.GetBytes(message);
                socket.Send(data);
                Console.WriteLine("Message sent to PLC: " + message);
            }
            else
            {
                Console.WriteLine("Not connected to PLC. Message not sent.");
            }
        }

        public void StartHeartbeat()
        {
            if (isConnected && !isHeartbeatRunning)
            {
                isHeartbeatRunning = true;
                heartbeatThread = new Thread(HeartbeatThread);
                heartbeatThread.Start();
                Console.WriteLine("Heartbeat started");
            }
        }

        public void StopHeartbeat()
        {
            if (isHeartbeatRunning)
            {
                isHeartbeatRunning = false;
                heartbeatThread.Join();
                Console.WriteLine("Heartbeat stopped");
            }
        }

        private void HeartbeatThread()
        {
            while (isHeartbeatRunning)
            {
                SendMessage("Heartbeat"); // Send a heartbeat message to the PLC
                Thread.Sleep(heartbeatInterval);
            }
        }
    }

}
