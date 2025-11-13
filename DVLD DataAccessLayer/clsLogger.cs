using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccessLayer
{
    public class clsLogger
    {
        public static void LogExecption(Exception ex, EventLogEntryType eventLogEntry = EventLogEntryType.Error)
        {
            string sourceName = "DVLD_Application";

            try
            {
                // Create the event source if it does not exist
                if (!EventLog.SourceExists(sourceName))
                {
                    EventLog.CreateEventSource(sourceName, "Application");
                }

                // Log an information event
                EventLog.WriteEntry(sourceName, ex.Message, eventLogEntry);
            }
            catch(Exception Ex)
            {
                EventLog.WriteEntry(sourceName, Ex.Message, eventLogEntry);
            }
            
        }

        public static void LogExecption(string Message, EventLogEntryType eventLogEntry = EventLogEntryType.Error)
        {
            string sourecName = "DVLD_Application";

            // Create the event source if it does not exist
            if (!EventLog.SourceExists(sourecName))
            {
                EventLog.CreateEventSource(sourecName, "Application");
            }

            // Log an information event
            EventLog.WriteEntry(sourecName, Message, eventLogEntry);
        }
    }
}
