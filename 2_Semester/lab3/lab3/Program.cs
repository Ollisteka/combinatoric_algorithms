using System.IO;

namespace lab3
{
    class Program
    {
        static void Main()
        {
            var args = File.ReadAllLines("in.txt");
            var helper = new RalphHelper(args);
            File.WriteAllText("out.txt", helper.HelpRalph().ToString());
        }
    }
}
