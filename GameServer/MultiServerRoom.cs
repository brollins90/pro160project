using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    public class MultiServerRoom
    {
        public string Name { get; set; }
        public List<ClientWorker> AllClientWorkers { get; set; }

        public MultiServerRoom(string name)
        {
            this.Name = name;
            this.AllClientWorkers = new List<ClientWorker>();
        }
    }
}
