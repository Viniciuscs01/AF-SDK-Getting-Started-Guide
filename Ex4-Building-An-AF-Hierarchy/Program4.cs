using System;
using OSIsoft.AF;
using OSIsoft.AF.Asset;

namespace Ex4_Building_AF_Hierarchy
{
    class Program4
    {
        static void Main(string[] args)
        {
            AFDatabase database = GetDatabase("PISRV01", "Magical Power Company");
            CreateElementTemplate(database);
            CreateFeedersRootElement(database);
            CreateFeederElements(database);
            CreateWeakReference(database);

            // This bonus exercise  creates a replica database
            //Bonus.Run();

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

        static void CreateElementTemplate(AFDatabase database)
        {
            string templateName = "FeederTemplate";
            AFElementTemplate feederTemplate;
            if (database.ElementTemplates.Contains(templateName))
                return;
            else
                feederTemplate = database.ElementTemplates.Add(templateName);

            AFAttributeTemplate district = feederTemplate.AttributeTemplates.Add("District");
            district.Type = typeof(string);

            AFAttributeTemplate power = feederTemplate.AttributeTemplates.Add("Power");
            power.Type = typeof(Single);

            power.DefaultUOM = database.PISystem.UOMDatabase.UOMs["watt"];
            power.DataReferencePlugIn = database.PISystem.DataReferencePlugIns["PI Point"];

            database.CheckIn();
        }

        static void CreateFeedersRootElement(AFDatabase database)
        {
            // Your code here
        }

        static void CreateFeederElements(AFDatabase database)
        {
            // Your code here
        }

        static void CreateWeakReference(AFDatabase database)
        {
            // Your code here
        }
    }
}
