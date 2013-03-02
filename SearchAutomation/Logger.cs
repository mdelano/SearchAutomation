using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SearchAutomation
{
    public class Logger
    {
        public static void Append(string logMessage)
        {
            using (StreamWriter w = File.AppendText("log.txt"))
            {
                w.WriteLine(logMessage);
            }
        }
    }
}
