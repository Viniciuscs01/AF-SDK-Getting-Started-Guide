using System;
using OSIsoft.AF;
using OSIsoft.AF.Asset;
using OSIsoft.AF.UnitsOfMeasure;

namespace Ex1_Connection_And_Hierarchy_Basics
{
    class Program
    {
        static void Main(string[] args)
        {
            AFDatabase database = GetDatabase(Environment.MachineName, "Magical Power Company");
            PrintRootElements(database);
            PrintElementTemplates(database);
            PrintAttributeTemplates(database, "MeterAdvanced");
            PrintEnergyUOMs(database.PISystem);
            PrintEnumerationSets(database);
            PrintCategories(database);

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

        static void PrintRootElements(AFDatabase database)
        {
            Console.WriteLine("Print Root Elements: {0}", database.Elements.Count);
            foreach (AFElement element in database.Elements)
            {
                Console.WriteLine("  {0}", element.Name);
            }

            Console.WriteLine();
        }

        static void PrintElementTemplates(AFDatabase database)
        {
            // Your code here
        }

        static void PrintAttributeTemplates(AFDatabase database, string elemTempName)
        {
            // Your code here
        }

        static void PrintEnergyUOMs(PISystem system)
        {
            // Your code here
        }

        static void PrintEnumerationSets(AFDatabase database)
        {
            // Your code here
        }

        static void PrintCategories(AFDatabase database)
        {
            // Your code here
        }
    }
}
