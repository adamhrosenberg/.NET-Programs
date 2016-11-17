// Skeleton written by Joe Zachary for CS 3500, September 2013
// Read the entire skeleton carefully and completely before you
// do anything else!

// Version 1.1 (9/22/13 11:45 a.m.)

// Change log:
//  (Version 1.1) Repaired mistake in GetTokens
//  (Version 1.1) Changed specification of second constructor to
//                clarify description of how validation works
//Adam Rosenberg
//PS3
//9/24/15
//Hey matthew, thanks so much for everything! I hope this is pretty readable. I got my coverage as high as I could
//but my uncovered code is mostly exceptions.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SpreadsheetUtilities  
{
    /// <summary>
    /// Represents formulas written in standard infix notation using standard precedence
    /// rules.  The allowed symbols are non-negative numbers written using double-precision 
    /// floating-point syntax; variables that consist of a letter or underscore followed by 
    /// zero or more letters, underscores, or digits; parentheses; and the four operator 
    /// symbols +, -, *, and /.  
    /// 
    /// Spaces are significant only insofar that they delimit tokens.  For example, "xy" is
    /// a single variable, "x y" consists of two variables "x" and y; "x23" is a single variable; 
    /// and "x 23" consists of a variable "x" and a number "23".
    /// 
    /// Associated with every formula are two delegates:  a normalizer and a validator.  The
    /// normalizer is used to convert variables into a canonical form, and the validator is used
    /// to add extra restrictions on the validity of a variable (beyond the standard requirement 
    /// that it consist of a letter or underscore followed by zero or more letters, underscores,
    /// or digits.)  Their use is described in detail in the constructor and method comments.
    /// </summary>
    public class Formula
    {
        private List<object> formulas;
        /// <summary>
        /// Creates a Formula from a string that consists of an infix expression written as
        /// described in the class comment.  If the expression is syntactically invalid,
        /// throws a FormulaFormatException with an explanatory Message.
        /// 
        /// The associated normalizer is the identity function, and the associated validator
        /// maps every string to true.  
        /// </summary>
        public Formula(String formula) :
            this(formula, s => s, s => true)
        {
        }

        /// <summary>
        /// Creates a Formula from a string that consists of an infix expression written as
        /// described in the class comment.  If the expression is syntactically incorrect,
        /// throws a FormulaFormatException with an explanatory Message.
        /// 
        /// The associated normalizer and validator are the second and third parameters,
        /// respectively.  
        /// 
        /// If the formula contains a variable v such that normalize(v) is not a legal variable, 
        /// throws a FormulaFormatException with an explanatory message. 
        /// 
        /// If the formula contains a variable v such that isValid(normalize(v)) is false,
        /// throws a FormulaFormatException with an explanatory message.
        /// 
        /// Suppose that N is a method that converts all the letters in a string to upper case, and
        /// that V is a method that returns true only if a string consists of one letter followed
        /// by one digit.  Then:
        /// 
        /// new Formula("x2+y3", N, V) should succeed
        /// new Formula("x+y3", N, V) should throw an exception, since V(N("x")) is false
        /// new Formula("2x+y3", N, V) should throw an exception, since "2x+y3" is syntactically incorrect.
        /// </summary>
        public Formula(String formula, Func<string, string> normalize, Func<string, bool> isValid)
        {
            string temp = formula.Trim();
            if (temp.Length > 0)
            {
                //parse string. pass parsed string into isValid to determine if the variable is valid or not.
                string[] formArray = GetTokens(formula).ToArray();
                int leftParan = 0;
                int rightParan = 0;

                //formula array now holds the formula character by char in an array
                //look at formarray[0] see if it is a double, left paran, or variable. if not throw exception
                string first = formArray[0];

                if (!(first.isVar() || first.isDouble() || first.isOpen()))
                    throw new FormulaFormatException("A character is of invalid syntax");

                //look at last element in array and see if it is a double, variable, or closing paran if not throw exception
                string last = formArray[formArray.Length - 1];
                if (!(last.isClosed() || last.isDouble() || last.isVar()))
                    throw new FormulaFormatException("Last character is of invalid syntax");
                for (int i=0; i<formArray.Length; i++)
                {
                    string s = formArray[i];
                    if (s.isOpen())
                        leftParan++; //keep paran count
                    if (s.isClosed())
                        rightParan++;
                    if (rightParan > leftParan)
                        throw new FormulaFormatException("more right parans than left");//paran off
                    if (!(i == formArray.Length - 1))//dont check if we're on the last loop. 
                    {
                        if (s.isOpen() || s.isOper())
                        {
                            if (!(formArray[i + 1].isDouble() || formArray[i + 1].isOpen() || formArray[i + 1].isVar()))
                                throw new FormulaFormatException("Paranthesis following rule");
                        }
                        if (s.isClosed() || s.isDouble() || s.isVar())
                        {
                            if (!(formArray[i + 1].isOper() || formArray[i + 1].isClosed()))
                                throw new FormulaFormatException("Extra following rule");
                        }
                    }
                }//end foreach
                if (!(leftParan==rightParan))
                    throw new FormulaFormatException("number of left and right parans dont equal eachother");
                //save formula into a list of strings
                
                formulas = new List<object>();//store formulas in a data structure thatholds them all
               foreach(string s in formArray)
                {
                    if (s.isVar())
                    {
                        string x = normalize(s);
                        if(isValid(x))//check new string 
                        {
                            formulas.Add(x);
                        }
                        else
                        {
                            throw new FormulaFormatException("Variable not valid");//if variable is not valid as defined by isvalid
                        }
                    }
                        
                    if (s.isDouble())//check if its a good double to add
                    {
                        double value;
                        Double.TryParse(s, out value);
                        formulas.Add(value);//add double value
                    }
                    if (s.isOper())//check is oper
                        formulas.Add(s);
                    if (s.isOpen() || s.isClosed())//check parans...only add valid stuff.
                        formulas.Add(s);
                }
            }
            else
            {
                throw new FormulaFormatException("No expression");
            }
        }

        /// <summary>
        /// Evaluates this Formula, using the lookup delegate to determine the values of
        /// variables.  When a variable symbol v needs to be determined, it should be looked up
        /// via lookup(normalize(v)). (Here, normalize is the normalizer that was passed to 
        /// the constructor.)
        /// 
        /// For example, if L("x") is 2, L("X") is 4, and N is a method that converts all the letters 
        /// in a string to upper case:
        /// 
        /// new Formula("x+7", N, s => true).Evaluate(L) is 11
        /// new Formula("x+7").Evaluate(L) is 9
        /// 
        /// Given a variable symbol as its parameter, lookup returns the variable's value 
        /// (if it has one) or throws an ArgumentException (otherwise).
        /// 
        /// If no undefined variables or divisions by zero are encountered when evaluating 
        /// this Formula, the value is returned.  Otherwise, a FormulaError is returned.  
        /// The Reason property of the FormulaError should have a meaningful explanation.
        ///
        /// This method should never throw an exception.
        /// </summary>
        public object Evaluate(Func<string, double> lookup)
        {
            ///Create regex that looks for a string (variable) that will help me look for a substring starting with a-z or A-Z (at least one) with numbers after it. 
            Regex regex = new Regex(@"[a-zA-Z_](?: [a-zA-Z_]|\d)*");
            ///Create my double stack nums that holds all the numbers I'm interacting with within my expression.
            Stack<double> nums = new Stack<double>();
            ///Create my string stack opers that holds all the operators that are used. 
            Stack<string> opers = new Stack<string>();
            //Create double temps that I tried to avoid using but couldn't totally avoid. I use the temps to store stack.Pop() values when passing them into my 
            //doOperation function.
            double temp1;
            double temp2;
            //split up the expression using Regex. This line of code was given to us. 
            string[] substrings = Regex.Split(this.ToString(), "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");
            //Start for loop, iterate through until I have parsed the entire expression.
            for (int i = 0; i < substrings.Length; i++)
            {
                //Trim the substring (token) as I get them. Easy and simple way to deal with whitespaces. 
                substrings[i] = substrings[i].Trim();
                double num;
                //Parse the substring and determine whether it is an integer or not. If so, assign the value to num if not continue.
                bool isNumber = double.TryParse(substrings[i], out num);
                if (isNumber)
                {
                    if (checkTop(nums, opers, "*", "/"))
                    {
                        try
                        {
                           nums.Push(doOperation(nums.Pop(), num, opers.Pop()));
                        }
                        catch(ArgumentException e)
                        {
                            return new FormulaError(e.Message);
                        }
                    }
                    else
                    {
                        nums.Push(num);
                    }
                }//end number if
                else if (regex.IsMatch(substrings[i]))
                {
                    try
                    {
                        num = lookup(substrings[i]);//lookup the variable in the dictionary. 
                    }
                    catch(ArgumentException e)
                    {
                        return new FormulaError(e.Message);//undefined variable so return now formulaerror!
                    }
                    if (checkTop(nums, opers, "*", "/"))
                    {
                        try
                        {
                            nums.Push(doOperation(nums.Pop(), num, opers.Pop()));//to keep PEMDAS 
                        }
                        catch(ArgumentException e)//check div/0
                        {
                            return new FormulaError(e.Message);
                        }
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
                        opers.Push(substrings[i]);
                    }
                }//end +,-
                else if (substrings[i].Equals("*") || substrings[i].Equals("/"))
                {
                    opers.Push(substrings[i]);
                }//end *,/
                else if (substrings[i].Equals("("))
                {
                    opers.Push(substrings[i]);//push ( on oper stack because there's nothing else to do with the (
                }
                else if (substrings[i].Equals(")"))
                {
                    if (opers.Peek().Equals("+") || opers.Peek().Equals("-"))
                    {
                        nums.Push(doOperation(nums.Pop(), nums.Pop(), opers.Pop()));//if not do the operation. 
                    }
                    if (opers.Peek().Equals("("))
                    {
                        opers.Pop(); //pop the ( of the operator stack to finish off the (..) expression.
                    }
                    if (opers.Count > 0)
                    {
                        if (opers.Peek().Equals("*") || opers.Peek().Equals("/"))
                        {
                            temp2 = nums.Pop();
                            temp1 = nums.Pop();
                            try
                            {
                               nums.Push(doOperation(temp1, temp2, opers.Pop()));
                            }
                            catch (ArgumentException e)//check div/0
                            {
                                return new FormulaError(e.Message);
                            }    
                               
                        }
                    }
                }//end )
            }
            if (opers.Count == 0 && nums.Count == 1)
            {
                //If the operator stack is empty, and there's something in the num stack then that is the result. so return it. 
                return (double)nums.Pop();
            }
            else if (opers.Count == 1 && nums.Count == 2)
            {
                try //
                {
                    double r = doOperation(nums.Pop(), nums.Pop(), opers.Pop());//One operator left on opstack and 2 values on number stack means we have to one last operation
                    return r;
                }
                catch (ArgumentException e)
                {
                    return new FormulaError(e.Message);
                }
              
            }
            else
            {
                return new FormulaError("Invalid Syntax");//throw last argument just in case.
            }
        }
        /// <summary>
        /// checktop of stack. made generic as a fix from ps1. no more redundency!
        /// </summary>
        /// <param name="nums"></param>
        /// <param name="opers"></param>
        /// <param name="oper1"></param>
        /// <param name="oper2"></param>
        /// <returns></returns>
        private static Boolean checkTop<T>(Stack<T> nums, Stack<string> opers, string oper1, string oper2)
        {
            if (nums.Count > 0 && opers.Count > 0)
            {
                return (opers.Peek().Equals(oper1) || opers.Peek().Equals(oper2));
            }
            return false;
        }

        /// <summary>
        /// This function takes in operators, determines what the operator is and applies it to the two double values also passed in.
        /// </summary>
        /// <param name="value1">double value 1</param>
        /// <param name="value2">double value 2</param>
        /// <param name="oper">string operator</param>
        /// <returns></returns>
        private static double doOperation(double value1, double value2, string oper)
        {
            if (oper.Equals("+"))//operator table and returns.
                return value1 + value2;
            if (oper.Equals("-"))
                return value2 - value1;
            if (oper.Equals("*"))
                return value1 * value2;
            if (oper.Equals("/"))
            {
                if (value2 == 0)
                {
                    throw new ArgumentException("Diving by zero");//throw exception that is caught by the try()
                }
                return value1 / value2;
            }
            return -1; //not needed but kept it just in case. did an else for my / sign by i wanted to keep this to be safe. 93% code coverage isn't that bad, right?
        }
        /// <summary>
        /// Enumerates the normalized versions of all of the variables that occur in this 
        /// formula.  No normalization may appear more than once in the enumeration, even 
        /// if it appears more than once in this Formula.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        /// 
        /// new Formula("x+y*z", N, s => true).GetVariables() should enumerate "X", "Y", and "Z"
        /// new Formula("x+X*z", N, s => true).GetVariables() should enumerate "X" and "Z".
        /// new Formula("x+X*z").GetVariables() should enumerate "x", "X", and "z".
        /// </summary>
        public IEnumerable<String> GetVariables()
        {
            HashSet<string> vars = new HashSet<string>();
            foreach(object s in formulas)
            {
                if(s is string)
                {
                    string x = (string)s;
                    if (x.isVar())
                    {

                        if (!(vars.Contains(x)))
                        {//dont add duplicate variables
                            vars.Add(x);
                        }
                    }
                }
            }

            return vars;
        }

        /// <summary>
        /// Returns a string containing no spaces which, if passed to the Formula
        /// constructor, will produce a Formula f such that this.Equals(f).  All of the
        /// variables in the string should be normalized.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        /// 
        /// new Formula("x + y", N, s => true).ToString() should return "X+Y"
        /// new Formula("x + Y").ToString() should return "x+Y"
        /// </summary>
        public override string ToString()
        {
            string newString = "";
            foreach(object s in formulas)
            {
                newString += s;
            }
            return newString;
        }

        /// <summary>
        /// If obj is null or obj is not a Formula, returns false.  Otherwise, reports
        /// whether or not this Formula and obj are equal.
        /// 
        /// Two Formulae are considered equal if they consist of the same tokens in the
        /// same order.  To determine token equality, all tokens are compared as strings 
        /// except for numeric tokens, which are compared as doubles, and variable tokens,
        /// whose normalized forms are compared as strings.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        ///  
        /// new Formula("x1+y2", N, s => true).Equals(new Formula("X1  +  Y2")) is true
        /// new Formula("x1+y2").Equals(new Formula("X1+Y2")) is false
        /// new Formula("x1+y2").Equals(new Formula("y2+x1")) is false
        /// new Formula("2.0 + x7").Equals(new Formula("2.000 + x7")) is true
        /// </summary>
        public override bool Equals(object obj)
        {
            if(obj is Formula && (!(Object.ReferenceEquals(obj, null))))//make sure obj is a formula and !=null
            {
               return Enumerable.SequenceEqual((obj as Formula).formulas, this.formulas);
            }
            return false;
        }

        /// <summary>
        /// Reports whether f1 == f2, using the notion of equality from the Equals method.
        /// Note that if both f1 and f2 are null, this method should return true.  If one is
        /// null and one is not, this method should return false.
        /// </summary>
        public static bool operator ==(Formula f1, Formula f2)
        {
           if (Object.ReferenceEquals(f1, null) && Object.ReferenceEquals(f2, null))
                return true; //null==null
            if (Object.ReferenceEquals(f2, null) && !Object.ReferenceEquals(f1, null))
                return false;
            if (!Object.ReferenceEquals(f2, null) && Object.ReferenceEquals(f1, null))
                return false;
            //TODO check this null ps3
            return f2.Equals(f1); 
        }

        /// <summary>
        /// Reports whether f1 != f2, using the notion of equality from the Equals method.
        /// Note that if both f1 and f2 are null, this method should return false.  If one is
        /// null and one is not, this method should return true.
        /// </summary>
        public static bool operator !=(Formula f1, Formula f2)
        {
            //use the == method
            return !(f1 == f2);
        }

        /// <summary>
        /// Returns a hash code for this Formula.  If f1.Equals(f2), then it must be the
        /// case that f1.GetHashCode() == f2.GetHashCode().  Ideally, the probability that two 
        /// randomly-generated unequal Formulae have the same hash code should be extremely small.
        /// </summary>
        public override int GetHashCode()
        {
            return (this.ToString().GetHashCode()*22);//convert to string, use that hashcode() multiply by 22 just to personalize. 
        }

        /// <summary>
        /// Given an expression, enumerates the tokens that compose it.  Tokens are left paren;
        /// right paren; one of the four operator symbols; a string consisting of a letter or underscore
        /// followed by zero or more letters, digits, or underscores; a double literal; and anything that doesn't
        /// match one of those patterns.  There are no empty tokens, and no token contains white space.
        /// </summary>
        private static IEnumerable<string> GetTokens(String formula)
        {
            // Patterns for individual tokens
            String lpPattern = @"\(";
            String rpPattern = @"\)";
            String opPattern = @"[\+\-*/]";
            String varPattern = @"[a-zA-Z_](?: [a-zA-Z_]|\d)*";
            String doublePattern = @"(?: \d+\.\d* | \d*\.\d+ | \d+ ) (?: [eE][\+-]?\d+)?";
            String spacePattern = @"\s+";
            // Overall pattern
            String pattern = String.Format("({0}) | ({1}) | ({2}) | ({3}) | ({4}) | ({5})",
                                            lpPattern, rpPattern, opPattern, varPattern, doublePattern, spacePattern);

            // Enumerate matching tokens that don't consist solely of white space.
            foreach (String s in Regex.Split(formula, pattern, RegexOptions.IgnorePatternWhitespace))
            {
                if (!Regex.IsMatch(s, @"^\s*$", RegexOptions.Singleline))
                {
                    yield return s;
                }
            }
        }
    }
    /// <summary>
    /// extension class for extension methods
    /// </summary>
    public static class Extension
    {
        /// <summary>
        /// Extension class to be implemented for our strings to check...check if double
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool isDouble(this string s)
        {
            double value;
            return (Double.TryParse(s, out value));
        }
        /// <summary>
        /// check if var
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool isVar(this string s)
        {
            return Regex.IsMatch(s, @"^[a-zA-Z_](?: [a-zA-Z_]|\d)*");
        }
        /// <summary>
        /// check if oper
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool isOper(this string s)
        {
            return Regex.IsMatch(s, @"^[\+\-*/]$");
        }
        /// <summary>
        /// check if left paran
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool isOpen(this string s)
        {
            return (s.Equals("("));
        }
        /// <summary>
        /// check if right paran
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool isClosed(this string s)
        {
            return (s.Equals(")"));
        }
    }
    /// <summary>
    /// Used to report syntactic errors in the argument to the Formula constructor.
    /// </summary>
    public class FormulaFormatException : Exception
    {
        /// <summary>
        /// Constructs a FormulaFormatException containing the explanatory message.
        /// </summary>
        public FormulaFormatException(String message)
            : base(message)
        {
        }
    }

    /// <summary>
    /// Used as a possible return value of the Formula.Evaluate method.
    /// </summary>
    public struct FormulaError
    {
        /// <summary>
        /// Constructs a FormulaError containing the explanatory reason.
        /// </summary>
        /// <param name="reason"></param>
        public FormulaError(String reason)
            : this()
        {
            Reason = reason;
        }

        /// <summary>
        ///  The reason why this FormulaError was created.
        /// </summary>
        public string Reason { get; private set; }
    }
}