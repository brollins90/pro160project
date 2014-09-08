using System;
using System.Threading;
using System.Windows;
using GameCode.Helpers;
using GameCode.Models;
using GameCode.Models.Projectiles;

namespace GameCode
{
    /// <summary>
    /// The manager controls the game.  Everything passes through the gamemanager
    /// </summary>
    public class GameManager
    {
        private GameWorld _World;
        public GameWorld World
        {
            get { return _World; }
            set { _World = value; }
        }

        private NetworkClient NetClient;
        private bool IsServer;
        private UpdateThread UT;
        private GameListener LT;
        private Random random = new Random();

        public GameManager(bool isServer, NetworkClient netClient, InputListener gl, int classChosen)
        {
            IsServer = isServer;
            NetClient = netClient;
            World = new GameWorld();

            UT = new UpdateThread(this, IsServer, gl, classChosen);

            LT = new GameListener(NetClient, this);
            // Start the listener thread
            new Thread(LT.Start).Start();

            if (IsServer)
            {
                // If this is the server, then load the world
                LoadWorld();
            }
            else
            {
                // If it is not the server then request the data
                SendInfo(MessageBuilder.RequestAllMessage());
            }
            // Go into the update thread code
            UT.Start();
        }

        /// <summary>
        /// Add an object to the world
        /// </summary>
        /// <param name="o"></param>
        public void AddObject(GameObject o)
        {
            AddObjectThreadSafe(o);
        }

        /// <summary>
        /// Add delegate to make WPF happy
        /// </summary>
        /// <param name="o"></param>
        private delegate void AddObjectThreadSafeDelegate(GameObject o);
        public void AddObjectThreadSafe(GameObject o)
        {
            Application.Current.Dispatcher.Invoke((Action)(() =>
            {
                World.AddObject(o);
            }));
        }

        /// <summary>
        /// Damage the bot with the specified ID
        /// </summary>
        /// <param name="botID"></param>
        /// <param name="Damage"></param>
        /// <param name="Attacker"></param>
        internal void DamageBot(int botID, int Damage, Bot Attacker)
        {
            Bot b = (Bot)World.Get(botID);
            int damageAmount = b.TakeDamage(Damage);
            SendInfo(MessageBuilder.DecreaseHPMessage(b.ID, damageAmount));

            if (!b.Alive && Attacker.GetType() == typeof(Character))
            {
                Character c = (Character)World.Get(Attacker.ID);
                int experienceAmount = c.IncreaseExperience(b.ClassType);
                SendInfo(MessageBuilder.IncreaseStatMessage(c.ID, GameConstants.STAT_XP, experienceAmount));
            }
        }

        /// <summary>
        /// Decrease the amount of Gold for the specified Character
        /// </summary>
        /// <param name="objectID"></param>
        /// <param name="amount"></param>
        /// <param name="sendMessage"></param>
        internal void DecreaseGold(int objectID, int amount, bool sendMessage = true)
        {
            Character o = (Character)World.Get(objectID);
            if (o != null)
            {
                o.DecreaseGold(amount);
                if (sendMessage)
                {
                    SendInfo(MessageBuilder.DecreaseGoldMessage(objectID, amount));
                }
            }
        }

        /// <summary>
        /// Decrease the health for the specified Character
        /// </summary>
        /// <param name="objectID"></param>
        /// <param name="amount"></param>
        /// <param name="sendMessage"></param>
        internal void DecreaseHealth(int objectID, int amount, bool sendMessage = true)
        {
            Bot o = (Bot)World.Get(objectID);
            if (o != null)
            {
                o.DecreaseHealth(amount);
                if (sendMessage)
                {
                    SendInfo(MessageBuilder.DecreaseHPMessage(objectID, amount));
                }
            }
        }

        /// <summary>
        /// Send a dead message for this character and stop all the threads
        /// </summary>
        /// <param name="sendMessage"></param>
        public void EndGame(bool sendMessage = true)
        {
            if (sendMessage)
            {
                SendInfo(MessageBuilder.DeadMessage(GetCurrentCharacter()));
                SendInfo(MessageBuilder.GameOverMessage());
            }
            LT.Running = false;
            UT.Running = false;
            Application.Current.Shutdown();
            Environment.Exit(0);
        }

        /// <summary>
        /// Get the Character associated with this Manager
        /// </summary>
        /// <returns></returns>
        public Character GetCurrentCharacter()
        {
            return UT.CurrentCharacter;
        }

        /// <summary>
        /// Increase the gold for the specified character
        /// </summary>
        /// <param name="objectID"></param>
        /// <param name="amount"></param>
        internal void IncreaseGold(int objectID, int amount)
        {
            Character o = (Character)World.Get(objectID);
            if (o != null)
            {
                o.IncreaseGold(amount);
                SendInfo(MessageBuilder.IncreaseStatMessage(objectID, GameConstants.STAT_GOLD, amount));
            }
        }

        /// <summary>
        /// Increase the health of the specified character
        /// </summary>
        /// <param name="objectID"></param>
        /// <param name="amount"></param>
        /// <param name="sendMessage"></param>
        internal void IncreaseHealth(int objectID, int amount, bool sendMessage = true)
        {
            Bot o = (Bot)World.Get(objectID);
            if (o != null)
            {
                o.IncreaseHealth(amount);
                if (sendMessage)
                {
                    SendInfo(MessageBuilder.IncreaseHPMessage(objectID, amount));
                }
            }
        }

        /// <summary>
        /// Increase a stat for the specified character
        /// </summary>
        /// <param name="objectID"></param>
        /// <param name="stat"></param>
        /// <param name="amount"></param>
        /// <param name="sendMessage"></param>
        public void IncreaseStat(int objectID, int stat, int amount, bool sendMessage = true)
        {
            Character o = (Character)World.Get(objectID);
            if (o != null)
            {
                o.IncreaseStat(stat, amount);
                if (sendMessage)
                {
                    SendInfo(MessageBuilder.IncreaseStatMessage(objectID, stat, amount));
                }
            }
        }

        /// <summary>
        /// Level up the specified character
        /// </summary>
        /// <param name="characterID"></param>
        /// <param name="sendMessage"></param>
        public void LevelUpCharacter(int characterID, bool sendMessage = true)
        {
            Character c = (Character)World.Get(characterID);
            c.LevelUp();
            if (sendMessage)
            {
                SendInfo(MessageBuilder.LevelUpMessage(c));
            }
        }

        /// <summary>
        /// Remove all the dead objects in the world
        /// </summary>
        public void RemoveAllDead()
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

        /// <summary>
        /// Remove the object from the world
        /// </summary>
        /// <param name="id"></param>
        /// <param name="sendMessage"></param>
        public void RemoveObject(int id, bool sendMessage = false)
        {
            RemoveObject(World.Get(id), sendMessage);
        }

        /// <summary>
        /// Remove the object from the world
        /// </summary>
        /// <param name="o"></param>
        /// <param name="sendMessage"></param>
        public void RemoveObject(GameObject o, bool sendMessage = false)
        {
            RemoveObjectThreadSafe(o);
            if (sendMessage)
            {
                SendInfo(MessageBuilder.DeadMessage(o));
            }
        }

        /// <summary>
        /// Remove delegate to make WPF happy
        /// </summary>
        /// <param name="o"></param>
        private delegate void RemoveObjectThreadSafeDelegate(GameObject o);
        public void RemoveObjectThreadSafe(GameObject o)
        {
            Application.Current.Dispatcher.Invoke((Action)(() =>
                {
                    World.RemoveObject(o);
                }));
        }

        /// <summary>
        /// Sends an Add message for every object (for a new client)
        /// </summary>
        internal void SendAllObjects()
        {
            foreach (GameObject o in World.Objects)
            {
                if (o.Alive)
                {
                    this.SendInfo(MessageBuilder.AddMessage(o));
                }
            }
        }

        /// <summary>
        /// Sends a message to the other clients
        /// </summary>
        /// <param name="toSend"></param>
        internal void SendInfo(string toSend)
        {
            //Console.WriteLine(toSend);
            NetClient.WriteLine(toSend);
        }

        /// <summary>
        /// Spawn an enemy unit
        /// </summary>
        public void SpawnEnemy()
        {
            if (random.Next(0, 2) == 0)
            {
                SpawnEnemy(new Vector3(950, 240, 0), GameConstants.TYPE_BOT_MELEE);
            }
            else
            {
                SpawnEnemy(new Vector3(950, 200, 0), GameConstants.TYPE_BOT_SHOOTER);
            }
        }

        /// <summary>
        /// Spawn a specific enemy unit
        /// </summary>
        /// <param name="position"></param>
        /// <param name="type"></param>
        public void SpawnEnemy(Vector3 position, int type)
        {
            Bot b = new Bot(position, this, type);
            bool collides = false;
            foreach (GameObject o in World.Objects)
            {
                if (b.CollidesWith(o))
                {
                    collides = true;
                }
            }
            if (!collides)
            {
                AddObject(b);
            }
        }

        internal void SubmitBotAttack(int ownerID)
        {
            ((Bot)World.Get(ownerID)).Weapon.Attack();
        }

        ///// <summary>
        ///// Submit an attack from another client
        ///// </summary>
        ///// <param name="ownerID"></param>
        //internal void SubmitBotAttack(int ownerID, bool sendMessage = true)
        //{
        //    Character c = (Character)World.Get(ownerID);
        //    if (sendMessage)
        //    {
        //        this.SendInfo(MessageBuilder.AttackMessage(c, 0));
        //    }
        //    else
        //    {
        //        c.Weapon.Attack();
        //    }
        //}

        /// <summary>
        /// Upgrade a stat for the specified character
        /// </summary>
        /// <param name="objectID"></param>
        /// <param name="stat"></param>
        /// <param name="amount"></param>
        /// <param name="cost"></param>
        public void UpgradeStat(int objectID, int stat, int amount, int cost)
        {
            Character c = (Character)World.Get(objectID);
            IncreaseStat(objectID, stat, amount);
            DecreaseGold(objectID, cost);
        }

        /// <summary>
        /// Load the world
        /// </summary>
        public void LoadWorld()
        {
            SpawnEnemy(new Vector3(910, 0, 0), GameConstants.TYPE_BOT_TOWER);
            SpawnEnemy(new Vector3(930, 100, 0), GameConstants.TYPE_BOT_BOSS);

            AddObject(new Bot(new Vector3(760, 260, 0), this, GameConstants.TYPE_BOT_TURRET));
            AddObject(new Bot(new Vector3(1120, 260, 0), this, GameConstants.TYPE_BOT_TURRET));

            AddObject(new Bot(new Vector3(760, 340, 0), this, GameConstants.TYPE_BOT_MERCENARY));
            AddObject(new Bot(new Vector3(1120, 340, 0), this, GameConstants.TYPE_BOT_MERCENARY));

            AddObject(new Bot(new Vector3(945, 200, 0), this, GameConstants.TYPE_BOT_MELEE));

            AddObject(new Bot(new Vector3(545, 200, 0), this, GameConstants.TYPE_BOT_SHOOTER));
            AddObject(new Bot(new Vector3(590, 265, 0), this, GameConstants.TYPE_BOT_SHOOTER));
            AddObject(new Bot(new Vector3(545, 300, 0), this, GameConstants.TYPE_BOT_SHOOTER));
            AddObject(new Bot(new Vector3(545, 350, 0), this, GameConstants.TYPE_BOT_SHOOTER));
            AddObject(new Bot(new Vector3(645, 350, 0), this, GameConstants.TYPE_BOT_SHOOTER));
            AddObject(new Bot(new Vector3(600, 400, 0), this, GameConstants.TYPE_BOT_SHOOTER));

            AddObject(new Bot(new Vector3(1305, 400, 0), this, GameConstants.TYPE_BOT_SHOOTER));
            AddObject(new Bot(new Vector3(1260, 350, 0), this, GameConstants.TYPE_BOT_SHOOTER));
            AddObject(new Bot(new Vector3(1360, 350, 0), this, GameConstants.TYPE_BOT_SHOOTER));
            AddObject(new Bot(new Vector3(1360, 300, 0), this, GameConstants.TYPE_BOT_SHOOTER));
            AddObject(new Bot(new Vector3(1315, 265, 0), this, GameConstants.TYPE_BOT_SHOOTER));
            AddObject(new Bot(new Vector3(1360, 200, 0), this, GameConstants.TYPE_BOT_SHOOTER));

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


        /// <summary>
        /// Add a unit from the listener thread
        /// </summary>
        /// <param name="objectID"></param>
        /// <param name="messageType"></param>
        /// <param name="objectType"></param>
        /// <param name="pos"></param>
        /// <param name="vel"></param>
        /// <param name="ang"></param>
        /// <param name="data"></param>
        /// <param name="sendMessage"></param>
        internal void AddFromListener(int objectID, int messageType, int objectType, Vector3 pos, Vector3 vel, double ang, string[] data, bool sendMessage = false)
        {
            GameObject o = World.Get(objectID);
            if (o != null)
            {
                return;
            }

            if (objectType > GameConstants.TYPE_BOT_LOW && objectType < GameConstants.TYPE_BOT_HIGH) // its a bot
            {
                o = new Bot(pos, this, objectType)
                {
                    Angle = ang,
                    Velocity = vel,
                    ID = objectID
                };
            }
            else if (objectType > GameConstants.TYPE_CHARACTER_LOW && objectType < GameConstants.TYPE_CHARACTER_HIGH) // its a character
            {
                //int damage = int.Parse(data[11]);
                o = new Character(pos, this, null, objectType)
                {
                    Angle = ang,
                    Velocity = vel,
                    ID = objectID
                };
                
            }
            else if (objectType > GameConstants.TYPE_DEBRIS_LOW && objectType < GameConstants.TYPE_DEBRIS_HIGH) // its a debris
            {
                switch (objectType)
                {
                    case GameConstants.TYPE_DEBRIS_BUSH:
                        o = new Bushes(pos, this, vel) // for Debris, velocity is the size
                        {
                            Position = pos,
                            ID = objectID
                        };
                        break;
                    case GameConstants.TYPE_DEBRIS_ROCK:
                        o = new Rocks(pos, this, vel) // for Debris, velocity is the size
                        {
                            Position = pos,
                            ID = objectID
                        };
                        break;
                    case GameConstants.TYPE_DEBRIS_WALL:
                        o = new CastleWalls(pos, this, vel) // for Debris, velocity is the size
                        {
                            Position = pos,
                            ID = objectID
                        };
                        break;
                }
            }
            else if (objectType > GameConstants.TYPE_PROJ_LOW && objectType < GameConstants.TYPE_PROJ_HIGH) // its a projectile
            {
                int ownerID = int.MaxValue;
                if (messageType == GameConstants.MOVEMENT_ATTACK)
                {
                    ownerID = int.Parse(data[11]);
                }
                switch (objectType)
                {
                    case GameConstants.TYPE_PROJ_ARROW:
                        o = new Arrow(ownerID, this, ang)
                        {
                            Position = pos,
                            Velocity = vel,
                            ID = objectID
                        };
                        break;
                    case GameConstants.TYPE_PROJ_FIRE:
                        o = new FireBall(ownerID, this, ang)
                        {
                            Position = pos,
                            Velocity = vel,
                            ID = objectID
                        };
                        break;
                    case GameConstants.TYPE_PROJ_STAB:
                        o = new StabAttack(ownerID, this, ang)
                        {
                            Position = pos,
                            Velocity = vel,
                            ID = objectID
                        };
                        break;
                }
            }
            this.AddObjectThreadSafe(o);
            //if (o is Character)
            //{
            //    (o as Character).Acceleration = (o as Character).Acceleration * 20;
            //}
            
            if (sendMessage)
            {
                // neveer true
            }
        }

        /// <summary>
        /// Update a unit from the listener thread
        /// </summary>
        /// <param name="objectID"></param>
        /// <param name="messageType"></param>
        /// <param name="objectType"></param>
        /// <param name="pos"></param>
        /// <param name="vel"></param>
        /// <param name="ang"></param>
        /// <param name="data"></param>
        internal void UpdateFromListener(int objectID, int messageType, int objectType, Vector3 pos, Vector3 vel, double ang, string[] data)
        {
            GameObject o = World.Get(objectID);
            if (o != null)
            {
                //Bot b = (Bot)o;
                o.Angle = ang;
                o.Position = pos;
                //b.Velocity = vel;
            }
            else
            {
                AddFromListener(objectID, messageType, objectType, pos, vel, ang, data);
            }
        }
    }
}
