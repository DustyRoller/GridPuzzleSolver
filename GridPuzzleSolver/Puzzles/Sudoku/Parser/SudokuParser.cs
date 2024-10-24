using GridPuzzleSolver.Components;
using GridPuzzleSolver.Components.Cells;
using GridPuzzleSolver.Parser;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("SudokuSolverUnitTests")]

namespace GridPuzzleSolver.Puzzles.Sudoku.Parser
{
    /// <summary>
    /// Class to parse Sudoku puzzles from text files.
    /// </summary>
    /// <remarks>
    /// Puzzle files will be text format using the following format:
    ///   |-|-|-|-|-|-|-|-|-|
    ///   |-|-|-|-|-|-|-|-|-|
    ///   |-|-|-|-|-|-|-|-|-|
    ///   |-|-|-|-|-|-|-|-|-|
    ///   |-|-|-|-|-|-|-|-|-|
    ///   |-|-|-|-|-|-|-|-|-|
    ///   |-|-|-|-|-|-|-|-|-|
    ///   |-|-|-|-|-|-|-|-|-|
    ///   |-|-|-|-|-|-|-|-|-|.
    /// </remarks>
    internal class SudokuParser : BaseParser
    {
        /// <summary>
        /// Gets the file extension of the file that the parser will read.
        /// </summary>
        public static string FileExtension => ".sud";

        /// <summary>
        /// Parse the given file to generate a Puzzle, ready to be solved.
        /// </summary>
        /// <param name="puzzleFilePath">The path to the file containing the puzzle.</param>
        /// <returns>A Puzzle object.</returns>
        public override Puzzle ParsePuzzle(string puzzleFilePath)
        {
            ValidateInputFile(puzzleFilePath, FileExtension);

            var puzzle = new Puzzle();

            // Now read in the puzzle.
            var lines = File.ReadAllLines(puzzleFilePath);

            ValidatePuzzleSize(lines);

            ParseSections(puzzle);

            return puzzle;
        }

        private static void ParseSections(Puzzle puzzle)
        {
            // Get all the column and row sections.
            for (int i = 0; i < 9; ++i)
            {
                // Get the column section cells.
                var columnCells = puzzle.Cells.Where(c => c.Coordinate.Y == i)
                                              .Select(c => (PuzzleCell)c)
                                              .ToList();

                var columnSection = new SudokuSection();
                columnSection.PuzzleCells.AddRange(columnCells);

                // Add the column section to the puzzle.
                puzzle.Sections.Add(columnSection);

                // Set the column section on each cell.
                columnCells.ForEach(cc => cc.Sections.Add(columnSection));

                // Get the row section cells.
                var rowCells = puzzle.Cells.Where(c => c.Coordinate.X == i)
                                           .Select(c => (PuzzleCell)c)
                                           .ToList();

                var rowSection = new SudokuSection();
                rowSection.PuzzleCells.AddRange(rowCells);

                // Add the row section to the puzzle.
                puzzle.Sections.Add(rowSection);

                // Set the row section on each cell.
                rowCells.ForEach(rc => rc.Sections.Add(rowSection));
            }

            // Get all of the 3x3 squares from within the puzzle,
            // this is a pretty ugly way of doing it but works for now.
            var squareStartingIndexes = new List<int>()
            {
                0, 3, 6, 27, 30, 33, 54, 57, 60,
            };

            foreach (var startingIndex in squareStartingIndexes)
            {
                var index = startingIndex;
                var squareCells = new List<PuzzleCell>();
                for (int x = 0; x < 3; ++x)
                {
                    for (int y = 0; y < 3; ++y)
                    {
                        squareCells.Add((PuzzleCell)puzzle.Cells[index]);
                        ++index;
                    }

                    index += 6;
                }

                var squareSection = new SudokuSection();
                squareSection.PuzzleCells.AddRange(squareCells);

                // Set the square section on each cell.
                squareCells.ForEach(sc => sc.Sections.Add(squareSection));

                puzzle.Sections.Add(squareSection);
            }
        }

        /// <summary>
        /// Validate that the given puzzle file is the right size.
        /// </summary>
        /// <param name="puzzle">The lines that make up the puzzle.</param>
        private static void ValidatePuzzleSize(string[] puzzle)
        {
            const int SectionSize = 9;

            // Make sure we have 9 rows.
            if (puzzle.Length != SectionSize)
            {
                throw new ParserException("Puzzle does not have 9 rows.");
            }

            // Make sure every row has 9 columns.
            foreach (var line in puzzle)
            {
                if (line.Length != (SectionSize * 2) + 1)
                {
                    throw new ParserException("Not all rows have 9 columns.");
                }
            }
        }
    }
}
