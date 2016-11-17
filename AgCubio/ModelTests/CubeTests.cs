using Microsoft.VisualStudio.TestTools.UnitTesting;
using AgCubio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//Adam Rosenberg
//Yance Mooso
namespace AgCubio.Tests
{
    [TestClass()]
    public class CubeTests
    {
        [TestMethod()]
        public void CubeTest()
        {
            Cube cube = new Cube(5,5,5, 5, false, "player 1", 5.0);
            Assert.AreEqual(5.0, cube.loc_x);
            Assert.AreEqual(5.0, cube.loc_y);
            Assert.AreEqual(5.0, cube.argb_color);
            Assert.AreEqual(5.0, cube.uid);
            Assert.AreEqual(5.0, cube.Mass);
            Assert.AreEqual("player 1",  cube.Name);
        }
        [TestMethod()]
        public void WorldTest()
        {
            World world = new World();
            Cube cube = new Cube(5, 5, 5, 5, false, "player 1", 5.0);
            world.dict[cube.uid] = cube; //add cube to world
            Assert.AreEqual(cube, world.dict[cube.uid]);
        }
    }
}