using GridPuzzleSolver.Cells;
using GridPuzzleSolver.Parser;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("KakuroSolverUnitTests")]

namespace GridPuzzleSolver.KakuroSolver.Parser
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
    internal class KakuroParser : IParser
    {
        /// <summary>
        /// The file extension used for kakuro puzzles.
        /// </summary>
        private const string KakuroPuzzleFileExtension = ".kak";

        /// <summary>
        /// Parse the given file to generate a Puzzle, ready to be solved.
        /// </summary>
        /// <param name="puzzleFilePath">The path to the file containing the puzzle.</param>
        /// <returns>A Puzzle object.</returns>
        public Puzzle ParsePuzzle(string puzzleFilePath)
        {
            ValidateInputFile(puzzleFilePath);

            var puzzle = new Puzzle();

            // Now read in the puzzle.
            var lines = File.ReadAllLines(puzzleFilePath);

            // Determine the height and width of the puzzle.
            puzzle.Height = (uint)lines.Length;

            // Width is minus two because of the starting and trailing '|'.
            puzzle.Width = (uint)lines[0].Split('|').Length - 2;

            // Now make sure every other row has the same number of cells.
            for (var i = 1u; i < lines.Length; ++i)
            {
                var lineWidth = (uint)lines[i].Split('|').Length - 2;

                if (lineWidth != puzzle.Width)
                {
                    throw new ParserException($"Mismatch in row width on row {i + 1}.");
                }
            }

            for (var row = 0u; row < lines.Length; ++row)
            {
                // Cells within the line will be delimited by '|'.
                var cellsStr = lines[row].Split('|', StringSplitOptions.RemoveEmptyEntries);

                for (var column = 0u; column < cellsStr.Length; ++column)
                {
                    var cell = ParseCell(cellsStr[column]);
                    cell.Coordinate = new Coordinate(column, row);

                    puzzle.AddCell(cell);
                }
            }

            ParseSections(puzzle);

            return puzzle;
        }

        /// <summary>
        /// Parse the given cell string to generate a Cell object.
        /// </summary>
        /// <param name="cellStr">The cell string to be parsed.</param>
        /// <returns>A Cell object.</returns>
        private static Cell ParseCell(string cellStr)
        {
            Cell cell;

            // Square will either contain:
            //  'x' - for blank squares, which are ignored for now
            //  '-' - for puzzle squares that need to be solved
            //  'n \ n' - for clue squares.
            if (cellStr == "  -  ")
            {
                cell = new PuzzleCell();
            }
            else if (cellStr.Contains('\\'))
            {
                cell = ParseClueCell(cellStr);
            }
            else if (cellStr == "  x  ")
            {
                cell = new BlankCell();
            }
            else
            {
                throw new ParserException($"Found invalid cell data: {cellStr}.");
            }

            return cell;
        }

        /// <summary>
        /// Parse the given string to generate a ClueCell object.
        /// </summary>
        /// <param name="clueCellStr">The clue cell string to parse.</param>
        /// <returns>A ClueCell object.</returns>
        private static ClueCell ParseClueCell(string clueCellStr)
        {
            // Need to get out the column and row clue.
            var columnClue = 0u;
            var rowClue = 0u;
            var clues = clueCellStr.Split('\\');

            if (!string.IsNullOrWhiteSpace(clues[0]))
            {
                columnClue = uint.Parse(clues[0].Trim());
            }

            if (!string.IsNullOrWhiteSpace(clues[1]))
            {
                rowClue = uint.Parse(clues[1].Trim());
            }

            return new ClueCell
            {
                ColumnClue = columnClue,
                RowClue = rowClue,
            };
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
            for (var j = (int)(cellIndex + puzzle.Width); j < (puzzle.Height * puzzle.Width); j += (int)puzzle.Width)
            {
                if (puzzle.Cells[j] is not PuzzleCell)
                {
                    break;
                }

                var puzzleCell = (PuzzleCell)puzzle.Cells[j];

                section.PuzzleCells.Add(puzzleCell);

                // Let the cell know it belongs to this section.
                puzzleCell.ColumnSection = section;
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
                puzzleCell.RowSection = section;
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

        /// <summary>
        /// Validate that the input file contains a valid puzzle.
        /// </summary>
        /// <param name="puzzleFilePath">The path to the file containing the puzzle.</param>
        private static void ValidateInputFile(string puzzleFilePath)
        {
            if (!File.Exists(puzzleFilePath))
            {
                throw new FileNotFoundException("Unable to find puzzle file.", puzzleFilePath);
            }

            if (Path.GetExtension(puzzleFilePath) != KakuroPuzzleFileExtension)
            {
                throw new ArgumentException($"Invalid file type, expected {KakuroPuzzleFileExtension}.", nameof(puzzleFilePath));
            }

            // Make sure the file actually contains some data.
            if (new FileInfo(puzzleFilePath).Length == 0)
            {
                throw new ArgumentException("Puzzle file is empty.", nameof(puzzleFilePath));
            }
        }
    }
}
