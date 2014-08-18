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
        private BotClass _BotClass;

        public BotClass BotClass
        {
            get { return _BotClass; }
            set { _BotClass = value; }
        }

        public Bot(int speed, int health, int attackDamage, int team, Vector position, GameManager manager)
            : base(position, manager)
        {
            this.Speed = speed;
            this._Team = team;
            this.Health = health;
        }

        public Bot(Vector position, GameManager manager, BotClass type = Models.BotClass.Melee)
            : base(position, manager)
        {
            switch (type)
            {
                case Models.BotClass.Boss:
                    //this.AttackDamage;
                    //this.AttackType;
                    //this.BotClass = type;
                    //this.Controller;
                    //this.Damage;
                    //this.Direction;
                    //this.Health;
                    //this.Height;
                    //this.MoveType;
                    //this.Position;
                    //this.Speed;
                    //this.Team;
                    //this.UniqueID;
                    //this.Width;
                    break;
                case Models.BotClass.Melee:
                    break;
                case Models.BotClass.Mercenary: // Sentry
                    break;
                case Models.BotClass.Shooter: // ???
                    break;
                case Models.BotClass.Tower: // Need to kill this to win
                    break;
                case Models.BotClass.Turret: // stationary
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

        public override void Update(int deltaTime)
        {
            Position = new Vector(Position.X + Speed, Position.Y + Speed);
        }
    }
}

