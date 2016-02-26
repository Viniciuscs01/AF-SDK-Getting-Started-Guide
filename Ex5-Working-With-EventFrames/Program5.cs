using System;
using System.Collections.Generic;
using System.Linq;
using OSIsoft.AF;
using OSIsoft.AF.Asset;
using OSIsoft.AF.EventFrame;
using OSIsoft.AF.Time;

namespace Ex5_Working_With_EventFrames
{
    class Program5
    {
        static void Main(string[] args)
        {
            AFDatabase database = GetDatabase(Environment.MachineName, "Magical Power Company");
            AFElementTemplate eventframetemplate = CreateEventFrameTemplate(database);
            CreateEventFrames(database, eventframetemplate);
            CaptureValues(database, eventframetemplate);
            PrintReport(database, eventframetemplate);

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }

        static AFDatabase GetDatabase(string servername, string databasename)
        {
            PISystem system = GetPISystem(null, servername);
            if (!string.IsNullOrEmpty(databasename))
                return system.Databases[databasename];
            else
                return system.Databases.DefaultDatabase;
        }

        static PISystem GetPISystem(PISystems systems = null, string systemname = null)
        {
            systems = systems == null ? new PISystems() : systems;
            if (!string.IsNullOrEmpty(systemname))
                return systems[systemname];
            else
                return systems.DefaultPISystem;
        }

        static AFElementTemplate CreateEventFrameTemplate(AFDatabase database)
        {
            AFElementTemplate eventframetemplate = null;
            // Your code here
            return eventframetemplate;
        }

        static void CreateEventFrames(AFDatabase database, AFElementTemplate eventFrameTemplate)
        {
            // Your code here
        }

        static public void CaptureValues(AFDatabase database, AFElementTemplate eventFrameTemplate)
        {
            // Your code here
        }

        static void PrintReport(AFDatabase database, AFElementTemplate eventFrameTemplate)
        {
            // Your code here
        }
    }
}
