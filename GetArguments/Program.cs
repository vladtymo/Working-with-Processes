using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetArguments
{
    class Program
    {
        static void Main(string[] args)
        {
            foreach (var item in args)
            {
                Console.WriteLine(item);
            }

            //int count = int.Parse(args[0]);
            //for (int i = 0; i < count; i++)
            //{
            //    Console.WriteLine(args[1]);
            //}

            Console.ReadKey();
        }
    }
}
