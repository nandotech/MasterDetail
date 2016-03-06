using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using log4net.Core;
using log4net.Layout;

namespace MasterDetail.Models
{
    public class TracingLayout : ExceptionLayout
    {
        public override void Format(TextWriter textWriter, LoggingEvent loggingEvent)
        {
            base.Format(textWriter, loggingEvent);

            if (loggingEvent.ExceptionObject != null)
                textWriter.Write(Environment.StackTrace);
        }
    }
}