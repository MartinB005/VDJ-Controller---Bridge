using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Windows.Input;
using System.Threading;

namespace VDJ_Controller
{
    internal class ActionManager
    {

        [DllImport("user32.dll")]
        static extern bool PostMessage(IntPtr hWnd, UInt32 Msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hwnd, int msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, uint dwExtraInfo);

        private readonly Dictionary<string, byte> actionKeys = new Dictionary<string, byte>();
        private readonly Dictionary<string, int> lastValues = new Dictionary<string, int>();

        private List<string> actions = new List<string>();
        private Process vdjProcess;

        private MainForm mainForm;

        public ActionManager(MainForm form)
        {
            mainForm = form;

            FindVDJProcess();
        }

        private void FindVDJProcess()
        {
            Process[] processes = Process.GetProcessesByName("virtualdj");
            if (processes.Length > 0)
            {
                vdjProcess = processes[0];
            }
        }

        public void Add(string header, string vdjScript)
        {
            actionKeys.Add(header, VirtualKeys.Next());
            actions.Add(vdjScript);
        }

        public void ExecuteAction(string header)
        {
            mainForm.ControlActive = true;
            keybd_event(VirtualKeys.CTRL, 0, 0, 0);
            SendKey(actionKeys[header]);
            keybd_event(VirtualKeys.CTRL, 0, VirtualKeys.WM_KEYUP, 0);
            mainForm.ControlActive = false;
            PostMessage(vdjProcess.MainWindowHandle, VirtualKeys.WM_KEYDOWN, VirtualKeys.ESC, 0);
        }

        public async void ExecuteSmoothAction(string header, int value)
        {
            bool exist = lastValues.TryGetValue(header, out int lastValue);
            if (!exist) lastValue = 100;

            bool firstSuccess = actionKeys.TryGetValue(header + " up", out byte up);
            bool secondSuccess = actionKeys.TryGetValue(header + " down", out byte down);

            while(mainForm.ControlActive)
            {
                await Task.Delay(1);
            }
            mainForm.ControlActive = true;
            if (firstSuccess && secondSuccess)
            {
                Console.WriteLine("ctrl");

                keybd_event(VirtualKeys.CTRL, 0, 0, 0);

                for (int i = 0; i < value - lastValue; i++)
                {
                    keybd_event(VirtualKeys.CTRL, 0, 0, 0);
                    SendKey(up);
                }
                for (int i = 0; i < lastValue - value; i++)
                {
                    keybd_event(VirtualKeys.CTRL, 0, 0, 0);
                    SendKey(down);
                }

                keybd_event(VirtualKeys.CTRL, 0, VirtualKeys.WM_KEYUP, 0);

                if (!exist) lastValues.Add(header, value);
                else lastValues[header] = value;
                
            }

            mainForm.ControlActive = false;
        }

        public void MoveJogwheel(String header, int direction)
        {
            bool firstSuccess = actionKeys.TryGetValue(header + " F", out byte forwardKey);
            bool secondSuccess = actionKeys.TryGetValue(header + " B", out byte backwardKey);

            if (firstSuccess && secondSuccess)
            {
                mainForm.ControlActive = true;
                keybd_event(VirtualKeys.CTRL, 0, 0, 0);
                SendKey(direction == 1 ? forwardKey : backwardKey);
                keybd_event(VirtualKeys.CTRL, 0, VirtualKeys.WM_KEYUP, 0);
                mainForm.ControlActive = false;
                PostMessage(vdjProcess.MainWindowHandle, VirtualKeys.WM_KEYDOWN, VirtualKeys.ESC, 0);
            }
        }

        public bool ActionExist(string header)
        {
            return actionKeys.TryGetValue(header, out _);
        }

        public bool SmoothActionExist(string header)
        {
            return actionKeys.TryGetValue(header + " up", out _) && actionKeys.TryGetValue(header + " up", out _);
        }

        public void SendKey(byte key)
        {
            PostMessage(vdjProcess.MainWindowHandle, VirtualKeys.WM_KEYDOWN, key, 0);
        }

        public async void Update()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\VirtualDJ\Mappers\KEYBOARD - Custom Mapping.xml");
            string previousContent = doc.InnerXml;

            XmlNodeList nodeList = doc.DocumentElement.SelectNodes("/mapper/map");

            VirtualKeys.Reset();

            for(int i = 0; i < actionKeys.Count; i++)
            {
                string key = "CTRL+" + VirtualKeys.GetName();
                XmlNode node = GetNodeByKeyName(key, nodeList);
                if (node != null)
                {
                    node.Attributes.GetNamedItem("action").Value = actions[i];
                }
                else
                {
                    XmlElement element =  doc.CreateElement("map");
                    element.SetAttribute("value", key);
                    element.SetAttribute("action", actions[i]);
                    doc.SelectSingleNode("mapper").AppendChild(element);
                }
                VirtualKeys.Next();
            }

            doc.Save(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\VirtualDJ\Mappers\KEYBOARD - Custom Mapping.xml");

            mainForm.StatusText = "Initialization Successful";
            await Task.Delay(500);

            if (!doc.InnerXml.Equals(previousContent))
            {
                RestartVirtualDJ();
            } else
            {
                await Task.Delay(500);
                StartVirtualDJ();
            }

            await Task.Delay(1000);
            mainForm.StatusText = "Bridge is ready";

            mainForm.BridgeActive = true;
        }

        public int GetKey(string header)
        {
            return actionKeys[header];
        }

        public Process GetVDJProc()
        {
            return vdjProcess;
        }

        private XmlNode GetNodeByKeyName(string keyName, XmlNodeList nodeList)
        {
            foreach (XmlNode node in nodeList)
            {
                if(node.Attributes.Item(0).Value == keyName)
                {
                    return node;
                }
            }
            return null;
        }

        private void StartVirtualDJ()
        {
            if (vdjProcess == null)
            {
                mainForm.StatusText = "Starting Virtual DJ...";
                string path = @"C:\Program Files\VirtualDJ\virtualdj.exe";
                Process.Start(path);
                FindVDJProcess();
            }
        }

        private void RestartVirtualDJ()
        {
            string path = @"C:\Program Files\VirtualDJ\virtualdj.exe";
            if (vdjProcess != null)
            {
                mainForm.StatusText = "Retarting Virtual DJ...";
                vdjProcess.Kill();
                Process.Start(path);
            }
            else
            {
                mainForm.StatusText = "Virtual DJ is not opened";
            }
        }
    }
}
