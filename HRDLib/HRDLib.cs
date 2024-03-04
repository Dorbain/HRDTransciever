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
using System.Security.Policy;


namespace HRDLib
{
    /// <summary>
    /// Ham Radio Deluxe Library. Using translated code from the WSJT-X mirror project.
    /// Converted and adjusted/altered from C++ to C#.
    /// Published by Grant B saitohirga on github.com https://github.com/saitohirga/WSJT-X/tree/master
    /// Released under the GNU GPL-3.0 License.
    /// </summary>
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




    
    

    internal static class HRDinitialize
    {
        internal static void Start()
        {
            if (!Directory.Exists(HRDinternal.Folder))
            {
                Directory.CreateDirectory(HRDinternal.Folder);
            }
            if(File.Exists(HRDinternal.logFileName))
            {
                File.Move(HRDinternal.logFileName, HRDinternal.logFileName + DateTime.Now.ToString("hhmmss"));
            }
            else if(!File.Exists(HRDinternal.logFileName))
            {
                WriteLog.log("");
            }
            WriteLog.log("Start...");
        }
        internal static class Get
        {
            internal static void Context()
            {
                WriteLog.log(System.Reflection.MethodBase.GetCurrentMethod().Name);
                HRDConnection.Write("get context");
                HRD.context = Int32.Parse(new string(HRDinternal.readMessage(HRDinternal.Stream).Where(c => char.IsDigit(c)).ToArray()));
                //HRD.context = HRDinternal.readMessage(HRDinternal.Stream);
            }

            internal static void ID()
            {
                WriteLog.log(System.Reflection.MethodBase.GetCurrentMethod().Name);
                HRDConnection.Write("get id");
                HRD.id = HRDinternal.readMessage(HRDinternal.Stream);
            }

            internal static void Version()
            {
                WriteLog.log(System.Reflection.MethodBase.GetCurrentMethod().Name);
                HRDConnection.Write("get version");
                string[] version = HRDinternal.readMessage(HRDinternal.Stream).Split(' ');
                HRD.version = version[0];
                HRD.build = version[1];
            }

            internal static void Radios()
            {
                // can be more radios, only one connected need to figure out more.
                WriteLog.log(System.Reflection.MethodBase.GetCurrentMethod().Name);
                HRDConnection.Write("get radios");
                HRD.radios = HRDinternal.readMessage(HRDinternal.Stream);

            }

            internal static void Radio()
            {
                WriteLog.log(System.Reflection.MethodBase.GetCurrentMethod().Name);
                HRDConnection.Write("get radio");
                HRD.radio = HRDinternal.readMessage(HRDinternal.Stream);
            }

            internal static void VFOcount()
            {
                WriteLog.log(System.Reflection.MethodBase.GetCurrentMethod().Name);
                HRDConnection.Write("get vfo-count");
                HRD.vfoCount = Int32.Parse(new string(HRDinternal.readMessage(HRDinternal.Stream).Where(c => char.IsDigit(c)).ToArray()));
            }

            internal static void Frequency()
            {
                WriteLog.log(System.Reflection.MethodBase.GetCurrentMethod().Name);
                HRDConnection.Write("get frequency");
                HRD.frequency = HRDinternal.readMessage(HRDinternal.Stream);
            }

            internal static void Frequencies()
            {
                WriteLog.log(System.Reflection.MethodBase.GetCurrentMethod().Name);
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
                WriteLog.log(System.Reflection.MethodBase.GetCurrentMethod().Name);
                HRDConnection.Write("get buttons");
                string[] buttons = HRDinternal.readMessage(HRDinternal.Stream).Split(',');
                HRD.buttons = buttons.ToList();
                //("get button-select " + buttons_.value (button_index)) to see if button is selected foreach loop through the buttons list
            }

            internal static void DropdownNames()
            {
                WriteLog.log(System.Reflection.MethodBase.GetCurrentMethod().Name);
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

    public static class WriteLog
    {
        internal static void log(string text)
        {
            string dtprefix = DateTime.Now.Day.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Year.ToString() + "|" + DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString() + ":" + DateTime.Now.Second.ToString() + ":" + DateTime.Now.Millisecond.ToString();
            if (!HRDinternal.debug)
            {
                string logprefix = " LOG: ";
                using (StreamWriter writer = File.AppendText(HRDinternal.logFileName))
                {
                    writer.WriteLine(dtprefix + logprefix + text);
                }
            }
            else if (HRDinternal.debug)
            {
                string logprefix = " DEBUG: ";
                    using (StreamWriter writer = File.AppendText(HRDinternal.logFileName))
                    {
                        writer.WriteLine(dtprefix + logprefix + text);
                    }
            }
        }
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