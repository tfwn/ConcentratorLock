using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.IO.Ports;
using UsbLibrary;

namespace _8009_Update
{
    public partial class FrmMain : Form
    {
        protected bool bPortOpened = false;
        public static String strComPort = "";
        protected Thread ConnectUsbThread = null;

        private int PortBufRdPos = 0;
        private int PortBufWrPos = 0;
        private byte[] PortRxBuf = new Byte[2000];

        UpdateFun UpdateCtrl = new UpdateFun();

        public delegate void SerialDataRecievedEventHandler(object sender, SerialDataReceivedEventArgs e);

        public FrmMain()
        {
            InitializeComponent();

            FillLabel();

            // 添加组件
            AddPanelContrl();

            // 创建USB线程
            CreateUsbThread();
        }
        private void FillLabel()
        {
            this.Text = "CTP共享停车集中器本地软件升级工具_V1.10";
            //this.lbSelectPort.Text = "选择端口" + ":";
            //this.btOpenPort.Text = "打开端口";
            this.tsUsbStatus.Text = "USB设备未连接!";
            this.tsComStatus.Text = "串口已关闭";
            //this.tsConcentratorAddress.Text = WindowHandler.GetLangValue("CONCENTRATOR_ADDR") + ": " + WindowHandler.GetLangValue("UNKNOW");
            //this.btConfig.Text = WindowHandler.GetLangValue("SYS_CONFIG");
            //this.btConfig.Location = new System.Drawing.Point(900, 10);
           
            //this.tspbFrmMain.Dock = DockStyle.Fill;
        }
        void AddPanelContrl()
        {
            // 增加升级选项卡
            panel.Controls.Add(UpdateCtrl);
            UpdateCtrl.setUartBaudrate = SetUartBaudrate;
            UpdateCtrl.dataTransmit = DataTransmit;
        }
        void CreateUsbThread()
        {
            ConnectUsbThread = new Thread(ConnectUsbWork);
            ConnectUsbThread.IsBackground = true;
            ConnectUsbThread.Priority = ThreadPriority.Normal;
            ConnectUsbThread.Start(); 
        }

        #region 串口处理
        bool _isConnect = false;
        /// <summary>
        /// usb是否连接
        /// </summary>
        public bool isConnect
        {
            set
            {
                _isConnect = value;
                SetConnectStatusText(value);
            }
            get { return _isConnect; }
        }
        protected delegate void SetConnectStatusTextCallBack(bool isSuccess);
        protected void SetConnectStatusText(bool isSuccess)
        {
            if (this.InvokeRequired)
            {
                SetConnectStatusTextCallBack st = new SetConnectStatusTextCallBack(SetConnectStatusText);
                this.Invoke(st, new object[] { isSuccess });
            }
            else
            {
                if (isSuccess)
                {
                    this.tsUsbStatus.Text = "USB设备连接成功!";
                }
                else
                {
                    this.tsUsbStatus.Text = "USB设备未连接!";
                    if (this.cmbPort.Text == "USB" && true == bPortOpened)
                    {
                        MessageBox.Show("USB设备异常断开！", "USB异常", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.btOpenPort.Text = "打开端口";
                        this.cmbPort.Enabled = true;
                        this.bPortOpened = false;
                    }
                }
            }
        }
        /// <summary>
        /// 添加可使用的端口到端口列表中
        /// </summary>
        void InitializePortList()
        {
            cmbPort.Items.Clear();
            foreach (string portName in SerialPort.GetPortNames())
            {
                cmbPort.Items.Add(portName);
            }

            if (true == _isConnect)
            {
                cmbPort.Items.Add("USB");
            }   
        }

        private delegate void SetErrorTextCallBack(string MsgText);
        protected void SetErrorText(string MsgText)
        {
            if (this.InvokeRequired)
            {
                SetErrorTextCallBack st = new SetErrorTextCallBack(SetErrorText);
                this.Invoke(st, new object[] { MsgText });
            }
            else
            {
                //this.tsConnectStatus.Text = MsgText;
            }
        }
        private void cmbPort_Click(object sender, EventArgs e)
        {
            InitializePortList();
        }

        private void btOpenPort_Click(object sender, EventArgs e)
        {
            strComPort = cmbPort.Text;
            if (cmbPort.SelectedIndex < 0 || cmbPort.Text == "")
            {
                MessageBox.Show("请先选择一个通讯端口!", "选择端口", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (strComPort == "USB")
            {
                if (btOpenPort.Text == "打开端口")
                {
                    if (true == _isConnect)
                    {
                        btOpenPort.Text = "关闭端口";
                        cmbPort.Enabled = false;
                        bPortOpened = true;
                    }
                    else
                    {
                        btOpenPort.Text = "打开端口";
                        strComPort = "";
                        cmbPort.Enabled = true;
                        bPortOpened = false;
                        MessageBox.Show("USB设备异常断开！", "USB异常", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    btOpenPort.Text = "打开端口";
                    strComPort = "";
                    cmbPort.Enabled = true;
                    bPortOpened = false;
                }
                return;
            }
            else
            {
                if (btOpenPort.Text == "打开端口")
                {
                    if (true == port_Ctrl(true))
                    {
                        btOpenPort.Text = "关闭端口"; ;
                        cmbPort.Enabled = false;
                        bPortOpened = true;
                    }
                    else
                    {
                        btOpenPort.Text = "打开端口";
                        strComPort = "";
                        cmbPort.Enabled = true;
                        bPortOpened = false;
                    }
                }
                else
                {
                    port_Ctrl(false);
                    btOpenPort.Text = "打开端口";
                    strComPort = "";
                    cmbPort.Enabled = true;
                    bPortOpened = false;
                }
            }
        }

        private bool port_Ctrl(bool ctrl)
        {
            string strBaudrate;

            if (true == ctrl)
            {
                strBaudrate = "115200";
                if (serialPort.IsOpen == false ||
                    serialPort.BaudRate != Convert.ToInt32(strBaudrate) ||
                    serialPort.PortName != cmbPort.Text)
                {
                    try
                    {
                        serialPort.Close();
                        serialPort.BaudRate = Convert.ToInt32(strBaudrate);
                        serialPort.PortName = strComPort;
                        serialPort.Open();
                        tsComStatus.Text = strComPort + ": " + strBaudrate + ", " + "Even" + ", " + "8, 1";
                        return true;
                    }
                    catch (System.Exception ex)
                    {
                        tsComStatus.Text = "串口已关闭";
                        MessageBox.Show("打开串口失败" + "," + ex.Message, "打开串口失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
            }
            else
            {
                try
                {
                    serialPort.DiscardInBuffer();
                    serialPort.DiscardOutBuffer();
                    serialPort.Close();
                    tsComStatus.Text = "串口已关闭";
                    return true;
                }
                catch (System.Exception ex)
                {
                    tsComStatus.Text = "串口已关闭";
                    MessageBox.Show("关闭串口失败" + "," + ex.Message, "串口错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            return true;
        }
        private void serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            int iRead;

            if (InvokeRequired)
            {
                try
                {
                    Invoke(new SerialDataRecievedEventHandler(serialPort_DataReceived), new object[] { sender, e });
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
            else
            {
                try
                {
                    while (serialPort.BytesToRead > 0)
                    {
                        iRead = serialPort.ReadByte();
                        PortRxBuf[PortBufWrPos] = (byte)iRead;
                        PortBufWrPos = (PortBufWrPos + 1) % PortRxBuf.Length;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }

        private void serialPort_DataTransmit(byte[] buf, int len)
        {
            try
            {
                serialPort.Write(buf, 0, len);
            }
            catch (System.Exception ex)
            {
                //AddMsg("\n串口通信出现异常，" + ex.Message);
            }
        }
        #endregion

        #region USB端口
        void ConnectUsbWork()
        {
            while (true)
            {
                ConnectUsb();
                Thread.Sleep(2000);
            }
        }

        private delegate void ConnectUsbCallBack(); //连接USB
        protected void ConnectUsb()
        {
            if (this.InvokeRequired)//等待异步
            {
                ConnectUsbCallBack fc = new ConnectUsbCallBack(ConnectUsb);
                this.Invoke(fc); //通过代理调用刷新方法
            }
            else
            {
                try
                {
                    SetErrorText("");
                    this.usbHidPort.ProductId = Convert.ToInt32("5750", 16);
                    this.usbHidPort.VendorId = Convert.ToInt32("0483", 16);
                    this.usbHidPort.CheckDevicePresent();
                }
                catch (Exception ex)
                {
                    SetErrorText(ex.ToString());
                }
            }
        }

        private void usbHidPort_OnSpecifiedDeviceArrived(object sender, EventArgs e)
        {
            isConnect = true;
        }

        private void usbHidPort_OnSpecifiedDeviceRemoved(object sender, EventArgs e)
        {
            isConnect = false;
        }

        private void usbHidPort_OnDataRecieved(object sender, DataRecievedEventArgs args)
        {
            int loop;

            if (InvokeRequired)
            {
                try
                {
                    Invoke(new DataRecievedEventHandler(usbHidPort_OnDataRecieved), new object[] { sender, args });
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
            else
            {
                for (loop = 0; loop < 64; loop++)
                {
                    PortRxBuf[PortBufWrPos] = args.data[loop + 1];
                    PortBufWrPos = (PortBufWrPos + 1) % PortRxBuf.Length;
                }
            }
        }

        public void usbHidPort_OnDataTransmit(byte[] buf, int len)
        {
            byte[] usbBuf = new byte[65];
            byte loop = 0;

            if (isConnect)
            {
                loop = (byte)((len + 1) / 64);
                if (((len + 1) % 64) != 0)
                {
                    loop++;
                }
                if (this.usbHidPort.SpecifiedDevice != null)
                {
                    for (byte i = 0; i < loop; i++)
                    {
                        Array.Clear(usbBuf, 0, usbBuf.Length);
                        for (int j = 0; j < 64; j++)
                        {
                            if (i * 64 + j >= buf.Length)
                            {
                                break;
                            }
                            usbBuf[j + 1] = buf[i * 64 + j];
                        }
                        this.usbHidPort.SpecifiedDevice.SendData(usbBuf);
                    }
                }
            }
            else
            {
                //MessageBox.Show(WindowHandler.GetLangValue("USBNOTCONNECTED"),
                //             Util.SystemTitle,
                //              MessageBoxButtons.OK,
                //             MessageBoxIcon.Error
                //              );
                //MessageBoxEx.Show(WindowHandler.GetLangValue("USBNOTCONNECTED"),
                //       Util.SystemTitle, MessageBoxIcon.Error);
            }
        }
        #endregion

        void Switch_Explain(byte[] RxBuf)
        {
            UpdateCtrl.ExplainPacket(RxBuf);
            return;
        }

        private void timerMonitor_Tick(object sender, EventArgs e)
        {
            int len, sum, loop;

            while (true)
            {
                len = (PortBufWrPos >= PortBufRdPos) ? (PortBufWrPos - PortBufRdPos) : (PortRxBuf.Length - PortBufRdPos + PortBufWrPos);
                if (len < 4)
                {
                    break;
                }
                // Boot下升级时调用
                if (UpdateCtrl.UpdateStep == UpdateFun.enUpdateProc.Boot_EnterUpdate || UpdateCtrl.UpdateStep == UpdateFun.enUpdateProc.Boot_UpdateInProcing)
                {
                    if (PortRxBuf[PortBufRdPos % PortRxBuf.Length] != UpdateFun.SyncWord1 || PortRxBuf[(PortBufRdPos + 1) % PortRxBuf.Length] != UpdateFun.SyncWord2)
                    {
                        PortBufRdPos = (UInt16)((PortBufRdPos + 1) % PortRxBuf.Length);
                        continue;
                    }
                    if (PortRxBuf[(PortBufRdPos + 2) % PortRxBuf.Length] > 50)
                    {
                        PortBufRdPos = (UInt16)((PortBufRdPos + 1) % PortRxBuf.Length);
                        continue;
                    }
                    if (len < PortRxBuf[(PortBufRdPos + 2) % PortRxBuf.Length] + 5)
                    {
                        break;
                    }
                    sum = PortRxBuf[(PortBufRdPos + 2) % PortRxBuf.Length] + 5;
                }
                else
                {
                    if (PortRxBuf[PortBufRdPos % PortRxBuf.Length] != 0xD3 || PortRxBuf[(PortBufRdPos + 1) % PortRxBuf.Length] != 0x91)
                    {
                        PortBufRdPos = (UInt16)((PortBufRdPos + 1) % PortRxBuf.Length);
                        continue;
                    }
                    sum = PortRxBuf[(PortBufRdPos + 2) % PortRxBuf.Length] + (PortRxBuf[(PortBufRdPos + 3) % PortRxBuf.Length] & 0x03) * 256 + 2;
                    if (sum > 900)
                    {
                        PortBufRdPos = (UInt16)((PortBufRdPos + 1) % PortRxBuf.Length);
                        continue;
                    }
                    if (len < sum + 2)
                    {
                        break;
                    }
                    sum += 2;   // 包含0xD3 0x91两个字节
                }
                byte[] rxBuf = new Byte[sum];
                for (loop = 0; loop < sum; loop++)
                {
                    rxBuf[loop] = PortRxBuf[(PortBufRdPos + loop) % PortRxBuf.Length];
                }
                PortBufRdPos = (PortBufRdPos + loop) % PortRxBuf.Length;
                Switch_Explain(rxBuf);
            }
        }
        public void DataTransmit(byte[] dataBuf)
        {
            if (strComPort == "USB")
            {
                usbHidPort_OnDataTransmit(dataBuf, dataBuf.Length);
            }
            else
            {
                serialPort_DataTransmit(dataBuf, dataBuf.Length);
            }
        }

        public Int32 SetUartBaudrate(Int32 NewBaudrate)
        {
            Int32 iOldBaudrate = 0;

            if (NewBaudrate == serialPort.BaudRate)
            {
                return NewBaudrate;
            }
            try
            {
                iOldBaudrate = serialPort.BaudRate;
                if (serialPort.IsOpen == true)
                {
                    serialPort.Close();
                }
                serialPort.BaudRate = Convert.ToInt32(NewBaudrate);
                serialPort.PortName = strComPort;
                serialPort.Open();
                return iOldBaudrate;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return 0;
        }
    }
}
