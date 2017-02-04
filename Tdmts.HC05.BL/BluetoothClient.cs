using System;
using Windows.Networking;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;

namespace Tdmts.HC05.BL
{

    public class BluetoothClient
    {
        private static BluetoothClient _instance;
        private StreamSocket _socket;

        public event EventHandler<string> OnConnect;
        public event EventHandler<string> OnSent;
        public event EventHandler<string> OnReceived;
        public event EventHandler<string> OnDisconnect;
        public event EventHandler<Exception> OnException;

        public static BluetoothClient getInstance()
        {
            if (_instance == null)
            {
                _instance = new BluetoothClient();
            }
            return _instance;
        }

        private BluetoothClient()
        {
            _socket = new StreamSocket();
        }

        public async void Connect(string mac, string address)
        {
            try
            {
                HostName host = new HostName(mac);
                await _socket.ConnectAsync(host, address, SocketProtectionLevel.BluetoothEncryptionWithAuthentication);
                OnConnect(this, String.Format("Connected to MAC: {0} on address: {1}", mac, address));
                Receive();
            }
            catch (Exception x)
            {
                OnException(this, x);
            }
        }

        public void Disconnect()
        {
            try
            {
                _socket.Dispose();
                OnDisconnect(this, "Disconnected from host");
            }
            catch (Exception x)
            {
                OnException(this, x);
            }
        }

        public async void Send(string message)
        {
            try
            {
                DataWriter _writer = new DataWriter(_socket.OutputStream);
                _writer.WriteString(message);
                await _writer.StoreAsync();
                _writer.DetachStream();
                OnSent(this, message);
            }
            catch (Exception x)
            {
                OnException(this, x);
            }
        }

        public async void Receive()
        {
            try
            {
                DataReader _reader = new DataReader(_socket.InputStream);
                uint header = await _reader.LoadAsync(4);

                if (header == 0)
                {
                    return;
                }

                int len = _reader.ReadInt32();
                uint lenBytes = await _reader.LoadAsync((uint)len);

                string message = _reader.ReadString(lenBytes);

                OnReceived(_socket, message);

                Receive();
            }
            catch (Exception x)
            {
                OnException(this, x);
            }
        }
    }
}
