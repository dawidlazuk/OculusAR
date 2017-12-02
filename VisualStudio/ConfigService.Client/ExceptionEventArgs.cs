using System;

namespace ConfigService.Client
{
    public delegate void ExceptionEventHandler(object sender, ExceptionEventArgs e);

    public class ExceptionEventArgs : EventArgs
    {
        public Exception Exception { get; set; }
    }
}
