using System.Linq;
using OSIsoft.AF;
using OSIsoft.AF.Asset;
using OSIsoft.AF.UnitsOfMeasure;

namespace Ex4_Building_An_AF_Hierarchy_Sln
{
    public class AFHierarchyBuilder
    {
        private PISystem _piSystem;
        private AFDatabase _database;

        public AFHierarchyBuilder(string server)
        {
            _piSystem = new PISystems()[server];
        }

        public void CreateDatabase()
        {
            if (_piSystem == null) return;

            _database = _piSystem.Databases.Add("Mythical Power Company");
            _database.CheckIn();
        }

        public void CreateCategories()
        {
            if (_database == null) return;

            _database.ElementCategories.Add("Measures Energy");
            _database.ElementCategories.Add("Shows Status");

            _database.AttributeCategories.Add("Building Info");
            _database.AttributeCategories.Add("Location");
            _database.AttributeCategories.Add("Time-Series Data");

            _database.CheckIn();
        }

        public void CreateEnumerationSets()
        {
            if (_database == null) return;

            AFEnumerationSet bTypeEnum = _database.EnumerationSets.Add("Building Type");
            bTypeEnum.Add("Residential", 0);
            bTypeEnum.Add("Business", 1);

            AFEnumerationSet mStatusEnum = _database.EnumerationSets.Add("Meter Status");
            mStatusEnum.Add("Good", 0);
            mStatusEnum.Add("Bad", 1);

            _database.CheckIn();
        }

        public void CreateTemplates()
        {
            if (_database == null || _piSystem == null) return;

            UOM uom = _piSystem.UOMDatabase.UOMs["kilowatt hour"];

            AFCategory mEnergyE = _database.ElementCategories["Measures Energy"];
            AFCategory sStatusE = _database.ElementCategories["Shows Status"];

            AFCategory bInfoA = _database.AttributeCategories["Building Info"];
            AFCategory locationA = _database.AttributeCategories["Location"];
            AFCategory tsDataA = _database.AttributeCategories["Time-Series Data"];

            AFEnumerationSet bTypeNum = _database.EnumerationSets["Building Type"];
            AFEnumerationSet mStatusEnum = _database.EnumerationSets["Meter Status"];

            // Create MeterBasic Element Template

            AFElementTemplate meterBasicTemplate = _database.ElementTemplates.Add("MeterBasic");
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
            energyUsageAttrTemp.DataReferencePlugIn = _piSystem.DataReferencePlugIns["PI Point"];
            energyUsageAttrTemp.ConfigString = @"\\%Server%\%Element%.%Attribute%;UOM=kWh";

            // Create MeterAdvanced Element Template

            AFElementTemplate meterAdvancedTemplate = _database.ElementTemplates.Add("MeterAdvanced");
            meterAdvancedTemplate.BaseTemplate = meterBasicTemplate;

            AFAttributeTemplate statusAttrTemp = meterAdvancedTemplate.AttributeTemplates.Add("Status");
            statusAttrTemp.TypeQualifier = mStatusEnum;
            statusAttrTemp.Categories.Add(tsDataA);
            statusAttrTemp.DataReferencePlugIn = _piSystem.DataReferencePlugIns["PI Point"];
            statusAttrTemp.ConfigString = @"\\%Server%\%Element%.%Attribute%";

            // Create District Element Template

            AFElementTemplate districtTemplate = _database.ElementTemplates.Add("District");

            AFAttributeTemplate districtEnergyUsageAttrTemp = districtTemplate.AttributeTemplates.Add("Energy Usage");
            districtEnergyUsageAttrTemp.Type = typeof(double);
            districtEnergyUsageAttrTemp.DefaultUOM = uom;
            districtEnergyUsageAttrTemp.DataReferencePlugIn = _piSystem.DataReferencePlugIns["PI Point"];
            districtEnergyUsageAttrTemp.ConfigString = @"\\%Server%\%Element%.%Attribute%";

            // Do a checkin at the end instead of one-by-one.

            _database.CheckIn();
        }

        public void CreateElements()
        {
            if (_database == null) return;

            AFElement meters = _database.Elements.Add("Meters");

            AFElementTemplate basic = _database.ElementTemplates["MeterBasic"];
            AFElementTemplate advanced = _database.ElementTemplates["MeterAdvanced"];

            foreach (int i in Enumerable.Range(1,12))
            {
                AFElementTemplate eTemp = i <= 8 ? basic : advanced;
                string name = "Meter" + i.ToString("D3");
                AFElement e = meters.Elements.Add(name, eTemp);
            }

            _database.CheckIn();
        }

        public void SetAttributeValues()
        {
            if (_database == null) return;

            AFElement meter001 = _database.Elements["Meters"].Elements["Meter001"];
            meter001.Attributes["Substation"].SetValue(new AFValue("Edinburgh"));
            meter001.Attributes["Usage Limit"].SetValue(new AFValue(350));
            meter001.Attributes["Building"].SetValue(new AFValue("Gryffindor"));

            AFEnumerationValue bTypeValue = _database.EnumerationSets["Building Type"]["Residential"];
            meter001.Attributes["Building Type"].SetValue(new AFValue(bTypeValue));
            meter001.Attributes["District"].SetValue(new AFValue("Hogwarts"));
        }

        public void CreateDistrictElements()
        {
            if (_database == null) return;

            AFElement wizardingWorld = _database.Elements.Add("Wizarding World");
            AFElementTemplate districtTemplate = _database.ElementTemplates["District"];

            wizardingWorld.Elements.Add("Diagon Alley", districtTemplate);
            wizardingWorld.Elements.Add("Hogsmeade", districtTemplate);
            wizardingWorld.Elements.Add("Hogwarts", districtTemplate);

            _database.CheckIn();
        }

        public void CreateWeakReferences()
        {
            if (_database == null) return;

            AFReferenceType weakRefType = _database.ReferenceTypes["Weak Reference"];

            AFElement meters = _database.Elements["Meters"];

            AFElement hogwarts = _database.Elements["Wizarding World"].Elements["Hogwarts"];
            hogwarts.Elements.Add(meters.Elements["Meter001"], weakRefType);
            hogwarts.Elements.Add(meters.Elements["Meter002"], weakRefType);
            hogwarts.Elements.Add(meters.Elements["Meter003"], weakRefType);
            hogwarts.Elements.Add(meters.Elements["Meter004"], weakRefType);

            AFElement diagonAlley = _database.Elements["Wizarding World"].Elements["Diagon Alley"];
            diagonAlley.Elements.Add(meters.Elements["Meter005"], weakRefType);
            diagonAlley.Elements.Add(meters.Elements["Meter006"], weakRefType);
            diagonAlley.Elements.Add(meters.Elements["Meter007"], weakRefType);
            diagonAlley.Elements.Add(meters.Elements["Meter008"], weakRefType);

            AFElement hogsmeade = _database.Elements["Wizarding World"].Elements["Hogsmeade"];
            hogsmeade.Elements.Add(meters.Elements["Meter009"], weakRefType);
            hogsmeade.Elements.Add(meters.Elements["Meter010"], weakRefType);
            hogsmeade.Elements.Add(meters.Elements["Meter011"], weakRefType);
            hogsmeade.Elements.Add(meters.Elements["Meter012"], weakRefType);

            _database.CheckIn();
        }
    }
}
