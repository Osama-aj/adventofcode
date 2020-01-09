using System;


namespace day1
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] lines = System.IO.File.ReadAllLines(@"/home/osse/Downloads/input.txt");

            double resultPart2 = CalcAllFuelPart2(lines);
            double resultPart1 = CalcAllFuelPart1(lines);

            Console.WriteLine("part 1 : " + resultPart1);
            Console.WriteLine("part 2 : " + resultPart2);
            Console.ReadKey();
        }


        private static double CalcAllFuelPart2(string[] lines)
        {
            double sumLines = 0;
            foreach (var line in lines)
                sumLines += CalcOneLine(double.Parse(line));

            return sumLines;
        }


        private static double CalcOneLine(double line)
        {
            line = CalcFuelEquation(line);
            if (line <= 0)
                return 0;

            return CalcOneLine(line) + line;
        }

        private static double CalcFuelEquation(double line)
        {
            return (Math.Floor(line / 3)) - 2;
        }

        static double CalcAllFuelPart1(string[] lines)
        {
            double sum = 0;
            foreach (var line in lines)
                sum += CalcFuelEquation(double.Parse(line));

            return sum;
        }
    }
}
