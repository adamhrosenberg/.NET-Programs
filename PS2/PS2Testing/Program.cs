using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpreadsheetUtilities;

using System.Threading.Tasks;

namespace PS2Testing
{
    class Program
    {
        static void Main(string[] args)
        {
            DependencyGraph dep = new DependencyGraph();
            dep.AddDependency("a", "b");//s is key t is value
            dep.AddDependency("a", "c");
            dep.AddDependency("b", "d");
            dep.AddDependency("c", "d");
            dep.AddDependency("a", "d");
            //Console.WriteLine((dep["d"]));
            HashSet<String> dees = new HashSet<String>(dep.GetDependents("a"));
            foreach (string s in dees)//print hashset
            {
                Console.WriteLine(s);
            }
            //Console.WriteLine(dep.HasDependees("a"));
            //Console.WriteLine(dep.HasDependents("x"));
            Console.Read();
        }
    }
}
