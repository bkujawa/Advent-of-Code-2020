namespace Advent_of_Code_2020
{
    class Program
    {
        static void Main(string[] args)
        {
            PuzzleSolver puzzleSolver;

            puzzleSolver = new Day1(InputStrings.InputStringDay1);
            puzzleSolver.SolvePuzzles();

            puzzleSolver = new Day2(InputStrings.InputStringDay2);
            puzzleSolver.SolvePuzzles();

            puzzleSolver = new Day3(InputStrings.InputStringDay3);
            puzzleSolver.SolvePuzzles();

            puzzleSolver = new Day4(InputStrings.InputStringDay4);
            puzzleSolver.SolvePuzzles();
        }
    }
}
