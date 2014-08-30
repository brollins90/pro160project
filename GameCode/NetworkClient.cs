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
        private TcpClient Client;
        private StreamReader SR;
        private StreamWriter SW;

        public NetworkClient(TcpClient client)
        {
            Client = client;
            SR = new StreamReader(Client.GetStream());
            SW = new StreamWriter(Client.GetStream());
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
            return SR.ReadLine();
        }
    }
}
