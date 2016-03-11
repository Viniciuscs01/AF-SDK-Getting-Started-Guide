using System;
using System.Linq;
using OSIsoft.AF;
using OSIsoft.AF.Asset;
using OSIsoft.AF.UnitsOfMeasure;

namespace Ex4_Building_An_AF_Hierarchy
{
    static class Bonus
    {
        static void Run()
        {
            AFDatabase database = CreateDatabase("PISRV01", "Mythical Power Company");
            CreateCategories(database);
            CreateEnumerationSets(database);
            CreateTemplates(database);
            CreateElements(database);
            SetAttributeValues(database);
            CreateDistrictElements(database);
            CreateWeakReferences(database);
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
