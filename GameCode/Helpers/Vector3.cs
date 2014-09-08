using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCode.Helpers
{
    public struct Vector3
    {
        public double x { get; set; }
        public double y { get; set; }
        public double z { get; set; }

        public Vector3(double X = 0.0, double Y = 0.0, double Z = 0.0)
            : this()
        {
            x = X;
            y = Y;
            z = Z;
        }

        public static Vector3 operator +(Vector3 lhs, Vector3 rhs) { return new Vector3(lhs.x + rhs.x, lhs.y + rhs.y, lhs.z + rhs.z); }
        public static Vector3 operator -(Vector3 lhs, Vector3 rhs) { return new Vector3(lhs.x - rhs.x, lhs.y - rhs.y, lhs.z - rhs.z); }
        public static Vector3 operator *(Vector3 lhs, Vector3 rhs) { return new Vector3(lhs.x * rhs.x, lhs.y * rhs.y, lhs.z * rhs.z); }
        public static Vector3 operator *(Vector3 lhs, double rhs) { return new Vector3(lhs.x * rhs, lhs.y * rhs, lhs.z * rhs); }
        public static Vector3 operator *(double lhs, Vector3 rhs) { return new Vector3(lhs * rhs.x, lhs * rhs.y, lhs * rhs.z); }
        public static Vector3 operator /(Vector3 lhs, Vector3 rhs)
        {
            if (rhs.x != 0 && rhs.y != 0 && rhs.z != 0)
            {
                return new Vector3(lhs.x / rhs.x, lhs.y / rhs.y, lhs.z / rhs.z);
            }
            else
            {
                return new Vector3(0, 0, 0);
            }
        }
        public static Vector3 operator /(Vector3 lhs, float rhs)
        {
            if (rhs != 0)
            {
                return new Vector3(lhs.x / rhs, lhs.y / rhs, lhs.z / rhs);
            }
            else
            {
                return new Vector3(0, 0, 0);
            }
        }
        public double CrossProduct(Vector3 lhs, Vector3 rhs) { return ((lhs.x * rhs.y) - (lhs.y * rhs.x)); }
        public double DotProduct(Vector3 rhs) { return ((x * rhs.x) + (y * rhs.y)); }
        //        public double DotProduct(Vector3 lhs, Vector3 rhs) { return ((lhs.x * rhs.x) + (lhs.y * rhs.y)); }
        public bool IsZero() { return x == 0 && y == 0 && z == 0; }
        public double Length() { return Math.Sqrt(x * x + y * y); }
        public double LengthSquared() { return (x * x + y * y); }
        public Vector3 Negate() { return new Vector3((-1 * x), (-1 * y), (-1 * z)); }
        public Vector3 Normalize()
        {
            Vector3 vect = new Vector3();
            double length = this.Length();
            if (length != 0)
            {
                vect.x = x / length;
                vect.y = y / length;
                vect.z = z / length;
            }
            return vect;
        }
        public Vector3 PerpCW() { return new Vector3(y, (-1 * x)); }
        public Vector3 PerpCCW() { return new Vector3((-1 * y), x); }
        public Vector3 ScalarMultiply(double scalar) { return new Vector3(x * scalar, y * scalar, z * scalar); }

        public override string ToString()
        {
            return string.Format("{0:0.00}, {1:0.00}, {2:0.00}", x, y, z);
        }
        public int Sign(Vector3 v2)
        {
            if (this.y * v2.x > this.x * v2.y)
            {
                return -1; // anticlockwise
            }
            else
            {
                return 1; // clockwise
            }
        }

    }
}
