using System;
using System.Windows.Media;
using GameCode.Helpers;
using GameCode.Models.Projectiles;

namespace GameCode.Models
{
    /// <summary>
    /// Any object in the game that can move
    /// </summary>
    public abstract class MovingObject : GameObject
    {
        /// <summary>
        /// The acceleration speed
        /// </summary>
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

        /// <summary>
        /// The speed the object slows down
        /// </summary>
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

        /// <summary>
        /// The direction vector
        /// </summary>
        public Vector3 Heading
        {
            get
            {
                double a = Math.PI * Angle / 180.0;
                return new Vector3(Math.Cos(a), Math.Sin(a), 0);
            }
        }

        /// <summary>
        /// The most the object can turn in one update cycle
        /// </summary>
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

        /// <summary>
        /// The velocity vector
        /// </summary>
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
            MaxTurnRate = 300;
            Velocity = new Vector3(0, 0, 0);
        }

        /// <summary>
        /// Rotates the object toward a specified vector
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        internal bool RotateTowardPosition(Vector3 target)
        {
            Vector3 toTarget = (target - Position);
            toTarget = toTarget.Normalize();
            double dot = Heading.DotProduct(toTarget);

            double angleToTarget = Math.Acos(dot);

            if (angleToTarget < 0.05) // facing toward the vector
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

        /// <summary>
        /// Changes the velocity of the object in the direction it is facing
        /// </summary>
        /// <param name="deltaTime"></param>
        public void MoveForward(double deltaTime)
        {
            Velocity += (Heading * Acceleration * deltaTime);
        }

        /// <summary>
        /// Changes the velocity of the object opposite the direction it is facing
        /// </summary>
        /// <param name="deltaTime"></param>
        public void MoveBackward(double deltaTime)
        {
            Velocity -= (Heading * Acceleration * deltaTime);
        }

        /// <summary>
        /// Changes the velocity of the object to the left
        /// </summary>
        /// <param name="deltaTime"></param>
        public void MoveLeft(double deltaTime)
        {
            Velocity += (Heading.PerpCW() * Acceleration * deltaTime);
        }

        /// <summary>
        /// Changes the velocity of the object to the right
        /// </summary>
        /// <param name="deltaTime"></param>
        public void MoveRight(double deltaTime)
        {
            Velocity -= (Heading.PerpCW() * Acceleration * deltaTime);
        }

        /// <summary>
        /// We always update the position here.  Sub objects can handle the Velocity update themselves
        /// </summary>
        /// <param name="deltaTime"></param>
        public override void Update(double deltaTime)
        {
            // add some natural breaking forces
            Velocity *= BreakingSpeed;

            // update position that we already calculated
            Position = Position + Velocity;
        }
    }
}
