using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace Parking.Common
{
    public  class ExcelImport
    {

        private ExcelObject excelObj;
        //private DataTable Griddt = null;

        public ExcelObject Excel
        {
            get
            {
                if (excelObj == null)
                {
                    excelObj = new ExcelObject(FilePathValue);
                }
                return excelObj;
            }
        }

        private string FilePathValue
        {
            get;
            set;
        }

        private string SheetName
        {
            get;
            set;
        }
        public void Import(string filepath, System.Windows.Forms.ListView lv,out string msg)
        {
            msg = "";
          
            FilePathValue = filepath;
            DataTable dt = null;
            try
            {
                dt = this.Excel.GetSchema();

                List<object> Objectlist = null;
                Objectlist = (from dr in dt.AsEnumerable()
                              where dr["TABLE_NAME"].ToString().EndsWith("$")
                              select dr["TABLE_NAME"]).ToList();
                Hashtable hashtb = new Hashtable();
                hashtb.Clear();
                List<string> datas = new List<string>();

                SheetName = "T_RecallUser";//string.Format("{0:MMddHHmmss}", DateTime.Now);//创建导入失败表名称 

                foreach (object obj in Objectlist) { if (SheetName + "$" == obj.ToString()) SheetName = SheetName + "1"; }
                StringBuilder sb = new StringBuilder();

                //DataTable objUserdt = CreateTb();
                //objUserdt.Clear();
                foreach (object obj in Objectlist)
                {
                    DataTable objdt = this.Excel.ReadTable(obj.ToString());
                    for (int i = 1; i < objdt.Rows.Count; i++)
                    {
                        if (Convert.ToInt32(objdt.Rows[i][0]) >= 300000000)
                        {
                            throw new Exception("异常数据发生在用户ID为" + objdt.Rows[i][0] + "上面，请检查数据");
                        }
                        var wid = objdt.Rows[i][0].ToString();
                        if (!lv.Items.ContainsKey(wid))
                        {
                            while (wid.Length < 10)//不足10位前面补0
                                wid = "0" + wid;
                            lv.Items.Add(wid, wid, "").SubItems.Add("未处理");
                        }

                    }
                    break;
                }


                //if (objUserdt.Rows.Count > 0)
                //{
                   
                //    //using (DbAction db = new DbAction(str150sync))
                //    //{
                //    //    db.AddDataTable(objUserdt);
                //    //}
                //}
                Excel.Dispose();
                msg = "导入成功！";
            }
            catch (Exception ex)
            {
                Excel.Dispose();
                msg = ex.Message;
            }
        }

        private DataTable CreateTb()
        {
            DataTable tb = new DataTable();
            tb.Columns.Add("wid", typeof(string));
            
            tb.TableName = SheetName;

            return tb;
        }
    }
    public class ExcelObject : IDisposable
    {
        private string excelObject = "Provider=Microsoft.{0}.OLEDB.{1};Data Source={2};Extended Properties=\"Excel {3};HDR=YES\"";
        private string filepath = string.Empty;
        private OleDbConnection con = null;

        public delegate void ProgressWork(float percentage);
        private event ProgressWork Reading;
        private event ProgressWork Writeing;
        private event EventHandler connectionStringChange;

        public event ProgressWork ReadProgress
        {
            add
            {
                Reading += value;
            }
            remove
            {
                Reading -= value;
            }
        }

        public virtual void onReadProgress(float percentage)
        {
            if (Reading != null)
                Reading(percentage);
        }


        public event ProgressWork WriteProgress
        {
            add
            {
                Writeing += value;
            }
            remove
            {
                Writeing -= value;
            }
        }

        public virtual void onWriteProgress(float percentage)
        {
            if (Writeing != null)
                Writeing(percentage);
        }


        public event EventHandler ConnectionStringChanged
        {
            add
            {
                connectionStringChange += value;
            }
            remove
            {

                connectionStringChange -= value;
            }
        }

        public virtual void onConnectionStringChanged()
        {
            if (this.Connection != null && !this.Connection.ConnectionString.Equals(this.ConnectionString))
            {
                if (this.Connection.State == ConnectionState.Open)
                    this.Connection.Close();
                this.Connection.Dispose();
                this.con = null;

            }
            if (connectionStringChange != null)
            {
                connectionStringChange(this, new EventArgs());
            }
        }

        /// <summary>
        /// 获取EXCLE版本
        /// </summary>
        public string ConnectionString
        {
            get
            {
                if (!(this.filepath == string.Empty))
                {
                    //Check for File Format
                    FileInfo fi = new FileInfo(this.filepath);
                    if (fi.Extension.Equals(".xls"))
                    {
                        return string.Format(this.excelObject, "Jet", "4.0", this.filepath, "8.0");
                    }
                    else if (fi.Extension.Equals(".xlsx"))
                    {
                        return string.Format(this.excelObject, "Ace", "12.0", this.filepath, "12.0");
                    }
                }
                else
                {
                    return string.Empty;
                }
                return string.Empty;
            }
        }

        /// <summary>
        /// 获取连接
        /// </summary>
        public OleDbConnection Connection
        {
            get
            {
                if (con == null)
                {
                    OleDbConnection _con = new OleDbConnection { ConnectionString = this.ConnectionString };
                    this.con = _con;
                }
                return this.con;
            }
        }

        public ExcelObject(string path)
        {

            this.filepath = path;
            this.onConnectionStringChanged();
        }


        /// <summary>
        /// 获取所有xls列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetSchema()
        {
            DataTable dtSchema = null;
            if (this.Connection.State != ConnectionState.Open) this.Connection.Open();
            dtSchema = this.Connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
            return dtSchema;
        }


        public DataTable ReadTable(string tableName)
        {
            return this.ReadTable(tableName, "");
        }

        public DataTable ReadTable(string tableName, string criteria)
        {

            try
            {
                //DataTable resultTable = null;
                if (this.Connection.State != ConnectionState.Open)
                {
                    this.Connection.Open();
                    onReadProgress(10);

                }
                string cmdText = "Select * from [{0}]";
                if (!string.IsNullOrEmpty(criteria))
                {
                    cmdText += " Where " + criteria;
                }
                OleDbCommand cmd = new OleDbCommand(string.Format(cmdText, tableName));
                cmd.Connection = this.Connection;
                OleDbDataAdapter adpt = new OleDbDataAdapter(cmd);
                onReadProgress(30);

                DataSet ds = new DataSet();
                onReadProgress(50);

                adpt.Fill(ds, tableName);
                onReadProgress(100);

                if (ds.Tables.Count == 1)
                {
                    return ds.Tables[0];
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
            finally
            {
                this.Connection.Close();
            }
        }

        public bool DropTable(string tablename)
        {

            try
            {
                if (this.Connection.State != ConnectionState.Open)
                {
                    this.Connection.Open();
                    onWriteProgress(10);

                }
                string cmdText = "Drop Table [{0}]";
                using (OleDbCommand cmd = new OleDbCommand(string.Format(cmdText, tablename), this.Connection))
                {
                    onWriteProgress(30);

                    cmd.ExecuteNonQuery();
                    onWriteProgress(80);

                }
                this.Connection.Close();
                onWriteProgress(100);

                return true;
            }
            catch //(Exception ex)
            {
                onWriteProgress(0);

                //MessageBox.Show(ex.Message);
                return false;
            }
        }

        public bool WriteTable(string tableName, Dictionary<string, string> tableDefination)
        {
            try
            {
                using (OleDbCommand cmd = new OleDbCommand(this.GenerateCreateTable(tableName, tableDefination), this.Connection))
                {
                    if (ReadTable(tableName) != null) DropTable(tableName);
                    if (this.Connection.State != ConnectionState.Open) this.Connection.Open();
                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public bool AddNewRow(DataRow dr)
        {

            using (OleDbCommand cmd = new OleDbCommand(this.GenerateInsertStatement(dr), this.Connection))
            {


                cmd.ExecuteNonQuery();
            }
            return true;
        }

        /// <summary>
        /// Generates Create Table Script
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="tableDefination"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        private string GenerateCreateTable(string tableName, Dictionary<string, string> tableDefination)
        {

            StringBuilder sb = new StringBuilder();
            bool firstcol = true;
            sb.AppendFormat("CREATE TABLE [{0}](", tableName);
            firstcol = true;
            foreach (KeyValuePair<string, string> keyvalue in tableDefination)
            {
                if (!firstcol)
                {
                    sb.Append(",");
                }
                firstcol = false;
                sb.AppendFormat("{0} {1}", keyvalue.Key, keyvalue.Value);
            }

            sb.Append(")");
            return sb.ToString();
        }

        private string GenerateInsertStatement(DataRow dr)
        {
            StringBuilder sb = new StringBuilder();
            bool firstcol = true;
            sb.AppendFormat("INSERT INTO [{0}](", dr.Table.TableName);


            foreach (DataColumn dc in dr.Table.Columns)
            {
                if (!firstcol)
                {
                    sb.Append(",");
                }
                firstcol = false;

                sb.Append(dc.Caption);
            }

            sb.Append(") VALUES(");
            firstcol = true;
            for (int i = 0; i <= dr.Table.Columns.Count - 1; i++)
            {
                if (!object.ReferenceEquals(dr.Table.Columns[i].DataType, typeof(int)))
                {
                    sb.Append("'");
                    sb.Append(dr[i].ToString().Replace("'", "''"));
                    sb.Append("'");
                }
                else
                {
                    sb.Append(dr[i].ToString().Replace("'", "''"));
                }
                if (i != dr.Table.Columns.Count - 1)
                {
                    sb.Append(",");
                }
            }

            sb.Append(")");
            return sb.ToString();
        }

        #region IDisposable Members

        public void Dispose()
        {
            if (this.con != null && this.con.State == ConnectionState.Open)
                this.con.Close();
            if (this.con != null)
                this.con.Dispose();
            this.con = null;
            this.filepath = string.Empty;
        }

        #endregion
    }


}
