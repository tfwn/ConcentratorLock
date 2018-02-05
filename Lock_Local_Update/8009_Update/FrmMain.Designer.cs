namespace _8009_Update
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            this.timerMonitor = new System.Windows.Forms.Timer(this.components);
            this.serialPort = new System.IO.Ports.SerialPort(this.components);
            this.statusMainStrip = new System.Windows.Forms.StatusStrip();
            this.tsUsbStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsComStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.lbSelectPort = new System.Windows.Forms.Label();
            this.cmbPort = new System.Windows.Forms.ComboBox();
            this.btOpenPort = new System.Windows.Forms.Button();
            this.panel = new System.Windows.Forms.Panel();
            this.usbHidPort = new UsbLibrary.UsbHidPort(this.components);
            this.statusMainStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // timerMonitor
            // 
            this.timerMonitor.Enabled = true;
            this.timerMonitor.Interval = 25;
            this.timerMonitor.Tick += new System.EventHandler(this.timerMonitor_Tick);
            // 
            // serialPort
            // 
            this.serialPort.BaudRate = 115200;
            this.serialPort.Parity = System.IO.Ports.Parity.Even;
            this.serialPort.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.serialPort_DataReceived);
            // 
            // statusMainStrip
            // 
            this.statusMainStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsUsbStatus,
            this.toolStripStatusLabel1,
            this.tsComStatus});
            this.statusMainStrip.Location = new System.Drawing.Point(0, 380);
            this.statusMainStrip.Name = "statusMainStrip";
            this.statusMainStrip.Size = new System.Drawing.Size(610, 22);
            this.statusMainStrip.TabIndex = 0;
            this.statusMainStrip.Text = "statusStrip1";
            // 
            // tsUsbStatus
            // 
            this.tsUsbStatus.Name = "tsUsbStatus";
            this.tsUsbStatus.Size = new System.Drawing.Size(131, 17);
            this.tsUsbStatus.Text = "toolStripStatusLabel1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(11, 17);
            this.toolStripStatusLabel1.Text = "|";
            // 
            // tsComStatus
            // 
            this.tsComStatus.Name = "tsComStatus";
            this.tsComStatus.Size = new System.Drawing.Size(131, 17);
            this.tsComStatus.Text = "toolStripStatusLabel2";
            // 
            // lbSelectPort
            // 
            this.lbSelectPort.AutoSize = true;
            this.lbSelectPort.Location = new System.Drawing.Point(21, 19);
            this.lbSelectPort.Name = "lbSelectPort";
            this.lbSelectPort.Size = new System.Drawing.Size(65, 12);
            this.lbSelectPort.TabIndex = 1;
            this.lbSelectPort.Text = "选择端口：";
            // 
            // cmbPort
            // 
            this.cmbPort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPort.FormattingEnabled = true;
            this.cmbPort.Location = new System.Drawing.Point(92, 15);
            this.cmbPort.Name = "cmbPort";
            this.cmbPort.Size = new System.Drawing.Size(121, 20);
            this.cmbPort.TabIndex = 2;
            this.cmbPort.Click += new System.EventHandler(this.cmbPort_Click);
            // 
            // btOpenPort
            // 
            this.btOpenPort.Location = new System.Drawing.Point(230, 14);
            this.btOpenPort.Name = "btOpenPort";
            this.btOpenPort.Size = new System.Drawing.Size(119, 23);
            this.btOpenPort.TabIndex = 3;
            this.btOpenPort.Text = "打开端口";
            this.btOpenPort.UseVisualStyleBackColor = true;
            this.btOpenPort.Click += new System.EventHandler(this.btOpenPort_Click);
            // 
            // panel
            // 
            this.panel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel.Location = new System.Drawing.Point(0, 43);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(610, 337);
            this.panel.TabIndex = 4;
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
            this.ClientSize = new System.Drawing.Size(610, 402);
            this.Controls.Add(this.panel);
            this.Controls.Add(this.btOpenPort);
            this.Controls.Add(this.cmbPort);
            this.Controls.Add(this.lbSelectPort);
            this.Controls.Add(this.statusMainStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmMain";
            this.statusMainStrip.ResumeLayout(false);
            this.statusMainStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        public System.Windows.Forms.ToolStripProgressBar tspbFrmMain;
        private System.Windows.Forms.Timer timerMonitor;
        private System.IO.Ports.SerialPort serialPort;
        private System.Windows.Forms.StatusStrip statusMainStrip;
        private UsbLibrary.UsbHidPort usbHidPort;
        private System.Windows.Forms.Label lbSelectPort;
        private System.Windows.Forms.ComboBox cmbPort;
        private System.Windows.Forms.Button btOpenPort;
        private System.Windows.Forms.ToolStripStatusLabel tsUsbStatus;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel tsComStatus;
        private System.Windows.Forms.Panel panel;
    }
}