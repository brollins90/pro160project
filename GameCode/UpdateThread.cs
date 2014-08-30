using GameCode.Helpers;
using GameCode.Models;
using GameCode.Models.Projectiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace GameCode
{
    public class UpdateThread
    {
        private DispatcherTimer Timer;
        private GameManager Manager;
        private GameWorld World;
        private int LastTimeMillis = 0;
        private int LastSpawnTimeMillis = 0;
        private bool IsServer;
        private InputListener GL;
        public Character CurrentCharacter; 
        public bool Running { get; set; }
        private Random Rand;


        public UpdateThread(GameManager manager, bool isServer, InputListener gl, int classChosen)
        {
            Console.WriteLine("{0} UpdateThread - Create", System.Threading.Thread.CurrentThread.ManagedThreadId);
            Running = false;
            Manager = manager;
            World = Manager.World;
            GL = gl;
            IsServer = isServer;
            Timer = new DispatcherTimer();
            Timer.Interval = TimeSpan.FromMilliseconds((15));
            Timer.Tick += Timer_Tick;
            Rand = new Random();

            //int r1 = new Random().Next(10000, 100000);

            CurrentCharacter = new Character(new Vector3(820 + Rand.Next(0,200), 800, 0), Manager, GL, classChosen)
            {
                ID = Rand.Next(10000, 100000)
            };
            Manager.AddObject(CurrentCharacter);
            //Console.WriteLine("{0} UpdateThread - CreatedCharacter: {1}", System.Threading.Thread.CurrentThread.ManagedThreadId, CurrentCharacter.ID);

            // i think this is already somewhere else...
            //if (!isServer)
            //{
            //    Manager.SendInfo(MessageBuilder.AddMessage(CurrentCharacter));
            //}


        }
        public void Start()
        {
            Console.WriteLine("{0} UpdateThread - Start", System.Threading.Thread.CurrentThread.ManagedThreadId);
            Running = true;
            LastTimeMillis = GetCurrentTime();
            LastSpawnTimeMillis = LastTimeMillis;
            Timer.Start();
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

        public void Update(float deltaTime)
        {
            //Console.WriteLine("{0} UpdateThread - Update({1})", System.Threading.Thread.CurrentThread.ManagedThreadId, deltaTime);

            // Spawn some bad guys
            if ((LastTimeMillis - LastSpawnTimeMillis) > 30000) // spawn every 30 seconds
            {
                LastSpawnTimeMillis = LastTimeMillis;
                Manager.SpawnEnemy();
            }

            // Since both the clients and the server are controlling a character, update the position before we check to see 
            // if this is the client or not
            CurrentCharacter.Update(deltaTime);

            if (IsServer)
            {
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
                        Manager.SendInfo(MessageBuilder.UpdateMessage(o));
                    }
                }

                // Update Bots
                foreach (Bot o in World.Bots)
                {
                    if (o.Alive)
                    {
                        o.Update(deltaTime);
                        Manager.SendInfo(MessageBuilder.UpdateMessage(o));
                    }
                }

                // Update the Players
                foreach (Character o in World.Characters)
                {
                    if (o.Alive)
                    {
                        o.Update(deltaTime);
                        Manager.SendInfo(MessageBuilder.UpdateMessage(o));
                    }
                }

                // Update the Debris  (Right now we dont need to do this since debris will never change)
                //foreach (Debris o in World.Debris)
                //{
                //    if (o.Alive)
                //    {
                //        o.Update(deltaTime);
                //    }
                //}

                // Remove the dead
                Manager.RemoveDead();

                // Check if the attack was pressed for the Current Character
                if (CurrentCharacter.IL.KeyAttack)
                {
                    CurrentCharacter.Weapon.Attack();
                }

            }
            else // not server
            {
                // Check if the attack was pressed for the Current Character
                if (CurrentCharacter.IL.KeyAttack)
                {
                    Manager.SendInfo(MessageBuilder.AttackMessage(CurrentCharacter, 0));
                }

                // Update Character position
                Manager.SendInfo(MessageBuilder.UpdateMessage(CurrentCharacter));
            }
        }

        public int GetCurrentTime()
        {
            return Environment.TickCount;
        }
    }
}
