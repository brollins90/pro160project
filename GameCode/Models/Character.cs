using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GameCode.Models
{
    public class Character : Bot
    {
        private float RotationSpeed = 3;
        private float acceleration = 5;

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

        private int _ExperienceCap;
        public int ExperienceCap
        {
            get { return _ExperienceCap; }
            set { _ExperienceCap = value;
            this.FirePropertyChanged("ExperienceCap");
            }
        }

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

        private Weapon _Weapon;

        public Weapon Weapon
        {
            get { return _Weapon; }
            set { _Weapon = value; }
        }


        public Character(Vector position, GameManager manager)
            : base(position, manager)
        {
            Constitution = 5;
            Defense = 6;
            Experience = 0;
            ExperienceCap = 100;
            Gold = 0;
            Health = 100;
            Level = 1;
            Size = new Vector(50, 50);
            Strength = 3;
            Weapon = new Weapon(this, 1, 200, 20);
        }


        public void LevelUp()
        {
            this.Level += 1;
            this.Strength += 1;
            this.Constitution += 2;
            this.Experience = 0;
            this.ExperienceCap += 30;
            this.MaxHealth = Constitution * 20;
            this.Health = MaxHealth;
            this.Gold += 100;

            if (this.Level % 3 == 0)
            {
                this.Defense += 1;
                this.Gold += 150;
            }           
           
        }



        public override void Update(float deltaTime)
        {
            //Console.WriteLine("Character.Update()");
            ////GameObject objToProcess = this;
            //Vector currentPosition = this.Position;
            //Vector newPosition = currentPosition;

            GameCommands keyPressed = this.Controller.GetMove();
            if (keyPressed == GameCommands.Up)
            {
                Velocity = Velocity - (deltaTime * acceleration * new Vector(-1 * Math.Sin(Angle), Math.Cos(Angle)));
                //newPosition = new Vector() { X = currentPosition.X, Y = currentPosition.Y - Speed };
                //Direction = 90;
            }
            else if (keyPressed == GameCommands.Down)
            {

                Velocity = Velocity - (deltaTime * acceleration * new Vector(Math.Sin(Angle), -1 * Math.Cos(Angle)));
                //newPosition = new Vector() { X = currentPosition.X, Y = currentPosition.Y + Speed };
                //Direction = 270;
            }
            else if (keyPressed == GameCommands.Left)
            {
                Rotate(-RotationSpeed);
                //newPosition = new Vector() { X = currentPosition.X - Speed, Y = currentPosition.Y };
                //Direction = 180;
            }
            else if (keyPressed == GameCommands.Right)
            {
                Rotate(RotationSpeed);
                //newPosition = new Vector() { X = currentPosition.X + Speed, Y = currentPosition.Y };
                //Direction = 0;
            }
            //else if (keyPressed == GameCommands.Space)
            //{
            //    Weapon.ShootAt(new Vector(Position.X, Position.Y));
            //    //Console.WriteLine("recieved a space");
            //    //if (objToProcess.AttackType == AttackType.Ranged)
            //    //{
            //    Manager.AddProjectile(new GameProjectile(currentPosition + new Vector(Width / 2, Height / 2), this.Manager, new Vector(10, 10), this.Heading, 20, (objToProcess as Bot).Damage, 200)
            //    {
            //        Controller = null
            //    });
            //    //}
            //}
            //objToProcess.Position = newPosition;
            Position = Position + Velocity * deltaTime;
            Console.WriteLine("Position: {0}", Position);
            Console.WriteLine("Velocity: {0}", Velocity);
            Console.WriteLine("deltaTime: {0}", deltaTime);

            //bool collided = false;
            //foreach (GameObject o in Manager.World.Objects)
            //{
            //    if (objToProcess.ID != o.ID && objToProcess.CollidesWith(o))
            //        collided = true;
            //}
            //if (collided)
            //{
            //    objToProcess.Position = currentPosition;
            //    //Console.WriteLine("Collided");
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
