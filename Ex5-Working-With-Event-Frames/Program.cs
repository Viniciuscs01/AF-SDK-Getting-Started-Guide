using System;
using External;

namespace Ex5_Working_With_Event_Frames_Sln
{
    class Program
    {
        static void Main(string[] args)
        {
            AFEventFrameCreator efCreator = new AFEventFrameCreator("PISRV01", "Magical Power Company");
            efCreator.CreateEventFrameTemplate();
            efCreator.CreateEventFrames();
            efCreator.CaptureValues();
            efCreator.PrintReport();

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}
