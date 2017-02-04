using System;
using Tdmts.HC05.BL;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;


namespace Tdmts.HC05.GUI
{
    public sealed partial class MainPage : Page
    {
        private BluetoothClient _client = BluetoothClient.getInstance();

        public MainPage()
        {
            this.InitializeComponent();
            _client.OnConnect += _client_OnConnect;
            _client.OnDisconnect += _client_OnDisconnect;
            _client.OnReceived += _client_OnReceived;
            _client.OnSent += _client_OnSent;
            _client.OnException += _client_OnException;
        }

        private void _client_OnConnect(object sender, string e)
        {
            txtMessage.Text += String.Format("{0}{1}", e, Environment.NewLine);
        }

        private void _client_OnDisconnect(object sender, string e)
        {
            txtMessage.Text += String.Format("{0}{1}", e, Environment.NewLine);
        }

        private void _client_OnReceived(object sender, string e)
        {
            txtMessage.Text += String.Format("{0}{1}", e, Environment.NewLine);
        }

        private void _client_OnSent(object sender, string e)
        {
            txtMessage.Text += String.Format("{0}{1}", e, Environment.NewLine);
        }

        private void _client_OnException(object sender, Exception e)
        {
            txtMessage.Text += String.Format("{0}{1}", e.Message, Environment.NewLine);
        }

        private void btnConnect_Click(object sender, RoutedEventArgs e)
        {
            _client.Connect(txtMAC.Text, txtAddress.Text);
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            _client.Disconnect();
        }

        private void btnDisconnect_Click(object sender, RoutedEventArgs e)
        {
            _client.Disconnect();
        }

        private void btnLedOn_Click(object sender, RoutedEventArgs e)
        {
            _client.Send("LED ON");
        }

        private void btnLedOff_Click(object sender, RoutedEventArgs e)
        {
            _client.Send("LED OFF");
        }
    }
}