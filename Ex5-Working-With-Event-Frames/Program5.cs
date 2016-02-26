using System;
using System.Collections.Generic;
using System.Linq;
using OSIsoft.AF;
using OSIsoft.AF.Asset;
using OSIsoft.AF.EventFrame;
using OSIsoft.AF.Time;


namespace Ex5_Working_With_EventFrames_Sln
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
            if (database.ElementTemplates.Contains("Daily Usage"))
            {
                return database.ElementTemplates["Daily Usage"];
            }

            eventframetemplate = database.ElementTemplates.Add("Daily Usage");
            eventframetemplate.InstanceType = typeof(AFEventFrame);
            eventframetemplate.NamingPattern = @"%TEMPLATE%-%ELEMENT%-%STARTTIME:yyyy-MM-dd%-EF*";

            AFAttributeTemplate usage = eventframetemplate.AttributeTemplates.Add("Average Energy Usage");
            usage.Type = typeof(double);
            usage.DataReferencePlugIn = AFDataReference.GetPIPointDataReference();
            usage.ConfigString = @".\Elements[.]|Energy Usage;TimeRangeMethod=Average";
            usage.DefaultUOM = database.PISystem.UOMDatabase.UOMs["kilowatt hour"];

            database.CheckIn();
            return eventframetemplate;
        }

        static void CreateEventFrames(AFDatabase database, AFElementTemplate eventFrameTemplate)
        {
            const int pageSize = 1000;
            int startIndex = 0;
            int totalCount;
            do
            {
                AFNamedCollectionList<AFBaseElement> results = database.ElementTemplates["MeterBasic"].FindInstantiatedElements(
                    includeDerived: true,
                    sortField: AFSortField.Name,
                    sortOrder: AFSortOrder.Ascending,
                    startIndex: startIndex,
                    maxCount: pageSize,
                    totalCount: out totalCount
                    );

                IList<AFElement> meters = results.Select(elm => (AFElement)elm).ToList();

                foreach (AFElement meter in meters)
                {
                    foreach (int day in Enumerable.Range(1, 5))
                    {
                        DateTime start = new DateTime(2016, 2, day, 0, 0, 0, DateTimeKind.Local);
                        AFTime startTime = new AFTime(start);
                        AFTime endTime = new AFTime(start.AddDays(1));
                        AFEventFrame ef = new AFEventFrame(database, "*", eventFrameTemplate);
                        ef.SetStartTime(startTime);
                        ef.SetEndTime(endTime);
                        ef.PrimaryReferencedElement = meter;
                    }
                }

                database.CheckIn();

                startIndex += pageSize;
            } while (startIndex < totalCount);
        }

        static public void CaptureValues(AFDatabase database, AFElementTemplate eventFrameTemplate)
        {
            const int pageSize = 1000;
            int startIndex = 0;

            AFTime startTime = new AFTime(new DateTime(2016, 2, 1, 0, 0, 0, DateTimeKind.Local));
            AFNamedCollectionList<AFEventFrame> efs;
            do
            {
                efs = AFEventFrame.FindEventFrames(
                    database: database,
                    searchRoot: null,
                    startTime: startTime,
                    startIndex: startIndex,
                    maxCount: pageSize,
                    searchMode: AFEventFrameSearchMode.ForwardFromStartTime,
                    nameFilter: "*",
                    referencedElementNameFilter: "*",
                    eventFrameCategory: null,
                    eventFrameTemplate: eventFrameTemplate,
                    referencedElementTemplate: null,
                    searchFullHierarchy: true
                    );

                foreach (AFEventFrame ef in efs)
                {
                    if (!ef.AreValuesCaptured)
                        ef.CaptureValues();
                }

                database.CheckIn();

                startIndex += pageSize;
            } while (efs != null && efs.Count > 0);
        }

        static void PrintReport(AFDatabase database, AFElementTemplate eventFrameTemplate)
        {
            const int pageSize = 1000;
            int startIndex = 0;

            AFTime startTime = new AFTime(new DateTime(2016, 2, 1, 0, 0, 0, DateTimeKind.Local));
            AFTime endTime = new AFTime(new DateTime(2016, 2, 6, 0, 0, 0, DateTimeKind.Local));

            AFNamedCollectionList<AFEventFrame> efs;
            do
            {
                efs = AFEventFrame.FindEventFrames(
                    database: database,
                    searchRoot: null,
                    searchMode: AFSearchMode.StartInclusive,
                    startTime: startTime,
                    endTime: endTime,
                    startIndex: startIndex,
                    maxCount: pageSize,
                    nameFilter: "*",
                    referencedElementNameFilter: "Meter003",
                    eventFrameCategory: null,
                    eventFrameTemplate: eventFrameTemplate,
                    referencedElementTemplate: null,
                    durationQuery: null,
                    searchFullHierarchy: true,
                    sortField: AFSortField.Name,
                    sortOrder: AFSortOrder.Ascending
                    );

                // This loads all the attributes in one call to the AF Server.
                // This prevents the loop below from making one call per iteration.
                AFEventFrame.LoadEventFrames(efs);

                foreach (AFEventFrame ef in efs)
                {
                    Console.WriteLine("{0}, {1}, {2}",
                        ef.Name,
                        ef.PrimaryReferencedElement.Name,
                        ef.Attributes["Average Energy Usage"].GetValue().Value);
                }

                startIndex += pageSize;
            } while (efs != null && efs.Count > 0);
        }
    }
}
