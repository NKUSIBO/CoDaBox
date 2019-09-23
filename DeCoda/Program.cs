using System;
using System.IO;

namespace DeCoda
{
    class Program
    {

        static void Main(string[] args)
        {
            Console.WriteLine("Start");

            var sourcePath = "/Users/bilal/Downloads/Coda/cod/";

            var dir = new DirectoryInfo(sourcePath);

            var files = dir.GetFiles();

            foreach (var file in files)
            {
                var decoda = new DeCoda();
                var statement = decoda.getStatement(file.FullName);
            }
            Console.WriteLine("End");

        }
    }
}
