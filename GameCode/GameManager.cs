﻿using System;
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

namespace GameCode
{
    public class GameManager
    {
        public const int TEAM_INT_DEBRIS = 97;
        public const int TEAM_INT_PLAYER = 98;
        public const int TEAM_INT_BADDIES = 99;

        private DispatcherTimer Timer;
        private GameWorld _World;
        public GameWorld World
        {
            get { return _World; }
            set { _World = value; }
        }

        private ObservableCollection<Controller> _Controllers;
        public ObservableCollection<Controller> Controllers
        {
            get { return _Controllers; }
            set { _Controllers = value; }
        }

        private int LastTimeMillis = 0;
        public int tempMyTeam = 15;
        public int tempOtherTeam = 20;
        

        public GameManager()
        {
            Timer = new DispatcherTimer();
            World = new GameWorld();
            Controllers = new ObservableCollection<Controller>();
            Timer.Interval = TimeSpan.FromMilliseconds((5));
            Timer.Start();
            Timer.Tick += Timer_Tick;

            LastTimeMillis = GetCurrentTime();
            LoadWorld("");

        }

        void Timer_Tick(object sender, EventArgs e)
        {
            int currentTimeMillis = Environment.TickCount;
            int elapsedTimeMillis = currentTimeMillis - LastTimeMillis;
            //Console.WriteLine("lastTime...: " + LastTimeMillis);
            //Console.WriteLine("CurrentTime: " + currentTimeMillis);
            //Console.WriteLine("elapsedTime: " + elapsedTimeMillis);
            float elapsedTimeFloat = (float)elapsedTimeMillis / 10; // should me 1000
            //Console.WriteLine(elapsedTimeFloat);

            Update(elapsedTimeFloat);
            LastTimeMillis = currentTimeMillis;
        }

        //public void AddNPC()
        //{
        //    GameObject newCharacter = new Bot(new Vector3(300, 150,0), this);
        //    World.Objects.Add(newCharacter);
        //}

        public void AddNPC(GameObject o)
        {
            World.Objects.Add(o);
        }

        public int AddPlayer(Controller playerController, CharacterClasses type = Models.CharacterClasses.Fighter)
        {
            Character c = new Character(new Vector3(920, 800, 0), this, type)
            {
                Team = TEAM_INT_PLAYER
            };
            c.Controller = playerController;
            World.Objects.Add(c);
            Controllers.Add(playerController);
            return c.ID;
        }

        public void AddWalls(CastleWalls Walls)
        {
            World.Objects.Add(Walls);
        }

        public void AddBush(Bushes Bush)
        {
            World.Objects.Add(Bush);
        }

        public void AddProjectile(GameProjectile p)
        {
            World.Objects.Add(p);            
        }

        public void LoadWorld(string filename)
        {
            AddNPC(new Bot(new Vector3(930, 100,0),this, BotClass.Boss));
            AddNPC(new Bot(new Vector3(750, 100, 0), this, BotClass.Melee));
            AddNPC(new Bot(new Vector3(945, 420, 0), this, BotClass.Mercenary));
            AddNPC(new Bot(new Vector3(800, 400, 0), this, BotClass.Shooter));
            AddNPC(new Bot(new Vector3(930, 260, 0), this, BotClass.Turret));
            AddNPC(new Bot(new Vector3(910, -10, 0), this, BotClass.Tower));


            AddBush(new Bushes(new Vector3(300, 300, 0), this, new Vector3(60, 60, 0)));
            AddBush(new Bushes(new Vector3(400, 450, 0), this, new Vector3(60, 60, 0)));
            AddBush(new Bushes(new Vector3(237, 900, 0), this, new Vector3(50, 50, 0)));
            AddBush(new Bushes(new Vector3(297, 836, 0), this, new Vector3(50, 50, 0)));
            AddBush(new Bushes(new Vector3(234, 104, 0), this, new Vector3(60, 60, 0)));
            AddBush(new Bushes(new Vector3(1370, 374, 0), this, new Vector3(60, 60, 0)));


            AddWalls(new CastleWalls(new Vector3(650, 870, 0), this, new Vector3(40, 200, 0)));
            AddWalls(new CastleWalls(new Vector3(1240, 870, 0), this, new Vector3(40, 200, 0)));
            AddWalls(new CastleWalls(new Vector3(650, 870, 0), this, new Vector3(250, 40, 0)));
            AddWalls(new CastleWalls(new Vector3(1030, 870, 0), this, new Vector3(250, 40, 0)));
            AddWalls(new CastleWalls(new Vector3(650, -10, 0), this, new Vector3(40, 200, 0)));
            AddWalls(new CastleWalls(new Vector3(1240, -10, 0), this, new Vector3(40, 200, 0)));
            AddWalls(new CastleWalls(new Vector3(650, 150, 0), this, new Vector3(250, 40, 0)));
            AddWalls(new CastleWalls(new Vector3(1030, 150, 0), this, new Vector3(250, 40, 0)));
        }

        public void Update(float deltaTime)
        {
            //Console.WriteLine("{0} {1} Update: {2}", (int)AppDomain.GetCurrentThreadId(), Environment.TickCount, deltaTime);

            //Console.WriteLine("deltatime: " + deltaTime);
            
            // If paused, return
            // TODO

            // Update Pathing
            // TODO

            // Update Projectiles
            foreach (GameProjectile o in World.Projectiles)
            {
                if (o.Alive)
                {
                    o.Update(deltaTime);
                }
            }

            // Update Bots
            foreach (Bot o in World.Bots) // implici cast
            {
                if (o.Alive)
                {
                    
                    o.Update(deltaTime);
                }
            }

            // Update the Players
            foreach (Character o in World.Characters) // implici cast
            {
                if (o.Alive)
                {
                    o.Update(deltaTime);
                }
            }

            foreach (Bushes o in World.Bushes)
            {
                if (o.Alive)
                {
                    o.Update(deltaTime);
                }
            }

            foreach (CastleWalls o in World.CastleWalls) 
            {
                if (o.Alive)
                {
                    o.Update(deltaTime);
                }
            }

            
            


            // Remove the dead
            World.Remove();

        }

        public void ResetGame()
        {

        }

        public void SendState()
        {

        }

        public void StartGame()
        {
        }

        public void SubmitMove(GameCommand command)
        {
            foreach (Controller c in Controllers)
            {
                if (c.GameObjectID == command.GameObjectId)
                {
                    c.KeyDown(command);
                }
            }
        }

        public int GetCurrentTime()
        {
            return Environment.TickCount;
        }
    }
}
