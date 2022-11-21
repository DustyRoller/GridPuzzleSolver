using System.Diagnostics;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("GridPuzzleSolverUnitTests")]

namespace GridPuzzleSolver
{
    /// <summary>
    /// The class containing the entry point for the application.
    /// </summary>
    internal static class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.Error.WriteLine("Expected 1 argument - puzzle file name");
                Environment.Exit(1);
            }

            var puzzle = Parser.Parser.ParsePuzzle(args[0]);

            // Time how long it takes to solve the puzzle.
            var stopwatch = Stopwatch.StartNew();

            if (puzzle.Solve())
            {
                Console.WriteLine("Successfully solved puzzle");
            }
            else
            {
                Console.WriteLine("Failed to solve puzzle\n");
                Console.WriteLine($"{puzzle.NumberOfUnsolvedCells} cells remain unsolved");
            }

            // Stop the stopwatch.
            stopwatch.Stop();

            // Print out the puzzle.
            Console.WriteLine();
            Console.WriteLine(puzzle.ToString());

            // Print out how long it took to solve the puzzle.
            Console.WriteLine();
            var timeTaken = (stopwatch.ElapsedMilliseconds / 1000.0).ToString("F2");
            Console.WriteLine($"Time taken: {timeTaken}s");
        }
    }
}