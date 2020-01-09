using System;
using System.Collections.Generic;
using System.Linq;

namespace day7
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            string commmands = System.IO.File.ReadAllText(@"/home/osse/Downloads/input.txt");
            var commandsList = commmands.Split(',');

            IList<int> IntCode = new List<int>();
            foreach (var command in commandsList)
                IntCode.Add(int.Parse(command));

        
            int highestSignalPart1 = Part1(IntCode);
            int highestSignalPart2 = Part2(IntCode);

            Console.WriteLine("highestSignalPart1 : " + highestSignalPart1);
            Console.WriteLine("highestSignalPart2 : " + highestSignalPart2);
        }

        //part1 func
        private static int Part1(IList<int> IntCode)
        {
            IList<int> IntCodeCopy = new List<int>(IntCode);

            int highestSignal = 0;
            foreach (var p in Permutate("01234"))
            {
                int inputSetting = int.Parse(p);
                IntCodeComputer amp1 = new IntCodeComputer(IntCodeCopy, inputSetting / 10000 % 10);
                IntCodeComputer amp2 = new IntCodeComputer(IntCodeCopy, inputSetting / 1000 % 10);
                IntCodeComputer amp3 = new IntCodeComputer(IntCodeCopy, inputSetting / 100 % 10);
                IntCodeComputer amp4 = new IntCodeComputer(IntCodeCopy, inputSetting / 10 % 10);
                IntCodeComputer amp5 = new IntCodeComputer(IntCodeCopy, inputSetting / 1 % 10);

                amp1.RunPart1(0);
                amp2.RunPart1(amp1.GetOutputQueue());
                amp3.RunPart1(amp2.GetOutputQueue());
                amp4.RunPart1(amp3.GetOutputQueue());
                amp5.RunPart1(amp4.GetOutputQueue());

                int result = amp5.GetOutputQueue();
                highestSignal = highestSignal < result ? result : highestSignal;
            }

            return highestSignal;
        }

        //part2 func
        private static int Part2(IList<int> IntCode)
        {
            IList<int> IntCodeCopy = new List<int>(IntCode);
            int highestSignal = 0;
            foreach (var p in Permutate("56789"))
            {
                int inputSetting = int.Parse(p);
                int result = RunIntCodeComputer(IntCodeCopy, inputSetting);
                highestSignal = highestSignal < result ? result : highestSignal;
            }

            return highestSignal;
        }

        public static int RunIntCodeComputer(IList<int> IntCode, int inputSetting)
        {
            IntCodeComputer amp1 = new IntCodeComputer(IntCode, inputSetting / 10000 % 10);
            IntCodeComputer amp2 = new IntCodeComputer(IntCode, inputSetting / 1000 % 10);
            IntCodeComputer amp3 = new IntCodeComputer(IntCode, inputSetting / 100 % 10);
            IntCodeComputer amp4 = new IntCodeComputer(IntCode, inputSetting / 10 % 10);
            IntCodeComputer amp5 = new IntCodeComputer(IntCode, inputSetting / 1 % 10);

            amp1.RunPart2(0);
            amp2.RunPart2(amp1.GetOutputQueue());
            amp3.RunPart2(amp2.GetOutputQueue());
            amp4.RunPart2(amp3.GetOutputQueue());
            amp5.RunPart2(amp4.GetOutputQueue());
            while (!amp5.IsHalted)
            {
                amp1.RunPart2(amp5.GetOutputQueue());
                amp2.RunPart2(amp1.GetOutputQueue());
                amp3.RunPart2(amp2.GetOutputQueue());
                amp4.RunPart2(amp3.GetOutputQueue());
                amp5.RunPart2(amp4.GetOutputQueue());
            }

            return amp5.GetOutputQueue();
        }


        //shared
        class IntCodeComputer
        {
            private int _instructionPointer;
            private int phaseSetting;
            public bool IsHalted { set; get; }
            private bool UsePhaseSetting { set; get; }
            private List<int> IntCode = new List<int>();
            private readonly Queue<int> _inputQueue = new Queue<int>();
            private readonly Queue<int> _outputQueue = new Queue<int>();

            public void RunPart1(int input)
            {
                int outPut = input;
                while (!IsHalted)
                {
                    ////ABCDE
                    var intCommand = IntCode[_instructionPointer];

                    var opCode = intCommand / 1 % 100;
                    var parameter1 = intCommand / 100 % 10;
                    var parameter2 = intCommand / 1000 % 10;
                    //the third parameter is always 0


                    var firstPValue = 0;
                    var secondPValue = 0;

                    switch (opCode)
                    {
                        //input
                        case 3:
                            if (UsePhaseSetting)
                            {
                                IntCode[IntCode[_instructionPointer + 1]] = phaseSetting;
                                UsePhaseSetting = false;
                            }
                            else
                                IntCode[IntCode[_instructionPointer + 1]] = outPut;

                            _instructionPointer += 2;

                            break;
                        //output
                        case 4:
                            firstPValue = parameter1 == 0
                                ? IntCode[IntCode[_instructionPointer + 1]]
                                : IntCode[_instructionPointer + 1];

                            outPut = firstPValue;
                            _instructionPointer += 2;
                            break;
                        case 99:
                            IsHalted = true;
                            break;

                        default:

                            firstPValue = parameter1 == 0
                                ? IntCode[IntCode[_instructionPointer + 1]]
                                : IntCode[_instructionPointer + 1];
                            secondPValue = parameter2 == 0
                                ? IntCode[IntCode[_instructionPointer + 2]]
                                : IntCode[_instructionPointer + 2];
                            switch (opCode)
                            {
                                //addition
                                case 1:
                                    IntCode[IntCode[_instructionPointer + 3]] = firstPValue + secondPValue;
                                    _instructionPointer += 4;
                                    break;
                                //multiplication
                                case 2:
                                    IntCode[IntCode[_instructionPointer + 3]] = firstPValue * secondPValue;
                                    _instructionPointer += 4;
                                    break;


                                //jump if true
                                case 5:
                                    _instructionPointer = firstPValue != 0 ? secondPValue : _instructionPointer += 3;
                                    break;
                                //jump if false
                                case 6:
                                    _instructionPointer = firstPValue == 0 ? secondPValue : _instructionPointer += 3;
                                    break;
                                //jump if firstPValue less than secondPValue
                                case 7:

                                    IntCode[IntCode[_instructionPointer + 3]] = firstPValue < secondPValue ? 1 : 0;

                                    _instructionPointer += 4;
                                    break;

                                //jump if firstPValue equal to secondPValue
                                case 8:
                                    IntCode[IntCode[_instructionPointer + 3]] = firstPValue == secondPValue ? 1 : 0;
                                    _instructionPointer += 4;
                                    break;
                            }

                            break;
                    }
                }

                _outputQueue.Enqueue(outPut);
            }

            public int GetOutputQueue()
            {
                return this._outputQueue.Dequeue();
            }

            public IntCodeComputer(IList<int> intCode, int phaseSetting)
            {
                _instructionPointer = 0;
                this.phaseSetting = phaseSetting;
                IsHalted = false;
                UsePhaseSetting = true;
                foreach (var item in intCode)
                {
                    IntCode.Add(item);
                }
            }


            public void RunPart2(int input)
            {
                _inputQueue.Enqueue(input);


                while (!IsHalted)
                {
                    ////ABCDE
                    var intCommand = IntCode[_instructionPointer];

                    var opCode = intCommand / 1 % 100;
                    var parameter1 = intCommand / 100 % 10;
                    var parameter2 = intCommand / 1000 % 10;
                    //the third parameter is always 0


                    var firstPValue = 0;
                    var secondPValue = 0;

                    switch (opCode)
                    {
                        //input
                        case 3:
                            if (UsePhaseSetting)
                            {
                                IntCode[IntCode[_instructionPointer + 1]] = phaseSetting;
                                _instructionPointer += 2;
                                UsePhaseSetting = false;
                            }
                            else
                            {
                                if (_inputQueue.Count == 0)
                                    return;

                                IntCode[IntCode[_instructionPointer + 1]] = _inputQueue.Dequeue();
                                _instructionPointer += 2;
                            }

                            break;
                        //output
                        case 4:
                            firstPValue = parameter1 == 0
                                ? IntCode[IntCode[_instructionPointer + 1]]
                                : IntCode[_instructionPointer + 1];

                            _outputQueue.Enqueue(firstPValue);
                            _instructionPointer += 2;
                            break;
                        case 99:
                            IsHalted = true;
                            break;
                        default:
                            firstPValue = parameter1 == 0
                                ? IntCode[IntCode[_instructionPointer + 1]]
                                : IntCode[_instructionPointer + 1];
                            secondPValue = parameter2 == 0
                                ? IntCode[IntCode[_instructionPointer + 2]]
                                : IntCode[_instructionPointer + 2];
                            switch (opCode)
                            {
                                //addition
                                case 1:
                                    IntCode[IntCode[_instructionPointer + 3]] = firstPValue + secondPValue;
                                    _instructionPointer += 4;
                                    break;
                                //multiplication
                                case 2:
                                    IntCode[IntCode[_instructionPointer + 3]] = firstPValue * secondPValue;
                                    _instructionPointer += 4;
                                    break;
                                //jump if true
                                case 5:
                                    _instructionPointer = firstPValue != 0 ? secondPValue : _instructionPointer += 3;
                                    break;
                                //jump if false
                                case 6:
                                    _instructionPointer = firstPValue == 0 ? secondPValue : _instructionPointer += 3;
                                    break;
                                //jump if firstPValue less than secondPValue
                                case 7:
                                    IntCode[IntCode[_instructionPointer + 3]] = firstPValue < secondPValue ? 1 : 0;
                                    _instructionPointer += 4;
                                    break;
                                //jump if firstPValue equal to secondPValue
                                case 8:
                                    IntCode[IntCode[_instructionPointer + 3]] = firstPValue == secondPValue ? 1 : 0;
                                    _instructionPointer += 4;
                                    break;
                            }

                            break;
                    }
                }
            }
        }

        private static IEnumerable<string> Permutate(string source)
        {
            if (source.Length == 1) return new List<string> {source};

            var permutations = from c in source
                from p in Permutate(new String(source.Where(x => x != c).ToArray()))
                select c + p;
            return permutations;
        }
    }
}