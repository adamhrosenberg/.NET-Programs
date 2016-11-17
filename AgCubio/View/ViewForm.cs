using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AgCubio;
using System.Net.Sockets;
using NetworkController;
using System.Diagnostics;

//Adam Rosenberg
//Yance Mooso



/// <summary>
/// GUI View for the client end!
/// </summary>
namespace AgCubio
{
    public partial class ViewForm : Form
    {
        public World world;
        public Socket socket;
        private System.Drawing.SolidBrush brush;
        private int uid;
        bool offView = false;
        bool isDead = false;
        private int xPos;
        private int yPos;
        Cube mainCube; //player cube
        private int xOff, yOff, cubeWidth, mainCubeScale, mainCubeWidth;
        private int teamID;
        private int foodEaten;

        /// <summary>
        /// ViewForm initizalizes our component and the world.
        /// </summary>
        public ViewForm()
        {
            InitializeComponent();
            this.world = new World();
            this.DoubleBuffered = true;
        }
        /// <summary>
        /// This method is called when the connect button is clicked. It hides the itinitial GUI and connects.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void connectButton_Click(object sender, EventArgs e)
        {
            nameBox.Visible = false;
            nameLabel.Visible = false;
            serverBox.Visible = false;
            serverLabel.Visible = false;
            connectButton.Visible = false;
            connectButton.Enabled = false;
            titleLabel.Visible = false;
            offView = true;
            //use our socket to connect to server. pass in the coonectDel as an action (no return 1 param) and the name of the server from the text box.
            //include an if or try here in case the serverBox.Text == string.empty.
            this.socket = Controller.Connect_to_Server(new Action<State>(this.connectDel), this.serverBox.Text);
            this.Refresh();
        }

        /// <summary>
        /// send player location from x,y of mouse to server..We use the 'maincube' as the player cube to attempt to 
        /// draw our food around the player since the player cube is only in the center of the screen
        /// </summary>
        private void sendPos()
        {
            Cube cube;
            if (!this.world.dict.TryGetValue(uid, out cube))
                return;
            //check if the maincube has been put in yet. We've given this a try and it seems to help a little
            //with where the mouse 'centers' on the cube.
            if (mainCube != null)
            {
                xPos = this.PointToClient(Control.MousePosition).X + ((int)mainCube.Width);
                yPos = this.PointToClient(Control.MousePosition).Y + ((int)mainCube.Width);
                Controller.Send(this.socket, new Tuple<string, int, int>("move", xPos, yPos) + "\n");
            }
            else
            {
                xPos = this.PointToClient(Control.MousePosition).X;
                yPos = this.PointToClient(Control.MousePosition).Y;
                Controller.Send(this.socket, new Tuple<string, int, int>("move", xPos, yPos) + "\n");
            }
        }

        /// <summary>
        /// 'helper' method for the connect so it will move into here once the state has been created etc. 
        /// </summary>
        /// <param name="state"></param>
        private void connectDel(State state)
        {
            if (state.uhoh)
            {
                //error occured..couldn't update an error label because of thread collision. even after locking
                //this all it still gave me the error
            }
            else
            {
                state.action = new Action<State>(this.deSerializer);//move onto deserializing the player cube.
                Controller.Send(this.socket, this.nameBox.Text + "\n"); //send the name of the player over our socket
            }
        }
        /// <summary>
        /// Processes the player cube 
        /// </summary>
        /// <param name="state"></param>
        private void deSerializer(State state)
        {
            //beinging in a string builder from our state
            StringBuilder sb = state.sb;
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.MissingMemberHandling = MissingMemberHandling.Error;
            try
            {
                //split returns a string array. we want the 0th element of the array. 
                //Uses new line since the server uses that to seperate cubes in JSON format.
                string data = sb.ToString();
                string[] jsonArray = data.Split('\n');
                string str = jsonArray[0];
                //deserialize cube from the string we just ocnverted from the Stringbuilder. 
                Cube cube = JsonConvert.DeserializeObject<Cube>(str, settings);
                lock (this.world) //prevent thread interference
                {
                    this.world.dict[cube.uid] = cube; //add player to dictionary.
                    this.uid = cube.uid; //keep track of this players uid
                    this.teamID = cube.team_ID; //keep track of this players team uid
                }
            }
            catch (Exception e)
            {
                //System.Diagnostics.Debug.WriteLine(e.Message);
            }
            state.action = new Action<State>(this.checkData);
            //move onto checking the data..
            this.checkData(state);
        }

        /// <summary>
        /// Checks the data from sever.
        /// </summary>
        /// <param name="state"></param>
        private void checkData(State state)
        {
            lock (this.world)
            {
                StringBuilder sb = state.sb;//bring in the stringbuilder and create an array of JSON strings.
                JsonSerializerSettings settings = new JsonSerializerSettings();
                settings.MissingMemberHandling = MissingMemberHandling.Error;
                char[] newLine = new char[1] { '\n' };
                string[] jsonArray = sb.ToString().Split(newLine, StringSplitOptions.RemoveEmptyEntries);
                int count = 0; //number of times we go through this loop. Used to append missing data.
                try
                {
                    foreach (string line in jsonArray)
                    {
                        if (line.StartsWith("{") && line.EndsWith("}"))//make sure we're only using full lines
                        {
                            Cube cube = JsonConvert.DeserializeObject<Cube>(line, settings);//crate the cube
                            if (cube != null)
                            {
                                count++;
                                if (cube.uid == this.uid && cube.Mass == 0)//if the player dies
                                {
                                    this.world.dict.Remove(cube.uid); //remove from dict - will no longer be drawn
                                    isDead = true;
                                }
                                if (cube.Mass == 0)//if another cube is dead/eaten
                                {
                                    this.world.dict.Remove(cube.uid);
                                    if (cube.food)
                                    {
                                        world.foodCount--;
                                        foodEaten++; //show how much food has been eaten for end statistics.
                                    }
                                    else
                                        world.playerCount--;
                                }
                                else
                                    this.world.dict[cube.uid] = cube;//override the cube with the current info about it (location etc)
                                if (cube.food)
                                    world.foodCount++;
                            }
                        }
                        else
                            break;//if it doesn't start and end with { } then break out.
                    }
                }
                catch (Exception e)
                {
                    //System.Diagnostics.Debug.WriteLine(e.Message);
                }
                sb.Clear();
                if (count > 0)
                {
                    this.Invalidate();
                }
                if (count != jsonArray.Length)
                {
                    sb.Append(jsonArray[jsonArray.Length - 1]);
                }
            }

                Controller.i_want_more_data(state);
        }

        private void reconnectButton_Click(object sender, EventArgs e)
        {
            offView = false; //stop the painting of cubes.
            isDeadPanel.Visible = false; //hide the panel
            offView = true;//allow to paint to keep going
            //reconnect
            this.socket = Controller.Connect_to_Server(new Action<State>(this.connectDel), this.serverBox.Text);
            this.Refresh();
        }

        /// <summary>
        /// Closes the form if player selects close
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void closeButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Paint method. This is where we draw all the cubes we have in the world's dictionary.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ViewForm_Paint(object sender, PaintEventArgs e)
        {
            lock (this.world)//lock into the world
            {
                if (offView)//only do this if the player has pressed connect
                {
                    try
                    {
                        this.sendPos();//send mouse position to server.
                        fCountLabel.Text = world.foodCount.ToString();
                        
                        e.Graphics.Clear(Color.Gray); //set background to gray because it looks so much better.
                        this.Update(); //update 

                        foreach (KeyValuePair<int, Cube> cube in this.world.dict)
                        {
                            if (cube.Value != null)
                            {
                                if (this.uid == cube.Value.uid)
                                {
                                    mainCube = world.dict[uid]; //this is the player cube that is in the center of the screen!
                                    massLabel.Text = ((int)mainCube.Mass).ToString();
                                }
                                //set color and brush based on the argb color of the cube that was sent from the server
                                Color color = Color.FromArgb(cube.Value.argb_color);
                                brush = new System.Drawing.SolidBrush(color);
                                //Set the name string color to yellow and set font
                                Brush playerString = new SolidBrush(Color.Yellow);
                                Font font = new Font("Console", 12);
                                //make sure we have the correct width. ran into problem using cube.width, it draws in the incorrect place
                                cubeWidth = (int)Math.Pow(cube.Value.Mass, .65)*(int)2f;
                                mainCubeWidth = (int)Math.Pow(mainCube.Mass, .65) * (int)2f;
                                mainCubeScale = (int)Math.Pow(cubeWidth, 2); //player cube width
                                //create the offset
                                xOff = ((int)(cube.Value.loc_x - mainCube.loc_x)* (int)2f) + ((this.Width - cubeWidth) / 2);
                                yOff = ((int)(cube.Value.loc_y - mainCube.loc_y) * (int)2f) + ((this.Height - cubeWidth) / 2);
                                //if we're on the player cube
                                if (cube.Value.uid == this.uid)
                                {
                                    Rectangle rect = new Rectangle(xOff, yOff, cubeWidth, cubeWidth);
                                    e.Graphics.FillRectangle(brush, rect);
                                    e.Graphics.DrawString(cube.Value.Name.ToString(), font, playerString, this.Width / 2, this.Width / 2);
                                    brush.Dispose();
                                }
                                if (cube.Value.food)//if the cube is food
                                {
                                    e.Graphics.FillRectangle(brush, xOff, yOff, 10f, 10f);//attempting to scale food..
                                    brush.Dispose();
                                }
                                if (!cube.Value.food && cube.Value.uid != this.uid)//if they're other players.
                                {
                                    e.Graphics.FillRectangle(brush, xOff, yOff, cubeWidth, cubeWidth);
                                    e.Graphics.DrawString(cube.Value.Name.ToString(), font, playerString, xOff, yOff);
                                    brush.Dispose();
                                }
                                if (isDead)//if youre dead display the deadPanel
                                {
                                    offView = false;
                                    isDead = false;
                                    //System.Diagnostics.Debug.WriteLine("you are dead");
                                    isDeadPanel.Visible = true;
                                    isDeadPanel.Width = 985;
                                    isDeadPanel.Height = 985;
                                    isDeadPanel.Location = new Point(0, 0);
                                    isDeadPanel.BackgroundImage = Image.FromFile("....\\....\\....\\resources\\trump.jpg");
                                    foodEatenLabel.Text = foodEaten.ToString();
                                    foodEaten = 0;
                                    this.Invalidate();
                                }
                            }
                        }
                        this.Invalidate();
                    }
                    catch (Exception ec)
                    {
                        System.Diagnostics.Debug.WriteLine(ec.Message);
                    }
                }
            }
        }

        /// <summary>
        /// Set key binding shortcuts..Only used really for the split
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="keyData"></param>
        /// <returns></returns>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
           
            if (offView)//not in the initial GUI
            {
                try
                {
                    if (keyData == (Keys.Space))
                    {
                        xPos = this.PointToClient(Control.MousePosition).X;
                        yPos = this.PointToClient(Control.MousePosition).Y;
                        Controller.Send(this.socket, "(split, " + xPos + ", " + yPos + ")\n");
                        //sending split message to server.
                    }
                }
                catch (Exception e)
                {
                    //exc
                }
                return true;
            }
            return false; //not not pressing space
        }
    }
}
