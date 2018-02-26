using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
//using Common;
using _8009_Update;

namespace _8009_Update
{
    public partial class UpdateFun : UserControl
    {
        // 升级文件参数
        string strUpdateFileName;                           // 升级文件的名字
        string strUpdateInfo;                               // 升级文件信息
        UInt16 updateCrc;                                   // 升级文件的CRC校验值
        private int updateLength = 0;                       // 升级文件大小
        const UInt32 updateBufSize = 100 * 1024;            // 升级文件的缓冲区，最大100k
        byte[] updateBuf = new byte[updateBufSize];         // 升级文件缓冲区  
        public enum enUpdateProc
        {
            Boot_EnterUpdate = 0,
            Boot_UpdateInProcing,
            App_UpdateInProcing,
            Update_End,
            Read_Version,
        };
        public enum enUpdateType
        {
            地锁系统 = 0,
            无线模块系统 = 1,
            未知集中器,
        };
        public enUpdateProc UpdateStep = enUpdateProc.Update_End;
        private int UpdateRetryTimes;
        private int UpdateWaitTime;
        public delegate void DataTransmit(byte[] buf);
        public DataTransmit dataTransmit;
        public static byte FrameSn = 0;                     // 帧序号
        // App下升级
        public static enUpdateType UpdateType = enUpdateType.未知集中器;
        public static byte NodeAddrLength = 0;
        public static byte AppSyncWord1 = 0x68;             // APP通信同步字1
        public static byte AppSyncWord2 = 0x68;             // APP通信同步字2
        private int AppUpdateWriteAddress;                  // 当前升级的写入位置
        private int AppUpdateCurPkgBytes;                   // 当前升级包字节数
        const int AppUpdateCountOnePacket = 456;            // APP下升级每包字节数
        // Boot下升级
        public const byte SyncWord1 = 0x55;
        public const byte SyncWord2 = 0xAA;
        Int32 iLastPortBaudrate = 115200;                   // Boot下升级串口的波特率
        public delegate Int32 SetUartBaudrate(Int32 strNewBaudrate);
        public SetUartBaudrate setUartBaudrate;
        private int BootUpdateTotalPkg;                     // 总包数
        private int BootUpdateCurrentPkg;                   // 当前包数
        const int BootUpdateCountOnePacket = 128;           // BOOT下升级每包字节数
        private int BootUpdateLastDownCounter;              // 倒计时
        private string strDirectory = "";

        public UpdateFun()
        {
            InitializeComponent();

            FillLabel();
        }
        private void FillLabel()
        {
            //this.gpSelectFile.Text = WindowHandler.GetLangValue("SELECT_UPDATE_FILE");
            //this.lbUpdateFilename.Text = WindowHandler.GetLangValue("UPDATE_FILENAME");
            //this.btOpenUpdateFile.Text = WindowHandler.GetLangValue("OPEN_FILE");
            //this.btRdVersionInfo.Text = WindowHandler.GetLangValue("READ_VERSION");
            //this.lbUpdateInfo.Text = WindowHandler.GetLangValue("FILE_INFO");
            //this.lbCrcVal.Text = WindowHandler.GetLangValue("CRC_VALUE");

            //this.gpSelectMode.Text = WindowHandler.GetLangValue("SELECT_UPDATE_MODE");
            //this.rbtBootUpdate.Text = WindowHandler.GetLangValue("BOOT_UPDATE");
            //this.rbtAppUpdate.Text = WindowHandler.GetLangValue("APP_UPDATE");
            //this.btStartUpdate.Text = WindowHandler.GetLangValue("START_UPDATE");
        }

        #region 事件处理部分
        private void btOpenUpdateFile_Click(object sender, EventArgs e)
        {
            //string strDirectory = Common.XmlHelper.GetNodeDefValue(Util.SystemConfigPath, "/Config/UpdateFun/UpdateFileDir", Application.StartupPath);
            if (strDirectory == "")
            {
                strDirectory = Application.StartupPath;
            }
            openUpdateFileDialog.Filter = "bin文件(*.bin)|*.bin|所有文件(*.*)|*.*";
            openUpdateFileDialog.DefaultExt = "*.bin";
            openUpdateFileDialog.FileName = "";
            openUpdateFileDialog.InitialDirectory = strDirectory;
            if (openUpdateFileDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            strDirectory = openUpdateFileDialog.FileName;
            strUpdateFileName = openUpdateFileDialog.FileName;
            loadUpdateBuf();
        }
        private void rbt_Click(object sender, EventArgs e)
        {
            RadioButton rbt = (RadioButton)sender;

            if (rbt.Name == "rbt6009")
            {
                UpdateType = enUpdateType.地锁系统;
                NodeAddrLength = NodeAddrLength_6009;
            }
            else if (rbt.Name == "rbt2e28")
            {
                UpdateType = enUpdateType.无线模块系统;
                NodeAddrLength = NodeAddrLength_6009;
            }
            else
            {
                UpdateType = enUpdateType.未知集中器;
            }
        }
        private void btStartUpdate_Click(object sender, EventArgs e)
        {
            if (btStartUpdate.Text == "停止升级")
            {
                UpdateStep = enUpdateProc.Update_End;
                timerUpdate.Stop();
                MessageBox.Show("升级进程被终止！", "升级中断", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                btStartUpdate.Text = "开始升级";
                btOpenUpdateFile.Enabled = true;
                btRdVersionInfo.Enabled = true;
                rbtBootUpdate.Enabled = true;
                rbtAppUpdate.Enabled = true;
                gpSelectType.Enabled = true;
                return;
            }
            if (FrmMain.strComPort == "")
            {
                MessageBox.Show("请先打开通讯端口！", "打开端口", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            loadUpdateBuf();
            if (updateLength == 0)
            {
                MessageBox.Show("请选择有效的升级文件！", "无效文件", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (FrmMain.strComPort == "")
            {
                MessageBox.Show("请先打开通讯端口！", "打开端口", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (rbtBootUpdate.Checked == true)
            {
                if (FrmMain.strComPort == "USB")
                {
                    MessageBox.Show("Boot下升级只能在串口下进行！", "端口错误", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
                iLastPortBaudrate = setUartBaudrate(115200);
                if (iLastPortBaudrate == 0)
                {
                    MessageBox.Show("打开端口出现错误！", "端口错误", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                UpdateStep = enUpdateProc.Boot_EnterUpdate;
                UpdateRetryTimes = 1000;
                BootUpdateLastDownCounter = 10 * (1000 / timerUpdate.Interval) + 5;
                btStartUpdate.Enabled = false;
            }
            else
            {
                UpdateStep = enUpdateProc.App_UpdateInProcing;
                AppUpdateWriteAddress = 0;
                UpdateRetryTimes = 5;
            }
            btOpenUpdateFile.Enabled = false;
            btRdVersionInfo.Enabled = false;
            rbtBootUpdate.Enabled = false;
            rbtAppUpdate.Enabled = false;
            gpSelectType.Enabled = false;
            btStartUpdate.Text = "停止升级";
            // 关闭监控功能
            UpdateWaitTime = 3;
            timerUpdate.Start();
        }
        private void btRdVersionInfo_Click(object sender, EventArgs e)
        {
            if (FrmMain.strComPort == "")
            {
                MessageBox.Show("请先打开通讯端口！", "串口错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            UpdateStep = enUpdateProc.Read_Version;
            UpdateRetryTimes = 3;
            btRdVersionInfo.Enabled = false;
            btStartUpdate.Enabled = false;
            gpSelectType.Enabled = false;
            UpdateWaitTime = 1;
            timerUpdate.Start();
        }
        #endregion

        #region 校验处理
        public static byte comCalCRC8(byte[] dataBuf, int pos, int dataLen, bool flag = false)
        {
            if (flag == false)
            {
                byte crc8 = 0;

                while (dataLen-- > 0)
                {
                    crc8 += dataBuf[pos++];
                }
                return crc8;
            }
            else
            {
                byte i, cCrc = 0;
                for (int m = 0; m < dataLen; m++)
                {
                    cCrc ^= dataBuf[m + pos];
                    for (i = 0; i < 8; i++)
                    {
                        if ((cCrc & 0x01) != 0)
                        {
                            cCrc >>= 1;
                            cCrc ^= 0x8c;
                        }
                        else
                        {
                            cCrc >>= 1;
                        }
                    }
                }
                return cCrc;
            }
        }
        public static UInt16 calCrc16(Byte[] buf, UInt32 start, UInt32 len)
        {
            UInt16 crc16 = 0xFFFF;
            UInt32 iLoop;
            UInt32 iLoop1;

            for (iLoop = 0; iLoop < len; iLoop++)
            {
                crc16 ^= buf[start + iLoop];
                for (iLoop1 = 0; iLoop1 < 8; iLoop1++)
                {
                    if ((crc16 & 1) == 1)
                    {
                        crc16 >>= 1;
                        crc16 ^= 0x8408;
                    }
                    else
                    {
                        crc16 >>= 1;
                    }
                }
            }
            crc16 ^= 0xffff;
            return crc16;
        }

        private static int __CalCRC16(byte[] dataBuf, int pos, int dataLen)
        {
            int i, cCrc = 0xffff;
            for (int m = 0; m < dataLen; m++)
            {
                cCrc ^= dataBuf[m + pos];
                for (i = 0; i < 8; i++)
                {
                    if ((cCrc & 0x0001) != 0)
                    {
                        cCrc >>= 1;
                        cCrc ^= 0x8408;
                    }
                    else
                    {
                        cCrc >>= 1;
                    }
                }
            }
            cCrc ^= 0xFFFF;
            return cCrc;
        }
        #endregion

        #region 地锁集中器
        public const byte SyncWord1_6009 = 0xD3;
        public const byte SyncWord2_6009 = 0x91;
        // 节点地址的长度
        public const int NodeAddrLength_6009 = 8;// 6;
        public struct Protocol6009Struct
        {
            public int PacketLength;                // 接收到消息长度
            public byte PacketProperty;             // 报文标识
            public byte TaskIDByte;                 // 任务号字节
            public byte CommandIDByte;              // 命令字节
            public byte DeviceTypeByte;             // 设备类型字节
            public byte LifeCycleByte;              // 生命周期字节
            public byte RouteLevelByte;             // 路径级数信息
            public byte RouteCurPos;                // 路由当前位置
            public byte[] RoutePathList;            // 传输路径列表
            public byte[] DataByte;                 // 数据域字节
            public byte DownSignalStrengthByte;     // 下行信号强度
            public byte UpSignalStrengthByte;       // 上行信号强度
            //public byte Crc8Byte;                   // 校验字
            public int Crc16Byte;                   // 校验字
            public byte EndByte;                    // 结束符
            public bool isSuccess;                  // 是否成功
        }
        public static Protocol6009Struct Extract6009RxBuffer(byte[] Buffer)
        {
            Protocol6009Struct proStruct = new Protocol6009Struct();
            int iLoop, packPos, startPos;
            //byte calCrc;
            int calCrc;

            try
            {
                if (Buffer[0] == SyncWord1_6009 && Buffer[1] == SyncWord2_6009)
                {
                    startPos = 2;
                }
                else
                {
                    startPos = 0;
                }
                packPos = startPos;
                // 包长度
                proStruct.PacketLength = Buffer[packPos++] + (Buffer[packPos++] & 0x03) * 256;
                //报文标识
                proStruct.PacketProperty = Buffer[packPos++];
                // 任务号
                proStruct.TaskIDByte = Buffer[packPos++];
                // 命令字
                proStruct.CommandIDByte = Buffer[packPos++];
                // 设备类型
                proStruct.DeviceTypeByte = Buffer[packPos++];
                // 生命周期
                proStruct.LifeCycleByte = Buffer[packPos++];
                // 路径条数
                proStruct.RouteLevelByte = (Byte)(Buffer[packPos] & 0x0F);
                // 当前位置
                proStruct.RouteCurPos = (byte)(Buffer[packPos] >> 4 & 0x0F);
                // 传输路径
                packPos++;
                proStruct.RoutePathList = new byte[proStruct.RouteLevelByte * NodeAddrLength];
                for (iLoop = 0; iLoop < proStruct.RoutePathList.Length; iLoop++)
                {
                    proStruct.RoutePathList[iLoop] = Buffer[packPos++];
                }
                // 数据域
                proStruct.DataByte = new byte[proStruct.PacketLength - 5 - packPos + startPos];
                for (iLoop = 0; iLoop < proStruct.DataByte.Length; iLoop++)
                {
                    proStruct.DataByte[iLoop] = Buffer[packPos++];
                }
                // 下行信号强度
                proStruct.DownSignalStrengthByte = Buffer[packPos++];
                // 上行信号强度
                proStruct.UpSignalStrengthByte = Buffer[packPos++];
                // 校验字
                //proStruct.Crc8Byte = Buffer[packPos++];
                proStruct.Crc16Byte = ((Buffer[packPos++] << 8) & 0xff00) | (Buffer[packPos++] & 0xFF);
                // 结束字
                proStruct.EndByte = Buffer[packPos++];
                // 是否成功
                //calCrc = comCalCRC8(Buffer, startPos, proStruct.PacketLength - 2, true);
                //proStruct.isSuccess = (calCrc == proStruct.Crc8Byte) ? true : false;
                calCrc = __CalCRC16(Buffer, startPos, proStruct.PacketLength - 3);
                proStruct.isSuccess = (calCrc == proStruct.Crc16Byte) ? true : false;

                return proStruct;
            }
            catch
            {
                proStruct.isSuccess = false;
                return proStruct;
            }
        }
        public static byte[] _6009CreatTxBuffer(Protocol6009Struct txStruct)
        {
            int crc16;
            //byte bCrc;
            int iLen = 0;
            try
            {
                byte[] buf = new byte[1500];
                // 同步字
                buf[iLen++] = SyncWord1_6009;
                buf[iLen++] = SyncWord2_6009;
                // 包长度,后面填充
                iLen += 2;
                // 报文标识:下行 命令帧 不加密 需要回执
                buf[iLen++] = 0x10;
                // 帧序号
                buf[iLen++] = FrameSn++;
                // 命令字
                buf[iLen++] = txStruct.CommandIDByte;
                // 设备类型
                buf[iLen++] = 0xFA;     // 上位机
                // 生命周期
                buf[iLen++] = 0x0F;
                // 传输路径
                buf[iLen++] = 0x02;
                // 设置地址
                for (int i = 0; i < NodeAddrLength; i++)
                {
                    buf[iLen++] = 0xFA;
                }
                for (int i = 0; i < NodeAddrLength; i++)
                {
                    buf[iLen++] = 0xD5;
                }
                // 数据域
                if (txStruct.DataByte != null)
                {
                    Array.Copy(txStruct.DataByte, 0, buf, iLen, txStruct.DataByte.Length);
                    iLen += txStruct.DataByte.Length;
                }
                // 上行信号强度
                buf[iLen++] = 0;
                // 下行信号强度
                buf[iLen++] = 0;
                // 包长度
                //buf[2] = (byte)(iLen);
                //buf[3] = (byte)(iLen >> 8);
                buf[2] = (byte)(iLen + 1);
                buf[3] = (byte)((iLen + 1) >> 8);
                // 校验字
                //bCrc = comCalCRC8(buf, 2, iLen - 2, true);
                //buf[iLen++] = bCrc;
                crc16 = __CalCRC16(buf, 2, iLen - 2);
                buf[iLen++] = (byte)((crc16 >> 8) & 0xFF);
                buf[iLen++] = (byte)((crc16) & 0xFF);

                // 结束符
                buf[iLen++] = 0x16;
                // 其他参数
                //buf[iLen++] = 0x00;
                //buf[iLen++] = 9 + 16;
                //if (Util.Protocol == enProtocol._6009)
                //{
                //    buf[iLen++] = 3;
                //}
                // 赋给另外一个数组
                byte[] txBuf = new byte[iLen];
                Array.Copy(buf, txBuf, iLen);
                return txBuf;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return null;
        }
        #endregion

        #region 文件处理部分
        private void loadUpdateBuf()
        {
            int length;

            length = loadUpdateFile();
            if (length > 0)
            {
                if (rbtAppUpdate.Checked == true)
                {
                    length = findBootFlagofUpdateBuf(length);
                    bootUpdateFileCheck(length);
                }
                else
                {
                    length = addHeadofUpdatebuf(length);
                    length = findBootFlagofUpdateBuf(length);
                    bootUpdateFileCheck(length);
                }
                if (updateLength > 0)
                {
                    tbUpdateFile.Text = strUpdateFileName;
                    tbUpdateFileInfo.ForeColor = System.Drawing.SystemColors.WindowText;
                    tbUpdateFileInfo.BackColor = System.Drawing.SystemColors.Window;
                    tbUpdateFileInfo.Text = strUpdateInfo;
                    tbCrcVal.Text = updateCrc.ToString("X4");
                    //Common.XmlHelper.SetNodeValue(Util.SystemConfigPath, "/Config/UpdateFun", "UpdateFileDir", strUpdateFileName);
                    return;
                }
            }
            tbUpdateFile.Text = strUpdateFileName;
            tbUpdateFileInfo.ForeColor = Color.Red;
            tbUpdateFileInfo.BackColor = Color.Yellow;
            tbUpdateFileInfo.Text = "无效的文件";
            tbCrcVal.Text = "";
            prgBarUpdate.Visible = false;
            return;
        }
        private int loadUpdateFile()
        {
            FileStream file;
            BinaryReader binRd;
            UInt32 iLoop;
            int length;

            updateLength = 0;
            if (strUpdateFileName == "")
            {
                return -1;
            }

            try
            {
                file = new FileStream(strUpdateFileName, FileMode.Open, FileAccess.Read);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("打开升级文件" + " " + strUpdateFileName + " " + "失败" + "!", "打开文件失败",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                return -1;
            }
            binRd = new BinaryReader(file);
            length = (int)file.Length;
            if (length > updateBufSize)
            {
                MessageBox.Show("升级文件超出范围" + "(" + length.ToString("D") + "字节 > " + updateBufSize.ToString("D") + "字节)！)",
                                "文件太大", MessageBoxButtons.OK, MessageBoxIcon.Error);
                binRd.Close();
                file.Close();
                return -2;
            }
            for (iLoop = 0; iLoop < updateBufSize; iLoop++)
            {
                updateBuf[iLoop] = 0xFF;
            }
            binRd.Read(updateBuf, 0, length);
            binRd.Close();
            file.Close();
            return length;
        }
        private int findBootFlagofUpdateBuf(int length)
        {
            string keyWord = "6009*Boot";
            int iLoop1, iLoop2, start;

            if (length == 0)
            {
                return length;
            }
            if (rbtAppUpdate.Checked == true)
            {
                start = 0;
            }
            else
            {
                start = 0x80;
            }
            for (iLoop1 = start; iLoop1 < length - keyWord.Length; iLoop1++)
            {
                for (iLoop2 = 0; iLoop2 < keyWord.Length; iLoop2++)
                {
                    if (updateBuf[iLoop1 + iLoop2] != keyWord[iLoop2])
                    {
                        break;
                    }
                }
                if (iLoop2 >= keyWord.Length)
                {
                    MessageBox.Show("文件中含有boot，不能升级！", "禁止升级", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 0;
                }
            }
            return length;
        }
        private int addHeadofUpdatebuf(int length)
        {
            int iLoop;
            UInt16 totalPkg, crc16, startPos;
            byte[] tempBuf = new byte[updateBufSize];

            startPos = 0;
            for (iLoop = 0; iLoop < length - startPos; iLoop++)
            {
                tempBuf[iLoop] = updateBuf[iLoop + startPos];
            }
            length -= startPos;
            for (iLoop = 0; iLoop < 0x80; iLoop++)
            {
                updateBuf[iLoop] = 0;
            }
            totalPkg = (UInt16)((length + 127) / 128);
            updateBuf[2] = (byte)totalPkg;
            updateBuf[3] = (byte)(totalPkg >> 8);
            for (iLoop = 0; iLoop < length; iLoop++)
            {
                updateBuf[iLoop + 0x80] = tempBuf[iLoop];
            }
            for (; iLoop < totalPkg * 128; iLoop++)
            {
                updateBuf[iLoop + 0x80] = 0xFF;
            }
            crc16 = calCrc16(updateBuf, 0x80, (uint)(totalPkg * 128));
            updateBuf[4] = (byte)crc16;
            updateBuf[5] = (byte)(crc16 >> 8);
            updateCrc = crc16;
            return (totalPkg * 128 + 0x80);
        }
        private void bootUpdateFileCheck(int length)
        {
            UInt16 crc16;

            if (length == 0)
            {
                updateLength = 0;
                return;
            }
            if (rbtBootUpdate.Checked == true)
            {
                crc16 = calCrc16(updateBuf, 0x80, (UInt32)(length - 0x80));
                if (crc16 != updateCrc)
                {
                    updateLength = 0;
                    MessageBox.Show("升级文件CRC校验错误！", "CRC错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else
            {
                updateCrc = calCrc16(updateBuf, 0, (UInt32)length);
            }
            if (true == findKeyword(length))
            {
                updateLength = length;
            }
            else
            {
                updateLength = 0;
            }
        }
        private bool findKeyword(int length)
        {
            int iLoop1, iLoop2, start;
            int startPos, count;
            string keyWord1 = "xxxxxxx";
            string keyWord2 = "-Vsp";
            char[] keyWord;

            if (UpdateType == enUpdateType.地锁系统)
            {
                keyWord1 = "SRWF-CTP-PARKING-";
            }
            else if (UpdateType == enUpdateType.无线模块系统)
            {
                keyWord1 = "SRWF-2E28-";
            }
            keyWord = keyWord1.ToCharArray();
            startPos = 0;
            count = 0;
            start = 0;
            if (rbtBootUpdate.Checked == true)
            {
                start = 0x80;
            }
            for (iLoop1 = start; iLoop1 < length; iLoop1++)
            {
                for (iLoop2 = 0; iLoop2 < keyWord.Length; iLoop2++)
                {
                    if (updateBuf[iLoop1 + iLoop2] != keyWord[iLoop2])
                    {
                        break;
                    }
                }
                if (iLoop2 >= keyWord.Length)
                {
                    if (startPos == 0)
                    {
                        startPos = iLoop1;
                        count = 0;
                        iLoop1 += keyWord.Length;
                        keyWord = keyWord2.ToCharArray();
                    }
                    else
                    {
                        strUpdateInfo = System.Text.Encoding.Default.GetString(updateBuf, startPos, 50);
                        return true;
                    }
                }
                if (startPos > 0)
                {
                    if (++count > 45)
                    {
                        startPos = 0;
                        keyWord = keyWord1.ToCharArray();
                    }
                }
            }
            MessageBox.Show("没有找到软件版本，请检查升级文件是否正确！", "版本未知", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }
        #endregion
        
        #region 通信处理
        private void timerUpdate_Tick(object sender, EventArgs e)
        {
            UpdateBootDownCount();
            if (UpdateWaitTime > 0)
            {
                UpdateWaitTime--;
                return;
            }
            update_Transmit();
        }
        private void update_Transmit()
        {
            if (UpdateStep == enUpdateProc.Read_Version)
            {
                TransmitReadVerPacket();
            }
            else if (rbtBootUpdate.Checked == true)
            {
                TransmitBootPacket();
            }
            else
            {
                TransmitAppPacket();
            }
        }
        public void ExplainPacket(byte[] RecBuf)
        {
            if (UpdateStep == enUpdateProc.Read_Version)
            {
                ExplainReadVerPacket(RecBuf);
            }
            else if (RecBuf[0] == 0x68 && RecBuf[3] == 0x68)
            {
                ExplainAppPacket(RecBuf);
            }
            else if (RecBuf[0] == 0xD3 && RecBuf[1] == 0x91)
            {
                ExplainAppPacket(RecBuf);
            }
            else if (rbtBootUpdate.Checked == true)
            {
                ExplainBootPacket(RecBuf);
            }
        }
        #endregion

        #region READ_VERION部分
        private void TransmitReadVerPacket()
        {
            int iLoop;

            if (UpdateStep != enUpdateProc.Read_Version)
            {
                return;
            }
            if (UpdateRetryTimes == 0)
            {
                timerUpdate.Stop();
                UpdateStep = enUpdateProc.Update_End;
                MessageBox.Show("读版本错误，主板没有应答！", "读版本错误", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                btRdVersionInfo.Enabled = true;
                btStartUpdate.Enabled = true;
                gpSelectType.Enabled = true;
                return;
            }
            if (UpdateType == enUpdateType.地锁系统)
            {
                Protocol6009Struct txStruct = new Protocol6009Struct();
                txStruct.CommandIDByte = 0x40;
                // 发送数据
                dataTransmit(_6009CreatTxBuffer(txStruct));
            }
            else if (UpdateType == enUpdateType.无线模块系统)
            {
                Protocol6009Struct txStruct = new Protocol6009Struct();
                txStruct.CommandIDByte = 0x40;
                // 发送数据
                dataTransmit(_6009CreatTxBuffer(txStruct));
            }
            else
            {
                timerUpdate.Stop();
                UpdateStep = enUpdateProc.Update_End;
                btRdVersionInfo.Enabled = true;
                btStartUpdate.Enabled = true;
                gpSelectType.Enabled = true;
                MessageBox.Show("请选择一个集中器类型");
                return;
            }
            UpdateWaitTime = 80;
            UpdateRetryTimes--;
        }
        private void ExplainReadVerPacket(byte[] RecBuf)
        {
            string strVer = "";

            if (UpdateType == enUpdateType.地锁系统)
            {
                Protocol6009Struct rxStruct = Extract6009RxBuffer(RecBuf);
                if (false == rxStruct.isSuccess || rxStruct.CommandIDByte != 0x40)
                {
                    return;
                }
                strVer = "软件版本" + ": " + rxStruct.DataByte[0].ToString("X2") + "." + rxStruct.DataByte[1].ToString("X2") + "\n";
                strVer += "硬件版本" + ": " + rxStruct.DataByte[2].ToString("X2") + "." + rxStruct.DataByte[3].ToString("X2") + "\n";
                strVer += "协议版本" + ": " + rxStruct.DataByte[4].ToString("X2") + "." + rxStruct.DataByte[5].ToString("X2");
            }
            else if (UpdateType == enUpdateType.无线模块系统)
            {
                Protocol6009Struct rxStruct = Extract6009RxBuffer(RecBuf);
                if (false == rxStruct.isSuccess || rxStruct.CommandIDByte != 0x40)
                {
                    return;
                }
                strVer = "软件版本" + ": " + rxStruct.DataByte[0].ToString("X2") + "." + rxStruct.DataByte[1].ToString("X2") + "\n";
                strVer += "硬件版本" + ": " + rxStruct.DataByte[2].ToString("X2") + "." + rxStruct.DataByte[3].ToString("X2") + "\n";
                strVer += "协议版本" + ": " + rxStruct.DataByte[4].ToString("X2") + "." + rxStruct.DataByte[5].ToString("X2");
            }
            timerUpdate.Stop();
            UpdateStep = enUpdateProc.Update_End;
            MessageBox.Show(strVer, "程序版本", MessageBoxButtons.OK, MessageBoxIcon.Information);
            btRdVersionInfo.Enabled = true;
            btStartUpdate.Enabled = true;
            gpSelectType.Enabled = true;
            return;
        }
        #endregion

        #region APP升级部分
        private void TransmitAppPacket()
        {
            int iLoop, iPos;

            if (UpdateRetryTimes == 0)
            {
                timerUpdate.Stop();
                UpdateStep = enUpdateProc.Update_End;
                MessageBox.Show("数据传输出错，升级失败！", "升级失败", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                prgBarUpdate.Visible = false;
                btOpenUpdateFile.Enabled = true;
                btRdVersionInfo.Enabled = true;
                btStartUpdate.Enabled = true;
                rbtBootUpdate.Enabled = true;
                rbtAppUpdate.Enabled = true;
                gpSelectType.Enabled = true;
                btStartUpdate.Text = "开始升级";
                return;
            }
            if (UpdateType == enUpdateType.地锁系统)
            {
                Protocol6009Struct txStruct = new Protocol6009Struct();
                txStruct.CommandIDByte = 0xF1;
                // 数据域
                if (updateLength - AppUpdateWriteAddress > AppUpdateCountOnePacket)
                {
                    AppUpdateCurPkgBytes = AppUpdateCountOnePacket;
                }
                else
                {
                    AppUpdateCurPkgBytes = updateLength - AppUpdateWriteAddress;
                }
                txStruct.DataByte = new byte[AppUpdateCurPkgBytes + 12];
                iLoop = 0;
                // 增加CRC
                txStruct.DataByte[iLoop++] = (byte)updateCrc;
                txStruct.DataByte[iLoop++] = (byte)(updateCrc >> 8);
                // 增加写入地址
                txStruct.DataByte[iLoop++] = (byte)AppUpdateWriteAddress;
                txStruct.DataByte[iLoop++] = (byte)(AppUpdateWriteAddress >> 8);
                txStruct.DataByte[iLoop++] = (byte)(AppUpdateWriteAddress >> 16);
                txStruct.DataByte[iLoop++] = (byte)(AppUpdateWriteAddress >> 24);
                // 增加写入代码总长度
                txStruct.DataByte[iLoop++] = (byte)updateLength;
                txStruct.DataByte[iLoop++] = (byte)(updateLength >> 8);
                txStruct.DataByte[iLoop++] = (byte)(updateLength >> 16);
                txStruct.DataByte[iLoop++] = (byte)(updateLength >> 24);
                // 本包升级代码的长度
                txStruct.DataByte[iLoop++] = (byte)AppUpdateCurPkgBytes;
                txStruct.DataByte[iLoop++] = (byte)(AppUpdateCurPkgBytes >> 8);
                // 升级数据
                iPos = AppUpdateWriteAddress;
                for (; iLoop < txStruct.DataByte.Length; iLoop++)
                {
                    txStruct.DataByte[iLoop] = updateBuf[iPos++];
                }

                // 发送数据
                dataTransmit(_6009CreatTxBuffer(txStruct));
                UpdateWaitTime = 200;
                UpdateRetryTimes--;
            }
            else if (UpdateType == enUpdateType.无线模块系统)
            {
                Protocol6009Struct txStruct = new Protocol6009Struct();
                txStruct.CommandIDByte = 0xF4;
                // 数据域
                if (updateLength - AppUpdateWriteAddress > AppUpdateCountOnePacket)
                {
                    AppUpdateCurPkgBytes = AppUpdateCountOnePacket;
                }
                else
                {
                    AppUpdateCurPkgBytes = updateLength - AppUpdateWriteAddress;
                }
                txStruct.DataByte = new byte[AppUpdateCurPkgBytes + 12];
                iLoop = 0;
                // 增加CRC
                txStruct.DataByte[iLoop++] = (byte)updateCrc;
                txStruct.DataByte[iLoop++] = (byte)(updateCrc >> 8);
                // 增加写入地址
                txStruct.DataByte[iLoop++] = (byte)AppUpdateWriteAddress;
                txStruct.DataByte[iLoop++] = (byte)(AppUpdateWriteAddress >> 8);
                txStruct.DataByte[iLoop++] = (byte)(AppUpdateWriteAddress >> 16);
                txStruct.DataByte[iLoop++] = (byte)(AppUpdateWriteAddress >> 24);
                // 增加写入代码总长度
                txStruct.DataByte[iLoop++] = (byte)updateLength;
                txStruct.DataByte[iLoop++] = (byte)(updateLength >> 8);
                txStruct.DataByte[iLoop++] = (byte)(updateLength >> 16);
                txStruct.DataByte[iLoop++] = (byte)(updateLength >> 24);
                // 本包升级代码的长度
                txStruct.DataByte[iLoop++] = (byte)AppUpdateCurPkgBytes;
                txStruct.DataByte[iLoop++] = (byte)(AppUpdateCurPkgBytes >> 8);
                // 升级数据
                iPos = AppUpdateWriteAddress;
                for (; iLoop < txStruct.DataByte.Length; iLoop++)
                {
                    txStruct.DataByte[iLoop] = updateBuf[iPos++];
                }
                 
                // 发送数据
                dataTransmit(_6009CreatTxBuffer(txStruct));
                UpdateWaitTime = 200;
                UpdateRetryTimes--;
            }
            UpdateWaitTime = 80;
            UpdateRetryTimes--;
        }
        private void ExplainAppPacket(byte[] RecBuf)
        {
            UInt16 u16Crc;
            byte[] dataBuf = new byte[100];

            if (UpdateType == enUpdateType.地锁系统)
            {
                Protocol6009Struct rxStruct = Extract6009RxBuffer(RecBuf);
                if (false == rxStruct.isSuccess || rxStruct.CommandIDByte != 0xF1)
                {
                    return;
                }
                Array.Copy(rxStruct.DataByte, dataBuf, rxStruct.DataByte.Length);
            }
            else if (UpdateType == enUpdateType.无线模块系统)
            {
                Protocol6009Struct rxStruct = Extract6009RxBuffer(RecBuf);
                if (false == rxStruct.isSuccess || rxStruct.CommandIDByte != 0xF4)
                {
                    return;
                }
                Array.Copy(rxStruct.DataByte, dataBuf, rxStruct.DataByte.Length);
            }
            else
            {
                return;
            }

            u16Crc = (UInt16)(dataBuf[0] + dataBuf[1] * 256);
            AppUpdateWriteAddress = dataBuf[2] + (dataBuf[3] << 8) +
                                    (dataBuf[4] << 16) + (dataBuf[5] << 24);
            if (u16Crc != updateCrc)
            {
                UpdateWaitTime = 3;
                AppUpdateWriteAddress = 0;
                return;
            }
            if (dataBuf[6] == 0xAA)
            {
                AppUpdateWriteAddress += AppUpdateCountOnePacket;
                prgBarUpdate.Maximum = updateLength;
                prgBarUpdate.Value = (AppUpdateWriteAddress > updateLength) ? updateLength : AppUpdateWriteAddress;
                prgBarUpdate.Visible = true;
                if (AppUpdateWriteAddress >= updateLength)
                {
                    timerUpdate.Stop();
                    UpdateStep = enUpdateProc.Update_End;
                    if (UpdateType == enUpdateType.地锁系统)
                    {
                        MessageBox.Show("程序升级成功，主板将在10秒钟内重新启动。", "升级成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (UpdateType == enUpdateType.无线模块系统)
                    {
                        MessageBox.Show("无线模块升级程序下发成功。", "升级成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    btOpenUpdateFile.Enabled = true;
                    btRdVersionInfo.Enabled = true;
                    rbtBootUpdate.Enabled = true;
                    rbtAppUpdate.Enabled = true;
                    btStartUpdate.Enabled = true;
                    gpSelectType.Enabled = true;
                    btStartUpdate.Text = "开始升级";
                    return;
                }
                UpdateWaitTime = 1;
                UpdateRetryTimes = 5;
            }
        }
        #endregion

        #region BOOT升级部分
        private void UpdateBootDownCount()
        {
            if (UpdateStep != enUpdateProc.Boot_EnterUpdate)
            {
                return;
            }
            if (BootUpdateLastDownCounter == 0)
            {
                timerUpdate.Stop();
                UpdateStep = enUpdateProc.Update_End;
                MessageBox.Show("没有进入到升级模式，请检查连线是否正确！", "未进入升级模式", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                prgBarUpdate.Visible = false;
                lbFront.Visible = false;
                lbMiddle.Visible = false;
                lbRear.Visible = false;
                btOpenUpdateFile.Enabled = true;
                btRdVersionInfo.Enabled = true;
                btStartUpdate.Enabled = true;
                rbtBootUpdate.Enabled = true;
                rbtAppUpdate.Enabled = true;
                gpSelectType.Enabled = true;
                // 恢复原先的波特率
                setUartBaudrate(iLastPortBaudrate);
                btStartUpdate.Text = "开始升级";
                return;
            }
            if (--BootUpdateLastDownCounter % 100 != 0)
            {
                return;
            }
            lbFront.Text = "请在";
            lbMiddle.Text = (BootUpdateLastDownCounter / 100).ToString("D");
            lbRear.Text = "秒钟内按下主板的复位按键或重新上电！";
            int x = 147, y = 300;
            lbFront.Location = new System.Drawing.Point(x, y);
            lbMiddle.Location = new System.Drawing.Point(x + lbFront.Size.Width, y - 18);
            lbRear.Location = new System.Drawing.Point(x + lbFront.Size.Width + lbMiddle.Size.Width, y);
            prgBarUpdate.Visible = false;
            lbFront.Visible = true;
            lbMiddle.Visible = true;
            lbRear.Visible = true;
            return;
        }
        private void TransmitBootPacket()
        {
            int iLoop, iPos;
            UInt16 crc;

            if (UpdateStep == enUpdateProc.Boot_EnterUpdate)
            {
                byte[] txBuf = new byte[19];
                iPos = 0;
                txBuf[iPos++] = SyncWord1;
                txBuf[iPos++] = SyncWord2;
                txBuf[iPos++] = (byte)(txBuf.Length - 5);           // 长度
                txBuf[iPos++] = 0x01;                               // 命令标识-进入升级指令
                txBuf[iPos++] = 0x04;                               // 命令选项
                for (iLoop = 0; iLoop < 12; iLoop++ )
                {
                    txBuf[iPos++] = updateBuf[iLoop];
                }
                crc = calCrc16(txBuf, 2, (UInt32)(iPos - 2));
                txBuf[iPos++] = (byte)crc;
                txBuf[iPos++] = (byte)(crc >> 8);
                dataTransmit(txBuf);                                // 发送数据
                UpdateWaitTime = 15;
            }
            else if (UpdateStep == enUpdateProc.Boot_UpdateInProcing)
            {
                if (UpdateRetryTimes == 0)
                {
                    timerUpdate.Stop();
                    UpdateStep = enUpdateProc.Update_End;
                    MessageBox.Show("数据传输出错，升级失败！", "升级失败", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    prgBarUpdate.Visible = false;
                    lbFront.Visible = false;
                    lbMiddle.Visible = false;
                    lbRear.Visible = false;
                    btOpenUpdateFile.Enabled = true;
                    btRdVersionInfo.Enabled = true;
                    btStartUpdate.Enabled = true;
                    rbtBootUpdate.Enabled = true;
                    rbtAppUpdate.Enabled = true;
                    gpSelectType.Enabled = true;
                    // 恢复原先的波特率
                    setUartBaudrate(iLastPortBaudrate);
                    btStartUpdate.Text = "开始升级";
                    return;
                }

                byte[] txBuf = new byte[141];
                iPos = 0;
                txBuf[iPos++] = SyncWord1;
                txBuf[iPos++] = SyncWord2;
                txBuf[iPos++] = (byte)(txBuf.Length - 5);           // 长度
                txBuf[iPos++] = 0x03;                               // 命令标识-发送升级数据包
                txBuf[iPos++] = 0x04;                               // 命令选项
                for (iLoop = 0; iLoop < 4; iLoop++ )
                {
                    txBuf[iPos++] = updateBuf[iLoop]; 
                }
                txBuf[iPos++] = (byte)BootUpdateCurrentPkg;
                txBuf[iPos++] = (byte)(BootUpdateCurrentPkg >> 8);
                for (iLoop = 0; iLoop < 128; iLoop++)
                {
                    txBuf[iPos++] = updateBuf[(BootUpdateCurrentPkg + 1) * BootUpdateCountOnePacket + iLoop];
                }
                crc = calCrc16(txBuf, 2, (UInt32)(iPos - 2));
                txBuf[iPos++] = (byte)crc;
                txBuf[iPos++] = (byte)(crc >> 8);
                dataTransmit(txBuf);
                UpdateRetryTimes--;
                UpdateWaitTime = 80;
            }
            else if (UpdateStep == enUpdateProc.Update_End)
            {
                timerUpdate.Stop();
            }
            else
            {
                timerUpdate.Stop();
            }
        }
        private void ExplainBootPacket(byte[] RecBuf)
        {
            UInt16 crc;
            byte bLen;

            if (RecBuf[0] != SyncWord1 || RecBuf[1] != SyncWord2)
            {
                return;
            }
            bLen = RecBuf[2];
            crc = calCrc16(RecBuf, 2, (UInt32)(bLen + 1));
            if (crc != (RecBuf[bLen + 4] * 256 + RecBuf[bLen + 3]))
            {
                return;
            }
            if (RecBuf[3] == 0x02)          // 进入升级命令的应答
            {
                if (RecBuf[5] == 0 && RecBuf[6] == 0)
                {
                    UpdateStep = enUpdateProc.Boot_UpdateInProcing;
                    UpdateWaitTime = 1;
                    UpdateRetryTimes = 5;
                    BootUpdateCurrentPkg = 0;
                    BootUpdateTotalPkg = updateBuf[3] * 256 + updateBuf[2];
                    prgBarUpdate.Maximum = BootUpdateTotalPkg;
                    prgBarUpdate.Value = BootUpdateCurrentPkg;
                    prgBarUpdate.Visible = true;
                    lbFront.Visible = false;
                    lbMiddle.Visible = false;
                    lbRear.Visible = false;
                    return;
                }
            }
            else if (RecBuf[3] == 0x04)     // 发送升级数据包应答
            {
                if (RecBuf[7] == 0 && RecBuf[8] == 0)
                {
                    UpdateStep = enUpdateProc.Boot_UpdateInProcing;
                    UpdateWaitTime = 1;
                    UpdateRetryTimes = 5;
                    BootUpdateCurrentPkg = RecBuf[5] + RecBuf[6] * 256;
                    prgBarUpdate.Maximum = BootUpdateTotalPkg;
                    prgBarUpdate.Value = BootUpdateCurrentPkg;
                    prgBarUpdate.Visible = true;
                    lbFront.Visible = false;
                    lbMiddle.Visible = false;
                    lbRear.Visible = false;
                    if (++BootUpdateCurrentPkg >= BootUpdateTotalPkg)
                    {
                        timerUpdate.Stop();
                        UpdateStep = enUpdateProc.Update_End;
                        MessageBox.Show("程序升级成功，主板将在10秒钟内重新启动。", "升级成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        btOpenUpdateFile.Enabled = true;
                        btRdVersionInfo.Enabled = true;
                        rbtBootUpdate.Enabled = true;
                        rbtAppUpdate.Enabled = true;
                        btStartUpdate.Enabled = true;
                        gpSelectType.Enabled = true;
                        // 恢复原波特率
                        setUartBaudrate(iLastPortBaudrate);
                        btStartUpdate.Text = "开始升级";
                    }
                }
            }
        }
        #endregion
    }
}
