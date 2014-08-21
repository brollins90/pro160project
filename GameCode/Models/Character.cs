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

namespace GameCode.Models
{
    public class Character : Bot
    {
        private float RotationSpeed = 3;
        //private Vector3 acceleration = new Vector3(10,10,0);

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

        //private PlayerInput _PlayerInput;

        //public PlayerInput PlayerInput
        //{
        //    get { return _PlayerInput; }
        //    set { _PlayerInput = value; }
        //}


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


        public Character(Vector3 position, GameManager manager)
            : base(position, manager)
        {
            Angle = -90;
            Constitution = 5;
            Defense = 6;
            Experience = 0;
            ExperienceCap = 100;
            Gold = 0;
            Health = 100;
            Level = 1;
            Size = new Vector3(50, 50,0);
            Strength = 3;
            Weapon = new CrossBow(this);
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



        public override void Update(double deltaTime)
        {






            //calculate the combined steering force
            // TODO

            //if no steering force is produced decelerate the player by applying a
            //braking force        
  
            const double BrakingRate = 0.8;
            Velocity *= BrakingRate;       


            //calculate the acceleration
            //Vector accel = force / m_dMass;
            Vector3 acceleration = new Vector3(10,10,0);


            //update the velocity
            //Velocity += acceleration;

            ////make sure vehicle does not exceed maximum velocity
            //Velocity.Truncate(MaxSpeed);

            ////update the position
            //Position += Velocity;
            
            ////if the vehicle has a non zero velocity the heading and side vectors must 
            ////be updated
            //if (!Velocity.IsZero())
            //{
            //    Heading = Velocity;
            //    Heading.Normalize();
            //    Side = Heading.Perp();
            //}

            GameCommand cmd = this.Controller.GetMove();
            GameCommands keyPressed = cmd.Command;
            if (keyPressed == GameCommands.Up)
            {

                //Vector3 temp = new Vector3(-1 * Math.Sin(Angle), Math.Cos(Angle), 0);
                Velocity = Velocity - (deltaTime * acceleration * -1 * Heading);
                //newPosition = new Vector() { X = currentPosition.X, Y = currentPosition.Y - Speed };
                //Direction = 90;
            }
            else if (keyPressed == GameCommands.Down)
            {
                Velocity = Velocity - (deltaTime * acceleration * Heading);
            }
            else if (keyPressed == GameCommands.Left)
            {
                //Rotate(-RotationSpeed); 
                Velocity = Velocity - (deltaTime * acceleration * -1 * Heading.PerpCW());
            }
            else if (keyPressed == GameCommands.Right)
            {
                //Rotate(RotationSpeed);
                Velocity = Velocity - (deltaTime * acceleration * -1 * Heading.PerpCCW());
            }
            else if (keyPressed == GameCommands.Space)
            {
                //RotateTowardPosition(new Vector(500, 500));
            }
            else if (keyPressed == GameCommands.MouseMove) {
                Console.WriteLine("YAYAYAYAY");
                RotateTowardPosition(new Vector3(((System.Windows.Point)cmd.Additional).X, ((System.Windows.Point)cmd.Additional).Y, 0));
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
            //Console.WriteLine("Position: {0}", Position);
            //Console.WriteLine("Velocity: {0}", Velocity);
            //Console.WriteLine("Heading: {0}", Heading);
            //Console.WriteLine("deltaTime: {0}", deltaTime);
            //Console.WriteLine("Angle: {0}", Angle);

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



            //Velocity *= .9;
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
