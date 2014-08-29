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
            get { return HealthBarLength; }
            set 
            {
                HealthBarLength = (double)((double)Health / (double)MaxHealth) * 100;
                FirePropertyChanged("HealthBarLength");
            }
        }

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

        private int _ExperienceCap;
        public int ExperienceCap
        {
            get { return _ExperienceCap; }
            set
            {
                _ExperienceCap = value;
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
            IL = il;

            switch (type)
            {
                case GameConstants.TYPE_CHARACTER_ARCHER:
                    Acceleration = new Vector3(5, 5, 0);
                    Weapon = new CrossBow(this);
                    Angle = -90;
                    ClassType = type;
                    Constitution = 5;
                    Defense = 6;
                    Experience = 0;
                    Damage = Strength * 2;
                    ExperienceCap = 100;
                    Gold = 0;
                    MaxHealth = Constitution * 20;
                    RestoreHealthToMax();
                    Level = 1;
                    Size = new Vector3(32, 32, 0);
                    Strength = 3;
                    break;

                case GameConstants.TYPE_CHARACTER_FIGHTER:
                    Acceleration = new Vector3(8, 8, 0);
                    Weapon = new Sword(this);
                    Angle = -90;
                    ClassType = type;
                    Constitution = 7;
                    Defense = 8;
                    Experience = 0;
                    Damage = Strength * 2;
                    ExperienceCap = 100;
                    Gold = 0;
                    MaxHealth = Constitution * 20;
                    RestoreHealthToMax();
                    Level = 1;
                    Size = new Vector3(32, 32, 0);
                    Strength = 3;
                    break;

                case GameConstants.TYPE_CHARACTER_MAGE:
                    Acceleration = new Vector3(3, 3, 0);
                    Weapon = new Magic(this);
                    Angle = -90;
                    ClassType = type;
                    Constitution = 4;
                    Defense = 4;
                    Experience = 0;
                    Damage = Strength * 3;
                    ExperienceCap = 100;
                    Gold = 0;
                    MaxHealth = Constitution * 20;
                    RestoreHealthToMax();
                    Level = 1;
                    Size = new Vector3(32, 32, 0);
                    Strength = 3;
                    break;
            }
        }

        public void LevelUp()
        {
            this.Level += 1;
            this.Strength += 2;
            this.Constitution += 2;
            this.Experience = 0;
            this.ExperienceCap += 30;
            this.Damage = Strength * 2;
            this.MaxHealth = Constitution * 20;
            RestoreHealthToMax();
            this.Gold += 100;

            if (this.Level % 3 == 0)
            {
                this.Defense += 1;
                this.Gold += 150;
            }

        }

        public override void CheckInput(double deltaTime) {

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
            if (IL.KeyAttack)//(keyPressed == GameCommands.Space || 
            //keyPressed == GameCommands.LeftClick)
            {
                this.Weapon.Attack();
            }
        }

        public override void Update(double deltaTime)
        {
            // add some natural breaking forces
            //Velocity *= BreakingSpeed;
            CheckInput(deltaTime);

            base.Update(deltaTime);



            ////Velocity = Heading * (Speed * deltaTime);

            //// save previous position
            //Vector3 previousPosition = new Vector3(Position.x, Position.y, Position.z);
            //// update position that we already calculated
            //Position = Position + Velocity;

            //// check for new collisions
            //bool collided = false;
            //foreach (GameObject o in Manager.World.Objects)
            //{
            //    if (this.ID != o.ID && this.CollidesWith(o))
            //        if (o.GetType() == typeof(GameProjectile) && ((GameProjectile)o).Owner.ID == this.ID)
            //        {
            //            // do nothing
            //        }
            //        else
            //        {
            //            collided = true;
            //        }
            //}
            //// if collided dont perform the move
            //if (collided)
            //{
            //    this.Position = previousPosition;
            //}
        }

        public void RestoreHealthToMax()
        {
            Health = MaxHealth;
        }

        public void IncreaseExperience(int amount)
        {
            Experience += amount;
            if (Experience > ExperienceCap)
                Experience = ExperienceCap;
        }
    }
}
