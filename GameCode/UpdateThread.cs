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
        private bool IsServer;
        private InputListener GL;

        public UpdateThread(GameManager manager, bool isServer, InputListener gl)
        {
            Console.WriteLine("{0} UpdateThread - Create", System.Threading.Thread.CurrentThread.ManagedThreadId);
            Manager = manager;
            World = Manager.World;
            GL = gl;
            IsServer = isServer;
            Timer = new DispatcherTimer();
            Timer.Interval = TimeSpan.FromMilliseconds((15));
            Timer.Tick += Timer_Tick;

        }
        public void Start()
        {
            Console.WriteLine("{0} UpdateThread - Start", System.Threading.Thread.CurrentThread.ManagedThreadId);
            LastTimeMillis = GetCurrentTime();
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

                        // Message sending code:
                        int type = GameConstants.TYPE_PROJ_ARROW;
                        if (o.GetType() == typeof(Arrow))
                        {
                            type = GameConstants.TYPE_PROJ_ARROW;
                        }
                        else if (o.GetType() == typeof(FireBall))
                        {
                            type = GameConstants.TYPE_PROJ_FIRE;
                        }
                        else if (o.GetType() == typeof(StabAttack))
                        {
                            type = GameConstants.TYPE_PROJ_STAB;
                        }
                        String msgString = "" + GameConstants.MSG_UPDATE + "," + type + "," + o.ID + "," + o.Position.x + "," + o.Position.y + "," + o.Position.z + "," + o.Velocity.x + "," + o.Velocity.y + "," + o.Velocity.z + "," + o.Angle;
                        Manager.SendInfo(msgString);
                        // End Message sending code:
                    }
                }

                // Update Bots
                foreach (Bot o in World.Bots)
                {
                    if (o.Alive)
                    {
                        o.Update(deltaTime);

                        // Message sending code:
                        String msgString = "" + GameConstants.MSG_UPDATE + "," + o.ClassType + "," + o.ID + "," + o.Position.x + "," + o.Position.y + "," + o.Position.z + "," + o.Velocity.x + "," + o.Velocity.y + "," + o.Velocity.z + "," + o.Angle;
                        Manager.SendInfo(msgString);
                        // End Message sending code:
                    }
                }

                // Update the Players
                foreach (Character o in World.Characters)
                {
                    if (o.Alive)
                    {
                        o.Update(deltaTime);

                        // Message sending code:
                        String msgString = "" + GameConstants.MSG_UPDATE + "," + o.ClassType + "," + o.ID + "," + o.Position.x + "," + o.Position.y + "," + o.Position.z + "," + o.Velocity.x + "," + o.Velocity.y + "," + o.Velocity.z + "," + o.Angle;
                        Manager.SendInfo(msgString);
                        // End Message sending code:
                    }
                }

                foreach (Debris o in World.Debris)
                {
                    if (o.Alive)
                    {
                        o.Update(deltaTime);
                    }
                }

                // Remove the dead
                World.RemoveDead();
            }
            else // not server
            {
                // Check input
                //CheckInput(deltaTime);
                // input
                // send self

            }

        }

        public int GetCurrentTime()
        {
            return Environment.TickCount;
        }
    }
}
