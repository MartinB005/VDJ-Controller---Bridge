using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO.Ports;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
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

        private SerialPort serialPort;
        private readonly Process vdjProcess;
        private ActionManager actionManager;
        private MainForm mainForm;

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


        
        public SerialCommunication(MainForm form)
        {

            mainForm = form;

            VirtualKeys.Init();
            VirtualKeys.Reset();

            InitActions();
        }

        private async void InitActions()
        {
            actionManager = new ActionManager(mainForm);

            actionManager.Add("L_PLAY", "deck left play_pause");
            actionManager.Add("L_CUE", "deck left cue_button");
            actionManager.Add("L_SYNC", "deck left sync");

            actionManager.Add("R_PLAY", "deck right play_pause");
            actionManager.Add("R_CUE", "deck right cue_action");
            actionManager.Add("R_SYNC", "deck right sync");

            actionManager.Add("R_EFFECT", "deck right effect_active");
            actionManager.Add("L_EFFECT", "deck left effect_active");

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

            actionManager.Add("eq2_L up", "deck left eq_mid +1%");
            actionManager.Add("eq2_L down", "deck left eq_mid -1%");

            actionManager.Add("eq2_R up", "deck right eq_mid +1%");
            actionManager.Add("eq2_R down", "deck right eq_mid -1%");

            actionManager.Add("eq3_L up", "deck left eq_low +1%");
            actionManager.Add("eq3_L down", "deck left eq_low -1%");

            actionManager.Add("eq3_R up", "deck right eq_low +1%");
            actionManager.Add("eq3_R down", "deck right eq_low -1%");

            actionManager.Add("jogwheel left F", "deck left jogwheel +0.07");
            actionManager.Add("jogwheel left B", "deck left jogwheel -0.07");

            actionManager.Add("jogwheel right F", "deck right jogwheel +0.07");
            actionManager.Add("jogwheel right B", "deck right jogwheel -0.07");

            actionManager.Add("eff_slider_L up", "deck left effect_slider 1 +1%");
            actionManager.Add("eff_slider_L down", "deck left effect_slider 1 -1%");

            actionManager.Add("eff_slider_R up", "deck right effect_slider 1 +1%");
            actionManager.Add("eff_slider_R down", "deck right effect_slider 1 -1%");

            actionManager.Add("L_PAD 1", "deck left pad 1");
            actionManager.Add("L_PAD 2", "deck left pad 2");
            actionManager.Add("L_PAD 3", "deck left pad 3");
            actionManager.Add("L_PAD 4", "deck left pad 4");

            actionManager.Add("R_PAD 1", "deck right pad 1");
            actionManager.Add("R_PAD 2", "deck right pad 2");
            actionManager.Add("R_PAD 3", "deck right pad 3");
            actionManager.Add("R_PAD 4", "deck right pad 4");


            actionManager.Add("play_pause left", "deck left play_pause");

            await Task.Delay(500);
            actionManager.Update();
        }

        public void ConnectArduino(string port)
        {
            try
            {
                serialPort = new SerialPort(port, 115200, Parity.None, 8, StopBits.One);
                serialPort.DataReceived += new SerialDataReceivedEventHandler(OnDataReceived);
                serialPort.Open();
            } catch (Exception ex)
            {
                mainForm.StatusText = String.Format("Unable to connect via {0} port", port);
                Console.WriteLine(ex.Message);
            }

            limiter = Stopwatch.StartNew();
        }

        public void DisconnectArduino()
        {
            if(serialPort != null)
            {
                serialPort.Close();
            }
        }

        internal ActionManager GetActionManager()
        {
            return actionManager;
        }

        private void OnDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (serialPort.IsOpen)
            {
                string incomingData = serialPort.ReadLine();

                if (!performing && limiter.ElapsedMilliseconds > 5 && mainForm.BridgeActive)
                {
                    limiter = Stopwatch.StartNew();

                    Console.WriteLine(incomingData);

                    SerialData serialData = null;

                    try
                    {
                        serialData = JsonConvert.DeserializeObject<SerialData>(incomingData);

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                    if (serialData != null)
                    {
                        if (actionManager.ActionExist(serialData.Header))
                        {
                            actionManager.ExecuteAction(serialData.Header);
                        }

                        else if (actionManager.SmoothActionExist(serialData.Header))
                        {
                            actionManager.ExecuteSmoothAction(serialData.Header, serialData.Value / 10);
                        }

                        else if (serialData.Header.Contains("jogwheel"))
                        {
                            actionManager.MoveJogwheel(serialData.Header, serialData.Value);
                        }
                        performing = false;
                    }
                }

            }
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
