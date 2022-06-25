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

        private static bool ctrlPressed;

        private readonly Dictionary<string, byte> actionKeys = new Dictionary<string, byte>();
        private readonly Dictionary<string, int> lastValues = new Dictionary<string, int>();

        private List<string> actions = new List<string>();
        private Process vdjProcess;

        private List<ThreadStart> awaitingActions = new List<ThreadStart>();


        public ActionManager()
        {
            Process[] processes = Process.GetProcessesByName("virtualdj");
            vdjProcess = processes[0];
        }

        public void Add(string header, string vdjScript)
        {
            actionKeys.Add(header, VirtualKeys.Next());
            actions.Add(vdjScript);
        }

        public void ExecuteAction(string header)
        {
            SendKey(actionKeys[header]);
        }

        public void ExecuteSmoothAction(string header, int value)
        {
            bool exist = lastValues.TryGetValue(header, out int lastValue);
            if (!exist) lastValue = 100;

            bool firstSuccess = actionKeys.TryGetValue(header + " up", out byte up);
            bool secondSuccess = actionKeys.TryGetValue(header + " down", out byte down);

            if (firstSuccess && secondSuccess)
            {
                ctrlPressed = true;
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

        }

        public List<ThreadStart> GetAwaitingActions()
        {
            return awaitingActions;
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

        public void Update()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(@"C:\Users\Martin Belej\Documents\VirtualDJ\Mappers\KEYBOARD - Custom Mapping.xml");
            string previousContent = doc.InnerXml;

            XmlNodeList nodeList = doc.DocumentElement.SelectNodes("/mapper/map");

            VirtualKeys.Reset();

            for(int i = 0; i < actionKeys.Count; i++)
            {
                XmlNode node = GetNodeByKeyName("CTRL+" + VirtualKeys.GetChar(), nodeList);
                if (node != null)
                {
                    node.Attributes.GetNamedItem("action").Value = actions[i];
                }
                else
                {
                    XmlElement element =  doc.CreateElement("map");
                    element.SetAttribute("value", "CTRL+" + VirtualKeys.GetChar());
                    element.SetAttribute("action", actions[i]);
                    doc.SelectSingleNode("mapper").AppendChild(element);
                }
                VirtualKeys.Next();
            }

            doc.Save(@"C:\Users\Martin Belej\Documents\VirtualDJ\Mappers\KEYBOARD - Custom Mapping.xml");

            if(!doc.InnerXml.Equals(previousContent))
            {
                RestartVirtualDJ();
            }
        }

        public int GetKey(string header)
        {
            return actionKeys[header];
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

        private void RestartVirtualDJ()
        {
            string path = @"C:\Program Files\VirtualDJ\virtualdj.exe";
            vdjProcess.Kill();
            Process.Start(path);
        }
    }
}
