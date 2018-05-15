using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace _3_task
{
    class Program
    {
        static void Main(string[] args)
        {
            args = File.ReadAllLines("in.txt");
            var finder = new NetworkPathFinder(args);
            var wayStack = finder.GetWay();
            if (wayStack == null)
            {
                File.WriteAllText("out.txt", "N");
                return;
            }
            var way = new StringBuilder();
            while (wayStack.Count != 0)
                way.Append($"{wayStack.Pop()} ");
            File.WriteAllLines("out.txt", new []{"Y", way.ToString().Trim(), finder.Cost.ToString()});
        }
       
    }
}
