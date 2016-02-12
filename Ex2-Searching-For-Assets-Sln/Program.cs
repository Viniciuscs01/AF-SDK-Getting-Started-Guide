﻿using System;
using External;

namespace Ex2_Searching_For_Assets_Sln
{
    class Program
    {
        static void Main(string[] args)
        {
            AFAssetSearcher searcher = new AFAssetSearcher("PISRV01", "Magical Power Company");
            searcher.FindMetersByName("Meter00*");
            searcher.FindMetersByTemplate("MeterBasic");
            searcher.FindMetersBySubstation("Edinburgh");
            searcher.FindMetersAboveUsage(300);
            searcher.FindBuildingInfo("MeterAdvanced");

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}
