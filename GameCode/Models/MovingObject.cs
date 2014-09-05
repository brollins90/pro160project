using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using GameCode;
using GameCode.Helpers;
using System.Windows.Media;
using GameCode.Models.Projectiles;

namespace GameCode.Models
{
    public abstract class MovingObject : GameObject
    {

        private Vector3 _Acceleration;
        public Vector3 Acceleration
        {
            get { return _Acceleration; }
            set
            {
                _Acceleration = value;
                this.FirePropertyChanged("Acceleration");
            }
        }

        private Vector3 _BreakingSpeed;
        public Vector3 BreakingSpeed
        {
            get { return _BreakingSpeed; }
            set
            {
                _BreakingSpeed = value;
                this.FirePropertyChanged("BreakingSpeed");
            }
        }
        
        public Vector3 Heading
        {
            get {
                double a = Math.PI * Angle / 180.0;
                return new Vector3(Math.Cos(a), Math.Sin(a), 0);
            }
        }

        private double _Mass;
        public double Mass
        {
            get { return _Mass; }
            set
            {
                _Mass = value;
                this.FirePropertyChanged("Mass");
            }
        }

        private double _MaxForce;
        public double MaxForce
        {
            get { return _MaxForce; }
            set
            {
                _MaxForce = value;
                this.FirePropertyChanged("MaxForce");
            }
        }

        private double _MaxSpeed;
        public double MaxSpeed
        {
            get { return _MaxSpeed; }
            set
            {
                _MaxSpeed = value;
                this.FirePropertyChanged("MaxSpeed");
            }
        }

        private double _MaxTurnRate;
        public double MaxTurnRate
        {
            get { return _MaxTurnRate; }
            set
            {
                _MaxTurnRate = value;
                this.FirePropertyChanged("MaxTurnRate");
            }
        }        

        //private Vector3 _Side;
        //public Vector3 Side
        //{
        //    get { return _Side; }
        //    set
        //    {
        //        _Side = value;
        //        this.FirePropertyChanged("Side");
        //    }
        //}

        //private Vector3 _Speed;
        //public Vector3 Speed
        //{
        //    get { return _Speed; }
        //    set
        //    {
        //        _Speed = value;
        //        this.FirePropertyChanged("Speed");
        //    }
        //}

        private Vector3 _Velocity;
        public Vector3 Velocity
        {
            get { return _Velocity; }
            set
            {
                _Velocity = value;
                this.FirePropertyChanged("Velocity");
            }
        }

        public MovingObject(Vector3 position, GameManager manager, Vector3 size)
            : base(position, manager, size)
        {
            Acceleration = new Vector3(1, 1, 0);
            BreakingSpeed = new Vector3(.1, .1, 0); // (1 is no breaking speed)
            Mass = 0;
            MaxForce = 0;
            MaxSpeed = 10;
            MaxTurnRate = 300;
            //Speed = new Vector3(.3, .3, 0);
            Velocity = new Vector3(0, 0, 0);
        }

        public void Rotate(double angleChange)
        {
            Angle += angleChange;
            Angle %= 360;
        }

        internal bool RotateTowardPosition(Vector3 target)
        {
            Vector3 toTarget = (target - Position);
            toTarget = toTarget.Normalize();
            double dot = Heading.DotProduct(toTarget);

            double angleToTarget = Math.Acos(dot);

            if (angleToTarget < 0.05)
            {
                return true;
            }

            // check max turn speed
            if (angleToTarget > MaxTurnRate)
            {
                angleToTarget = MaxTurnRate;
            }

            Rotate(Heading.Sign(toTarget) * angleToTarget);

            return false;
        }
        public bool MoveForward(double deltaTime)
        {
            Velocity += (Heading * Acceleration * deltaTime);
            // check collision
            // TODO
            return true;
        }
        public bool MoveBackward(double deltaTime)
        {
            Velocity -= (Heading * Acceleration * deltaTime);
            // check collision
            // TODO
            return true;
        }
        public bool MoveLeft(double deltaTime)
        {
            Velocity += (Heading.PerpCW() * Acceleration * deltaTime);
            // check collision
            // TODO
            return true;
        }
        public bool MoveRight(double deltaTime)
        {
            Velocity -= (Heading.PerpCW() * Acceleration * deltaTime);
            // check collision
            // TODO
            return true;
        }
        //public bool StopMoving(double deltaTime)
        //{
        //    Velocity = new Vector3();
        //    // check collision
        //    // TODO
        //    return true;
        //}

        /// <summary>
        /// We always update the position here.  Sub objects can handle the Velocity update themselves
        /// </summary>
        /// <param name="deltaTime"></param>
        public override void Update(double deltaTime)
        {
            // add some natural breaking forces
            Velocity *= BreakingSpeed;

            // Acceleration is static to the object and set in the constructor
            //Acceleration = Force / Mass

            //Velocity += Heading * (Acceleration * deltaTime);

            // update position that we already calculated
            Position = Position + Velocity;

            bool collided = false;
            foreach (GameObject o in Manager.World.Objects)
            {
                if (this.ID != o.ID && this.CollidesWith(o))
                    if (o.GetType() == typeof(Character))
                    {
                        // do nothing
                    }
                    else
                    {
                        collided = true;
                    }
            }
            if (collided)
            {
                if (this.Position.y >= 384)
                {
                    this.RotateTowardPosition(new Vector3(950, 700, 0));
                }
                else
                {
                    this.RotateTowardPosition(new Vector3(950, 0, 0));
                }
            }
        }
    }
}
