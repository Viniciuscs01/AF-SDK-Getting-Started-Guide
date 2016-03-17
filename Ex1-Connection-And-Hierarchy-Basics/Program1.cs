using System;
using OSIsoft.AF;
using OSIsoft.AF.Asset;
using OSIsoft.AF.UnitsOfMeasure;

namespace Ex1_Connection_And_Hierarchy_Basics
{
    class Program1
    {
        static void Main(string[] args)
        {
            AFDatabase database = GetDatabase("PISRV01", "Magical Power Company");

            PrintRootElements(database);
            PrintElementTemplates(database);
            PrintAttributeTemplates(database, "MeterAdvanced");
            PrintEnergyUOMs(database.PISystem);
            PrintEnumerationSets(database);
            PrintCategories(database);

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }

        static AFDatabase GetDatabase(string server, string database)
        {
            PISystems piSystems = new PISystems();
            PISystem piSystem = piSystems[server];
            AFDatabase afDatabase = piSystem.Databases[database];
            return afDatabase;
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
