using System;
using System.Linq;
using OSIsoft.AF;
using OSIsoft.AF.Asset;
using OSIsoft.AF.UnitsOfMeasure;

namespace Ex4_Building_AF_Hierarchy
{
    class Program4
    {
        static void Main(string[] args)
        {
            AFDatabase database = CreateDatabase(Environment.MachineName, "Mythical Power Company");
            CreateCategories(database);
            CreateEnumerationSets(database);
            CreateTemplates(database);
            CreateElements(database);
            SetAttributeValues(database);
            CreateDistrictElements(database);
            CreateWeakReferences(database);

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }

        static AFDatabase CreateDatabase(string servername, string databasename)
        {
            AFDatabase database = null;
            // Your code here
            return database;
        }

        static void CreateCategories(AFDatabase database)
        {
            if (database == null) return;
            // Your code here
        }

        static void CreateEnumerationSets(AFDatabase database)
        {
            if (database == null) return;
            // Your code here
        }

        static void CreateTemplates(AFDatabase database)
        {
            if (database == null) return;
            // Your code here
        }

        static void CreateElements(AFDatabase database)
        {
            if (database == null) return;
            // Your code here
        }

        static void SetAttributeValues(AFDatabase database)
        {
            if (database == null) return;
            // Your code here
        }

        static void CreateDistrictElements(AFDatabase database)
        {
            if (database == null) return;
            // Your code here
        }

        static void CreateWeakReferences(AFDatabase database)
        {
            if (database == null) return;
            // Your code here
        }
    }
}
