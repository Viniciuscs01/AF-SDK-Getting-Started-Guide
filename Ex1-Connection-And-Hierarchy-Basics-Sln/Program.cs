using System;
using OSIsoft.AF;
using OSIsoft.AF.Asset;
using OSIsoft.AF.UnitsOfMeasure;

namespace Ex1_Connection_And_Hierarchy_Basics_Sln
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
            Console.WriteLine("Print Element Templates");
            AFNamedCollectionList<AFElementTemplate> elemTemplates = database.ElementTemplates.FilterBy(typeof(AFElement));
            foreach (AFElementTemplate elemTemp in elemTemplates)
            {
                // Note: An alternative and faster approach is to use eTemp.CategoriesString
                string[] categories = new string[elemTemp.Categories.Count];
                int i = 0;
                foreach (AFCategory category in elemTemp.Categories)
                {
                    categories[i++] = category.Name;
                }

                string categoriesString = string.Join(",", categories);
                Console.WriteLine("Name: {0}, Categories: {1}", elemTemp.Name, elemTemp.CategoriesString);
            }

            Console.WriteLine();
        }

        static void PrintAttributeTemplates(AFDatabase database, string elemTempName)
        {
            Console.WriteLine("Print Attribute Templates for Element Template: {0}", elemTempName);
            AFElementTemplate elemTemp = database.ElementTemplates[elemTempName];
            foreach (AFAttributeTemplate attrTemp in elemTemp.AttributeTemplates)
            {
                string drName = attrTemp.DataReferencePlugIn == null ? "None" : attrTemp.DataReferencePlugIn.Name;
                Console.WriteLine("Name: {0}, DRPlugin: {1}", attrTemp.Name, drName);
            }

            Console.WriteLine();
        }

        static void PrintEnergyUOMs(PISystem system)
        {
            Console.WriteLine("Print Energy UOMs");
            UOMClass uomClass = system.UOMDatabase.UOMClasses["Energy"];
            foreach (UOM uom in uomClass.UOMs)
            {
                Console.WriteLine("UOM: {0}, Abbreviation: {1}", uom.Name, uom.Abbreviation);
            }

            Console.WriteLine();
        }

        static void PrintEnumerationSets(AFDatabase database)
        {
            Console.WriteLine("Print Enumeration Sets\n");
            AFEnumerationSets enumSets = database.EnumerationSets;
            foreach (AFEnumerationSet enumSet in enumSets)
            {
                Console.WriteLine(enumSet.Name);
                foreach (AFEnumerationValue state in enumSet)
                {
                    int stateValue = state.Value;
                    string stateName = state.Name;
                    Console.WriteLine("{0} - {1}", stateValue, stateName);
                }

                Console.WriteLine();
            }
        }

        static void PrintCategories(AFDatabase database)
        {
            Console.WriteLine("Print Categories\n");
            AFCategories elemCategories = database.ElementCategories;
            AFCategories attrCategories = database.AttributeCategories;

            Console.WriteLine("Element Categories");
            foreach (AFCategory category in elemCategories)
            {
                Console.WriteLine(category.Name);
            }

            Console.WriteLine();
            Console.WriteLine("Attribute Categories");
            foreach (AFCategory category in attrCategories)
            {
                Console.WriteLine(category.Name);
            }

            Console.WriteLine();
        }
    }
}
