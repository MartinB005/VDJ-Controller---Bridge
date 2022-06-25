using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VDJ_Controller
{
    public partial class MainForm : Form
    {
        SerialCommunication communication;

        public MainForm()
        {
            InitializeComponent();


            communication = new SerialCommunication();

        }

        public interface OnCtrlPressedListener {
            void OnCtrlPressed();
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            /*
            if (ModifierKeys.HasFlag(Keys.Control))
            {
                List<ThreadStart> awaitingActions = communication.GetActionManager().GetAwaitingActions();

                foreach(ThreadStart action in awaitingActions)
                {
                    Thread thread = new Thread(action);

                    thread.Start();
                }


                awaitingActions.Clear();

                */
            
        }
    }
}
