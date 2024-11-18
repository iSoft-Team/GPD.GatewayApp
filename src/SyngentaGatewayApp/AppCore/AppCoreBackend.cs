using GPD_App.Common;
using SuperSimpleTcp;
using SyngentaGatewayApp.Events;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;
using DataReceivedEventArgs = SuperSimpleTcp.DataReceivedEventArgs;

namespace SyngentaGatewayApp.AppCore
{
    public partial class AppCore
    {
        private static AppCore _ins = new AppCore();
        private CommonTCPServer _AppServer { get; set; }
        public event DataReceiveHandler OnDataReceived;
        public event DataSentHandler OnDataSent;
        string IpAddress = Environment.GetEnvironmentVariable("SERVER_IP");
        string Port = Environment.GetEnvironmentVariable("SERVER_PORT");
        public byte[] Data;
        public bool ServerStatus = false;
        public bool PrinterStatus = false;
        private CommonTCPClient _Printer { get; set; }
        public event ClientDataReceiveHandler ClientDataReceive;
        public event ClientDataSentHandler ClientDataSent;
        private bool ConnectPrinterStatus;
        public byte[] PrinterData;
        string PrinterIpAddress = Environment.GetEnvironmentVariable("PRINTER_IP");
        string PrinterPort = Environment.GetEnvironmentVariable("PRINTER_PORT");
        string name = "Printer";

        public static AppCore Ins
        {
            get
            {
                return _ins == null ? _ins = new AppCore() : _ins;
            }
        }
        public AppCore()
        {
            var process = Process.GetProcessesByName($"{System.Reflection.Assembly.GetEntryAssembly().GetName().Name}");
            if (process.Length > 1)
            {
                process[1].Kill();
            }
        }

        public async Task Init()
        {
            StartShowUI();
        }
        







        public async Task InitOpenServer(string host, int port)
        {
            try
            {
                if (_AppServer != null)
                {
                    _AppServer.Dispose();
                }
                _AppServer = new CommonTCPServer();
                if (string.IsNullOrWhiteSpace(host) && string.IsNullOrWhiteSpace(port.ToString()))
                {

                    //host = _AppServer.GetLocalIPAddress();
                    host = IpAddress;
                    port = int.Parse(Port);
                }

                _AppServer.Init(host, port, true);
                //_AppServer.OnClientSend += _AppServer_OnClientSend;
                _AppServer.OnDataReceived += _AppServer_OnDataReceived;
                _AppServer.Start();
                ServerStatus = true;
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace.ToString());
                ServerStatus = false;
                throw ex;
            }
        }

        private async Task _AppServer_OnDataReceived(object? sender, DataReceivedEventArgs bytesReceive)
        {
            try
            {
                string ReceivedDataString = Encoding.ASCII.GetString(bytesReceive.Data.Array);
                byte[] ReceivedDataByte = Encoding.ASCII.GetBytes(ReceivedDataString);
                Data = ReceivedDataByte;
                SendToPrinter(Data);
                //_AppServer.Send(bytesReceive.IpPort, Data);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace.ToString());
                throw ex;
            }
        }

        //private async Task _AppServer_OnClientSend(object sender, DataSentEventArgs bytesSent)
        //{
        //    try
        //    {

        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message + ex.StackTrace.ToString());
        //        throw ex;
        //    }
        //}
        public void StopServer()
        {
            _AppServer.Dispose();
        }
        public void SendToClient(byte[] Data)
        {
            var connection = _AppServer.ListClients();
            foreach (var item in connection)
            {
                _AppServer.Send(item, Data);
            }
        }


        public async void InitConnectPrinter(string host, int port)
        {
            try
            {
                if (_Printer != null)
                {
                    _Printer.Disconnect();
                }
                _Printer = new CommonTCPClient(host, port, name);
                if (string.IsNullOrWhiteSpace(host) && string.IsNullOrWhiteSpace(port.ToString()))
                {
                    host = PrinterIpAddress;
                    port = int.Parse(PrinterPort);
                }
                _Printer.Init(host, port, name);
                _Printer.Connect();
                _Printer.OnDataReceive += _Printer_OnDataReceive;
                PrinterStatus = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace.ToString());
                throw ex;
            }
        }

        private void _Printer_OnDataReceive(object? sender, SuperSimpleTcp.DataReceivedEventArgs e)
        {
            string ReceivedDataString = Encoding.ASCII.GetString(e.Data.Array);
            byte[] ReceivedDataByte = Encoding.ASCII.GetBytes(ReceivedDataString);
            PrinterData = ReceivedDataByte;
            SendToClient(PrinterData);
        }

        public void DisconnectPrinter()
        {
            _Printer.Disconnect();
            PrinterStatus = false;
        }

        public void SendToPrinter(byte[] SendData)
        {
            try
            {
                if (_Printer.Connected)
                {
                    _Printer.Send(SendData);
                }
                else
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace.ToString());
                throw ex;
            }

        }






















    }
}
