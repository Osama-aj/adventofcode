using System;

namespace day4
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            const int from = 402328;
            const int to = 864247;
            Part1(from, to);
            Part2(from, to);
            Console.ReadKey();
        }

        //part2 functions
        private static void Part2(int from, int to)
        {
            double counter = 0;

            for (var num = from; num <= to; num++)
            {
                var isIncreasing = IsItIncreasing(num);
                var hasTwoAdjacentDigits = IsThereAPairPart2(num, isIncreasing);

                //counting 
                if (hasTwoAdjacentDigits && isIncreasing)
                    counter++;
            }

            Console.WriteLine("part 2 : " + counter);
        }


        private static bool IsThereAPairPart2(int num, bool isItIncreasing)
        {
            //if it's not increasing, there is no meaning to test the second condition.
            if (!isItIncreasing)
                return false;


            var hasTwoAdjacentDigits = false;

            var numbers = new Numbers(0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            var iStr = num.ToString().ToCharArray();

            //look if there i a pair of any number
            foreach (var number in iStr)
            {
                switch (number)
                {
                    case '0':
                        numbers.Zero++;
                        break;
                    case '1':
                        numbers.One++;
                        break;
                    case '2':
                        numbers.Two++;
                        break;
                    case '3':
                        numbers.Three++;
                        break;
                    case '4':
                        numbers.Four++;
                        break;
                    case '5':
                        numbers.Five++;
                        break;
                    case '6':
                        numbers.Six++;
                        break;
                    case '7':
                        numbers.Seven++;
                        break;
                    case '8':
                        numbers.Eight++;
                        break;
                    case '9':
                        numbers.Nine++;
                        break;
                    default:
                        Console.WriteLine("Something went wrong!");
                        break;
                }
            }

            if (numbers.Zero == 2
                || numbers.One == 2
                || numbers.Two == 2
                || numbers.Three == 2
                || numbers.Four == 2
                || numbers.Five == 2
                || numbers.Six == 2
                || numbers.Seven == 2
                || numbers.Eight == 2
                || numbers.Nine == 2)
            {
                hasTwoAdjacentDigits = true;
            }

            return hasTwoAdjacentDigits;
        }


        private class Numbers
        {
            public int Zero, One, Two, Three, Four, Five, Six, Seven, Eight, Nine;

            public Numbers(int zero, int one, int two, int three, int four, int five, int six, int seven, int eight,
                int nine)
            {
                this.Zero = zero;
                this.One = one;
                this.Two = two;
                this.Three = three;
                this.Four = four;
                this.Five = five;
                this.Six = six;
                this.Seven = seven;
                this.Eight = eight;
                this.Nine = nine;
            }
        }

        //part1 functions
        private static void Part1(int from, int to)
        {
            double counter = 0;

            for (var num = from; num <= to; num++)
            {
                var isIncreasing = IsItIncreasing(num);
                var hasTwoAdjacentDigits = IsThereAPairPart1(num);

                if (isIncreasing && hasTwoAdjacentDigits)
                    counter++;
            }

            Console.WriteLine("part 1 :" + counter);
        }


        private static bool IsThereAPairPart1(int num)
        {
            var hasTwoAdjacentDigits = false;
            var iStr = num.ToString().ToCharArray();
            for (var j = 0; j < iStr.Length; j++)
            for (var k = 0; k < iStr.Length; k++)
            {
                if (iStr[j] != iStr[k] || j == k) continue;
                hasTwoAdjacentDigits = true;
                break;
            }

            return hasTwoAdjacentDigits;
        }


        //shared
        private static bool IsItIncreasing(int num)
        {
            var isIncreasing = true;
            var iStr = num.ToString().ToCharArray();
            var holder = iStr[0];
            for (var j = 0; j < iStr.Length - 1; j++)
                if (holder <= iStr[j + 1])
                    holder = iStr[j + 1];
                else
                {
                    isIncreasing = false;
                    break;
                }

            return isIncreasing;
        }
    }
}