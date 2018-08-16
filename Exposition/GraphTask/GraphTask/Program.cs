using System;
using System.IO;

namespace GraphTask
{
    class Program
    {
        static int Main(string[] args)
        {
            try
            {
                if (args.Length == 1)
                {
                    using (FileStream source = new FileStream(args[0], FileMode.Open))
                    {
                        using (StreamReader streamReader = new StreamReader(source))
                        {
                            Inspector fileInspector = new Inspector(streamReader);

                            fileInspector.PrepareRailway();
                            if (!fileInspector.CheckCollision())
                            {
                                Console.WriteLine("Collision was found!");
                            }
                            else
                            {
                                Console.WriteLine("No collisions");
                            }
                            return 0;
                        }
                    }
                }
                Console.WriteLine("Wrong command!");
                Console.ReadKey();
                return 1;
            }
            catch (IOException e)
            {
                if (e.Source != null)
                    Console.WriteLine("IOException: {0}", e.Message);
                Console.ReadKey();
                return 1;
            }
            catch (Exception e)
            {
                if (e.Source != null)
                    Console.WriteLine("System exception: {0}", e.Message);
                Console.ReadKey();
                return 1;
            }
        }
    }
}
