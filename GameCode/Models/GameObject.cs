using System.ComponentModel;
using System.Drawing;
using GameCode.Helpers;

namespace GameCode.Models
{
    /// <summary>
    /// Represents all the displayable entities in the game
    /// </summary>
    public abstract class GameObject : INotifyPropertyChanged
    {
        // Since we are using WPF, we are adding display stuff to the model.
        // Stupid WPF
        public event PropertyChangedEventHandler PropertyChanged;
        public void FirePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        
        /// <summary>
        /// If the object still exists in the game
        /// </summary>
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

        /// <summary>
        /// The rotation angle for the object, in degrees with 0 pointing to the right
        /// </summary>
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

        /// <summary>
        /// The type of object, cause polymorphism just isnt good enough 
        /// </summary>
        private int _ClassType;
        public int ClassType
        {
            get { return _ClassType; }
            set
            {
                _ClassType = value;
                this.FirePropertyChanged("ClassType");
            }
        }

        /// <summary>
        /// A reference back to the game manager so the object can interact with the game
        /// </summary>
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

        /// <summary>
        /// The position of the object
        /// </summary>
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

        /// <summary>
        /// The size of the object
        /// </summary>
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

        /// <summary>
        /// The team this object is on
        /// </summary>
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

        /// <summary>
        /// The ID number for this object
        /// </summary>
        private static int NextID = 0;
        private int _ID;
        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }


        public GameObject(Vector3 position, GameManager manager, Vector3 size)
        {
            Alive = true;
            Angle = 0;
            ID = NextID++;
            Manager = manager;
            Position = position;
            Size = size;
            Team = GameConstants.TEAM_INT_NONE;
        }

        /// <summary>
        /// Lets cheat and use rectangles for the collision detection
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public bool CollidesWith(GameObject o)
        {
            Rectangle r1 = new Rectangle((int)Position.x, (int)Position.y, (int)Size.x, (int)Size.y);
            Rectangle r2 = new Rectangle((int)o.Position.x, (int)o.Position.y, (int)o.Size.x, (int)o.Size.y);
            return !((r1.Bottom < r2.Top) || (r1.Top > r2.Bottom) || (r1.Left > r2.Right) || (r1.Right < r2.Left));
        }

        /// <summary>
        /// Changes the angle of the object
        /// </summary>
        /// <param name="angleChange"></param>
        protected void Rotate(double angleChange)
        {
            Angle += angleChange;
            Angle %= 360;
        }

        /// <summary>
        /// Update the object, called from the update loop
        /// </summary>
        /// <param name="deltaTime"></param>
        public abstract void Update(double deltaTime);
    }
}
