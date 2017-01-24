using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Ava1.shared.core.network
{
    public class BaseServer
    {
        public delegate void ServerFailedToStart(Exception ex);
        public delegate void ServerAcceptedSocket(BaseClient socket);

        public event ServerFailedToStart OnServerFailedToStart;
        public event ServerAcceptedSocket OnServerAcceptedSocket;

        public event Action OnServerStopped;
        public event Action OnServerStarted;

        private Socket socket { get; set; }

        public string ip { get; private set; }
        public bool status { get; private set; }
        public int prt { get; private set; }

        private readonly object Object = new object();

        public BaseServer(string address, int port)
        {
            ip = address;
            prt = port;

            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public void Start()
        {
            if (!status)
            {
                try
                {
                    status = true;
                    socket.Bind(new IPEndPoint(IPAddress.Parse(ip), prt));
                    socket.Listen(100);
                    new Thread(new ThreadStart(this.AcceptThread)).Start();
                    ServerStart();
                }
                catch (Exception ex)
                {
                    ServerFailToStart(ex);
                }
            }
        }

        private void AcceptThread()
        {
            lock (Object)
            {
                try
                {
                    socket.BeginAccept(new AsyncCallback(this.AcceptCallBack), socket);
                }
                catch (Exception)
                {
                    this.AcceptThread();
                }
            }
        }

        private void AcceptCallBack(IAsyncResult ar)
        {
            lock (Object)
            {
                try
                {
                    BaseClient Socket = new BaseClient(socket.EndAccept(ar));
                    ServerAcceptSocket(Socket);
                    AcceptThread();
                }
                catch (Exception)
                {
                    AcceptThread();
                }
            }
        }

        private void ServerStart()
        {
            if (OnServerStarted != null)
                OnServerStarted();
        }

        private void ServerStop()
        {
            if (OnServerStopped != null)
                OnServerStopped();
        }

        private void ServerAcceptSocket(BaseClient socket)
        {
            if (OnServerAcceptedSocket != null)
                OnServerAcceptedSocket(socket);
        }

        private void ServerFailToStart(Exception ex)
        {
            if (OnServerFailedToStart != null)
                OnServerFailedToStart(ex);
        }
    }
}
