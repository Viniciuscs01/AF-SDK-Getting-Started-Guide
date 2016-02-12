using System;
using OSIsoft.AF;
using OSIsoft.AF.Asset;
using OSIsoft.AF.UnitsOfMeasure;

namespace Ex1_Connection_And_Hierarchy_Basics_Sln
{
    public class AFPrinter
    {
        private AFDatabase _database;

        public AFPrinter(string server, string database)
        {
            PISystem piSystem = new PISystems()[server];
            if (piSystem != null) _database = piSystem.Databases[database];
        }

        public void PrintRootElements()
        {
            Console.WriteLine("Print Root Elements");
            AFElements elements = _database.Elements;
            foreach (AFElement e in elements)
            {
                Console.WriteLine(e.Name);
            }
            Console.WriteLine();
        }

        public void PrintElementTemplates()
        {
            Console.WriteLine("Print Element Templates");
            AFNamedCollectionList<AFElementTemplate> elemTemplates = _database.ElementTemplates.FilterBy(typeof(AFElement));
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

        public void PrintAttributeTemplates(string elemTempName)
        {
            Console.WriteLine("Print Attribute Templates for Element Template: {0}", elemTempName);
            AFElementTemplate elemTemp = _database.ElementTemplates[elemTempName];
            foreach (AFAttributeTemplate attrTemp in elemTemp.AttributeTemplates)
            {
                string drName = attrTemp.DataReferencePlugIn == null ? "None" : attrTemp.DataReferencePlugIn.Name;
                Console.WriteLine("Name: {0}, DRPlugin: {1}", attrTemp.Name, drName);
            }
            Console.WriteLine();
        }

        public void PrintEnergyUOMs()
        {
            Console.WriteLine("Print Energy UOMs");
            PISystem piSystem = _database.PISystem;
            UOMClass uomClass = piSystem.UOMDatabase.UOMClasses["Energy"];
            foreach (UOM uom in uomClass.UOMs)
            {
                Console.WriteLine("UOM: {0}, Abbreviation: {1}", uom.Name, uom.Abbreviation);
            }
            Console.WriteLine();
        }

        public void PrintEnumerationSets()
        {
            Console.WriteLine("Print Enumeration Sets\n");
            AFEnumerationSets enumSets = _database.EnumerationSets;
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

        public void PrintCategories()
        {
            Console.WriteLine("Print Categories\n");
            AFCategories elemCategories = _database.ElementCategories;
            AFCategories attrCategories = _database.AttributeCategories;

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
