namespace SR6009_Concentrator_Tools.FunList
{
    partial class LockControl
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
            this.cmbLockCtrl = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // cmbLockCtrl
            // 
            this.cmbLockCtrl.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLockCtrl.FormattingEnabled = true;
            this.cmbLockCtrl.Items.AddRange(new object[] {
            "开锁",
            "关锁"});
            this.cmbLockCtrl.Location = new System.Drawing.Point(11, 12);
            this.cmbLockCtrl.Name = "cmbLockCtrl";
            this.cmbLockCtrl.Size = new System.Drawing.Size(86, 21);
            this.cmbLockCtrl.TabIndex = 0;
            // 
            // LockControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cmbLockCtrl);
            this.Name = "LockControl";
            this.Size = new System.Drawing.Size(300, 433);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbLockCtrl;
    }
}
