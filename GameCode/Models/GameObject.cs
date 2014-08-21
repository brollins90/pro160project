using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.ComponentModel;
using System.Windows;
using GameCode.Helpers;

namespace GameCode.Models
{
    public abstract class GameObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        //public enum GameObjectType { Bot, Player, Projectile };

        public void FirePropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
            {
                //Console.WriteLine("ColorSelectorModel.FirePropertyChanged({0})", propertyName);
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }


        private bool _Alive;
        public bool Alive
        {
            get { return _Alive; }
            set
            {
                _Alive = value;
                this.FirePropertyChanged("Alive");
            }
        }

        private double _Angle;
        public double Angle
        {
            get { return _Angle; }
            set
            {
                _Angle = value;
                this.FirePropertyChanged("Angle");
            }
        }

        private Controller _Controller;
        public Controller Controller
        {
            get { return _Controller; }
            set
            {
                    _Controller = value;
                    this.FirePropertyChanged("Controller");
            }
        }

        private GameManager _Manager;
        public GameManager Manager
        {
            get { return _Manager; }
            set
            {
                    _Manager = value;
                    this.FirePropertyChanged("Manager");
            }
        }

        //public GameObjectType _ObjectType;
        //public GameObjectType ObjectType
        //{
        //    get { return _ObjectType; }
        //    set
        //    {
        //        _ObjectType = value;
        //        this.FirePropertyChanged("ObjectType");
        //    }
        //}

        private Vector3 _Position;
        public Vector3 Position
        {
            get { return _Position; }
            set
            {
                _Position = value;
                this.FirePropertyChanged("Position");
            }
        }

        private Vector3 _Size;
        public Vector3 Size
        {
            get { return _Size; }
            set
        {
                _Size = value;
                this.FirePropertyChanged("Size");
            }
        }

        public int _Team;
        public int Team
        {
            get { return _Team; }
            set
            {
                _Team = value;
                this.FirePropertyChanged("Team");
        }
        }

        private static int NextID = 0;
        private int _ID;
        public int ID
        {
            get { return _ID; }
            private set { _ID = value; }
        }

        public GameObject(
            Vector3 position,
            GameManager manager,
            Vector3 size,
            int team = 1
            )
        {
            Alive = true;
            Controller = new Controller();
            Manager = manager;
            Position = position;
            Size = size;
            Team = team;
            ID = NextID++;
            Angle = 0;
        }


        public abstract void Update(double deltaTime);

        internal bool CollidesWith(GameObject o)
        {
            Rectangle r1 = new Rectangle((int)Position.x, (int)Position.y, (int)Size.x, (int)Size.y);
            Rectangle r2 = new Rectangle((int)o.Position.x, (int)o.Position.y, (int)o.Size.x, (int)o.Size.y);
            return !((r1.Bottom < r2.Top) || (r1.Top > r2.Bottom) || (r1.Left > r2.Right) || (r1.Right < r2.Left));
        }
    }
}
