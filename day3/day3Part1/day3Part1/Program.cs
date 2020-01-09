using System;
using System.Collections.Generic;


namespace day3Part1
{
    class Program
    {
        /// <summary>
        /// the point model 
        /// </summary>
        public class Point
        {
            public double X, Y;

            public Point(double x, double y)
            {
                this.X = x;
                this.Y = y;
            }
        }


        /// <summary>
        /// a function to find intersection points
        /// </summary>
        public static Point LineLineIntersection(Point A, Point B, Point C, Point D)
        {
            // Line AB represented as a1x + b1y = c1  
            double a1 = B.Y - A.Y;
            double b1 = A.X - B.X;
            double c1 = a1 * (A.X) + b1 * (A.Y);

            // Line CD represented as a2x + b2y = c2  
            double a2 = D.Y - C.Y;
            double b2 = C.X - D.X;
            double c2 = a2 * (C.X) + b2 * (C.Y);

            double determinant = a1 * b2 - a2 * b1;

            if (determinant == 0)
            {
                // The lines are parallel. This is simplified  
                // by returning a pair of FLT_MAX  
                return new Point(double.MaxValue, double.MaxValue);
            }
            else
            {
                double x = (b2 * c1 - b1 * c2) / determinant;
                double y = (a1 * c2 - a2 * c1) / determinant;
                if (Math.Min(A.X, B.X) <= x && x <= Math.Max(A.X, B.X) && Math.Min(A.Y, B.Y) <= y &&
                    y <= Math.Max(A.Y, B.Y)
                    && Math.Min(C.X, D.X) <= x && x <= Math.Max(C.X, D.X) && Math.Min(C.Y, D.Y) <= y &&
                    y <= Math.Max(C.Y, D.Y))
                {
                    return new Point(x, y);
                }

                return new Point(double.MaxValue, double.MaxValue);
            }
        }

        /// <summary>
        /// find all points in the given input
        /// </summary>
        /// <param name="wirePointsList"></param>
        /// <returns></returns>
        public static IList<Point> FindPoints(string[] wirePointsList)
        {
            List<Point> points = new List<Point>();

            Point currentPoint = new Point(0, 0);
            foreach (var point in wirePointsList)
                if (point[0] == 'R')
                {
                    string temp = point.Substring(1);
                    currentPoint.X += int.Parse(temp);

                    points.Add(new Point(currentPoint.X, currentPoint.Y));
                }
                else if (point[0] == 'L')
                {
                    string temp = point.Substring(1);
                    currentPoint.X -= int.Parse(temp);

                    points.Add(new Point(currentPoint.X, currentPoint.Y));
                }
                else if (point[0] == 'U')
                {
                    string temp = point.Substring(1);
                    currentPoint.Y += int.Parse(temp);

                    points.Add(new Point(currentPoint.X, currentPoint.Y));
                }
                else if (point[0] == 'D')
                {
                    string temp = point.Substring(1);
                    currentPoint.Y -= int.Parse(temp);

                    points.Add(new Point(currentPoint.X, currentPoint.Y));
                }
                else
                    Console.WriteLine("something went wrong");

            return points;
        }


        /// <summary>
        /// Main ... the driver 
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            var wire1 = System.IO.File.ReadAllText(@"/home/osse/Downloads/input1.txt");
            var wire2 = System.IO.File.ReadAllText(@"/home/osse/Downloads/input2.txt");
            var wire1List = wire1.Split(',');
            var wire2List = wire2.Split(',');

            //all points for the first wire
            IList<Point> points1 = FindPoints(wire1List);
            //all points for the second wire
            IList<Point> points2 = FindPoints(wire2List);


            //calculating all lines to find all intersection points
            IList<Point> intersectionPoints = new List<Point>();
            for (int i = 0; i < points1.Count - 1; i++)
            for (int j = 0; j < points2.Count - 1; j++)
            {
                Point intersection = LineLineIntersection(points1[i], points1[i + 1], points2[j], points2[j + 1]);
                if (intersection.X != double.MaxValue && intersection.Y != double.MaxValue)
                {
                    intersectionPoints.Add(new Point(intersection.X, intersection.Y));
                }
            }

            //findig the shortest distans using the manhatten equation 
            var shortestDistance = new Point(double.MaxValue, double.MaxValue);
            foreach (var interPoint in intersectionPoints)
            {
                double manhattanDistance = Math.Abs(0 - interPoint.X) + Math.Abs(0 - interPoint.Y);
                if (manhattanDistance < Math.Abs(0 - shortestDistance.X) + Math.Abs(0 - shortestDistance.Y))
                    shortestDistance = interPoint;
            }

            Console.WriteLine("the shortest manhattan distant is = " +
                              (Math.Abs(shortestDistance.X) + Math.Abs(shortestDistance.Y)));
            Console.ReadKey();
        }
    }
}