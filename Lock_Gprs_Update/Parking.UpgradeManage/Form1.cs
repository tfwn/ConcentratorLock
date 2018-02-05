using Parking.Common;
using Parking.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Parking.UpgradeManage
{
    public partial class Form1 : Form
    {
        string url1 = "";
        PostFunction post1 = new PostFunction();

        private string username = "111";
        private string pass = "111";

        public Form1()
        {
            InitializeComponent();
        }

        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            if (!getdtulist())
            {
                MessageBox.Show("集中器离线");
                return ;
            }
            var collectorNo = txt_Collector.Text.Trim();
         
            try
            {
                openFileDialog1.Filter = "bin文件|*.bin";
                openFileDialog1.FilterIndex = 0;
                openFileDialog1.CheckFileExists = true;
                openFileDialog1.CheckPathExists = true;
                openFileDialog1.Title = "请选择上传的代码文件";
                openFileDialog1.Multiselect = false;
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                  
                    FileInfo fi = new FileInfo(openFileDialog1.FileName);
                    long _size = fi.Length;//得到文件的字节大小
                    if (_size > 1024 * 100)
                    {
                        MessageBox.Show("文件不能大于100K");
                      
                    }
                    
                    using (var sr = fi.OpenText())
                    {
                        string restOfStream = sr.ReadToEnd();
                        if(restOfStream.Contains("6009*Boot"))
                        {
                            MessageBox.Show("文件包含有boot在里面，不能升级");
                         
                            return;
                        }
                        var m= Regex.Match(restOfStream, @"SRWF-\d{4}-[a-zA-Z]{1,4}-\d{8}-Vsp\d{1}.\d{2}");
                        if ( m.Success)
                        {
                            lblVersionInfo.Text = m.Value;
                        }else
                        {
                            MessageBox.Show("文件不能读取版本信息");
                           
                            return;
                        }
                    }
                    lblFileName.Text = fi.Name;
                    byte[] buffer = new byte[fi.Length];
                    using (var fs = fi.OpenRead())
                    {
                        fs.Read(buffer,0, (int)fi.Length);
                    }

                    Logger.Instance.Info("开始上传。。。");
                    //lbxMemo.Items.Add();
                    
                    long max = fi.Length % 200 == 0 ? fi.Length / 200 : fi.Length / 200 + 1;
                    pb_upgrade.Maximum = (int)max;

                    //开始上传文件
                    var crc16 = CRC.CRC16(buffer, 0, (int)fi.Length);
                    
                    UInt32 currentPackageOffset = 0;
                    UInt16 currentPackageLen = 0;
                    UInt16 totalLen = (UInt16)fi.Length;
                    int upgradeCount = 0;
                   
                    Task.Factory.StartNew(()=> {
                        
                        this.Invoke((EventHandler)(delegate
                        {
                            btnOpenFile.Enabled = false;
                        }));
                        while (totalLen > 0)
                        {
                            if (totalLen <= 200)
                            {
                                currentPackageLen = totalLen;
                                totalLen = 0;
                            }
                            else
                            {
                                currentPackageLen = 200;
                                totalLen -= 200;
                            }

                            var reqInfo = new CollectorUpgrade
                            {
                                FILE_CRC16 = crc16,
                                FILE_OFFSET = currentPackageOffset,
                                FILE_LEN = (uint)fi.Length,
                                CODE_LEN = currentPackageLen,
                                UserName = "111",
                                Pass = "111",
                                CollectorNo = collectorNo,
                            };
                            reqInfo.CODE_CONTENT = new Byte[currentPackageLen];
                            Array.Copy(buffer, currentPackageOffset, reqInfo.CODE_CONTENT, 0, currentPackageLen);

                            CollectorUpgrade resInfo = null;
                            for (int i = 0; i < 3; i++)
                            {
                                resInfo = upgradeFileCode(reqInfo);
                                if (resInfo.RESULT == 0xAA)
                                    break;
                            }
                            if(resInfo.RESULT == 0xAA)
                            {
                                currentPackageOffset += currentPackageLen;
                                this.Invoke((EventHandler)(delegate
                                {
                                    pb_upgrade.Value = ++upgradeCount;
                                    lbl_packageCount.Text = upgradeCount+"/"+ max;
                                }));
                                    
                            }
                            else
                            {
                                MessageBox.Show("上传失败");
                                this.Invoke((EventHandler)(delegate
                                {
                                    pb_upgrade.Value = 0;
                                    btnOpenFile.Enabled = true;
                                    Logger.Instance.Error("上传失败");
                                    
                                }));
                              
                                return;
                            }
                        }
                        MessageBox.Show("上传成功");
                        this.Invoke((EventHandler)(delegate
                        {
                            btnOpenFile.Enabled = true;
                            Logger.Instance.Info("上传成功");
                        }));
                    });
                    
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("上传代码文件出错: " + ex.Message);
                
                btnOpenFile.Enabled = true;
                Logger.Instance.Error(ex.Message);
               
                pb_upgrade.Value = 0;
            }
           
        }

        private CollectorUpgrade upgradeFileCode(CollectorUpgrade reqInfo)
        {
            var resInfo = new CollectorUpgrade { RESULT=0xAB };
          
            string method = "upgradecollector";
            try
            {
                string rel = "";
                string param = "";
                param = post1.SerializeStringByObject(reqInfo);
                rel = post1.PostInterface(url1 + method, param);

                var ret = post1.DeserializeFromString<Ret<CollectorUpgrade>>(rel);
                if (ret.Code == 0 && ret.Obj != null)
                {
                    return ret.Obj;
                }
                else
                {
                    this.Invoke((EventHandler)(delegate
                    {
                        //lbxMemo.Items.Add(ret.Msg);
                        Logger.Instance.Error(ret.Msg);
                    }));
                    return resInfo;
                }
            }
            catch (Exception ex)
            {
                this.Invoke((EventHandler)(delegate
                {
                    //lbxMemo.Items.Add(ex.Message);
                    Logger.Instance.Error(ex.Message);
                }));
                return resInfo;
            }
            
        }

        private void btn_upgradeFile_Click(object sender, EventArgs e)
        {

        }

        private void btnGetDtu_Click(object sender, EventArgs e)
        {
            getdtulist();
        }

        private bool getdtulist()
        {
            lbl_isOnline.Text = "";
            while(txt_Collector.Text.Length<16)
            {
                txt_Collector.Text = "0" + txt_Collector.Text;
            }
            var collectorNo = txt_Collector.Text.Trim();
            if (!Regex.IsMatch(collectorNo, @"\d{16}"))
            {
                MessageBox.Show("集中器格式输入有误!");
                return false;
            }
            try
            {
                var rel = post1.PostInterface(url1 + "getdtulist", post1.SerializeStringByObject(new UserInfo { UserName = "111", Pass = "111" }));
                var ret = post1.DeserializeFromString<Ret<List<string>>>(rel);
                if (ret.Code==0 && ret.Obj != null && ret.Obj.Count > 0)
                {
                    //cbxDtu.Items.AddRange(ret.Obj.ToArray());
                    //cbxDtu.SelectedItem = ret.Obj[0];
                    if (ret.Obj.Contains(txt_Collector.Text))
                    {
                        lbl_isOnline.Text = "在线";
                        return true;
                    }
                    else
                    {
                        lbl_isOnline.Text = "离线";
                        return false;
                    }
                }
                else
                {
                    //lbxMemo.Items.Add("验证集中器是否在线错误：" + ret.Msg);
                    Logger.Instance.Error("验证集中器是否在线错误：" + ret.Msg);
                    return false;
                }
            }
            catch (Exception ex)
            {

                //lbxMemo.Items.Add("验证集中器是否在线错误："+ex.Message);
                Logger.Instance.Error("验证集中器是否在线错误：" + ex.Message);
                return false;
            }
        }

        private void lbxMemo_SizeChanged(object sender, EventArgs e)
        {

        }

        public void showmsg(string msg)
        {
            if (this.IsHandleCreated)
            {
                this.Invoke((EventHandler)(delegate
                {
                    // if(listboxMsg!=null)
                    if (lbxMemo.Items.Count >= 20)
                        lbxMemo.Items.RemoveAt(0);
                    lbxMemo.Items.Add(msg);
                }));
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            url1 = ConfigurationManager.AppSettings["httpUrl"];
            Logger.Instance.setLogAction(new Action<string>(showmsg));
            this.skinEngine1.SkinFile = Application.StartupPath + "\\Skins\\mp10.ssk";

            this.skinEngine1.SkinAllForm = true;

        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Logger.Instance.Close();
        }
    }
}
