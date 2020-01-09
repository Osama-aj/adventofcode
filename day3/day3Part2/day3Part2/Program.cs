using System;
using System.Collections.Generic;


namespace day3Part2
{
    internal class Program
    {
        /// <summary>
        /// the point model 
        /// </summary>
        private class Point
        {
            public long X, Y;

            public Point(long x, long y)
            {
                this.X = x;
                this.Y = y;
            }
        }


        /// <summary>
        /// find all points in the given input
        /// </summary>
        /// <param name="wirePointsList"></param>
        /// <returns></returns>
        private static IList<Point> FindPoints(string[] wirePointsList)
        {
            IList<Point> points = new List<Point>();

            Point currentPoint = new Point(0, 0);
            foreach (var point in wirePointsList)
                if (point[0] == 'R')
                {
                    var temp = point.Substring(1);
                    for (var i = 0; i < int.Parse(temp); i++)
                    {
                        currentPoint.X++;
                        points.Add(new Point(currentPoint.X, currentPoint.Y));
                    }
                }
                else if (point[0] == 'L')
                {
                    var temp = point.Substring(1);
                    for (var i = 0; i < int.Parse(temp); i++)
                    {
                        currentPoint.X--;
                        points.Add(new Point(currentPoint.X, currentPoint.Y));
                    }
                }
                else if (point[0] == 'U')
                {
                    var temp = point.Substring(1);
                    for (var i = 0; i < int.Parse(temp); i++)
                    {
                        currentPoint.Y++;
                        points.Add(new Point(currentPoint.X, currentPoint.Y));
                    }
                }
                else if (point[0] == 'D')
                {
                    var temp = point.Substring(1);
                    for (var i = 0; i < int.Parse(temp); i++)
                    {
                        currentPoint.Y--;
                        points.Add(new Point(currentPoint.X, currentPoint.Y));
                    }
                }
                else
                    Console.WriteLine("something went wrong");

            return points;
        }


        /// <summary>
        /// Main ... the driver 
        /// </summary>
        /// <param name="args"></param>
        private static void Main(string[] args)
        {
            var wire1 = System.IO.File.ReadAllText(@"/home/osse/Downloads/input1.txt");
            var wire2 = System.IO.File.ReadAllText(@"/home/osse/Downloads/input2.txt");
            var wire1List = wire1.Split(',');
            var wire2List = wire2.Split(',');

            //all points for the first wire
            IList<Point> points1 = FindPoints(wire1List);
            //all points for the second wire
            IList<Point> points2 = FindPoints(wire2List);

            Console.WriteLine("use the first intersection point because it is always the shortest.");


            var fewestSteps = long.MaxValue;
            for (var i = 0; i < points1.Count; i++)
            {
                var secondWireCounter = 0;
                foreach (var point2 in points2)
                {
                    secondWireCounter++;
                    if (points1[i].X != point2.X || points1[i].Y != point2.Y) continue;
                    long sum = i + 1 + secondWireCounter;

                    if (sum < fewestSteps)
                        fewestSteps = sum;
                }
            }

            Console.WriteLine("fewest steps : " + fewestSteps);
            Console.ReadKey();
        }
    }
}