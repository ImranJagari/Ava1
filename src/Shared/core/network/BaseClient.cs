using System;
using System.Net;
using System.Net.Sockets;

namespace Ava1.shared.core.network
{
    public class BaseClient
    {
        public delegate void ClientReceivedData(byte[] data);
        public delegate void ClientFailedToConnect(Exception ex);
        public delegate void ClientSocketConnected();
        public delegate void ClientSocketClosed();

        public event ClientSocketConnected OnClientSocketConnected;
        public event ClientSocketClosed OnClientSocketClosed;
        public event ClientReceivedData OnClientReceivedData;
        public event ClientFailedToConnect OnClientFailedToConnect;

        private Socket socket { get; set; }

        public byte[] buff { get; private set; }
        public string ip { get; private set; }
        public string prt { get; private set; }

        private readonly object Object = new object();

        public BaseClient(Socket sock)
        {
            buff = new byte[1024];

            socket = sock;

            ip = ((IPEndPoint)(socket.RemoteEndPoint)).Address.ToString();
            prt = ((IPEndPoint)(socket.RemoteEndPoint)).Port.ToString();

            BeginReceive();
        }

        public void ConnectTo(string address, int port)
        {
            try
            {
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.BeginConnect(IPAddress.Parse(address), port, new AsyncCallback(ConnectCallBack), socket);
            }
            catch (Exception ex)
            {
                ClientFailToConnect(ex);
            }
        }

        public void Close()
        {
            lock (Object)
            {
                try
                {
                    socket.Close();
                }
                catch
                {
                }
            }
        }

        public void BeginReceive()
        {
            try
            {
                socket.BeginReceive(buff, 0, buff.Length, SocketFlags.None, new AsyncCallback(ReceiveCallBack), socket);
            }
            catch
            {
            }
        }

        private void ConnectCallBack(IAsyncResult ar)
        {
            try
            {
                if (socket.Connected)
                {
                    ClientSocketConnect();
                    BeginReceive();
                }
                else
                {
                    ClientFailToConnect(new Exception("Failed"));
                }
            }
            catch (Exception)
            {
            }
        }

        private void ReceiveCallBack(IAsyncResult ar)
        {
            lock (Object)
            {
                try
                {
                    int num = socket.EndReceive(ar);
                    if (num > 0)
                    {
                        byte[] data = new byte[num];
                        for (int i = 0; i <= (num - 1); i++)
                        {
                            data[i] = buff[i];
                        }
                        ClientReceiveData(data);
                        buff = new byte[1024];
                        BeginReceive();
                    }
                    else
                    {
                        ClientSocketClose();
                    }
                }
                catch (Exception)
                {
                    ClientSocketClose();
                }
            }
        }

        public void Send(byte[] data)
        {
            lock (Object)
            {
                try
                {
                    socket.Send(data);
                }
                catch (Exception)
                {
                }
            }
        }

        private void ClientReceiveData(byte[] data)
        {
            if (OnClientReceivedData != null)
                OnClientReceivedData(data);
        }

        private void ClientSocketClose()
        {
            if (OnClientSocketClosed != null)
                OnClientSocketClosed();
        }

        private void ClientFailToConnect(Exception ex)
        {
            if (OnClientFailedToConnect != null)
                OnClientFailedToConnect(ex);
        }

        private void ClientSocketConnect()
        {
            if (OnClientSocketConnected != null)
                OnClientSocketConnected();
        }
    }
}
