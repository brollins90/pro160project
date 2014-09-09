using GameCode.Helpers;
using GameCode.Models.Projectiles;
using GameCode.Models.Weapons;
using System;
using System.Windows;

namespace GameCode.Models
{
    /// <summary>
    /// A Character is a user controlled unit in the game
    /// </summary>
    public class Character : Bot
    {
        /// <summary>
        /// IDK
        /// </summary>
        private int _Constitution;
        public int Constitution
        {
            get { return _Constitution; }
            set
            {
                _Constitution = value;
                this.FirePropertyChanged("Constitution");
            }
        }

        /// <summary>
        /// More Defense means less damage taken
        /// </summary>
        private int _Defense;
        public int Defense
        {
            get { return _Defense; }
            set
            {
                _Defense = value;
                this.FirePropertyChanged("Defense");
            }
        }

        /// <summary>
        /// Store the progress on the way to the next level
        /// </summary>
        private int _Experience;
        public int Experience
        {
            get { return _Experience; }
            set
            {
                _Experience = value;
                this.FirePropertyChanged("Experience");
            }
        }

        /// <summary>
        /// The amount of Experience required to Level Up
        /// </summary>
        private int _ExperienceNextLevel;
        public int ExperienceNextLevel
        {
            get { return _ExperienceNextLevel; }
            set
            {
                _ExperienceNextLevel = value;
                this.FirePropertyChanged("ExperienceNextLevel");
            }
        }

        /// <summary>
        /// The interface for the User to control the Character
        /// </summary>
        private InputListener _IL;
        public InputListener IL
        {
            get { return _IL; }
            set
            {
                _IL = value;
                this.FirePropertyChanged("IL");
            }
        }

        /// <summary>
        /// The current level of the Character
        /// </summary>
        private int _Level;
        public int Level
        {
            get { return _Level; }
            set
            {
                _Level = value;
                this.FirePropertyChanged("Level");
            }
        }

        /// <summary>
        /// IDK
        /// </summary>
        private int _Strength;
        public int Strength
        {
            get { return _Strength; }
            set
            {
                _Strength = value;
                this.FirePropertyChanged("Strength");
            }
        }

        /// <summary>
        /// The amount of money the character has
        /// </summary>
        private int _Gold;
        public int Gold
        {
            get { return _Gold; }
            set
            {
                _Gold = value;
                this.FirePropertyChanged("Gold");
            }
        }


        public Character(Vector3 position, GameManager manager, InputListener il, int type = GameConstants.TYPE_CHARACTER_ARCHER)
            : base(position, manager, type)
        {
            Angle = -90;
            ClassType = type;
            Experience = 0;
            ExperienceNextLevel = 10;
            Gold = 10000;
            IL = il;
            Level = 1;
            Size = new Vector3(32, 32, 0);
            Team = GameConstants.TEAM_INT_PLAYERS;

            switch (type)
            {
                case GameConstants.TYPE_CHARACTER_ARCHER: //Average class, decent speed and average stats. Medium Range
                    Acceleration = new Vector3(5, 5, 0);
                    Constitution = 5;
                    Defense = 5;
                    Strength = 3;
                    Weapon = new CrossBow(this);
                    break;

                case GameConstants.TYPE_CHARACTER_FIGHTER: //Short Range, but quick movement. Also Tanky, small range
                    Acceleration = new Vector3(8, 8, 0);
                    Constitution = 7;
                    Defense = 6;
                    Strength = 2;
                    Weapon = new Sword(this);
                    break;

                case GameConstants.TYPE_CHARACTER_MAGE: //DPS class, slow, but has really high damage. longest range but the squishiest of all classes
                    Acceleration = new Vector3(3, 3, 0);
                    Constitution = 4;
                    Defense = 3;
                    Strength = 6;
                    Weapon = new Magic(this); 
                    break;
            }
            SetDamage();
            SetHealth();
            RestoreHealthToMax();
        }

        /// <summary>
        /// Check for input from the user
        /// </summary>
        /// <param name="deltaTime"></param>
        public override void CheckInput(double deltaTime)
        {
            // Only process input if the listener exists
            if (IL != null)
            {
                if (IL.KeyForward)
                {
                    this.MoveForward(deltaTime);
                }
                if (IL.KeyBackward)
                {
                    this.MoveBackward(deltaTime);
                }
                if (IL.KeyLeft)
                {
                    this.MoveLeft(deltaTime);
                }
                if (IL.KeyRight)
                {
                    this.MoveRight(deltaTime);
                }
                Point mousePos = IL.MousePos;
                // Always be rotating toward the mouse
                this.RotateTowardPosition(new Vector3(mousePos.X, mousePos.Y, 0));
            }
        }

        /// <summary>
        /// Decrease the amount of gold
        /// </summary>
        /// <param name="amount"></param>
        public void DecreaseGold(int amount)
        {
            this.Gold -= amount;
        }

        internal override void DecreaseHealth(int val)
        {
            base.DecreaseHealth(val);

            if (!Alive)
            {
                Manager.EndGame(true);
            }
        }

        /// <summary>
        /// Public way to increase experience
        /// </summary>
        /// <param name="killType"></param>
        /// <returns></returns>
        public int IncreaseExperience(int killType)
        {
            int amount = 10;
            switch (killType)
            {
                case GameConstants.TYPE_BOT_BOSS:
                    amount = 250;
                    break;
                case GameConstants.TYPE_BOT_MELEE:
                default:
                    amount = 15;
                    break;
                case GameConstants.TYPE_BOT_MERCENARY:
                    amount = 100;
                    break;
                case GameConstants.TYPE_BOT_SHOOTER:
                    amount = 20;
                    break;
                case GameConstants.TYPE_BOT_TOWER:
                    amount = 5000;
                    break;
                case GameConstants.TYPE_BOT_TURRET:
                    amount = 150;
                    break;
            }

            IncreaseStat(GameConstants.STAT_XP, amount);

            // If we need to level up, do it here
            if (Experience >= ExperienceNextLevel)
            {
                int leftovers = Experience - ExperienceNextLevel;

                //ExperienceNextLevel += ExperienceNextLevel;
                //LevelUp();
                Manager.LevelUpCharacter(this.ID);
                Experience = leftovers;
            }
            return amount;
        }

        /// <summary>
        /// Increase the amount of gold
        /// </summary>
        /// <param name="amount"></param>
        public void IncreaseGold(int amount)
        {
            this.Gold += amount;
        }

        /// <summary>
        /// public way to increase a stat
        /// </summary>
        /// <param name="stat"></param>
        /// <param name="amount"></param>
        public void IncreaseStat(int stat, int amount)
        {
            switch (stat)
            {
                case GameConstants.STAT_CONSTITUTION:
                    Constitution += amount;
                    break;
                case GameConstants.STAT_DEFENSE:
                    Defense += amount;
                    break;
                case GameConstants.STAT_LEVEL:
                    Level += amount;
                    break;
                case GameConstants.STAT_STRENGTH:
                    Strength += amount;
                    break;
                case GameConstants.STAT_XP:
                    Experience += amount;
                    break;
                default:
                    throw new Exception("Unknown stat");
            }
            SetDamage();
            SetHealth();
        }

        /// <summary>
        /// Increase the level of the character
        /// </summary>
        public void LevelUp()
        {
            ExperienceNextLevel = ExperienceNextLevel + 40;
            IncreaseStat(GameConstants.STAT_LEVEL, 1);
            IncreaseStat(GameConstants.STAT_CONSTITUTION, 2);
            IncreaseGold(100);
            IncreaseStat(GameConstants.STAT_STRENGTH, 2);
            SetDamage();
            SetHealth();
            if (this.Level % 3 == 0)
            {
                IncreaseStat(GameConstants.STAT_DEFENSE, 1);
                IncreaseGold(150);
            }
        }

        /// <summary>
        /// Set health to the max
        /// </summary>
        public void RestoreHealthToMax()
        {
            Health = MaxHealth;
        }

        /// <summary>
        /// set the amount of damage to the amount relative to the strength value
        /// </summary>
        private void SetDamage()
        {
            Damage = Strength * 2;
        }

        /// <summary>
        /// set the amount of health to the amount relative to the constitution value
        /// </summary>
        private void SetHealth()
        {
            MaxHealth = Constitution * 20;
            RestoreHealthToMax();
        }

        /// <summary>
        /// public way to lower the health
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public override int TakeDamage(int amount)
        {
            amount -= Defense;
            amount = (amount <= 0) ? 1 : amount;
            return base.TakeDamage(amount);
        }

        /// <summary>
        /// move the character
        /// </summary>
        /// <param name="deltaTime"></param>
        public override void Update(double deltaTime)
        {            
            // check for input
            CheckInput(deltaTime);

            // move the character
            base.Update(deltaTime);
        }
    }
}
