using GameCode.Models.Weapons;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using GameCode.Helpers;
using GameCode.Models.Projectiles;

namespace GameCode.Models
{
    public enum CharacterClasses {
        Mage,
        Fighter,
        Archer
    };
    public class Character : Bot
    {
        
        private double _HealthBarLength;
        public double HealthBarLength
        {
            get { return _HealthBarLength; }
            set 
            {
                _HealthBarLength = (double)((double)Health / (double)MaxHealth) * 100;
                FirePropertyChanged("HealthBarLength");
            }
        }

        private int _Constitution;
        public int Constitution
        {
            get { return _Constitution; }
            set { _Constitution = value;
                this.FirePropertyChanged("Constitution");
            }
        }

        private int _Defense;
        public int Defense
        {
            get { return _Defense; }
            set { _Defense = value;
                this.FirePropertyChanged("Defense");
            }
        }

        private int _Experience;
        public int Experience
        {
            get { return _Experience; }
            set { _Experience = value;
                this.FirePropertyChanged("Experience");
            }
        }

        private int _ExperienceNextLevel = 10;
        public int ExperienceNextLevel
        {
            get { return _ExperienceNextLevel; }
            set
            {
                _ExperienceNextLevel = value;
                this.FirePropertyChanged("ExperienceNextLevel");
            }
        }

        private int _ExperienceCap;
        public int ExperienceCap
        {
            get { return _ExperienceCap; }
            set { _ExperienceCap = value;
                this.FirePropertyChanged("ExperienceCap");
            }
        }

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

        //private double _ExpBarLength;
        //public double ExpBarLength
        //{
        //    get { return _ExpBarLength; }
        //    set { _ExpBarLength = ((double)Experience / (double)ExperienceCap) * 580;
        //        FirePropertyChanged("ExpBarLength");
        //    }
        //}

        private int _Level;
        public int Level
        {
            get { return _Level; }
            set { _Level = value;
                this.FirePropertyChanged("Level");
            }
        }

        private int _Strength;
        public int Strength
        {
            get { return _Strength; }
            set { _Strength = value;
                this.FirePropertyChanged("Strength");
            }
        }

        private int _Gold;
        public int Gold
        {
            get { return _Gold; }
            set { _Gold = value;
                this.FirePropertyChanged("Gold");
            }
        }

        public Character(Vector3 position, GameManager manager, InputListener il, int type = GameConstants.TYPE_CHARACTER_ARCHER)
            : base(position, manager, type)
        {
            Team = GameConstants.TEAM_INT_PLAYERS;
            IL = il;

            switch (type)
            {
                case GameConstants.TYPE_CHARACTER_ARCHER:
                    Acceleration = new Vector3(5, 5, 0);
                    Weapon = new CrossBow(this);
                    Angle = -90;
                    ClassType = type;
                    Constitution = 5;
                    Defense = 5;
                    Experience = 0;
                    //ExperienceCap = 100;
                    Gold = 0;
                    Level = 1;
                    Size = new Vector3(32, 32, 0);
                    Strength = 3;

                    Damage = Strength * 2;
                    MaxHealth = Constitution * 20;
                    RestoreHealthToMax();
                    break;

                case GameConstants.TYPE_CHARACTER_FIGHTER:
                    Acceleration = new Vector3(8, 8, 0);
                    Weapon = new Sword(this);
                    Angle = -90;
                    ClassType = type;
                    Constitution = 7;
                    Defense = 6;
                    Experience = 0;
                    //ExperienceCap = 100;
                    Gold = 0;
                    Level = 1;
                    Size = new Vector3(32, 32, 0);
                    Strength = 3;

                    Damage = Strength * 2;
                    MaxHealth = Constitution * 20;
                    RestoreHealthToMax();
                    break;

                case GameConstants.TYPE_CHARACTER_MAGE:
                    Acceleration = new Vector3(3, 3, 0);
                    Weapon = new Magic(this);
                    Angle = -90;
                    ClassType = type;
                    Constitution = 4;
                    Defense = 4;
                    Experience = 0;
                    //ExperienceCap = 100;
                    Gold = 0;
                    Level = 1;
                    Size = new Vector3(32, 32, 0);
                    Strength = 3;

                    Damage = Strength * 3;
                    MaxHealth = Constitution * 20;
                    RestoreHealthToMax();
                    break;
            }
        }

        public int IncreaseExperience(int killType)
        {
            int amount = 10;
            switch (killType)
            {
                case GameConstants.TYPE_BOT_BOSS:
                    amount = 400;
                    break;
                case GameConstants.TYPE_BOT_MELEE:
                default:
                    amount = 20;
                    break;
                case GameConstants.TYPE_BOT_MERCENARY:
                    amount = 100;
                    break;
                case GameConstants.TYPE_BOT_SHOOTER:
                    amount = 30;
                    break;
                case GameConstants.TYPE_BOT_TOWER:
                    amount = 1000;
                    break;
                case GameConstants.TYPE_BOT_TURRET:
                    amount = 100;
                    break;

        }

            Experience += amount;
            //if (Experience > ExperienceCap) { 
            //    Experience = ExperienceCap;
            //}
            return amount;

        }

        public void LevelUp()
        {
            this.Level += 1;
            this.Strength += 2;
            this.Constitution += 2;
            //this.Experience = 0;
            //this.ExperienceCap += 30;
            this.Damage = Strength * 2;
            this.MaxHealth = Constitution * 20;
            RestoreHealthToMax();
            this.Gold += 100;
            this.HealthBarLength = ((double)Health / (double)MaxHealth) * 100;
            if (this.Level % 3 == 0)
            {
                this.Defense += 1;
                this.Gold += 150;
            }
        }

        public void RestoreHealthToMax()
        {
            Health = MaxHealth;
        }

        public override int TakeDamage(int amount)
        {
            amount -= Defense;
            amount = (amount <= 0) ? 1 : amount;
            return base.TakeDamage(amount);
        }

        public override void CheckInput(double deltaTime) {
            if (IL != null)
            {


                if (IL.KeyForward)//(keyPressed == GameCommands.Up)
                {
                    this.MoveForward(deltaTime);
            }
                if (IL.KeyBackward)//(keyPressed == GameCommands.Down)
            {
                    this.MoveBackward(deltaTime);
            }
                if (IL.KeyLeft)//(keyPressed == GameCommands.Left)
            {
                    this.MoveLeft(deltaTime);
            }
                if (IL.KeyRight)//(keyPressed == GameCommands.Right)
            {
                    this.MoveRight(deltaTime);
            }
            //else if (Controller.InputListener.KeyBackward)//(keyPressed == GameCommands.None)
            //{
            //    StopMoving(deltaTime);
            //}
                //if (keyPressed == GameCommands.MouseMove)
                //{
                System.Windows.Point mousePos = IL.MousePos;// (System.Windows.Point)cmd.Additional;
                this.RotateTowardPosition(new Vector3(mousePos.X, mousePos.Y, 0));
                //}
                //if (IL.KeyAttack)//(keyPressed == GameCommands.Space || 
                ////keyPressed == GameCommands.LeftClick)
                //{
                //    int projID = 0;// this.Weapon.Attack();
                //    Manager.SendInfo(MessageBuilder.AttackMessage(this, projID));
                //}
            }
        }

        public override void Update(double deltaTime)
        {
            if (Experience >= ExperienceNextLevel)
            {
                ExperienceNextLevel += ExperienceNextLevel;
                LevelUp();
        }

            if (IL != null)
        {
                CheckInput(deltaTime);
        }

            base.Update(deltaTime);
        }
    }
}
