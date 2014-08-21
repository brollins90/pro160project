﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GameCode.Models
{
    public enum BotClass { Shooter, Melee, Boss, Tower, Turret, Mercenary };

    //The main class for all Non Playable Characters. The information on them is based off of the information passed through the constructor.
    public class Bot : GameObject, IMovingObject, IAttackingObject
    {
        public ObjectType _objectType;
        public ObjectType objectType
        {
            get { return _objectType; }
            set { _objectType = value; }
        }

        private BotClass _BotClass;
        public BotClass BotClass
        {
            get { return _BotClass; }
            set { _BotClass = value; }
        }

        public Bot(int speed, int health, int attackDamage, int team, Vector position, GameManager manager, BotClass botClass, ObjectType objectType)
            : base(position, manager)
        {
            this.Speed = speed;
            this._Team = team;
            this.Health = health;
            this.objectType = ObjectType.Bot;
            this.BotClass = BotClass.Melee;
        }

        public Bot(Vector position, GameManager manager, BotClass botClass = Models.BotClass.Melee, ObjectType objectType = ObjectType.Bot)
            : base(position, manager)
        {
            this.objectType = ObjectType.Bot;
            this.Team = 5;
            switch (botClass)
            {
                case Models.BotClass.Boss:
                    //this.Controller;
                    //this.MoveType;
                    //this.UniqueID;
                    this.Speed = 4;
                    this.Health = 100;
                    this.Width = 50;
                    this.Height = 50;
                    this.Damage = 40;
                    this.AttackType = Melee;
                    break;
                case Models.BotClass.Melee:
                    this.Speed = 3;
                    this.Health = 25;
                    this.Width = 20;
                    this.Height = 20;
                    this.Damage = 20;
                    this.AttackType = Melee;
                    break;
                case Models.BotClass.Mercenary: // Sentry
                    this.Speed = 2;
                    this.Health = 50;
                    this.Width = 30;
                    this.Height = 30;
                    this.Damage = 30;
                    this.AttackType = Melee;
                    break;
                case Models.BotClass.Shooter: // ???
                    this.Speed = 2;
                    this.Health = 10;
                    this.Width = 20;
                    this.Height = 20;
                    this.Damage = 15;
                    this.AttackType = Ranged;
                    break;
                case Models.BotClass.Tower: // Need to kill this to win
                    this.Speed = 0;
                    this.Health = 75;
                    this.Width = 75;
                    this.Height = 75;
                    this.Damage = 0;
                    break;
                case Models.BotClass.Turret: // stationary
                    this.Speed = 0;
                    this.Health = 75;
                    this.Width = 60;
                    this.Height = 60;
                    this.Damage = 50;
                    this.AttackType = Ranged;
                    break;
            }
        }

        public void Attack(Vector destination)
        {
            throw new NotImplementedException();
        }

        public void Move(Vector destination)
        {
            throw new NotImplementedException();
        }

        private int temp = 0;
        public override void Update(int deltaTime)
        {
            GameObject objToProcess = this;
            Vector currentPosition = this.Position;
            Vector newPosition = currentPosition;

            if (Health <= 0)
            {
                Alive = false;
            }

            temp++;
            if (temp < 50) {
                Position = new Vector(Position.X + Speed, Position.Y + Speed);            
            }
            else if (temp < 100)
            {
                Position = new Vector(Position.X - Speed, Position.Y + Speed);
            }
            else if (temp < 150)
            {
                Position = new Vector(Position.X - Speed, Position.Y - Speed);
            }
            else if (temp < 200)
            {
                Position = new Vector(Position.X + Speed, Position.Y - Speed);
            }
            else
            {
                temp = 0;
            }

            bool collided = false;
            foreach (GameObject o in Manager.World.Objects)
            {
                if (objToProcess.UniqueID != o.UniqueID && objToProcess.CollidesWith(o))
                    collided = true;
            }
            if (collided)
            {
                objToProcess.Position = currentPosition;
                if (objToProcess.objectType == ObjectType.Bot)
                {

                        Health -= 20;
         
                }
            } 
        }

        public Models.AttackType Melee { get; set; }

        public Models.AttackType Ranged { get; set; }
    }
}

