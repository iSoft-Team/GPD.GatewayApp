﻿using SuperSimpleTcp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyngentaGatewayApp.Events
{
    // TCP server event
    public delegate Task DataSentHandler(object sender, DataSentEventArgs bytesSent);
    public delegate Task DataReceiveHandler(object sender, DataReceivedEventArgs bytesReceive);
    public delegate Task DataBackupHandler(object sender, DataReceivedEventArgs bytesReceive);
    // TCP client event
    public delegate Task ClientDataSentHandler(object sender, ConnectionEventArgs eventHandler);
    public delegate Task ClientDataReceiveHandler(object sender, DataReceivedEventArgs bytesReceive);
}
