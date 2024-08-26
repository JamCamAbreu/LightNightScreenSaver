using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HPScreen.Admin
{
    public static class Global
    {
        public static int IntSqrt(int num)
        {
            if (0 == num) { return 0; }  // Avoid zero divide  
            int n = (num / 2) + 1;       // Initial estimate, never low  
            int n1 = (n + (num / n)) / 2;
            while (n1 < n)
            {
                n = n1;
                n1 = (n + (num / n)) / 2;
            } // end while  
            return n;
        }
        public static double DegreesToRadians(double angleInDegrees)
        {
            return (Math.PI / 180) * angleInDegrees;
        }
        public static double RadiansToDegrees(double radians)
        {
            return radians * (180.0 / Math.PI);
        }
        public static int ApproxDist(int x1, int y1, int x2, int y2)
        {
            return IntSqrt((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1)); // Pythagorean brah.
        }
        public static int ApproxDist(Vector2 from, Vector2 to)
        {
            return IntSqrt((int)(((to.X - from.X) * (to.X - from.X)) + ((to.Y - from.Y) * (to.Y - from.Y)))); // Pythagorean brah.
        }
        public static float Ease(float curVal, float targetVal, float speed)
        {
            return curVal + (targetVal - curVal) * speed;
        }
        public static int Ease(int curVal, int targetVal, float speed)
        {
            return (int)(curVal + (targetVal - curVal) * speed);
        }
        public static double Ease(double curVal, double targetVal, float speed)
        {
            return (double)(curVal + (targetVal - curVal) * speed);
        }
        public static Vector2 Ease(Vector2 curVal, Vector2 targetVal, float speed)
        {
            return (Vector2)(curVal + (targetVal - curVal) * speed);
        }
        public static Vector3 Ease(Vector3 curVal, Vector3 targetVal, float speed)
        {
            return (Vector3)(curVal + (targetVal - curVal) * speed);
        }
        public static double VectorToDegrees(Vector2 vector)
        {
            return Math.Atan2(vector.X, vector.Y) * 180 / Math.PI;
        }
        public static double VectorToRadians(Vector2 vector)
        {
            return Math.Atan2(vector.Y, vector.X);
        }
        public static List<Vector2> GenerateRadialPoints(int originX, int originY, Vector2 direction, int length, float spread, int numberPoints, bool relativePosition = false)
        {
            List<Vector2> points = new List<Vector2>();
            double directionDegrees = VectorToDegrees(direction);

            float degreesEach = spread / (numberPoints - 1);
            for (int i = 0; i < numberPoints; i++)
            {
                double degs = directionDegrees - (spread / 2) + (degreesEach * i);
                double pos = Global.DegreesToRadians(degs);

                float pointx = (relativePosition ? 0 : originX) + (float)Math.Sin(pos) * length;
                float pointy = (relativePosition ? 0 : originY) + (float)Math.Cos(pos) * length;
                Vector2 pointPos = new Vector2(pointx, pointy);
                points.Add(pointPos);
            }

            return points;
        }
        public static Vector2 GetVectorBetweenTwoPoints(Vector2 from, Vector2 to, bool normalized = false)
        {
            Vector2 direction = from - to;
            if (normalized)
            {
                direction.Normalize();
            }
            return direction;
        }
        public static Vector2 GetVectorBetweenTwoPoints(float fromX, float fromY, float toX, float toY, bool normalized = false)
        {
            return GetVectorBetweenTwoPoints(new Vector2(fromX, fromY), new Vector2(toX, toY), normalized);
        }
    }
}
