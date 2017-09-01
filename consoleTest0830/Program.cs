using System;
using System.Collections.Generic;

namespace consoleTest0830
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Console.Title = "水野由結最高！";
            //Console.WriteLine("Start");
            List<Asmart.aProd> resStockList = Asmart.getServerJson();
           // Console.WriteLine(resStockList);

            Asmart.listToTable(resStockList);
            Console.ReadLine();
            Console.WriteLine("clear");
            Console.ReadLine();
            //                Console.WriteLine(Asmart.getServerJson().Values.ToString());
            //Console.WriteLine(Asmart.AsmartJson);
            //Console.WriteLine(Asmart.Compute4());
        }
    }
}
