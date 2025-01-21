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

            var puzzle = new SudokuPuzzle();

            // Now read in the puzzle.
            var lines = File.ReadAllLines(puzzleFilePath);

            ValidatePuzzleSize(lines);

            for (var row = 0u; row < lines.Length; ++row)
            {
                // Cells within the line will be delimited by '|'.
                var cellsStr = lines[row].Split('|', StringSplitOptions.RemoveEmptyEntries);

                for (var column = 0u; column < cellsStr.Length; ++column)
                {
                    var coordinate = new Coordinate
                    {
                        X = column,
                        Y = row,
                    };
                    var cell = ParseCell(coordinate, cellsStr[column]);

                    puzzle.Cells.Add(cell);
                }
            }

            puzzle.CreateSections();

            // Ensure that there is at least one solved cell.
            if (puzzle.NumberOfUnsolvedCells == 81)
            {
                throw new ParserException("Puzzle contains no solved cells");
            }

            return puzzle;
        }

        /// <summary>
        /// Parse the given cell string to generate a Cell object.
        /// </summary>
        /// <param name="coordinate">The coordinate to be associated with the cell.</param>
        /// <param name="cellStr">The cell string to be parsed.</param>
        /// <returns>A Cell object.</returns>
        private static PuzzleCell ParseCell(Coordinate coordinate, string cellStr)
        {
            var puzzleCell = new PuzzleCell
            {
                Coordinate = coordinate,
            };

            // Square will be either empty or already have a value in it.
            if (cellStr != "-")
            {
                if (!uint.TryParse(cellStr, out uint cellValue))
                {
                    throw new ParserException($"Failed to parse cell value: {cellStr}");
                }

                puzzleCell.CellValue = cellValue;
            }

            return puzzleCell;
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
