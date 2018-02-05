namespace Parking.UpgradeManage
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btnOpenFile = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.label1 = new System.Windows.Forms.Label();
            this.lblVersionInfo = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnGetDtu = new System.Windows.Forms.Button();
            this.pb_upgrade = new System.Windows.Forms.ProgressBar();
            this.label2 = new System.Windows.Forms.Label();
            this.lbl_packageCount = new System.Windows.Forms.Label();
            this.txt_Collector = new System.Windows.Forms.TextBox();
            this.lbl_isOnline = new System.Windows.Forms.Label();
            this.lbxMemo = new System.Windows.Forms.ListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.lblFileName = new System.Windows.Forms.Label();
            this.skinEngine1 = new Sunisoft.IrisSkin.SkinEngine();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOpenFile
            // 
            this.btnOpenFile.Location = new System.Drawing.Point(221, 20);
            this.btnOpenFile.Name = "btnOpenFile";
            this.btnOpenFile.Size = new System.Drawing.Size(93, 23);
            this.btnOpenFile.TabIndex = 0;
            this.btnOpenFile.Text = "上传代码文件";
            this.btnOpenFile.UseVisualStyleBackColor = true;
            this.btnOpenFile.Click += new System.EventHandler(this.btnOpenFile_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 107);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "版本信息：";
            // 
            // lblVersionInfo
            // 
            this.lblVersionInfo.AutoSize = true;
            this.lblVersionInfo.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblVersionInfo.Location = new System.Drawing.Point(95, 107);
            this.lblVersionInfo.Name = "lblVersionInfo";
            this.lblVersionInfo.Size = new System.Drawing.Size(21, 14);
            this.lblVersionInfo.TabIndex = 3;
            this.lblVersionInfo.Text = "  ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(33, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 15;
            this.label3.Text = "集中器：";
            // 
            // btnGetDtu
            // 
            this.btnGetDtu.Location = new System.Drawing.Point(221, 21);
            this.btnGetDtu.Name = "btnGetDtu";
            this.btnGetDtu.Size = new System.Drawing.Size(93, 21);
            this.btnGetDtu.TabIndex = 14;
            this.btnGetDtu.Text = "是否在线";
            this.btnGetDtu.UseVisualStyleBackColor = true;
            this.btnGetDtu.Click += new System.EventHandler(this.btnGetDtu_Click);
            // 
            // pb_upgrade
            // 
            this.pb_upgrade.Location = new System.Drawing.Point(91, 150);
            this.pb_upgrade.Name = "pb_upgrade";
            this.pb_upgrade.Size = new System.Drawing.Size(264, 23);
            this.pb_upgrade.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.pb_upgrade.TabIndex = 17;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 156);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 18;
            this.label2.Text = "上传进度：";
            // 
            // lbl_packageCount
            // 
            this.lbl_packageCount.AutoSize = true;
            this.lbl_packageCount.Location = new System.Drawing.Point(358, 157);
            this.lbl_packageCount.Name = "lbl_packageCount";
            this.lbl_packageCount.Size = new System.Drawing.Size(17, 12);
            this.lbl_packageCount.TabIndex = 19;
            this.lbl_packageCount.Text = "  ";
            // 
            // txt_Collector
            // 
            this.txt_Collector.Location = new System.Drawing.Point(95, 21);
            this.txt_Collector.MaxLength = 16;
            this.txt_Collector.Name = "txt_Collector";
            this.txt_Collector.Size = new System.Drawing.Size(113, 21);
            this.txt_Collector.TabIndex = 20;
            // 
            // lbl_isOnline
            // 
            this.lbl_isOnline.AutoSize = true;
            this.lbl_isOnline.Location = new System.Drawing.Point(321, 25);
            this.lbl_isOnline.Name = "lbl_isOnline";
            this.lbl_isOnline.Size = new System.Drawing.Size(17, 12);
            this.lbl_isOnline.TabIndex = 21;
            this.lbl_isOnline.Text = "  ";
            // 
            // lbxMemo
            // 
            this.lbxMemo.FormattingEnabled = true;
            this.lbxMemo.HorizontalScrollbar = true;
            this.lbxMemo.ItemHeight = 12;
            this.lbxMemo.Location = new System.Drawing.Point(13, 283);
            this.lbxMemo.Name = "lbxMemo";
            this.lbxMemo.Size = new System.Drawing.Size(408, 160);
            this.lbxMemo.TabIndex = 22;
            this.lbxMemo.SizeChanged += new System.EventHandler(this.lbxMemo_SizeChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.btnGetDtu);
            this.groupBox1.Controls.Add(this.lbl_isOnline);
            this.groupBox1.Controls.Add(this.txt_Collector);
            this.groupBox1.Location = new System.Drawing.Point(13, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(406, 57);
            this.groupBox1.TabIndex = 23;
            this.groupBox1.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lblFileName);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.btnOpenFile);
            this.groupBox2.Controls.Add(this.lblVersionInfo);
            this.groupBox2.Controls.Add(this.lbl_packageCount);
            this.groupBox2.Controls.Add(this.pb_upgrade);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Location = new System.Drawing.Point(13, 75);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(406, 187);
            this.groupBox2.TabIndex = 24;
            this.groupBox2.TabStop = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(34, 65);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 20;
            this.label4.Text = "文件名：";
            // 
            // lblFileName
            // 
            this.lblFileName.AutoSize = true;
            this.lblFileName.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblFileName.Location = new System.Drawing.Point(95, 65);
            this.lblFileName.Name = "lblFileName";
            this.lblFileName.Size = new System.Drawing.Size(21, 14);
            this.lblFileName.TabIndex = 21;
            this.lblFileName.Text = "  ";
            // 
            // skinEngine1
            // 
            this.skinEngine1.@__DrawButtonFocusRectangle = true;
            this.skinEngine1.DisabledButtonTextColor = System.Drawing.Color.Gray;
            this.skinEngine1.DisabledMenuFontColor = System.Drawing.SystemColors.GrayText;
            this.skinEngine1.InactiveCaptionColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.skinEngine1.SerialNumber = "";
            this.skinEngine1.SkinFile = null;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(439, 458);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lbxMemo);
            this.Name = "Form1";
            this.Text = "停车场系统gprs升级管理";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnOpenFile;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblVersionInfo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnGetDtu;
        private System.Windows.Forms.ProgressBar pb_upgrade;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lbl_packageCount;
        private System.Windows.Forms.TextBox txt_Collector;
        private System.Windows.Forms.Label lbl_isOnline;
        private System.Windows.Forms.ListBox lbxMemo;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label lblFileName;
        private System.Windows.Forms.Label label4;
        private Sunisoft.IrisSkin.SkinEngine skinEngine1;
    }
}

