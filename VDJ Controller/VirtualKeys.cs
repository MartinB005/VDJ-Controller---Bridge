using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VDJ_Controller
{
    internal class VirtualKeys
    {

        public const int WM_KEYDOWN = 0x0100;
        public const int WM_KEYUP = 2;
        public const int A = 0x41;
        public const int B = 0x42;
        public const int C = 0x43;
        public const int D = 0x44;
        public const int E = 0x45;
        public const int P = 0x50;
        public const int Q = 0x51;
        public const int CTRL = 0x11;
        public const int TAB = 0x09;

        public static byte key = A;


        public static byte Next()
        {
            byte keyValue = key;
            key++;
            return keyValue;
        }

        public static char GetChar()
        {
            return (char)key;
        }

        public static void Reset()
        {
            key = A;
        }
    }
}
