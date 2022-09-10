using Guna.UI.WinForms;
using System;
using System.Drawing;
using System.IO.Ports;
using System.Windows.Forms;

namespace VDJ_Controller
{
    public partial class MainForm : Form
    {
        private SerialCommunication communication;
        private bool connected;
        private bool controlActive;

        public MainForm()
        {
            InitializeComponent();
            UpdateSerialPorts();
            portSelect.Items.Add("--select port--");
            portSelect.SelectedIndex = 0;

            communication = new SerialCommunication(this);
        }

        private void connect_Click(object sender, EventArgs e)
        {
            GunaButton button = (GunaButton)sender;

            string port = portSelect.SelectedItem.ToString();
            if(connected)
            {
                button.Text = "Connect";
                connected = false;
                communication.DisconnectArduino();
            }
            else if(!port.Equals("--select port--"))
            {
                communication.ConnectArduino(port);
                button.Text = "Disconnect";
                connected = true;
            }
            else
            {
                StatusText = "Port is not selected";
                connected = false;
            }
          
        }

        private void UpdateSerialPorts()
        {
            portSelect.Items.Clear();
            string[] ports = SerialPort.GetPortNames();
            portSelect.Items.Add("--select port--");

            foreach (string port in ports)
            {
                portSelect.Items.Add(port);
            }
        }



        private void portSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }

        private void portSelect_Click(object sender, EventArgs e)
        {
            UpdateSerialPorts();
        }

        public string StatusText
        {
            get
            {
                return statusView.Text;
            }
            set
            {
                statusView.Text = value;
            }
        }

        public bool BridgeActive
        {
            get
            {
                return bridgeActive.Checked;
            }
            set
            {
                bridgeActive.Checked = value;
            }
        }

        public bool ControlActive
        {
            get
            {
                return controlActive;
            }
            set
            {
                controlActive = value;
                control.BackColor = value ? Color.OrangeRed : Color.Black;
            }
        }

        private void bridgeActive_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
