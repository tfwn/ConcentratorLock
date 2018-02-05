using log4net;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Parking.Common
{
    public class Log4NetInfo
    {
        private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger("WebLogger");
        
        static Log4NetInfo()
        {

            log4net.Config.XmlConfigurator.Configure();
        
        }



        
        #region DEBUG 调试
        public static void Debug(object message)
        {
            Logger.Debug(message);
        }

        public static void Debug(object message, Exception exp)
        {
            Logger.Debug(message, exp);
        }

        public static void DebugFormat(string format, object arg0)
        {
            Logger.DebugFormat(format, arg0);
        }

        public static void DebugFormat(string format, params object[] args)
        {
            Logger.DebugFormat(format, args);
        }

        public static void DebugFormat(IFormatProvider provider, string format, params object[] args)
        {
            Logger.DebugFormat(provider, format, args);
        }

        public static void DebugFormat(string format, object arg0, object arg1)
        {
            Logger.DebugFormat(format, arg0, arg1);
        }

        public static void DebugFormat(string format, object arg0, object arg1, object arg2)
        {
            Logger.DebugFormat(format, arg0, arg1, arg2);
        }
        #endregion

        #region Info 信息
        public static void Info(object message)
        {
            Logger.Info(message);
        }

        public static void Info(object message, Exception exception)
        {
            Logger.Info(message, exception);
        }

        public static void InfoFormat(string format, object arg0)
        {
            Logger.InfoFormat(format, arg0);
        }

        public static void InfoFormat(string format, params object[] args)
        {
            Logger.InfoFormat(format, args);
        }

        public static void InfoFormat(IFormatProvider provider, string format, params object[] args)
        {
            Logger.InfoFormat(provider, format, args);
        }

        public static void InfoFormat(string format, object arg0, object arg1)
        {
            Logger.InfoFormat(format, arg0, arg1);
        }

        public static void InfoFormat(string format, object arg0, object arg1, object arg2)
        {
            Logger.InfoFormat(format, arg0, arg1, arg2);
        }
        #endregion

        #region Warn 警告,注意,通知
        /// <summary>
        /// 警告
        /// </summary>
        /// <param name="message"></param>
        public static void Warn(object message)
        {
            Logger.Warn(message);
        }

        public static void Warn(object message, Exception exception)
        {
            Logger.Warn(message, exception);
        }

        public static void WarnFormat(string format, object arg0)
        {
            Logger.WarnFormat(format, arg0);
        }

        public static void WarnFormat(string format, params object[] args)
        {
            Logger.WarnFormat(format, args);
        }

        public static void WarnFormat(IFormatProvider provider, string format, params object[] args)
        {
            Logger.WarnFormat(provider, format, args);
        }

        public static void WarnFormat(string format, object arg0, object arg1)
        {
            Logger.WarnFormat(format, arg0, arg1);
        }

        public static void WarnFormat(string format, object arg0, object arg1, object arg2)
        {
            Logger.WarnFormat(format, arg0, arg1, arg2);
        }
        #endregion

        #region Error 错误
        public static void Error(object message)
        {
            Logger.Error(message);
        }

        public static void Error(object message, Exception exception)
        {
            Logger.Error(message, exception);
        }

        public static void ErrorFormat(string format, object arg0)
        {
            Logger.ErrorFormat(format, arg0);
        }

        public static void ErrorFormat(string format, params object[] args)
        {
            Logger.ErrorFormat(format, args);
        }

        public static void ErrorFormat(IFormatProvider provider, string format, params object[] args)
        {
            Logger.ErrorFormat(provider, format, args);
        }

        public static void ErrorFormat(string format, object arg0, object arg1)
        {
            Logger.ErrorFormat(format, arg0, arg1);
        }

        public static void ErrorFormat(string format, object arg0, object arg1, object arg2)
        {
            Logger.ErrorFormat(format, arg0, arg1, arg2);
        }
        #endregion

        #region Fatal 致命的

        public static void Fatal(object message)
        {
            Logger.Fatal(message);
        }

        public static void Fatal(object message, Exception exception)
        {
            Logger.Fatal(message, exception);
        }

        public static void FatalFormat(string format, object arg0)
        {
            Logger.FatalFormat(format, arg0);
        }

        public static void FatalFormat(string format, params object[] args)
        {
            Logger.FatalFormat(format, args);
        }

        public static void FatalFormat(IFormatProvider provider, string format, params object[] args)
        {
            Logger.FatalFormat(provider, format, args);
        }

        public static void FatalFormat(string format, object arg0, object arg1)
        {
            Logger.FatalFormat(format, arg0, arg1);
        }

        public static void FatalFormat(string format, object arg0, object arg1, object arg2)
        {
            Logger.FatalFormat(format, arg0, arg1, arg2);
        }
        #endregion
    }

    public class LogInfo
    {
        public string Message { get; set; }

        public LogLevel Level { get; set; }
    }

    public enum LogLevel { FATAL, RROR, WARN, INFO, DEBUG }
}
