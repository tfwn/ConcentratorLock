using Parking.Common.Config;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Web;
using System.Windows.Forms;

namespace Parking.Common
{
    /// <summary>
    /// 写日志模块，add by fucanzhen
    /// 1、高并发写文件
    /// 2、文件超过长度后自动分块
    /// </summary>
    public  class Logger
    {

        internal  Queue<LogEntry> LoggingQueue = null;
        private StreamWriter writer;
        private FileStream fileStream = null;
        static AllConfig config = null;
        private  Thread logThread = null;
        private static Logger _instance = null;

        private Action<string> showFormMsg;

   

        public  void setLogAction(Action<string>  a)
        {
            showFormMsg = a;
        }

        static Logger()
        {
            _instance = new Logger();
            
        }
        private Logger()
        {
            LoggingQueue = new Queue<LogEntry>();
            config = GetConfig.GetAllConfig();
            if (config.LoggerConfig == null)
                throw new Exception("缺少Logger配置节");

            if (System.Environment.CurrentDirectory+"\\" == AppDomain.CurrentDomain.BaseDirectory)
            {
                string stmp = Assembly.GetExecutingAssembly().Location;

                stmp = stmp.Substring(0, stmp.LastIndexOf('\\'));//删除文件名
                filePath = stmp;
            }
            else
            {//web路径
                filePath = HttpRuntime.AppDomainAppPath;
            }
            filePath += "\\Log\\";
            CreateDirectory(filePath);
            logThread = new Thread(new ThreadStart(timerCallback));
            logThread.Start();
        }

        void timerCallback()
        {
            //创建文件
            DateTime currTime = DateTime.Now;
            string file = filePath + currTime.ToString("yyyyMMdd") + ".txt";
            //System.IO.FileInfo fileInfo = new System.IO.FileInfo(file);
            //if (!File.Exists(file))
            //{
            //    fileStream = new FileStream(file, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
            //}
            //else
            //{
            //    // FileShare.ReadWrite解决读写文件时文件正由另一进程使用，因此该进程无法访问该文件   
            //    fileStream = new FileStream(file, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
            //}
            //writer = new StreamWriter(fileStream);
            int fileMaxSize =Convert.ToInt32(config.LoggerConfig.MaxFileSize);
            int interval = Convert.ToInt32(config.LoggerConfig.ReadLogQueueInterval);
            //开始循环从队列读出日志写入文件
            while (true)
            {
                if (currTime.Date < DateTime.Now.Date || !File.Exists(file))
                {//按日期创建每天的日志文件
                    currTime = DateTime.Now;
                    file = filePath + currTime.ToString("yyyyMMdd") + ".txt";
                    if (!File.Exists(file))
                    {
                        if(writer!=null)
                        { 
                            writer.Close();
                            writer.Dispose();
                            writer = null;
                        }
                        if(fileStream!=null)
                        { 
                            fileStream.Close();
                            fileStream.Dispose();
                            writer = null;
                        }
                        //创建第二天的日志文件
                        fileStream = new FileStream(file, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                        writer = new StreamWriter(fileStream);
                    }
                }
                
                ///判断文件是否存在以及是否大于10
                var fileInfo = new FileInfo(file);
                if ( fileInfo.Length >fileMaxSize* 1024 * 1024)
                {
                    if (writer != null)
                    {
                        writer.Close();
                        writer.Dispose();
                        writer = null;
                    }
                    if (fileStream != null)
                    {
                        fileStream.Close();
                        fileStream.Dispose();
                        fileStream = null;
                    }
                    ///文件超过10MB则重命名
                    File.Move(file, filePath + currTime.ToString("yyyyMMdd_HHmmss") + ".txt");
                    
                    ////读写文件时文件正由另一进程使用，因此该进程无法访问该文件   
                    //fileStream = new FileStream(file, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
                    //writer = new StreamWriter(fileStream, System.Text.Encoding.UTF8);
                }
                if (writer==null)
                {
                    fileStream = new FileStream(file, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
                    writer = new StreamWriter(fileStream);
                }
                try
                {
                    DoGlobalWrite();
                }
                catch (Exception ex)
                {
                    string str = ex.Message;
                    Console.WriteLine(str);
                    if (writer != null)
                    {
                        writer.Close();
                        writer.Dispose();
                        writer = null;
                    }
                    if (fileStream != null)
                    {
                        fileStream.Close();
                        fileStream.Dispose();
                        fileStream = null;
                    }
                }
                Thread.Sleep(interval);
            }
        }

        private void DoGlobalWrite()
        {
            LogEntry entry = null;
            while (LoggingQueue.Count() > 0)
            {
                lock (LoggingQueue)
                { 
                    entry = LoggingQueue.Dequeue();
                }
                if (entry != null)
                {
                    string cateName = Enum.GetName(typeof(LogMessageCategory), entry.Category);
                    string msg = string.Format("{0} {1} {2}", entry.TimeStamp.ToString("yyyy-MM-dd HH:mm:ss,fff"), cateName, entry.Message);
                    writer.WriteLine(msg);
                    writer.Flush();
                    showFormMsg(msg);
                }

            }
           

        }
        private void CreateDirectory(string infoPath)
        {
            DirectoryInfo directoryInfo = Directory.GetParent(infoPath);
            if (!directoryInfo.Exists)
            {
                directoryInfo.Create();
            }
        }  
        public  void Warn(string message)
        {
            //配置段是否允许写该级别日志
            if (!IsAlowWrite(LogMessageCategory.WARN))
                return;
            
            var log=new LogEntry{ Category=LogMessageCategory.WARN, Message=message, TimeStamp=DateTime.Now};
            lock(LoggingQueue)
            { 
                LoggingQueue.Enqueue(log);
            }
        }
        public  void Error(string message)
        {
            //配置段是否允许写该级别日志
            if (!IsAlowWrite(LogMessageCategory.RROR))
                return;

            var log = new LogEntry { Category = LogMessageCategory.RROR, Message = message, TimeStamp = DateTime.Now };
            lock (LoggingQueue)
            {
                LoggingQueue.Enqueue(log);
            }
        }
        public  void Info(string message)
        {
            //配置段是否允许写该级别日志
            if (!IsAlowWrite(LogMessageCategory.INFO))
                return;

            var log = new LogEntry { Category = LogMessageCategory.INFO, Message = message, TimeStamp = DateTime.Now };
            lock (LoggingQueue)
            {
                LoggingQueue.Enqueue(log);
            }
        }
        public  void Debug(string message)
        {
            //配置段是否允许写该级别日志
            if (!IsAlowWrite(LogMessageCategory.DEBUG))
                return;

            var log = new LogEntry { Category = LogMessageCategory.DEBUG, Message = message, TimeStamp = DateTime.Now };
            lock (LoggingQueue)
            {
                LoggingQueue.Enqueue(log);
            }
        }
        /// <summary>
        /// 配置段是否允许写该级别日志
        /// </summary>
        /// <param name="cate"></param>
        /// <returns></returns>
        private bool IsAlowWrite(LogMessageCategory cate)
        {
            var level = config.LoggerConfig.LogLevel;
            LogMessageCategory cateConfig;
            cateConfig = (LogMessageCategory)Enum.Parse(typeof(LogMessageCategory), level.ToUpper());
            if (cateConfig < cate)
                return false;
            else
                return true;
        }
        public static Logger Instance
        {
            get
            {
                return _instance;
            }
        }
        private static string filePath = "";

        // The concurrent logs writing queue, logs will actually be written until DoGlobalWrite() method is called or timer checker found items in queue.
        //private readonly LogMessageCategory Category;


        public void Close()
        {
            logThread.Abort();
            logThread.Join();
            if (writer != null)
            {
                writer.Close();
                writer.Dispose();
            }
            if (fileStream != null)
            {
                fileStream.Close();
                fileStream.Dispose();
            }
            lock (LoggingQueue)
            {
                LoggingQueue.Clear();
            }

        }
    }
    public enum LogMessageCategory { OFF, RROR, WARN, INFO, DEBUG, ALL };

    
    public class LogEntry
    { 
        public  LogMessageCategory Category;
        public  string Message;
        public  DateTime TimeStamp;

    }
}
