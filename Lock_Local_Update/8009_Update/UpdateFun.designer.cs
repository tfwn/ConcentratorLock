namespace _8009_Update
{
    partial class UpdateFun
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.gpSelectFile = new System.Windows.Forms.GroupBox();
            this.btRdVersionInfo = new System.Windows.Forms.Button();
            this.tbCrcVal = new System.Windows.Forms.TextBox();
            this.lbCrcVal = new System.Windows.Forms.Label();
            this.btOpenUpdateFile = new System.Windows.Forms.Button();
            this.tbUpdateFileInfo = new System.Windows.Forms.TextBox();
            this.lbUpdateInfo = new System.Windows.Forms.Label();
            this.tbUpdateFile = new System.Windows.Forms.TextBox();
            this.lbUpdateFilename = new System.Windows.Forms.Label();
            this.gpSelectMode = new System.Windows.Forms.GroupBox();
            this.btStartUpdate = new System.Windows.Forms.Button();
            this.rbtAppUpdate = new System.Windows.Forms.RadioButton();
            this.rbtBootUpdate = new System.Windows.Forms.RadioButton();
            this.prgBarUpdate = new System.Windows.Forms.ProgressBar();
            this.openUpdateFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.timerUpdate = new System.Windows.Forms.Timer(this.components);
            this.lbFront = new System.Windows.Forms.Label();
            this.lbMiddle = new System.Windows.Forms.Label();
            this.lbRear = new System.Windows.Forms.Label();
            this.gpSelectType = new System.Windows.Forms.GroupBox();
            this.rbt6009 = new System.Windows.Forms.RadioButton();
            this.rbt2e28 = new System.Windows.Forms.RadioButton();
            this.gpSelectFile.SuspendLayout();
            this.gpSelectMode.SuspendLayout();
            this.gpSelectType.SuspendLayout();
            this.SuspendLayout();
            // 
            // gpSelectFile
            // 
            this.gpSelectFile.Controls.Add(this.btRdVersionInfo);
            this.gpSelectFile.Controls.Add(this.tbCrcVal);
            this.gpSelectFile.Controls.Add(this.lbCrcVal);
            this.gpSelectFile.Controls.Add(this.btOpenUpdateFile);
            this.gpSelectFile.Controls.Add(this.tbUpdateFileInfo);
            this.gpSelectFile.Controls.Add(this.lbUpdateInfo);
            this.gpSelectFile.Controls.Add(this.tbUpdateFile);
            this.gpSelectFile.Controls.Add(this.lbUpdateFilename);
            this.gpSelectFile.Location = new System.Drawing.Point(21, 80);
            this.gpSelectFile.Name = "gpSelectFile";
            this.gpSelectFile.Size = new System.Drawing.Size(567, 108);
            this.gpSelectFile.TabIndex = 0;
            this.gpSelectFile.TabStop = false;
            this.gpSelectFile.Text = "选择升级文件";
            // 
            // btRdVersionInfo
            // 
            this.btRdVersionInfo.Location = new System.Drawing.Point(461, 67);
            this.btRdVersionInfo.Name = "btRdVersionInfo";
            this.btRdVersionInfo.Size = new System.Drawing.Size(83, 25);
            this.btRdVersionInfo.TabIndex = 7;
            this.btRdVersionInfo.Text = "读版本";
            this.btRdVersionInfo.UseVisualStyleBackColor = true;
            this.btRdVersionInfo.Click += new System.EventHandler(this.btRdVersionInfo_Click);
            // 
            // tbCrcVal
            // 
            this.tbCrcVal.BackColor = System.Drawing.SystemColors.Window;
            this.tbCrcVal.Location = new System.Drawing.Point(393, 68);
            this.tbCrcVal.Name = "tbCrcVal";
            this.tbCrcVal.ReadOnly = true;
            this.tbCrcVal.Size = new System.Drawing.Size(52, 20);
            this.tbCrcVal.TabIndex = 6;
            this.tbCrcVal.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lbCrcVal
            // 
            this.lbCrcVal.AutoSize = true;
            this.lbCrcVal.Location = new System.Drawing.Point(328, 73);
            this.lbCrcVal.Name = "lbCrcVal";
            this.lbCrcVal.Size = new System.Drawing.Size(65, 13);
            this.lbCrcVal.TabIndex = 5;
            this.lbCrcVal.Text = "CRC校验值";
            // 
            // btOpenUpdateFile
            // 
            this.btOpenUpdateFile.Location = new System.Drawing.Point(461, 26);
            this.btOpenUpdateFile.Name = "btOpenUpdateFile";
            this.btOpenUpdateFile.Size = new System.Drawing.Size(83, 25);
            this.btOpenUpdateFile.TabIndex = 4;
            this.btOpenUpdateFile.Text = "打开";
            this.btOpenUpdateFile.UseVisualStyleBackColor = true;
            this.btOpenUpdateFile.Click += new System.EventHandler(this.btOpenUpdateFile_Click);
            // 
            // tbUpdateFileInfo
            // 
            this.tbUpdateFileInfo.BackColor = System.Drawing.SystemColors.Window;
            this.tbUpdateFileInfo.ForeColor = System.Drawing.SystemColors.WindowText;
            this.tbUpdateFileInfo.Location = new System.Drawing.Point(95, 68);
            this.tbUpdateFileInfo.Name = "tbUpdateFileInfo";
            this.tbUpdateFileInfo.ReadOnly = true;
            this.tbUpdateFileInfo.Size = new System.Drawing.Size(215, 20);
            this.tbUpdateFileInfo.TabIndex = 3;
            this.tbUpdateFileInfo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lbUpdateInfo
            // 
            this.lbUpdateInfo.AutoSize = true;
            this.lbUpdateInfo.Location = new System.Drawing.Point(25, 73);
            this.lbUpdateInfo.Name = "lbUpdateInfo";
            this.lbUpdateInfo.Size = new System.Drawing.Size(55, 13);
            this.lbUpdateInfo.TabIndex = 2;
            this.lbUpdateInfo.Text = "文件信息";
            // 
            // tbUpdateFile
            // 
            this.tbUpdateFile.BackColor = System.Drawing.SystemColors.Window;
            this.tbUpdateFile.Location = new System.Drawing.Point(95, 27);
            this.tbUpdateFile.Name = "tbUpdateFile";
            this.tbUpdateFile.ReadOnly = true;
            this.tbUpdateFile.Size = new System.Drawing.Size(350, 20);
            this.tbUpdateFile.TabIndex = 1;
            // 
            // lbUpdateFilename
            // 
            this.lbUpdateFilename.AutoSize = true;
            this.lbUpdateFilename.Location = new System.Drawing.Point(25, 31);
            this.lbUpdateFilename.Name = "lbUpdateFilename";
            this.lbUpdateFilename.Size = new System.Drawing.Size(55, 13);
            this.lbUpdateFilename.TabIndex = 0;
            this.lbUpdateFilename.Text = "升级文件";
            // 
            // gpSelectMode
            // 
            this.gpSelectMode.Controls.Add(this.btStartUpdate);
            this.gpSelectMode.Controls.Add(this.rbtAppUpdate);
            this.gpSelectMode.Controls.Add(this.rbtBootUpdate);
            this.gpSelectMode.Location = new System.Drawing.Point(21, 194);
            this.gpSelectMode.Name = "gpSelectMode";
            this.gpSelectMode.Size = new System.Drawing.Size(567, 108);
            this.gpSelectMode.TabIndex = 1;
            this.gpSelectMode.TabStop = false;
            this.gpSelectMode.Text = "选择升级方式";
            // 
            // btStartUpdate
            // 
            this.btStartUpdate.Location = new System.Drawing.Point(461, 31);
            this.btStartUpdate.Name = "btStartUpdate";
            this.btStartUpdate.Size = new System.Drawing.Size(83, 56);
            this.btStartUpdate.TabIndex = 2;
            this.btStartUpdate.Text = "开始升级";
            this.btStartUpdate.UseVisualStyleBackColor = true;
            this.btStartUpdate.Click += new System.EventHandler(this.btStartUpdate_Click);
            // 
            // rbtAppUpdate
            // 
            this.rbtAppUpdate.AutoSize = true;
            this.rbtAppUpdate.Checked = true;
            this.rbtAppUpdate.Location = new System.Drawing.Point(27, 68);
            this.rbtAppUpdate.Name = "rbtAppUpdate";
            this.rbtAppUpdate.Size = new System.Drawing.Size(80, 17);
            this.rbtAppUpdate.TabIndex = 1;
            this.rbtAppUpdate.TabStop = true;
            this.rbtAppUpdate.Text = "App下升级";
            this.rbtAppUpdate.UseVisualStyleBackColor = true;
            // 
            // rbtBootUpdate
            // 
            this.rbtBootUpdate.AutoSize = true;
            this.rbtBootUpdate.Location = new System.Drawing.Point(27, 34);
            this.rbtBootUpdate.Name = "rbtBootUpdate";
            this.rbtBootUpdate.Size = new System.Drawing.Size(83, 17);
            this.rbtBootUpdate.TabIndex = 0;
            this.rbtBootUpdate.Text = "Boot下升级";
            this.rbtBootUpdate.UseVisualStyleBackColor = true;
            // 
            // prgBarUpdate
            // 
            this.prgBarUpdate.BackColor = System.Drawing.SystemColors.Control;
            this.prgBarUpdate.ForeColor = System.Drawing.Color.ForestGreen;
            this.prgBarUpdate.Location = new System.Drawing.Point(21, 323);
            this.prgBarUpdate.Name = "prgBarUpdate";
            this.prgBarUpdate.Size = new System.Drawing.Size(567, 16);
            this.prgBarUpdate.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.prgBarUpdate.TabIndex = 2;
            this.prgBarUpdate.Value = 25;
            this.prgBarUpdate.Visible = false;
            // 
            // openUpdateFileDialog
            // 
            this.openUpdateFileDialog.FileName = "openFileDialog1";
            // 
            // timerUpdate
            // 
            this.timerUpdate.Interval = 10;
            this.timerUpdate.Tick += new System.EventHandler(this.timerUpdate_Tick);
            // 
            // lbFront
            // 
            this.lbFront.AutoSize = true;
            this.lbFront.Location = new System.Drawing.Point(46, 347);
            this.lbFront.Name = "lbFront";
            this.lbFront.Size = new System.Drawing.Size(302, 13);
            this.lbFront.TabIndex = 3;
            this.lbFront.Text = "Please press the reset button on the main board or power on in";
            this.lbFront.Visible = false;
            // 
            // lbMiddle
            // 
            this.lbMiddle.AutoSize = true;
            this.lbMiddle.Font = new System.Drawing.Font("宋体", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbMiddle.ForeColor = System.Drawing.Color.Red;
            this.lbMiddle.Location = new System.Drawing.Point(413, 325);
            this.lbMiddle.Name = "lbMiddle";
            this.lbMiddle.Size = new System.Drawing.Size(53, 35);
            this.lbMiddle.TabIndex = 3;
            this.lbMiddle.Text = "10";
            this.lbMiddle.Visible = false;
            // 
            // lbRear
            // 
            this.lbRear.AutoSize = true;
            this.lbRear.Location = new System.Drawing.Point(462, 347);
            this.lbRear.Name = "lbRear";
            this.lbRear.Size = new System.Drawing.Size(50, 13);
            this.lbRear.TabIndex = 3;
            this.lbRear.Text = "seconds!";
            this.lbRear.Visible = false;
            // 
            // gpSelectType
            // 
            this.gpSelectType.Controls.Add(this.rbt2e28);
            this.gpSelectType.Controls.Add(this.rbt6009);
            this.gpSelectType.Location = new System.Drawing.Point(21, 11);
            this.gpSelectType.Name = "gpSelectType";
            this.gpSelectType.Size = new System.Drawing.Size(567, 64);
            this.gpSelectType.TabIndex = 4;
            this.gpSelectType.TabStop = false;
            this.gpSelectType.Text = "选择集中器类型";
            // 
            // rbt6009
            // 
            this.rbt6009.AutoSize = true;
            this.rbt6009.Location = new System.Drawing.Point(15, 29);
            this.rbt6009.Name = "rbt6009";
            this.rbt6009.Size = new System.Drawing.Size(49, 17);
            this.rbt6009.TabIndex = 2;
            this.rbt6009.TabStop = true;
            this.rbt6009.Text = "地锁";
            this.rbt6009.UseVisualStyleBackColor = true;
            this.rbt6009.Click += new System.EventHandler(this.rbt_Click);
            // 
            // rbt2e28
            // 
            this.rbt2e28.AutoSize = true;
            this.rbt2e28.Location = new System.Drawing.Point(95, 29);
            this.rbt2e28.Name = "rbt2e28";
            this.rbt2e28.Size = new System.Drawing.Size(73, 17);
            this.rbt2e28.TabIndex = 3;
            this.rbt2e28.TabStop = true;
            this.rbt2e28.Text = "无线模块";
            this.rbt2e28.UseVisualStyleBackColor = true;
            this.rbt2e28.Click += new System.EventHandler(this.rbt_Click);
            // 
            // UpdateFun
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gpSelectType);
            this.Controls.Add(this.lbRear);
            this.Controls.Add(this.lbFront);
            this.Controls.Add(this.gpSelectMode);
            this.Controls.Add(this.gpSelectFile);
            this.Controls.Add(this.lbMiddle);
            this.Controls.Add(this.prgBarUpdate);
            this.Name = "UpdateFun";
            this.Size = new System.Drawing.Size(609, 376);
            this.gpSelectFile.ResumeLayout(false);
            this.gpSelectFile.PerformLayout();
            this.gpSelectMode.ResumeLayout(false);
            this.gpSelectMode.PerformLayout();
            this.gpSelectType.ResumeLayout(false);
            this.gpSelectType.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox gpSelectFile;
        private System.Windows.Forms.GroupBox gpSelectMode;
        private System.Windows.Forms.TextBox tbCrcVal;
        private System.Windows.Forms.Label lbCrcVal;
        private System.Windows.Forms.Button btOpenUpdateFile;
        private System.Windows.Forms.TextBox tbUpdateFileInfo;
        private System.Windows.Forms.Label lbUpdateInfo;
        private System.Windows.Forms.TextBox tbUpdateFile;
        private System.Windows.Forms.Label lbUpdateFilename;
        private System.Windows.Forms.RadioButton rbtAppUpdate;
        private System.Windows.Forms.RadioButton rbtBootUpdate;
        private System.Windows.Forms.Button btStartUpdate;
        private System.Windows.Forms.ProgressBar prgBarUpdate;
        private System.Windows.Forms.OpenFileDialog openUpdateFileDialog;
        private System.Windows.Forms.Timer timerUpdate;
        private System.Windows.Forms.Label lbFront;
        private System.Windows.Forms.Label lbMiddle;
        private System.Windows.Forms.Label lbRear;
        private System.Windows.Forms.Button btRdVersionInfo;
        private System.Windows.Forms.GroupBox gpSelectType;
        private System.Windows.Forms.RadioButton rbt6009;
        private System.Windows.Forms.RadioButton rbt2e28;
    }
}
