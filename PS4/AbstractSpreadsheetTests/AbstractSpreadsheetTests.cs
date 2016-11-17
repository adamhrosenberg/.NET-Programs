using SS;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using SpreadsheetUtilities;

namespace GradingTests
{


    /// <summary>
    ///This is a test class for SpreadsheetTest and is intended
    ///to contain all SpreadsheetTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SpreadsheetTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion

        // EMPTY SPREADSHEETS
        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void Test1()
        {
            Spreadsheet s = new Spreadsheet();
            s.GetCellContents(null);
        }

        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void Test2()
        {
            Spreadsheet s = new Spreadsheet();
            s.GetCellContents("1AA");
        }

        [TestMethod()]
        public void Test3()
        {
            Spreadsheet s = new Spreadsheet();
            Assert.AreEqual("", s.GetCellContents("A2"));
        }

        // SETTING CELL TO A DOUBLE
        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void Test4()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetCellContents(null, 1.5);
        }

        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void Test5()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetCellContents("1A1A", 1.5);
        }

        [TestMethod()]
        public void Test6()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetCellContents("Z7", 1.5);
            Assert.AreEqual(1.5, (double)s.GetCellContents("Z7"), 1e-9);
        }

        // SETTING CELL TO A STRING
        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Test7()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetCellContents("A8", (string)null);
        }

        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void Test8()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetCellContents(null, "hello");
        }

        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void Test9()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetCellContents("1AZ", "hello");
        }

        [TestMethod()]
        public void Test10()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetCellContents("Z7", "hello");
            Assert.AreEqual("hello", s.GetCellContents("Z7"));
        }

        // SETTING CELL TO A FORMULA
        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Test11()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetCellContents("A8", (Formula)null);
        }

        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void Test12()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetCellContents(null, new Formula("2"));
        }

        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void Test13()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetCellContents("1AZ", new Formula("2"));
        }

        [TestMethod()]
        public void Test14()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetCellContents("Z7", new Formula("3"));
            Formula f = (Formula)s.GetCellContents("Z7");
            Assert.AreEqual(new Formula("3"), f);
            Assert.AreNotEqual(new Formula("2"), f);
        }

        // CIRCULAR FORMULA DETECTION
        [TestMethod()]
        [ExpectedException(typeof(CircularException))]
        public void Test15()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetCellContents("A1", new Formula("A2"));
            s.SetCellContents("A2", new Formula("A1"));
        }

        [TestMethod()]
        [ExpectedException(typeof(CircularException))]
        public void Test16()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetCellContents("A1", new Formula("A2+A3"));
            s.SetCellContents("A3", new Formula("A4+A5"));
            s.SetCellContents("A5", new Formula("A6+A7"));
            s.SetCellContents("A7", new Formula("A1+A1"));
        }

        [TestMethod()]
        [ExpectedException(typeof(CircularException))]
        public void Test17()
        {
            Spreadsheet s = new Spreadsheet();
            try
            {
                s.SetCellContents("A1", new Formula("A2+A3"));
                s.SetCellContents("A2", 15);
                s.SetCellContents("A3", 30);
                s.SetCellContents("A2", new Formula("A3*A1"));
            }
            catch (CircularException e)
            {
                Assert.AreEqual(15, (double)s.GetCellContents("A2"), 1e-9);
                throw e;
            }
        }

        // NONEMPTY CELLS
        [TestMethod()]
        public void Test18()
        {
            Spreadsheet s = new Spreadsheet();
            Assert.IsFalse(s.GetNamesOfAllNonemptyCells().GetEnumerator().MoveNext());
        }

        [TestMethod()]
        public void Test19()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetCellContents("B1", "");
            Assert.IsFalse(s.GetNamesOfAllNonemptyCells().GetEnumerator().MoveNext());
        }

        [TestMethod()]
        public void Test20()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetCellContents("B1", "hello");
            Assert.IsTrue(new HashSet<string>(s.GetNamesOfAllNonemptyCells()).SetEquals(new HashSet<string>() { "B1" }));
        }

        [TestMethod()]
        public void Test21()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetCellContents("B1", 52.25);
            Assert.IsTrue(new HashSet<string>(s.GetNamesOfAllNonemptyCells()).SetEquals(new HashSet<string>() { "B1" }));
        }

        [TestMethod()]
        public void Test22()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetCellContents("B1", new Formula("3.5"));
            Assert.IsTrue(new HashSet<string>(s.GetNamesOfAllNonemptyCells()).SetEquals(new HashSet<string>() { "B1" }));
        }

        [TestMethod()]
        public void Test23()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetCellContents("A1", 17.2);
            s.SetCellContents("C1", "hello");
            s.SetCellContents("B1", new Formula("3.5"));
            Assert.IsTrue(new HashSet<string>(s.GetNamesOfAllNonemptyCells()).SetEquals(new HashSet<string>() { "A1", "B1", "C1" }));
        }

        // RETURN VALUE OF SET CELL CONTENTS
        [TestMethod()]
        public void Test24()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetCellContents("B1", "hello");
            s.SetCellContents("C1", new Formula("5"));
            Assert.IsTrue(s.SetCellContents("A1", 17.2).SetEquals(new HashSet<string>() { "A1" }));
        }

        [TestMethod()]
        public void Test25()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetCellContents("A1", 17.2);
            s.SetCellContents("C1", new Formula("5"));
            Assert.IsTrue(s.SetCellContents("B1", "hello").SetEquals(new HashSet<string>() { "B1" }));
        }

        [TestMethod()]
        public void Test26()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetCellContents("A1", 17.2);
            s.SetCellContents("B1", "hello");
            Assert.IsTrue(s.SetCellContents("C1", new Formula("5")).SetEquals(new HashSet<string>() { "C1" }));
        }

        [TestMethod()]
        public void Test27()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetCellContents("A1", new Formula("A2+A3"));
            s.SetCellContents("A2", 6);
            s.SetCellContents("A3", new Formula("A2+A4"));
            s.SetCellContents("A4", new Formula("A2+A5"));
            Assert.IsTrue(s.SetCellContents("A5", 82.5).SetEquals(new HashSet<string>() { "A5", "A4", "A3", "A1" }));
        }

        // CHANGING CELLS
        [TestMethod()]
        public void Test28()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetCellContents("A1", new Formula("A2+A3"));
            s.SetCellContents("A1", 2.5);
            Assert.AreEqual(2.5, (double)s.GetCellContents("A1"), 1e-9);
        }

        [TestMethod()]
        public void Test29()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetCellContents("A1", new Formula("A2+A3"));
            s.SetCellContents("A1", "Hello");
            Assert.AreEqual("Hello", (string)s.GetCellContents("A1"));
        }

        [TestMethod()]
        public void Test30()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetCellContents("A1", "Hello");
            s.SetCellContents("A1", new Formula("23"));
            Assert.AreEqual(new Formula("23"), (Formula)s.GetCellContents("A1"));
            Assert.AreNotEqual(new Formula("24"), (Formula)s.GetCellContents("A1"));
        }

        // STRESS TESTS
        [TestMethod()]
        public void Test31()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetCellContents("A1", new Formula("B1+B2"));
            s.SetCellContents("B1", new Formula("C1-C2"));
            s.SetCellContents("B2", new Formula("C3*C4"));
            s.SetCellContents("C1", new Formula("D1*D2"));
            s.SetCellContents("C2", new Formula("D3*D4"));
            s.SetCellContents("C3", new Formula("D5*D6"));
            s.SetCellContents("C4", new Formula("D7*D8"));
            s.SetCellContents("D1", new Formula("E1"));
            s.SetCellContents("D2", new Formula("E1"));
            s.SetCellContents("D3", new Formula("E1"));
            s.SetCellContents("D4", new Formula("E1"));
            s.SetCellContents("D5", new Formula("E1"));
            s.SetCellContents("D6", new Formula("E1"));
            s.SetCellContents("D7", new Formula("E1"));
            s.SetCellContents("D8", new Formula("E1"));
            ISet<String> cells = s.SetCellContents("E1", 0);
            Assert.IsTrue(new HashSet<string>() { "A1", "B1", "B2", "C1", "C2", "C3", "C4", "D1", "D2", "D3", "D4", "D5", "D6", "D7", "D8", "E1" }.SetEquals(cells));
        }
        [TestMethod()]
        public void Test32()
        {
            Test31();
        }
        [TestMethod()]
        public void Test33()
        {
            Test31();
        }
        [TestMethod()]
        public void Test34()
        {
            Test31();
        }

        [TestMethod()]
        public void Test35()
        {
            Spreadsheet s = new Spreadsheet();
            ISet<String> cells = new HashSet<string>();
            for (int i = 1; i < 200; i++)
            {
                cells.Add("A" + i);
                Assert.IsTrue(cells.SetEquals(s.SetCellContents("A" + i, new Formula("A" + (i + 1)))));
            }
        }
        [TestMethod()]
        public void Test36()
        {
            Test35();
        }
        [TestMethod()]
        public void Test37()
        {
            Test35();
        }
        [TestMethod()]
        public void Test38()
        {
            Test35();
        }
        [TestMethod()]
        public void Test39()
        {
            Spreadsheet s = new Spreadsheet();
            for (int i = 1; i < 200; i++)
            {
                s.SetCellContents("A" + i, new Formula("A" + (i + 1)));
            }
            try
            {
                s.SetCellContents("A150", new Formula("A50"));
                Assert.Fail();
            }
            catch (CircularException)
            {
            }
        }
        [TestMethod()]
        public void Test40()
        {
            Test39();
        }
        [TestMethod()]
        public void Test41()
        {
            Test39();
        }
        [TestMethod()]
        public void Test42()
        {
            Test39();
        }

        [TestMethod()]
        public void Test43()
        {
            Spreadsheet s = new Spreadsheet();
            for (int i = 0; i < 500; i++)
            {
                s.SetCellContents("A1" + i, new Formula("A1" + (i + 1)));
            }
            HashSet<string> firstCells = new HashSet<string>();
            HashSet<string> lastCells = new HashSet<string>();
            for (int i = 0; i < 250; i++)
            {
                firstCells.Add("A1" + i);
                lastCells.Add("A1" + (i + 250));
            }
            Assert.IsTrue(s.SetCellContents("A1249", 25.0).SetEquals(firstCells));
            Assert.IsTrue(s.SetCellContents("A1499", 0).SetEquals(lastCells));//this is failing
        }
        [TestMethod()]
        public void Test44()
        {
            Test43();
        }
        [TestMethod()]
        public void Test45()
        {
            Test43();
        }
        [TestMethod()]
        public void Test46()
        {
            Test43();
        }

        [TestMethod()]
        public void Test47()
        {
            RunRandomizedTest(47, 2519);
        }
        [TestMethod()]
        public void Test48()
        {
            RunRandomizedTest(48, 2521);
        }
        [TestMethod()]
        public void Test49()
        {
            RunRandomizedTest(49, 2526);
        }
        [TestMethod()]
        public void Test50()
        {
            RunRandomizedTest(50, 2521);
        }

        public void RunRandomizedTest(int seed, int size)
        {
            Spreadsheet s = new Spreadsheet();
            Random rand = new Random(seed);
            for (int i = 0; i < 10000; i++)
            {
                try
                {
                    switch (rand.Next(3))
                    {
                        case 0:
                            s.SetCellContents(randomName(rand), 3.14);
                            break;
                        case 1:
                            s.SetCellContents(randomName(rand), "hello");
                            break;
                        case 2:
                            s.SetCellContents(randomName(rand), randomFormula(rand));
                            break;
                    }
                }
                catch (CircularException)
                {
                }
            }
            ISet<string> set = new HashSet<string>(s.GetNamesOfAllNonemptyCells());
            Assert.AreEqual(size, set.Count);
        }

        private String randomName(Random rand)
        {
            return "ABCDEFGHIJKLMNOPQRSTUVWXYZ".Substring(rand.Next(26), 1) + (rand.Next(99) + 1);
        }

        private String randomFormula(Random rand)
        {
            String f = randomName(rand);
            for (int i = 0; i < 10; i++)
            {
                switch (rand.Next(4))
                {
                    case 0:
                        f += "+";
                        break;
                    case 1:
                        f += "-";
                        break;
                    case 2:
                        f += "*";
                        break;
                    case 3:
                        f += "/";
                        break;
                }
                switch (rand.Next(2))
                {
                    case 0:
                        f += 7.2;
                        break;
                    case 1:
                        f += randomName(rand);
                        break;
                }
            }
            return f;
        }

    }
}

/**using Microsoft.VisualStudio.TestTools.UnitTesting;
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
}**/
