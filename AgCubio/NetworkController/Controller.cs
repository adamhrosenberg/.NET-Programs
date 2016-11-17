using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
//Adam Rosenberg
//Yance Mooso

namespace NetworkController
{
    public static class Controller
    {
        public const int PORT = 11000;

        /// <summary>
        /// hostname - the name of the server to connect to
        /// callback function - a function inside the View to be called when a connection is made
        ///This function should attempt to connect to the server via a provided hostname.It should 
        /// save the callback function (in a state object) for use when data arrives.
        ///It will need to open a socket and then use the BeginConnect method. Note this method take the "state" 
        /// object and "regurgitates" it back to you when a connection is made, thus allowing "communication" between this
        ///  function and the Connected_to_Server function.
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="hostname"></param>
        /// <returns></returns>
        public static Socket Connect_to_Server(Action<State> callback, String hostname)
        {
            try
            {
                try
                {
                    IPAddress address;
                    try
                    {
                        address = Dns.GetHostEntry(hostname).AddressList[0];
                    }
                    catch (Exception)
                    {
                        address = IPAddress.Parse(hostname);
                    }
                    IPEndPoint ipeServer = new IPEndPoint(address, PORT);
                    Socket sck = new Socket(address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                    //reference: http://stackoverflow.com/questions/22116873/set-socket-option-is-why-so-important-for-a-socket-ip-hdrincl-in-icmp-request
                    sck.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.IPv6Only, false);
                    State state = new State();
                    state.socket = sck;
                    state.action = callback;
                    sck.BeginConnect((EndPoint)ipeServer, new AsyncCallback(Controller.Connected_to_Server), state);
                    return sck;
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            return null as Socket;
        }

        /// <summary>
        /// This function is reference by the BeginConnect method above and is "called" by the OS when the socket connects to the server. 
        /// The "state_in_an_ar_object" object contains a field "AsyncState" which contains the "state" object saved away in the above function. 
        /// Once a connection is established the "saved away" callback function needs to called. 
        /// Additionally, the network connection should"BeginReceive" expecting more data to arrive (and provide the ReceiveCallback function for this purpose)
        /// </summary>
        /// <param name="state_in_an_ar_object"></param>
        public static void Connected_to_Server(IAsyncResult state_in_an_ar_object)
        {
            State state = (State)state_in_an_ar_object.AsyncState;
            try
            {
                state.socket.EndConnect(state_in_an_ar_object);
                state.action(state);
                Controller.Connect_Helper(state);
            }
            catch (Exception ex)
            {
                state.uhoh = true;
                state.action(state);
                Console.WriteLine(ex.ToString());
            }
        }
        private static void Connect_Helper(State state)
        {
            try
            {
                state.socket.BeginReceive(state.buffer, 0, 1024, SocketFlags.None, new AsyncCallback(Controller.ReceiveCallback), (object)state);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// The ReceiveCallback method is called by the OS when new data arrives. This method should check to see how much data has arrived. 
        /// If 0, the connection has been closed (presumably by the server). 
        /// On greater than zero data, this method should call the callback function provided above. For our purposes, this function should not request more data. 
        /// It is up to the code in the callback function above to request more data.
        /// </summary>
        /// <param name="state_in_ar_object"></param>
        public static void ReceiveCallback(IAsyncResult state_in_ar_object)
        {
            State state = (State)state_in_ar_object.AsyncState;
            //https://msdn.microsoft.com/en-us/library/w7wtt64b(v=vs.110).aspx
            int dataBytes = state.socket.EndReceive(state_in_ar_object);
            if (dataBytes > 0)
            {
                //https://social.msdn.microsoft.com/Forums/sqlserver/en-US/0b712696-8f3a-4838-9a3b-51f3687d088a/how-to-receive-binary-data-using-tcpip-asynchronous-socket-programming?forum=wcf
                //We dont have a totalBytes read member for state or anything, but if we did we could use the conditional from ^^^ which may be useful. But this shuold get it all.
                state.sb.Append(Encoding.UTF8.GetString(state.buffer, 0, dataBytes));
                state.action(state);
            }
            else
            {
                return;
            }
        }

        /// <summary>
        /// This is a small helper function that the client 
        /// View code will call whenever it wants more data. 
        /// Note: the client will probably want more data every time it gets data.
        /// </summary>
        /// <param name="state"></param>
        public static void i_want_more_data(State state)
        {
            state.socket.BeginReceive(state.buffer, 0, 1024, SocketFlags.None, new AsyncCallback(Controller.ReceiveCallback), (object)state);
        }

        /// <summary>
        /// This function (along with it's helper 'SendCallback') will allow a program to send data over a socket. 
        /// This function needs to convert the data into bytes and then send them using socket.BeginSend.
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="data"></param>
        public static bool Send(Socket socket, String data)
        {
            try
            {
            byte[] bytes = Encoding.UTF8.GetBytes(data);
            socket.BeginSend(bytes, 0, bytes.Length, SocketFlags.None, new AsyncCallback(Controller.SendCallBack), (object)socket);
            return true;
            }
            catch(SocketException se)
            {
                socket.Shutdown(SocketShutdown.Both);//exit gracefully
                socket.Close();
            }
            return false;
        }

        /// <summary>
        /// This function "assists" the Send function. If all the data has been sent, then life is good and nothing needs to be done 
        /// (note: you may, when first prototyping your program, put a WriteLine in here to see when data goes out). 
        /// If there is more data to send, the SendCallBack needs to arrange to send this data(see the ChatClient example program).
        /// </summary>
        /// <param name="state_in_ar_object"></param>
        /// <returns></returns>
        public static void SendCallBack(IAsyncResult state_in_ar_object)
        {
            ((Socket)state_in_ar_object.AsyncState).EndSend(state_in_ar_object);
        }
    }
    public class State
    {
        // Client socket.
        public Socket socket = null;
        // Size of receive buffer.
        public const int BufferSize = 1024;
        // Receive buffer.
        public byte[] buffer = new byte[BufferSize];
        // Received data string.
        public StringBuilder sb = new StringBuilder();
        public long uid;
        public Action<State> action;
        public bool uhoh;
    }
}
