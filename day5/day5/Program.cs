using System;
using System.Collections.Generic;

namespace day5
{
    class Program
    {
        private static void Main(string[] args)
        {
            var commmands = System.IO.File.ReadAllText(@"/home/osse/Downloads/input.txt");
            var commandsList = commmands.Split(',');

            IList<int> IntCode = new List<int>();
            foreach (var command in commandsList)
                IntCode.Add(int.Parse(command));


            var part1Result = Computer(IntCode, 1);
            var part2Result = Computer(IntCode, 5);

            Console.WriteLine("part1 output : " + part1Result);
            Console.WriteLine("part2 output : " + part2Result);
            Console.WriteLine("Done.");
            Console.ReadKey();
        }


        private static int Computer(IList<int> IntCode, int input)
        {
            IList<int> IntCodeCopy = new List<int>(IntCode);
            var output = 0;
            var instructionPointer = 0;
            var isRunning = true;
            while (isRunning)
            {
                ////ABCDE
                var intCommand = IntCodeCopy[instructionPointer];

                var opCode = intCommand / 1 % 100;
                var parameter1 = intCommand / 100 % 10;
                var parameter2 = intCommand / 1000 % 10;
                //the third parameter is always 0


                var firstPValue = 0;
                var secondPValue = 0;

                switch (opCode)
                {
                    case 3:
                    {
                        var address = IntCodeCopy[instructionPointer + 1];
                        IntCodeCopy[address] = input;
                        instructionPointer += 2;
                        break;
                    }
                    case 4:
                        firstPValue = parameter1 == 0
                            ? IntCodeCopy[IntCodeCopy[instructionPointer + 1]]
                            : IntCodeCopy[instructionPointer + 1];
                        output = firstPValue;
                        instructionPointer += 2;
                        break;
                    case 99:
                        isRunning = false;
                        break;
                    default:
                    {
                        firstPValue = parameter1 == 0
                            ? IntCodeCopy[IntCodeCopy[instructionPointer + 1]]
                            : IntCodeCopy[instructionPointer + 1];
                        secondPValue = parameter2 == 0
                            ? IntCodeCopy[IntCodeCopy[instructionPointer + 2]]
                            : IntCodeCopy[instructionPointer + 2];
                        switch (opCode)
                        {
                            case 1:
                                IntCodeCopy[IntCodeCopy[instructionPointer + 3]] = firstPValue + secondPValue;
                                instructionPointer += 4;
                                break;
                            
                            case 2:
                                IntCodeCopy[IntCodeCopy[instructionPointer + 3]] = firstPValue * secondPValue;
                                instructionPointer += 4;
                                break;
                            
                            case 5:

                                instructionPointer = firstPValue != 0 ? secondPValue :  instructionPointer+= 3;
                                break;
                            case 6:
                                instructionPointer = firstPValue == 0 ? secondPValue :  instructionPointer+= 3;
                                break;
                            case 7:
                                IntCodeCopy[IntCodeCopy[instructionPointer + 3]] = firstPValue < secondPValue ? 1 : 0;
                                instructionPointer += 4;
                                break;
                            case 8:
                                IntCodeCopy[IntCodeCopy[instructionPointer + 3]] = firstPValue == secondPValue ? 1 : 0;
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

            return output;
        }
    }
}