using System;
using System.Windows.Threading;
using GameCode.Helpers;
using GameCode.Models;
using GameCode.Models.Projectiles;

namespace GameCode
{
    /// <summary>
    /// Thread to update the game state
    /// </summary>
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
            Running = false;
            Manager = manager;
            World = Manager.World;
            GL = gl;
            IsServer = isServer;
            Timer = new DispatcherTimer();
            Timer.Interval = TimeSpan.FromMilliseconds((15));
            Timer.Tick += Timer_Tick;
            Rand = new Random();

            // Create this instance's character here
            CurrentCharacter = new Character(new Vector3(820 + Rand.Next(0,200), 800, 0), Manager, GL, classChosen)
            {
                ID = Rand.Next(10000, 100000)
            };
            Manager.AddObject(CurrentCharacter);

            // If this is not the server, double the acceleration to compensate lag
            if (!isServer)
            {
                CurrentCharacter.Acceleration = CurrentCharacter.Acceleration * 2;
            }
        }

        /// <summary>
        /// Start the update timer
        /// </summary>
        public void Start()
        {
            Running = true;
            LastTimeMillis = GetCurrentTime();
            LastSpawnTimeMillis = LastTimeMillis;
            Timer.Start();
        }

        /// <summary>
        /// What happens when the timer ticks
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Timer_Tick(object sender, EventArgs e)
        {
            int currentTimeMillis = GetCurrentTime();
            int elapsedTimeMillis = currentTimeMillis - LastTimeMillis;
            float elapsedTimeFloat = (float)elapsedTimeMillis / 10;

            Update(elapsedTimeFloat);
            LastTimeMillis = currentTimeMillis;
        }

        /// <summary>
        /// One update cycle
        /// </summary>
        /// <param name="deltaTime"></param>
        public void Update(float deltaTime)
        {
            // Spawn the bad guys
            if ((LastTimeMillis - LastSpawnTimeMillis) > 10000) // spawn every 10 seconds
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

                // Remove the dead
                Manager.RemoveAllDead();

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
