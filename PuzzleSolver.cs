
using System;
using System.IO;

namespace Advent_of_Code_2020
{
    public abstract class PuzzleSolver
    {
        protected readonly string inputString;
        protected string[] input;
        protected string Name { get; set; }

        public PuzzleSolver(string inputString)
        {
            this.inputString = inputString;
            this.input = File.ReadAllLines(inputString);
        }

        public virtual void SolvePuzzles()
        {
            Console.WriteLine($"Puzzle solved for {Name}");
        }
    }
}
