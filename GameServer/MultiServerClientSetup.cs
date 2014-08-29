using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    public class MultiServerClientSetup
    {
        private TcpClient Socket;
        private List<MultiServerRoom> Rooms;
        private StreamReader sr;
        private StreamWriter sw;
        private int ConnectionID;

        public MultiServerClientSetup(TcpClient s, List<MultiServerRoom> rooms, int connectionID)
        {
            this.Socket = s;
            this.Rooms = rooms;
            sr = new StreamReader(Socket.GetStream());
            sw = new StreamWriter(Socket.GetStream());
            this.ConnectionID = connectionID;
        }

        public void Start()
        {
            MultiServerRoom room;
            ClientWorker w;
            int i;
            sw.WriteLine("Welcome to the server:");
            sw.WriteLine("");
            sw.WriteLine("Available Games");
            sw.Flush();
            i = 1;
            foreach (MultiServerRoom msr in Rooms)
            {
                sw.WriteLine(string.Format("{0} {1} ({2})", i, msr.Name, msr.AllClientWorkers.Count));
                i++;
            }
            sw.WriteLine(string.Format("{0} <new game>", i));
            //sw.Flush();
            do
            {
                sw.WriteLine("?Select a game to join: ");
                sw.Flush();
                try
                {
                    i = Int32.Parse(sr.ReadLine()) - 1;
                    if (i < 0 || i > Rooms.Count)
                    {
                        i = -1;
                    }

                }
                catch (IOException ex)
                {
                    sw.WriteLine("bye");
                    Console.WriteLine("client disconnected before starting a game: {0}",ex.ToString());
                    return;
                }
                catch (FormatException)
                {
                    i = -1;
                }
                if (i == -1)
                {
                    sw.WriteLine(" > invalid selection");
                }
            } while (i == -1);


            if (i < Rooms.Count)
            {
                room = Rooms[i];
            }
            else
            {
                // Create a new room
                string name = null;
                sw.WriteLine("?Name for this room: ");
                sw.Flush();
                do
                {
                    try
                    {
                        name = sr.ReadLine();

                    }
                    catch (Exception ex)
                    {
                        sw.WriteLine("bye");
                        Console.WriteLine("client disconnected before starting a game: {0}",ex.ToString());
                        return;
                    }
                } while (name == null);
                room = new MultiServerRoom(name);
                Rooms.Add(room);
            }
            sw.WriteLine(string.Format("Now hosting {0} clients in {1}", (room.AllClientWorkers.Count + 1), room.Name));
            sw.Flush();
            sw.WriteLine((room.AllClientWorkers.Count == 0) ? "!is server" : "!not server");
            sw.Flush();
            w = new ClientWorker(Socket, room.AllClientWorkers, ConnectionID);
            room.AllClientWorkers.Add(w);
            w.Start();
        }
    }
}
