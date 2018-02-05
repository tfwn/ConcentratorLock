namespace SR6009_Concentrator_Tools
{
    partial class FrmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            this.statusMainStrip = new System.Windows.Forms.StatusStrip();
            this.tsComPortInfo = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsLocalAddress = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.prgBar = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripStatusLabel4 = new System.Windows.Forms.ToolStripStatusLabel();
            this.lbCurState = new System.Windows.Forms.ToolStripStatusLabel();
            this.lbCurTime = new System.Windows.Forms.ToolStripStatusLabel();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tbDevParam = new System.Windows.Forms.TabPage();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbHostFreq = new System.Windows.Forms.TextBox();
            this.HostChannel = new System.Windows.Forms.NumericUpDown();
            this.btWriteHostChannel = new System.Windows.Forms.Button();
            this.tbReadHostChannel = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btWriteConvParam = new System.Windows.Forms.Button();
            this.btReadConvParam = new System.Windows.Forms.Button();
            this.ManualAddNode = new System.Windows.Forms.RadioButton();
            this.AutoSaveNode = new System.Windows.Forms.RadioButton();
            this.gpbNodeEdit = new System.Windows.Forms.GroupBox();
            this.cbSyncWithConc = new System.Windows.Forms.CheckBox();
            this.btReadDevNodeCount = new System.Windows.Forms.Button();
            this.btCreatNewNode = new System.Windows.Forms.Button();
            this.btDelNode = new System.Windows.Forms.Button();
            this.btModifyNode = new System.Windows.Forms.Button();
            this.cmbNodeType = new System.Windows.Forms.ComboBox();
            this.label22 = new System.Windows.Forms.Label();
            this.tbNewNodeAddr = new System.Windows.Forms.TextBox();
            this.label23 = new System.Windows.Forms.Label();
            this.tbCurNodeAddr = new System.Windows.Forms.TextBox();
            this.label24 = new System.Windows.Forms.Label();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.btWriteDoc = new System.Windows.Forms.Button();
            this.btReadDoc = new System.Windows.Forms.Button();
            this.btFormatDoc = new System.Windows.Forms.Button();
            this.btFormatData = new System.Windows.Forms.Button();
            this.btWriteDocToDev = new System.Windows.Forms.Button();
            this.btReadDocFromDev = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.btReadAllLockData = new System.Windows.Forms.Button();
            this.label19 = new System.Windows.Forms.Label();
            this.btReadLockData = new System.Windows.Forms.Button();
            this.btSaveDataToXml = new System.Windows.Forms.Button();
            this.tbCurAddr = new System.Windows.Forms.TextBox();
            this.lbVerInfo = new System.Windows.Forms.Label();
            this.btMemCheck = new System.Windows.Forms.Button();
            this.dtpTime = new System.Windows.Forms.DateTimePicker();
            this.dtpDate = new System.Windows.Forms.DateTimePicker();
            this.cmbTimeType = new System.Windows.Forms.ComboBox();
            this.btSetRtc = new System.Windows.Forms.Button();
            this.btReadRtc = new System.Windows.Forms.Button();
            this.btReadDevVerInfo = new System.Windows.Forms.Button();
            this.btRestartDev = new System.Windows.Forms.Button();
            this.tbNewConcAddr = new System.Windows.Forms.TextBox();
            this.btSetConcAddr = new System.Windows.Forms.Button();
            this.tbGprsParam = new System.Windows.Forms.TabPage();
            this.btReadGprsInfo = new System.Windows.Forms.Button();
            this.btReadGprsParam = new System.Windows.Forms.Button();
            this.btSetGprsParam = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.pgbGrpsRssi = new System.Windows.Forms.ProgressBar();
            this.label18 = new System.Windows.Forms.Label();
            this.lbIMSI = new System.Windows.Forms.Label();
            this.lbConnectStatus = new System.Windows.Forms.Label();
            this.lbGprsModle = new System.Windows.Forms.Label();
            this.gpbGprsInfo = new System.Windows.Forms.GroupBox();
            this.rtbGprsMsg = new System.Windows.Forms.RichTextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.nudHeatBeat = new System.Windows.Forms.NumericUpDown();
            this.tbPassword = new System.Windows.Forms.TextBox();
            this.lbPassword = new System.Windows.Forms.Label();
            this.tbServerIp02 = new System.Windows.Forms.TextBox();
            this.tbServerIp03 = new System.Windows.Forms.TextBox();
            this.tbServerIp01 = new System.Windows.Forms.TextBox();
            this.lbServerIP0 = new System.Windows.Forms.Label();
            this.lbUnit = new System.Windows.Forms.Label();
            this.lbDot02 = new System.Windows.Forms.Label();
            this.lbHeatBeat = new System.Windows.Forms.Label();
            this.tbServerIp00 = new System.Windows.Forms.TextBox();
            this.lbServerIP1 = new System.Windows.Forms.Label();
            this.tbApn = new System.Windows.Forms.TextBox();
            this.lbPort1 = new System.Windows.Forms.Label();
            this.tbServerPort0 = new System.Windows.Forms.TextBox();
            this.tbServerIp10 = new System.Windows.Forms.TextBox();
            this.lbPort0 = new System.Windows.Forms.Label();
            this.tbUsername = new System.Windows.Forms.TextBox();
            this.tbServerPort1 = new System.Windows.Forms.TextBox();
            this.lbDot1 = new System.Windows.Forms.Label();
            this.tbServerIp13 = new System.Windows.Forms.TextBox();
            this.tbServerIp11 = new System.Windows.Forms.TextBox();
            this.lbUsername = new System.Windows.Forms.Label();
            this.tbServerIp12 = new System.Windows.Forms.TextBox();
            this.lbApn = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.lbDot0 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.cmsMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiAddToForwardList = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiClearDocument = new System.Windows.Forms.ToolStripMenuItem();
            this.tbConcAddr = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btReadConcAddr = new System.Windows.Forms.Button();
            this.scon1 = new System.Windows.Forms.SplitContainer();
            this.label20 = new System.Windows.Forms.Label();
            this.btPortCtrl = new System.Windows.Forms.Button();
            this.cmbComType = new System.Windows.Forms.ComboBox();
            this.cmbPort = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.scon2 = new System.Windows.Forms.SplitContainer();
            this.scon3 = new System.Windows.Forms.SplitContainer();
            this.tbPageDocument = new System.Windows.Forms.TabControl();
            this.tbPageRec = new System.Windows.Forms.TabPage();
            this.rtbCommMsg = new System.Windows.Forms.RichTextBox();
            this.cntMenuStripCommInfo = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiAutoScroll = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiAutoClear = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiClearAll = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiSaveRecord = new System.Windows.Forms.ToolStripMenuItem();
            this.tbPageDocDS = new System.Windows.Forms.TabPage();
            this.dgvDocDS = new System.Windows.Forms.DataGridView();
            this.tbPageRecord = new System.Windows.Forms.TabControl();
            this.serialPort = new System.IO.Ports.SerialPort(this.components);
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.tipInfo = new System.Windows.Forms.ToolTip(this.components);
            this.openFileDlg = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDlg = new System.Windows.Forms.SaveFileDialog();
            this.usbHidPort = new UsbLibrary.UsbHidPort(this.components);
            this.statusMainStrip.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tbDevParam.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.HostChannel)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.gpbNodeEdit.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.tbGprsParam.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.gpbGprsInfo.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudHeatBeat)).BeginInit();
            this.cmsMenu.SuspendLayout();
            this.scon1.Panel1.SuspendLayout();
            this.scon1.Panel2.SuspendLayout();
            this.scon1.SuspendLayout();
            this.scon2.Panel1.SuspendLayout();
            this.scon2.Panel2.SuspendLayout();
            this.scon2.SuspendLayout();
            this.scon3.Panel1.SuspendLayout();
            this.scon3.Panel2.SuspendLayout();
            this.scon3.SuspendLayout();
            this.tbPageDocument.SuspendLayout();
            this.tbPageRec.SuspendLayout();
            this.cntMenuStripCommInfo.SuspendLayout();
            this.tbPageDocDS.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDocDS)).BeginInit();
            this.SuspendLayout();
            // 
            // statusMainStrip
            // 
            this.statusMainStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsComPortInfo,
            this.toolStripStatusLabel2,
            this.tsLocalAddress,
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel3,
            this.prgBar,
            this.toolStripStatusLabel4,
            this.lbCurState,
            this.lbCurTime});
            this.statusMainStrip.Location = new System.Drawing.Point(3, 704);
            this.statusMainStrip.Name = "statusMainStrip";
            this.statusMainStrip.Size = new System.Drawing.Size(1422, 23);
            this.statusMainStrip.TabIndex = 1;
            this.statusMainStrip.Text = "statusStrip1";
            // 
            // tsComPortInfo
            // 
            this.tsComPortInfo.AutoSize = false;
            this.tsComPortInfo.Name = "tsComPortInfo";
            this.tsComPortInfo.Size = new System.Drawing.Size(300, 18);
            this.tsComPortInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(11, 18);
            this.toolStripStatusLabel2.Text = "|";
            // 
            // tsLocalAddress
            // 
            this.tsLocalAddress.AutoSize = false;
            this.tsLocalAddress.Name = "tsLocalAddress";
            this.tsLocalAddress.Size = new System.Drawing.Size(200, 18);
            this.tsLocalAddress.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(11, 18);
            this.toolStripStatusLabel1.Text = "|";
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(68, 18);
            this.toolStripStatusLabel3.Text = "执行进度：";
            // 
            // prgBar
            // 
            this.prgBar.Name = "prgBar";
            this.prgBar.Size = new System.Drawing.Size(250, 17);
            // 
            // toolStripStatusLabel4
            // 
            this.toolStripStatusLabel4.Name = "toolStripStatusLabel4";
            this.toolStripStatusLabel4.Size = new System.Drawing.Size(59, 18);
            this.toolStripStatusLabel4.Text = "            |";
            // 
            // lbCurState
            // 
            this.lbCurState.Name = "lbCurState";
            this.lbCurState.Size = new System.Drawing.Size(450, 18);
            this.lbCurState.Spring = true;
            this.lbCurState.Text = "设备状态：通信端口关闭";
            this.lbCurState.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbCurTime
            // 
            this.lbCurTime.Name = "lbCurTime";
            this.lbCurTime.Size = new System.Drawing.Size(56, 18);
            this.lbCurTime.Text = "当前时间";
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tbDevParam);
            this.tabControl.Controls.Add(this.tbGprsParam);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(384, 652);
            this.tabControl.TabIndex = 1;
            // 
            // tbDevParam
            // 
            this.tbDevParam.BackColor = System.Drawing.Color.Snow;
            this.tbDevParam.Controls.Add(this.groupBox4);
            this.tbDevParam.Controls.Add(this.groupBox1);
            this.tbDevParam.Controls.Add(this.gpbNodeEdit);
            this.tbDevParam.Controls.Add(this.groupBox7);
            this.tbDevParam.Controls.Add(this.groupBox5);
            this.tbDevParam.Controls.Add(this.lbVerInfo);
            this.tbDevParam.Controls.Add(this.btMemCheck);
            this.tbDevParam.Controls.Add(this.dtpTime);
            this.tbDevParam.Controls.Add(this.dtpDate);
            this.tbDevParam.Controls.Add(this.cmbTimeType);
            this.tbDevParam.Controls.Add(this.btSetRtc);
            this.tbDevParam.Controls.Add(this.btReadRtc);
            this.tbDevParam.Controls.Add(this.btReadDevVerInfo);
            this.tbDevParam.Controls.Add(this.btRestartDev);
            this.tbDevParam.Controls.Add(this.tbNewConcAddr);
            this.tbDevParam.Controls.Add(this.btSetConcAddr);
            this.tbDevParam.Location = new System.Drawing.Point(4, 22);
            this.tbDevParam.Name = "tbDevParam";
            this.tbDevParam.Padding = new System.Windows.Forms.Padding(3);
            this.tbDevParam.Size = new System.Drawing.Size(376, 626);
            this.tbDevParam.TabIndex = 3;
            this.tbDevParam.Text = " 参数管理 ";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label3);
            this.groupBox4.Controls.Add(this.tbHostFreq);
            this.groupBox4.Controls.Add(this.HostChannel);
            this.groupBox4.Controls.Add(this.btWriteHostChannel);
            this.groupBox4.Controls.Add(this.tbReadHostChannel);
            this.groupBox4.Location = new System.Drawing.Point(196, 555);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(163, 68);
            this.groupBox4.TabIndex = 64;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "主机模块信道";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(131, 47);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(27, 13);
            this.label3.TabIndex = 68;
            this.label3.Text = "Mhz";
            // 
            // tbHostFreq
            // 
            this.tbHostFreq.Enabled = false;
            this.tbHostFreq.Location = new System.Drawing.Point(67, 42);
            this.tbHostFreq.Name = "tbHostFreq";
            this.tbHostFreq.Size = new System.Drawing.Size(58, 20);
            this.tbHostFreq.TabIndex = 8;
            this.tbHostFreq.Text = "470.80";
            // 
            // HostChannel
            // 
            this.HostChannel.Location = new System.Drawing.Point(67, 19);
            this.HostChannel.Maximum = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.HostChannel.Name = "HostChannel";
            this.HostChannel.Size = new System.Drawing.Size(49, 20);
            this.HostChannel.TabIndex = 7;
            this.HostChannel.ValueChanged += new System.EventHandler(this.HostChannel_ValueChanged);
            // 
            // btWriteHostChannel
            // 
            this.btWriteHostChannel.Location = new System.Drawing.Point(6, 42);
            this.btWriteHostChannel.Name = "btWriteHostChannel";
            this.btWriteHostChannel.Size = new System.Drawing.Size(58, 23);
            this.btWriteHostChannel.TabIndex = 6;
            this.btWriteHostChannel.Text = "写信道";
            this.btWriteHostChannel.UseVisualStyleBackColor = true;
            this.btWriteHostChannel.Click += new System.EventHandler(this.btWriteHostChannel_Click);
            // 
            // tbReadHostChannel
            // 
            this.tbReadHostChannel.Location = new System.Drawing.Point(6, 19);
            this.tbReadHostChannel.Name = "tbReadHostChannel";
            this.tbReadHostChannel.Size = new System.Drawing.Size(58, 23);
            this.tbReadHostChannel.TabIndex = 5;
            this.tbReadHostChannel.Text = "读信道";
            this.tbReadHostChannel.UseVisualStyleBackColor = true;
            this.tbReadHostChannel.Click += new System.EventHandler(this.tbReadHostChannel_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btWriteConvParam);
            this.groupBox1.Controls.Add(this.btReadConvParam);
            this.groupBox1.Controls.Add(this.ManualAddNode);
            this.groupBox1.Controls.Add(this.AutoSaveNode);
            this.groupBox1.Location = new System.Drawing.Point(18, 555);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(163, 68);
            this.groupBox1.TabIndex = 63;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "集中器参数管理";
            // 
            // btWriteConvParam
            // 
            this.btWriteConvParam.Location = new System.Drawing.Point(6, 42);
            this.btWriteConvParam.Name = "btWriteConvParam";
            this.btWriteConvParam.Size = new System.Drawing.Size(56, 23);
            this.btWriteConvParam.TabIndex = 6;
            this.btWriteConvParam.Text = "写参数";
            this.btWriteConvParam.UseVisualStyleBackColor = true;
            this.btWriteConvParam.Click += new System.EventHandler(this.btWriteConvParam_Click);
            // 
            // btReadConvParam
            // 
            this.btReadConvParam.Location = new System.Drawing.Point(6, 19);
            this.btReadConvParam.Name = "btReadConvParam";
            this.btReadConvParam.Size = new System.Drawing.Size(56, 23);
            this.btReadConvParam.TabIndex = 5;
            this.btReadConvParam.Text = "读参数";
            this.btReadConvParam.UseVisualStyleBackColor = true;
            this.btReadConvParam.Click += new System.EventHandler(this.btReadConvParam_Click);
            // 
            // ManualAddNode
            // 
            this.ManualAddNode.AutoSize = true;
            this.ManualAddNode.Location = new System.Drawing.Point(64, 45);
            this.ManualAddNode.Name = "ManualAddNode";
            this.ManualAddNode.Size = new System.Drawing.Size(97, 17);
            this.ManualAddNode.TabIndex = 4;
            this.ManualAddNode.TabStop = true;
            this.ManualAddNode.Text = "手动添加档案";
            this.ManualAddNode.UseVisualStyleBackColor = true;
            // 
            // AutoSaveNode
            // 
            this.AutoSaveNode.AutoSize = true;
            this.AutoSaveNode.Checked = true;
            this.AutoSaveNode.Location = new System.Drawing.Point(64, 22);
            this.AutoSaveNode.Name = "AutoSaveNode";
            this.AutoSaveNode.Size = new System.Drawing.Size(97, 17);
            this.AutoSaveNode.TabIndex = 3;
            this.AutoSaveNode.TabStop = true;
            this.AutoSaveNode.Text = "自动保存档案";
            this.AutoSaveNode.UseVisualStyleBackColor = true;
            // 
            // gpbNodeEdit
            // 
            this.gpbNodeEdit.Controls.Add(this.cbSyncWithConc);
            this.gpbNodeEdit.Controls.Add(this.btReadDevNodeCount);
            this.gpbNodeEdit.Controls.Add(this.btCreatNewNode);
            this.gpbNodeEdit.Controls.Add(this.btDelNode);
            this.gpbNodeEdit.Controls.Add(this.btModifyNode);
            this.gpbNodeEdit.Controls.Add(this.cmbNodeType);
            this.gpbNodeEdit.Controls.Add(this.label22);
            this.gpbNodeEdit.Controls.Add(this.tbNewNodeAddr);
            this.gpbNodeEdit.Controls.Add(this.label23);
            this.gpbNodeEdit.Controls.Add(this.tbCurNodeAddr);
            this.gpbNodeEdit.Controls.Add(this.label24);
            this.gpbNodeEdit.Location = new System.Drawing.Point(18, 411);
            this.gpbNodeEdit.Name = "gpbNodeEdit";
            this.gpbNodeEdit.Size = new System.Drawing.Size(352, 138);
            this.gpbNodeEdit.TabIndex = 61;
            this.gpbNodeEdit.TabStop = false;
            this.gpbNodeEdit.Text = "当前表具编辑";
            // 
            // cbSyncWithConc
            // 
            this.cbSyncWithConc.AutoSize = true;
            this.cbSyncWithConc.Location = new System.Drawing.Point(186, 22);
            this.cbSyncWithConc.Name = "cbSyncWithConc";
            this.cbSyncWithConc.Size = new System.Drawing.Size(146, 17);
            this.cbSyncWithConc.TabIndex = 72;
            this.cbSyncWithConc.Text = "档案变动和集中器同步";
            this.tipInfo.SetToolTip(this.cbSyncWithConc, "勾选后，新建档案、删除档案和修改档案将会同步到集中器中，否则只是在文档中编辑。\r\n该操作不影响集中器中的路由信息。");
            this.cbSyncWithConc.UseVisualStyleBackColor = true;
            // 
            // btReadDevNodeCount
            // 
            this.btReadDevNodeCount.Location = new System.Drawing.Point(6, 19);
            this.btReadDevNodeCount.Name = "btReadDevNodeCount";
            this.btReadDevNodeCount.Size = new System.Drawing.Size(157, 23);
            this.btReadDevNodeCount.TabIndex = 53;
            this.btReadDevNodeCount.Text = "读取设备中档案数量";
            this.tipInfo.SetToolTip(this.btReadDevNodeCount, "读取集中器中所有表具的数量。");
            this.btReadDevNodeCount.UseVisualStyleBackColor = true;
            this.btReadDevNodeCount.Click += new System.EventHandler(this.btReadDevNodeCount_Click);
            // 
            // btCreatNewNode
            // 
            this.btCreatNewNode.Location = new System.Drawing.Point(6, 47);
            this.btCreatNewNode.Name = "btCreatNewNode";
            this.btCreatNewNode.Size = new System.Drawing.Size(157, 23);
            this.btCreatNewNode.TabIndex = 54;
            this.btCreatNewNode.Text = "新建档案";
            this.tipInfo.SetToolTip(this.btCreatNewNode, "新建一个表具档案。");
            this.btCreatNewNode.UseVisualStyleBackColor = true;
            this.btCreatNewNode.Click += new System.EventHandler(this.btCreatNewNode_Click);
            // 
            // btDelNode
            // 
            this.btDelNode.Location = new System.Drawing.Point(6, 75);
            this.btDelNode.Name = "btDelNode";
            this.btDelNode.Size = new System.Drawing.Size(157, 23);
            this.btDelNode.TabIndex = 55;
            this.btDelNode.Text = "删除档案";
            this.tipInfo.SetToolTip(this.btDelNode, "删除一个表具档案。");
            this.btDelNode.UseVisualStyleBackColor = true;
            this.btDelNode.Click += new System.EventHandler(this.btDelNode_Click);
            // 
            // btModifyNode
            // 
            this.btModifyNode.Location = new System.Drawing.Point(6, 104);
            this.btModifyNode.Name = "btModifyNode";
            this.btModifyNode.Size = new System.Drawing.Size(157, 23);
            this.btModifyNode.TabIndex = 56;
            this.btModifyNode.Text = "修改档案";
            this.tipInfo.SetToolTip(this.btModifyNode, "修改当前表具档案。");
            this.btModifyNode.UseVisualStyleBackColor = true;
            this.btModifyNode.Click += new System.EventHandler(this.btModifyNode_Click);
            // 
            // cmbNodeType
            // 
            this.cmbNodeType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbNodeType.FormattingEnabled = true;
            this.cmbNodeType.Location = new System.Drawing.Point(251, 107);
            this.cmbNodeType.Name = "cmbNodeType";
            this.cmbNodeType.Size = new System.Drawing.Size(89, 21);
            this.cmbNodeType.TabIndex = 70;
            this.tipInfo.SetToolTip(this.cmbNodeType, "类型信息。");
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(164, 52);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(55, 13);
            this.label22.TabIndex = 67;
            this.label22.Text = "当前地址";
            // 
            // tbNewNodeAddr
            // 
            this.tbNewNodeAddr.Location = new System.Drawing.Point(225, 78);
            this.tbNewNodeAddr.Name = "tbNewNodeAddr";
            this.tbNewNodeAddr.Size = new System.Drawing.Size(115, 20);
            this.tbNewNodeAddr.TabIndex = 68;
            this.tbNewNodeAddr.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tipInfo.SetToolTip(this.tbNewNodeAddr, "新表具地址，用于修改当前的表具信息。");
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(176, 85);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(43, 13);
            this.label23.TabIndex = 69;
            this.label23.Text = "新地址";
            // 
            // tbCurNodeAddr
            // 
            this.tbCurNodeAddr.Location = new System.Drawing.Point(225, 49);
            this.tbCurNodeAddr.Name = "tbCurNodeAddr";
            this.tbCurNodeAddr.Size = new System.Drawing.Size(115, 20);
            this.tbCurNodeAddr.TabIndex = 58;
            this.tbCurNodeAddr.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tipInfo.SetToolTip(this.tbCurNodeAddr, "当前操作的表具地址，可以在节点档案中选择，也可以手动输入。");
            this.tbCurNodeAddr.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbCurNodeAddr_KeyPress);
            this.tbCurNodeAddr.Leave += new System.EventHandler(this.tbCurNodeAddr_Leave);
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(188, 114);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(31, 13);
            this.label24.TabIndex = 71;
            this.label24.Text = "类型";
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.btWriteDoc);
            this.groupBox7.Controls.Add(this.btReadDoc);
            this.groupBox7.Controls.Add(this.btFormatDoc);
            this.groupBox7.Controls.Add(this.btFormatData);
            this.groupBox7.Controls.Add(this.btWriteDocToDev);
            this.groupBox7.Controls.Add(this.btReadDocFromDev);
            this.groupBox7.Location = new System.Drawing.Point(18, 301);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(352, 104);
            this.groupBox7.TabIndex = 62;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "设备档案操作";
            // 
            // btWriteDoc
            // 
            this.btWriteDoc.Location = new System.Drawing.Point(183, 78);
            this.btWriteDoc.Name = "btWriteDoc";
            this.btWriteDoc.Size = new System.Drawing.Size(157, 23);
            this.btWriteDoc.TabIndex = 48;
            this.btWriteDoc.Text = "保存档案到文件";
            this.tipInfo.SetToolTip(this.btWriteDoc, "保存表具档案信息到文件中。");
            this.btWriteDoc.UseVisualStyleBackColor = true;
            this.btWriteDoc.Click += new System.EventHandler(this.btWriteDoc_Click);
            // 
            // btReadDoc
            // 
            this.btReadDoc.Location = new System.Drawing.Point(6, 78);
            this.btReadDoc.Name = "btReadDoc";
            this.btReadDoc.Size = new System.Drawing.Size(157, 23);
            this.btReadDoc.TabIndex = 47;
            this.btReadDoc.Text = "从文件中读取档案";
            this.tipInfo.SetToolTip(this.btReadDoc, "从文件中读取表具档案，如果表具档案没有类型信息，则默认为“地锁”。");
            this.btReadDoc.UseVisualStyleBackColor = true;
            this.btReadDoc.Click += new System.EventHandler(this.btReadDoc_Click);
            // 
            // btFormatDoc
            // 
            this.btFormatDoc.Location = new System.Drawing.Point(6, 49);
            this.btFormatDoc.Name = "btFormatDoc";
            this.btFormatDoc.Size = new System.Drawing.Size(157, 23);
            this.btFormatDoc.TabIndex = 51;
            this.btFormatDoc.Text = "档案初始化";
            this.tipInfo.SetToolTip(this.btFormatDoc, "删除全部档案和路由信息。");
            this.btFormatDoc.UseVisualStyleBackColor = true;
            this.btFormatDoc.Click += new System.EventHandler(this.btFormatDoc_Click);
            // 
            // btFormatData
            // 
            this.btFormatData.Location = new System.Drawing.Point(183, 49);
            this.btFormatData.Name = "btFormatData";
            this.btFormatData.Size = new System.Drawing.Size(158, 23);
            this.btFormatData.TabIndex = 52;
            this.btFormatData.Text = "数据初始化";
            this.tipInfo.SetToolTip(this.btFormatData, "删除全部表具抄表信息。");
            this.btFormatData.UseVisualStyleBackColor = true;
            this.btFormatData.Click += new System.EventHandler(this.btFormatData_Click);
            // 
            // btWriteDocToDev
            // 
            this.btWriteDocToDev.Location = new System.Drawing.Point(183, 20);
            this.btWriteDocToDev.Name = "btWriteDocToDev";
            this.btWriteDocToDev.Size = new System.Drawing.Size(157, 23);
            this.btWriteDocToDev.TabIndex = 50;
            this.btWriteDocToDev.Text = "保存档案到设备";
            this.tipInfo.SetToolTip(this.btWriteDocToDev, "保存当前档案信息到集中器中。");
            this.btWriteDocToDev.UseVisualStyleBackColor = true;
            this.btWriteDocToDev.Click += new System.EventHandler(this.btWriteDocToDev_Click);
            // 
            // btReadDocFromDev
            // 
            this.btReadDocFromDev.Location = new System.Drawing.Point(6, 20);
            this.btReadDocFromDev.Name = "btReadDocFromDev";
            this.btReadDocFromDev.Size = new System.Drawing.Size(157, 23);
            this.btReadDocFromDev.TabIndex = 49;
            this.btReadDocFromDev.Text = "从设备中读取档案";
            this.tipInfo.SetToolTip(this.btReadDocFromDev, "从集中器中读取表具档案信息。");
            this.btReadDocFromDev.UseVisualStyleBackColor = true;
            this.btReadDocFromDev.Click += new System.EventHandler(this.btReadDocFromDev_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.btReadAllLockData);
            this.groupBox5.Controls.Add(this.label19);
            this.groupBox5.Controls.Add(this.btReadLockData);
            this.groupBox5.Controls.Add(this.btSaveDataToXml);
            this.groupBox5.Controls.Add(this.tbCurAddr);
            this.groupBox5.Location = new System.Drawing.Point(18, 175);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(352, 120);
            this.groupBox5.TabIndex = 60;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "读取集中器中地锁数据";
            // 
            // btReadAllLockData
            // 
            this.btReadAllLockData.Location = new System.Drawing.Point(183, 53);
            this.btReadAllLockData.Name = "btReadAllLockData";
            this.btReadAllLockData.Size = new System.Drawing.Size(157, 23);
            this.btReadAllLockData.TabIndex = 75;
            this.btReadAllLockData.Text = "全部地锁最后一条数据";
            this.tipInfo.SetToolTip(this.btReadAllLockData, "读取集中器中保存的最后一条数据");
            this.btReadAllLockData.UseVisualStyleBackColor = true;
            this.btReadAllLockData.Click += new System.EventHandler(this.btReadAllLockData_Click);
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(7, 29);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(55, 13);
            this.label19.TabIndex = 74;
            this.label19.Text = "地锁地址";
            // 
            // btReadLockData
            // 
            this.btReadLockData.Location = new System.Drawing.Point(6, 53);
            this.btReadLockData.Name = "btReadLockData";
            this.btReadLockData.Size = new System.Drawing.Size(157, 23);
            this.btReadLockData.TabIndex = 60;
            this.btReadLockData.Text = "单个地锁最后一条数据";
            this.tipInfo.SetToolTip(this.btReadLockData, "读取集中器中保存的地锁的最后一条数据");
            this.btReadLockData.UseVisualStyleBackColor = true;
            this.btReadLockData.Click += new System.EventHandler(this.btReadLockData_Click);
            // 
            // btSaveDataToXml
            // 
            this.btSaveDataToXml.Location = new System.Drawing.Point(6, 82);
            this.btSaveDataToXml.Name = "btSaveDataToXml";
            this.btSaveDataToXml.Size = new System.Drawing.Size(157, 23);
            this.btSaveDataToXml.TabIndex = 52;
            this.btSaveDataToXml.Text = "保存所有数据到Excel";
            this.tipInfo.SetToolTip(this.btSaveDataToXml, "保存当前所有数据到Excel文件中。");
            this.btSaveDataToXml.UseVisualStyleBackColor = true;
            this.btSaveDataToXml.Click += new System.EventHandler(this.btSaveDataToXml_Click);
            // 
            // tbCurAddr
            // 
            this.tbCurAddr.Location = new System.Drawing.Point(70, 26);
            this.tbCurAddr.Name = "tbCurAddr";
            this.tbCurAddr.Size = new System.Drawing.Size(166, 20);
            this.tbCurAddr.TabIndex = 59;
            this.tbCurAddr.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tipInfo.SetToolTip(this.tbCurAddr, "当前操作的地锁地址，可以在节点档案中选择，也可以手动输入。");
            this.tbCurAddr.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbCurAddr_KeyPress);
            this.tbCurAddr.Leave += new System.EventHandler(this.tbCurAddr_Leave);
            // 
            // lbVerInfo
            // 
            this.lbVerInfo.AutoSize = true;
            this.lbVerInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbVerInfo.Location = new System.Drawing.Point(189, 114);
            this.lbVerInfo.Margin = new System.Windows.Forms.Padding(3);
            this.lbVerInfo.Name = "lbVerInfo";
            this.lbVerInfo.Padding = new System.Windows.Forms.Padding(3);
            this.lbVerInfo.Size = new System.Drawing.Size(99, 21);
            this.lbVerInfo.TabIndex = 54;
            this.lbVerInfo.Text = "版本信息未读出";
            this.tipInfo.SetToolTip(this.lbVerInfo, "集中器的版本信息。");
            // 
            // btMemCheck
            // 
            this.btMemCheck.Location = new System.Drawing.Point(189, 146);
            this.btMemCheck.Name = "btMemCheck";
            this.btMemCheck.Size = new System.Drawing.Size(157, 23);
            this.btMemCheck.TabIndex = 53;
            this.btMemCheck.Text = "存储器检查";
            this.tipInfo.SetToolTip(this.btMemCheck, "存储器检查用于检测EEPROM是否有错误，本功能运行时间较长且中间不可以中断，否则会影响原EEPROM中的数据内容。");
            this.btMemCheck.UseVisualStyleBackColor = true;
            this.btMemCheck.Click += new System.EventHandler(this.btMemCheck_Click);
            // 
            // dtpTime
            // 
            this.dtpTime.Cursor = System.Windows.Forms.Cursors.Default;
            this.dtpTime.CustomFormat = " HH:mm:ss";
            this.dtpTime.Enabled = false;
            this.dtpTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTime.Location = new System.Drawing.Point(279, 51);
            this.dtpTime.MaxDate = new System.DateTime(2099, 12, 31, 0, 0, 0, 0);
            this.dtpTime.MinDate = new System.DateTime(2000, 1, 1, 0, 0, 0, 0);
            this.dtpTime.Name = "dtpTime";
            this.dtpTime.ShowUpDown = true;
            this.dtpTime.Size = new System.Drawing.Size(75, 20);
            this.dtpTime.TabIndex = 18;
            this.tipInfo.SetToolTip(this.dtpTime, "读取或设置当前集中器的时间。");
            // 
            // dtpDate
            // 
            this.dtpDate.Cursor = System.Windows.Forms.Cursors.Default;
            this.dtpDate.CustomFormat = "yyyy-MM-dd";
            this.dtpDate.Enabled = false;
            this.dtpDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpDate.Location = new System.Drawing.Point(187, 51);
            this.dtpDate.MaxDate = new System.DateTime(2099, 12, 31, 0, 0, 0, 0);
            this.dtpDate.MinDate = new System.DateTime(2000, 1, 1, 0, 0, 0, 0);
            this.dtpDate.Name = "dtpDate";
            this.dtpDate.Size = new System.Drawing.Size(86, 20);
            this.dtpDate.TabIndex = 17;
            this.tipInfo.SetToolTip(this.dtpDate, "读取或设置当前集中器的时间。");
            this.dtpDate.Value = new System.DateTime(2016, 9, 22, 0, 0, 0, 0);
            // 
            // cmbTimeType
            // 
            this.cmbTimeType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTimeType.FormattingEnabled = true;
            this.cmbTimeType.Items.AddRange(new object[] {
            "系统当前时间",
            "自定义时间"});
            this.cmbTimeType.Location = new System.Drawing.Point(187, 80);
            this.cmbTimeType.Name = "cmbTimeType";
            this.cmbTimeType.Size = new System.Drawing.Size(97, 21);
            this.cmbTimeType.TabIndex = 16;
            this.tipInfo.SetToolTip(this.cmbTimeType, "系统当前时间即为本机的系统时间，也可以自定义时间。");
            this.cmbTimeType.SelectedIndexChanged += new System.EventHandler(this.cmbTimeType_SelectedIndexChanged);
            // 
            // btSetRtc
            // 
            this.btSetRtc.Location = new System.Drawing.Point(22, 79);
            this.btSetRtc.Name = "btSetRtc";
            this.btSetRtc.Size = new System.Drawing.Size(157, 23);
            this.btSetRtc.TabIndex = 9;
            this.btSetRtc.Text = "设置集中器时钟";
            this.tipInfo.SetToolTip(this.btSetRtc, "设置集中器时钟，当集中器的时钟和设置时钟相差24小时以上时，需连续设置三次才能成功，设置成功后可能会启动主动上传和数据补抄进程。");
            this.btSetRtc.UseVisualStyleBackColor = true;
            this.btSetRtc.Click += new System.EventHandler(this.btSetRtc_Click);
            // 
            // btReadRtc
            // 
            this.btReadRtc.Location = new System.Drawing.Point(22, 50);
            this.btReadRtc.Name = "btReadRtc";
            this.btReadRtc.Size = new System.Drawing.Size(157, 23);
            this.btReadRtc.TabIndex = 8;
            this.btReadRtc.Text = "读取集中器时钟";
            this.tipInfo.SetToolTip(this.btReadRtc, "读取集中器时钟。");
            this.btReadRtc.UseVisualStyleBackColor = true;
            this.btReadRtc.Click += new System.EventHandler(this.btReadRtc_Click);
            // 
            // btReadDevVerInfo
            // 
            this.btReadDevVerInfo.Location = new System.Drawing.Point(22, 113);
            this.btReadDevVerInfo.Name = "btReadDevVerInfo";
            this.btReadDevVerInfo.Size = new System.Drawing.Size(157, 23);
            this.btReadDevVerInfo.TabIndex = 7;
            this.btReadDevVerInfo.Text = "读取集中器版本信息";
            this.tipInfo.SetToolTip(this.btReadDevVerInfo, "读取集中器的版本信息。");
            this.btReadDevVerInfo.UseVisualStyleBackColor = true;
            this.btReadDevVerInfo.Click += new System.EventHandler(this.btReadDevVerInfo_Click);
            // 
            // btRestartDev
            // 
            this.btRestartDev.Location = new System.Drawing.Point(22, 146);
            this.btRestartDev.Name = "btRestartDev";
            this.btRestartDev.Size = new System.Drawing.Size(157, 23);
            this.btRestartDev.TabIndex = 6;
            this.btRestartDev.Text = "重新启动集中器";
            this.tipInfo.SetToolTip(this.btRestartDev, "重新启动集中器会在10秒钟内进行。");
            this.btRestartDev.UseVisualStyleBackColor = true;
            this.btRestartDev.Click += new System.EventHandler(this.btRestartDev_Click);
            // 
            // tbNewConcAddr
            // 
            this.tbNewConcAddr.Location = new System.Drawing.Point(189, 17);
            this.tbNewConcAddr.Name = "tbNewConcAddr";
            this.tbNewConcAddr.Size = new System.Drawing.Size(157, 20);
            this.tbNewConcAddr.TabIndex = 5;
            this.tbNewConcAddr.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tipInfo.SetToolTip(this.tbNewConcAddr, "集中器编号为16位数字，不足16位时左边自动补零。");
            this.tbNewConcAddr.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbNewConcAddr_KeyPress);
            this.tbNewConcAddr.Leave += new System.EventHandler(this.tbNewConcAddr_Leave);
            // 
            // btSetConcAddr
            // 
            this.btSetConcAddr.ForeColor = System.Drawing.Color.Red;
            this.btSetConcAddr.Location = new System.Drawing.Point(22, 16);
            this.btSetConcAddr.Name = "btSetConcAddr";
            this.btSetConcAddr.Size = new System.Drawing.Size(157, 23);
            this.btSetConcAddr.TabIndex = 1;
            this.btSetConcAddr.Text = "设置集中器编号";
            this.tipInfo.SetToolTip(this.btSetConcAddr, "设置集中器编号必须在调试串口或USB端口连接时操作，并且会导致集中器重新启动。");
            this.btSetConcAddr.UseVisualStyleBackColor = true;
            this.btSetConcAddr.Click += new System.EventHandler(this.btSetConcAddr_Click);
            // 
            // tbGprsParam
            // 
            this.tbGprsParam.BackColor = System.Drawing.Color.Snow;
            this.tbGprsParam.Controls.Add(this.btReadGprsInfo);
            this.tbGprsParam.Controls.Add(this.btReadGprsParam);
            this.tbGprsParam.Controls.Add(this.btSetGprsParam);
            this.tbGprsParam.Controls.Add(this.groupBox3);
            this.tbGprsParam.Controls.Add(this.gpbGprsInfo);
            this.tbGprsParam.Controls.Add(this.groupBox2);
            this.tbGprsParam.Location = new System.Drawing.Point(4, 22);
            this.tbGprsParam.Name = "tbGprsParam";
            this.tbGprsParam.Padding = new System.Windows.Forms.Padding(3);
            this.tbGprsParam.Size = new System.Drawing.Size(376, 626);
            this.tbGprsParam.TabIndex = 4;
            this.tbGprsParam.Text = " Gprs参数 ";
            // 
            // btReadGprsInfo
            // 
            this.btReadGprsInfo.Location = new System.Drawing.Point(22, 232);
            this.btReadGprsInfo.Name = "btReadGprsInfo";
            this.btReadGprsInfo.Size = new System.Drawing.Size(157, 23);
            this.btReadGprsInfo.TabIndex = 69;
            this.btReadGprsInfo.Text = "读取GPRS模块信息";
            this.tipInfo.SetToolTip(this.btReadGprsInfo, "读取GPRS模块的相关信息。");
            this.btReadGprsInfo.UseVisualStyleBackColor = true;
            this.btReadGprsInfo.Click += new System.EventHandler(this.btReadGprsInfo_Click);
            // 
            // btReadGprsParam
            // 
            this.btReadGprsParam.Location = new System.Drawing.Point(22, 16);
            this.btReadGprsParam.Name = "btReadGprsParam";
            this.btReadGprsParam.Size = new System.Drawing.Size(157, 23);
            this.btReadGprsParam.TabIndex = 46;
            this.btReadGprsParam.Text = "读取GPRS参数";
            this.tipInfo.SetToolTip(this.btReadGprsParam, "读取集中器中连接服务器的参数。");
            this.btReadGprsParam.UseVisualStyleBackColor = true;
            this.btReadGprsParam.Click += new System.EventHandler(this.btReadGprsParam_Click);
            // 
            // btSetGprsParam
            // 
            this.btSetGprsParam.Location = new System.Drawing.Point(196, 16);
            this.btSetGprsParam.Name = "btSetGprsParam";
            this.btSetGprsParam.Size = new System.Drawing.Size(157, 23);
            this.btSetGprsParam.TabIndex = 56;
            this.btSetGprsParam.Text = "设置GPRS参数";
            this.tipInfo.SetToolTip(this.btSetGprsParam, "设置集中器中连接服务器的参数。");
            this.btSetGprsParam.UseVisualStyleBackColor = true;
            this.btSetGprsParam.Click += new System.EventHandler(this.btSetGprsParam_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label16);
            this.groupBox3.Controls.Add(this.label15);
            this.groupBox3.Controls.Add(this.pgbGrpsRssi);
            this.groupBox3.Controls.Add(this.label18);
            this.groupBox3.Controls.Add(this.lbIMSI);
            this.groupBox3.Controls.Add(this.lbConnectStatus);
            this.groupBox3.Controls.Add(this.lbGprsModle);
            this.groupBox3.Location = new System.Drawing.Point(13, 252);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(352, 83);
            this.groupBox3.TabIndex = 70;
            this.groupBox3.TabStop = false;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(251, 61);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(19, 13);
            this.label16.TabIndex = 6;
            this.label16.Text = "强";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(81, 61);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(19, 13);
            this.label15.TabIndex = 5;
            this.label15.Text = "弱";
            // 
            // pgbGrpsRssi
            // 
            this.pgbGrpsRssi.Enabled = false;
            this.pgbGrpsRssi.Location = new System.Drawing.Point(98, 61);
            this.pgbGrpsRssi.MarqueeAnimationSpeed = 1000;
            this.pgbGrpsRssi.Maximum = 31;
            this.pgbGrpsRssi.Name = "pgbGrpsRssi";
            this.pgbGrpsRssi.Size = new System.Drawing.Size(150, 12);
            this.pgbGrpsRssi.Step = 3;
            this.pgbGrpsRssi.TabIndex = 4;
            this.tipInfo.SetToolTip(this.pgbGrpsRssi, "GPRS信号强度");
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(6, 61);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(67, 13);
            this.label18.TabIndex = 3;
            this.label18.Text = "信号强度：";
            this.tipInfo.SetToolTip(this.label18, "当前GPRS的信号强度。");
            // 
            // lbIMSI
            // 
            this.lbIMSI.AutoSize = true;
            this.lbIMSI.Location = new System.Drawing.Point(6, 39);
            this.lbIMSI.Name = "lbIMSI";
            this.lbIMSI.Size = new System.Drawing.Size(65, 13);
            this.lbIMSI.TabIndex = 2;
            this.lbIMSI.Text = "IMSI编码：";
            this.tipInfo.SetToolTip(this.lbIMSI, "GPRS模块中SIM卡的IMSI编码。");
            // 
            // lbConnectStatus
            // 
            this.lbConnectStatus.AutoSize = true;
            this.lbConnectStatus.Location = new System.Drawing.Point(221, 17);
            this.lbConnectStatus.Name = "lbConnectStatus";
            this.lbConnectStatus.Size = new System.Drawing.Size(91, 13);
            this.lbConnectStatus.TabIndex = 1;
            this.lbConnectStatus.Text = "联机状态：未知";
            this.tipInfo.SetToolTip(this.lbConnectStatus, "服务器当前联机状态。");
            // 
            // lbGprsModle
            // 
            this.lbGprsModle.AutoSize = true;
            this.lbGprsModle.Location = new System.Drawing.Point(6, 17);
            this.lbGprsModle.Name = "lbGprsModle";
            this.lbGprsModle.Size = new System.Drawing.Size(67, 13);
            this.lbGprsModle.TabIndex = 0;
            this.lbGprsModle.Text = "模块型号：";
            this.tipInfo.SetToolTip(this.lbGprsModle, "集中器使用的GPRS模块型号。");
            // 
            // gpbGprsInfo
            // 
            this.gpbGprsInfo.Controls.Add(this.rtbGprsMsg);
            this.gpbGprsInfo.Location = new System.Drawing.Point(13, 341);
            this.gpbGprsInfo.Name = "gpbGprsInfo";
            this.gpbGprsInfo.Size = new System.Drawing.Size(352, 279);
            this.gpbGprsInfo.TabIndex = 68;
            this.gpbGprsInfo.TabStop = false;
            this.gpbGprsInfo.Text = "Gprs连接信息（串口或USB端口有效）";
            this.tipInfo.SetToolTip(this.gpbGprsInfo, "Gprs连接的监控信息。");
            // 
            // rtbGprsMsg
            // 
            this.rtbGprsMsg.AccessibleRole = System.Windows.Forms.AccessibleRole.TitleBar;
            this.rtbGprsMsg.BackColor = System.Drawing.Color.Snow;
            this.rtbGprsMsg.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbGprsMsg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbGprsMsg.Font = new System.Drawing.Font("新宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rtbGprsMsg.ForeColor = System.Drawing.Color.Navy;
            this.rtbGprsMsg.Location = new System.Drawing.Point(3, 16);
            this.rtbGprsMsg.Name = "rtbGprsMsg";
            this.rtbGprsMsg.ReadOnly = true;
            this.rtbGprsMsg.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedBoth;
            this.rtbGprsMsg.Size = new System.Drawing.Size(346, 260);
            this.rtbGprsMsg.TabIndex = 46;
            this.rtbGprsMsg.Text = "";
            this.tipInfo.SetToolTip(this.rtbGprsMsg, "GPRS模块的连接信息，只在串口连接和USB连接时有效。");
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.nudHeatBeat);
            this.groupBox2.Controls.Add(this.tbPassword);
            this.groupBox2.Controls.Add(this.lbPassword);
            this.groupBox2.Controls.Add(this.tbServerIp02);
            this.groupBox2.Controls.Add(this.tbServerIp03);
            this.groupBox2.Controls.Add(this.tbServerIp01);
            this.groupBox2.Controls.Add(this.lbServerIP0);
            this.groupBox2.Controls.Add(this.lbUnit);
            this.groupBox2.Controls.Add(this.lbDot02);
            this.groupBox2.Controls.Add(this.lbHeatBeat);
            this.groupBox2.Controls.Add(this.tbServerIp00);
            this.groupBox2.Controls.Add(this.lbServerIP1);
            this.groupBox2.Controls.Add(this.tbApn);
            this.groupBox2.Controls.Add(this.lbPort1);
            this.groupBox2.Controls.Add(this.tbServerPort0);
            this.groupBox2.Controls.Add(this.tbServerIp10);
            this.groupBox2.Controls.Add(this.lbPort0);
            this.groupBox2.Controls.Add(this.tbUsername);
            this.groupBox2.Controls.Add(this.tbServerPort1);
            this.groupBox2.Controls.Add(this.lbDot1);
            this.groupBox2.Controls.Add(this.tbServerIp13);
            this.groupBox2.Controls.Add(this.tbServerIp11);
            this.groupBox2.Controls.Add(this.lbUsername);
            this.groupBox2.Controls.Add(this.tbServerIp12);
            this.groupBox2.Controls.Add(this.lbApn);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.label13);
            this.groupBox2.Controls.Add(this.lbDot0);
            this.groupBox2.Controls.Add(this.label14);
            this.groupBox2.Location = new System.Drawing.Point(13, 36);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(352, 186);
            this.groupBox2.TabIndex = 67;
            this.groupBox2.TabStop = false;
            // 
            // nudHeatBeat
            // 
            this.nudHeatBeat.BackColor = System.Drawing.Color.White;
            this.nudHeatBeat.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudHeatBeat.Location = new System.Drawing.Point(98, 69);
            this.nudHeatBeat.Maximum = new decimal(new int[] {
            600,
            0,
            0,
            0});
            this.nudHeatBeat.Minimum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.nudHeatBeat.Name = "nudHeatBeat";
            this.nudHeatBeat.ReadOnly = true;
            this.nudHeatBeat.Size = new System.Drawing.Size(65, 20);
            this.nudHeatBeat.TabIndex = 69;
            this.nudHeatBeat.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tipInfo.SetToolTip(this.nudHeatBeat, "集中器向服务器发送心跳包的时间间隔。");
            this.nudHeatBeat.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            // 
            // tbPassword
            // 
            this.tbPassword.Location = new System.Drawing.Point(98, 157);
            this.tbPassword.MaxLength = 12;
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.Size = new System.Drawing.Size(177, 20);
            this.tbPassword.TabIndex = 68;
            this.tbPassword.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tipInfo.SetToolTip(this.tbPassword, "连接运营商时使用的密码。");
            this.tbPassword.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbApnInfo_KeyPress);
            // 
            // lbPassword
            // 
            this.lbPassword.AutoSize = true;
            this.lbPassword.Location = new System.Drawing.Point(6, 161);
            this.lbPassword.Name = "lbPassword";
            this.lbPassword.Size = new System.Drawing.Size(37, 13);
            this.lbPassword.TabIndex = 67;
            this.lbPassword.Text = "密  码";
            // 
            // tbServerIp02
            // 
            this.tbServerIp02.Location = new System.Drawing.Point(170, 13);
            this.tbServerIp02.Name = "tbServerIp02";
            this.tbServerIp02.Size = new System.Drawing.Size(30, 20);
            this.tbServerIp02.TabIndex = 39;
            this.tbServerIp02.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tipInfo.SetToolTip(this.tbServerIp02, "首选服务器IP地址。");
            this.tbServerIp02.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbServerIp_KeyPress);
            // 
            // tbServerIp03
            // 
            this.tbServerIp03.Location = new System.Drawing.Point(206, 13);
            this.tbServerIp03.Name = "tbServerIp03";
            this.tbServerIp03.Size = new System.Drawing.Size(30, 20);
            this.tbServerIp03.TabIndex = 40;
            this.tbServerIp03.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tipInfo.SetToolTip(this.tbServerIp03, "首选服务器IP地址。");
            this.tbServerIp03.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbServerIp_KeyPress);
            // 
            // tbServerIp01
            // 
            this.tbServerIp01.Location = new System.Drawing.Point(134, 13);
            this.tbServerIp01.Name = "tbServerIp01";
            this.tbServerIp01.Size = new System.Drawing.Size(30, 20);
            this.tbServerIp01.TabIndex = 37;
            this.tbServerIp01.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tipInfo.SetToolTip(this.tbServerIp01, "首选服务器IP地址。");
            this.tbServerIp01.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbServerIp_KeyPress);
            // 
            // lbServerIP0
            // 
            this.lbServerIP0.AutoSize = true;
            this.lbServerIP0.Location = new System.Drawing.Point(6, 17);
            this.lbServerIP0.Name = "lbServerIP0";
            this.lbServerIP0.Size = new System.Drawing.Size(91, 13);
            this.lbServerIP0.TabIndex = 33;
            this.lbServerIP0.Text = "首选服务器地址";
            this.tipInfo.SetToolTip(this.lbServerIP0, "首选服务器IP地址。");
            // 
            // lbUnit
            // 
            this.lbUnit.AutoSize = true;
            this.lbUnit.Location = new System.Drawing.Point(165, 73);
            this.lbUnit.Name = "lbUnit";
            this.lbUnit.Size = new System.Drawing.Size(19, 13);
            this.lbUnit.TabIndex = 66;
            this.lbUnit.Text = "秒";
            // 
            // lbDot02
            // 
            this.lbDot02.AutoSize = true;
            this.lbDot02.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbDot02.Location = new System.Drawing.Point(198, 16);
            this.lbDot02.Name = "lbDot02";
            this.lbDot02.Size = new System.Drawing.Size(14, 14);
            this.lbDot02.TabIndex = 57;
            this.lbDot02.Text = ".";
            // 
            // lbHeatBeat
            // 
            this.lbHeatBeat.AutoSize = true;
            this.lbHeatBeat.Location = new System.Drawing.Point(6, 73);
            this.lbHeatBeat.Name = "lbHeatBeat";
            this.lbHeatBeat.Size = new System.Drawing.Size(67, 13);
            this.lbHeatBeat.TabIndex = 65;
            this.lbHeatBeat.Text = "心跳包间隔";
            // 
            // tbServerIp00
            // 
            this.tbServerIp00.Location = new System.Drawing.Point(98, 13);
            this.tbServerIp00.Name = "tbServerIp00";
            this.tbServerIp00.Size = new System.Drawing.Size(30, 20);
            this.tbServerIp00.TabIndex = 34;
            this.tbServerIp00.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tipInfo.SetToolTip(this.tbServerIp00, "首选服务器IP地址。");
            this.tbServerIp00.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbServerIp_KeyPress);
            // 
            // lbServerIP1
            // 
            this.lbServerIP1.AutoSize = true;
            this.lbServerIP1.Location = new System.Drawing.Point(6, 45);
            this.lbServerIP1.Name = "lbServerIP1";
            this.lbServerIP1.Size = new System.Drawing.Size(91, 13);
            this.lbServerIP1.TabIndex = 60;
            this.lbServerIP1.Text = "备用服务器地址";
            // 
            // tbApn
            // 
            this.tbApn.Location = new System.Drawing.Point(98, 98);
            this.tbApn.MaxLength = 12;
            this.tbApn.Name = "tbApn";
            this.tbApn.Size = new System.Drawing.Size(177, 20);
            this.tbApn.TabIndex = 51;
            this.tbApn.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tipInfo.SetToolTip(this.tbApn, "连接运营商时使用的接口。");
            this.tbApn.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbApnInfo_KeyPress);
            // 
            // lbPort1
            // 
            this.lbPort1.AutoSize = true;
            this.lbPort1.Location = new System.Drawing.Point(246, 50);
            this.lbPort1.Name = "lbPort1";
            this.lbPort1.Size = new System.Drawing.Size(31, 13);
            this.lbPort1.TabIndex = 61;
            this.lbPort1.Text = "端口";
            // 
            // tbServerPort0
            // 
            this.tbServerPort0.Location = new System.Drawing.Point(283, 13);
            this.tbServerPort0.Name = "tbServerPort0";
            this.tbServerPort0.Size = new System.Drawing.Size(57, 20);
            this.tbServerPort0.TabIndex = 42;
            this.tbServerPort0.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tipInfo.SetToolTip(this.tbServerPort0, "首选服务器连接端口。");
            this.tbServerPort0.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbServerPort_KeyPress);
            // 
            // tbServerIp10
            // 
            this.tbServerIp10.Location = new System.Drawing.Point(98, 41);
            this.tbServerIp10.Name = "tbServerIp10";
            this.tbServerIp10.Size = new System.Drawing.Size(30, 20);
            this.tbServerIp10.TabIndex = 43;
            this.tbServerIp10.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tipInfo.SetToolTip(this.tbServerIp10, "备用服务器IP地址。");
            this.tbServerIp10.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbServerIp_KeyPress);
            // 
            // lbPort0
            // 
            this.lbPort0.AutoSize = true;
            this.lbPort0.Location = new System.Drawing.Point(246, 17);
            this.lbPort0.Name = "lbPort0";
            this.lbPort0.Size = new System.Drawing.Size(31, 13);
            this.lbPort0.TabIndex = 35;
            this.lbPort0.Text = "端口";
            // 
            // tbUsername
            // 
            this.tbUsername.Location = new System.Drawing.Point(98, 127);
            this.tbUsername.MaxLength = 12;
            this.tbUsername.Name = "tbUsername";
            this.tbUsername.Size = new System.Drawing.Size(177, 20);
            this.tbUsername.TabIndex = 52;
            this.tbUsername.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tipInfo.SetToolTip(this.tbUsername, "连接运营商时使用的用户名。");
            this.tbUsername.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbApnInfo_KeyPress);
            // 
            // tbServerPort1
            // 
            this.tbServerPort1.Location = new System.Drawing.Point(283, 43);
            this.tbServerPort1.Name = "tbServerPort1";
            this.tbServerPort1.Size = new System.Drawing.Size(57, 20);
            this.tbServerPort1.TabIndex = 48;
            this.tbServerPort1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tipInfo.SetToolTip(this.tbServerPort1, "备用服务器连接端口。");
            this.tbServerPort1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbServerPort_KeyPress);
            // 
            // lbDot1
            // 
            this.lbDot1.AutoSize = true;
            this.lbDot1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbDot1.Location = new System.Drawing.Point(162, 16);
            this.lbDot1.Name = "lbDot1";
            this.lbDot1.Size = new System.Drawing.Size(14, 14);
            this.lbDot1.TabIndex = 55;
            this.lbDot1.Text = ".";
            // 
            // tbServerIp13
            // 
            this.tbServerIp13.Location = new System.Drawing.Point(206, 41);
            this.tbServerIp13.Name = "tbServerIp13";
            this.tbServerIp13.Size = new System.Drawing.Size(30, 20);
            this.tbServerIp13.TabIndex = 47;
            this.tbServerIp13.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tipInfo.SetToolTip(this.tbServerIp13, "备用服务器IP地址。");
            this.tbServerIp13.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbServerIp_KeyPress);
            // 
            // tbServerIp11
            // 
            this.tbServerIp11.Location = new System.Drawing.Point(134, 41);
            this.tbServerIp11.Name = "tbServerIp11";
            this.tbServerIp11.Size = new System.Drawing.Size(30, 20);
            this.tbServerIp11.TabIndex = 44;
            this.tbServerIp11.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tipInfo.SetToolTip(this.tbServerIp11, "备用服务器IP地址。");
            this.tbServerIp11.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbServerIp_KeyPress);
            // 
            // lbUsername
            // 
            this.lbUsername.AutoSize = true;
            this.lbUsername.Location = new System.Drawing.Point(6, 131);
            this.lbUsername.Name = "lbUsername";
            this.lbUsername.Size = new System.Drawing.Size(43, 13);
            this.lbUsername.TabIndex = 38;
            this.lbUsername.Text = "用户名";
            // 
            // tbServerIp12
            // 
            this.tbServerIp12.Location = new System.Drawing.Point(170, 41);
            this.tbServerIp12.Name = "tbServerIp12";
            this.tbServerIp12.Size = new System.Drawing.Size(30, 20);
            this.tbServerIp12.TabIndex = 45;
            this.tbServerIp12.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tipInfo.SetToolTip(this.tbServerIp12, "备用服务器IP地址。");
            this.tbServerIp12.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbServerIp_KeyPress);
            // 
            // lbApn
            // 
            this.lbApn.AutoSize = true;
            this.lbApn.Location = new System.Drawing.Point(6, 102);
            this.lbApn.Name = "lbApn";
            this.lbApn.Size = new System.Drawing.Size(53, 13);
            this.lbApn.TabIndex = 36;
            this.lbApn.Text = "APN接口";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label12.Location = new System.Drawing.Point(162, 44);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(14, 14);
            this.label12.TabIndex = 63;
            this.label12.Text = ".";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label13.Location = new System.Drawing.Point(198, 44);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(14, 14);
            this.label13.TabIndex = 64;
            this.label13.Text = ".";
            // 
            // lbDot0
            // 
            this.lbDot0.AutoSize = true;
            this.lbDot0.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbDot0.Location = new System.Drawing.Point(126, 16);
            this.lbDot0.Name = "lbDot0";
            this.lbDot0.Size = new System.Drawing.Size(14, 14);
            this.lbDot0.TabIndex = 53;
            this.lbDot0.Text = ".";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label14.Location = new System.Drawing.Point(126, 44);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(14, 14);
            this.label14.TabIndex = 62;
            this.label14.Text = ".";
            // 
            // cmsMenu
            // 
            this.cmsMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiAddToForwardList,
            this.tsmiClearDocument});
            this.cmsMenu.Name = "cmsAddTask";
            this.cmsMenu.Size = new System.Drawing.Size(185, 48);
            this.cmsMenu.Opening += new System.ComponentModel.CancelEventHandler(this.cmsMenu_Opening);
            // 
            // tsmiAddToForwardList
            // 
            this.tsmiAddToForwardList.Name = "tsmiAddToForwardList";
            this.tsmiAddToForwardList.Size = new System.Drawing.Size(184, 22);
            this.tsmiAddToForwardList.Text = "添加到数据转发队列";
            this.tsmiAddToForwardList.Click += new System.EventHandler(this.tsmiAddToForwardList_Click);
            // 
            // tsmiClearDocument
            // 
            this.tsmiClearDocument.Name = "tsmiClearDocument";
            this.tsmiClearDocument.Size = new System.Drawing.Size(184, 22);
            this.tsmiClearDocument.Text = "清空所有档案";
            this.tsmiClearDocument.Click += new System.EventHandler(this.tsmiClearDocument_Click);
            // 
            // tbConcAddr
            // 
            this.tbConcAddr.Location = new System.Drawing.Point(617, 10);
            this.tbConcAddr.Name = "tbConcAddr";
            this.tbConcAddr.Size = new System.Drawing.Size(128, 20);
            this.tbConcAddr.TabIndex = 4;
            this.tbConcAddr.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tipInfo.SetToolTip(this.tbConcAddr, "集中器编号为16位数字，不足16位时左边自动补零。");
            this.tbConcAddr.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbConcAddr_KeyPress);
            this.tbConcAddr.Leave += new System.EventHandler(this.tbConcAddr_Leave);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(550, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "集中器编号";
            // 
            // btReadConcAddr
            // 
            this.btReadConcAddr.BackColor = System.Drawing.Color.Snow;
            this.btReadConcAddr.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btReadConcAddr.Location = new System.Drawing.Point(751, 10);
            this.btReadConcAddr.Name = "btReadConcAddr";
            this.btReadConcAddr.Size = new System.Drawing.Size(123, 23);
            this.btReadConcAddr.TabIndex = 0;
            this.btReadConcAddr.Text = "读取集中器编号";
            this.tipInfo.SetToolTip(this.btReadConcAddr, "读取集中器编号，在无线通信类型时需要手动输入集中器编号。");
            this.btReadConcAddr.UseVisualStyleBackColor = true;
            this.btReadConcAddr.Click += new System.EventHandler(this.btReadConcAddr_Click);
            // 
            // scon1
            // 
            this.scon1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.scon1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scon1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.scon1.IsSplitterFixed = true;
            this.scon1.Location = new System.Drawing.Point(3, 3);
            this.scon1.Name = "scon1";
            this.scon1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // scon1.Panel1
            // 
            this.scon1.Panel1.BackColor = System.Drawing.Color.Snow;
            this.scon1.Panel1.Controls.Add(this.label20);
            this.scon1.Panel1.Controls.Add(this.btPortCtrl);
            this.scon1.Panel1.Controls.Add(this.cmbComType);
            this.scon1.Panel1.Controls.Add(this.cmbPort);
            this.scon1.Panel1.Controls.Add(this.label1);
            this.scon1.Panel1.Controls.Add(this.tbConcAddr);
            this.scon1.Panel1.Controls.Add(this.label2);
            this.scon1.Panel1.Controls.Add(this.btReadConcAddr);
            // 
            // scon1.Panel2
            // 
            this.scon1.Panel2.BackColor = System.Drawing.Color.Snow;
            this.scon1.Panel2.Controls.Add(this.scon2);
            this.scon1.Panel2.Enabled = false;
            this.scon1.Size = new System.Drawing.Size(1422, 701);
            this.scon1.SplitterDistance = 43;
            this.scon1.TabIndex = 0;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(198, 14);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(55, 13);
            this.label20.TabIndex = 4;
            this.label20.Text = "通讯类型";
            // 
            // btPortCtrl
            // 
            this.btPortCtrl.Location = new System.Drawing.Point(380, 9);
            this.btPortCtrl.Name = "btPortCtrl";
            this.btPortCtrl.Size = new System.Drawing.Size(114, 23);
            this.btPortCtrl.TabIndex = 3;
            this.btPortCtrl.Text = "打开端口";
            this.tipInfo.SetToolTip(this.btPortCtrl, "打开或关闭通信端口。");
            this.btPortCtrl.UseVisualStyleBackColor = true;
            this.btPortCtrl.Click += new System.EventHandler(this.btPortCtrl_Click);
            // 
            // cmbComType
            // 
            this.cmbComType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbComType.FormattingEnabled = true;
            this.cmbComType.Items.AddRange(new object[] {
            "串口通信",
            "无线通信",
            "485通信",
            "USB通信"});
            this.cmbComType.Location = new System.Drawing.Point(261, 10);
            this.cmbComType.Name = "cmbComType";
            this.cmbComType.Size = new System.Drawing.Size(85, 21);
            this.cmbComType.TabIndex = 2;
            this.tipInfo.SetToolTip(this.cmbComType, "选择通信类型。");
            // 
            // cmbPort
            // 
            this.cmbPort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPort.FormattingEnabled = true;
            this.cmbPort.Location = new System.Drawing.Point(87, 10);
            this.cmbPort.Name = "cmbPort";
            this.cmbPort.Size = new System.Drawing.Size(85, 21);
            this.cmbPort.TabIndex = 1;
            this.tipInfo.SetToolTip(this.cmbPort, "选择通信接口");
            this.cmbPort.SelectedIndexChanged += new System.EventHandler(this.cmbPort_SelectedIndexChanged);
            this.cmbPort.Click += new System.EventHandler(this.cmbPort_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "端口选择";
            // 
            // scon2
            // 
            this.scon2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scon2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.scon2.Location = new System.Drawing.Point(0, 0);
            this.scon2.Name = "scon2";
            // 
            // scon2.Panel1
            // 
            this.scon2.Panel1.Controls.Add(this.scon3);
            this.scon2.Panel1MinSize = 0;
            // 
            // scon2.Panel2
            // 
            this.scon2.Panel2.Controls.Add(this.tbPageRecord);
            this.scon2.Panel2MinSize = 0;
            this.scon2.Size = new System.Drawing.Size(1420, 652);
            this.scon2.SplitterDistance = 1338;
            this.scon2.TabIndex = 3;
            this.scon2.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.scon2_splitterMoved);
            // 
            // scon3
            // 
            this.scon3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scon3.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.scon3.IsSplitterFixed = true;
            this.scon3.Location = new System.Drawing.Point(0, 0);
            this.scon3.Name = "scon3";
            // 
            // scon3.Panel1
            // 
            this.scon3.Panel1.Controls.Add(this.tabControl);
            // 
            // scon3.Panel2
            // 
            this.scon3.Panel2.Controls.Add(this.tbPageDocument);
            this.scon3.Size = new System.Drawing.Size(1338, 652);
            this.scon3.SplitterDistance = 384;
            this.scon3.TabIndex = 2;
            // 
            // tbPageDocument
            // 
            this.tbPageDocument.Controls.Add(this.tbPageRec);
            this.tbPageDocument.Controls.Add(this.tbPageDocDS);
            this.tbPageDocument.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbPageDocument.ItemSize = new System.Drawing.Size(72, 18);
            this.tbPageDocument.Location = new System.Drawing.Point(0, 0);
            this.tbPageDocument.Name = "tbPageDocument";
            this.tbPageDocument.SelectedIndex = 0;
            this.tbPageDocument.Size = new System.Drawing.Size(950, 652);
            this.tbPageDocument.TabIndex = 0;
            // 
            // tbPageRec
            // 
            this.tbPageRec.Controls.Add(this.rtbCommMsg);
            this.tbPageRec.Location = new System.Drawing.Point(4, 22);
            this.tbPageRec.Name = "tbPageRec";
            this.tbPageRec.Padding = new System.Windows.Forms.Padding(3);
            this.tbPageRec.Size = new System.Drawing.Size(942, 626);
            this.tbPageRec.TabIndex = 1;
            this.tbPageRec.Text = " 通信日志 ";
            this.tbPageRec.UseVisualStyleBackColor = true;
            // 
            // rtbCommMsg
            // 
            this.rtbCommMsg.BackColor = System.Drawing.Color.Snow;
            this.rtbCommMsg.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.rtbCommMsg.ContextMenuStrip = this.cntMenuStripCommInfo;
            this.rtbCommMsg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbCommMsg.Location = new System.Drawing.Point(3, 3);
            this.rtbCommMsg.Name = "rtbCommMsg";
            this.rtbCommMsg.ReadOnly = true;
            this.rtbCommMsg.Size = new System.Drawing.Size(936, 620);
            this.rtbCommMsg.TabIndex = 0;
            this.rtbCommMsg.Text = "";
            this.tipInfo.SetToolTip(this.rtbCommMsg, "和集中器通信的监控记录，右键进行相关操作。");
            // 
            // cntMenuStripCommInfo
            // 
            this.cntMenuStripCommInfo.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiAutoScroll,
            this.tsmiAutoClear,
            this.tsmiClearAll,
            this.tsmiSaveRecord});
            this.cntMenuStripCommInfo.Name = "cntMenuStripCommInfo";
            this.cntMenuStripCommInfo.Size = new System.Drawing.Size(125, 92);
            this.cntMenuStripCommInfo.Opening += new System.ComponentModel.CancelEventHandler(this.cntMenuStripCommInfo_Opening);
            // 
            // tsmiAutoScroll
            // 
            this.tsmiAutoScroll.Checked = true;
            this.tsmiAutoScroll.CheckOnClick = true;
            this.tsmiAutoScroll.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tsmiAutoScroll.Name = "tsmiAutoScroll";
            this.tsmiAutoScroll.Size = new System.Drawing.Size(124, 22);
            this.tsmiAutoScroll.Text = "自动滚动";
            this.tsmiAutoScroll.Click += new System.EventHandler(this.tsmiAutoScroll_Click);
            // 
            // tsmiAutoClear
            // 
            this.tsmiAutoClear.CheckOnClick = true;
            this.tsmiAutoClear.Name = "tsmiAutoClear";
            this.tsmiAutoClear.Size = new System.Drawing.Size(124, 22);
            this.tsmiAutoClear.Text = "自动清空";
            this.tsmiAutoClear.Click += new System.EventHandler(this.tsmiAutoClear_Click);
            // 
            // tsmiClearAll
            // 
            this.tsmiClearAll.Name = "tsmiClearAll";
            this.tsmiClearAll.Size = new System.Drawing.Size(124, 22);
            this.tsmiClearAll.Text = "清空全部";
            this.tsmiClearAll.Click += new System.EventHandler(this.tsmiClearAll_Click);
            // 
            // tsmiSaveRecord
            // 
            this.tsmiSaveRecord.Name = "tsmiSaveRecord";
            this.tsmiSaveRecord.Size = new System.Drawing.Size(124, 22);
            this.tsmiSaveRecord.Text = "保存记录";
            this.tsmiSaveRecord.Click += new System.EventHandler(this.tsmiSaveRecord_Click);
            // 
            // tbPageDocDS
            // 
            this.tbPageDocDS.BackColor = System.Drawing.Color.Snow;
            this.tbPageDocDS.Controls.Add(this.dgvDocDS);
            this.tbPageDocDS.Location = new System.Drawing.Point(4, 22);
            this.tbPageDocDS.Name = "tbPageDocDS";
            this.tbPageDocDS.Padding = new System.Windows.Forms.Padding(3);
            this.tbPageDocDS.Size = new System.Drawing.Size(942, 626);
            this.tbPageDocDS.TabIndex = 0;
            this.tbPageDocDS.Text = " 地锁数据 ";
            // 
            // dgvDocDS
            // 
            this.dgvDocDS.AllowUserToAddRows = false;
            this.dgvDocDS.AllowUserToDeleteRows = false;
            this.dgvDocDS.AllowUserToOrderColumns = true;
            this.dgvDocDS.AllowUserToResizeRows = false;
            this.dgvDocDS.BackgroundColor = System.Drawing.Color.Snow;
            this.dgvDocDS.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvDocDS.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgvDocDS.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvDocDS.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvDocDS.ContextMenuStrip = this.cmsMenu;
            this.dgvDocDS.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDocDS.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvDocDS.GridColor = System.Drawing.Color.DarkGreen;
            this.dgvDocDS.Location = new System.Drawing.Point(3, 3);
            this.dgvDocDS.Name = "dgvDocDS";
            this.dgvDocDS.ReadOnly = true;
            this.dgvDocDS.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgvDocDS.RowHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvDocDS.RowHeadersVisible = false;
            this.dgvDocDS.RowHeadersWidth = 45;
            this.dgvDocDS.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvDocDS.RowTemplate.Height = 23;
            this.dgvDocDS.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDocDS.ShowCellErrors = false;
            this.dgvDocDS.ShowEditingIcon = false;
            this.dgvDocDS.ShowRowErrors = false;
            this.dgvDocDS.Size = new System.Drawing.Size(936, 620);
            this.dgvDocDS.TabIndex = 0;
            this.dgvDocDS.ColumnDisplayIndexChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this.dgvDocDS_ColumnDisplayIndexChanged);
            this.dgvDocDS.SelectionChanged += new System.EventHandler(this.dgvDocDS_SelectionChanged);
            // 
            // tbPageRecord
            // 
            this.tbPageRecord.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbPageRecord.Location = new System.Drawing.Point(0, 0);
            this.tbPageRecord.Name = "tbPageRecord";
            this.tbPageRecord.SelectedIndex = 0;
            this.tbPageRecord.Size = new System.Drawing.Size(78, 652);
            this.tbPageRecord.TabIndex = 0;
            // 
            // serialPort
            // 
            this.serialPort.BaudRate = 115200;
            this.serialPort.Parity = System.IO.Ports.Parity.Even;
            this.serialPort.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.serialPort_DataReceived);
            // 
            // timer
            // 
            this.timer.Enabled = true;
            this.timer.Interval = 10;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // tipInfo
            // 
            this.tipInfo.AutoPopDelay = 8000;
            this.tipInfo.ForeColor = System.Drawing.Color.Navy;
            this.tipInfo.InitialDelay = 500;
            this.tipInfo.ReshowDelay = 100;
            this.tipInfo.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.tipInfo.ToolTipTitle = "6009集中器调试工具";
            // 
            // openFileDlg
            // 
            this.openFileDlg.FileName = "openFileDialog1";
            // 
            // usbHidPort
            // 
            this.usbHidPort.ProductId = 81;
            this.usbHidPort.VendorId = 1105;
            this.usbHidPort.OnSpecifiedDeviceArrived += new System.EventHandler(this.usbHidPort_OnSpecifiedDeviceArrived);
            this.usbHidPort.OnSpecifiedDeviceRemoved += new System.EventHandler(this.usbHidPort_OnSpecifiedDeviceRemoved);
            this.usbHidPort.OnDataRecieved += new UsbLibrary.DataRecievedEventHandler(this.usbHidPort_OnDataRecieved);
            // 
            // FrmMain
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.Snow;
            this.ClientSize = new System.Drawing.Size(1428, 730);
            this.Controls.Add(this.scon1);
            this.Controls.Add(this.statusMainStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(1360, 768);
            this.Name = "FrmMain";
            this.Padding = new System.Windows.Forms.Padding(3);
            this.Text = "CTP共享停车集中器V1.00";
            this.Load += new System.EventHandler(this.FrmMain_Load);
            this.SizeChanged += new System.EventHandler(this.FrmMain_SizeChanged);
            this.statusMainStrip.ResumeLayout(false);
            this.statusMainStrip.PerformLayout();
            this.tabControl.ResumeLayout(false);
            this.tbDevParam.ResumeLayout(false);
            this.tbDevParam.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.HostChannel)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.gpbNodeEdit.ResumeLayout(false);
            this.gpbNodeEdit.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.tbGprsParam.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.gpbGprsInfo.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudHeatBeat)).EndInit();
            this.cmsMenu.ResumeLayout(false);
            this.scon1.Panel1.ResumeLayout(false);
            this.scon1.Panel1.PerformLayout();
            this.scon1.Panel2.ResumeLayout(false);
            this.scon1.ResumeLayout(false);
            this.scon2.Panel1.ResumeLayout(false);
            this.scon2.Panel2.ResumeLayout(false);
            this.scon2.ResumeLayout(false);
            this.scon3.Panel1.ResumeLayout(false);
            this.scon3.Panel2.ResumeLayout(false);
            this.scon3.ResumeLayout(false);
            this.tbPageDocument.ResumeLayout(false);
            this.tbPageRec.ResumeLayout(false);
            this.cntMenuStripCommInfo.ResumeLayout(false);
            this.tbPageDocDS.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDocDS)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusMainStrip;
        private System.Windows.Forms.ToolStripStatusLabel tsComPortInfo;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.ToolStripStatusLabel tsLocalAddress;
        private System.Windows.Forms.SplitContainer scon1;
        private System.Windows.Forms.SplitContainer scon3;
        private System.Windows.Forms.TabPage tbDevParam;
        private System.Windows.Forms.TabPage tbGprsParam;
        private System.Windows.Forms.Button btPortCtrl;
        private System.Windows.Forms.ComboBox cmbComType;
        private System.Windows.Forms.ComboBox cmbPort;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbNewConcAddr;
        private System.Windows.Forms.TextBox tbConcAddr;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btSetConcAddr;
        private System.Windows.Forms.Button btReadConcAddr;
        private System.Windows.Forms.Button btSetRtc;
        private System.Windows.Forms.Button btReadRtc;
        private System.Windows.Forms.Button btReadDevVerInfo;
        private System.Windows.Forms.Button btRestartDev;
        private System.Windows.Forms.DateTimePicker dtpTime;
        private System.Windows.Forms.DateTimePicker dtpDate;
        private System.Windows.Forms.ComboBox cmbTimeType;
        private System.Windows.Forms.Label lbUnit;
        private System.Windows.Forms.Label lbHeatBeat;
        private System.Windows.Forms.Label lbServerIP1;
        private System.Windows.Forms.Label lbPort1;
        private System.Windows.Forms.TextBox tbServerIp10;
        private System.Windows.Forms.Label lbPort0;
        private System.Windows.Forms.TextBox tbServerPort1;
        private System.Windows.Forms.TextBox tbServerIp13;
        private System.Windows.Forms.Label lbServerIP0;
        private System.Windows.Forms.TextBox tbServerIp11;
        private System.Windows.Forms.Button btReadGprsParam;
        private System.Windows.Forms.TextBox tbServerIp12;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button btSetGprsParam;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label lbApn;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox tbServerIp03;
        private System.Windows.Forms.Label lbUsername;
        private System.Windows.Forms.TextBox tbServerIp02;
        private System.Windows.Forms.Label lbDot1;
        private System.Windows.Forms.TextBox tbUsername;
        private System.Windows.Forms.TextBox tbServerIp01;
        private System.Windows.Forms.TextBox tbServerPort0;
        private System.Windows.Forms.TextBox tbApn;
        private System.Windows.Forms.TextBox tbServerIp00;
        private System.Windows.Forms.Label lbDot0;
        private System.Windows.Forms.Label lbDot02;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label lbIMSI;
        private System.Windows.Forms.Label lbConnectStatus;
        private System.Windows.Forms.Label lbGprsModle;
        private System.Windows.Forms.Button btReadGprsInfo;
        private System.Windows.Forms.GroupBox gpbGprsInfo;
        private System.Windows.Forms.RichTextBox rtbGprsMsg;
        private System.Windows.Forms.Button btMemCheck;
        private System.IO.Ports.SerialPort serialPort;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.ToolStripProgressBar prgBar;
        private System.Windows.Forms.Label lbVerInfo;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.TextBox tbPassword;
        private System.Windows.Forms.Label lbPassword;
        private System.Windows.Forms.NumericUpDown nudHeatBeat;
        private System.Windows.Forms.ToolTip tipInfo;
        private System.Windows.Forms.ProgressBar pgbGrpsRssi;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.SplitContainer scon2;
        private System.Windows.Forms.TabControl tbPageRecord;
        private System.Windows.Forms.OpenFileDialog openFileDlg;
        private System.Windows.Forms.SaveFileDialog saveFileDlg;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel4;
        private System.Windows.Forms.ToolStripStatusLabel lbCurState;
        private UsbLibrary.UsbHidPort usbHidPort;
        private System.Windows.Forms.ToolStripStatusLabel lbCurTime;
        private System.Windows.Forms.ContextMenuStrip cntMenuStripCommInfo;
        private System.Windows.Forms.ToolStripMenuItem tsmiAutoScroll;
        private System.Windows.Forms.ToolStripMenuItem tsmiAutoClear;
        private System.Windows.Forms.ToolStripMenuItem tsmiClearAll;
        private System.Windows.Forms.ToolStripMenuItem tsmiSaveRecord;
        private System.Windows.Forms.ContextMenuStrip cmsMenu;
        private System.Windows.Forms.ToolStripMenuItem tsmiAddToForwardList;
        private System.Windows.Forms.ToolStripMenuItem tsmiClearDocument;
        private System.Windows.Forms.TabControl tbPageDocument;
        private System.Windows.Forms.DataGridView dgvDocDS;
        private System.Windows.Forms.TabPage tbPageRec;
        private System.Windows.Forms.RichTextBox rtbCommMsg;
        private System.Windows.Forms.TabPage tbPageDocDS;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Button btReadAllLockData;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Button btReadLockData;
        private System.Windows.Forms.Button btSaveDataToXml;
        private System.Windows.Forms.TextBox tbCurAddr;
        private System.Windows.Forms.GroupBox gpbNodeEdit;
        private System.Windows.Forms.CheckBox cbSyncWithConc;
        private System.Windows.Forms.Button btReadDevNodeCount;
        private System.Windows.Forms.Button btCreatNewNode;
        private System.Windows.Forms.Button btDelNode;
        private System.Windows.Forms.Button btModifyNode;
        private System.Windows.Forms.ComboBox cmbNodeType;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.TextBox tbNewNodeAddr;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.TextBox tbCurNodeAddr;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.Button btFormatDoc;
        private System.Windows.Forms.Button btFormatData;
        private System.Windows.Forms.Button btWriteDocToDev;
        private System.Windows.Forms.Button btReadDocFromDev;
        private System.Windows.Forms.Button btReadDoc;
        private System.Windows.Forms.Button btWriteDoc;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btWriteConvParam;
        private System.Windows.Forms.Button btReadConvParam;
        private System.Windows.Forms.RadioButton ManualAddNode;
        private System.Windows.Forms.RadioButton AutoSaveNode;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button btWriteHostChannel;
        private System.Windows.Forms.Button tbReadHostChannel;
        private System.Windows.Forms.NumericUpDown HostChannel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbHostFreq;
    }
}