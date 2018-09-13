using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab1
{
    class Program
    {
        static void Main(string[] args)
        {
            args = File.ReadAllLines("in.txt");
            var reader = new MSTFinder(args);

            var minimalSpanningTree = reader.FindMinimalSpanningTree();
            File.WriteAllLines("out.txt", reader.PrintMST(minimalSpanningTree));
        }
    }
}
