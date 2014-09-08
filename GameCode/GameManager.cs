using System;
using System.Threading;
using System.Windows;
using GameCode.Helpers;
using GameCode.Models;
using GameCode.Models.Projectiles;

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

        private NetworkClient NetClient;
        private bool IsServer;
        private UpdateThread UT;
        private GameListener LT;
        private Random random = new Random();

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

        public void EndGame(bool sendMessage = true)
        {
            if (sendMessage)
            {
                SendInfo(MessageBuilder.DeadMessage(GetCurrentCharacter()));
            }
            LT.Running = false;
            UT.Running = false;
            Application.Current.Shutdown();
            Environment.Exit(0);
        }

        public Character GetCurrentCharacter()
        {
            return UT.CurrentCharacter;
        }

        internal void IncreaseGold(int objectID, int amount)
        {
            Character o = (Character)World.Get(objectID);
            if (o != null)
            {
                o.IncreaseGold(amount);
                SendInfo(MessageBuilder.IncreaseStatMessage(objectID, GameConstants.STAT_GOLD, amount));
            }
        }

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

        public void LevelUpCharacter(int characterID, bool sendMessage = true)
        {
            Character c = (Character)World.Get(characterID);
            c.LevelUp();
            if (sendMessage)
            {
                SendInfo(MessageBuilder.LevelUpMessage(c));
            }
        }

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

        public void RemoveObject(int id, bool sendMessage = false)
        {
            RemoveObject(World.Get(id), sendMessage);
        }

        public void RemoveObject(GameObject o, bool sendMessage = false)
        {
            RemoveObjectThreadSafe(o);
            if (sendMessage)
            {
                SendInfo(MessageBuilder.DeadMessage(o));
            }
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

        internal void SendAllObjects()
        {
            // send every object
            foreach (GameObject o in World.Objects)
            {
                if (o.Alive)
                {
                    //Manager.SendInfo(msgString);
                    this.SendInfo(MessageBuilder.AddMessage(o));
                }
            }
        }

        internal void SendInfo(string toSend)
        {
            //Console.WriteLine(toSend);
            NetClient.WriteLine(toSend);
        }

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

        public void UpgradeStat(int objectID, int stat, int amount, int cost)
        {
            Character c = (Character)World.Get(objectID);
            IncreaseStat(objectID, stat, amount);
            DecreaseGold(objectID, cost);
        }

        //public void UpgradeStr(Character CurrentCharacter, int StatIncrease, int GoldAmount)
        //{
        //    CurrentCharacter.Strength += StatIncrease;

        //}

        //public void UpgradeLife(Character CurrentCharacter, int StatIncrease, int GoldAmount)
        //{
        //    CurrentCharacter.Constitution += StatIncrease;
        //}

        //public void UpgradeDef(Character CurrentCharacter, int StatIncrease, int GoldAmount)
        //{
        //    CurrentCharacter.Defense += StatIncrease;
        //}

        public void LoadWorld()
        {
            SpawnEnemy(new Vector3(910, 0, 0), GameConstants.TYPE_BOT_TOWER);
            SpawnEnemy(new Vector3(930, 100, 0), GameConstants.TYPE_BOT_BOSS);

            AddObject(new Bot(new Vector3(760, 260, 0), this, GameConstants.TYPE_BOT_TURRET));
            AddObject(new Bot(new Vector3(1120, 260, 0), this, GameConstants.TYPE_BOT_TURRET));

            AddObject(new Bot(new Vector3(760, 320, 0), this, GameConstants.TYPE_BOT_MERCENARY));
            AddObject(new Bot(new Vector3(1120, 320, 0), this, GameConstants.TYPE_BOT_MERCENARY));

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
            
            if (sendMessage)
            {
                // neveer true
            }
        }

        internal void UpdateFromListener(int objectID, int messageType, int objectType, Vector3 pos, Vector3 vel, double ang, string[] data)
        {
            GameObject o = World.Get(objectID);
            if (o != null)
            {
                o.Angle = ang;
                o.Position = pos;
            }
            else
            {
                AddFromListener(objectID, messageType, objectType, pos, vel, ang, data);
            }
        }
    }
}
