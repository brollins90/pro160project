using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace GameCode.Helpers
{
    public static class VectorHelpers
    {
        public static int Sign(this Vector3 v, Vector3 v2)
        {
            if (v.y * v2.x > v.x * v2.y)
            {
                return -1; // anticlockwise
            }
            else
            {
                return 1; // clockwise
            }
        }

        //public static float DotProduct(this Vector v, Vector other)
        //{
        //    return (float)(((v.X * other.X) + (v.Y * other.Y)));
        //}

        public static Vector3 TransformVector2Ds(this Matrix m, Vector3 vPoint)
        {
            double tempX = (m.M11 * vPoint.x) + (m.M21 * vPoint.y) + (0);
            double tempY = (m.M12 * vPoint.x) + (m.M22 * vPoint.y) + (0);
            vPoint.x = tempX;
            vPoint.y = tempY;
            return vPoint;
        }

        //public static bool IsZero(this Vector v)
        //{
        //    return (v.X == 0 && v.Y == 0);
        //}

        //public static Vector Perp(this Vector v)
        //{
        //    return new Vector((-1 * v.Y), v.X);
        //}

        //public static void Truncate(this Vector v, double max)
        //{
        //    if (v.Length > max)
        //    {
        //        v.Normalize();
        //        v *= max;
        //    }
        //}
        //public static double DegreeToRad(this double d)
        //{
        //    return Math.PI * d / 180.0;
        //}
    }
}
