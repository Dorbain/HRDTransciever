using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlTypes;
using System.IO;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ComTypes;
using System.Net;

namespace HRDLib
{
    internal static class HRDinternal
    {
        internal static TcpClient Client = new TcpClient();
        internal static Stream Stream = null;
        internal static bool Connection = false;
        internal static byte[] composeSendMessage(string _hrdmessage)
        {
            // convert the string into unicode byte array
            byte[] ba = Encoding.Unicode.GetBytes(_hrdmessage);
            // someting to add unclear why
            byte[] magic = new byte[15] { 0x00, 0x00, 0x00, 0xcd, 0xab, 0x34, 0x12, 0x34, 0x12, 0xcd, 0xab, 0x00, 0x00, 0x00, 0x00 };
            byte[] theend = new byte[2] { 0x00, 0x00 };

            var addMagicInFront = AppendInto(ba, magic, 0);
            var addTheEndInTheBack = AppendInto(theend, addMagicInFront, 0);

            // get the whole length of the byte array
            int length = addTheEndInTheBack.Length + 1;
            // convert the lenght into a byte
            byte something = (byte)length;
            byte[] buffer = new byte[1] { something };
            // add the lenth in the front of the byte array
            var textToSend = AppendInto(addTheEndInTheBack, buffer, 0);

            return textToSend;
        }

        internal static byte[] AppendInto(byte[] original, byte[] toInsert, int appendIn)
        {
            var bytes = original.ToList();
            bytes.InsertRange(appendIn, toInsert);
            return bytes.ToArray();
        }

        internal static string readMessage(Stream stream)
        {
            string returnText = string.Empty;
            byte[] bytesToRead = new byte[2048]; // MTU 1500 so 2048 should be enough.
            int totalBytesRead = stream.Read(bytesToRead, 0, 2048);


            for (int i = 14; i < totalBytesRead; i++)
                returnText = returnText + Convert.ToChar(bytesToRead[i]);

            return returnText.ToString();
        }


    }


    public static class HRD
    {
        public static int context = 0; 
        public static string id = string.Empty; 
        public static string version = string.Empty;
        public static string build = string.Empty;
        public static string radios = string.Empty; //List<string> radios = new List<string>();
        public static string radio = string.Empty;
        public static int vfoCount = 0;
        public static string frequency = string.Empty;
        public static string frequencies = string.Empty;
        public static string vfoAfrequency = string.Empty;
        public static string vfoBfrequency = string.Empty;
        public static List<string> buttons = new List<string>();
        public static List<string> dropdownNames = new List<string>();
        public static List<string> dropdownLists = new List<string>();
        public static List<string> dropdownTexts = new List<string>();
    }
    
    
    public static class HRDConnection
    {
        /// <summary>
        /// The port which is used for the Ham Radio Deluxe IP Server - Default = 7809
        /// </summary>
        public static int Port = 7809;
        /// <summary>
        /// The IP Address which is used for the Ham Radio Deluxe IP Server - Default = 127.0.0.1
        /// </summary>
        public static string Address = "127.0.0.1";

        public static string UserName = string.Empty;
        
        public static string Password = string.Empty;

        public static bool connected = false;

        /// <summary>
        /// Opens the connection.
        /// </summary>
        /// <returns>Returns a stream</returns>
        public static void Connect()
        {
            if (!HRDinternal.Connection) 
            {
                HRDinternal.Client.Connect(Address, Port);
                HRDinternal.Stream = HRDinternal.Client.GetStream();
                HRDinternal.Connection = true;
                connected = true;
            }
        }

        public static void Initialize()
        {
            // we should get the data here from Ham Radio Deluxe first and add those into the items.
            initialize.Get.Context();
            initialize.Get.ID();
            initialize.Get.Version();
            initialize.Get.Radios();
            initialize.Get.Radio();
            initialize.Get.VFOcount();
            initialize.Get.Frequency();
            initialize.Get.Frequencies();
            initialize.Get.Buttons();
            initialize.Get.DropdownNames();
        }

        /// <summary>
        /// Closes the connection
        /// </summary>
        public static void Close()
        {
            if (HRDinternal.Connection)
            {
                HRDinternal.Client.Close();
                HRDinternal.Client.Dispose();
                HRDinternal.Stream.Close();
                HRDinternal.Stream.Dispose();
                HRDinternal.Connection = false;
                connected = false;
            }
        }

        /// <summary>
        /// Renews/ Refreshes the connection after closure.
        /// </summary>
        public static void Renew()
        {
            if (!HRDinternal.Connection)
            {
                HRDinternal.Client = new TcpClient();
                HRDinternal.Stream = null;
            }
        }

        internal static void Write(string message)
        {
            byte[] messageToSend = HRDinternal.composeSendMessage(message);
            HRDinternal.Stream.Write(messageToSend, 0, messageToSend.Length);
        }

        

    }

    internal static class initialize
    {
        internal static class Get
        {
            internal static void Context()
            {
                HRDConnection.Write("get context");
                HRD.context = Int32.Parse(new string(HRDinternal.readMessage(HRDinternal.Stream).Where(c => char.IsDigit(c)).ToArray()));
                //HRD.context = HRDinternal.readMessage(HRDinternal.Stream);
            }

            internal static void ID()
            {
                HRDConnection.Write("get id");
                HRD.id = HRDinternal.readMessage(HRDinternal.Stream);
            }

            internal static void Version()
            {
                HRDConnection.Write("get version");
                string[] version = HRDinternal.readMessage(HRDinternal.Stream).Split(' ');
                HRD.version = version[0];
                HRD.build = version[1];
            }

            internal static void Radios()
            {
                // can be more radios, only one connected need to figure out more.

                HRDConnection.Write("get radios");
                HRD.radios = HRDinternal.readMessage(HRDinternal.Stream);

            }

            internal static void Radio()
            {
                HRDConnection.Write("get radio");
                HRD.radio = HRDinternal.readMessage(HRDinternal.Stream);
            }

            internal static void VFOcount()
            {
                HRDConnection.Write("get vfo-count");
                HRD.vfoCount = Int32.Parse(new string(HRDinternal.readMessage(HRDinternal.Stream).Where(c => char.IsDigit(c)).ToArray()));
            }

            internal static void Frequency()
            {
                HRDConnection.Write("get frequency");
                HRD.frequency = HRDinternal.readMessage(HRDinternal.Stream);
            }

            internal static void Frequencies()
            {
                HRDConnection.Write("get frequencies");
                HRD.frequencies = HRDinternal.readMessage(HRDinternal.Stream);
                if (HRD.vfoCount >= 2)
                {
                    string[] split = HRD.frequencies.Split('-');
                    HRD.vfoAfrequency = split[0];
                    HRD.vfoBfrequency = split[1];
                }
            }

            internal static void Buttons()
            {
                HRDConnection.Write("get buttons");
                string[] buttons = HRDinternal.readMessage(HRDinternal.Stream).Split(',');
                HRD.buttons = buttons.ToList();
                //("get button-select " + buttons_.value (button_index)) to see if button is selected foreach loop through the buttons list
            }

            internal static void DropdownNames()
            {
                HRDConnection.Write("get dropdowns");
                string[] dropdownNames = HRDinternal.readMessage(HRDinternal.Stream).Split(',');
                HRD.dropdownNames = dropdownNames.ToList();
                foreach(string dropdownName in HRD.dropdownNames)
                {
                    HRDConnection.Write("get dropdown-list {" + dropdownName + "}");
                    HRD.dropdownLists.Add(HRDinternal.readMessage(HRDinternal.Stream));
                }
                foreach (string dropdownName in HRD.dropdownNames)
                {
                    HRDConnection.Write("get dropdown-text {" + dropdownName + "}");
                    HRD.dropdownTexts.Add(HRDinternal.readMessage(HRDinternal.Stream));
                }

                ////("get dropdown-list {" + dd + "}")
                ////("get dropdown-text {" + dd_name + "}")
            }





            //comm("get sliders", HRDstream);
            ////("get slider-range " + current_radio_name + " " + s)






        }
    }

    public static class Get
    {





    }


    

}


//quint32 size_;
//qint32 magic_1_;
//qint32 magic_2_;
//qint32 checksum_;            // Apparently not used.
//QChar payload_[0];           // UTF-16 (which is wchar_t on Windows)

//static qint32 constexpr magic_1_value_ = 0x1234ABCD;
//static qint32 constexpr magic_2_value_ = 0xABCD1234;



//byte[] magic1 = new byte[4] { 0x12, 0x34, 0xAB, 0xCD };
//byte[] magic2 = new byte[4] { 0xAB, 0xCD, 0x12, 0x34 };