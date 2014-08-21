using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using GameCode;
using GameCode.Helpers;
using System.Windows.Media;

namespace GameCode.Models
{
    public abstract class MovingObject : GameObject
    {

        private Vector3 _Acceleration;

        public Vector3 Acceleration
        {
            get { return _Acceleration; }
            set { _Acceleration = value; }
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
            MaxTurnRate = 10;
            MaxSpeed = 15;
            Acceleration = new Vector3(10, 10, 0);
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
                return true; // we are looking in the correct direction
                //Rotate(Heading.Sign(toTarget));
                //dot = Heading.DotProduct(toTarget);
                //angleToTarget = Math.Acos(dot);

            }

            // check max turn speed
            if (angleToTarget > MaxTurnRate)
            {
                angleToTarget = MaxTurnRate;
            }

            Rotate(Heading.Sign(toTarget) * angleToTarget);

            //Side = Heading.PerpCCW();

            return false;
        }

        public override void Update(double deltaTime)
        {
            Position = Position + Velocity * deltaTime;
        }

//        inline void MovingEntity::SetHeading(Vector2D new_heading)
//{
//  assert( (new_heading.LengthSq() - 1.0) < 0.00001);
  
//  m_vHeading = new_heading;

//  //the side vector must always be perpendicular to the heading
//  m_vSide = m_vHeading.Perp();
//}
    }
}
