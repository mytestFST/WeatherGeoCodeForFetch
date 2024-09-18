using Microsoft.VisualStudio.TestTools.UnitTesting;
using NLog.Config;
using NLog.Targets;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

namespace Utilities.LogHelper
{
    public enum LogState
    {
        Info,
        Error
    }
    public static class LogWriter
    {

        private static string _logFolderName, _logFileName;
        private static Logger _logger;
        private static LoggingConfiguration _config;
        private static string _logDirectory = AppDomain.CurrentDomain.BaseDirectory + @"\TestLogs";
        private static string _mainLogFilePath;
      //  public static TestContext TestContext { get; set; }



        public static void InitLogHelper(TestContext testContext)
        {



            string[] FilePathNames;
            FilePathNames = testContext.ManagedType.Split('.');

            _config = new LoggingConfiguration();

            _logFolderName = FilePathNames[0];
            _logFileName = FilePathNames[1];

            string dateStamp = DateTime.Now.ToString("yyyy_MM_dd");
            _mainLogFilePath = Path.Combine(_logDirectory, $"{_logFolderName}", $"{_logFileName}_{dateStamp}.log");

            ArchiveLogFiles(Path.Combine(_logDirectory, $"{_logFolderName}"));

            var MainLogTarget = GetLoggerTarget(_mainLogFilePath, "MainLog");
            MainLogTarget.Layout = "${date} : ${level:uppercase=true} : ${message} ${exception} ";

            _config.AddTarget(MainLogTarget);

            var rule = new LoggingRule("*", LogLevel.Debug, MainLogTarget);
            _config.LoggingRules.Add(rule);

            LogManager.Configuration = _config;
            LogManager.GetLogger("MainLog");
            _logger = LogManager.GetCurrentClassLogger();

        }

        private static void ArchiveLogFiles(string LogFilePath)
        {
            string dateTimeStamp = DateTime.Now.ToString("yyyy_MM_dd");

            List<string> sourceFiles = new List<string>();


            if (Directory.Exists(LogFilePath))
            {
                sourceFiles = Directory.GetFiles(LogFilePath).ToList();
            }


            if (sourceFiles.Count() > 0)
            {
                DirectoryInfo ArchiveDirectrory = System.IO.Directory.CreateDirectory(Path.Combine(LogFilePath, "Archive"));
                DirectoryInfo ArchiveSubDirectory = ArchiveDirectrory.CreateSubdirectory(dateTimeStamp);
                foreach (string sourceFile in sourceFiles)
                {
                    // Get the name of the log file from the log folder.
                    string fileName = Path.GetFileNameWithoutExtension(sourceFile);

                    // Get the extension of the file. 
                    string extension = Path.GetExtension(sourceFile);

                    // Create a datetimestamp of time it the file is archived
                    string timestamp = DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss");

                    // Create the new file name with the timestamp.
                    string newFileName = $"{fileName}_{timestamp}{extension}";

                    // Create the full path for the new file, append the extension and timestamp
                    string targetFile = Path.Combine(ArchiveSubDirectory.FullName, newFileName);

                    // Copy the file to the archive directory.
                    File.Copy(sourceFile, targetFile);

                    File.Delete(sourceFile);
                }

            }

        }

        public static void Log(LogState logLevel, string message, TestContext testContext = null)
        {
            string testName;

            if (testContext == null)
            {
                testName = "";

            }
            else
            {
                testName = testContext.TestName;

            }

            switch (logLevel)
            {
                case LogState.Info:
                    string TestMsg = (testName == "") ? "" : " : " + testName;
                    _logger.Info(message + TestMsg);
                    break;
                case LogState.Error:

                    string dateStamp = DateTime.Now.ToString("yyyy_MM_dd_HHmmss");
                    
                    string ErrorLogFileName;

                    if (testName == "")
                        ErrorLogFileName = _mainLogFilePath;
                    else
                        ErrorLogFileName = Path.Combine(_logDirectory, $"{_logFolderName}", $"{_logFileName}_{testName}_{dateStamp}.log");

                    var ErrorLogTarget = GetLoggerTarget(ErrorLogFileName, testName);
                    ErrorLogTarget.Layout = "${date} : ${level:uppercase=true} : ${message} ${exception} : ${var:testName} ";
                    _config.AddTarget(ErrorLogTarget);



                    var Errorrule = new LoggingRule(testName, LogLevel.Error, ErrorLogTarget);
                    _config.LoggingRules.Add(Errorrule);

                    LogManager.Configuration = _config;
                    LogManager.Configuration.Variables["testName"] = testName;

                    _logger = LogManager.GetLogger(testName);
                    _logger.Error(message);

                    //if testcontext is not null then add the file to test case
                    testContext?.AddResultFile(ErrorLogFileName);

                    break;
                default:
                    break;
            }

        }

        private static FileTarget GetLoggerTarget(string FilePath, string TargetName)
        {
            string LoggerName = (TargetName == "") ? "Error" : TargetName;
            var LogTarget = new FileTarget();
            LogTarget.CreateDirs = true;
            LogTarget.FileNameKind = FilePathKind.Relative;
            LogTarget.FileName = FilePath;
            LogTarget.Name = LoggerName;
            LogManager.GetLogger(LoggerName);
            return LogTarget;
        }
    }

}

