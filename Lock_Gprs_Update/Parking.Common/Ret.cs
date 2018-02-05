using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.Common
{
    public class Ret<T>
    {
        public int Code { get; set; }
        public string Msg { get; set; }
        public T Obj { get; set; }

        public static Ret<T> Create(int code, string msg, T obj)
        {
            return new Ret<T>() { Code = code, Msg = msg, Obj = obj };
        }

        public new string ToString()
        {
            return string.Format("Code:{0},Msg:{1},Obj:{2}", Code, Msg, Obj);
        }
    }
    public class Ret<T, T1>
    {
        public int Code { get; set; }
        public string Msg { get; set; }
        public T Obj { get; set; }
        public T1 Obj1 { get; set; }
        public static Ret<T> Create(int code, string msg, T obj)
        {
            return new Ret<T>() { Code = code, Msg = msg, Obj = obj };
        }
        public static Ret<T, T1> Create(int code, string msg, T obj, T1 obj1)
        {
            return new Ret<T, T1>() { Code = code, Msg = msg, Obj = obj, Obj1 = obj1 };
        }
        public new string ToString()
        {
            return string.Format("Code:{0},Msg:{1},Obj:{2}", Code, Msg, Obj);
        }
    }
}
