using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace HRDLib
{
    internal static class HRDinternal
    {
        internal static TcpClient Client = new TcpClient();
        internal static Stream Stream = null;
        internal static bool Connection = false;
        internal static bool debug = true;
        internal static System.Timers.Timer poll = new System.Timers.Timer();
        internal static byte[] composeSendMessage(string _hrdmessage)
        {
            WriteLog.log(System.Reflection.MethodBase.GetCurrentMethod().Name);
            // convert the string into unicode byte array
            byte[] hrdMessage = Encoding.Unicode.GetBytes(_hrdmessage);
            // Get the size of the message string for payload calculation
            int payloadSize = _hrdmessage.Length;
            // Convert the length into a byte array
            byte payLoadSize = (byte)(payloadSize);
            byte[] _payload = new byte[1] { payLoadSize };
            // Checksum is always 00? Seems not to be used.
            byte[] _checksum = new byte[2] { 0x00, 0x00 };

            byte[] username = new byte[7] { 0x00, 0x00, 0x00, 0xcd, 0xab, 0x34, 0x12 };
            byte[] password = new byte[7] { 0x34, 0x12, 0xcd, 0xab, 0x00, 0x00, 0x00 };

            // Now construct the message:
            byte[] addLogon = CombineByteArrays(username, password);
            byte[] addPayload = CombineByteArrays(addLogon, _payload);
            byte[] addMessage = CombineByteArrays(addPayload, hrdMessage);
            byte[] totalMessage = CombineByteArrays(addMessage, _checksum);

            // get the whole length of the byte array
            int length = totalMessage.Length + 1;
            // convert the lenght into a byte
            byte something = (byte)length;
            byte[] buffer = new byte[1] { something };
            // add the lenth in the front of the byte array
            var textToSend = CombineByteArrays(buffer, totalMessage);
            return textToSend;
        }

        internal static byte[] CombineByteArrays(byte[] original, byte[] toAppend)
        {
            WriteLog.log(System.Reflection.MethodBase.GetCurrentMethod().Name);
            var bytes = original.ToList();
            bytes.InsertRange(original.Length, toAppend);
            return bytes.ToArray();
        }

        internal static string readMessage(Stream stream)
        {
            WriteLog.log(System.Reflection.MethodBase.GetCurrentMethod().Name);
            string returnText = string.Empty;
            byte[] bytesToRead = new byte[2048]; // Asuming a MTU size of 1500 so 2048 should be enough.
            int totalBytesRead = stream.Read(bytesToRead, 0, 2048);


            for (int i = 14; i < totalBytesRead; i++)
                returnText = returnText + Convert.ToChar(bytesToRead[i]);

            return returnText.ToString();
        }
        internal static readonly string Folder = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\HRDlib\\";
        internal static readonly string logFileName = Path.Combine(Folder, "HRDCommLog-" + DateTime.Now.ToString("MMddyyyy") + ".log");
    }
}
