using System;
using System.Linq;
using OSIsoft.AF;
using OSIsoft.AF.Asset;
using OSIsoft.AF.UnitsOfMeasure;

namespace Ex4_Building_An_AF_Hierarchy_Sln
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
            PISystems piafsystems = new PISystems();
            PISystem system = piafsystems[servername];
            if (system != null)
            {
                if (system.Databases.Contains(databasename))
                    database = system.Databases[databasename];
                else
                    database = system.Databases.Add(databasename);
            }

            return database;
        }

        static void CreateCategories(AFDatabase database)
        {
            if (database == null) return;

            if (!database.ElementCategories.Contains("Measures Energy"))
                database.ElementCategories.Add("Measures Energy");

            if (!database.ElementCategories.Contains("Shows Status"))
                database.ElementCategories.Add("Shows Status");

            if (!database.AttributeCategories.Contains("Building Info"))
                database.AttributeCategories.Add("Building Info");

            if (!database.AttributeCategories.Contains("Location"))
                database.AttributeCategories.Add("Location");

            if (!database.AttributeCategories.Contains("Time-Series Data"))
                database.AttributeCategories.Add("Time-Series Data");

            database.CheckIn();
        }

        static void CreateEnumerationSets(AFDatabase database)
        {
            if (database == null) return;

            if (!database.EnumerationSets.Contains("Building Type"))
            {
                AFEnumerationSet bTypeEnum = database.EnumerationSets.Add("Building Type");
                bTypeEnum.Add("Residential", 0);
                bTypeEnum.Add("Business", 1);
            }

            if (!database.EnumerationSets.Contains("Meter Status"))
            {
                AFEnumerationSet mStatusEnum = database.EnumerationSets.Add("Meter Status");
                mStatusEnum.Add("Good", 0);
                mStatusEnum.Add("Bad", 1);
            }

            database.CheckIn();
        }

        static void CreateTemplates(AFDatabase database)
        {
            if (database == null) return;

            UOM uom = database.PISystem.UOMDatabase.UOMs["kilowatt hour"];

            AFCategory mEnergyE = database.ElementCategories["Measures Energy"];
            AFCategory sStatusE = database.ElementCategories["Shows Status"];

            AFCategory bInfoA = database.AttributeCategories["Building Info"];
            AFCategory locationA = database.AttributeCategories["Location"];
            AFCategory tsDataA = database.AttributeCategories["Time-Series Data"];

            AFEnumerationSet bTypeNum = database.EnumerationSets["Building Type"];
            AFEnumerationSet mStatusEnum = database.EnumerationSets["Meter Status"];

            // Create MeterBasic Element Template

            AFElementTemplate meterBasicTemplate;
            if (!database.ElementTemplates.Contains("MeterBasic"))
            {
                meterBasicTemplate = database.ElementTemplates.Add("MeterBasic");
                meterBasicTemplate.Categories.Add(mEnergyE);

                AFAttributeTemplate substationAttrTemp = meterBasicTemplate.AttributeTemplates.Add("Substation");
                substationAttrTemp.Type = typeof(string);

                AFAttributeTemplate usageLimitAttrTemp = meterBasicTemplate.AttributeTemplates.Add("Usage Limit");
                usageLimitAttrTemp.Type = typeof(string);
                usageLimitAttrTemp.DefaultUOM = uom;

                AFAttributeTemplate buildingAttrTemp = meterBasicTemplate.AttributeTemplates.Add("Building");
                buildingAttrTemp.Type = typeof(string);
                buildingAttrTemp.Categories.Add(bInfoA);

                AFAttributeTemplate bTypeAttrTemp = meterBasicTemplate.AttributeTemplates.Add("Building Type");
                bTypeAttrTemp.TypeQualifier = bTypeNum;
                bTypeAttrTemp.Categories.Add(bInfoA);

                AFAttributeTemplate districtAttrTemp = meterBasicTemplate.AttributeTemplates.Add("District");
                districtAttrTemp.Type = typeof(string);
                districtAttrTemp.Categories.Add(locationA);

                AFAttributeTemplate energyUsageAttrTemp = meterBasicTemplate.AttributeTemplates.Add("Energy Usage");
                energyUsageAttrTemp.Type = typeof(double);
                energyUsageAttrTemp.Categories.Add(tsDataA);
                energyUsageAttrTemp.DefaultUOM = uom;
                energyUsageAttrTemp.DataReferencePlugIn = database.PISystem.DataReferencePlugIns["PI Point"];
                energyUsageAttrTemp.ConfigString = @"\\%Server%\%Element%.%Attribute%;UOM=kWh";
            }
            else
                meterBasicTemplate = database.ElementTemplates["MeterBasic"];

            // Create MeterAdvanced Element Template

            if (!database.ElementTemplates.Contains("MeterAdvanced"))
            {
                AFElementTemplate meterAdvancedTemplate = database.ElementTemplates.Add("MeterAdvanced");
                meterAdvancedTemplate.BaseTemplate = meterBasicTemplate;

                AFAttributeTemplate statusAttrTemp = meterAdvancedTemplate.AttributeTemplates.Add("Status");
                statusAttrTemp.TypeQualifier = mStatusEnum;
                statusAttrTemp.Categories.Add(tsDataA);
                statusAttrTemp.DataReferencePlugIn = database.PISystem.DataReferencePlugIns["PI Point"];
                statusAttrTemp.ConfigString = @"\\%Server%\%Element%.%Attribute%";

                // Create District Element Template

                AFElementTemplate districtTemplate = database.ElementTemplates.Add("District");

                AFAttributeTemplate districtEnergyUsageAttrTemp = districtTemplate.AttributeTemplates.Add("Energy Usage");
                districtEnergyUsageAttrTemp.Type = typeof(double);
                districtEnergyUsageAttrTemp.DefaultUOM = uom;
                districtEnergyUsageAttrTemp.DataReferencePlugIn = database.PISystem.DataReferencePlugIns["PI Point"];
                districtEnergyUsageAttrTemp.ConfigString = @"\\%Server%\%Element%.%Attribute%";
            }

            // Do a checkin at the end instead of one-by-one.

            database.CheckIn();
        }

        static void CreateElements(AFDatabase database)
        {
            if (database == null) return;

            AFElement meters;
            if (!database.Elements.Contains("Meters"))
                meters = database.Elements.Add("Meters");
            else
                meters = database.Elements["Meters"];

            AFElementTemplate basic = database.ElementTemplates["MeterBasic"];
            AFElementTemplate advanced = database.ElementTemplates["MeterAdvanced"];

            foreach (int i in Enumerable.Range(1, 12))
            {
                string name = "Meter" + i.ToString("D3");
                if (!meters.Elements.Contains(name))
                {
                    AFElementTemplate eTemp = i <= 8 ? basic : advanced;
                    AFElement e = meters.Elements.Add(name, eTemp);
                }
            }

            database.CheckIn();
        }

        static void SetAttributeValues(AFDatabase database)
        {
            if (database == null) return;

            AFElement meter001 = database.Elements["Meters"].Elements["Meter001"];
            meter001.Attributes["Substation"].SetValue(new AFValue("Edinburgh"));
            meter001.Attributes["Usage Limit"].SetValue(new AFValue(350));
            meter001.Attributes["Building"].SetValue(new AFValue("Gryffindor"));

            AFEnumerationValue bTypeValue = database.EnumerationSets["Building Type"]["Residential"];
            meter001.Attributes["Building Type"].SetValue(new AFValue(bTypeValue));
            meter001.Attributes["District"].SetValue(new AFValue("Hogwarts"));
        }

        static void CreateDistrictElements(AFDatabase database)
        {
            if (database == null) return;

            if (!database.Elements.Contains("Wizarding World"))
            {
                AFElement wizardingWorld = database.Elements.Add("Wizarding World");
                AFElementTemplate districtTemplate = database.ElementTemplates["District"];

                wizardingWorld.Elements.Add("Diagon Alley", districtTemplate);
                wizardingWorld.Elements.Add("Hogsmeade", districtTemplate);
                wizardingWorld.Elements.Add("Hogwarts", districtTemplate);
            }

            database.CheckIn();
        }

        static void CreateWeakReferences(AFDatabase database)
        {
            if (database == null) return;

            AFReferenceType weakRefType = database.ReferenceTypes["Weak Reference"];

            AFElement meters = database.Elements["Meters"];

            AFElement hogwarts = database.Elements["Wizarding World"].Elements["Hogwarts"];
            hogwarts.Elements.Add(meters.Elements["Meter001"], weakRefType);
            hogwarts.Elements.Add(meters.Elements["Meter002"], weakRefType);
            hogwarts.Elements.Add(meters.Elements["Meter003"], weakRefType);
            hogwarts.Elements.Add(meters.Elements["Meter004"], weakRefType);

            AFElement diagonAlley = database.Elements["Wizarding World"].Elements["Diagon Alley"];
            diagonAlley.Elements.Add(meters.Elements["Meter005"], weakRefType);
            diagonAlley.Elements.Add(meters.Elements["Meter006"], weakRefType);
            diagonAlley.Elements.Add(meters.Elements["Meter007"], weakRefType);
            diagonAlley.Elements.Add(meters.Elements["Meter008"], weakRefType);

            AFElement hogsmeade = database.Elements["Wizarding World"].Elements["Hogsmeade"];
            hogsmeade.Elements.Add(meters.Elements["Meter009"], weakRefType);
            hogsmeade.Elements.Add(meters.Elements["Meter010"], weakRefType);
            hogsmeade.Elements.Add(meters.Elements["Meter011"], weakRefType);
            hogsmeade.Elements.Add(meters.Elements["Meter012"], weakRefType);

            database.CheckIn();
        }
    }
}
