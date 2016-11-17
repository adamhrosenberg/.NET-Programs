using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//Adam Rosenberg
//Yance Mooso
namespace AgCubio
{
    /// <summary>
    /// Cube class. Cubes muts contain the following data:
    /// 
    /// A unique id
    /// The position in space (x,y)
    /// A color
    /// A name -- if this is a player cube
    /// A mass
    /// Food status (is this cube food or a player)
    /// 
    /// Note: it would be useful from an SE point of view to create other properties associated with your cube object, such as: 
    /// Width, Top, Left, Right, Bottom. (As should be apparent, all of these are computed based on the mass and position.
    /// 
    /// Note: the width of a cube is defined by the square root of the mass.
    /// 
    /// loc_x float
    /// loc_y float
    /// argb_color int
    /// uid int
    /// food bool
    /// Name String
    /// Mass Double
    /// 
    /// </summary>
    public class Cube
    {
        ///JSON props:
        [JsonProperty]
        public float loc_x;
        [JsonProperty]
        public float loc_y;
        [JsonProperty]
        public int argb_color;
        [JsonProperty]
        public int uid;
        [JsonProperty]
        public bool food;

        [JsonProperty]
        public String Name { get; set; }
        [JsonProperty]
        public double Mass { get; set; }
        //public int Width { get; set; }
        public int Width;
        public int team_ID { get; set; }

        /// <summary>
        /// Cube constructor that the JSON will use. 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="color"></param>
        /// <param name="_uid"></param>
        /// <param name="isFood"></param>
        /// <param name="_name"></param>
        /// <param name="_mass"></param>
        [JsonConstructor]
        public Cube(float x, float y, int color, int _uid, bool isFood, String _name, double _mass)
        {
            this.loc_x = x;
            this.loc_y = y;
            this.argb_color = color;
            this.food = isFood;
            this.Name = _name;
            this.Mass = _mass;
            this.uid = _uid;
            this.Width = (int)Math.Pow(_mass, .65);
        }
    }
}
