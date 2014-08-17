using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                _CurrentHealth = value - DamageTaken;
                this.FirePropertyChanged("CurrentHealth");
            }
        }

        private int _DamageTaken;
        public int DamageTaken
        {
            get { return _DamageTaken; }
            set { _DamageTaken = value;
            this.FirePropertyChanged("DamageTaken");
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

        public Character(Point position) : base(position)
        {
            Constitution = 5;
            Defense = 6;
            Experience = 0;
            Level = 1;
            Strength = 3;
            ExperienceCap = 100;
            CurrentHealth = 100;
        }

        public void Attack(Point destination)
        {
            throw new NotImplementedException();
        }

        public void Move(Point destination)
        {
            throw new NotImplementedException();
        }

        public void LevelUp()
        {
            this.DamageTaken = 0;
            this.Level += 1;
            this.Strength += 1;
            this.Constitution += 2;
            this.Experience = 0;
            this.ExperienceCap += 30;
            this.MaxHealth = Constitution * 20;
            this.CurrentHealth = MaxHealth;

            if (this.Level % 3 == 0)
            {
                this.Defense += 1;
            }           
           
        }



        public override void Update()
        {
            return;
        }
    }
}
