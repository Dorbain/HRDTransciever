using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using HRDLib;

namespace Test
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Connecting to Ham Radio Deluxe.....");
            try
            {
                // Creating the TCP client for Ham Radio Deluxe v5 or v6
                HRDConnection.Connect();
                if (HRDConnection.connected)
                {
                    Console.WriteLine("Connected");
                    HRDConnection.Initialize();

                    Console.WriteLine("Context: " +HRD.context.ToString());
                    Console.WriteLine("ID: " + HRD.id);
                    Console.WriteLine("Version: " + HRD.version);
                    Console.WriteLine("Build: " + HRD.build);
                    Console.WriteLine("Radios: " + HRD.radios);
                    Console.WriteLine("Radio: " + HRD.radio);
                    Console.WriteLine("VFO count: " + HRD.vfoCount.ToString());
                    Console.WriteLine("Frequency: " + HRD.frequency);
                    Console.WriteLine("Frequencies: " + HRD.frequencies);
                    Console.WriteLine("VFO- A: " + HRD.vfoAfrequency);
                    Console.WriteLine("VFO- B: " + HRD.vfoBfrequency);
                    Console.WriteLine(Environment.NewLine + "Buttons");
                    foreach (string button in HRD.buttons)
                    { Console.Write(": " + button); }
                    Console.WriteLine(Environment.NewLine + "DropdownNames");
                    foreach (string dropdownName in HRD.dropdownNames)
                    { Console.Write(": " + dropdownName); }
                    Console.WriteLine(Environment.NewLine + "DropdownLists");
                    foreach (string dropdownList in HRD.dropdownLists)
                    { Console.Write(": " + dropdownList); }
                    Console.WriteLine(Environment.NewLine + "DropdownTexts");
                    foreach (string dropdownText in HRD.dropdownTexts)
                    { Console.Write(": " + dropdownText); }
                    //Console.WriteLine(Environment.NewLine + "Dropdowns: " + HRD.dropdowns);


                }
                HRDConnection.Close();
            }

            catch (Exception ex)
            {
                Console.WriteLine("Error..... " + ex.StackTrace);
            }
            Console.ReadKey();
        }

        //private static void comm(string message, Stream stream)
        //{
        //    byte[] messageToSend = HRDmessage(message);

        //    Console.WriteLine("Getting the " + message + " :");

        //    stream.Write(messageToSend, 0, messageToSend.Length);

        //    Console.WriteLine(readMessage(stream));

        //}

        //private static byte[] AppendInto(byte[] original, byte[] toInsert, int appendIn)
        //{
        //    var bytes = original.ToList();
        //    bytes.InsertRange(appendIn, toInsert);
        //    return bytes.ToArray();
        //}

        //private static byte[] HRDmessage(string _hrdmessage)
        //{
        //    // convert the string into unicode byte array
        //    byte[] ba = Encoding.Unicode.GetBytes(_hrdmessage);
        //    // someting to add unclear why
        //    byte[] magic = new byte[15] { 0x00, 0x00, 0x00, 0xcd, 0xab, 0x34, 0x12, 0x34, 0x12, 0xcd, 0xab, 0x00, 0x00, 0x00, 0x00 };
        //    byte[] theend = new byte[2] { 0x00, 0x00 };
 
        //    var addMagicInFront = AppendInto(ba, magic, 0);
        //    var addTheEndInTheBack = AppendInto(theend, addMagicInFront, 0);
            
        //    // get the whole length of the byte array
        //    int length = addTheEndInTheBack.Length + 1;
        //    // convert the lenght into a byte
        //    byte something = (byte)length;
        //    byte[] buffer = new byte[1] { something };
        //    // add the lenth in the front of the byte array
        //    var textToSend = AppendInto(addTheEndInTheBack, buffer, 0);

        //    return textToSend;
        //}

        //private static string readMessage(Stream stream)
        //{
        //    string returnText = string.Empty;
        //    byte[] bytesToRead = new byte[4096];
        //    int totalBytesRead = stream.Read(bytesToRead, 0, 4096);


        //    for (int i = 14; i < totalBytesRead; i++)
        //        returnText = returnText + Convert.ToChar(bytesToRead[i]);

        //    return returnText.ToString();
        //}
    }
}


//// Let try if it works:
//comm("get context", HRDstream);

//comm("get id", HRDstream);

//comm("get version", HRDstream);

//comm("get radios", HRDstream);

//comm("get radio", HRDstream);

//comm("get vfo-count", HRDstream);

//comm("get buttons", HRDstream);
////("get button-select " + buttons_.value (button_index))

//comm("get dropdowns", HRDstream);

////("get dropdown-list {" + dd + "}")
////("get dropdown-text {" + dd_name + "}")

//comm("get sliders", HRDstream);
////("get slider-range " + current_radio_name + " " + s)

//comm("get frequency", HRDstream);

//comm("get frequencies", HRDstream);
//Stream stm = tcpclnt.GetStream();
//TcpClient tcpclnt = new TcpClient();
//tcpclnt.Connect("127.0.0.1", 7809);
// We need to be on the local computer and use the default port 7809
//tcpclnt.Close();