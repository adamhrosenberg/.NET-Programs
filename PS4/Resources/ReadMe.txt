
Your name and the date associated with your comments
your initial design thoughts about the project (how you are going to set things up/code the project)
a comment on which versions of the PS2/PS3 libraries you are building against
Any notes you want the graders to be aware of when evaluating your work
The graders will look to make sure you have something interesting to say.

Adam Rosenberg 
u0751643
PS4


10/5 I figured out what was wrong, the test cs file was excluded from the commit. I included it, it's there now. Will talk to you today in lab Matt.

So after comitting for the last time I drove home, checked github and saw that for some reason my unittest.cs never synced with github at all
Just got back to school it's 1130. I'm going to copy-paste my tests into this readme just in case I can't get it synced before midnight. If I loose
points I understand I just wanted to make sure that you know I had these tests done before midnight. I have no idea what's causing tihs problem
on github it has never happened to me before.

I use new versions of ps3 library. (The new dlls). I used the ps3 grading tets
to check my ps3 cs file and check it against the tests. Then I fixed it. This included 
fixing getvariables, my == method, and the regex expressions. 

I used a value part of my cell as well. The value is basically the evaluate of the contents

The problems I ran into was not having a lookup function (I just use lamda notation: s=>1). 
So if I have a cell, B2, the refers to A2, the value of B2 is always 1 because I can't lookup that cell value

But I'm sure we'll get to that next week.

I was only able to get 90% coverage because I had a lot of checks I couldn't get to (they're basically backups)
I added those checks because it was in the assignment comments. But all my actual code is covered.

Thanks!


Adam


using Microsoft.VisualStudio.TestTools.UnitTesting;
using SS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpreadsheetUtilities;
// Adam Rosenberg
// u0751643
// PS4
namespace SS.Tests
{
    [TestClass()]
    public class AbstractSpreadsheetTests
    {
        public Spreadsheet x;
        /// <summary>
        /// Initializer for tester..
        /// </summary>
        [TestInitialize]
        public void setup()
        {
            x = new Spreadsheet();
        }
        /// <summary>
        /// Check value for something not in there
        /// </summary>
        [TestMethod()]
        public void GetCellValueofNotINcluded()
        {
            x.SetCellContents("A", 5.0);
            Assert.AreEqual(string.Empty, x.GetCellValue("B"));
            Assert.AreEqual(string.Empty, x.GetCellContents("B"));
        }
        /// <summary>
        /// Check cell contents for something that doesn't exist..
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void GetCellContentsNull()
        {
            Assert.AreEqual(string.Empty, x.GetCellContents(""));
        }
        /// <summary>
        /// Get value for something that doesn't exist..
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void GetCellValueNull()
        {
            Assert.AreEqual(string.Empty, x.GetCellValue(""));
        }
        /// <summary>
        /// Check contents.
        /// </summary>
        [TestMethod()]
        public void GetCellContentsTestString()
        {
            x.SetCellContents("X", "1.2");
            Assert.AreEqual("1.2", x.GetCellContents("X"));
        }
        /// <summary>
        /// Testing double set cell and eval
        /// </summary>
        [TestMethod()]
        public void CheckSetCellContentsDouble()
        {
            x.SetCellContents("A", 1.2);
            
            Assert.AreEqual(1.2, x.GetCellValue("A"));
        }
        /// <summary>
        /// testing formula set cell and eval (eval is called when setcell is used)
        /// </summary>
        [TestMethod()]
        public void CheckSetCellContentsFormula()
        {
            Formula f = new Formula("5.0+3.0");
            x.SetCellContents("A", f);
            Assert.AreEqual(8.0, x.GetCellValue("A"));
        }
        /// <summary>
        /// testing string set cell and eval (eval is called when setcell is used)
        /// </summary>
        [TestMethod()]
        public void CheckGetCellValueString()
        {
            x.SetCellContents("A", "5");
            Assert.AreEqual(5.0, x.GetCellValue("A"));
        }
        /// <summary>
        /// x1 has value 1 from spreadsheet class, should be 3
        /// </summary>
        [TestMethod()]
        public void checkEvaluator4()
        {
            Formula f = new Formula("x+3");
            x.SetCellContents("A", f);
            Assert.AreEqual(4.0, x.GetCellValue("A"));
        }
        /// <summary>
        /// Check getnamesofallnonemptycells..
        /// </summary>
        [TestMethod()]
        public void ValidSetCellContentsTest1()
        {
            x.SetCellContents("X", "1.2");
            List<string> test = new List<string>(x.GetNamesOfAllNonemptyCells());
            Assert.AreEqual("X", test[0]);
        }

        /// <summary>
        /// test invalid name
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void InvalidSetCellContentsTest3()
        {
            x.SetCellContents("(&*%", 5.0);
        }
        /// <summary>
        /// Test more invalid names 
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void InvalidSetCellContentsTest4()
        {
            x.SetCellContents("a1_", 5.0);
        }
        /// <summary>
        /// Test depending cells
        /// </summary>
        [TestMethod()]
        public void TestReturnofSetCellContents() 
        { 
            //TODO gotta fix underscore

            HashSet<string> checkAgainst = new HashSet<string>();
            Formula z = new Formula("A1+B1");
            checkAgainst.Add("A1");
            checkAgainst.Add("B1");
            x.SetCellContents("A1",  1);//set =1 because we have no current lookup function in evaluate just s=>1
            x.SetCellContents("B1", 1);
            Assert.AreEqual(checkAgainst.ElementAt(1).ToString(), x.SetCellContents("A2", z).ElementAt(1).ToString());
            Assert.AreEqual(2.0, x.GetCellValue("A2"));
        }
        /// <summary>
        /// 
        /// </summary>
        [TestMethod()]
        public void TestDependingCells()
        {
            HashSet<string> checkAgainst = new HashSet<string>();
            Formula b1 = new Formula("A1+A1");
            Formula c1 = new Formula("B1+A1");
            Formula d1= new Formula("B1-C1");
            checkAgainst.Add("A1");
            checkAgainst.Add("B1");
            //x.SetCellContents("A1", 1);//set =1 because we have no current lookup function in evaluate just s=>1
            x.SetCellContents("B1", b1); //B1=2
            x.SetCellContents("C1", c1); //C1=2
            x.SetCellContents("D1", d1); //D1=0
            //Assert.AreEqual(1.0, x.GetCellValue("A1"));
            //Assert.AreEqual(0.0, x.GetCellValue("D1"));
            Assert.AreEqual(checkAgainst.ElementAt(0).ToString(), x.SetCellContents("A1", 1).ElementAt(0).ToString());//offset by one because also add the name of the cell.
        }

        /// <summary>
        /// Throws circular exception
        /// </summary>
        [ExpectedException(typeof(CircularException))]
        [TestMethod()]
        public void TestCircular()
        {
            Formula a1 = new Formula("B1+B1");
            Formula b1 = new Formula("A1+A1");
            x.SetCellContents("A1", a1); 
            x.SetCellContents("B1", b1); 
        }  

        /// <summary>
        /// Throw null exception
        /// </summary>
        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod()]
        public void SetCellwithEmptyString()
        {
            x.SetCellContents("a1", string.Empty);
        }
        /// <summary>
        /// Another exception throw
        /// </summary>
        [ExpectedException(typeof(InvalidNameException))]
        [TestMethod()]
        public void SetCellwithEmptyDouble()
        {
            x.SetCellContents(string.Empty, 5);
        }
        /// <summary>
        /// Test overriding cells
        /// </summary>
        [TestMethod()]
        public void TestDupes()
        {
            //Check for double
            x.SetCellContents("A1", 5);
            x.SetCellContents("B1", 3);
            x.SetCellContents("A1", 2);
            Assert.AreEqual(2.0, x.GetCellValue("A1"));
            Assert.AreEqual(2.0, x.GetCellContents("A1"));
            //Check for string
            x.SetCellContents("A1", "5");
            x.SetCellContents("B1", "3");
            x.SetCellContents("A1", "2");
            Assert.AreEqual(2.0, x.GetCellValue("A1"));
            Assert.AreEqual("2", x.GetCellContents("A1"));
            //Check for formula
            Formula a = new Formula("5+2");
            Formula b = new Formula("5");
            x.SetCellContents("A1", a);
            x.SetCellContents("B1", b);
            x.SetCellContents("A1", new Formula("100/25"));
            Assert.AreEqual(4.0, x.GetCellValue("A1"));
            Assert.AreEqual(new Formula("100/25"), x.GetCellContents("A1"));
        }
 
        /// <summary>
        /// Test throwing invlaidnameexception
        /// </summary>
        [ExpectedException(typeof(InvalidNameException))]
        [TestMethod()]
        public void TestEmptyNameDouble()
        {
            x.SetCellContents("", 5);
        }
        /// <summary>
        /// Test invalidnameexception
        /// </summary>
        [ExpectedException(typeof(InvalidNameException))]
        [TestMethod()]
        public void TestEmptyNameString()
        {
            x.SetCellContents(null, "5");
        }
        /// <summary>
        /// More testing exceptions..Sorry I haven't been able to condense multiple tests like this into one
        /// </summary>
        [ExpectedException(typeof(InvalidNameException))]
        [TestMethod()]
        public void TestEmptyNameFormula()
        {
            x.SetCellContents("", new Formula("5"));
        }
    }
}
