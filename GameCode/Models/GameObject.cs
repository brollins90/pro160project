﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.ComponentModel;

namespace GameCode.Models
{
    public abstract class GameObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void FirePropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
            {
                Console.WriteLine("ColorSelectorModel.FirePropertyChanged({0})", propertyName);
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private AttackType _AttackType;
        public AttackType AttackType
        {
            get { return _AttackType; }
            set { _AttackType = value;
            this.FirePropertyChanged("AttackType");
            }
        }

        private Controller _Controller;

        public Controller Controller
        {
            get { return _Controller; }
            set { _Controller = value; }
        }
        

        private int _Damage;
        public int Damage
        {
            get { return _Damage; }
            set { _Damage = value; }
        }

        private float _Direction;

        public float Direction
        {
            get { return _Direction; }
            set { _Direction = value; }
        }

        
        private int _Health;
        public int Health
        {
            get { return _Health; }
            set { _Health = value; }
        }

        private int _Height;

        public int Height
        {
            get { return _Height; }
            set { _Height = value; }
        }
        

        private MoveType _MoveType;
        public MoveType MoveType
        {
            get { return _MoveType; }
            set { _MoveType = value; }
        }

        private Point _Position;
        public Point Position
        {
            get { return _Position; }
            set
            {
                _Position = value;
                this.FirePropertyChanged("Position");
            }
        }

        private int _Speed;
        public int Speed
        {
            get { return _Speed; }
            set { _Speed = value; }
        }

        private int _Width;

        public int Width
        {
            get { return _Width; }
            set { _Width = value; }
        }

        private static int NextID = 0;
        private int _UniqueID;
        public int UniqueID
        {
            get { return _UniqueID; }
            private set { _UniqueID = value; }
        }

        public GameObject(
            Point position,
            AttackType attackType = AttackType.Melee,
            int damage = 2,
            float direction = 90f,
            int health = 10,
            int height = 50,
            MoveType moveType = MoveType.Walk,
            int speed = 5,
            int width = 30
            )
        {
            AttackType = attackType;
            Damage = damage;
            Direction = direction;
            Health = health;
            Height = height;
            MoveType = moveType;
            Position = position;
            Speed = speed;
            Width = width;
            UniqueID = NextID++;
        }


        public abstract void Update();

        internal bool CollidesWith(GameObject o)
        {
            Rectangle r1 = new Rectangle(Position.X, Position.Y, Width, Height);
            Rectangle r2 = new Rectangle(o.Position.X, o.Position.Y, o.Width, o.Height);
            return !((r1.Bottom < r2.Top) || (r1.Top > r2.Bottom) || (r1.Left > r2.Right) || (r1.Right < r2.Left));
        }
    }
}
