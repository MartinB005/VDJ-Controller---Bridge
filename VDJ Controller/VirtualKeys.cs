using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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

        private static int index = 0;
        private static List<Key> keys;

        public static void Init() {

            using (StreamReader r = new StreamReader("keys.json"))
            {
                string json = r.ReadToEnd();
                keys = JsonConvert.DeserializeObject<List<Key>>(json);
            }

        }


        public static int GetMaxCount()
        {
            return keys.Count;
        }

        public static byte Next()
        {
            byte keyValue = keys[index].Value;
            index++;
            return keyValue;
        }

        public static string GetName() => keys[index].Name;

        public static void Reset()
        {
            index = 0;
        }

        public class Key
        {
            public string Name { get; set; }
            public byte Value { get; set; }
        }
    }
}
