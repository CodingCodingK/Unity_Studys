using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PENet;

namespace PEProtocol
{
    public enum LogType
    {
        Log = 0,
        Warn = 1,
        Error = 2,
        Info = 3
    }
    public class PECommon
    {
        public static void Log(string msg = "", LogType tp = LogType.Log)
        {
            LogLevel lv = (LogLevel) tp;
            PETool.LogMsg(msg, lv);
        }
    }
}
