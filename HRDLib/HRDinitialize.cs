using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRDLib
{
    internal static class HRDinitialize
    {
        internal static void Start()
        {
            if (!Directory.Exists(HRDinternal.Folder))
            {
                Directory.CreateDirectory(HRDinternal.Folder);
            }
            if (File.Exists(HRDinternal.logFileName))
            {
                File.Move(HRDinternal.logFileName, HRDinternal.logFileName + DateTime.Now.ToString("hhmmss"));
            }
            else if (!File.Exists(HRDinternal.logFileName))
            {
                HRDinternal.WriteLog.log("");
            }
            
            HRDinternal.WriteLog.log("Start...");
        }
        internal static class Get
        {
            internal static void Context()
            {
                HRDinternal.WriteLog.log(System.Reflection.MethodBase.GetCurrentMethod().Name);
                HRDConnection.Write("get context");
                HRD.context = Int32.Parse(new string(HRDinternal.readMessage(HRDinternal.Stream).Where(c => char.IsDigit(c)).ToArray()));
                //HRD.context = HRDinternal.readMessage(HRDinternal.Stream);
            }

            internal static void ID()
            {
                HRDinternal.WriteLog.log(System.Reflection.MethodBase.GetCurrentMethod().Name);
                HRDConnection.Write("get id");
                HRD.id = HRDinternal.readMessage(HRDinternal.Stream);
            }

            internal static void Version()
            {
                HRDinternal.WriteLog.log(System.Reflection.MethodBase.GetCurrentMethod().Name);
                HRDConnection.Write("get version");
                string[] version = HRDinternal.readMessage(HRDinternal.Stream).Split(' ');
                HRD.version = version[0];
                HRD.build = version[1];
            }

            internal static void Radios()
            {
                // can be more radios, only one connected need to figure out more.
                HRDinternal.WriteLog.log(System.Reflection.MethodBase.GetCurrentMethod().Name);
                HRDConnection.Write("get radios");
                HRD.radios = HRDinternal.readMessage(HRDinternal.Stream);

            }

            internal static void Radio()
            {
                HRDinternal.WriteLog.log(System.Reflection.MethodBase.GetCurrentMethod().Name);
                HRDConnection.Write("get radio");
                HRD.radio = HRDinternal.readMessage(HRDinternal.Stream);
            }

            internal static void VFOcount()
            {
                HRDinternal.WriteLog.log(System.Reflection.MethodBase.GetCurrentMethod().Name);
                HRDConnection.Write("get vfo-count");
                HRD.vfoCount = Int32.Parse(new string(HRDinternal.readMessage(HRDinternal.Stream).Where(c => char.IsDigit(c)).ToArray()));
            }

            internal static void Frequency()
            {
                HRDinternal.WriteLog.log(System.Reflection.MethodBase.GetCurrentMethod().Name);
                HRDConnection.Write("get frequency");
                HRD.frequency = HRDinternal.readMessage(HRDinternal.Stream);
            }

            internal static void Frequencies()
            {
                HRDinternal.WriteLog.log(System.Reflection.MethodBase.GetCurrentMethod().Name);
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
                HRDinternal.WriteLog.log(System.Reflection.MethodBase.GetCurrentMethod().Name);
                HRDConnection.Write("get buttons");
                string[] buttons = HRDinternal.readMessage(HRDinternal.Stream).Split(',');
                HRD.buttons = buttons.ToList();
                ButtonSelect();
                // we have buttons? then we need to get their index number as well.
                //("get button-select " + buttons_.value (button_index)) to see if button is selected foreach loop through the buttons list
            }

            internal static void ButtonSelect()
            {
                HRDinternal.WriteLog.log(System.Reflection.MethodBase.GetCurrentMethod().Name);
                for (int i = 0; i < HRD.buttons.Count; i++)
                {
                    HRDConnection.Write("get button-select " + i.ToString());
                    string button = HRDinternal.readMessage(HRDinternal.Stream);
                    HRDinternal.WriteLog.log("button:" + button);
                }

                // we have buttons? then we need to get their index number as well.
                //("get button-select " + buttons_.value (button_index)) to see if button is selected foreach loop through the buttons list
            }


            internal static void DropdownNames()
            {
                HRDinternal.WriteLog.log(System.Reflection.MethodBase.GetCurrentMethod().Name);
                HRDConnection.Write("get dropdowns");
                string[] dropdownNames = HRDinternal.readMessage(HRDinternal.Stream).Split(',');
                HRD.dropdownNames = dropdownNames.ToList();

                int ddn = 0;
                foreach (string dropdownName in HRD.dropdownNames)
                {
                    //HRDConnection.Write("get dropdown-list {" + dropdownName + "}");
                    HRDConnection.Write("get dropdown-list {" + ddn.ToString() + "}");
                    HRD.dropdownLists.Add(HRDinternal.readMessage(HRDinternal.Stream));
                    ddn++;
                }
                int ddnms = 0;
                foreach (string dropdownName in HRD.dropdownNames)
                {
                    //HRDConnection.Write("get dropdown-text {" + dropdownName + "}");
                    HRDConnection.Write("get dropdown-text {" + ddnms.ToString() + "}");
                    HRD.dropdownTexts.Add(HRDinternal.readMessage(HRDinternal.Stream));
                    ddnms++;
                }

                ////("get dropdown-list {" + dd + "}")
                ////("get dropdown-text {" + dd_name + "}")
            }





            //comm("get sliders", HRDstream);
            ////("get slider-range " + current_radio_name + " " + s)






        }
    }
}
