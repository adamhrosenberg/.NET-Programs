using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
//Adam Rosenberg
//Yance Mooso

namespace AgCubio
{
    public class World
    {
        public int playerCount;
        public int foodCount;
        public Dictionary<int, Cube> dict;
        public readonly int Width;
        public readonly int Height;
        public World()
        {
            dict = new Dictionary<int, Cube>();
        }

        /// <summary>
        /// Remove the cube and update the counts for the labels
        /// </summary>
        /// <param name="cube"></param>
        public void Remove(Cube cube)
        {
            dict.Remove((int)cube.uid);
            if (cube.food)
                foodCount--;
            else
                playerCount--;
        }
    }
}
