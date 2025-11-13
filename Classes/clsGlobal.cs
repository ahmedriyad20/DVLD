using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DVLD_BusinessLayer;

namespace DVLD_Project.Classes
{
    public class clsGlobal
    {
        public static clsUser CurrentUser { get; set; }

        public static void LogExecption(Exception ex, EventLogEntryType eventLogEntry = EventLogEntryType.Error)
        {
            string sourceName = "DVLD_Application";

            // Create the event source if it does not exist
            if (!EventLog.SourceExists(sourceName))
            {
                EventLog.CreateEventSource(sourceName, "Application");
            }

            // Log an information event
            EventLog.WriteEntry(sourceName, ex.Message, eventLogEntry);
        }

        public static void LogExecption(string Message, EventLogEntryType eventLogEntry = EventLogEntryType.Error)
        {
            string sourceName = "DVLD_Application";

            // Create the event source if it does not exist
            if (!EventLog.SourceExists(sourceName))
            {
                EventLog.CreateEventSource(sourceName, "Application");
            }

            // Log an information event
            EventLog.WriteEntry(sourceName, Message, eventLogEntry);
        }
    }
}
