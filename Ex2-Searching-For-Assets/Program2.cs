using System;
using OSIsoft.AF;
using OSIsoft.AF.Asset;
using OSIsoft.AF.Search;

namespace Ex2_Searching_For_Assets
{
    class Program2
    {
        static void Main(string[] args)
        {
            AFDatabase database = GetDatabase(Environment.MachineName, "Magical Power Company");
            FindMetersByName(database, "Meter00*");
            FindMetersByTemplate(database, "MeterBasic");
            FindMetersBySubstation(database, "Edinburgh");
            FindMetersAboveUsage(database, 300);
            FindBuildingInfo(database, "MeterAdvanced");

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }

        static AFDatabase GetDatabase(string servername, string databasename)
        {
            PISystems piafsystems = new PISystems();
            PISystem system = piafsystems[servername];
            if (system != null && system.Databases.Contains(databasename))
            {
                Console.WriteLine("Found '{0}' with '{1}' databases", system.Name, system.Databases.Count);
                return system.Databases[databasename];
            }
            else
                return null;
        }

        static void FindMetersByName(AFDatabase database, string elementNameFilter)
        {
            Console.WriteLine("Find Meters by Name: {0}", elementNameFilter);

            AFElementSearch elementquery = new AFElementSearch(database, "ElementSearch", elementNameFilter);
            foreach (AFElement element in elementquery.FindElements())
            {
                Console.WriteLine("Element: {0}, Template: {1}, Categories: {2}",
                    element.Name,
                    element.Template.Name,
                    element.CategoriesString);
            }

            Console.WriteLine();
        }

        static void FindMetersByTemplate(AFDatabase database, string templateName)
        {
            // Your code here
        }

        static void FindMetersBySubstation(AFDatabase database, string substationLocation)
        {
            // Your code here
        }

        static void FindMetersAboveUsage(AFDatabase database, double val)
        {
            // Your code here
        }

        static void FindBuildingInfo(AFDatabase database, string templateName)
        {
            // Your code here
        }
    }
}
