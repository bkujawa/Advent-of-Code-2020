
using System;
using System.IO;
using System.Diagnostics;

namespace Advent_of_Code_2020
{
    public abstract class PuzzleSolver
    {
        private Stopwatch stopwatch;
        protected string[] input;
        protected string Name { get; set; }

        public PuzzleSolver(string inputString)
        {
            this.input = File.ReadAllLines(inputString);
            this.stopwatch = new Stopwatch();
        }

        public void SolvePuzzles()
        {
            this.stopwatch.Start();
            SolvePuzzleOne();
            this.stopwatch.Stop();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Puzzle one: {this.stopwatch.Elapsed.TotalSeconds}s");
            Console.ResetColor();
            this.stopwatch.Reset();

            this.stopwatch.Start();
            SolvePuzzleTwo();
            this.stopwatch.Stop();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Puzzle two: {this.stopwatch.Elapsed.TotalSeconds}s");
            this.stopwatch.Reset();
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"Puzzle solved for {Name}");
            Console.ResetColor();
        }

        protected abstract void SolvePuzzleOne();

        protected abstract void SolvePuzzleTwo();
    }
}
