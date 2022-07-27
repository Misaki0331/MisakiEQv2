using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Diagnostics;

namespace MisakiEQ
{
    class Log
    {
        const bool IS_LOGFILE = true;
        LogLevel LOG_LEVEL = 0;
        const string LOGDIR_PATH = "./logs/";
        const string LOGFILE_NAME = "console";
        const long LOGFILE_MAXSIZE = 1024*256;
        const int LOGFILE_PERIOD = 90;
        const int STACKLEN = 30;

        /// <summary>
        /// 出力ログの変更
        /// </summary>
        /// <param name="level">ログの種類</param>
        public void SetLogLevel(LogLevel level)
        {
            LOG_LEVEL = level;
            Info($"ログの出力レベルを{LOG_LEVEL}に変更しました。");
        }

        /// <summary>
        /// 現在の出力ログを取得
        /// </summary>
        public LogLevel GetLogLevel()
        {
            return LOG_LEVEL;
        }



        /// <summary>
        /// ログレベル
        /// </summary>
        public enum LogLevel
        {
            DEBUG,
            INFO,
            WARN,
            ERROR,
            FATAL
        }

        private static Log? singleton = null;
        private readonly string? logFilePath = null;
        private readonly object lockObj = new();
        private StreamWriter? stream = null;

        /// <summary>
        /// インスタンスを生成する
        /// </summary>
        private static Log GetInstance()
        {
            if (singleton == null)
            {
                singleton = new Log();
            }
            return singleton;
        }
        public static Log Instance { get { return GetInstance(); } }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        private Log()
        {
            this.logFilePath = LOGDIR_PATH + LOGFILE_NAME + ".log";

            // ログファイルを生成する
            CreateLogfile(new FileInfo(logFilePath));
        }

        /// <summary>
        /// ERRORレベルのログを出力する
        /// </summary>
        /// <param name="msg">メッセージ</param>
        public void Error(string msg, [CallerMemberName] string callerMethodName = "")
        {
            
            if (LogLevel.ERROR >= LOG_LEVEL)
            {
                Out(LogLevel.ERROR, msg, callerMethodName);
            }
        }

        /// <summary>
        /// ERRORレベルのスタックトレースログを出力する
        /// </summary>
        /// <param name="ex">例外オブジェクト</param>
        public void Error(Exception ex, [CallerMemberName] string callerMethodName = "",
            [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] int callerLineNumber = -1)
        {
            if (LogLevel.ERROR >= LOG_LEVEL)
            {
                Out(LogLevel.ERROR, ex.Message + Environment.NewLine
                    + $"{callerFilePath} - {callerLineNumber}行目" + Environment.NewLine
                    + $"[{ex.Source}] {ex.GetType()} : {ex.Message}" + Environment.NewLine
                    + ex.StackTrace, callerMethodName);
            }
        }

        /// <summary>
        /// FATALレベルのスタックトレースログを出力する
        /// </summary>
        /// <param name="ex">例外オブジェクト</param>
        public void Fatal(Exception ex, [CallerMemberName] string callerMethodName = "",
            [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] int callerLineNumber = -1)
        {
            if (LogLevel.FATAL >= LOG_LEVEL)
            {
                Out(LogLevel.FATAL, ex.Message + Environment.NewLine
                    + $"{callerFilePath} - {callerLineNumber}行目" + Environment.NewLine
                    + $"[{ex.Source}] {ex.GetType()} : {ex.Message}" + Environment.NewLine
                    + ex.StackTrace, callerMethodName);
            }
        }

        /// <summary>
        /// FATALレベルのログを出力する
        /// </summary>
        /// <param name="msg">メッセージ</param>
        public void Fatal(string msg, [CallerMemberName] string callerMethodName = "")
        {
            if (LogLevel.FATAL >= LOG_LEVEL)
            {
                Out(LogLevel.FATAL, msg, callerMethodName);
            }
        }

        /// <summary>
        /// WARNレベルのログを出力する
        /// </summary>
        /// <param name="msg">メッセージ</param>
        public void Warn(string msg, [CallerMemberName] string callerMethodName = "")
        {
            if (LogLevel.WARN >= LOG_LEVEL)
            {
                Out(LogLevel.WARN, msg, callerMethodName);
            }
        }

        /// <summary>
        /// INFOレベルのログを出力する
        /// </summary>
        /// <param name="msg">メッセージ</param>
        public void Info(string msg, [CallerMemberName] string callerMethodName = "")
        {
            if (LogLevel.INFO >= LOG_LEVEL)
            {
                Out(LogLevel.INFO, msg, callerMethodName);
            }
        }

        /// <summary>
        /// DEBUGレベルのログを出力する
        /// </summary>
        /// <param name="msg">メッセージ</param>
        public void Debug(string msg, [CallerMemberName] string callerMethodName = "")
        {
            if (LogLevel.DEBUG >= LOG_LEVEL)
            {
                Out(LogLevel.DEBUG, msg, callerMethodName);
            }
        }


        /// <summary>
        /// ログを出力する
        /// </summary>
        /// <param name="level">ログレベル</param>
        /// <param name="msg">メッセージ</param>
        private void Out(LogLevel level, string msg, string calledMethodName)
        {
            string strMethodName="";
            for(int nFrame = 2; ; nFrame++)
            {
#pragma warning disable CS8602 // null 参照の可能性があるものの逆参照です。
                StackFrame? objStackFrame = new(nFrame);
                if (objStackFrame == null) break;
                if (objStackFrame.GetMethod() == null) break;
                if (strMethodName == "") strMethodName = $"{objStackFrame.GetMethod().Name}";
                if (objStackFrame.GetMethod().ReflectedType == null) break;
                strMethodName = $"{objStackFrame.GetMethod().ReflectedType.FullName}.{strMethodName}";
                if (objStackFrame.GetMethod().ReflectedType.FullName.Contains("MisakiEQ")) break;
#pragma warning restore CS8602 // null 参照の可能性があるものの逆参照です。

            }
            // 呼び出し元のメソッド名を取得する
            if (strMethodName == "") strMethodName = "Undefined";
            int tid = Environment.CurrentManagedThreadId;
            string para = strMethodName;
            if (para.Length > STACKLEN)
            {
                para= para.Substring(para.Length-(STACKLEN - 2), STACKLEN-2);
                para = ".." + para;
            }
            else
            {
                para=para.PadLeft(STACKLEN);
            }
            string trace = msg.Replace("\n", "\n"+"".PadLeft(44+STACKLEN, ' '));
            Trace.WriteLine($"[{DateTime.Now:yyyy/MM/dd HH:mm:ss.fff}][{tid,5}][{level,-5}] [{para}]: {trace}");
            if (IS_LOGFILE)
            {
                msg = msg.Replace("\n", "\n\t");
                string fullMsg = $"[{DateTime.Now:yyyy/MM/dd HH:mm:ss.fff}][{tid,5}][{level,-5}] [{strMethodName}]: {msg}";

                lock (lockObj)
                {
                    if (stream != null) stream.WriteLine(fullMsg);
                    if (logFilePath == null) return;

                    FileInfo logFile = new(logFilePath);
                    if (LOGFILE_MAXSIZE < logFile.Length)
                    {
                        // ログファイルを圧縮する
                        CompressLogFile();

                        // 古いログファイルを削除する
                        DeleteOldLogFile();
                    }
                }
            }
        }

        /// <summary>
        /// ログファイルを生成する
        /// </summary>
        /// <param name="logFile">ファイル情報</param>
        private void CreateLogfile(FileInfo logFile)
        {
            if (!Directory.Exists(logFile.DirectoryName)&&logFile.DirectoryName!=null)
            {
                Directory.CreateDirectory(logFile.DirectoryName);
            }

            this.stream = new StreamWriter(logFile.FullName, true, Encoding.UTF8)
            {
                AutoFlush = true
            };
        }

        /// <summary>
        /// ログファイルを圧縮する
        /// </summary>
        private void CompressLogFile()
        {
            if(stream!=null)stream.Close();
            string oldFilePath = LOGDIR_PATH + LOGFILE_NAME + "_" + DateTime.Now.ToString("yyyyMMddHHmmss");
            if(logFilePath!=null)File.Move(this.logFilePath, oldFilePath + ".log");

            FileStream inStream = new(oldFilePath + ".log", FileMode.Open, FileAccess.Read);
            FileStream outStream = new(oldFilePath + ".gz", FileMode.Create, FileAccess.Write);
            GZipStream gzStream = new(outStream, CompressionMode.Compress);

            int size;
            byte[] buffer = new byte[LOGFILE_MAXSIZE + 1000];
            while (0 < (size = inStream.Read(buffer, 0, buffer.Length)))
            {
                gzStream.Write(buffer, 0, size);
            }

            inStream.Close();
            gzStream.Close();
            outStream.Close();

            File.Delete(oldFilePath + ".log");
            if(logFilePath!=null)CreateLogfile(new FileInfo(logFilePath));
        }

        /// <summary>
        /// 古いログファイルを削除する
        /// </summary>
        private static void DeleteOldLogFile()
        {
            Regex regex = new(LOGFILE_NAME + @"_(\d{14}).*\.gz");
            DateTime retentionDate = DateTime.Today.AddDays(-LOGFILE_PERIOD);
            string[] filePathList = Directory.GetFiles(LOGDIR_PATH, LOGFILE_NAME + "_*.gz", SearchOption.TopDirectoryOnly);
            foreach (string filePath in filePathList)
            {
                Match match = regex.Match(filePath);
                if (match.Success)
                {
                    DateTime logCreatedDate = DateTime.ParseExact(match.Groups[1].Value.ToString(), "yyyyMMddHHmmss", null);
                    if (logCreatedDate < retentionDate)
                    {
                        File.Delete(filePath);
                    }
                }
            }
        }
    }
}