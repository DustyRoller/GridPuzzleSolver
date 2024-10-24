﻿using GridPuzzleSolver.Parser;
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
        /// <summary>
        /// Runs the grid solver program. Parses the given puzzle file and attempts to solve it.
        /// </summary>
        /// <param name="puzzleFilePath">The path to the puzzle file containing the puzzle to be solved.</param>
        /// <exception cref="ArgumentException">Thrown if the puzzle file path is invalid.</exception>
        /// <exception cref="ParserException">Thrown if no suitable parser for the puzzle is found.</exception>
        public static void Run(string puzzleFilePath)
        {
            // Validate the input.
            if (string.IsNullOrEmpty(puzzleFilePath))
            {
                throw new ArgumentException("Puzzle file path is null or empty.", nameof(puzzleFilePath));
            }

            // Get the file extension from the file name.
            var puzzleFileExtension = Path.GetExtension(puzzleFilePath);

            if (string.IsNullOrEmpty(puzzleFileExtension))
            {
                throw new ArgumentException(
                    $"Failed to get file extension from puzzle file - {puzzleFilePath}.");
            }

            // Get the parser for the given puzzle type.
            var parser = ParserFactory.GetParser(puzzleFileExtension);

            var puzzle = parser.ParsePuzzle(puzzleFilePath);

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

        private static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.Error.WriteLine("Expected 1 argument - puzzle file name");
                Environment.Exit(1);
            }

            Run(args[0]);
        }
    }
}