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
    

    public static class Get
    {


    }

    public static class Set
    {
        //"set frequency-hz " + QString::number (f)
        //"set frequencies-hz " + QString::number(frequencies[0].toUInt()) + ' ' + fo_string)
        //"set dropdown " + dd_name.replace (' ', '~') + ' ' + dropdowns_.value (dd_name).value (value).replace (' ', '~') + ' ' + QString::number (value)
        //"set button-select " + buttons_.value (button_index) + (checked ? " 1" : " 0")

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