using System;
using ILogger = VendingMachineLib.ILogger;

namespace VendingMachineTest
{
    public class LoggerSubstitute : ILogger
    {
        public bool ControlPassed => _controlPassedLog || _controlPassedLogWarning || _controlPassedLogError;
        
        private bool _controlPassedLog;
        
        private bool _controlPassedLogWarning;
        
        private bool _controlPassedLogError;

        public void Log(string message)
        {
            _controlPassedLog = true;
        }

        public void LogWarning(string message)
        {
            _controlPassedLogWarning = true;
        }

        public void LogError(string message)
        {
            _controlPassedLogError = true;
        }
    }
}