using System;
using System.Collections.Generic;

namespace day9
{
    class Program
    {
        private static void Main(string[] args)
        {
            //preparing 
            var commmands = System.IO.File.ReadAllText(@"/home/osse/Downloads/input.txt");
            var commandsList = commmands.Split(',');

            IList<long> IntCode = new List<long>();
            foreach (var command in commandsList)
                IntCode.Add(long.Parse(command));
            //////////////////////////
            
            //running the intCode on the computer
            long part1Result = Computer(IntCode, 1);
            long part2Result = Computer(IntCode, 2);

            //listing results
            Console.WriteLine("part1 output : " + part1Result);
             Console.WriteLine("part2 output : " + part2Result);
            Console.WriteLine("Done.");
            
            Console.ReadKey();
        }


        private static long Computer(IList<long> IntCode, long input)
        {
            IList<long> IntCodeCopy = new List<long>(IntCode);

            for (int i = IntCodeCopy.Count; i < 90001; i++)
                IntCodeCopy.Add(0);

            long output = 0;
            int instructionPointer = 0;
            int relativeBase = 0;
            var isRunning = true;

            while (isRunning)
            {
                ////ABCDE
                int intCommand = (int) IntCodeCopy[instructionPointer];

                int opCode = intCommand / 1 % 100;
                int parameter1 = intCommand / 100 % 10;
                int parameter2 = intCommand / 1000 % 10;
                int parameter3 = intCommand / 10000 % 10;

                //the third parameter is always 0


                long firstPValue = 0;
                long secondPValue = 0;
                int address = 0;

                if (opCode == 3)
                {
                    if (parameter1 == 2)
                        address = (int) IntCodeCopy[instructionPointer + 1]+ relativeBase;
                    else 
                        address = (int) IntCodeCopy[instructionPointer + 1];
                        

                    IntCodeCopy[address] = input;
                    instructionPointer += 2;
                }
                else if (opCode == 99)
                    isRunning = false;
                else
                {
                    if (parameter1 == 0)
                        firstPValue = IntCodeCopy[(int) IntCodeCopy[instructionPointer + 1]];
                    else if (parameter1 == 1)
                        firstPValue = IntCodeCopy[instructionPointer + 1];
                    else if (parameter1 == 2)
                        firstPValue = IntCodeCopy[(int) IntCodeCopy[instructionPointer + 1] + relativeBase];

                    switch (opCode)
                    {
                        case 4:

                            output = firstPValue;
                            instructionPointer += 2;
                            break;
                        case 9:
                            relativeBase += (int) firstPValue;
                            instructionPointer += 2;

                            break;

                        default:
                        {
                            if (parameter2 == 0)
                                secondPValue = IntCodeCopy[(int) IntCodeCopy[instructionPointer + 2]];
                            else if (parameter2 == 1)
                                secondPValue = IntCodeCopy[instructionPointer + 2];
                            else if (parameter2 == 2)
                                secondPValue = IntCodeCopy[(int) IntCodeCopy[instructionPointer + 2] + relativeBase];

                            
                            if (parameter3 == 2)
                                address = (int) IntCodeCopy[instructionPointer + 3]+ relativeBase;
                            else
                                address = (int) IntCodeCopy[instructionPointer + 3];

                            switch (opCode)
                            {
                                case 1:
                                    IntCodeCopy[address] = firstPValue + secondPValue;
                                    instructionPointer += 4;
                                    break;

                                case 2:
                                    IntCodeCopy[address] = firstPValue * secondPValue;
                                    instructionPointer += 4;
                                    break;

                                case 5:

                                    instructionPointer =
                                        firstPValue != 0 ? (int) secondPValue : instructionPointer += 3;
                                    break;
                                case 6:
                                    instructionPointer =
                                        firstPValue == 0 ? (int) secondPValue : instructionPointer += 3;
                                    break;
                                case 7:
                                    IntCodeCopy[address] =
                                        firstPValue < secondPValue ? 1 : 0;
                                    instructionPointer += 4;
                                    break;
                                case 8:
                                    IntCodeCopy[address] =
                                        firstPValue == secondPValue ? 1 : 0;
                                    instructionPointer += 4;
                                    break;

                                default:
                                    Console.WriteLine("Wrong opCode!!");
                                    break;
                            }

                            break;
                        }
                    }
                }
            }

            return output;
        }
    }
}