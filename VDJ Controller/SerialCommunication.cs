using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO.Ports;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace VDJ_Controller
{
    public class SerialData
    {
        public String Header { get; set; }
        public int Value { get; set; }
    }
    public partial class SerialCommunication
    {

        private readonly SerialPort serialPort;
        private readonly Process vdjProcess;
        private ActionManager actionManager;

        private int currentVolume = 1000;
        private int crossfaderValue = 500;

        private Stopwatch limiter;
        private bool performing;

        [DllImport("user32.dll")]
        static extern bool PostMessage(IntPtr hWnd, UInt32 Msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hwnd, int msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, uint dwExtraInfo);


        
        public SerialCommunication()
        {

            actionManager = new ActionManager();
            actionManager.Add("lvl_L up", "deck left level +1%");
            actionManager.Add("lvl_L down", "deck left level -1%");

            actionManager.Add("lvl_R up", "deck right level +1%");
            actionManager.Add("lvl_R down", "deck right level -1%");

            actionManager.Add("cfdr up", "crossfader +1%");
            actionManager.Add("cfdr down", "crossfader -1%");

            actionManager.Add("eq1_L up", "deck left eq_high +1%");
            actionManager.Add("eq1_L down", "deck left eq_high -1%");

            actionManager.Add("eq1_R up", "deck right eq_high +1%");
            actionManager.Add("eq1_R down", "deck right eq_high -1%");

            actionManager.Add("play_pause left", "deck left play_pause");
            actionManager.Update();

            serialPort = new SerialPort("COM4", 115200, Parity.None, 8, StopBits.One);
            serialPort.DataReceived += new SerialDataReceivedEventHandler(OnDataReceived);
            serialPort.Open();

            limiter = Stopwatch.StartNew();

        }

        internal ActionManager GetActionManager()
        {
            return actionManager;
        }

        private void OnDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            
                string incomingData = serialPort.ReadLine();
            
            if (!performing && limiter.ElapsedMilliseconds > 5)
            {
                Console.WriteLine("start");
                limiter = Stopwatch.StartNew();

                Console.WriteLine(incomingData);
                SerialData serialData = JsonConvert.DeserializeObject<SerialData>(incomingData);
                // keybd_event(VirtualKeys.CTRL, 0, 0, 0);

                if (actionManager.ActionExist(serialData.Header))
                {
                    actionManager.ExecuteAction(serialData.Header);
                }

                else if (actionManager.SmoothActionExist(serialData.Header))
                {
                    actionManager.ExecuteSmoothAction(serialData.Header, serialData.Value / 10);
                }
                performing = false;
            }

            //keybd_event(VirtualKeys.CTRL, 0, VirtualKeys.WM_KEYUP, 0);

        }


        private void MoveJogwheel(int direction)
        {
            PostMessage(vdjProcess.MainWindowHandle, VirtualKeys.WM_KEYDOWN, direction == 1 ? VirtualKeys.P : VirtualKeys.Q, 0);
        }

        private async void MoveToValue(int value, int forwardKey, int backwardKey)
        {
            keybd_event(VirtualKeys.CTRL, 0, 0, 0);
            for (int i = 0; i < value - crossfaderValue; i++)
            {
                SendKey(VirtualKeys.D);
            }
            for (int i = 0; i < crossfaderValue - value; i++)
            {
                SendKey(VirtualKeys.A);
            }
            keybd_event(VirtualKeys.CTRL, 0, VirtualKeys.WM_KEYUP, 0);
            crossfaderValue = value;
        }

        private void Volume_toValue(int value)
        {
            Process[] processes = Process.GetProcessesByName("virtualdj");
            for (int i = 0; i < value - currentVolume; i++)
            {
                PostMessage(processes[0].MainWindowHandle, VirtualKeys.WM_KEYDOWN, VirtualKeys.P, 0);
            }
            for (int i = 0; i < currentVolume - value; i++)
            {
                PostMessage(processes[0].MainWindowHandle, VirtualKeys.WM_KEYDOWN, VirtualKeys.Q, 0);
            }
            currentVolume = value;
        }

        private void SendKey(byte key)
        {
            PostMessage(vdjProcess.MainWindowHandle, VirtualKeys.WM_KEYDOWN, key, 0);    
        }
    }
}
