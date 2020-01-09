////////////////////////////////////////
///representing every input line as a moon which orbiting around an earth
///earth)moon
///  COM)B
///    B)C
////////////////////////////////////////


using System;
using System.Collections.Generic;

namespace day6
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            ////reading from the input file 
            char[] charSeparators = new char[] { };
            string inputStr = System.IO.File.ReadAllText(@"/home/osse/Downloads/input.txt");
            string[] inputList = inputStr.Split(charSeparators, StringSplitOptions.RemoveEmptyEntries);
            ////


            //saving all input lines in a List
            IList<EarthsAndMoons> earthsAndMoons = new List<EarthsAndMoons>();
            foreach (var input in inputList)
            {
                string earth = input.Substring(0, 3);
                string moon = input.Substring(4);
                earthsAndMoons.Add(new EarthsAndMoons(earth, moon));
            }

            //a list to save all nodes "as earth or moon" 
            IList<Orbits> orbitsCounter = new List<Orbits>();
            //adding all earths
            foreach (var earth in earthsAndMoons)
            {
                bool found = false;
                foreach (var orbit in orbitsCounter)
                    if (earth.Earth == orbit.OrbitName)
                    {
                        found = true;
                        break;
                    }
                if (!found)
                    orbitsCounter.Add(new Orbits(earth.Earth, -1, -1)); // -1 means not calculated yet
            }

            //adding all moons
            foreach (var moon in earthsAndMoons)
            {
                bool found = false;
                foreach (var orbit in orbitsCounter)
                    if (moon.Moon == orbit.OrbitName)
                    {
                        found = true;
                        break;
                    }
                if (!found)
                    orbitsCounter.Add(new Orbits(moon.Moon, -1, -1)); // -1 means not calculated yet
            }


            Part1(orbitsCounter, earthsAndMoons);
            Part2(earthsAndMoons);
            Console.ReadKey();
        }

        //part1 func
        private static void Part1(IList<Orbits> orbitsCounter, IList<EarthsAndMoons> earthsAndMoons)
        {
            IList<Orbits> orbitsCounterCopy = new List<Orbits>(orbitsCounter);
            IList<EarthsAndMoons> earthsAndMoonsCopy = new List<EarthsAndMoons>(earthsAndMoons);
            FindOrbitLinks(ref orbitsCounterCopy, earthsAndMoonsCopy);

            var allDirectlyAndIndirectlyOrbits = 0;

            foreach (var item in orbitsCounterCopy)
            {
                // Console.WriteLine(item.OrbitName + ", Indi: " + item.IndirectlyOrbit + ", dire: " + item.DirectlyOrbit);
                allDirectlyAndIndirectlyOrbits += item.IndirectlyOrbit + item.DirectlyOrbit;
            }

            Console.WriteLine("The total number of direct and indirect orbits : " + allDirectlyAndIndirectlyOrbits);
        }

        /// <summary>
        /// iteration of the input strings and calling "FindOrbitInfo" func with right arguments
        /// </summary>
        /// <param name="orbitsCounter"></param>
        /// <param name="earthsAndMoons"></param>
        private static void FindOrbitLinks(ref IList<Orbits> orbitsCounter, IList<EarthsAndMoons> earthsAndMoons)
        {
            //iterating on input lines
            foreach (var earthAndMoon in earthsAndMoons)
            {
                bool earthOrbitFound = false;
                bool moonOrbitFound = false;
                int earthOrbitFoundAt = 0;
                int moonOrbitFoundAt = 0;
                //finidig the earth's index in "orbitsCounter" list
                for (int i = 0; i < orbitsCounter.Count; i++)
                {
                    if (earthAndMoon.Earth == orbitsCounter[i].OrbitName)
                    {
                        earthOrbitFound = true;
                        earthOrbitFoundAt = i;
                        break;
                    }
                }

                //finidig the moon's index in "orbitsCounter" list
                for (int i = 0; i < orbitsCounter.Count; i++)
                {
                    if (earthAndMoon.Moon == orbitsCounter[i].OrbitName)
                    {
                        moonOrbitFound = true;
                        moonOrbitFoundAt = i;
                        break;
                    }
                }

                //calling "FindOrbitInfo" with the moon and it's earth
                if (earthOrbitFound && moonOrbitFound)
                {
                    FindOrbitInfo(ref orbitsCounter, earthsAndMoons, earthOrbitFoundAt, moonOrbitFoundAt);
                }
                else
                {
                    Console.WriteLine("an orbit is missing!!");
                    Console.WriteLine("if you are reading this, something is really wrong!!");
                }
            }
        }


        private static void FindOrbitInfo(ref IList<Orbits> orbitsCounter, IList<EarthsAndMoons> earthsAndMoons,
            int earthOrbitFoundAt, int moonOrbitFoundAt)
        {
            if (orbitsCounter[earthOrbitFoundAt].DirectlyOrbit == -1)
            {
                //just in case of "COM"
                if (orbitsCounter[earthOrbitFoundAt].OrbitName == "COM")
                {
                    orbitsCounter[earthOrbitFoundAt].IndirectlyOrbit = 0;
                    orbitsCounter[earthOrbitFoundAt].DirectlyOrbit = 0;

                    orbitsCounter[moonOrbitFoundAt].DirectlyOrbit = 1;
                    orbitsCounter[moonOrbitFoundAt].IndirectlyOrbit = 0;
                }
                else
                {
                    //if the EARTH have not been calculated yet, the func "FindOrbitInfo" will be recalld to calculate it
                    for (int k = 0; k < earthsAndMoons.Count; k++)
                    {
                        if (earthsAndMoons[k].Moon == orbitsCounter[earthOrbitFoundAt].OrbitName)
                        {
                            for (int i = 0; i < orbitsCounter.Count; i++)
                            {
                                if (earthsAndMoons[k].Earth == orbitsCounter[i].OrbitName)
                                {
                                    int newEarthOrbitFoundAt = i;
                                    int newMoonOrbitFoundAt = earthOrbitFoundAt;

                                    FindOrbitInfo(ref orbitsCounter, earthsAndMoons, newEarthOrbitFoundAt,
                                        newMoonOrbitFoundAt);

                                    FindOrbitInfo(ref orbitsCounter, earthsAndMoons, earthOrbitFoundAt,
                                        moonOrbitFoundAt);
                                    break;
                                }
                            }

                            break;
                        }
                    }
                }
            }
            //if the earth has been calculated, the moon taking the earth info and adding "1" to IndirectlyOrnit
            else
            {
                orbitsCounter[moonOrbitFoundAt].DirectlyOrbit = 1;
                orbitsCounter[moonOrbitFoundAt].IndirectlyOrbit =
                    orbitsCounter[earthOrbitFoundAt].IndirectlyOrbit + orbitsCounter[earthOrbitFoundAt].DirectlyOrbit;
            }
        }


        //part2 func
        private static void Part2(IList<EarthsAndMoons> earthsAndMoons)
        {
            IList<EarthsAndMoons> earthsAndMoonsCopy = new List<EarthsAndMoons>(earthsAndMoons);
            //find way from YOU to COM

            //find way from YOU to COM
            IList<string> youToCom = new List<string>();
            FindAllTheWay(earthsAndMoonsCopy, ref youToCom, "YOU");

            //find way from SAN to COM
            IList<string> sanToCom = new List<string>();
            _allWayFound = true;
            FindAllTheWay(earthsAndMoonsCopy, ref sanToCom, "SAN");

            bool pathFound = false;
            int shortestPath = 0;
            for (int i = 0; i < youToCom.Count; i++)
            {
                for (int j = 0; j < sanToCom.Count; j++)
                {
                    if (youToCom[i] == sanToCom[j])
                    {
                        shortestPath = i + j;
                        pathFound = true;
                        break;
                    }
                }

                if (pathFound)
                    break;
            }


            if (pathFound)
                Console.WriteLine("shortest path : " + shortestPath);
            else
                Console.WriteLine("Path between You and Santa not found!");
        }

        private static bool _allWayFound = true;

        private static void FindAllTheWay(IList<EarthsAndMoons> earthsAndMoons, ref IList<string> savePathHere,
            string nextMoon)
        {
            while (_allWayFound)
            {
                foreach (var t in earthsAndMoons)
                {
                    if (t.Moon != nextMoon) continue;
                    savePathHere.Add(t.Earth);
                    nextMoon = t.Earth;
                    break;
                }

                if (nextMoon == "COM")
                {
                    _allWayFound = false;
                }
                else
                {
                    FindAllTheWay(earthsAndMoons, ref savePathHere, nextMoon);
                }
            }
        }


        //shared
        public class Orbits
        {
            public string OrbitName { get; }
            public int IndirectlyOrbit { set; get; }
            public int DirectlyOrbit { set; get; }

            public Orbits(string orbitName, int indirectlyOrbit, int directlyOrbit)
            {
                this.OrbitName = orbitName;
                this.IndirectlyOrbit = indirectlyOrbit;
                this.DirectlyOrbit = directlyOrbit;
            }
        }

        public class EarthsAndMoons
        {
            public string Earth { get; }
            public string Moon { get; }

            public EarthsAndMoons(string earth, string moon)
            {
                this.Earth = earth;
                this.Moon = moon;
            }
        }
    }
}