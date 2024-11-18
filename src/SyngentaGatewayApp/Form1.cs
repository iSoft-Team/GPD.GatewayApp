
using SyngentaGatewayApp.AppCore;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SyngentaGatewayApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        #region Singleton parttern
        private static Form1 _Instance = null;
        public AppCore.AppCore app = new AppCore.AppCore();
        public static Form1 Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new Form1();
                }
                return _Instance;
            }
        }
        #endregion  

        private async void btnOpenSer_Click(object sender, EventArgs e)
        {
            app.InitOpenServer(txtServerIP.Text,int.Parse(txtServerPort.Text));
            if (app.ServerStatus) 
            {
                pnAppSerStatus.BackColor = Color.Green;
            }
            else
            {
                pnAppSerStatus.BackColor= Color.Red;
            }
        }
        private void btnCloseSer_Click(object sender, EventArgs e)
        {
            if (app.ServerStatus)
            {
                app.StopServer();
            }
            else 
            {
                return;
            }
            if (app.ServerStatus)
            {
                pnAppSerStatus.BackColor = Color.Green;
            }
            else
            {
                pnAppSerStatus.BackColor = Color.Red;
            }

        }
        private void bntServerRead_Click(object sender, EventArgs e)
        {
            
        }

        private void btnServerSend_Click(object sender, EventArgs e)
        {
            
        }



        private void btnConnectPrinter_Click(object sender, EventArgs e)
        {
            app.InitConnectPrinter(txtPrinterIP.Text,int.Parse(txtPrinterPort.Text));
            if (app.PrinterStatus)
            {
                pnPrinterStatus.BackColor = Color.Green;
            }
            else
            {
                pnPrinterStatus.BackColor = Color.Red;
            }
        }
        private void btnDisconnectPrinter_Click(object sender, EventArgs e)
        {
            if (app.PrinterStatus)
            {
                app.DisconnectPrinter();
            }
            else
            {
                return;
            }
            if (app.PrinterStatus)
            {
                pnPrinterStatus.BackColor = Color.Green;
            }
            else
            {
                pnPrinterStatus.BackColor = Color.Red;
            }
        }
        private void btnSendPrinter_Click(object sender, EventArgs e)
        {
        }

        private void btnReadPrinter_Click(object sender, EventArgs e)
        {
            if(app.PrinterData == null) return;
            txtServerRead.Text = Encoding.ASCII.GetString(app.PrinterData);
        }


    }
}
