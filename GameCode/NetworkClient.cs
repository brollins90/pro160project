using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace GameCode
{
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
