﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace HRDLib
{
    /// <summary>
    /// HRD Connection Class
    /// </summary>
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
        public static readonly bool isConnected = HRDinternal.Connection;
        /// <summary>
        /// Opens the connection.
        /// </summary>
        /// <returns>Returns a stream</returns>
        public static void Connect()
        {
            HRDinitialize.Start();
            WriteLog.log(System.Reflection.MethodBase.GetCurrentMethod().Name);
            if (!HRDinternal.Connection)
            {
                HRDinternal.Client.Connect(Address, Port);
                HRDinternal.Stream = HRDinternal.Client.GetStream();
                HRDinternal.Connection = true;
            }
        }
        /// <summary>
        /// Gets the first data from HRD when the connection is succesfully opened.
        /// </summary>
        public static void Initialize()
        {
            WriteLog.log(System.Reflection.MethodBase.GetCurrentMethod().Name);
            // we should get the data here from Ham Radio Deluxe first and add those into the items.
            HRDinitialize.Get.Context();
            HRDinitialize.Get.ID();
            HRDinitialize.Get.Version();
            HRDinitialize.Get.Radios();
            HRDinitialize.Get.Radio();
            HRDinitialize.Get.VFOcount();
            HRDinitialize.Get.Frequency();
            HRDinitialize.Get.Frequencies();
            HRDinitialize.Get.Buttons();
            HRDinitialize.Get.DropdownNames();
        }
        /// <summary>
        /// Closes the connection
        /// </summary>
        public static void Close()
        {
            WriteLog.log(System.Reflection.MethodBase.GetCurrentMethod().Name);
            if (HRDinternal.Connection)
            {
                HRDinternal.poll.Stop();
                HRDinternal.Client.Close();
                HRDinternal.Client.Dispose();
                HRDinternal.Stream.Close();
                HRDinternal.Stream.Dispose();
                HRDinternal.Connection = false;
            }
        }
        /// <summary>
        /// Renews/ Refreshes the connection after closure.
        /// </summary>
        public static void Renew()
        {
            WriteLog.log(System.Reflection.MethodBase.GetCurrentMethod().Name);
            if (!HRDinternal.Connection)
            {
                HRDinternal.Client = new TcpClient();
                HRDinternal.Stream = null;
            }
        }
        /// <summary>
        /// Starts polling HRD
        /// </summary>
        public static void Poll()
        {
            HRDinternal.poll.Elapsed += new System.Timers.ElapsedEventHandler(OnPollTimeEvent);
            HRDinternal.poll.Interval = 10000; // WSJT-X, WSJT-Z and JTDX are polling every minute? or 10 seconds/ needs more research.
            HRDinternal.poll.Enabled = true;
            HRDinternal.poll.Start();
        }
        internal static void OnPollTimeEvent(object sender, System.Timers.ElapsedEventArgs e)
        {

        }
        internal static void Write(string message)
        {
            WriteLog.log(System.Reflection.MethodBase.GetCurrentMethod().Name);
            byte[] messageToSend = HRDinternal.composeSendMessage(message);
            HRDinternal.Stream.Write(messageToSend, 0, messageToSend.Length);
        }
    }
}
