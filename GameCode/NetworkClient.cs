using System;
using System.IO;
using System.Net.Sockets;

namespace GameCode
{
    /// <summary>
    /// Handles the network communication for the game
    /// </summary>
    public class NetworkClient
    {
        private NetworkStream Stream;
        private StreamReader SR;
        private StreamWriter SW;

        public NetworkClient(NetworkStream stream)
        {
            Stream = stream;
            SR = new StreamReader(Stream);
            SW = new StreamWriter(Stream);
        }

        /// <summary>
        /// Write a line to the network
        /// </summary>
        /// <param name="toWrite"></param>
        public void WriteLine(string toWrite)
        {
            try 
            { 
                SW.WriteLine(toWrite);
                SW.Flush();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to write: {0}", ex.Message);
            }
        }

        /// <summary>
        /// Read a line from the network
        /// </summary>
        /// <returns></returns>
        public string ReadLine()
        {
            try
            {
                return SR.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to read: {0}", ex.Message);
                return null;
            }
        }
    }
}
