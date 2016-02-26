using System;
using System.Collections.Generic;
using OSIsoft.AF;
using OSIsoft.AF.Asset;
using OSIsoft.AF.Search;

namespace Ex2_Searching_For_Assets_Sln
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
            Console.WriteLine("Find Meters by Template: {0}", templateName);

            AFElementSearch elementquery = new AFElementSearch(database, "TemplateSearch", string.Format("template:\"{0}\"", templateName));
            foreach (AFElement element in elementquery.FindElements())
            {
                Console.WriteLine("Element: {0}, Template: {1}", element.Name, element.Template.Name);
            }

            Console.WriteLine();
        }

        static void FindMetersBySubstation(AFDatabase database, string substationLocation)
        {
            Console.WriteLine("Find Meters by Substation: {0}", substationLocation);

            string templateName = "MeterBasic";
            string attributeName = "Substation";
            AFElementSearch elementquery = new AFElementSearch(database, "AttributeValueEQSearch",
                string.Format("template:\"{0}\" \"|{1}\":\"{2}\"", templateName, attributeName, substationLocation));

            int countNames = 0;
            foreach (AFElement element in elementquery.FindElements())
            {
                Console.Write("{0}{1}", countNames++ == 0 ? string.Empty : ", ", element.Name);
            }

            Console.WriteLine("\n");
        }

        static void FindMetersAboveUsage(AFDatabase database, double val)
        {
            Console.WriteLine("Find Meters above Usage: {0}", val);

            string templateName = "MeterBasic";
            string attributeName = "Energy Usage";
            AFElementSearch elementquery = new AFElementSearch(database, "AttributeValueGTSearch",
                string.Format("template:\"{0}\" \"|{1}\":>{2}", templateName, attributeName, val));

            int countNames = 0;
            foreach (AFElement element in elementquery.FindElements())
            {
                Console.Write("{0}{1}", countNames++ == 0 ? string.Empty : ", ", element.Name);
            }

            Console.WriteLine("\n");
        }

        static void FindBuildingInfo(AFDatabase database, string templateName)
        {
            Console.WriteLine("Find Building Info: {0}", templateName);

            AFElementTemplate elemTemp = database.ElementTemplates[templateName];
            AFCategory buildingInfoCat = database.AttributeCategories["Building Info"];

            AFElement root = database.Elements["Wizarding World"];

            AFNamedCollectionList<AFAttribute> foundAttributes = AFAttribute.FindElementAttributes(
                                                    database: database,
                                                    searchRoot: root,
                                                    nameFilter: "*",
                                                    elemCategory: null,
                                                    elemTemplate: elemTemp,
                                                    elemType: AFElementType.Any,
                                                    attrNameFilter: "*",
                                                    attrCategory: buildingInfoCat,
                                                    attrType: TypeCode.Empty,
                                                    searchFullHierarchy: true,
                                                    sortField: AFSortField.Name,
                                                    sortOrder: AFSortOrder.Ascending,
                                                    maxCount: 100);

            Console.WriteLine("Found {0} attributes.", foundAttributes.Count);
            Console.WriteLine();
        }
    }
}
