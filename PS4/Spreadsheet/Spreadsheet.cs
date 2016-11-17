using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SS;
using SpreadsheetUtilities;
using System.Text.RegularExpressions;

// Adam Rosenberg
// u0751643
// PS4
 //test
namespace SS 
{
    /// <summary>
    /// An AbstractSpreadsheet object represents the state of a simple spreadsheet.  A 
    /// spreadsheet consists of an infinite number of named cells.
    /// 
    /// A string is a valid cell name if and only if:
    ///   (1) its first character is an underscore or a letter
    ///   (2) its remaining characters (if any) are underscores and/or letters and/or digits
    /// Note that this is the same as the definition of valid variable from the PS3 Formula class.
    /// 
    /// For example, "x", "_", "x2", "y_15", and "___" are all valid cell  names, but
    /// "25", "2x", and "&" are not.  Cell names are case sensitive, so "x" and "X" are
    /// different cell names.
    /// 
    /// A spreadsheet contains a cell corresponding to every possible cell name.  (This
    /// means that a spreadsheet contains an infinite number of cells.)  In addition to 
    /// a name, each cell has a contents and a value.  The distinction is important.
    /// 
    /// The contents of a cell can be (1) a string, (2) a double, or (3) a Formula.  If the
    /// contents is an empty string, we say that the cell is empty.  (By analogy, the contents
    /// of a cell in Excel is what is displayed on the editing line when the cell is selected.)
    /// 
    /// In a new spreadsheet, the contents of every cell is the empty string.
    ///  
    /// The value of a cell can be (1) a string, (2) a double, or (3) a FormulaError.  
    /// (By analogy, the value of an Excel cell is what is displayed in that cell's position
    /// in the grid.)
    /// 
    /// If a cell's contents is a string, its value is that string.
    /// 
    /// If a cell's contents is a double, its value is that double.
    /// 
    /// If a cell's contents is a Formula, its value is either a double or a FormulaError,
    /// as reported by the Evaluate method of the Formula class.  The value of a Formula,
    /// of course, can depend on the values of variables.  The value of a variable is the 
    /// value of the spreadsheet cell it names (if that cell's value is a double) or 
    /// is undefined (otherwise).
    /// 
    /// Spreadsheets are never allowed to contain a combination of Formulas that establish
    /// a circular dependency.  A circular dependency exists when a cell depends on itself.
    /// For example, suppose that A1 contains B1*2, B1 contains C1*2, and C1 contains A1*2.
    /// A1 depends on B1, which depends on C1, which depends on A1.  That's a circular
    /// dependency.
    /// </summary>
    public class Spreadsheet : AbstractSpreadsheet
    {
        //TODO test isVlid extension method make sure it lines up with new regulations. - isValid 
        /// <summary>
        /// DependencyGraph to store dependency amongst cells
        /// </summary>
        private DependencyGraph dep;
        /// <summary>
        /// Dictionary (basically the entire spreadsheet)
        /// </summary>
        private Dictionary<String, Cell> dic;
        /// <summary>
        /// Public Spreadsheet - that is an abstract spreadsheet.
        /// </summary>
        public Spreadsheet()
        {
            dep = new DependencyGraph();
            dic = new Dictionary<string, Cell>();
        }
        /// <summary>
        /// Enumerates the names of all the non-empty cells in the spreadsheet.
        /// </summary>
        public override IEnumerable<string> GetNamesOfAllNonemptyCells()
        {
            foreach(KeyValuePair<string, Cell> x in dic)
            {
                if (!(x.Value.Contents.Equals(string.Empty)))//if the cell is not empty, build the ienumerable with yield return of key
                    yield return x.Key; 
            }

        }
        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, returns the contents (as opposed to the value) of the named cell.  The return
        /// value should be either a string, a double, or a Formula.
        public override object GetCellContents(string name)
        {
            if (name.isNull() || (!(name.isValid())))//if name is null or not a valid variable throw exception
                throw new InvalidNameException();
            if(!(dic.ContainsKey(name)))//if name isn't in the dictionary - return empty string
                return string.Empty;
            return dic[name].Contents;//since this means name is in dictionary, return the contents.
        }
        /// <summary>
        /// Self-made and tested method to return cell value
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public object GetCellValue(string name)
        {
            if (name.isNull() || (!(name.isValid())))//if name is null or not a valid variable throw exception
                throw new InvalidNameException();
            if (!(dic.ContainsKey(name)))//if name isn't in the dictionary - return empty string
                return string.Empty;
            return dic[name].Value;//since this means name is in dictionary, return the contents.
        }
        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, the contents of the named cell becomes number.  The method returns a
        /// set consisting of name plus the names of all other cells whose value depends, 
        /// directly or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        public override ISet<string> SetCellContents(string name, double number)
        {
            HashSet<string> dependees = new HashSet<string>();
            if (name.isNull() || !name.isValid())//check validity
                throw new InvalidNameException();

            dep.ReplaceDependents(name, new HashSet<string>());//consider replacing with vars
       
            if (dic.ContainsKey(name))//check duplicates
            {
                dic[name].Contents = number;
                dic[name].Value = EvaluateHere(number);//if it already exists override the value and contents
            }
                
            else
                dic.Add(name, new Cell(name, number, EvaluateHere(number)));//add normally
            HashSet<string> set = new HashSet<string>(GetCellsToRecalculate(name));//hashset implement ISet so create a sortedset from dees of name
            return set;
        }
        /// <summary>
        /// If text is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, the contents of the named cell becomes text.  The method returns a
        /// set consisting of name plus the names of all other cells whose value depends, 
        /// directly or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        public override ISet<string> SetCellContents(string name, string text)
        {
            HashSet<string> dependees = new HashSet<string>();
            if (text.isNull())//checks..
                throw new ArgumentNullException();
            if (name.isNull() || (!(name.isValid())))
                throw new InvalidNameException();

            dep.ReplaceDependents(name, new HashSet<string>());//vconsider replacing with vars

            if (dic.ContainsKey(name))//check duplicate and overwrite the value and content if it exists
            {
                dic[name].Contents = text;
                dic[name].Value = EvaluateHere(text);
            }
            else
                dic.Add(name, new Cell(name, text, EvaluateHere(text)));

            HashSet<string> set = new HashSet<string>(GetCellsToRecalculate(name));//sortedset implement ISet so create a sortedset from dents of name
            return set;
            
        }

    
    /// <summary>
    /// If the formula parameter is null, throws an ArgumentNullException.
    /// 
    /// Otherwise, if name is null or invalid, throws an InvalidNameException.
    /// 
    /// Otherwise, if changing the contents of the named cell to be the formula would cause a 
    /// circular dependency, throws a CircularException.  (No change is made to the spreadsheet.)
    /// 
    /// Otherwise, the contents of the named cell becomes formula.  The method returns a
    /// Set consisting of name plus the names of all other cells whose value depends,
    /// directly or indirectly, on the named cell.
    /// 
    /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
    /// set {A1, B1, C1} is returned.
    /// </summary>
    public override ISet<string> SetCellContents(string name, Formula formula)
        {
            if (Object.ReferenceEquals(formula, null))//checking...
                throw new ArgumentNullException();
            if (name.isNull() || !name.isValid())
                throw new InvalidNameException();
            
            IEnumerable<string> origDents = dep.GetDependents(name);
            dep.ReplaceDependents(name, new HashSet<string>());//vconsider replacing with vars
            HashSet<string> dependees = new HashSet<string>();
            
            foreach (string x in formula.GetVariables())
            {
                dep.AddDependency(name, x);
            }
            try
            {
                GetCellsToRecalculate(name);
            }
            catch (CircularException){
                dep.ReplaceDependents(name, origDents);//undo the work i've done since theres circular
                throw;//get outta here
            }
            if (dic.ContainsKey(name))//check for duplicates
            {
                dic[name].Contents = formula;//update content and value
                dic[name].Value = EvaluateHere(formula);
            }
            else
                dic.Add(name, new Cell(name, formula, EvaluateHere(formula)));//if not dupe add that baby in as usual
            foreach (string s in GetCellsToRecalculate(name))
                dic[s].Contents = dic[s].Contents;

            HashSet<string> set = new HashSet<string>(GetCellsToRecalculate(name));//hashset implement ISet so create a sortedset from dees of name
            return set;
        }
        /// <summary>
        /// If name is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name isn't a valid cell name, throws an InvalidNameException.
        /// 
        /// Otherwise, returns an enumeration, without duplicates, of the names of all cells whose
        /// values depend directly on the value of the named cell.  In other words, returns
        /// an enumeration, without duplicates, of the names of all cells that contain
        /// formulas containing name.
        /// 
        /// For example, suppose that
        /// A1 contains 3
        /// B1 contains the formula A1 * A1
        /// C1 contains the formula B1 + A1
        /// D1 contains the formula B1 - C1
        /// The direct dependents of A1 are B1 and C1
        /// </summary>

        protected override IEnumerable<string> GetDirectDependents(string name)
        {
            if (name.isNull())
                throw new ArgumentNullException();//lost code coverage because its a protected method
            if (!(name.isValid()))
                throw new InvalidNameException();

            return dep.GetDependees(name);
        }
        /// <summary>
        /// Evaluates formula/string/double for the value of cell
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public object EvaluateHere(object o)
        {
            if (o is double)
                return o;
            if (o is string)
                return o;
            if (o is Formula)//seperate this so i can test values of cells as 1 (no lookup)
            { 
                Formula toEval = new Formula(o.ToString());

                return toEval.Evaluate(s=>0);//not really needed..but in future will replace this with a lookup function
            }
            return new FormulaError("");//backup
        }
        /// <summary>
        /// Cell class 
        /// </summary>
        private class Cell
        {
            public String Name
            {
                get;
                set;
            }
            public object Contents
            {
                get;
                set;
            }
            public Object Value
            {
                get;
                set;
            }
            /// <summary>
            /// Do object c and v since double and formulaerror can be returned. The more general the better.
            /// </summary>
            /// <param name="n"></param>
            /// <param name="c"></param>
            /// <param name="v"></param>
            public Cell(String n, Object c, Object v)
            {
                Name = n;
                Contents = c;
                Value = v;
            }
        }
      
    }
    /// <summary>
    /// Extension class
    /// </summary>
    public static class Extension
    {
        /// <summary>
        /// Check if null
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool isNull(this string s)
        {
            return (Object.ReferenceEquals(s, null));
        }
        /// <summary>
        /// check if var
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool isValid(this string s)
        {
            /// For example, "x", "_", "x2", "y_15", and "___" are all valid cell  names, but
            /// "25", "2x", and "&" are not.  Cell names are case sensitive, so "x" and "X" are
            /// different cell names.
            return Regex.IsMatch(s, @"^[a-zA-Z_](?: [a-zA-Z_]|\d)*$"); 
        }
    }
}
