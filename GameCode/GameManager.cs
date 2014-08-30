using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Threading;
using System.Threading.Tasks;
using GameCode.Models;
using System.Windows;
using GameCode.Helpers;
using GameCode.Models.Projectiles;
using System.Net.Sockets;
using System.IO;
using System.Threading;

namespace GameCode
{
    public class GameManager
    {
        private GameWorld _World;
        public GameWorld World
        {
            get { return _World; }
            set { _World = value; }
        }
        //public string Name;        
        private NetworkClient NetClient;
        private bool IsServer;
        private UpdateThread UT;
        private GameListener LT;

        public GameManager(bool isServer, NetworkClient netClient, InputListener gl, int classChosen)
        {
            Console.WriteLine("{0} GameManager - Create", System.Threading.Thread.CurrentThread.ManagedThreadId);

            IsServer = isServer;
            NetClient = netClient;
            World = new GameWorld(); 

            UT = new UpdateThread(this, IsServer, gl, classChosen);

            LT = new GameListener(NetClient, this);
            new Thread(LT.Start).Start();

            if (IsServer)
            {
                LoadWorld();
            }
            else
            {
                SendInfo(MessageBuilder.RequestAllMessage());
            }
            //new Thread(UT.Start).Start();
            UT.Start();

        }


        public void AddObject(GameObject o)
        {
            AddObjectThreadSafe(o);
        }

        private delegate void AddObjectThreadSafeDelegate(GameObject o);
        public void AddObjectThreadSafe(GameObject o)
        {
            Application.Current.Dispatcher.Invoke((Action)(() =>
            {
                World.AddObject(o);
            }));
        }

        public void EndGame()
        {
            // Called from listen thread, so LT is already stopped
            LT.Running = false;
            UT.Running = false;
            Application.Current.Shutdown();
            Environment.Exit(0);
        }
        
        public Character GetCurrentCharacter()
        {
            return UT.CurrentCharacter;
        }

        public void RemoveDead()
        {
            foreach (GameObject o in World.Dead)
            {
                RemoveObject(o);
                if (IsServer)
                {
                    SendInfo(MessageBuilder.DeadMessage(o));
                }
            }
        }

        public void RemoveObject(int id)
        {
            RemoveObjectThreadSafe(id);
        }

        public void RemoveObject(GameObject o)
        {
            RemoveObjectThreadSafe(o);
        }

        private delegate void RemoveObjectThreadSafeDelegate(GameObject o);
        public void RemoveObjectThreadSafe(GameObject o)
        {
            Application.Current.Dispatcher.Invoke((Action)(() =>
            {
                World.RemoveObject(o);
            }));
        }

        private delegate void RemoveObjectThreadSafeDelegateInt(int id);
        public void RemoveObjectThreadSafe(int id)
        {
            Application.Current.Dispatcher.Invoke((Action)(() =>
            {
                World.RemoveObject(id);
            }));
        }

        internal void SendInfo(String toSend)
        {
            //Console.WriteLine(toSend);
            NetClient.WriteLine(toSend);
        }

        public void LoadWorld()
        {
            AddObject(new Bot(new Vector3(930, 100, 0), this, GameConstants.TYPE_BOT_BOSS));
            AddObject(new Bot(new Vector3(750, 100, 0), this, GameConstants.TYPE_BOT_MELEE));
            AddObject(new Bot(new Vector3(945, 420, 0), this, GameConstants.TYPE_BOT_MERCENARY));
            AddObject(new Bot(new Vector3(800, 400, 0), this, GameConstants.TYPE_BOT_SHOOTER));
            AddObject(new Bot(new Vector3(930, 260, 0), this, GameConstants.TYPE_BOT_TOWER));
            AddObject(new Bot(new Vector3(910, -10, 0), this, GameConstants.TYPE_BOT_TURRET));
            
            //Left side of enemy castle
            AddObject(new Bushes(new Vector3(580, -30, 0), this, new Vector3(60, 60, 0)));
            AddObject(new Bushes(new Vector3(520, -30, 0), this, new Vector3(60, 60, 0)));
            AddObject(new Bushes(new Vector3(460, -30, 0), this, new Vector3(60, 60, 0)));
            AddObject(new Bushes(new Vector3(400, -30, 0), this, new Vector3(60, 60, 0)));
            AddObject(new Bushes(new Vector3(340, -30, 0), this, new Vector3(60, 60, 0)));
            AddObject(new Bushes(new Vector3(280, -30, 0), this, new Vector3(60, 60, 0)));
            AddObject(new Bushes(new Vector3(220, -30, 0), this, new Vector3(60, 60, 0)));
            AddObject(new Bushes(new Vector3(160, -30, 0), this, new Vector3(60, 60, 0)));
            AddObject(new Bushes(new Vector3(100, -30, 0), this, new Vector3(60, 60, 0)));
            AddObject(new Bushes(new Vector3(40, -30, 0), this, new Vector3(60, 60, 0)));
            AddObject(new Bushes(new Vector3(-20, -30, 0), this, new Vector3(60, 60, 0)));

            //Right side of enemy castle
            AddObject(new Bushes(new Vector3(1300, -30, 0), this, new Vector3(60, 60, 0)));
            AddObject(new Bushes(new Vector3(1360, -30, 0), this, new Vector3(60, 60, 0)));
            AddObject(new Bushes(new Vector3(1420, -30, 0), this, new Vector3(60, 60, 0)));
            AddObject(new Bushes(new Vector3(1480, -30, 0), this, new Vector3(60, 60, 0)));
            AddObject(new Bushes(new Vector3(1540, -30, 0), this, new Vector3(60, 60, 0)));
            AddObject(new Bushes(new Vector3(1600, -30, 0), this, new Vector3(60, 60, 0)));
            AddObject(new Bushes(new Vector3(1660, -30, 0), this, new Vector3(60, 60, 0)));
            AddObject(new Bushes(new Vector3(1720, -30, 0), this, new Vector3(60, 60, 0)));
            AddObject(new Bushes(new Vector3(1780, -30, 0), this, new Vector3(60, 60, 0)));
            AddObject(new Bushes(new Vector3(1840, -30, 0), this, new Vector3(60, 60, 0)));
            AddObject(new Bushes(new Vector3(1900, -30, 0), this, new Vector3(60, 60, 0)));

            //Right Border
            AddObject(new Rocks(new Vector3(1900, 960, 0), this, new Vector3(60, 60, 0)));
            AddObject(new Rocks(new Vector3(1900, 900, 0), this, new Vector3(60, 60, 0)));
            AddObject(new Rocks(new Vector3(1900, 840, 0), this, new Vector3(60, 60, 0)));
            AddObject(new Rocks(new Vector3(1900, 780, 0), this, new Vector3(60, 60, 0)));
            AddObject(new Rocks(new Vector3(1900, 720, 0), this, new Vector3(60, 60, 0)));
            AddObject(new Rocks(new Vector3(1900, 660, 0), this, new Vector3(60, 60, 0)));
            AddObject(new Rocks(new Vector3(1900, 600, 0), this, new Vector3(60, 60, 0)));
            AddObject(new Rocks(new Vector3(1900, 540, 0), this, new Vector3(60, 60, 0)));

            AddObject(new Bushes(new Vector3(1900, 480, 0), this, new Vector3(60, 60, 0)));
            AddObject(new Bushes(new Vector3(1900, 420, 0), this, new Vector3(60, 60, 0)));
            AddObject(new Bushes(new Vector3(1900, 360, 0), this, new Vector3(60, 60, 0)));
            AddObject(new Bushes(new Vector3(1900, 300, 0), this, new Vector3(60, 60, 0)));
            AddObject(new Bushes(new Vector3(1900, 240, 0), this, new Vector3(60, 60, 0)));
            AddObject(new Bushes(new Vector3(1900, 180, 0), this, new Vector3(60, 60, 0)));
            AddObject(new Bushes(new Vector3(1900, 120, 0), this, new Vector3(60, 60, 0)));
            AddObject(new Bushes(new Vector3(1900, 60, 0), this, new Vector3(60, 60, 0)));

            //Left Border
            AddObject(new Rocks(new Vector3(-20, 960, 0), this, new Vector3(60, 60, 0)));
            AddObject(new Rocks(new Vector3(-20, 900, 0), this, new Vector3(60, 60, 0)));
            AddObject(new Rocks(new Vector3(-20, 840, 0), this, new Vector3(60, 60, 0)));
            AddObject(new Rocks(new Vector3(-20, 780, 0), this, new Vector3(60, 60, 0)));
            AddObject(new Rocks(new Vector3(-20, 720, 0), this, new Vector3(60, 60, 0)));
            AddObject(new Rocks(new Vector3(-20, 660, 0), this, new Vector3(60, 60, 0)));
            AddObject(new Rocks(new Vector3(-20, 600, 0), this, new Vector3(60, 60, 0)));
            AddObject(new Rocks(new Vector3(-20, 540, 0), this, new Vector3(60, 60, 0)));

            AddObject(new Bushes(new Vector3(-20, 480, 0), this, new Vector3(60, 60, 0)));
            AddObject(new Bushes(new Vector3(-20, 420, 0), this, new Vector3(60, 60, 0)));
            AddObject(new Bushes(new Vector3(-20, 360, 0), this, new Vector3(60, 60, 0)));
            AddObject(new Bushes(new Vector3(-20, 300, 0), this, new Vector3(60, 60, 0)));
            AddObject(new Bushes(new Vector3(-20, 240, 0), this, new Vector3(60, 60, 0)));
            AddObject(new Bushes(new Vector3(-20, 180, 0), this, new Vector3(60, 60, 0)));
            AddObject(new Bushes(new Vector3(-20, 120, 0), this, new Vector3(60, 60, 0)));
            AddObject(new Bushes(new Vector3(-20, 60, 0), this, new Vector3(60, 60, 0)));

            //Left side of friendly castle
            AddObject(new Rocks(new Vector3(580, 1030, 0), this, new Vector3(60, 60, 0)));
            AddObject(new Rocks(new Vector3(520, 1030, 0), this, new Vector3(60, 60, 0)));
            AddObject(new Rocks(new Vector3(460, 1030, 0), this, new Vector3(60, 60, 0)));
            AddObject(new Rocks(new Vector3(400, 1030, 0), this, new Vector3(60, 60, 0)));
            AddObject(new Rocks(new Vector3(340, 1030, 0), this, new Vector3(60, 60, 0)));
            AddObject(new Rocks(new Vector3(280, 1030, 0), this, new Vector3(60, 60, 0)));
            AddObject(new Rocks(new Vector3(220, 1030, 0), this, new Vector3(60, 60, 0)));
            AddObject(new Rocks(new Vector3(160, 1030, 0), this, new Vector3(60, 60, 0)));
            AddObject(new Rocks(new Vector3(100, 1030, 0), this, new Vector3(60, 60, 0)));
            AddObject(new Rocks(new Vector3(40, 1030, 0), this, new Vector3(60, 60, 0)));
            AddObject(new Rocks(new Vector3(-20, 1030, 0), this, new Vector3(60, 60, 0)));

            //Right side of friendly castle
            AddObject(new Rocks(new Vector3(1300, 1030, 0), this, new Vector3(60, 60, 0)));
            AddObject(new Rocks(new Vector3(1360, 1030, 0), this, new Vector3(60, 60, 0)));
            AddObject(new Rocks(new Vector3(1420, 1030, 0), this, new Vector3(60, 60, 0)));
            AddObject(new Rocks(new Vector3(1480, 1030, 0), this, new Vector3(60, 60, 0)));
            AddObject(new Rocks(new Vector3(1540, 1030, 0), this, new Vector3(60, 60, 0)));
            AddObject(new Rocks(new Vector3(1600, 1030, 0), this, new Vector3(60, 60, 0)));
            AddObject(new Rocks(new Vector3(1660, 1030, 0), this, new Vector3(60, 60, 0)));
            AddObject(new Rocks(new Vector3(1720, 1030, 0), this, new Vector3(60, 60, 0)));
            AddObject(new Rocks(new Vector3(1780, 1030, 0), this, new Vector3(60, 60, 0)));
            AddObject(new Rocks(new Vector3(1840, 1030, 0), this, new Vector3(60, 60, 0)));
            AddObject(new Rocks(new Vector3(1900, 1030, 0), this, new Vector3(60, 60, 0)));

            //Friendly walls
            AddObject(new CastleWalls(new Vector3(650, 870, 0), this, new Vector3(40, 200, 0)));
            AddObject(new CastleWalls(new Vector3(1240, 870, 0), this, new Vector3(40, 200, 0)));
            AddObject(new CastleWalls(new Vector3(650, 870, 0), this, new Vector3(250, 40, 0)));
            AddObject(new CastleWalls(new Vector3(1030, 870, 0), this, new Vector3(250, 40, 0)));
            //Friendly backwall
            AddObject(new CastleWalls(new Vector3(680, 1070, 0), this, new Vector3(600, 40, 0)));

            //Enemy walls
            AddObject(new CastleWalls(new Vector3(650, -10, 0), this, new Vector3(40, 200, 0)));
            AddObject(new CastleWalls(new Vector3(1240, -10, 0), this, new Vector3(40, 200, 0)));
            AddObject(new CastleWalls(new Vector3(650, 150, 0), this, new Vector3(250, 40, 0)));
            AddObject(new CastleWalls(new Vector3(1030, 150, 0), this, new Vector3(250, 40, 0)));
            //Enemy backwall
            AddObject(new CastleWalls(new Vector3(680, -50, 0), this, new Vector3(600, 40, 0)));
        }
    }
}
