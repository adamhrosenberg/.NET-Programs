using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Text.RegularExpressions;
///Adam Rosenberg
///CS 3500
///September 10 2015
///PS1
///Homework 1
namespace FormulaEvaluator
{
    /// <summary>
    /// Evaluator class holds all my functions and computes the result of any expression.
    /// </summary>
    public static class Evaluator
    {
        /// <summary>
        /// Lookup delegate for lookup function.
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public delegate int Lookup(String v);
        /// <summary>
        /// Evaluator takes in an expression and a function (lookup). The evaluator
        /// parses the expression, removes any whitespace via trimming, an then follows the algorithm given to us
        /// to find the result of the expression. At the end, we cast the result as an int and return back to the tester class
        /// which writes it to the console. LookupVar is a function in my testing class that implements a dictionary full of variables and
        /// values - the we look through when we find a variable to find its value. 
        /// </summary>
        /// <param name="string exp, Lookup lookupVar"></param>
        /// <returns>int result</returns>
        public static int evaluate(String exp, Lookup lookupVar)//everytime yuo use a lookup function you do a try catch. 
        {
            ///Create regex that looks for a string (variable) that will help me look for a substring starting with a-z or A-Z (at least one) with numbers after it. 
            Regex regex = new Regex("^[a-zA-Z]+[0-9]+$");
            ///Create my double stack nums that holds all the numbers I'm interacting with within my expression.
            Stack<double> nums = new Stack<double>();
            ///Create my string stack opers that holds all the operators that are used. 
            Stack<string> opers = new Stack<string>();
            //Create double temps that I tried to avoid using but couldn't totally avoid. I use the temps to store stack.Pop() values when passing them into my 
            //doOperation function.
            double temp1;
            double temp2;
            //split up the expression using Regex. This line of code was given to us. 
            string[] substrings = Regex.Split(exp, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");
            //Start for loop, iterate through until I have parsed the entire expression.
            for (int i = 0; i < substrings.Length; i++)
            {
                //Trim the substring (token) as I get them. Easy and simple way to deal with whitespaces. 
                substrings[i] = substrings[i].Trim();
                int num;
                //Parse the substring and determine whether it is an integer or not. If so, assign the value to num if not continue.
                bool isNumber = int.TryParse(substrings[i], out num);
                if (isNumber)
                {
                    if (checkTop(nums, opers, "*", "/"))
                    {
                        nums.Push(doOperation(nums.Pop(), num, opers.Pop()));
                    }
                    else
                    {
                        nums.Push(num);
                    }
                }//end number if
                else if (regex.IsMatch(substrings[i]))
                {
                    num=lookupVar(substrings[i]);//lookup the variable in the dictionary. 
                    if (checkTop(nums, opers, "*", "/"))
                    {
                        nums.Push(doOperation(nums.Pop(), num, opers.Pop()));//to keep PEMDAS 
                    }
                    else
                    {
                        nums.Push(num);
                    }
                }//variable
                else if (substrings[i].Equals("+") || substrings[i].Equals("-"))
                {
                    //Too complicated to imlpement by checkTop function so I just do it by hand to catch all exceptions and push correctly. 
                        if (opers.Count > 0)
                       {
                            if (opers.Peek().Equals("+") || opers.Peek().Equals("-"))//Looking for +,- on top.
                            {
                                if (nums.Count < 2)
                                {
                                    throw new ArgumentException("Invalid syntax");//extra operator in wrong place.
                                }
                                nums.Push(doOperation(nums.Pop(), nums.Pop(), opers.Pop()));
                                opers.Push(substrings[i]);
                            }
                            else
                            {
                                opers.Push(substrings[i]);
                            }
                       }
                    else
                    {
                        if (nums.Count == 0)
                        {
                            throw new ArgumentException("Invalid Syntax");//leading operation before anything else throw an argument.
                        }
                        opers.Push(substrings[i]);
                    }
                }//end +,-
                else if (substrings[i].Equals("*") || substrings[i].Equals("/"))
                {
                    if (nums.Count == 0)
                    {
                        throw new ArgumentException("Invalid Syntax");//leading operation invalid syntax.
                    }
                    opers.Push(substrings[i]);
                }//end *,/
                else if (substrings[i].Equals("("))
                {
                    opers.Push(substrings[i]);//push ( on oper stack because there's nothing else to do with the (
                }
                else if (substrings[i].Equals(")"))
                {
                    if (opers.Count == 0)
                    {
                        throw new ArgumentException("Invalid Syntax");//cant start with a )
                    }
                        if (opers.Peek().Equals("+") || opers.Peek().Equals("-"))
                        {
                            if (nums.Count < 2)
                            {
                                throw new ArgumentException("Invalid Syntax");//there has to be at least 2 numbers inside a paranthesis. otherwise invalid syntax.
                            }
                            nums.Push(doOperation(nums.Pop(), nums.Pop(), opers.Pop()));//if not do the operation. 
                        }
                    if (opers.Count==0)
                    {
                        throw new ArgumentException("Invalid Syntax");
                    }
                    if (opers.Peek().Equals("("))
                        {
                            opers.Pop(); //pop the ( of the operator stack to finish off the (..) expression.
                        }
                        else
                        {
                        throw new ArgumentException("Invalid syntax");

                        }
                        if (opers.Count > 0)
                        {
                            if (opers.Peek().Equals("*") || opers.Peek().Equals("/"))
                            {
                                temp2 = nums.Pop();
                                temp1 = nums.Pop();
                                nums.Push(doOperation(temp1, temp2, opers.Pop()));
                            }
                        }
                }//end )
            }
            if (opers.Count == 0 && nums.Count==1)
            {
                //If the operator stack is empty, and there's something in the num stack then that is the result. so return it. 
                return (int)nums.Pop();
            }
            else if (opers.Count ==1 && nums.Count==2)
            {
                return (int)doOperation(nums.Pop(), nums.Pop(), opers.Pop());//One operator left on opstack and 2 values on number stack means we have to one last operation
            }
            else if(opers.Count>0 && nums.Count > 0)
            {
                throw new ArgumentException("Invaild Syntax");//most of the time there is a left paran sitting in the oper stack for this to execute.
            }
            else
            {
                throw new ArgumentException("Invalid Syntax");//throw last argument just in case.
            }
        }
        /// <summary>
        /// This function takes in operators, determines what the operator is and applies it to the two double values also passed in.
        /// </summary>
        /// <param name="value1">double value 1</param>
        /// <param name="value2">double value 2</param>
        /// <param name="oper">string operator</param>
        /// <returns></returns>
        public static double doOperation(double value1, double value2, string oper)
        {
            if (oper.Equals("+"))
                return value1 + value2;
            if (oper.Equals("-"))
                return value2 - value1;
            if (oper.Equals("*"))
                return value1 * value2;
            if (oper.Equals("/"))
            {
                if (value2 == 0)
                {
                    throw new ArgumentException("Diving by zero");
                }
                return value1 / value2;
            }
            return 1;
        }
        /// <summary>
        /// Checks the count of both stacks, then peeks the stack that is passed in to see whether or not the desired operator is on top of the oper stack.
        /// </summary>
        /// <param name="nums">number stack</param>
        /// <param name="opers">operator stack</param>
        /// <param name="oper1">first oper</param>
        /// <param name="oper2">second oper</param>
        /// <returns></returns>
        public static Boolean checkTop(Stack<double>nums, Stack<string>opers, string oper1, string oper2)
        {
            if(nums.Count>0 && opers.Count > 0)
            {
                return (opers.Peek().Equals(oper1) || opers.Peek().Equals(oper2));
            }
            return false;
        }
        /// <summary>
        /// Checks the count, then peeks the stack that is passed in to see whether or not the desired operator is on top of the oper stack. 
        /// Returns true if one of the operators are on top.
        /// </summary>
        /// <param name="opers">operator stack</param>
        /// <param name="oper1">first oper to check for</param>
        /// <param name="oper2">second oper to check for</param>
        /// <returns></returns>
        public static Boolean checkTop(Stack<string> opers, string oper1, string oper2)
        {
            if (opers.Count > 0)
            {
                return (opers.Peek().Equals(oper1) || opers.Peek().Equals(oper2));
            }
            return false;
        }
    }
}

