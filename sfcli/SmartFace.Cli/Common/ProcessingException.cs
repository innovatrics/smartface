using System;

namespace SmartFace.Cli.Common
{
    public class ProcessingException : Exception
    {
        public ProcessingException(string msg) : base(msg) {}
        public ProcessingException(string msg, Exception ex) : base(msg, ex) {}
    }
}