using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using FormulaEvaluator;

namespace HW1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(Evaluator.evaluate("5+5-2*3/5", lookup));
            Console.Read();
        }
        static int lookup(string s)
        {
            var values = new Dictionary<string, int>
            {
                {"XX1",5}, {"y2",5}
            };
            if (!values.ContainsKey(s)) throw new ArgumentException("Variable has no value");
            return values[s];
        }
    }
}
