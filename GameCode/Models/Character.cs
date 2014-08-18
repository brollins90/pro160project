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
    public class Character : GameObject, IMovingObject, IAttackingObject, INotifyPropertyChanged
    {
        public int _Team;
        public int Team
        {
            get { return _Team; }
            set { _Team = value; }
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

        private int _CurrentHealth;
        public int CurrentHealth
        {
            get { return _CurrentHealth; }
            set
            {
                _CurrentHealth = value;
                this.FirePropertyChanged("CurrentHealth");
            }
        }

        private int _MaxHealth;
        public int MaxHealth
        {
            get { return Constitution * 20; }
            set { _MaxHealth = Constitution * 20;
            this.FirePropertyChanged("MaxHealth");
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

        public Character(Vector position, GameManager manager)
            : base(position, manager)
        {
            Constitution = 5;
            Defense = 6;
            Experience = 0;
            Level = 1;
            Strength = 3;
            ExperienceCap = 100;
            CurrentHealth = 100;
            Gold = 0;
        }

        public void Attack(Vector destination)
        {
            throw new NotImplementedException();
        }

        public void Move(Vector destination)
        {
            throw new NotImplementedException();
        }

        public void LevelUp()
        {
            this.Level += 1;
            this.Strength += 1;
            this.Constitution += 2;
            this.Experience = 0;
            this.ExperienceCap += 30;
            this.MaxHealth = Constitution * 20;
            this.CurrentHealth = MaxHealth;
            this.Gold += 100;

            if (this.Level % 3 == 0)
            {
                this.Defense += 1;
                this.Gold += 150;
            }           
           
        }



        public override void Update()
        {
            Console.WriteLine("Character.Update()");
            GameObject objToProcess = this;
            Vector currentPosition = this.Position;
            Vector newPosition = currentPosition;

            GameCommands keyPressed = this.Controller.GetMove();
            if (keyPressed == GameCommands.Up)
            {
                newPosition = new Vector() { X = currentPosition.X, Y = currentPosition.Y - objToProcess.Speed };
                objToProcess.Direction = 90;
            }
            else if (keyPressed == GameCommands.Down)
            {
                newPosition = new Vector() { X = currentPosition.X, Y = currentPosition.Y + objToProcess.Speed };
                objToProcess.Direction = 270;
            }
            else if (keyPressed == GameCommands.Left)
            {
                newPosition = new Vector() { X = currentPosition.X - objToProcess.Speed, Y = currentPosition.Y };
                objToProcess.Direction = 180;
            }
            else if (keyPressed == GameCommands.Right)
            {
                newPosition = new Vector() { X = currentPosition.X + objToProcess.Speed, Y = currentPosition.Y };
                objToProcess.Direction = 0;
            }
            else if (keyPressed == GameCommands.Space)
            {
                Console.WriteLine("recieved a space");
                //if (objToProcess.AttackType == AttackType.Ranged)
                //{
                Manager.AddProjectile(new GameProjectile(currentPosition, this.Manager, objToProcess.Direction, 25, objToProcess.Damage)
                {
                    Height = 10,
                    Width = 10,
                    Controller = new Controller()

                });
                //}
            }
            objToProcess.Position = newPosition;

            bool collided = false;
            foreach (GameObject o in Manager.World.Objects)
            {
                if (objToProcess.UniqueID != o.UniqueID && objToProcess.CollidesWith(o))
                    collided = true;
            }
            if (collided)
            {
                objToProcess.Position = currentPosition;
                Console.WriteLine("Collided");
            }
        }
    }
}
