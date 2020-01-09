using System;
using System.Collections.Generic;


namespace day2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var commmands = System.IO.File.ReadAllText(@"/home/osse/Downloads/input.txt");
            var commandsList = commmands.Split(',');

            IList<int> IntCode = new List<int>();
            foreach (var command in commandsList)
                IntCode.Add(int.Parse(command));


            Part1(IntCode);
            Part2(IntCode);


            Console.ReadKey();
        }

        private static int Computer(IList<int> IntCode)
        {
            int instructionPointer = 0;


            while (IntCode[instructionPointer] != 99)
            {
                if (IntCode[instructionPointer] == 1)
                {
                    IntCode[IntCode[instructionPointer + 3]] =
                        IntCode[IntCode[instructionPointer + 1]] +
                        IntCode[IntCode[instructionPointer + 2]];
                }
                else if (IntCode[instructionPointer] == 2)
                {
                    IntCode[IntCode[instructionPointer + 3]] =
                        IntCode[IntCode[instructionPointer + 1]] *
                        IntCode[IntCode[instructionPointer + 2]];
                }
                else
                {
                    Console.WriteLine("Something went wrong !!");
                    break;
                }

                instructionPointer += 4;
            }

            return IntCode[0];
        }

        private static void Part1(IList<int> IntCode)
        {
            IList<int> IntCodeCopy = new List<int>(IntCode);

            IntCodeCopy[1] = 12;
            IntCodeCopy[2] = 2;
            var result = Computer(IntCodeCopy);
            Console.WriteLine("Part 1 result : " + result);
        }

        private static void Part2(IList<int> IntCode)
        {
            int result = 0;
            for (int noun = 0; noun < 99; noun++)
            {
                for (int verb = 0; verb < 99; verb++)
                {
                    IList<int> IntCodeCopy = new List<int>(IntCode);
                    IntCodeCopy[1] = noun;
                    IntCodeCopy[2] = verb;
                    var computerResult = Computer(IntCodeCopy);

                    if (computerResult == 19690720)
                    {
                        result = (100 * noun) + verb;
                        break;
                    }
                }
            }

            Console.WriteLine("Part 2 result : " + result);
        }
    }
}