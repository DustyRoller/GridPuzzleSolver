using GridPuzzleSolver.Components;
using GridPuzzleSolver.Components.Cells;
using GridPuzzleSolver.Parser;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("KakuroSolverUnitTests")]

namespace GridPuzzleSolver.Puzzles.Kakuro.Parser
{
    /// <summary>
    /// Class to parse Kakuro puzzles from text files.
    /// </summary>
    /// <remarks>
    /// Puzzle files will be text format using the following format:
    ///   |  x  |17\  |24\  |  x  |  x  |
    ///   |  \16|  -  |  -  |20\  |  \  |
    ///   |  \23|  -  |  -  |  -  |15\  |
    ///   |  x  |  \23|  -  |  -  |  -  |
    ///   |  x  |  x  |  \14|  -  |  -  |.
    /// </remarks>
    internal class KakuroParser : BaseParser
    {
        /// <summary>
        /// Gets the file extension of the file that the parser will read.
        /// </summary>
        public static string FileExtension => ".kak";

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

            // Determine the height and width of the puzzle.
            puzzle.Height = (uint)lines.Length;

            // Width is minus two because of the starting and trailing '|'.
            var firstLineWidth = (uint)lines[0].Split('|').Length;
            if (firstLineWidth <= 3)
            {
                throw new ParserException("Puzzle must be at least two cells wide.");
            }

            puzzle.Width = firstLineWidth - 2;

            // Now make sure every other row has the same number of cells.
            for (var i = 1u; i < lines.Length; ++i)
            {
                var lineWidth = (uint)lines[i].Split('|').Length - 2;

                if (lineWidth != puzzle.Width)
                {
                    throw new ParserException($"Mismatch in row width on row {i + 1}.");
                }
            }

            ParseSections(puzzle);

            return puzzle;
        }

        /// <summary>
        /// Parse a column section out of the puzzle from the given ClueCell.
        /// </summary>
        /// <param name="puzzle">The Puzzle to parse the section out of.</param>
        /// <param name="clueCell">The ClueCell that the section originates from.</param>
        /// <param name="cellIndex">The index of where the ClueCell is in the puzzle.</param>
        private static void ParseColumnSection(Puzzle puzzle, ClueCell clueCell, int cellIndex)
        {
            var section = new KakuroSection(clueCell.ColumnClue);

            // Find all clue cells in the column until there is a break.
            for (var j = (int)(cellIndex + puzzle.Width); j < puzzle.Height * puzzle.Width; j += (int)puzzle.Width)
            {
                if (puzzle.Cells[j] is not PuzzleCell)
                {
                    break;
                }

                var puzzleCell = (PuzzleCell)puzzle.Cells[j];

                section.PuzzleCells.Add(puzzleCell);

                // Let the cell know it belongs to this section.
                puzzleCell.Sections.Add(section);
            }

            // Add the section to the puzzle.
            puzzle.Sections.Add(section);
        }

        /// <summary>
        /// Parse a row section out of the puzzle from the given ClueCell.
        /// </summary>
        /// <param name="puzzle">The Puzzle to parse the section out of.</param>
        /// <param name="clueCell">The ClueCell that the section originates from.</param>
        /// <param name="cellIndex">The index of where the ClueCell is in the puzzle.</param>
        private static void ParseRowSection(Puzzle puzzle, ClueCell clueCell, int cellIndex)
        {
            var section = new KakuroSection(clueCell.RowClue);

            // Find all clue cells in the row until there is a break.
            for (var j = cellIndex + 1; j < puzzle.Height * puzzle.Width; ++j)
            {
                if (puzzle.Cells[j] is not PuzzleCell)
                {
                    break;
                }

                var puzzleCell = (PuzzleCell)puzzle.Cells[j];

                section.PuzzleCells.Add(puzzleCell);

                // Let the cell know it belongs to this section.
                puzzleCell.Sections.Add(section);
            }

            // Add the section to the puzzle.
            puzzle.Sections.Add(section);
        }

        /// <summary>
        /// Parse out all of the sections from the given puzzle.
        /// </summary>
        /// <param name="puzzle">The Puzzle to parse the sections from.</param>
        private static void ParseSections(Puzzle puzzle)
        {
            // Need to generate segments that can be solved.
            for (var i = 0; i < puzzle.Cells.Count; ++i)
            {
                if (puzzle.Cells[i] is ClueCell clueCell)
                {
                    // Depending on which direction clue the cell has determines which way segment will be created.
                    if (clueCell.ColumnClue != 0u)
                    {
                        ParseColumnSection(puzzle, clueCell, i);
                    }

                    if (clueCell.RowClue != 0u)
                    {
                        ParseRowSection(puzzle, clueCell, i);
                    }
                }
            }
        }
    }
}
